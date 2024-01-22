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

            return env.declareVar(declaration.identifier, Interpreter.evaluate(val, env), declaration.isConstant);
        }

        public static Values.RuntimeVal eval_function_declaration(AST.FunctionDeclaration declaration, Environment env)
        {
            List<string> param_names = new List<string>();

            foreach (KeyValuePair<string, string> pair in declaration.parameters)
            {
                param_names.Add(pair.Key);
            }

            Values.FunctionVal fn = new Values.FunctionVal(declaration.name, param_names, env, declaration.body);

            return env.declareVar(fn.name, fn, true);
        }

        public static Values.RuntimeVal eval_return(AST.ReturnStatement ret_stmt, Environment env)
        {

            return Interpreter.evaluate(ret_stmt.value, env);
        }
    }
}
