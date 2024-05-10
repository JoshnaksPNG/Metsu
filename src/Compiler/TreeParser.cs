using NewLangInterpreter.src.Frontend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewLangInterpreter.src.Compiler
{
    internal class TreeParser
    {
        public class CIL_Instruction
        {
            public string body = "";
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
                    this.body += "." + value + "\n";
                    this.size = 1;
                }
                else
                {
                    this.body += " " + value + "\n";
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
                this.body = "ldc.i4 " + (int) value + "\n";
            }
        }

        public class String_Literal : CIL_Instruction
        {
            public string value;

            public String_Literal(string value) 
            {
                this.value = value;
                this.body = "ldstr \"" + value + "\"\n";
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
                    this.body = "ldc.i4.1\n";
                } else
                {
                    this.body = "ldc.i4.0\n";
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
                this.body = "ldc.r4 " + value + "\n";
            }
        }

        public class Array_Literal : CIL_Instruction
        {
            public Array_Literal(List<AST.Expression> elements)
            {
                this.body = "newobj instance void class [mscorlib]System.Collections.Generic.List`1<int32>::ctor()\n";

                foreach (AST.Expression e in elements)
                {
                    this.body += "dup\n";
                    this.body += get_expression_body(e);
                    this.body += "callvirt instance void class [mscorlib]System.Collections.Generic.List`1<int32>::Add(!0)\n";
                    this.body += "nop\n";
                }
            }

            public static string get_expression_body(AST.Expression element)
            {
                CIL_Instruction instruction = CIL_Emitter.get_node_instruction(element);

                if (instruction != null)
                {
                    return instruction.body;
                }
                else
                {
                    return "nop\n";
                }
            }
        }

        public class Function_Declaration : CIL_Instruction
        {
            public string return_type;
            public string name;

            public Function_Declaration(AST.FunctionDeclaration declaration)
            {
                return_type = declaration.returnType.ToString();
                name = declaration.name;

                body = ".method public static " + return_type + " " + name + "\n{\n";

                foreach(AST.Statement stmt in declaration.body) 
                {
                    body += CIL_Emitter.get_node_instruction(stmt).body;
                }
            }

            public string declaration_parameters() 
            {
                throw new NotImplementedException();
            }

            public static string front_to_cil_type(string front) 
            {
                switch (front.ToLower())
                {
                    case "int":
                        return "int32";

                    case "float":
                        return "float32";

                    case "string":
                        return "string";

                    case "char":
                        return "char";

                    case "bool":
                        return "bool";

                    case "void":
                    default:
                        return "void";
                    
                }
            }
        }

        public class Program : CIL_Instruction
        {
            public Program(List<AST.Statement> programBody)
            {
                this.body += ".assembly ProgramName {}\r\n";
                this.body += ".assembly extern mscorlib {}\r\n";
                this.body += ".class public auto ansi beforefieldinit ProgramName extends [System.Runtime] System.Object\r\n" +
                    "{\r\n    .method private hidebysig static void Main() cil managed\r\n    " +
                    "{\r\n        .entrypoint\r\n        ";

                foreach (AST.Statement stmt in programBody)
                {
                    this.body += CIL_Emitter.get_node_instruction(stmt).body;
                }

                this.body += "        ret\r\n    }\r\n}";
            }
        }

        public class CallExpr : CIL_Instruction
        {
            public CallExpr(AST.CallExpr callExpr)
            {
                foreach (AST.Statement stmt in callExpr.args)
                {
                    this.body += CIL_Emitter.get_node_instruction(stmt).body;
                }

                this.body += "call " + "void " + "[mscorlib]System.Console::WriteLine(string)\r\n";
            }
        }
    }
}
