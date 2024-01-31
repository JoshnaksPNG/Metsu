using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static NewLangInterpreter.Frontend.AST;

namespace NewLangInterpreter.Frontend
{
    internal class AST
    {
        public enum NodeType
        {
            // Statements
            Program,
            VarDeclaration,
            FunctionDeclaration,
            Return,
            IfStatement,

            // Literals
            IntLiteral,
            NullLiteral,
            Property,
            ObjectLiteral,
            StringLiteral,
            CharLiteral,
            FloatLiteral,
            BoolLiteral,

            // Expressions
            Identifier,
            BinaryExpr,
            UnaryExpr,
            AssignmentExpr,
            MemberExpr,
            CallExper
        }

        public enum DataType
        {
            String,
            Char,
            Float,
            Int,
            Bool,

            Function,
            Object,

            Null,
        }

        public enum Operator
        {
            Add,
            Subtract,
            Multiply,
            Divide,
            Modulo,

            AddAssignment,
            SubtractAssignment,
            MultiplyAssignment,
            DivideAssignment,
            ModuloAssignment,

            Increment,
            Decrement,
            Exponentiate,

            LogicalAnd,
            LogicalOr,

            LogicalNot,

            Unknown,
        }

        public class Statement
        {
            public NodeType kind;
        }

        public class Expression : Statement 
        {
            public DataType dataType;
        }

        public class Program : Statement
        {
            public List<Statement> body;

            public Program()
            {
                kind = NodeType.Program;
                body = new List<Statement>();
            }

            public override string ToString()
            {
                string returned = "{ kind: ";

                returned += kind.ToString();

                returned += ", body: ";

                returned += body.ToString() + " }";

                return returned;
            }
        }

        public class VarDeclaration : Statement
        {
            public bool isConstant;

            public string identifier;

            public DataType dataType;

            public Expression? value;

            public bool is_default;

            public VarDeclaration(string ident, DataType dataType)
            {
                kind = NodeType.VarDeclaration;
                isConstant = false;
                is_default = true;
                identifier = ident;
                this.dataType = dataType;
            }

            public VarDeclaration(string ident, bool isConst, DataType dataType)
            {
                kind = NodeType.VarDeclaration;
                isConstant = isConst;
                identifier = ident;
                this.dataType = dataType;
                is_default = false;
            }

            public VarDeclaration(string ident, bool isConst, Expression val, DataType dataType)
            {
                kind = NodeType.VarDeclaration;
                isConstant = isConst;
                identifier = ident;
                value = val;
                this.dataType = dataType;
                is_default = false;
            }

            public VarDeclaration(string ident, Expression val, DataType dataType)
            {
                kind = NodeType.VarDeclaration;
                isConstant = false;
                identifier = ident;
                value = val;
                this.dataType = dataType;
                is_default = true;
            }

            public override string ToString()
            {
                string returned = "{ kind: ";

                returned += kind.ToString();

                returned += ", identifier: ";

                returned += identifier.ToString();

                returned += ", value: ";

                returned += value.ToString();

                returned += ", isConstant: ";

                returned += isConstant.ToString() + " }";

                return returned;
            }
        }

        public class ReturnStatement : Statement
        {
            //public string type;

            public Expression value;

            public ReturnStatement(string type, Expression value) 
            {
                //this.type = type;
                this.kind = NodeType.Return;
                this.value = value;
            }
        }

        public class FunctionDeclaration : Statement
        {
            // {identifier, datatype}
            public List<KeyValuePair<string, string>> parameters;

            public string name;

            public List<Statement> body;

            ///public Datatype Here

            public Expression? value;

            public FunctionDeclaration(string ident, List<Statement> body, List<KeyValuePair<string, string>> args)
            {
                kind = NodeType.FunctionDeclaration;
                name = ident;
                this.body = body;
                this.parameters = args;
            }

            /*public override string ToString()
            {
                string returned = "{ kind: ";

                returned += kind.ToString();

                returned += ", name: ";

                returned += name.ToString();

                returned += ", value: ";

                returned += value.ToString();

                returned += ", isConstant: ";

                returned += isConstant.ToString() + " }";

                return returned;
            }*/
        }

