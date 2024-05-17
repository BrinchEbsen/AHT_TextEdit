using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHT_TextEdit.Common
{
    public class EXRelPtr
    {
        public EXRelPtr(int Value, int SelfAddress) { 
            this.Value = Value;
            this.SelfAddress = SelfAddress;
        }

        private int Value { get; set; }
        private int SelfAddress { get; set; }

        public int GetAbsoluteAddress()
        {
            return Value + SelfAddress;
        }
    }

    public enum GamePlatform
    {
        None,
        PS2,
        GameCube,
        Xbox
    }

    public enum Endian
    {
        None,
        Little,
        Big
    }
}
