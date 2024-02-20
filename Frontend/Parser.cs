using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NewLangInterpreter.Frontend
{
    internal class Parser
    {
        private List<Token> tokens = new List<Token>();

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

            if (token == null)
            {
                Console.Error.WriteLine("Parser Error: " + failure_msg + "\nToken is null");
                System.Environment.Exit(0);
            }
            if (token.type != type)
            {
                Console.Error.WriteLine("Parser Error: " + failure_msg + "\nRecieved: " + token.type + "\nExpecting: " + type);
                System.Environment.Exit(0);
            }

            return token;
        }

        private Token advance(Token.TokenType[] types, string failure_msg)
        {
            Token token = tokens[0];
            tokens.RemoveAt(0);

            if (token == null)
            {
                Console.Error.WriteLine("Parser Error: " + failure_msg + "\nToken is null");
                System.Environment.Exit(0);
            }
            if (!types.Contains(token.type))
            {
                Console.Error.WriteLine("Parser Error: " + failure_msg + "\nRecieved: " + token.type + "\nExpecting: " + types.ToString());
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
                            raw = raw.Remove(1,1);
                            int_base = 2;
                            break;

                        case 'q':
                            raw = raw.Remove(1, 1);
                            int_base = 4;
                            break;

                        case 'o':
                            raw = raw.Remove(1, 1);
                            int_base = 8;
                            break;

                        case 'x':
                            raw = raw.Remove(1, 1);
                            int_base = 16;
                            break;

                        case 'v':
                            raw = raw.Remove(1, 1);
                            int_base = 32;
                            break;

                        case 'z':
                            raw = raw.Remove(1, 1);
                            int_base = 36;
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
                    if (digitval(raw[i]) > int_base - 1)
                    {
                        throw new Exception("Cannot parse digit with larger value than integer base");
                    }

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

        bool parse_bool(string raw) 
        {
            return raw == "true";
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

                // B16 HEX
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

                // B36
                case 'g':
                    digitval = 16;
                    break;
                case 'h':
                    digitval = 17;
                    break;
                case 'i':
                    digitval = 18;
                    break;
                case 'j':
                    digitval = 19;
                    break;
                case 'k':
                    digitval = 20;
                    break;
                case 'l':
                    digitval = 21;
                    break;
                case 'm':
                    digitval = 22;
                    break;
                case 'n':
                    digitval = 23;
                    break;
                case 'o':
                    digitval = 24;
                    break;
                case 'p':
                    digitval = 25;
                    break;
                case 'q':
                    digitval = 26;
                    break;
                case 'r':
                    digitval = 27;
                    break;
                case 's':
                    digitval = 28;
                    break;
                case 't':
                    digitval = 29;
                    break;
                case 'u':
                    digitval = 30;
                    break;
                case 'v':
                    digitval = 31;
                    break;
                case 'w':
                    digitval = 32;
                    break;
                case 'x':
                    digitval = 33;
                    break;
                case 'y':
                    digitval = 34;
                    break;
                case 'z':
                    digitval = 35;
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
                            return parse_func_declaration(true, false);
                        }
                        else
                        {
                            throw new Exception("Illegal token after identifier!");
                        }
                    }
                    else if (tokens[1].type == Token.TokenType.OpenSquareBracket && tokens[2].type == Token.TokenType.CloseSquareBracket)
                    {
                        return parse_array_declaration(false);
                    }
                    else
                    {
                        throw new Exception("Expected Identifier after datatype");
                    }

                case Token.TokenType.Mut:
                case Token.TokenType.Const:
                    return this.parse_var_declaration();

                case Token.TokenType.Function:
                    if (tokens[1].type == Token.TokenType.Identifier)
                    {
                        if (tokens[2].type == Token.TokenType.Assign)
                        {
                            return parse_var_function_declaration();
                        }
                        else
                        {
                            if (tokens[3].type == Token.TokenType.DataType)
                            {
                                return this.parse_func_declaration(false, false);
                            }
                            else
                            {
                                return this.parse_func_declaration(true, true);
                            }
                            
                        }
                    }
                    else if (tokens[1].type == Token.TokenType.DataType)
                    {
                        return this.parse_func_declaration(false, false);
                    }
                    else
                    {
                        throw new Exception("Expected Identifier or Data Type after Func Keyword");
                    }

                case Token.TokenType.Return:
                    return this.parse_return_stmt();

                case Token.TokenType.Procedure:
                    tokens[0] = new Token(Token.TokenType.DataType, "void");
                    return this.parse_func_declaration(true, false);

                case Token.TokenType.If:
                    return this.parse_if_statement();

                case Token.TokenType.While:
                    return this.parse_while_statement();

                case Token.TokenType.Do:
                    return this.parse_do_while_statement();

                case Token.TokenType.For: 
                    return this.parse_for_statement();

                case Token.TokenType.MetaSilly:
                case Token.TokenType.MetaMutable:
                case Token.TokenType.MetaImmutable:
                    return this.parse_meta_stmt();

                default:
                    AST.Expression expr = this.parse_expr();

                    advance(Token.TokenType.SemiColon, "Expected Semi-colon after expression");

                    return expr;
            }
        }

        private AST.ReturnStatement parse_return_stmt()
        {
            advance(); // Advance past return keyword

            AST.Expression? ret_val =  null;

            if (tokens[0].type != Token.TokenType.SemiColon)
            {
                ret_val = parse_expr();
            }            

            advance(Token.TokenType.SemiColon, "Expected Semi-Colon After Return Statement");

            return new AST.ReturnStatement(ret_val);
        }

        private AST.Statement parse_func_declaration(bool is_default, bool returns_function)
        {
            if(!is_default) 
            {
                advance(); // Advance Past Function Keyword
            }

            Token return_type_token;

            if (returns_function)
            {
                return_type_token = this.advance(Token.TokenType.Function, "Expected variable datatype following function keyword!");
            }
            else
            {
                return_type_token = this.advance(Token.TokenType.DataType, "Expected variable datatype following function keyword!");
            }
            

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

            AST.FunctionDeclaration function = new AST.FunctionDeclaration(identifier, body, parameters, return_type_token.value);

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

        private AST.Statement parse_array_declaration(bool is_default)
        {
            bool isConstant;
            string identifier;
            AST.DataType dataType;
            Token type_token;

            if (is_default)
            {
                isConstant = this.advance().type == Token.TokenType.Const;

                type_token = this.advance(Token.TokenType.DataType, "Expected variable datatype!");

                dataType = AST.type_from_string(type_token.value);

                // Advance past square brackets
                advance(Token.TokenType.OpenSquareBracket, "Expected Opening square bracket.");
                advance(Token.TokenType.CloseSquareBracket, "Expected Closing square bracket");

                identifier = this.advance(Token.TokenType.Identifier, "Expected identifier following datatype!").value;

                // Check if Declaration
                if (tokens[0].type == Token.TokenType.SemiColon)
                {
                    this.advance();
                    if (is_default && isConstant)
                    {
                        Console.Error.WriteLine("Parser Error: Must define value for Constant Variable declaration!");
                        System.Environment.Exit(0);
                    }

                    return new AST.VarDeclaration(identifier, isConstant, AST.DataType.Array);
                }
            }
            else
            {
                type_token = this.advance(Token.TokenType.DataType, "Expected variable datatype!");

                dataType = AST.type_from_string(type_token.value);

                // Advance past square brackets
                advance(Token.TokenType.OpenSquareBracket, "Expected Opening square bracket.");
                advance(Token.TokenType.CloseSquareBracket, "Expected Closing square bracket");

                identifier = this.advance(Token.TokenType.Identifier, "Expected identifier following datatype!").value;
            }

            this.advance(Token.TokenType.Assign, "Expected assignment operator following identifier in variable declaration");

            AST.Statement declaration = new AST.VarDeclaration(identifier, parse_expr(), AST.DataType.Array);

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

        private AST.Statement parse_var_function_declaration()
        {
            Token type_token = this.advance(Token.TokenType.Function, "Expected variable datatype!");

            AST.DataType dataType = AST.DataType.Function;

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

        private AST.Statement parse_if_statement()
        {
            advance(); // Advance Past If Keyword

            AST.Expression condition = parse_expr();

            advance(Token.TokenType.OpenCurleyBracket, "Expected function body in if declaration");

            List<AST.Statement> body = new List<AST.Statement>();

            while (tokens[0].type != Token.TokenType.EOF && tokens[0].type != Token.TokenType.CloseCurleyBracket)
            {
                body.Add(parse_stmt());
            }

            advance(Token.TokenType.CloseCurleyBracket, "Expected closing '}' at end of if body");

            AST.IfStatement if_stmt = new AST.IfStatement(body, condition);

            if (tokens[0].type == Token.TokenType.Else)
            {
                advance(); // Advance Past Else Keyword

                List<AST.Statement> else_body = new List<AST.Statement>();

                if (tokens[0].type == Token.TokenType.If)
                {
                    else_body.Add(parse_if_statement());
                }
                else
                {
                    advance(Token.TokenType.OpenCurleyBracket, "Expected function body in else declaration");

                    while (tokens[0].type != Token.TokenType.EOF && tokens[0].type != Token.TokenType.CloseCurleyBracket)
                    {
                        else_body.Add(parse_stmt());
                    }

                    advance(Token.TokenType.CloseCurleyBracket, "Expected closing '}' at end of else body");

                }

                return new AST.IfElseStatement(if_stmt, else_body);
            }
            else
            {
                return if_stmt;
            }

            
        }

        private AST.Statement parse_while_statement()
        {
            advance(); // Advance Past While Keyword

            AST.Expression condition = parse_expr();

            advance(Token.TokenType.OpenCurleyBracket, "Expected function body in while declaration");

            List<AST.Statement> body = new List<AST.Statement>();

            while (tokens[0].type != Token.TokenType.EOF && tokens[0].type != Token.TokenType.CloseCurleyBracket)
            {
                body.Add(parse_stmt());
            }

            advance(Token.TokenType.CloseCurleyBracket, "Expected closing '}' at end of while body");

            AST.WhileStatement while_stmt = new AST.WhileStatement(body, condition);
            
            return while_stmt;
        }

        private AST.Statement parse_do_while_statement()
        {
            advance(); // Advance Past While Keyword

            advance(Token.TokenType.OpenCurleyBracket, "Expected function body in do while declaration");

            List<AST.Statement> body = new List<AST.Statement>();

            while (tokens[0].type != Token.TokenType.EOF && tokens[0].type != Token.TokenType.CloseCurleyBracket)
            {
                body.Add(parse_stmt());
            }

            advance(Token.TokenType.CloseCurleyBracket, "Expected closing '}' at end of do while body");

            advance(Token.TokenType.While, "Expected While Token at end of Do While Body");

            AST.Expression condition = parse_expr();

            advance(Token.TokenType.SemiColon, "Expeced Semi-colon at end of Do While Statement");

            AST.DoWhileStatement do_while_stmt = new AST.DoWhileStatement(body, condition);

            return do_while_stmt;
        }

        // TODO Later
        private AST.Statement parse_for_statement()
        {
            advance(); // Advance Past For Keyword

            AST.Statement declaration = parse_var_declaration();

            AST.Expression condition = parse_expr();

            advance(Token.TokenType.SemiColon, "Expected semicolon after for condition");

            AST.Expression post_expr = parse_expr();

            advance(Token.TokenType.CloseParen, "Expected closing parenthasis before for body");

            advance(Token.TokenType.OpenCurleyBracket, "Expected function body in while declaration");

            List<AST.Statement> body = new List<AST.Statement>();

            while (tokens[0].type != Token.TokenType.EOF && tokens[0].type != Token.TokenType.CloseCurleyBracket)
            {
                body.Add(parse_stmt());
            }

            advance(Token.TokenType.CloseCurleyBracket, "Expected closing '}' at end of while body");

            AST.ForStatement for_stmt = new AST.ForStatement(body, condition, declaration, post_expr);

            return for_stmt;
        }

        AST.Expression parse_expr()
        {
            AST.Expression expr = parse_assignment_expr();

            return expr;
        }

        AST.Expression parse_assignment_expr()
        {
            AST.Expression left = this.parse_logical_expr();

            if (tokens[0].type == Token.TokenType.Assign)
            {
                this.advance();

                AST.Expression value = this.parse_assignment_expr();

                return new AST.AssignmentExpr(left, value);
            }

            return left;
        }

        AST.Expression parse_logical_expr()
        {
            AST.Expression left_expr = parse_bitwise_expr();

            while (tokens[0].value == "&&" || tokens[0].value == "||")
            {
                string oper = advance().value;

                AST.Expression right_expr = parse_bitwise_expr();

                left_expr = new AST.BinaryExpr(left_expr, right_expr, AST.operator_from_string(oper));
            }

            return left_expr;
        }

        AST.Expression parse_bitwise_expr()
        {
            return this.parse_comparison_expr();
        }

        AST.Expression parse_comparison_expr()
        {
            AST.Expression left_expr = parse_array_expr();

            while (tokens[0].type == Token.TokenType.ComparisonOperator)
            {
                string oper = advance().value;

                AST.Expression right_expr = parse_array_expr();

                left_expr = new AST.BinaryExpr(left_expr, right_expr, AST.operator_from_string(oper));
            }

            return left_expr;
        }

        AST.Expression parse_array_expr() 
        {
            if (tokens[0].type != Token.TokenType.OpenSquareBracket)
            {
                return parse_object_expr();
            }

            // Advance Past Opening Square Bracket
            advance(Token.TokenType.OpenSquareBracket, "Expected open square bracket at begining of array literal");

            List<AST.Expression> elements = new List<AST.Expression>();

            while (tokens[0].type != Token.TokenType.EOF && tokens[0].type != Token.TokenType.CloseSquareBracket)
            {
                AST.Expression value = parse_expr();

                elements.Add(value);

                if (tokens[0].type != Token.TokenType.CloseSquareBracket)
                {
                    advance(Token.TokenType.Comma, "Expected Comma or closing bracket following element");
                }
            }

            advance(Token.TokenType.CloseSquareBracket, "Array literal missing brace");

            return new AST.ArrayLiteral(elements);
        }

        //{ Properties[] }
        AST.Expression parse_object_expr()
        {
            if (tokens[0].type != Token.TokenType.OpenCurleyBracket)
            {
                return this.parse_element_expr();
            }

            advance();

            List<AST.Property> properties = new List<AST.Property>();

            while (tokens[0].type != Token.TokenType.EOF && tokens[0].type != Token.TokenType.CloseCurleyBracket)
            {
                string data_type = advance(Token.TokenType.DataType, "DataType expected").value;

                string key = advance(Token.TokenType.Identifier, "Object Literal key expected").value;

                advance(Token.TokenType.Colon, "missing Colon following identifier in Object Literal");

                AST.Expression value = parse_expr();

                properties.Add(new AST.Property(key, value, data_type));
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
           // Array Accessor
        // AdditiveExpr
        // MultiplicativeExpr
        // Call
        // Member
        // Primary

        AST.Expression parse_element_expr()
        {
            AST.Expression arr_expr = parse_additive_expr();

            while (tokens[0].type == Token.TokenType.OpenSquareBracket)
            {
                advance();

                AST.Expression index_expr = parse_additive_expr();

                advance(Token.TokenType.CloseSquareBracket, "Expecting closing square bracket after array accessor");

                arr_expr = new AST.ArrayIndexExpr(arr_expr, index_expr);
            }

            return arr_expr;
        }

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
            AST.Expression member; 
            
            member = parse_member_expr();

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
            Token type_token = advance(new Token.TokenType[] {Token.TokenType.DataType, Token.TokenType.Function}, "Expected Data Type for Parameter");
            string arg_type = type_token.value;

            List<KeyValuePair<AST.Expression, string>> args = new List<KeyValuePair<AST.Expression, string>>() { new KeyValuePair<AST.Expression, string>( parse_expr(), arg_type) };

            while (tokens[0].type == Token.TokenType.Comma && advance() != null)
            {
                type_token = advance(new Token.TokenType[] { Token.TokenType.DataType, Token.TokenType.Function }, "Expected Data Type for Parameter");
                arg_type = type_token.value;
                KeyValuePair<AST.Expression, string> arg = new KeyValuePair<AST.Expression, string>(parse_expr(), arg_type);

                args.Add(arg);
            }

            return args;
        }

        AST.Expression parse_member_expr()
        {
            AST.Expression obj = parse_primary_expr();

            while (tokens[0].type == Token.TokenType.Period)// || tokens[0].type == Token.TokenType.OpenSquareBracket) 
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

                case Token.TokenType.Boolean:
                    return new AST.BoolLiteral(parse_bool(advance().value));

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

        AST.MetaStatement parse_meta_stmt()
        {
            Token.TokenType tkn = tokens[0].type;

            switch (tkn)
            {
                case Token.TokenType.MetaImmutable:
                    advance();
                    return new AST.MutDefaultStatement(true);

                case Token.TokenType.MetaMutable:
                    advance();
                    return new AST.MutDefaultStatement(false);

                case Token.TokenType.MetaSilly:
                    advance();
                    return new AST.SillyDefaultStatement();

                default:
                    throw new Exception("Unrecognized Meta Statement");
            }
        }
    }
}
