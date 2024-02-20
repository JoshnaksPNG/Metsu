using NewLangInterpreter.Frontend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static NewLangInterpreter.Frontend.AST;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NewLangInterpreter.Runtime.eval
{
    internal static class Expressions
    {
        public static Values.RuntimeVal eval_int_bin_expr(Values.IntVal left_side, Values.IntVal right_side, AST.Operator opr, bool is_silly)
        {
            int result = 0;

            switch (opr)
            {
                case AST.Operator.Add:
                    if(is_silly && left_side.value == 9 && right_side.value == 10) 
                    {
                        result = 21;
                        break;
                    }
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
                    return new Values.ErrorVal("Operator not supported: " + opr.ToString());
            }

            return new Values.IntVal(result);
        }

        public static Values.RuntimeVal eval_float_bin_expr(Values.FloatVal left_side, Values.FloatVal right_side, AST.Operator opr, bool is_silly)
        {
            float result = 0;

            switch (opr)
            {
                case AST.Operator.Add:
                    if (is_silly && left_side.value == 9 && right_side.value == 10)
                    {
                        result = 21;
                        break;
                    }
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
                    return new Values.ErrorVal("Operator not supported: " + opr.ToString());
            }

            return new Values.FloatVal(result);
        }

        public static Values.RuntimeVal eval_string_bin_expr(Values.RuntimeVal left_side, Values.RuntimeVal right_side, AST.Operator opr, bool is_silly)
        {
            string result = "";

            string left = "";
            string right = "";

            switch (left_side.type)
            {
                case Values.ValueType.Null:
                    left = "null"; 
                    break;

                case Values.ValueType.Boolean:
                    left = "" + ((Values.BoolVal)left_side).value; 
                    break;

                case Values.ValueType.Float:
                    left = "" + ((Values.FloatVal)left_side).value;
                    break;

                case Values.ValueType.Integer:
                    left = "" + ((Values.IntVal)left_side).value;
                    break;

                case Values.ValueType.Character:
                    left = "" + ((Values.CharVal)left_side).value;
                    break;

                case Values.ValueType.String:
                    left = ((Values.StringVal)left_side).value;
                    break;

                case Values.ValueType.Array:
                    left = "[ARRAY VALUE]";
                    break;

                case Values.ValueType.Object:
                    left = "{OBJECT VALUE}";
                    break;

                case Values.ValueType.NativeFn:
                case Values.ValueType.Function:
                    left = "{FUNCTION VALUE}";
                    break;

                default:
                    return new Values.ErrorVal("Unexpected DataType: " + left_side.type.ToString());
            }

            switch (right_side.type)
            {
                case Values.ValueType.Null:
                    right = "null";
                    break;

                case Values.ValueType.Boolean:
                    right = "" + ((Values.BoolVal)right_side).value;
                    break;

                case Values.ValueType.Float:
                    right = "" + ((Values.FloatVal)right_side).value;
                    break;

                case Values.ValueType.Integer:
                    right = "" + ((Values.IntVal)right_side).value;
                    break;

                case Values.ValueType.Character:
                    right = "" + ((Values.CharVal)right_side).value;
                    break;

                case Values.ValueType.String:
                    right = ((Values.StringVal)right_side).value;
                    break;

                case Values.ValueType.Array:
                    right = "[ARRAY VALUE]";
                    break;

                case Values.ValueType.Object:
                    right = "{OBJECT VALUE}";
                    break;

                case Values.ValueType.NativeFn:
                case Values.ValueType.Function:
                    right = "{FUNCTION VALUE}";
                    break;

                default:
                    return new Values.ErrorVal("Unexpected DataType: " + right_side.type.ToString());
            }

            switch (opr)
            {
                case AST.Operator.Add:
                    result = left + right;
                    break;

                default:
                    return new Values.ErrorVal("Operator not supported: " + opr.ToString());
            }

            return new Values.StringVal(result);
        }

        public static Values.RuntimeVal eval_comparison_expr(Values.RuntimeVal left_val, Values.RuntimeVal right_val, AST.Operator opr)
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
                        return new Values.ErrorVal("Cannot Apply Operator " + opr + "To String Value");

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
                        return new Values.ErrorVal("Cannot Apply Operator " + opr + "To String Value");

                    case AST.Operator.EqualTo:
                        return new Values.BoolVal(left_state == right_state);

                    case AST.Operator.NotEqualTo:
                        return new Values.BoolVal(left_state != right_state);
                }
            }

            return new Values.ErrorVal("Cannot Apply Operator " + opr + "To Value");
        }

        public static Values.RuntimeVal eval_logical_bin_expr(Values.BoolVal left,  Values.BoolVal right, AST.Operator opr) 
        {
            if (opr == AST.Operator.LogicalAnd)
            {
                return new Values.BoolVal(left.value && right.value);
            }
            else
            {
                return new Values.BoolVal(left.value || right.value);
            }
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

                    case AST.Operator.LogicalAnd:
                    case AST.Operator.LogicalOr:
                        if (left_side.type == Values.ValueType.Boolean && right_side.type == Values.ValueType.Boolean)
                        {
                            return eval_logical_bin_expr((Values.BoolVal)left_side, (Values.BoolVal)right_side, binaryExpr.opr);
                        }
                        else
                        {
                            return new Values.ErrorVal("Logical operators only apply to boolean values");
                        }

                }

                if (left_side.type == Values.ValueType.Integer)
                {
                    return eval_int_bin_expr((Values.IntVal)left_side, (Values.IntVal)right_side, binaryExpr.opr, env.is_silly);
                }
                else if (left_side.type == Values.ValueType.Float)
                {
                    return eval_float_bin_expr((Values.FloatVal)left_side, (Values.FloatVal)right_side, binaryExpr.opr, env.is_silly);
                }
                else if (left_side.type == Values.ValueType.String)
                {
                    return eval_string_bin_expr(left_side, right_side, binaryExpr.opr, env.is_silly);
                }
                return new Values.NullVal();
            }
            else if (left_side.type == Values.ValueType.Float && right_side.type == Values.ValueType.Integer)
            {
                return eval_float_bin_expr((Values.FloatVal)left_side, new Values.FloatVal(((Values.IntVal)right_side).value), binaryExpr.opr, env.is_silly);
            }
            else if (left_side.type == Values.ValueType.Integer && right_side.type == Values.ValueType.Float)
            {
                return eval_float_bin_expr(new Values.FloatVal(((Values.IntVal)left_side).value), (Values.FloatVal)right_side, binaryExpr.opr, env.is_silly);
            }
            else if (left_side.type == Values.ValueType.String || right_side.type == Values.ValueType.String)
            {
                return eval_string_bin_expr(left_side, right_side, binaryExpr.opr, env.is_silly);
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
            if(node.assignee.kind == NodeType.IndexExpr) 
            {
                AST.ArrayIndexExpr idx_expr = (AST.ArrayIndexExpr) node.assignee;

                Identifier arr_symbol = (Identifier) idx_expr.arr;

                Values.ArrayVal arr_val = (Values.ArrayVal)env.lookupVar(arr_symbol.symbol);

                Values.IntVal int_val = (Values.IntVal)Interpreter.evaluate(idx_expr.idx, env);

                if (int_val.value >= arr_val.elements.Count)
                {
                    return new Values.ErrorVal("Index: " + int_val.value + " Out Of Bounds");
                }

                Values.RuntimeVal assignVal = Interpreter.evaluate(node.value, env);

                arr_val.elements[int_val.value] = assignVal;

                return assignVal;

            } else if(node.assignee.kind != AST.NodeType.Identifier) 
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

        public static Values.RuntimeVal eval_array_expr(AST.ArrayLiteral arr, Environment env)
        {
            List<Values.RuntimeVal> elems = new List<Values.RuntimeVal>();

            Values.ValueType arrType = Values.ValueType.Null;

            foreach (AST.Expression element in arr.elements)
            {
                Values.RuntimeVal val = Interpreter.evaluate(element, env);

                if (arrType == Values.ValueType.Null)
                {
                    arrType = val.type;
                }
                else if(val.type != arrType)
                {
                    return new Values.ErrorVal("Array element type does not match the type of array literal");
                }

                elems.Add(val);
            }

            Values.ArrayVal array = new Values.ArrayVal(Values.value_type_to_data_type(arrType), elems);

            return array;
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

                Environment scope = new Environment(func.declaration_environment, true, func.returnType);

                // Create Variables from Parameter List
                for(int i = 0; i < func.parameters.Count; i++) 
                {
                    // TODO: Verify function has all parameters

                    scope.declareVar(func.parameters[i], args[i], true, Values.value_type_to_data_type(args[i].type)); // Parameters are constant
                }

                Values.RuntimeVal? result = null;
                

                // Evaluate Function Body
                foreach (AST.Statement stmt in func.body)
                {
                    result = Interpreter.evaluate(stmt, scope);

                    if(scope.should_kill) 
                    {
                        break;
                    }
                }

                if (result != null)
                {
                    return result;
                }

                if (func.returnType == AST.DataType.Void)
                {
                    return new Values.NullVal();
                }

                throw new Exception("Function Body Returned Null Result");
            }

            throw new Exception("Cannot call non-function value");
        }

        public static Values.RuntimeVal eval_member_expr(AST.MemberExpr member, Environment env)
        {
            Identifier obj = (Identifier)(member.obj);

            Identifier prop = (Identifier) member.property;

            Values.ObjectVal obj_val = (Values.ObjectVal) env.lookupVar(obj.symbol);

            return obj_val.properties[prop.symbol];
        }

        public static Values.RuntimeVal eval_index_expr(AST.ArrayIndexExpr arr, Environment env)
        {
            Identifier array = (Identifier)(arr.arr);

            Values.ArrayVal arr_val = (Values.ArrayVal) env.lookupVar (array.symbol);

            Values.IntVal int_val = (Values.IntVal) Interpreter.evaluate(arr.idx, env);

            if(int_val.value >= arr_val.elements.Count) 
            {
                return new Values.ErrorVal("Index: " + int_val.value + " Out Of Bounds");
            }

            return arr_val.elements[int_val.value];
        }
    }
}
