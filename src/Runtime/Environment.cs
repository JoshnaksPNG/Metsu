using Microsoft.VisualBasic;
using NewLangInterpreter.Frontend;
using NewLangInterpreter.src.Frontend;
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

            if (type != env.var_types[name] && type != AST.DataType.Null)
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
