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

            // Define native method
            scope.declareVar("print", new Values.NativeFnVal( (List<RuntimeVal>args, Environment env) => 
            {
                foreach(RuntimeVal arg in args) 
                {
                    Console.Write(arg.ToString());
                }
                return new Values.NullVal();
            }), true, AST.DataType.Function);

            scope.declareVar("println", new Values.NativeFnVal((List<RuntimeVal> args, Environment env) =>
            {
                foreach (RuntimeVal arg in args)
                {
                    Console.WriteLine(arg.ToString());
                }
                return new Values.NullVal();
            }), true, AST.DataType.Function);

            scope.declareVar("hour", new Values.NativeFnVal((List<RuntimeVal> args, Environment env) =>
            {

                return new Values.IntVal(DateTime.Now.Hour);

            }), true, AST.DataType.Function);

            scope.declareVar("minute", new Values.NativeFnVal((List<RuntimeVal> args, Environment env) =>
            {

                return new Values.IntVal(DateTime.Now.Minute);

            }), true, AST.DataType.Function);

            scope.declareVar("second", new Values.NativeFnVal((List<RuntimeVal> args, Environment env) =>
            {

                return new Values.IntVal(DateTime.Now.Second);

            }), true, AST.DataType.Function);

            scope.declareVar("sleep_second", new Values.NativeFnVal((List<RuntimeVal> args, Environment env) =>
            {
                System.Threading.Thread.Sleep(1000);
                return new Values.IntVal(0);

            }), true, AST.DataType.Function);

            scope.declareVar("sleep", new Values.NativeFnVal((List<RuntimeVal> args, Environment env) =>
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

                
                return new Values.IntVal(0);

            }), true, AST.DataType.Function);
        }

        private bool is_default_constant;

        public bool isGlobal;

        public Environment() 
        {
            isGlobal = true;
            parent = null;
            this.variables = new Dictionary<string, Values.RuntimeVal>();
            this.var_types = new Dictionary<string, AST.DataType>();
            this.constants = new HashSet<string> { };
            this.is_default_constant = true;

            setupGlobal(this);
        }

        public Environment(Environment parent)
        {
            isGlobal = false;
            this.parent = parent;
            this.variables = new Dictionary<string, Values.RuntimeVal>();
            this.var_types = new Dictionary<string, AST.DataType>();
            this.constants = new HashSet<string> { };
            this.is_default_constant = true;
        }

        private Environment? parent;

        public Dictionary<string, Values.RuntimeVal> variables;
        public Dictionary<string, AST.DataType> var_types;
        public HashSet<string> constants;

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

            if (val_type != type)
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
