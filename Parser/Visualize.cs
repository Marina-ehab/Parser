using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetGraph;
using DotNetGraph.Node;
using DotNetGraph.Edge;
using DotNetGraph.Extensions;
using GraphVizNet;

namespace Parser
{
    internal class Visualize
    {
        DotNode graphBuilder(Node head, ref DotGraph dotGraph)
        {
            if(head.children.Count == 0) {
                return null;
            }

            
            if(head.type == NodeType.Program)
            {
                return graphBuilder(head.children[0], ref dotGraph);
            }
            else if(head.type == NodeType.StmtSequence)
            {
                List<DotNode> nodes = new List<DotNode>();
                for (int i = 0; i < head.children.Count; i++) { 
                    if(head.type == NodeType.Statement)
                    {
                        nodes.Add(graphBuilder(head.children[i], ref dotGraph));
                    }
                }
                for (int i = 1; i < nodes.Count ; i++) {
                    DotEdge edge = new DotEdge(nodes[i], nodes[i+1]);
                    dotGraph.Elements.Add(edge);
                }

            }
            else if(head.type == NodeType.Statement)
            {
                return graphBuilder(head.children[0],ref dotGraph);
            }
            else if (head.type == NodeType.IfStmt)
            {
                DotNode ifNode = new DotNode("if")
                {
                    Shape = DotNodeShape.Rectangle,
                    Label = "if",
                    FillColor = Color.White,
                    FontColor = Color.Black,
                    Style = DotNodeStyle.Solid,
                    Width = 0.5f,
                    Height = 0.5f,
                };
                dotGraph.Elements.Add(ifNode);
                for (int i = 1; i < head.children.Count ; i++)
                {
                    if (head.type == NodeType.Expression || head.type == NodeType.StmtSequence)
                    {
                        DotEdge edge = new DotEdge(ifNode, graphBuilder(head.children[i], ref dotGraph));
                        dotGraph.Elements.Add(edge);
                    }
                }

            }
            else if( head.type == NodeType.Expression)
            {
                DotNode expNode = new DotNode("exp") {
                    Shape = DotNodeShape.Ellipse,
                    Label = "My node!",
                    FillColor = Color.Coral,
                    FontColor = Color.Black,
                    Style = DotNodeStyle.Dotted,
                    Width = 0.5f,
                    Height = 0.5f,
                    PenWidth = 1.5f
                };
                dotGraph.Elements.Add(expNode);
                return expNode;
            }


            DotNode node = new DotNode("gen")
            {
                Shape = DotNodeShape.Ellipse,
                Label = "My node!",
                FillColor = Color.Coral,
                FontColor = Color.Black,
                Style = DotNodeStyle.Dotted,
                Width = 0.5f,
                Height = 0.5f,
                PenWidth = 1.5f
            };
            dotGraph.Elements.Add(node);
            return node;

        }
    }
}
