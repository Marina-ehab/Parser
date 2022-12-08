using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    internal class Parser
    {
        private Node? parsingTree;
        private Scanner scanner;

        public Parser(String tinyCode)
        {
            this.scanner = new Scanner(tinyCode);
        }

        private Token? peek()
        {
            Token? top = scanner.GetNextToken();
            scanner.GoBackTokens(1);
            return top;
        }
        private bool match(Token token)
        {
            return scanner.GetNextToken() == token;
        }
        private void unmatch(Token token)
        {
            scanner.GoBackTokens(1);
        }
    }
}
