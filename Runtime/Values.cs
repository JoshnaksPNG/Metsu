using NewLangInterpreter.Frontend;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewLangInterpreter.Runtime
{
    internal static class Values
    {
        public enum ValueType
        {
            Null,
            Integer,
            Boolean,
            Float,
            Character,
            String,
            Object,
            NativeFn,
            Function,
        }

        public class RuntimeVal 
        {
            public ValueType type;
        }

        public class NullVal : RuntimeVal
        {
            public string value;

            public NullVal() 
            {
                type = ValueType.Null;
                value = "null";
            }

            public override string ToString()
            {
                string returned = "{ type: ";

                returned += type.ToString();

                returned += ", value: ";

                returned += value.ToString() + " }";

                return returned;
            }

        }

        public class IntVal : RuntimeVal
        {
            public int value;

            public IntVal(int val)
            {
                type = ValueType.Integer;
                value = val;
            }

            public override string ToString()
            {
                /*string returned = "{ type: ";

                returned += type.ToString();

                returned += ", value: ";

                returned += value.ToString() + " }";

                return returned;*/
                return value.ToString();
            }
        }

        public class FloatVal : RuntimeVal
        {
            public float value;

            public FloatVal(float val)
            {
                type = ValueType.Float;
                value = val;
            }

            public override string ToString()
            {
                /*string returned = "{ type: ";

                returned += type.ToString();

                returned += ", value: ";

                returned += value.ToString() + " }";

                return returned;*/
                return value.ToString();
            }
        }

        public class CharVal : RuntimeVal
        {
            public char value;

            public CharVal(char val)
            {
                type = ValueType.Character;
                value = val;
            }

            public override string ToString()
            {
                /*string returned = "{ type: ";

                returned += type.ToString();

                returned += ", value: ";

                returned += value.ToString() + " }";

                return returned;*/
                return value.ToString();
            }
        }

        public class StringVal : RuntimeVal
        {
            public string value;

            public StringVal(string val)
            {
                type = ValueType.String;
                value = val;
            }

            public override string ToString()
            {
                /*string returned = "{ type: ";

                returned += type.ToString();

                returned += ", value: ";

                returned += value.ToString() + " }";

                return returned;*/
                return value.ToString();
            }
        }

        public class BoolVal : RuntimeVal
        {
            public bool value;

            public BoolVal(bool val)
            {
                type = ValueType.Boolean;
                value = val;
            }

            public override string ToString()
            {
                string returned = "{ type: ";

                returned += type.ToString();

                returned += ", value: ";

                returned += value.ToString() + " }";

                return returned;
            }
        }

        public class ObjectVal : RuntimeVal
        {
            public Dictionary<string, RuntimeVal> properties;

            public ObjectVal()
            {
                type = ValueType.Object;
                properties = new Dictionary<string, RuntimeVal>();
            }

            public override string ToString()
            {
                string returned = "{ type: ";

                returned += type.ToString();

                returned += ", properties: [";

                List<KeyValuePair<string, RuntimeVal>> prop_list = properties.ToList();

                foreach(KeyValuePair<string, RuntimeVal> pair in prop_list) 
                {
                    returned += "\n{key: " + pair.Key + ", value: " + pair.Value + "},";
                }

                returned += "\n]}";

                return returned;
            }
        }

        public delegate RuntimeVal FunctionCall(List<RuntimeVal> args, Environment env);

        public class NativeFnVal : RuntimeVal
        {
            public FunctionCall call;

            public NativeFnVal(FunctionCall call)
            {
                type = ValueType.NativeFn;
                this.call = call;
            }

            public override string ToString()
            {
                string returned = "{ type: ";

                returned += type.ToString();

                returned += ", call: " + call.ToString();

                returned += "}";

                return returned;
            }
        }

        public class FunctionVal : RuntimeVal
        {
            public string name;
            public List<string> parameters;
            public Environment declaration_environment;
            public List<AST.Statement> body;

            public AST.DataType returnType;

            public FunctionVal(string name, List<string> parameters, Environment declaration_environment, List<AST.Statement> body, AST.DataType ret_type)
            {
                type = ValueType.Function;
                this.name = name;
                this.parameters = parameters;
                this.declaration_environment = declaration_environment;
                this.body = body;
                returnType = ret_type;
            }

            public override string ToString()
            {
                string returned = "{ type: ";

                returned += type.ToString();

                returned += "}";

                return returned;
            }
        }

        public static AST.DataType value_type_to_data_type(ValueType valueType) 
        {
            switch(valueType) 
            {
                case ValueType.NativeFn:
                case ValueType.Function:
                    return AST.DataType.Function;

                case ValueType.Null: 
                    return AST.DataType.Null;

                case ValueType.Object: 
                    return AST.DataType.Object;

                case ValueType.Boolean: 
                    return AST.DataType.Bool;

                case ValueType.Integer: 
                    return AST.DataType.Int;

                case ValueType.Character: 
                    return AST.DataType.Char;

                case ValueType.String: 
                    return AST.DataType.String;

                case ValueType.Float: 
                    return AST.DataType.Float;

                default:
                    throw new Exception("Unsupported ValueType");
            }
        }
    }
}
