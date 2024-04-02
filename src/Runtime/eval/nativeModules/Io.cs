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
    internal static class Io
    {
        // Define Native IO Functions
        static Dictionary<string, Values.RuntimeVal> ioFunctions = new Dictionary<string, Values.RuntimeVal>()
        {
            { "print", new Values.NativeFnVal( (List<RuntimeVal>args, Environment env) =>
                {
                    foreach(RuntimeVal arg in args)
                    {
                        Console.Write(arg.ToString());
                    }
                    return new Values.NullVal();
                })
            },
            { "println", new Values.NativeFnVal((List<RuntimeVal> args, Environment env) =>
                {
                    foreach (RuntimeVal arg in args)
                    {
                        Console.WriteLine(arg.ToString());
                    }
                    return new Values.NullVal();
                })
            },
            { "readln", new Values.NativeFnVal((List<RuntimeVal> args, Environment env) =>
                {
                    string? readVal = Console.ReadLine();

                    if (readVal == null)
                    {
                        readVal = "";
                    }

                    return new Values.StringVal(readVal);
                })
            },
            { "clearConsole", new Values.NativeFnVal((List<RuntimeVal> args, Environment env) =>
                {
                    Console.Clear();
                    return new Values.NullVal();
                })
            },
            { "consoleBackgroundColor", new Values.NativeFnVal((List<RuntimeVal> args, Environment env) =>
                {
                    if(args.Count > 0)
                    {
                        if (args[0].type == Values.ValueType.Integer)
                        {
                            Values.IntVal intVal = (Values.IntVal)args[0];
                            if(intVal.value >= 0 && intVal.value <= 15)
                            {
                                Console.BackgroundColor = (ConsoleColor)intVal.value;
                                return new Values.NullVal();
                            } else
                            {
                                throw new Exception("Parameter should be in range between 0-15");
                            }
                        } else
                        {
                            throw new Exception("First Argument Should be Integer Type");
                        }
                    } else
                    {
                        throw new Exception("Expected Integer Argument");
                    }
                })

            },
            { "consoleTextColor", new Values.NativeFnVal((List<RuntimeVal> args, Environment env) =>
                {
                    if(args.Count > 0)
                    {
                        if (args[0].type == Values.ValueType.Integer)
                        {
                            Values.IntVal intVal = (Values.IntVal)args[0];
                            if(intVal.value >= 0 && intVal.value <= 15)
                            {
                                Console.ForegroundColor = (ConsoleColor)intVal.value;
                                return new Values.NullVal();
                            } else
                            {
                                throw new Exception("Parameter should be in range between 0-15");
                            }
                        } else
                        {
                            throw new Exception("First Argument Should be Integer Type");
                        }
                    } else
                    {
                        throw new Exception("Expected Integer Argument");
                    }
                })

            },
            { "readTextFile", new Values.NativeFnVal((List<RuntimeVal> args, Environment env) =>
                {
                    if(args.Count > 0)
                    {
                        if(args[0].type == Values.ValueType.String)
                        {
                            return new Values.StringVal( File.ReadAllText( ((Values.StringVal) args[0]).value ) );
                        } else
                        {
                            throw new Exception("Expected String at first argument, recieved type: " + args[0].type);
                        }
                    } else
                    {
                        throw new Exception("Expected argument");
                    }

                })
            },
            { "writeTextFile", new Values.NativeFnVal((List<RuntimeVal> args, Environment env) =>
                {
                    if(args.Count > 1)
                    {
                        if(args[0].type == Values.ValueType.String && args[1].type == Values.ValueType.String)
                        {
                            File.WriteAllText( ((Values.StringVal) args[0]).value, ((Values.StringVal) args[1]).value );
                            return new NullVal();
                        } else
                        {
                            throw new Exception("Expected String at first and second argument, recieved types: " + args[0].type + ", " + args[1].type);
                        }
                    } else
                    {
                        throw new Exception("Expected argument");
                    }

                })
            },
        };

        public static Values.ObjectVal getModule() 
        {
            Values.ObjectVal io = new Values.ObjectVal();
            io.properties = ioFunctions;

            return io;
        }
    }
}
