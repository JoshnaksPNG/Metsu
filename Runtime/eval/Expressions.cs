using NewLangInterpreter.Frontend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewLangInterpreter.Runtime.eval
{
    internal static class Expressions
    {
        public static Values.IntVal eval_numeric_bin_expr(Values.IntVal left_side, Values.IntVal right_side, AST.Operator opr)
        {
            int result = 0;

            switch (opr)
            {
                case AST.Operator.Add:
                    result = left_side.value + right_side.value;
                    break;

                case AST.Operator.Subtract:
                    result = left_side.value - right_side.value;
                    break;

                case AST.Operator.Multiply:
                    result = left_side.value * right_side.value;
                    break;

                case AST.Operator.Divide:
                    result = left_side.value / right_side.value;
                    break;

                case AST.Operator.Modulo:
                    result = left_side.value % right_side.value;
                    break;

                default:
                    result = 0;
                    break;
            }

            return new Values.IntVal(result);
        }

        public static Values.RuntimeVal eval_binary_expression(AST.BinaryExpr binaryExpr, Environment env)
        {
            Values.RuntimeVal left_side = Interpreter.evaluate(binaryExpr.left, env);
            Values.RuntimeVal right_side = Interpreter.evaluate(binaryExpr.right, env);

            if (left_side.type == right_side.type && left_side.type != Values.ValueType.Null)
            {
                return eval_numeric_bin_expr((Values.IntVal)left_side, (Values.IntVal)right_side, binaryExpr.opr);
            }
            else
            {
                return new Values.NullVal();
            }
        }

        public static Values.RuntimeVal eval_identifier(AST.Identifier identifier, Environment env)
        {
            Values.RuntimeVal val = env.lookupVar(identifier.symbol);
            return val;
        }

        public static Values.RuntimeVal eval_assignment(AST.AssignmentExpr node, Environment env)
        {
            if(node.assignee.kind != AST.NodeType.Identifier) 
            {
                Console.Error.WriteLine("Error: Cannot assign to non-identifier");
                System.Environment.Exit(0);
            }

            return env.assignVar(((AST.Identifier)node.assignee).symbol, Interpreter.evaluate(node.value, env));
        }

        public static Values.RuntimeVal eval_object_expr(AST.ObjectLiteral obj, Environment env)
        {
            Values.ObjectVal _obj = new Values.ObjectVal();

            foreach(AST.Property property in obj.properties) 
            {
                
                _obj.properties.Add(property.key, Interpreter.evaluate(property.value, env));
            }

            return _obj;
        }

        public static Values.RuntimeVal eval_call_expr(AST.CallExpr call, Environment env)
        {
            List<Values.RuntimeVal> args = new List<Values.RuntimeVal>();//(List<Values.RuntimeVal>) call.args.Select((arg) => Interpreter.evaluate(arg, env));

            foreach (AST.Expression expression in call.args)
            {
                args.Add(Interpreter.evaluate(expression, env));
            }

            Values.RuntimeVal fn = Interpreter.evaluate(call.callee, env);

            if (fn.type == Values.ValueType.NativeFn)
            {
                Values.RuntimeVal result = ((Values.NativeFnVal)fn).call(args, env);

                return result;

            }
            
            if (fn.type == Values.ValueType.Function)
            {
                Values.FunctionVal func = (Values.FunctionVal) fn;

                Environment scope = new Environment(func.declaration_environment);

                // Create Variables from Parameter List
                for(int i = 0; i < func.parameters.Count; i++) 
                {
                    // TODO: Verify function has all parameters

                    scope.declareVar(func.parameters[i], args[i], true, Values.value_type_to_data_type(args[i].type)); // Parameters are constant
                }

                Values.RuntimeVal result = null;
                

                // Evaluate Function Body
                foreach (AST.Statement stmt in func.body)
                {
                    result = Interpreter.evaluate(stmt, scope);
                }

                if (result != null)
                {
                    return result;
                }

                throw new Exception("Function Body Returned Null Result");
            }

            throw new Exception("Cannot call non-function value");
        }
    }
}