        public class IfStatement : Statement
        {
            public List<Statement> body;
            public Expression condition;

            public IfStatement(List<Statement> body, Expression condition)
            {
                kind = NodeType.IfStatement;
                this.body = body;
                this.condition = condition;
            }

            public override string ToString()
            {
                string returned = "{ kind: ";

                returned += kind.ToString();

                returned += ", body: ";

                returned += body.ToString() + " }";

                return returned;
            }
        }

        public class AssignmentExpr : Expression 
        {
            public Expression assignee;

            public Expression value;

            public AssignmentExpr(Expression assignee, Expression value)
            {
                kind = NodeType.AssignmentExpr;
                this.assignee = assignee;
                this.value = value;
            }

            public override string ToString()
            {
                string returned = "{ kind: ";

                returned += kind.ToString();
                returned += ", identifier: ";

                returned += assignee.ToString();

                returned += ", value: ";

                returned += value.ToString() + " }";

                return returned;
            }
        }

        public class BinaryExpr : Expression
        {
            public BinaryExpr(Expression left, Expression right, Operator opr)
            {
                this.left = left;
                this.right = right;
                this.opr = opr;

                kind = NodeType.BinaryExpr;
            }

            public Expression left;
            public Expression right;
            public Operator opr;

            public override string ToString()
            {
                string returned = "{ kind: ";

                returned += kind.ToString();

                returned += ", left: ";

                returned += left.ToString();

                returned += ", right: ";

                returned += right.ToString();

                returned += ", opr: ";

                returned += opr.ToString() + " }";

                return returned;
            }
        }

        public class CallExpr : Expression
        {
            public CallExpr(Expression callee)
            {
                this.args = new List<Expression>();
                this.callee = callee;

                kind = NodeType.CallExper;
            }

            public CallExpr(Expression callee, List<Expression> arg_list)
            {
                this.args = arg_list;
                this.callee = callee;

                kind = NodeType.CallExper;
            }

            public List<Expression> args;
            public Expression callee;

            public override string ToString()
            {
                string returned = "{ kind: ";

                returned += kind.ToString();

                returned += ", callee: ";

                returned += callee.ToString();

                returned += ", args: ";

                returned += args.ToString() + " }";

                return returned;
            }
        }

        public class MemberExpr : Expression
        {
            public MemberExpr(Expression obj, Expression property, bool comp)
            {
                this.obj = obj;
                this.property = property;
                this.is_computed = comp;

                kind = NodeType.MemberExpr;
            }

            public Expression obj;
            public Expression property;
            public bool is_computed;

            public override string ToString()
            {
                string returned = "{ kind: ";

                returned += kind.ToString();

                returned += ", obj: ";

                returned += obj.ToString();

                returned += ", property: ";

                returned += property.ToString();

                returned += ", is_computed: ";

                returned += is_computed.ToString() + " }";

                return returned;
            }
        }

        public class UnaryExpr : Expression
        {
            UnaryExpr(Expression operand, Operator opr)
            {
                this.operand = operand;
                this.opr = opr;

                kind = NodeType.UnaryExpr;
            }

            Expression operand;
            Operator opr;

            public override string ToString()
            {
                string returned = "{ kind: ";

                returned += kind.ToString();

                returned += ", operand: ";

                returned += operand.ToString();

                returned += ", opr: ";

                returned += opr.ToString() + " }";

                return returned;
            }
        }

        public class Identifier : Expression
        {
            public Identifier(string symbol)
            {
                this.symbol = symbol;

                kind = NodeType.Identifier;
            }

            public string symbol;

            public override string ToString()
            {
                string returned = "{ kind: ";

                returned += kind.ToString();

                returned += ", symbol: ";

                returned += symbol.ToString() + " }";

                return returned;
            }
        }

        public class IntLiteral : Expression
        {
            public IntLiteral(int value)
            {
                kind = NodeType.IntLiteral;
                this.value = value;
            }

            public int value;

            public override string ToString()
            {
                string returned = "{ kind: ";

                returned += kind.ToString();

                returned += ", value: ";

                returned += value.ToString() + " }";

                return returned;
            }
        }

