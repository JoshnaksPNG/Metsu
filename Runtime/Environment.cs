using NewLangInterpreter.Frontend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NewLangInterpreter.Runtime.Values;

namespace NewLangInterpreter.Runtime
{
    internal class Environment
    {
        public static void setupGlobal(Environment scope)
        {
            scope.declareVar("true", new Values.BoolVal(true), true, AST.DataType.Bool);
            scope.declareVar("false", new Values.BoolVal(false), true, AST.DataType.Bool);

            // Define Native IO Functions
            Dictionary<string, Values.RuntimeVal> ioFunctions = new Dictionary<string, Values.RuntimeVal>() 
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

            Values.ObjectVal io = new Values.ObjectVal();
            io.properties = ioFunctions;

            scope.declareVar("io", io, true, AST.DataType.Object);

            // Define Time Functions
            Dictionary<string, Values.RuntimeVal> chronoFunctions = new Dictionary<string, Values.RuntimeVal>()
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

            Values.ObjectVal chrono = new Values.ObjectVal();
            chrono.properties = chronoFunctions;

            scope.declareVar("chrono", chrono, true, AST.DataType.Object);

            // Define Native IO Functions
            Dictionary<string, Values.RuntimeVal> mathFunctions = new Dictionary<string, Values.RuntimeVal>()
            {
                { "randomInt", new Values.NativeFnVal((List<RuntimeVal> args, Environment env) => 
                    {
                        Random r = new Random();

                        if(args.Count > 1 && args[0].type == Values.ValueType.Integer && args[1].type == Values.ValueType.Integer)
                        {
                            int low = ((Values.IntVal)args[0]).value;
                            int high = ((Values.IntVal)args[1]).value;

                            return new Values.IntVal(r.Next(low, high));
                        }

                        return new Values.IntVal(r.Next());
                    })
                },
                { "randomFloat", new Values.NativeFnVal((List<RuntimeVal> args, Environment env) =>
                    {
                        Random r = new Random();

                        return new Values.FloatVal((float)r.NextDouble());
                        
                    })
                },
            };

            Values.ObjectVal math = new Values.ObjectVal();
            math.properties = mathFunctions;

            scope.declareVar("math", math, true, AST.DataType.Object);
        }

        public bool is_default_constant;

        public bool isGlobal;

        public Environment() 
        {
            isGlobal = true;
            parent = null;
            this.variables = new Dictionary<string, Values.RuntimeVal>();
            this.var_types = new Dictionary<string, AST.DataType>();
            this.constants = new HashSet<string> { };
            this.is_default_constant = true;
            this.is_silly = false;
            should_kill = false;

            func_return_type = AST.DataType.Void;

            setupGlobal(this);
        }

        public Environment(Environment parent)
        {
            isGlobal = false;
            this.parent = parent;
            this.variables = new Dictionary<string, Values.RuntimeVal>();
            this.var_types = new Dictionary<string, AST.DataType>();
            this.constants = new HashSet<string> { };
            this.is_default_constant = parent.is_default_constant; ;
            this.is_silly = parent.is_silly;
            should_kill = parent.should_kill;

            func_return_type = parent.func_return_type;
        }

        public Environment(Environment parent, bool is_function_body, AST.DataType ret_type)
        {
            isGlobal = false;
            this.is_function_body = is_function_body;
            this.parent = parent;
            this.variables = new Dictionary<string, Values.RuntimeVal>();
            this.var_types = new Dictionary<string, AST.DataType>();
            this.constants = new HashSet<string> { };
            this.is_default_constant = parent.is_default_constant;
            this.is_silly = parent.is_silly;
            should_kill = parent.should_kill;
            func_return_type = ret_type;
        }

        public Environment? parent;

        public Dictionary<string, Values.RuntimeVal> variables;
        public Dictionary<string, AST.DataType> var_types;
        public HashSet<string> constants;
        public bool is_silly;
        public bool is_function_body;
        public bool should_kill;
        public AST.DataType func_return_type;

        public Values.RuntimeVal declareVar(string name, Values.RuntimeVal value, bool isConst, AST.DataType type) 
        {
            if(this.variables.ContainsKey(name)) 
            {
                Console.Error.WriteLine("Cannot declare variable \"" + name + "\", as it is already defined");
                System.Environment.Exit(0);
            }

            if(isConst) 
            {
                constants.Add(name);
            }

            AST.DataType val_type = Values.value_type_to_data_type(value.type);

            if (val_type != type && val_type != AST.DataType.Null)
            {
                throw new Exception("Cannot assign value of type: " + val_type + " to variable of type: " + type);
            }

            this.variables[name] = value;
            var_types[name] = type;
            return value;
        }

        public Values.RuntimeVal assignVar(string name, Values.RuntimeVal value) 
        {
            Environment env = this.resolve(name);

            // Cannot assign to constant
            if (env.constants.Contains(name))
            {
                Console.Error.WriteLine("Cannot reassign to constant \"" + name + "\"");
                System.Environment.Exit(0);
            }

            AST.DataType type = Values.value_type_to_data_type(value.type);

            if (type != env.var_types[name])
            {
                throw new Exception("Cannot assign value of type: " + type + " to variable of type: " + env.var_types[name]);
            }

            env.variables[name] = value;

            return value;
        }

        public Values.RuntimeVal lookupVar (string name) 
        {
            Environment env = this.resolve(name);
            return env.variables[name];
        }

        public Environment resolve(string name) 
        {
            if (this.variables.ContainsKey(name))
            {
                return this;
            }

            if(this.parent == null) 
            {
                Console.Error.WriteLine("Cannot resolve " + name + " as it does not exist.");
                System.Environment.Exit(0);
            }

            return this.parent.resolve(name);
        }

        public void set_default_const(bool isConst)
        {
            is_default_constant = isConst;
        }
    }
}
