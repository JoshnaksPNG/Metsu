using NewLangInterpreter.Frontend;
using System;
using System.Collections.Generic;
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

        public class StringVal : RuntimeVal
        {
            public string value;

            public StringVal(string val)
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

            public FunctionVal(string name, List<string> parameters, Environment declaration_environment, List<AST.Statement> body)
            {
                type = ValueType.Function;
                this.name = name;
                this.parameters = parameters;
                this.declaration_environment = declaration_environment;
                this.body = body;
            }

            public override string ToString()
            {
                string returned = "{ type: ";

                returned += type.ToString();

                returned += "}";

                return returned;
            }
        }
    }
}
