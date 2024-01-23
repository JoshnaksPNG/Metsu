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
                        tokens.Add(new Token(Token.TokenType.BinaryOperator, src[0].ToString()));
                        src.RemoveAt(0);
                        break;

                    case '!':
                        tokens.Add(new Token(Token.TokenType.UnaryOperator, src[0].ToString()));
                        src.RemoveAt(0);
                        break;

                    case '=':
                        tokens.Add(new Token(Token.TokenType.Assign, src[0].ToString()));
                        src.RemoveAt(0);
                        break;

                    case '\'':
                        tokens.Add(new Token(Token.TokenType.SingleQuote, src[0].ToString()));
                        src.RemoveAt(0);
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


                    // Handle Multi-Character Token
                    default:

                        // Build Number Token
                        if (char.IsDigit(src[0]))
                        {
                            string num = "";

                            while (src.Count > 0 && (char.IsDigit(src[0]) || valid_num_chars.Contains(src[0])))
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
                        else if (src[0] == '"')
                        {
                            src.RemoveAt(0);
                            string value = "";

                            while (src.Count > 0 && src[0] != '"')
                            {
                                value += src[0];
                                src.RemoveAt(0);
                            }

                            src.RemoveAt(0);

                            tokens.Add(new Token(Token.TokenType.String, value));
                        }
                        else if (src[0] == '\'')
                        {
                            src.RemoveAt(0);
                            char value ;

                            if (src[0] == '\\')
                            {
                                src.RemoveAt(0);

                                value = ESCAPE_CHARACTERS[src[0]];
                            }
                            else
                            {
                                value = src[0];
                            }

                            src.RemoveAt(0);

                            tokens.Add(new Token(Token.TokenType.Char, "" + value));
                        }
                        else
                        {
                            Console.WriteLine("Unrecognized character found in source: " + src[0] + "\nCharcode: " + ((byte)src[0]));
                            Environment.Exit(0);
                        }

                        break;
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

            { "return", Token.TokenType.Return },

            { "int", Token.TokenType.DataType },
            { "char", Token.TokenType.DataType },
            { "float", Token.TokenType.DataType },
            { "bool", Token.TokenType.DataType },
            { "string", Token.TokenType.DataType },
            { "obj", Token.TokenType.DataType },

            { "true", Token.TokenType.Boolean },
            { "false", Token.TokenType.Boolean },

            { "null", Token.TokenType.Null },
        };

        public Dictionary<char, char> ESCAPE_CHARACTERS = new Dictionary<char, char>()
        {
            { 'n', '\n' },
            { 'r', '\r' },
            { 't', '\t' },
            { 'v', '\v' },
            { '\\', '\\' },
            { '\'', '\'' },
            { '\"', '\"' },
        };
    }

}
