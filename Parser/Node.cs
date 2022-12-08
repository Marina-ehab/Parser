using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    internal class Node
    {
        string tokenType;
        string value;
        List<Node> children;
        Node? nextNode;
    }
}
