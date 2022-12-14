using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    enum NodeType
    {
        Program,
        StmtSequence,
        Statement,
        IfStmt,
        RepeatStmt,
        AssignStmt,
        ReadStmt,
        Expression,
        WriteStmt,
        SimpleExpression,
        ComparisonOp,
        Term,
        Addop,
        Mulop,
        Factor,
        Identifier,
        Number
    }
    
    internal class Node
    {
        public String? value;
        public NodeType? type;
        public List<Node> children;
        public static string output;
        public Node(NodeType? type = null, String? value = null)
        {
            this.value = value;
            this.type = type;
            this.children = new List<Node>();
            
            // just for debugging
            if (value != null) output += value + " ";
        }
        
        public void AddChild(Node child)
        {
            this.children.Add(child);
        }

    }
}
