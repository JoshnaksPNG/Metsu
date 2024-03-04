using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewLangInterpreter.src.Compiler
{
    internal class TreeCompiler
    {
        public class CIL_Instruction
        {
            public string? body;
            public int size;
        }

        public class Int_Literal : CIL_Instruction
        {
            public int value;

            public Int_Literal(int value) 
            {
                this.size = 5;
                this.value = value;
                this.body = "ldc.i4";

                if (value >= 0 && value <= 8)
                {
                    this.body += "." + value;
                }
                else
                {
                    this.body += " " + value;
                }
            }
        }

        public class Char_Literal : CIL_Instruction
        {
            public char value;

            public Char_Literal(char value)
            {
                this.size = 5;
                this.value = value;
                this.body = "ldc.i4 " + (int) value;
            }
        }

        public class String_Literal : CIL_Instruction
        {
            public string value;

            public String_Literal(string value) 
            {
                this.value = value;
                this.body = "ldstr \"" + value + "\"";
                this.size = 5;
            }
        }

        public class Bool_Literal : CIL_Instruction
        {
            public bool value;

            public Bool_Literal(bool value) 
            {
                this.size = 1;
                this.value = value;
                if (value)
                {
                    this.body = "ldc.i4.1";
                } else
                {
                    this.body = "ldc.i4.0";
                }
            }
        }

        public class Float_Literal : CIL_Instruction
        {
            public float value;

            public Float_Literal(float value)
            {
                this.size = 5;
                this.value = value;
                this.body = "ldc.r4 " + value;
            }
        }
    }
}
