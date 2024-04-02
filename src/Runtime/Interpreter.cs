using NewLangInterpreter.src.Frontend;
using NewLangInterpreter.Runtime.eval;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewLangInterpreter.Runtime
{
    internal static class Interpreter
    {
        public static Values.RuntimeVal evaluate(AST.Statement astNode, Environment env)
        {
            Values.RuntimeVal? eval_value = null;

            switch(astNode.kind) 
            {
                case AST.NodeType.IntLiteral:
                    eval_value = new Values.IntVal(((AST.IntLiteral)astNode).value);
                    break;

                case AST.NodeType.FloatLiteral:
                    eval_value = new Values.FloatVal(((AST.FloatLiteral)astNode).value);
                    break;

                case AST.NodeType.CharLiteral:
                    eval_value = new Values.CharVal(((AST.CharLiteral)astNode).value);
                    break;

                case AST.NodeType.StringLiteral:
                    eval_value = new Values.StringVal(((AST.StringLiteral)astNode).value);
                    break;

                case AST.NodeType.NullLiteral:
                    eval_value = new Values.NullVal();
                    break;

                case AST.NodeType.BoolLiteral:
                    eval_value = new Values.BoolVal(((AST.BoolLiteral)astNode).value);
                    break;

                case AST.NodeType.Identifier:
                    eval_value = Expressions.eval_identifier((AST.Identifier)astNode, env);
                    break;

                case AST.NodeType.ObjectLiteral:
                    eval_value = Expressions.eval_object_expr((AST.ObjectLiteral)astNode, env);
                    break;

                case AST.NodeType.ArrayLiteral:
                    eval_value = Expressions.eval_array_expr((AST.ArrayLiteral)astNode, env);
                    break;

                case AST.NodeType.CallExper:
                    eval_value = Expressions.eval_call_expr((AST.CallExpr)astNode, env);
                    break;

                case AST.NodeType.BinaryExpr:
                    eval_value = Expressions.eval_binary_expression((AST.BinaryExpr)astNode, env);
                    break;

                case AST.NodeType.Program:
                    eval_value = Statements.eval_program((AST.Program)astNode, env);
                    break;

                case AST.NodeType.VarDeclaration:
                    eval_value = Statements.eval_var_declaration((AST.VarDeclaration)astNode, env);
                    break;

                case AST.NodeType.AssignmentExpr:
                    eval_value = Expressions.eval_assignment((AST.AssignmentExpr)astNode, env);
                    break;

                case AST.NodeType.MemberExpr:
                    eval_value = Expressions.eval_member_expr((AST.MemberExpr)astNode,env);
                    break;

                case AST.NodeType.IndexExpr:
                    eval_value = Expressions.eval_index_expr((AST.ArrayIndexExpr)astNode, env);
                    break;

                case AST.NodeType.FunctionDeclaration:
                    eval_value = Statements.eval_function_declaration((AST.FunctionDeclaration)astNode, env);
                    break;

                case AST.NodeType.Return:
                    eval_value = Statements.eval_return((AST.ReturnStatement)astNode, env);
                    break;

                case AST.NodeType.IfStatement:
                    eval_value = Statements.eval_if_stmt((AST.IfStatement)astNode, env);
                    break;

                case AST.NodeType.IfElseStatement:
                    eval_value = Statements.eval_if_else_stmt((AST.IfElseStatement)astNode, env);
                    break;

                case AST.NodeType.WhileStatement:
                    eval_value = Statements.eval_while_stmt((AST.WhileStatement)astNode, env);
                    break;

                case AST.NodeType.DoWhileStatement:
                    eval_value = Statements.eval_do_while_stmt((AST.DoWhileStatement)astNode, env);
                    break;

                case AST.NodeType.ForStatement:
                    eval_value = Statements.eval_for_stmt((AST.ForStatement)astNode, env);
                    break;

                case AST.NodeType.SetMutDefault:
                    eval_value = MetaStatements.eval_meta_mutable_default(((AST.MutDefaultStatement)astNode).is_immutable, env);
                    break;

                case AST.NodeType.SetSilly:
                    eval_value = MetaStatements.eval_meta_silly_default(((AST.SillyDefaultStatement)astNode).val, env);
                    break;

                case AST.NodeType.Include:
                    AST.IncludeStatement includeStatement = (AST.IncludeStatement)astNode;
                    eval_value = MetaStatements.eval_meta_include(includeStatement.lib, includeStatement.alias, env);
                    break;

                default:
                    eval_value = new Values.ErrorVal("Error: This AST Node has not been set up for interpretation: " + astNode);
                    break;
            }

            if (eval_value == null)
            {
                throw new Exception("Interpreter value error, no value returned");
            }
            // Handle Interpretation Errors
            else if (eval_value.type == Values.ValueType.Error)
            {
                Console.Error.WriteLine("Runtime Error:");
                Console.Error.WriteLine(((Values.ErrorVal)eval_value).errMsg);
                System.Environment.Exit(0);
                return new Values.NullVal();
            }
            else
            {
                return eval_value;
            }
        }

        
    }
}