        public class FloatLiteral : Expression
        {
            public FloatLiteral(float value)
            {
                kind = NodeType.FloatLiteral;
                this.value = value;
            }

            public float value;

            public override string ToString()
            {
                string returned = "{ kind: ";

                returned += kind.ToString();

                returned += ", value: ";

                returned += value.ToString() + " }";

                return returned;
            }
        }

        public class StringLiteral : Expression
        {
            public StringLiteral(string value)
            {
                kind = NodeType.StringLiteral;
                this.value = value;
            }

            public string value;

            public override string ToString()
            {
                string returned = "{ kind: ";

                returned += kind.ToString();

                returned += ", value: ";

                returned += value.ToString() + " }";

                return returned;
            }
        }

        public class CharLiteral : Expression
        {
            public CharLiteral(char value)
            {
                kind = NodeType.CharLiteral;
                this.value = value;
            }

            public char value;

            public override string ToString()
            {
                string returned = "{ kind: ";

                returned += kind.ToString();

                returned += ", value: ";

                returned += value.ToString() + " }";

                return returned;
            }
        }

        public class BoolLiteral : Expression
        {
            public BoolLiteral(bool value)
            {
                kind = NodeType.BoolLiteral;
                this.value = value;
            }

            public bool value;

            public override string ToString()
            {
                string returned = "{ kind: ";

                returned += kind.ToString();

                returned += ", value: ";

                returned += value.ToString() + " }";

                return returned;
            }
        }

        public class ObjectLiteral : Expression
        {
            public ObjectLiteral()
            {
                kind = NodeType.ObjectLiteral;
                this.properties = new List<Property>();
            }

            public ObjectLiteral(List<Property> properties)
            {
                kind = NodeType.ObjectLiteral;
                this.properties = properties;
            }

            public List<Property> properties;

            public override string ToString()
            {
                string returned = "{ kind: ";

                returned += kind.ToString();

                returned += ", properties: [";

                foreach (Property property in properties) 
                {
                    returned += property.ToString() + ", ";
                }

                returned += " ]}";

                return returned;
            }
        }

        public class Property : Expression
        {
            public Property(string key, Expression value)
            {
                kind = NodeType.Property;
                this.key = key;
                this.value = value;
            }

            public string key;
            public Expression? value;   

            public override string ToString()
            {
                string returned = "{ kind: ";

                returned += kind.ToString();

                returned += ", key: ";

                returned += key.ToString();

                returned += ", value: ";

                returned += value.ToString() + " }";

                return returned;
            }
        }

        public class NullLiteral : Expression
        {
            public NullLiteral()
            {
                kind = NodeType.NullLiteral;
                this.value = "null";
            }

            public string value;

            public override string ToString()
            {
                string returned = "{ kind: ";

                returned += kind.ToString();

                returned += ", value: ";

                returned += value.ToString() + " }";

                return returned;
            }
        }

        public static Operator operator_from_string(string val)
        {
            switch (val) 
            {
                case "+":
                    return Operator.Add;

                case "-":
                    return Operator.Subtract;

                case "*":
                    return Operator.Multiply;

                case "/":
                    return Operator.Divide;

                case "%": 
                    return Operator.Modulo;

                case "&&":
                    return Operator.LogicalAnd;

                case "||":
                    return Operator.LogicalOr;

                case "!":
                    return Operator.LogicalNot;

                default:
                    Console.Error.WriteLine("Error! Unknown Operator!" + val);
                    System.Environment.Exit(0);
                    return Operator.Unknown;
            }
        }

        public static DataType type_from_string(string val)
        {
            switch (val) 
            {
                case "int":
                    return DataType.Int;

                case "char":
                    return DataType.Char;

                case "float":
                    return DataType.Float;

                case "bool":
                    return DataType.Bool;

                case "string":
                    return DataType.String;

                case "obj":
                    return DataType.Object;

                case "func":
                    return DataType.Function;

                default:
                    throw new Exception("Unrecognized Datatype!");
            }
        }
    }
}
