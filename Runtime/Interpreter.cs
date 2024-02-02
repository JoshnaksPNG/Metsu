using NewLangInterpreter.Frontend;
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
            switch(astNode.kind) 
            {
                case AST.NodeType.IntLiteral:
                    return new Values.IntVal(((AST.IntLiteral)astNode).value);

                case AST.NodeType.FloatLiteral:
                    return new Values.FloatVal(((AST.FloatLiteral)astNode).value);

                case AST.NodeType.CharLiteral:
                    return new Values.CharVal(((AST.CharLiteral)astNode).value);

                case AST.NodeType.StringLiteral:
                    return new Values.StringVal(((AST.StringLiteral)astNode).value);

                case AST.NodeType.NullLiteral:
                    return new Values.NullVal();

                case AST.NodeType.BoolLiteral:
                    return new Values.BoolVal(((AST.BoolLiteral)astNode).value);

                case AST.NodeType.Identifier:
                    return Expressions.eval_identifier((AST.Identifier)astNode, env);

                case AST.NodeType.ObjectLiteral:
                    return Expressions.eval_object_expr((AST.ObjectLiteral)astNode, env);

                case AST.NodeType.CallExper:
                    return Expressions.eval_call_expr((AST.CallExpr)astNode, env);

                case AST.NodeType.BinaryExpr:
                    return Expressions.eval_binary_expression((AST.BinaryExpr)astNode, env);

                case AST.NodeType.Program:
                    return Statements.eval_program((AST.Program)astNode, env);

                case AST.NodeType.VarDeclaration:
                    return Statements.eval_var_declaration((AST.VarDeclaration)astNode, env);

                case AST.NodeType.AssignmentExpr:
                    return Expressions.eval_assignment((AST.AssignmentExpr)astNode, env);

                //case AST.NodeType.MemberExpr:
                //return Expressions.e((AST.MemberExpr)astNode,env);
                case AST.NodeType.FunctionDeclaration:
                    return Statements.eval_function_declaration((AST.FunctionDeclaration)astNode, env);

                case AST.NodeType.Return:
                    return Statements.eval_return((AST.ReturnStatement)astNode, env);

                case AST.NodeType.IfStatement:
                    return Statements.eval_if_stmt((AST.IfStatement)astNode, env);

                case AST.NodeType.IfElseStatement:
                    return Statements.eval_if_else_stmt((AST.IfElseStatement)astNode, env);

                case AST.NodeType.WhileStatement:
                    return Statements.eval_while_stmt((AST.WhileStatement)astNode, env);

                case AST.NodeType.DoWhileStatement:
                    return Statements.eval_do_while_stmt((AST.DoWhileStatement)astNode, env);

                case AST.NodeType.ForStatement:
                    return Statements.eval_for_stmt((AST.ForStatement)astNode, env);

                default:
                    Console.Error.WriteLine("Error: This AST Node has not been set up for interpretation: " + astNode);
                    System.Environment.Exit(0);
                    return new Values.NullVal();
            }
        }

        
    }
}
