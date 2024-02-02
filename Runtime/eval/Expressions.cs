using NewLangInterpreter.Frontend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NewLangInterpreter.Frontend.AST;

namespace NewLangInterpreter.Runtime.eval
{
    internal static class Expressions
    {
        public static Values.IntVal eval_int_bin_expr(Values.IntVal left_side, Values.IntVal right_side, AST.Operator opr)
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

        // TODO
        public static Values.BoolVal eval_comparison_expr(Values.RuntimeVal left_val, Values.RuntimeVal right_val, AST.Operator opr)
        {
            if (right_val.type == Values.ValueType.Float || right_val.type == Values.ValueType.Integer)
            {
                float right_state;
                float left_state;

                if (right_val.type == Values.ValueType.Float)
                {
                    right_state = ((Values.FloatVal)right_val).value;
                }
                else
                {
                    right_state = ((Values.IntVal)right_val).value;
                }

                if (left_val.type == Values.ValueType.Float)
                {
                    left_state = ((Values.FloatVal)left_val).value;
                }
                else
                {
                    left_state = ((Values.IntVal)left_val).value;
                }


                switch (opr)
                {
                    case AST.Operator.LessEqTo:
                        return new Values.BoolVal(left_state <= right_state);

                    case AST.Operator.LessThan:
                        return new Values.BoolVal(left_state < right_state);

                    case AST.Operator.GreaterEqTo:
                        return new Values.BoolVal(left_state >= right_state);

                    case AST.Operator.GreaterThan:
                        return new Values.BoolVal(left_state > right_state);

                    case AST.Operator.EqualTo:
                        return new Values.BoolVal(left_state == right_state);

                    case AST.Operator.NotEqualTo:
                        return new Values.BoolVal(left_state != right_state);
                }

            }
            else if (right_val.type == Values.ValueType.String)
            {
                string right_state = ((Values.StringVal)right_val).value;
                string left_state = ((Values.StringVal)left_val).value;

                switch (opr)
                {
                    case AST.Operator.LessEqTo:
                    case AST.Operator.LessThan:
                    case AST.Operator.GreaterEqTo:
                    case AST.Operator.GreaterThan:
                        throw new Exception("Cannot Apply Operator " + opr + "To String Value");

                    case AST.Operator.EqualTo:
                        return new Values.BoolVal(left_state == right_state);

                    case AST.Operator.NotEqualTo:
                        return new Values.BoolVal(left_state != right_state);
                }
            }
            else if (right_val.type == Values.ValueType.Character)
            {
                char right_state = ((Values.CharVal)right_val).value;
                char left_state = ((Values.CharVal)left_val).value;

                switch (opr)
                {
                    case AST.Operator.LessEqTo:
                    case AST.Operator.LessThan:
                    case AST.Operator.GreaterEqTo:
                    case AST.Operator.GreaterThan:
                        throw new Exception("Cannot Apply Operator " + opr + "To String Value");

                    case AST.Operator.EqualTo:
                        return new Values.BoolVal(left_state == right_state);

                    case AST.Operator.NotEqualTo:
                        return new Values.BoolVal(left_state != right_state);
                }
            }

            throw new Exception("Cannot Apply Operator " + opr + "To Value");
        }

        public static Values.RuntimeVal eval_binary_expression(AST.BinaryExpr binaryExpr, Environment env)
        {
            Values.RuntimeVal left_side = Interpreter.evaluate(binaryExpr.left, env);
            Values.RuntimeVal right_side = Interpreter.evaluate(binaryExpr.right, env);

            if (left_side.type == right_side.type)
            {
                switch (binaryExpr.opr)
                {
                    case AST.Operator.LessEqTo:
                    case AST.Operator.LessThan:
                    case AST.Operator.GreaterEqTo:
                    case AST.Operator.GreaterThan:
                    case AST.Operator.EqualTo:
                    case AST.Operator.NotEqualTo:
                        return eval_comparison_expr(left_side, right_side, binaryExpr.opr);
                }

                if (left_side.type == Values.ValueType.Integer)
                {
                    return eval_int_bin_expr((Values.IntVal)left_side, (Values.IntVal)right_side, binaryExpr.opr);
                } 
                return new Values.NullVal();
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
