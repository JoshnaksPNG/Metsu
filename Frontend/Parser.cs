using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace NewLangInterpreter.Frontend
{
    internal class Parser
    {
        private List<Token> tokens;

        private Token advance()
        {
            Token token = tokens[0];
            tokens.RemoveAt(0);
            return token;
        }

        private Token advance(Token.TokenType type, string failure_msg)
        {
            Token token = tokens[0];
            tokens.RemoveAt(0);

            if (token == null || token.type != type)
            {
                Console.Error.WriteLine("Parser Error: " + failure_msg + "\nRecieved: " + token.type + "\nExpecting: " + type);
                System.Environment.Exit(0);
            }

            return token;
        }

        int parse_int(string raw)
        {
            int result = 0;

            int int_base = 0;

            // Determine Base
            if (raw[0] == '0')
            {
                if (raw.Length > 1)
                {
                    switch (raw[1])
                    {
                        case 'b':
                            int_base = 2;
                            break;

                        case 'x':
                            int_base = 16;
                            break;
                    }
                }
                else
                {
                    return 0;
                }
                
            }
            else // Base 10
            {
                int_base = 10;
            }

            int j = 0;
            for (int i = raw.Length - 1; i >= 0; --i)
            {
                if (raw[i] != '_')
                {
                    result += digitval(raw[i]) * (int)Math.Pow(int_base, j);

                    ++j;
                }
                
            }

            return result;
        }

        float parse_float(string raw) 
        {
            float result = 0;
            string[] split_raw = raw.Split('.');
            string whole = split_raw[0];
            string frac = split_raw[1];

            
            int frac_length = frac.Length;
            int frag_basis = parse_int(frac);

            result += parse_int(whole);

            result += (float) Math.Pow(10, -1d * frac_length) * frag_basis;


            return result;
        }

        int digitval(char digit)
        {
            int digitval = 0;

            string dig = "" + digit;

            dig = dig.ToLower();

            switch (dig[0])
            {
                case '0':
                    digitval = 0;
                    break;
                case '1':
                    digitval = 1;
                    break;
                case '2':
                    digitval = 2;
                    break;
                case '3':
                    digitval = 3;
                    break;
                case '4':
                    digitval = 4;
                    break;
                case '5':
                    digitval = 5;
                    break;
                case '6':
                    digitval = 6;
                    break;
                case '7':
                    digitval = 7;
                    break;
                case '8':
                    digitval = 8;
                    break;
                case '9':
                    digitval = 9;
                    break;
                case 'a':
                    digitval = 10;
                    break;
                case 'b':
                    digitval = 11;
                    break;
                case 'c':
                    digitval = 12;
                    break;
                case 'd':
                    digitval = 13;
                    break;
                case 'e':
                    digitval = 14;
                    break;
                case 'f':
                    digitval = 15;
                    break;

                default:
                    break;
            }

            return digitval;
        }

        public AST.Program produceAST(string source)
        {
            Lexer lexer = new Lexer();
            tokens = lexer.tokenize(source);

            AST.Program program = new AST.Program();

            // Parse until at end of file
            while (tokens[0].type != Token.TokenType.EOF)
            {
                program.body.Add(parse_stmt());
            }

            return program;
        }

        AST.Statement parse_stmt()
        {
            switch (tokens[0].type)
            {
                case Token.TokenType.DataType:
                    if (tokens[1].type == Token.TokenType.Identifier)
                    {
                        if (tokens[2].type == Token.TokenType.Assign)
                        {
                            return parse_var_declaration_default();
                        }
                        else if (tokens[2].type == Token.TokenType.OpenParen)
                        {
                            return parse_func_declaration(true);
                        }
                        else
                        {
                            throw new Exception("Illegal token after identifier!");
                        }
                    } 
                    else
                    {
                        throw new Exception("Expected Identifier after datatype");
                    }

                case Token.TokenType.Mut:
                case Token.TokenType.Const:
                    return this.parse_var_declaration();

                case Token.TokenType.Function:
                    return this.parse_func_declaration(false);

                case Token.TokenType.Return:
                    return this.parse_return_stmt();

                case Token.TokenType.If:

                default:
                    return this.parse_expr();
            }
        }

        private AST.ReturnStatement parse_return_stmt()
        {
            advance(); // Advance past return keyword

            AST.Expression ret_val = parse_expr();

            advance(Token.TokenType.SemiColon, "Expected Semi-Colon After Return Statement");

            return new AST.ReturnStatement("", ret_val);
        }

        private AST.Statement parse_func_declaration(bool is_default)
        {
            if(!is_default) 
            {
                advance(); // Advance Past Function Keyword
            }
            
            Token return_type_token = this.advance(Token.TokenType.DataType, "Expected variable datatype following mut or const!");

            string identifier = this.advance(Token.TokenType.Identifier, "Expected identifier following datatype!").value;

            List<KeyValuePair<AST.Expression, string>> args = parse_args_typed();

            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();

            foreach (KeyValuePair<AST.Expression, string> e in args) 
            {
                if (e.Key.kind != AST.NodeType.Identifier)
                {
                    throw new Exception("Expected identifiers inside function definition");
                }

                KeyValuePair<string, string> param = new KeyValuePair<string, string>( ((AST.Identifier)e.Key).symbol, e.Value );

                parameters.Add(param);
            }

            advance(Token.TokenType.OpenCurleyBracket, "Expected function body in function declaration");

            List<AST.Statement> body = new List<AST.Statement>();

            while (tokens[0].type != Token.TokenType.EOF && tokens[0].type != Token.TokenType.CloseCurleyBracket) 
            {
                body.Add(parse_stmt());
            }

            advance(Token.TokenType.CloseCurleyBracket, "Expected closing '}' at end of function body");

            AST.FunctionDeclaration function = new AST.FunctionDeclaration(identifier, body, parameters);

            return function;
        }

        // (MUT) TYPE IDENTIFIER;
        // (CONST / MUT) TYPE IDENTIFIER = Expression;
        private AST.Statement parse_var_declaration()
        {
            bool isConstant = this.advance().type == Token.TokenType.Const;

            Token type_token = this.advance(Token.TokenType.DataType, "Expected variable datatype following mut or const!");

            AST.DataType dataType = AST.type_from_string(type_token.value);

            string identifier = this.advance(Token.TokenType.Identifier, "Expected identifier following datatype!").value;

            // Check if Declaration
            if (tokens[0].type == Token.TokenType.SemiColon)
            {
                this.advance();
                if (isConstant)
                {
                    Console.Error.WriteLine("Parser Error: Must define value for Constant Variable declaration!");
                    System.Environment.Exit(0);
                }

                return new AST.VarDeclaration(identifier, isConstant, dataType);
            }

            this.advance(Token.TokenType.Assign, "Expected assignment operator following identifier in variable declaration");

            AST.Statement declaration = new AST.VarDeclaration(identifier, isConstant, parse_expr(), dataType);

            this.advance(Token.TokenType.SemiColon, "Expected Semicolon ';' after variable declaration statement");

            return declaration;
        }

        private AST.Statement parse_var_declaration_default()
        {
            Token type_token = this.advance(Token.TokenType.DataType, "Expected variable datatype!");

            AST.DataType dataType = AST.type_from_string(type_token.value);

            string identifier = this.advance(Token.TokenType.Identifier, "Expected identifier following datatype!").value;

            // Check if Declaration
            if (tokens[0].type == Token.TokenType.SemiColon)
            {
                this.advance();

                return new AST.VarDeclaration(identifier, dataType);
            }

            this.advance(Token.TokenType.Assign, "Expected assignment operator following identifier in variable declaration");

            AST.Statement declaration = new AST.VarDeclaration(identifier, parse_expr(), dataType);

            this.advance(Token.TokenType.SemiColon, "Expected Semicolon ';' after variable declaration statement");

            return declaration;
        }

        AST.Expression parse_expr()
        {
            return parse_assignment_expr();
        }

        AST.Expression parse_assignment_expr()
        {
            AST.Expression left = this.parse_object_expr();

            if (tokens[0].type == Token.TokenType.Assign)
            {
                this.advance();

                AST.Expression value = this.parse_assignment_expr();

                this.advance(Token.TokenType.SemiColon, "Expected Semicolon ';' after variable assignment statement");

                return new AST.AssignmentExpr(left, value);
            }

            return left;
        }

        //{ Properties[] }
        AST.Expression parse_object_expr()
        {
            if (tokens[0].type != Token.TokenType.OpenCurleyBracket)
            {
                return this.parse_additive_expr();
            }

            advance();

            List<AST.Property> properties = new List<AST.Property>();

            while (tokens[0].type != Token.TokenType.EOF && tokens[0].type != Token.TokenType.CloseCurleyBracket)
            {
                string key = advance(Token.TokenType.Identifier, "Object Literal key expected").value;

                advance(Token.TokenType.Colon, "missing Colon following identifier in Object Literal");

                AST.Expression value = parse_expr();

                properties.Add(new AST.Property(key, value));
                if (tokens[0].type != Token.TokenType.CloseCurleyBracket)
                {
                    advance(Token.TokenType.Comma, "Expected Comma or closing bracket following property");
                }

                
            }

            advance(Token.TokenType.CloseCurleyBracket, "Object literal missing closing brace");

            return new AST.ObjectLiteral(properties);
        }

        // Order of Presciedence
        //
        // AssignmentExpr
        // Object
        // AdditiveExpr
        // MultiplicativeExpr
        // Call
        // Member
        // Primary

        AST.Expression parse_additive_expr()
        {
            // Implement Left Presciedence
            AST.Expression left_expr = parse_multiplicative_expr();

            while (tokens[0].value == "+" || tokens[0].value == "-")
            {
                string oper = advance().value;

                AST.Expression right_expr = parse_multiplicative_expr();

                left_expr = new AST.BinaryExpr(left_expr, right_expr, AST.operator_from_string(oper));
            }

            return left_expr;
        }

        AST.Expression parse_multiplicative_expr()
        {
            // Implement Left Presciedence
            AST.Expression left_expr = parse_call_member_expr();

            while (tokens[0].value == "*" || tokens[0].value == "/" || tokens[0].value == "%")
            {
                string oper = advance().value;

                AST.Expression right_expr = parse_call_member_expr();

                left_expr = new AST.BinaryExpr(left_expr, right_expr, AST.operator_from_string(oper));
            }

            return left_expr;
        }

        AST.Expression parse_call_member_expr()
        {
            AST.Expression member = parse_member_expr();

            if (tokens[0].type == Token.TokenType.OpenParen)
            {
                return parse_call_expr(member);
            }

            return member;
        }

        AST.Expression parse_call_expr(AST.Expression callee)
        {
            AST.Expression call_expr = new AST.CallExpr(callee, parse_args());

            if (tokens[0].type == Token.TokenType.OpenParen)
            {
                call_expr = parse_call_expr(call_expr);
            }

            return call_expr;
        }

        List<AST.Expression> parse_args()
        {
            advance(Token.TokenType.OpenParen, "Expected Open Paren");

            List<AST.Expression> args = tokens[0].type == Token.TokenType.CloseParen ? new List<AST.Expression>() : parse_args_list();

            advance(Token.TokenType.CloseParen, "Missing Closing Paren in arguments list");

            return args;
        }

        List<KeyValuePair<AST.Expression, string>> parse_args_typed()
        {
            advance(Token.TokenType.OpenParen, "Expected Open Paren");

            List<KeyValuePair<AST.Expression, string>> args = tokens[0].type == Token.TokenType.CloseParen ? new List<KeyValuePair<AST.Expression, string>>() : parse_args_list_typed();

            advance(Token.TokenType.CloseParen, "Missing Closing Paren in arguments list");

            return args;
        }

        List<AST.Expression> parse_args_list()
        {
            List<AST.Expression> args = new List<AST.Expression>() { parse_expr() };

            while (tokens[0].type == Token.TokenType.Comma && advance() != null) 
            {
                args.Add(parse_assignment_expr());
            }

            return args;
        }

        List<KeyValuePair<AST.Expression, string>> parse_args_list_typed()
        {
            Token type_token = advance(Token.TokenType.DataType, "Expected Data Type for Parameter");
            string arg_type = type_token.value;

            List<KeyValuePair<AST.Expression, string>> args = new List<KeyValuePair<AST.Expression, string>>() { new KeyValuePair<AST.Expression, string>( parse_expr(), arg_type) };

            while (tokens[0].type == Token.TokenType.Comma && advance() != null)
            {
                type_token = advance(Token.TokenType.DataType, "Expected Data Type for Parameter");
                arg_type = type_token.value;
                KeyValuePair<AST.Expression, string> arg = new KeyValuePair<AST.Expression, string>(parse_expr(), arg_type);

                args.Add(arg);
            }

            return args;
        }

        AST.Expression parse_member_expr()
        {
            AST.Expression obj = parse_primary_expr();

            while (tokens[0].type == Token.TokenType.Period || tokens[0].type == Token.TokenType.OpenSquareBracket) 
            {
                Token _operator = advance();
                AST.Expression property;
                bool is_computed;

                // Non-computed
                if (_operator.type == Token.TokenType.Period)
                {
                    is_computed = false;
                    property = parse_primary_expr();

                    if (property.kind != AST.NodeType.Identifier)
                    {
                        Console.Error.WriteLine("Expected Identifier for dot operator");
                        System.Environment.Exit(0);
                    }
                }
                else // Allows obj[computedValue]
                {
                    is_computed = true;
                    property = parse_expr();

                    advance(Token.TokenType.CloseSquareBracket, "Missing closing bracket in computed value");
                }

                obj = new AST.MemberExpr(obj, property, is_computed);
            }

            return obj;
        }

        AST.Expression parse_primary_expr()
        {
            Token.TokenType tkn = tokens[0].type;

            switch (tkn)
            {

                case Token.TokenType.Null:
                    advance();
                    return new AST.NullLiteral();
                
                case Token.TokenType.Identifier:
                    return new AST.Identifier(advance().value);

                case Token.TokenType.Integer:
                    return new AST.IntLiteral(parse_int(advance().value));

                case Token.TokenType.Float:
                    return new AST.FloatLiteral(parse_float(advance().value));

                case Token.TokenType.Char:
                    return new AST.CharLiteral((advance().value)[0]);

                case Token.TokenType.String:
                    return new AST.StringLiteral(advance().value);

                case Token.TokenType.OpenParen:
                    advance(); // Eat Open Paren
                    AST.Expression value = parse_expr();
                    advance(Token.TokenType.CloseParen, "Missing Closed Parenthasis in Parenthasized Expression!"); // Eat Closing Paren
                    return value;

                default:
                    Console.Error.WriteLine("Unexpected Token found in Parsing: " + tkn.ToString());
                    Environment.Exit(0);
                    return new AST.IntLiteral(0);

            }
        }
    }
}
