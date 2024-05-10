using NewLangInterpreter.src.Frontend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewLangInterpreter.src.Compiler
{
    internal class CIL_Emitter
    {
        public List<TreeParser.CIL_Instruction> instructions;

        public static TreeParser.CIL_Instruction get_node_instruction(AST.Statement node) 
        {
            switch (node.kind)
            {
                case AST.NodeType.IntLiteral:
                    return new TreeParser.Int_Literal(((AST.IntLiteral) node).value);

                case AST.NodeType.CharLiteral:
                    return new TreeParser.Char_Literal(((AST.CharLiteral)node).value);

                case AST.NodeType.FloatLiteral:
                    return new TreeParser.Float_Literal(((AST.FloatLiteral)node).value);

                case AST.NodeType.BoolLiteral:
                    return new TreeParser.Bool_Literal(((AST.BoolLiteral)node).value);

                case AST.NodeType.StringLiteral:
                    return new TreeParser.String_Literal(((AST.StringLiteral)node).value);

                case AST.NodeType.NullLiteral:
                    break;

                case AST.NodeType.Identifier:
                    //return new TreeParser.Identifier(((AST.Identifier)node).symbol);
                    break;

                case AST.NodeType.ObjectLiteral: 
                    
                    break;

                case AST.NodeType.ArrayLiteral:
                    return new TreeParser.Array_Literal(((AST.ArrayLiteral)node).elements);

                case AST.NodeType.CallExper:
                    return new TreeParser.CallExpr(((AST.CallExpr)node));

                case AST.NodeType.BinaryExpr: break;

                case AST.NodeType.Program:
                    return new TreeParser.Program(((AST.Program)node).body);

                case AST.NodeType.VarDeclaration: break;

                case AST.NodeType.AssignmentExpr: break;

                case AST.NodeType.MemberExpr: break;

                case AST.NodeType.IndexExpr: break;

                case AST.NodeType.FunctionDeclaration: 

                    break;

                case AST.NodeType.Return:
                    break;

                case AST.NodeType.IfStatement: break;

                case AST.NodeType.IfElseStatement: break;

                case AST.NodeType.WhileStatement: break;

                case AST.NodeType.DoWhileStatement: break;

                case AST.NodeType.ForStatement: break;

                case AST.NodeType.SetMutDefault: break;

                case AST.NodeType.SetSilly: break;

            }

            throw new NotImplementedException();
        }

        public CIL_Emitter()
        {
            instructions = new List<TreeParser.CIL_Instruction>();


        }
    }
}
