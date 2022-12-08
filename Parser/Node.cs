using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{

    internal class Node
    {
        private Token token;
        private List<Node> children;
        private Node? nextNode;

        public Node(Token token)
        {
            this.token = token;
            this.children = new List<Node>();
            this.nextNode = null;
        }

        public void AddChild(Node child)
        {
            this.children.Add(child);
        }

        public void AddNextNode(Node nextNode)
        {
            this.nextNode = nextNode;
        }

        public Token GetToken()
        {
            return this.token;
        }

    }
}
