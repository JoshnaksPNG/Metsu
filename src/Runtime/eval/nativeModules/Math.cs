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
    internal static class Math
    {
        // Define Native IO Functions
        static Dictionary<string, Values.RuntimeVal> mathFunctions = new Dictionary<string, Values.RuntimeVal>()
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
            { "round", new Values.NativeFnVal((List<RuntimeVal> args, Environment env) =>
                {
                    float param;

                    if (args[0].type == Values.ValueType.Integer)
                    {
                        return args[0];
                    } else if (args[0].type == Values.ValueType.Float)
                    {
                        param = ((Values.FloatVal)args[0]).value;
                    } else
                    {
                        return new Values.ErrorVal("Native Function Call Error: math.round only accepts an int or float value as the first parameter.");
                    }

                    int val = (int) System.Math.Round(param);

                    return new Values.IntVal(val);

                })
            },
            { "floor", new Values.NativeFnVal((List<RuntimeVal> args, Environment env) =>
                {
                    float param;

                    if (args[0].type == Values.ValueType.Integer)
                    {
                        return args[0];
                    } else if (args[0].type == Values.ValueType.Float)
                    {
                        param = ((Values.FloatVal)args[0]).value;
                    } else
                    {
                        return new Values.ErrorVal("Native Function Call Error: math.floor only accepts an int or float value as the first parameter.");
                    }

                    int val = (int) System.Math.Floor(param);

                    return new Values.IntVal(val);

                })
            },
            { "ceiling", new Values.NativeFnVal((List<RuntimeVal> args, Environment env) =>
                {
                    float param;

                    if (args[0].type == Values.ValueType.Integer)
                    {
                        return args[0];
                    } else if (args[0].type == Values.ValueType.Float)
                    {
                        param = ((Values.FloatVal)args[0]).value;
                    } else
                    {
                        return new Values.ErrorVal("Native Function Call Error: math.ceiling only accepts an int or float value as the first parameter.");
                    }

                    int val = (int) System.Math.Ceiling(param);

                    return new Values.IntVal(val);

                })
            },
            { "sin", new Values.NativeFnVal((List<RuntimeVal> args, Environment env) =>
                {
                    float param;

                    if (args[0].type == Values.ValueType.Integer)
                    {
                        param = ((Values.IntVal)args[0]).value;
                    } else if (args[0].type == Values.ValueType.Float)
                    {
                        param = ((Values.FloatVal)args[0]).value;
                    } else
                    {
                        return new Values.ErrorVal("Native Function Call Error: math.sin only accepts an int or float value as the first parameter.");
                    }

                    float val = (float) System.Math.Sin(param);

                    return new Values.FloatVal(val);

                })
            },
            { "cos", new Values.NativeFnVal((List<RuntimeVal> args, Environment env) =>
                {
                    float param;

                    if (args[0].type == Values.ValueType.Integer)
                    {
                        param = ((Values.IntVal)args[0]).value;
                    } else if (args[0].type == Values.ValueType.Float)
                    {
                        param = ((Values.FloatVal)args[0]).value;
                    } else
                    {
                        return new Values.ErrorVal("Native Function Call Error: math.cos only accepts an int or float value as the first parameter.");
                    }

                    float val = (float) System.Math.Cos(param);

                    return new Values.FloatVal(val);

                })
            },
            { "tan", new Values.NativeFnVal((List<RuntimeVal> args, Environment env) =>
                {
                    float param;

                    if (args[0].type == Values.ValueType.Integer)
                    {
                        param = ((Values.IntVal)args[0]).value;
                    } else if (args[0].type == Values.ValueType.Float)
                    {
                        param = ((Values.FloatVal)args[0]).value;
                    } else
                    {
                        return new Values.ErrorVal("Native Function Call Error: math.tan only accepts an int or float value as the first parameter.");
                    }

                    float val = (float) System.Math.Tan(param);

                    return new Values.FloatVal(val);

                })
            },
            { "asin", new Values.NativeFnVal((List<RuntimeVal> args, Environment env) =>
                {
                    float param;

                    if (args[0].type == Values.ValueType.Integer)
                    {
                        param = ((Values.IntVal)args[0]).value;
                    } else if (args[0].type == Values.ValueType.Float)
                    {
                        param = ((Values.FloatVal)args[0]).value;
                    } else
                    {
                        return new Values.ErrorVal("Native Function Call Error: math.asin only accepts an int or float value as the first parameter.");
                    }

                    float val = (float) System.Math.Asin(param);

                    return new Values.FloatVal(val);

                })
            },
            { "acos", new Values.NativeFnVal((List<RuntimeVal> args, Environment env) =>
                {
                    float param;

                    if (args[0].type == Values.ValueType.Integer)
                    {
                        param = ((Values.IntVal)args[0]).value;
                    } else if (args[0].type == Values.ValueType.Float)
                    {
                        param = ((Values.FloatVal)args[0]).value;
                    } else
                    {
                        return new Values.ErrorVal("Native Function Call Error: math.acos only accepts an int or float value as the first parameter.");
                    }

                    float val = (float) System.Math.Acos(param);

                    return new Values.FloatVal(val);

                })
            },
            { "atan", new Values.NativeFnVal((List<RuntimeVal> args, Environment env) =>
                {
                    float param;

                    if (args[0].type == Values.ValueType.Integer)
                    {
                        param = ((Values.IntVal)args[0]).value;
                    } else if (args[0].type == Values.ValueType.Float)
                    {
                        param = ((Values.FloatVal)args[0]).value;
                    } else
                    {
                        return new Values.ErrorVal("Native Function Call Error: math.atan only accepts an int or float value as the first parameter.");
                    }

                    float val = (float) System.Math.Atan(param);

                    return new Values.FloatVal(val);

                })
            },
            { "atan2", new Values.NativeFnVal((List<RuntimeVal> args, Environment env) =>
                {
                    float y;
                    float x;

                    if (args[0].type == Values.ValueType.Integer)
                    {
                        y = ((Values.IntVal)args[0]).value;
                    } else if (args[0].type == Values.ValueType.Float)
                    {
                        y = ((Values.FloatVal)args[0]).value;
                    } else
                    {
                        return new Values.ErrorVal("Native Function Call Error: math.atan2 only accepts an int or float value as first parameters.");
                    }

                    if (args[0].type == Values.ValueType.Integer)
                    {
                        x = ((Values.IntVal)args[1]).value;
                    } else if (args[0].type == Values.ValueType.Float)
                    {
                        x = ((Values.FloatVal)args[1]).value;
                    } else
                    {
                        return new Values.ErrorVal("Native Function Call Error: math.atan2 only accepts an int or float value as first parameters.");
                    }

                    float val = (float) System.Math.Atan2(y, x);

                    return new Values.FloatVal(val);

                })
            },
        };

        public static Values.ObjectVal getModule()
        {
            Values.ObjectVal math = new Values.ObjectVal();
            math.properties = mathFunctions;

            return math;
        }
    }
}
