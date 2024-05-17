using AHT_TextEdit.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AHT_TextEdit.Data
{
    /*
     * Extends BinaryReader such that binary data can be read regardless of endianness
     */
    internal class EDBReader : BinaryReader
    {
        private Endian Endian;

        public EDBReader(FileStream stream, Encoding encoding, Endian Endian) : base(stream, encoding, false) {
            this.Endian = Endian;
        }

        public override ushort ReadUInt16()
        {
            if (Endian == Endian.Big)
            {
                return ByteSwapper.SwapBytes(base.ReadUInt16());
            }
            else
            {
                return base.ReadUInt16();
            }
        }

        public override short ReadInt16()
        {
            return (short)ReadUInt16();
        }

        public override uint ReadUInt32()
        {
            if (Endian == Endian.Big)
            {
                return ByteSwapper.SwapBytes(base.ReadUInt32());
            } else
            {
                return base.ReadUInt32();
            }
        }

        public override int ReadInt32()
        {
            return (int)ReadUInt32();
        }

        /// <summary>
        /// Reads a 4-byte relative pointer from the current stream, wraps it in a EXRelPtr object
        /// and advances the current position of the stream by 4 bytes.
        /// </summary>
        /// <returns>
        /// EXRelPtr object containing its own position and its value.
        /// </returns>
        public EXRelPtr ReadRelPtr()
        {
            int val = ReadInt32();

            return new EXRelPtr(val, (int)BaseStream.Position-4);
        }
    }
}
