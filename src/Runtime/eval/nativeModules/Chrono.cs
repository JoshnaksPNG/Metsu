using NewLangInterpreter.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NewLangInterpreter.Runtime.Values;
using Environment = NewLangInterpreter.Runtime.Environment;

namespace metsu.src.Runtime.eval.nativeModules
{
    internal static class Chrono
    {
        // Define Time Functions
        static Dictionary<string, Values.RuntimeVal> chronoFunctions = new Dictionary<string, Values.RuntimeVal>()
        {
            { "year", new Values.NativeFnVal((List<RuntimeVal> args, Environment env) =>
                {

                    return new Values.IntVal(DateTime.Now.Year);

                })
            },
            { "month", new Values.NativeFnVal((List<RuntimeVal> args, Environment env) =>
                {

                    return new Values.IntVal(DateTime.Now.Month);

                })
            },
            { "day", new Values.NativeFnVal((List<RuntimeVal> args, Environment env) =>
                {

                    return new Values.IntVal(DateTime.Now.Day);

                })
            },
            { "hour", new Values.NativeFnVal((List<RuntimeVal> args, Environment env) =>
                {

                    return new Values.IntVal(DateTime.Now.Hour);

                })
            },
            { "minute", new Values.NativeFnVal((List<RuntimeVal> args, Environment env) =>
                {

                    return new Values.IntVal(DateTime.Now.Minute);

                })
            },
            { "second", new Values.NativeFnVal((List<RuntimeVal> args, Environment env) =>
                {

                    return new Values.IntVal(DateTime.Now.Second);

                })
            },
            { "sleep_second", new Values.NativeFnVal((List<RuntimeVal> args, Environment env) =>
                {
                    System.Threading.Thread.Sleep(1000);
                    return new Values.NullVal();

                })
            },
            { "sleep", new Values.NativeFnVal((List<RuntimeVal> args, Environment env) =>
                {
                    if (args[0].type == Values.ValueType.Integer)
                    {
                        IntVal time = (IntVal)args[0];
                        System.Threading.Thread.Sleep(time.value);
                    }
                    else
                    {
                        throw new Exception("sleep only accepts one int");
                    }


                    return new Values.NullVal();

                })
            },
        };

        public static Values.ObjectVal getModule()
        {
            Values.ObjectVal chrono = new Values.ObjectVal();
            chrono.properties = chronoFunctions;

            return chrono;
        }
    }
}
