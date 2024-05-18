using AHT_TextEdit.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHT_TextEdit.Data
{
    /// <summary>
    /// Contains methods for swapping the endianness of values.
    /// </summary>
    public class ByteSwapper
    {
        /*
         * Endian swap methods curtesy of bizzehdee
         * https://stackoverflow.com/questions/19560436/bitwise-endian-swap-for-various-types
         */

        private ByteSwapper() { }

        public static ushort SwapBytes(ushort x)
        {
            return (ushort)((ushort)((x & 0xff) << 8) | ((x >> 8) & 0xff));
        }

        public static uint SwapBytes(uint x)
        {
            return ((x & 0x000000ff) << 24) +
                   ((x & 0x0000ff00) << 8) +
                   ((x & 0x00ff0000) >> 8) +
                   ((x & 0xff000000) >> 24);
        }

        public static ulong SwapBytes(ulong value)
        {
            ulong uvalue = value;
            ulong swapped =
                  ((0x00000000000000FF) & (uvalue >> 56)
                 | (0x000000000000FF00) & (uvalue >> 40)
                 | (0x0000000000FF0000) & (uvalue >> 24)
                 | (0x00000000FF000000) & (uvalue >> 8)
                 | (0x000000FF00000000) & (uvalue << 8)
                 | (0x0000FF0000000000) & (uvalue << 24)
                 | (0x00FF000000000000) & (uvalue << 40)
                 | (0xFF00000000000000) & (uvalue << 56));
            return swapped;
        }
    }

    public class FileHandler
    {
        public static TextEDB ReadFile(string filename, GamePlatform Platform)
        {
            TextEDB textEDB = new TextEDB();

            using (var stream = File.Open(filename, FileMode.Open))
            {
                //Determine encoding
                Endian Endian = Platform == GamePlatform.GameCube ? Endian.Big : Endian.Little;
                Encoding Encoding = Endian == Endian.Big ? Encoding.BigEndianUnicode : Encoding.Unicode;

                using (var reader = new EDBReader(stream, Encoding, Endian))
                {
                    //First, check if the text spreadsheet exists.
                    stream.Seek((long)Struct_GeoHeader.SpreadSheetList, SeekOrigin.Begin);

                    short spreadSheetsSize = reader.ReadInt16();
                    stream.Seek(2, SeekOrigin.Current); //Skip past HashSize
                    int spreadSheetsAddress = reader.ReadRelPtr().GetAbsoluteAddress();

                    if (spreadSheetsSize == 0)
                    {
                        throw new IOException("No spreadsheets found in GeoFile.");
                    }

                    stream.Seek(spreadSheetsAddress, SeekOrigin.Begin);

                    uint spreadSheetHashCode = reader.ReadUInt32();
                    if (spreadSheetHashCode != (uint)EXHashCode.HT_SpreadSheet_Text)
                    {
                        throw new IOException("GeoFile does not contain Text spreadsheet - did you mean to select 'text.edb'?");
                    }

                    //Create list of sections
                    SectionList sectionList = new SectionList();

                    stream.Seek((long)Struct_GeoHeader.SectionList, SeekOrigin.Begin);

                    sectionList.Size = reader.ReadInt16();

                    stream.Seek(2, SeekOrigin.Current); //Skip past HashSize

                    sectionList.Address = reader.ReadRelPtr().GetAbsoluteAddress();

                    //Get the base address of the refpointer list, for writing pointers ón export later
                    stream.Seek((long)Struct_GeoHeader.RefPointerList + 4, SeekOrigin.Begin);
                    textEDB.RefPointerStartAddress = reader.ReadRelPtr().GetAbsoluteAddress();


                    //Read section list data
                    stream.Seek(sectionList.Address, SeekOrigin.Begin);
                    sectionList.Items = new List<Section>();
                    for (int i = 0; i < sectionList.Size; i++)
                    {
                        Section entry = new Section
                        {
                            SectionIndex = i,
                            SectionHashCode = reader.ReadUInt32(),
                            StartOffset = reader.ReadInt32(),
                            EndOffset = reader.ReadInt32()
                        };
                        reader.ReadUInt32(); //Skip past empty file pointer

                        sectionList.Items.Add(entry);
                    }


                    //Read text entries

                    SpreadSheetSectionList spreadSheetSectionList = new SpreadSheetSectionList();
                    spreadSheetSectionList.Items = new List<SpreadSheetSection>();
                    bool firstSection = true;

                    //Text sections start from the second section onward
                    for (int i = 1; i < sectionList.Size; i++)
                    {
                        SpreadSheetSection spreadSheetSection = new SpreadSheetSection();
                        spreadSheetSection.SectionIndex = i;
                        spreadSheetSection.Address = sectionList.Items[i].StartOffset;
                        spreadSheetSection.SectionHashCode = sectionList.Items[i].SectionHashCode;

                        stream.Seek(spreadSheetSection.Address, SeekOrigin.Begin);

                        //If it's the first section, this is where we want to fill our data blob up to.
                        if (firstSection)
                        {
                            firstSection = false;
                            textEDB.DataBlobEndAddress = stream.Position;
                            Console.WriteLine("Data blob ends at {0:X}", (int)textEDB.DataBlobEndAddress);
                        }

                        //Skip past some stuff
                        stream.Seek(0x4*5, SeekOrigin.Current);

                        spreadSheetSection.Size = reader.ReadInt32();
                        //We're now at the list's start

                        Console.WriteLine(string.Format("Parsing text items from section {0:X} ({1} items)",
                            spreadSheetSection.SectionHashCode, spreadSheetSection.Size));

                        //Read text items
                        spreadSheetSection.Items = new List<TextItem>();
                        long spreadSheetStart = stream.Position; //Keeping track of this for later
                        for (int j = 0; j < spreadSheetSection.Size; j++)
                        {
                            stream.Seek(spreadSheetStart + (16*j), SeekOrigin.Begin);

                            //Table information
                            TextItem item = new TextItem();
                            item.TextIndex = j;
                            item.TextHash = (EXHashCode)reader.ReadUInt32();
                            item.StartAddress = reader.ReadRelPtr();
                            item.EndAddress = reader.ReadRelPtr();
                            item.SoundHash = (EXHashCode)reader.ReadUInt32();

                            //Read text
                            stream.Seek(item.StartAddress.GetAbsoluteAddress(), SeekOrigin.Begin);

                            //Length of the text entry
                            int len = item.EndAddress.GetAbsoluteAddress() - item.StartAddress.GetAbsoluteAddress();
                            len = len / 2;

                            //C# strings don't understand null terminators :) :) :) :) :)
                            char[] chars = TrimCharArray(reader.ReadChars(len));
                            item.Str = new string(chars);
                            item.UserData1 = reader.ReadUInt32();
                            item.UserData2 = reader.ReadUInt32();

                            spreadSheetSection.Items.Add(item);
                        }

                        spreadSheetSectionList.Items.Add(spreadSheetSection);
                    }

                    //Fill up our data blob
                    //Make array of uints a quarter of the length of the blob's byte length
                    textEDB.DataBlob = new uint[textEDB.DataBlobEndAddress / 4];
                    //Fill it up with data
                    stream.Seek(0, SeekOrigin.Begin);
                    for (int i = 0; i < (textEDB.DataBlobEndAddress / 4); i++)
                    {
                        textEDB.DataBlob[i] = reader.ReadUInt32();
                    }

                    textEDB.Platform = Platform;
                    textEDB.SectionList = sectionList;
                    textEDB.SpreadSheetSectionList = spreadSheetSectionList;
                }
            }

            return textEDB;
        }

        public static GamePlatform CheckPlatform(string filename)
        {
            GamePlatform Platform;

            using (var stream = File.Open(filename, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream, Encoding.UTF8, false))
                {
                    //Check endian
                    Endian Endian;

                    char[] MagicValue = reader.ReadChars(4);
                    string s = new string(MagicValue);

                    if (s == "GEOM")
                    {
                        Console.WriteLine("File identified as big endian");
                        Endian = Endian.Big;
                    }
                    else if (s == "MOEG")
                    {
                        Console.WriteLine("File identified as little endian");
                        Endian = Endian.Little;
                    }
                    else
                    {
                        Console.WriteLine("Invalid file: Magic value read: " + s);
                        throw new IOException("Invalid GeoHeader.");
                    }

                    //Check EDB version
                    int EDBVersion;

                    if (Endian == Endian.Big)
                    {
                        stream.Seek(11, SeekOrigin.Begin);
                    } else
                    {
                        stream.Seek(8, SeekOrigin.Begin);
                    }

                    EDBVersion = reader.ReadByte();

                    if (EDBVersion != 0xF0) //240 is final
                    {
                        throw new IOException("Non-final GeoFile version! 240 expected, read "+EDBVersion);
                    }

                    //Check platform
                    stream.Seek((long)Struct_GeoHeader.PlatformVersions, SeekOrigin.Begin);

                    int PlatformValue = reader.ReadInt32();

                    if (PlatformValue == 0 && Endian == Endian.Big)
                    {
                        Platform = GamePlatform.GameCube;
                    } else if (PlatformValue == 1 && Endian == Endian.Little)
                    {
                        Platform = GamePlatform.Xbox;
                    } else if (PlatformValue == 0 && Endian == Endian.Little)
                    {
                        Platform = GamePlatform.PS2;
                    } else
                    {
                        Console.WriteLine("Could not determine platform of "+filename);
                        throw new IOException("Could not determine game version.");
                    }

                    Console.WriteLine("File identified as " + Platform.ToString());
                }
            }

            return Platform;
        }

        public static void WriteFile(string filename, GamePlatform Platform, TextEDB textEDB)
        {
            //Determine encoding
            Endian Endian = Platform == GamePlatform.GameCube ? Endian.Big : Endian.Little;
            Encoding Encoding = Endian == Endian.Big ? Encoding.BigEndianUnicode : Encoding.Unicode;

            using (var stream = File.Open(filename, FileMode.Create))
            {
                using (var writer = new EDBWriter(stream, Encoding, Endian))
                {
                    //Write data blob
                    foreach (uint n in textEDB.DataBlob)
                    {
                        writer.Write(n);
                    }

                    //Write text spreadsheet

                    //Save pointers for later
                    List<long> SheetSectionStartPtrs = new List<long>();
                    List<long> SheetSectionEndPtrs   = new List<long>();

                    //Write each section
                    foreach (SpreadSheetSection section in textEDB.SpreadSheetSectionList.Items)
                    {
                        SheetSectionStartPtrs.Add(writer.BaseStream.Position);

                        //Starting addresses of list and text data
                        int listStartAddress = (int)writer.BaseStream.Position;
                        int textStartAddress = (int)writer.BaseStream.Position + (section.Size*0x10 + 0x18);

                        //Write header
                        //We'll have to write the EndAddress pointer later, we don't know it yet
                        writer.Seek(4, SeekOrigin.Current); //Skip EndAddress
                        writer.Write(0x1200); //this is always 0x1200, no idea
                        writer.Seek(12, SeekOrigin.Current); //3 0-ints
                        writer.Write(section.Size);

                        //Current working addresses
                        int listCurr = (int)writer.BaseStream.Position;
                        int textCurr = textStartAddress;

                        //Write the things
                        foreach (TextItem item in section.Items)
                        {
                            writer.Write((uint)item.TextHash);
                            listCurr += 4;

                            //Skip ahead to write the text
                            writer.Seek(textCurr, SeekOrigin.Begin);

                            //Write string
                            char[] chars = item.Str.ToCharArray();
                            writer.Write(chars);

                            //Align to 4 bytes
                            int leftOver = (int)writer.BaseStream.Position % 4;
                            if (leftOver != 0)
                            {
                                writer.Seek(4 - leftOver, SeekOrigin.Current);
                            }
                            writer.Seek(4, SeekOrigin.Current);

                            int strEnd = (int)writer.BaseStream.Position;

                            //Go back and write the pointers
                            writer.Seek(listCurr, SeekOrigin.Begin);
                            writer.Write(textCurr - listCurr);
                            listCurr += 4;
                            writer.Write(strEnd - listCurr);
                            writer.Write(0xFFFFFFFF);
                            listCurr += 8;

                            writer.Seek(strEnd, SeekOrigin.Begin);
                            writer.Write(item.UserData1);
                            writer.Write(item.UserData2);
                            textCurr = (int)writer.BaseStream.Position;

                            //Seek backward to next list item before loop
                            //but not if we've reached the end of the loop
                            if (item.TextIndex != (section.Size - 1))
                            {
                                writer.Seek(listCurr, SeekOrigin.Begin);
                            }
                        }

                        //Add padding bytes to align to 0x20
                        {
                            int leftOver = (int)writer.BaseStream.Position % 0x20;
                            if (leftOver != 0)
                            {
                                writer.Seek(0x20 - leftOver, SeekOrigin.Current);
                            }

                            textCurr = (int)writer.BaseStream.Position;
                        }

                        //Save section end pointer
                        SheetSectionEndPtrs.Add(textCurr);

                        //Update EndAddress pointer at start of list
                        writer.Seek(listStartAddress, SeekOrigin.Begin);
                        writer.Write(textCurr);

                        //Seek to end address before next section
                        writer.Seek(textCurr, SeekOrigin.Begin);
                    }

                    //Store end of file for later
                    long eof = writer.BaseStream.Position;

                    //Overwrite pointers in data blob, so it matches our newly written data

                    //Overwrite filesize
                    writer.Seek((int)Struct_GeoHeader.FileSize, SeekOrigin.Begin);
                    writer.Write((int)eof);

                    //Overwrite debug section info (even if unused)
                    writer.Seek((int)Struct_GeoHeader.DebugSectionStart, SeekOrigin.Begin);
                    writer.Write((int)eof);
                    //Not sure why, but the debug section is always defined to be 0xB0 long, even though it'd be past EOF
                    writer.Write((int)eof + 0xB0);

                    //Overwrite section list pointers (skip ahead 16 bytes to the sections we care about)
                    writer.Seek(textEDB.SectionList.Address + 16, SeekOrigin.Begin);
                    for (int i = 0; i < SheetSectionStartPtrs.Count; i++)
                    {
                        writer.Seek(4, SeekOrigin.Current);
                        //Overwrite with our saved pointers from earlier
                        writer.Write((int)SheetSectionStartPtrs[i]);
                        writer.Write((int)SheetSectionEndPtrs[i]);
                        writer.Seek(4, SeekOrigin.Current);
                    }

                    //Overwrite refpointer list pointers
                    writer.Seek(textEDB.RefPointerStartAddress, SeekOrigin.Begin);
                    for (int i = 0; i < SheetSectionStartPtrs.Count; i++)
                    {
                        //Get section number at this list entry by accessing the data blob
                        int sectionNr = (int)textEDB.DataBlob[(writer.BaseStream.Position / 4) + 1];
                        sectionNr >>= 0x10; //Shift right 2 bytes because it was a short

                        writer.Seek(8, SeekOrigin.Current);
                        //Overwrite with new pointer. For some reason the pointer is 0x10 bytes further.
                        writer.Write((int)SheetSectionStartPtrs[sectionNr-1] + 0x10);
                        writer.Seek(4, SeekOrigin.Current);
                    }
                }
            }
        }

        public static void WriteTestFile()
        {
            //Big endian
            using (var stream = File.Open("C:\\Users\\Ebbers\\Documents\\test_be.bin", FileMode.Create))
            {
                using (var writer = new EDBWriter(stream, Encoding.BigEndianUnicode, Endian.Big))
                {
                    writer.Write(-34);
                    writer.Write((uint)0xF0800000);
                    writer.Write((short)-250);
                    writer.Write((ushort)0xF080);

                    char[] chars = new char[] { 'T', 'e', 's', 't', ' ', 'S', 't', 'r', 'i', 'n', 'g' };
                    writer.Write(chars);

                    writer.Seek(4, SeekOrigin.Begin);
                    writer.Write(0xE0A00000);

                    writer.Seek(0x100, SeekOrigin.Begin);
                    writer.Write(0xFFFFFFFF);
                }
            }

            //Little endian
            using (var stream = File.Open("C:\\Users\\Ebbers\\Documents\\test_le.bin", FileMode.Create))
            {
                using (var writer = new EDBWriter(stream, Encoding.Unicode, Endian.Little))
                {
                    writer.Write(-34);
                    writer.Write((uint)0xF0800000);
                    writer.Write((short)-250);
                    writer.Write((ushort)0xF080);

                    char[] chars = new char[] { 'T', 'e', 's', 't', ' ', 'S', 't', 'r', 'i', 'n', 'g' };
                    writer.Write(chars);

                    writer.Seek(4, SeekOrigin.Begin);
                    writer.Write(0xE0A00000);
                }
            }
        }

        /// <summary>
        /// Trim char array so null characters are cut off.
        /// </summary>
        /// <param name="array">Array to trim</param>
        /// <returns>Trimmed array</returns>
        private static char[] TrimCharArray(char[] array)
        {
            int len = 0;

            foreach (char c in array) {
                if (c == 0) {
                    break;
                }
                len++;
            }

            char[] newArray = new char[len];
            Array.Copy(array, newArray, len);

            return newArray;
        }
    }
}
