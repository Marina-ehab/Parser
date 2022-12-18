using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    enum TokenType
    {
        Reserved,
        Symbol,
        Identifier,
        Number
    }

    internal class Token
    {
        public TokenType? tokenType;
        public String? tokenValue;
        public String? printText;
        public int line;
        public int column;
        public Token(TokenType? tokenType, string? tokenValue, string? printText, int line, int column)
        {
            this.tokenType = tokenType;
            this.tokenValue = tokenValue;
            this.printText = printText;
            this.line = line;
            this.column = column;
        }
    }
}