using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace NewLangInterpreter.Frontend
{
    internal class Lexer
    {
        // Tokenize Function to Tokenize Source Code
        public List<Token> tokenize(string source)
        {
            // Init List
            List<Token> tokens = new List<Token>();

            List<char> src;

            {
                char[] src_org = source.ToCharArray();

                src = src_org.ToList();
            }


            // Build each token until EOF
            while (src.Count > 0)
            {
                // Check For Comment
                if (src[0] == '/' && src[1] == '/')
                {
                    // Remove Comment Opening
                    src.RemoveAt(0);
                    src.RemoveAt(0);

                    // Remove Comment
                    while (src.Count > 0 && src[0] != '\n' && src[0] != '\r')
                    {
                        src.RemoveAt(0);
                    }
                }
                else if (src[0] == '/' && src[1] == '*')
                {
                    // Remove Comment Opening
                    src.RemoveAt(0);
                    src.RemoveAt(0);

                    // Remove Comment
                    while (!(src[0] == '*' && src[1] == '/'))
                    {
                        src.RemoveAt(0);
                    }

                    // Remove Comment Closing
                    src.RemoveAt(0);
                    src.RemoveAt(0);
                }
                else
                {
                    switch (src[0])
                    {
                        case '(':
                            tokens.Add(new Token(Token.TokenType.OpenParen, src[0].ToString()));
                            src.RemoveAt(0);
                            break;

                        case ')':
                            tokens.Add(new Token(Token.TokenType.CloseParen, src[0].ToString()));
                            src.RemoveAt(0);
                            break;

                        case '[':
                            tokens.Add(new Token(Token.TokenType.OpenSquareBracket, src[0].ToString()));
                            src.RemoveAt(0);
                            break;

                        case ']':
                            tokens.Add(new Token(Token.TokenType.CloseSquareBracket, src[0].ToString()));
                            src.RemoveAt(0);
                            break;

                        case '{':
                            tokens.Add(new Token(Token.TokenType.OpenCurleyBracket, src[0].ToString()));
                            src.RemoveAt(0);
                            break;

                        case '}':
                            tokens.Add(new Token(Token.TokenType.CloseCurleyBracket, src[0].ToString()));
                            src.RemoveAt(0);
                            break;

                        case '+':
                        case '-':
                        case '*':
                        case '/':
                        case '%':
                            if ((src[0] == '+' || src[0] == '-') && src[0] == src[1])
                            {
                                tokens.Add(new Token(Token.TokenType.UnaryOperator, "" + src[0] + src[1]));
                                src.RemoveAt(0);
                                src.RemoveAt(0);
                                break;
                            }
                            else if (src[1] == '=')
                            {
                                tokens.Add(new Token(Token.TokenType.BinaryOperator, "" + src[0] + src[1]));
                                src.RemoveAt(0);
                                src.RemoveAt(0);
                                break;
                            }

                            tokens.Add(new Token(Token.TokenType.BinaryOperator, src[0].ToString()));
                            src.RemoveAt(0);
                            break;

                        case '!':
                            if (src[1] == '=')
                            {
                                tokens.Add(new Token(Token.TokenType.ComparisonOperator, "" + src[0] + src[1]));
                                src.RemoveAt(0);
                                src.RemoveAt(0);
                                break;
                            }
                            tokens.Add(new Token(Token.TokenType.UnaryOperator, src[0].ToString()));
                            src.RemoveAt(0);
                            break;

                        case '&':
                            if (src[0] == src[1])
                            {
                                tokens.Add(new Token(Token.TokenType.BooleanOperator, "" + src[0] + src[1]));
                                src.RemoveAt(0);
                                src.RemoveAt(0);
                            }
                            break;

                        case '|':
                            if (src[0] == src[1])
                            {
                                tokens.Add(new Token(Token.TokenType.BooleanOperator, "" + src[0] + src[1]));
                                src.RemoveAt(0);
                                src.RemoveAt(0);
                            }
                            break;

                        case '=':
                            if (src[1] == '=')
                            {
                                tokens.Add(new Token(Token.TokenType.ComparisonOperator, "=="));
                                src.RemoveAt(0);
                                src.RemoveAt(0);
                            }
                            else
                            {
                                tokens.Add(new Token(Token.TokenType.Assign, src[0].ToString()));
                                src.RemoveAt(0);
                            }

                            break;

                        case '>':
                        case '<':
                            if (src[1] == '=')
                            {
                                tokens.Add(new Token(Token.TokenType.ComparisonOperator, "" + src[0] + src[1]));
                                src.RemoveAt(0);
                                src.RemoveAt(0);
                            }
                            else
                            {
                                tokens.Add(new Token(Token.TokenType.ComparisonOperator, "" + src[0]));
                                src.RemoveAt(0);
                            }
                            break;

                        case '.':
                            tokens.Add(new Token(Token.TokenType.Period, src[0].ToString()));
                            src.RemoveAt(0);
                            break;

                        case ',':
                            tokens.Add(new Token(Token.TokenType.Comma, src[0].ToString()));
                            src.RemoveAt(0);
                            break;

                        case ';':
                            tokens.Add(new Token(Token.TokenType.SemiColon, src[0].ToString()));
                            src.RemoveAt(0);
                            break;

                        case ':':
                            tokens.Add(new Token(Token.TokenType.Colon, src[0].ToString()));
                            src.RemoveAt(0);
                            break;

                        /*case '#':
                            tokens.Add(new Token(Token.TokenType.MetaStart, src[0].ToString()));
                            src.RemoveAt(0);
                            break;*/


                        // Handle Multi-Character Token
                        default:

                            if (src[0] == '#') // Parse Meta Statement
                            {
                                // Advance Past #
                                src.RemoveAt(0);

                                string ident = "";

                                while (src.Count > 0 && (char.IsLetter(src[0]) || valid_ident_chars.Contains(src[0])))
                                {
                                    ident += src[0];
                                    src.RemoveAt(0);
                                }

                                // Check for Reserved Keyword
                                if (META_KEYWORDS.ContainsKey(ident))
                                {
                                    Token.TokenType reserved;

                                    reserved = META_KEYWORDS[ident];

                                    tokens.Add(new Token(reserved, ident));

                                }
                                else
                                {
                                    throw_lexer_error("Invalid Meta Keyword");
                                }
                                

                            }
                            else if (char.IsDigit(src[0])) // Build Number Token
                            {
                                string num = "";
                                
                                while (src.Count > 0 && (char.IsDigit(src[0]) || valid_num_chars.Contains(Char.ToLower(src[0]))))
                                {
                                    num += src[0];
                                    src.RemoveAt(0);
                                }

                                if (num.Contains('.')) // If is Float
                                {
                                    tokens.Add(new Token(Token.TokenType.Float, num));
                                }
                                else // If is Int
                                {
                                    tokens.Add(new Token(Token.TokenType.Integer, num));
                                }

                            }
                            else if (char.IsLetter(src[0])) // Build Identifier
                            {
                                string ident = "";

                                while (src.Count > 0 && (char.IsLetter(src[0]) || valid_ident_chars.Contains(src[0])))
                                {
                                    ident += src[0];
                                    src.RemoveAt(0);
                                }

                                // Check for Reserved Keyword
                                Token.TokenType reserved = Token.TokenType.Identifier;

                                if (KEYWORDS.ContainsKey(ident))
                                {
                                    reserved = KEYWORDS[ident];
                                }

                                tokens.Add(new Token(reserved, ident));
                            }
                            else if (skipable_chars.Contains(src[0])) // Discard Skipable Chars
                            {
                                src.RemoveAt(0);
                            }
                            else if (src[0] == '"') // Parse String Token
                            {
                                src.RemoveAt(0);
                                string value = "";

                                while (src.Count > 0 && src[0] != '"' )
                                {
                                    value += src[0];
                                    src.RemoveAt(0);
                                }

                                if (src.Count == 0)
                                {
                                    throw_lexer_error("Expected \" character at end of string literal");
                                }
                                
                                src.RemoveAt(0);

                                tokens.Add(new Token(Token.TokenType.String, value));
                            }
                            else if (src[0] == '\'') // Parse Character Token
                            {
                                src.RemoveAt(0);
                                char value;

                                if (src.Count == 0)
                                {
                                    throw_lexer_error("Expected Character after \'");
                                }

                                if (src[0] == '\\')
                                {
                                    src.RemoveAt(0);

                                    value = ESCAPE_CHARACTERS[src[0]];
                                    src.RemoveAt(0);
                                }
                                else
                                {

                                    value = src[0];
                                    src.RemoveAt(0);
                                }

                                if (src[0] == '\'')
                                {
                                    src.RemoveAt(0);
                                }
                                else
                                {
                                    throw_lexer_error("Expected Closing single quote \"'\" ");
                                }

                                tokens.Add(new Token(Token.TokenType.Char, "" + value));
                            }
                            else
                            {
                                throw_lexer_error("Unrecognized character found in source: " + src[0] + "\nCharcode: " + ((byte)src[0]));
                            }

                            break;
                    }
                }
            }

                

            tokens.Add(new Token(Token.TokenType.EOF, "EndOfFile"));

            return tokens;
        }

        List<char> valid_ident_chars = new List<char>()
        {
            '_',
            '-',
            '1',
            '2',
            '3',
            '4',
            '5',
            '6',
            '7',
            '8',
            '9',
            '0',

        };

        List<char> valid_num_chars = new List<char>()
        {
            '_',
            '.',
            'x',
            'b',
            'q',
            'o',
            'v',
            'z',

            
            // HEX
            'a',
            'b',
            'c',
            'd',
            'e',
            'f',

            // Base-36
            'g',
            'h',
            'i',
            'j',
            'k',
            'l',
            'm',
            'n',
            'o',
            'p',
            'q',
            'r',
            's',
            't',
            'u',
            'v',
            'w',
            'x',
            'y',
            'z',
        };

        List<char> skipable_chars = new List<char>()
        {
            ' ',
            '\n',
            '\t',
            '\r',
        };

        public Dictionary<string, Token.TokenType> KEYWORDS = new Dictionary<string, Token.TokenType>()
        {
            { "mut", Token.TokenType.Mut },
            { "const", Token.TokenType.Const },
            { "func", Token.TokenType.Function },
            { "proc", Token.TokenType.Procedure },

            { "return", Token.TokenType.Return },

            { "int", Token.TokenType.DataType },
            { "char", Token.TokenType.DataType },
            { "float", Token.TokenType.DataType },
            { "bool", Token.TokenType.DataType },
            { "string", Token.TokenType.DataType },
            { "obj", Token.TokenType.DataType },
            { "void", Token.TokenType.DataType },

            { "true", Token.TokenType.Boolean },
            { "false", Token.TokenType.Boolean },

            { "null", Token.TokenType.Null },

            { "if", Token.TokenType.If },
            { "else", Token.TokenType.Else },
            { "do", Token.TokenType.Do },
            { "while", Token.TokenType.While },
            { "for", Token.TokenType.For },

            
        };

        public Dictionary<string, Token.TokenType> META_KEYWORDS = new Dictionary<string, Token.TokenType>()
        {
            { "immutable", Token.TokenType.MetaImmutable },
            { "constant", Token.TokenType.MetaImmutable },
            { "mutable", Token.TokenType.MetaMutable },

            { "silly", Token.TokenType.MetaSilly },
            { "funny", Token.TokenType.MetaSilly },
        };

        public Dictionary<char, char> ESCAPE_CHARACTERS = new Dictionary<char, char>()
        {
            { 'n', '\n' },
            { 'r', '\r' },
            { 't', '\t' },
            { 'v', '\v' },
            { 'a', '\a' },
            { '\\', '\\' },
            { '\'', '\'' },
            { '\"', '\"' },
        };

        static void throw_lexer_error(string msg)
        {
            Console.Error.WriteLine("Lexer Error: " + msg);
            System.Environment.Exit(0);
        }
    }

}
