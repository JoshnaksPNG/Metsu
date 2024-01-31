using NewLangInterpreter.Frontend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewLangInterpreter.Runtime.eval
{
    internal static class Statements
    {
        public static Values.RuntimeVal eval_program(AST.Program program, Environment env)
        {
            Values.RuntimeVal lastEvaluated = new Values.NullVal();

            foreach (AST.Statement statement in program.body)
            {
                lastEvaluated = Interpreter.evaluate(statement, env);
            }

            return lastEvaluated;
        }

        public static Values.RuntimeVal eval_var_declaration(AST.VarDeclaration declaration, Environment env)
        {
            AST.Expression val = declaration.value != null ? declaration.value : new AST.NullLiteral();

            Values.RuntimeVal v = Interpreter.evaluate(val, env);

            bool assign_const = declaration.is_default ? env.immutable_by_default : declaration.isConstant;

            return env.declareVar(declaration.identifier, v, assign_const, declaration.dataType);
        }

        public static Values.RuntimeVal eval_function_declaration(AST.FunctionDeclaration declaration, Environment env)
        {
            List<string> param_names = new List<string>();

            foreach (KeyValuePair<string, string> pair in declaration.parameters)
            {
                param_names.Add(pair.Key);
            }

            Values.FunctionVal fn = new Values.FunctionVal(declaration.name, param_names, env, declaration.body);

            return env.declareVar(fn.name, fn, true, AST.DataType.Function);
        }

        public static Values.RuntimeVal eval_return(AST.ReturnStatement ret_stmt, Environment env)
        {

            return Interpreter.evaluate(ret_stmt.value, env);
        }

        public static Values.RuntimeVal eval_if_stmt(AST.IfStatement if_stmt, Environment env)
        {
            Values.RuntimeVal condition_val = Interpreter.evaluate(if_stmt.condition, env);

            Environment scope = new Environment(env);

            if (((Values.BoolVal)condition_val).value)
            {
                Values.RuntimeVal result = null;

                foreach (AST.Statement stmt in if_stmt.body) 
                {
                    result = Interpreter.evaluate(stmt, env);
                }

                if (result != null) 
                {
                    return result;
                }

                return new Values.NullVal();
            }

            return new Values.NullVal();
        }

        public static Values.RuntimeVal eval_if_else_stmt(AST.IfElseStatement if_else_stmt, Environment env)
        {
            Values.RuntimeVal condition_val = Interpreter.evaluate(if_else_stmt.ifstmt.condition, env);

            Environment scope = new Environment(env);

            if (((Values.BoolVal)condition_val).value)
            {
                Values.RuntimeVal result = null;

                foreach (AST.Statement stmt in if_else_stmt.ifstmt.body)
                {
                    result = Interpreter.evaluate(stmt, env);
                }

                if (result != null)
                {
                    return result;
                }
            }
            else
            {
                Values.RuntimeVal result = null;

                foreach (AST.Statement stmt in if_else_stmt.body)
                {
                    result = Interpreter.evaluate(stmt, env);
                }

                if (result != null)
                {
                    return result;
                }
            }

            return new Values.NullVal();
        }
    }
}
