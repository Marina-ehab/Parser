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
        public TokenType tokenType;
        public String tokenValue;
        public String printText;
        public Token(TokenType tokenType, string tokenValue, string printText)
        {
            this.tokenType = tokenType;
            this.tokenValue = tokenValue;
            this.printText = printText;
        }
    }
}