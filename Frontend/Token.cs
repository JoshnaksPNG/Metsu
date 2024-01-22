using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewLangInterpreter.Frontend
{
    internal class Token
    {
        public Token(TokenType typ, string val)
        {
            type = typ;

            value = val;
        }

        public string value;

        public TokenType type;

        public enum TokenType
        {
            // Literal Types
            Null,
            Boolean,
            Integer,
            Float,
            String,

            DataType,
            Identifier,
            Assign,

            Period,

            Comma,

            DoubleQuote,
            SingleQuote,
            BackTick,

            // Brackets
            OpenParen, CloseParen,
            OpenSquareBracket, CloseSquareBracket,
            OpenCurleyBracket, CloseCurleyBracket,

            LessThan, GreaterThan,

            BinaryOperator,
            UnaryOperator,

            Mut,
            Const,
            Function,
            Return,

            SemiColon,
            Colon,

            EOF,
        }

        override public string ToString()
        {
            string returned = "{ type: ";

            returned += type.ToString();

            returned += ", val: ";

            returned += value.ToString() + " }";

            return returned;
        }
    }
}
