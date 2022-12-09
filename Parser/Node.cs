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
        private String? value;
        private NodeType? type;
        private List<Node> children;

        public Node(NodeType? type = null, String? value = null)
        {
            this.value = value;
            this.type = type;
            this.children = new List<Node>();
        }
        
        public void AddChild(Node child)
        {
            this.children.Add(child);
        }

    }
}
