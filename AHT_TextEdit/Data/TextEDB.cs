using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AHT_TextEdit.Common;

namespace AHT_TextEdit.Data
{
    public enum SectionLanguage
    {
        English = 0x10000,
        French  = 0x50000,
        German  = 0x70000,
        Italian = 0x90000,
        Spanish = 0xB0000,
        Dutch   = 0x150000
    }
    public enum SectionContents
    {
        Generic = 0x1,
        Realm1  = 0xA,
        Realm2  = 0xB,
        Realm3  = 0xC,
        Realm4  = 0xD,
        Panel   = 0x32,
        Credits = 0x3C
    }

    public enum Struct_GeoHeader
    {
        //General
        MagicValue = 0x0,
        HashCode = 0x4,
        EDB_Version = 0x8,
        Flags = 0xC,
        TimeStamp = 0x10,
        FileSize = 0x14,
        BaseFileSize = 0x18,
        PlatformVersions = 0x1c,
        DebugSectionStart = 0x34,
        DebugSectionEnd = 0x38,
        //GeoLists
        SectionList = 0x54,
        RefPointerList = 0x5C,
        EntityList = 0x64,
        AnimList = 0x6C,
        AnimSkinList = 0x74,
        ScriptList = 0x7C,
        MapList = 0x84,
        AnimModeList = 0x8C,
        AnimSetList = 0x94,
        ParticleList = 0x9C,
        SwooshList = 0xA4,
        SpreadSheetList = 0xAC,
        FontList = 0xB4,
        TextureList = 0xBC,
        TextureUpdateList = 0xC4
    }

    public class TextItem
    {
        public int TextIndex;
        public EXRelPtr StartAddress;
        public EXRelPtr EndAddress;
        public string Str;
        public EXHashCode TextHash;
        public EXHashCode SoundHash;
        public uint UserData1;
        public uint UserData2;
        public bool Edited = false;

        public override string ToString()
        {
            return Str;
        }
    }

    public struct Section
    {
        public int SectionIndex;
        public uint SectionHashCode;
        public int StartOffset;
        public int EndOffset;
    }

    public struct SectionList
    {
        public short Size;
        public int Address;
        public List<Section> Items;
    }

    public struct SpreadSheetSection
    {
        public int SectionIndex;
        public uint SectionHashCode;
        public int Size;
        public int Address;
        public List<TextItem> Items;
    }

    public struct SpreadSheetSectionList
    {
        public List<SpreadSheetSection> Items;
    }

    public class TextEDB
    {
        /// <summary>
        /// Data from the source .edb file that is positioned before the spreadsheet data.
        /// It is written back into the output file on export, and the pointers inside are updated.
        /// </summary>
        public uint[] DataBlob;
        public long DataBlobEndAddress;

        public GamePlatform Platform;
        public SectionList SectionList;
        public SpreadSheetSectionList SpreadSheetSectionList;
    }
}
