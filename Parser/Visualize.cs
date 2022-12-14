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
using System.Drawing;

namespace Parser
{
    internal class Visualize
    {
        static int counter = 0;
        public DotNode graphBuilder(Node head, ref DotGraph dotGraph)
        {
            counter++;
            if(head.children.Count == 0) {
                DotNode node1 = new DotNode(""+counter+"")
                {
                    Shape = DotNodeShape.Ellipse,
                    Label = "nullll!",
                    FillColor = Color.Coral,
                    FontColor = Color.Black,
                    Style = DotNodeStyle.Dotted,
                    Width = 0.5f,
                    Height = 0.5f,
                    PenWidth = 1.5f
                };
                dotGraph.Elements.Add(node1);
                return node1;
            }

            
            if(head.type == NodeType.Program)
            {
                return graphBuilder(head.children[0], ref dotGraph);
            }
            else if(head.type == NodeType.StmtSequence)
            {
                List<DotNode> nodes = new List<DotNode>();
                for (int i = 0; i < head.children.Count; i++) { 
                    if(head.children[i].type == NodeType.Statement)
                    {
                        nodes.Add(graphBuilder(head.children[i], ref dotGraph));
                    }
                }
                for (int i = 0; i < nodes.Count -1 ; i++) {
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
                DotNode ifNode = new DotNode("" + counter + "")
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
                for (int i = 0; i < head.children.Count  ; i++)
                {
                    if (head.children[i].type == NodeType.Expression || head.children[i].type == NodeType.StmtSequence)
                    {
                        DotEdge edge = new DotEdge(ifNode, graphBuilder(head.children[i], ref dotGraph));
                        dotGraph.Elements.Add(edge);
                    }
                }

            }
            else if( head.type == NodeType.Expression)
            {
                DotNode expNode = new DotNode("" + counter + "") {
                    Shape = DotNodeShape.Ellipse,
                    Label = "exp!",
                    FillColor = Color.Coral,
                    FontColor = Color.Black,
                    Style = DotNodeStyle.Dotted,
                    Width = 0.5f,
                    Height = 0.5f,
                    PenWidth = 1.5f
                };
                dotGraph.Elements.Add(expNode);
                return graphBuilder(head.children[0], ref dotGraph);
            }
            else {
                DotNode enode = new DotNode(""+counter+"")
                {
                    Shape = DotNodeShape.Ellipse,
                    Label = "else node!",
                    FillColor = Color.Coral,
                    FontColor = Color.Black,
                    Style = DotNodeStyle.Dotted,
                    Width = 0.5f,
                    Height = 0.5f,
                    PenWidth = 1.5f
                };
                dotGraph.Elements.Add(enode);
                return graphBuilder(head.children[0], ref dotGraph);
            }

            DotNode node = new DotNode("" + counter + "")
            {
                Shape = DotNodeShape.Ellipse,
                Label = "md5lsh!",
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
