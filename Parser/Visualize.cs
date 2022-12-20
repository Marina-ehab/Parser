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
using DotNetGraph.SubGraph;

namespace Parser
{
    internal class Visualize
    {
        static int counter = 0;
        static int clusterNo = 0;
        public static DotNode graphBuilder(Node head, ref DotGraph dotGraph)
        {
            /*
         * Types to implement
         * 
         * Program done
         * Stmt-sequence done
         * Statement done 
         * If-stmt done 
         * Repeat-stmt done
         * Assign-stmt done
         * Read-stmt done
         * Write-stmt done
         * Expression
         * Comparison-op
         * Simple= exp
         * Add-op
         * Term
         * Mulop
         * Factor
         * Number done
         * Identifier done

        */
            counter++;

            //if (head.children.Count == 0)
            //{
            //    DotNode node1 = new DotNode("" + counter + "")
            //    {
            //        Shape = DotNodeShape.Rectangle,
            //        Label = "Terminal",
            //        FillColor = Color.Coral,
            //        FontColor = Color.Black,
            //        Style = DotNodeStyle.Solid,
            //        Width = 0.5f,
            //        Height = 0.5f,
            //        PenWidth = 1.5f
            //    };
            //    dotGraph.Elements.Add(node1);
            //    return node1;
            //}


            if (head.type == NodeType.Program)
            {
                return graphBuilder(head.children[0], ref dotGraph);
            }
            else if (head.type == NodeType.StmtSequence)
            {
                DotSubGraph mySubGraph = new DotSubGraph("cluster_" + clusterNo + "");
                mySubGraph.SetCustomAttribute("style", "invis");
                clusterNo++;

                List<DotNode> nodes = new List<DotNode>();
                for (int i = 0; i < head.children.Count; i++)
                {
                    if (head.children[i].type == NodeType.Statement)
                    {
                        nodes.Add(graphBuilder(head.children[i], ref dotGraph));

                    }
                }
               
                for (int i = 0; i < nodes.Count - 1; i++)
                {
                    
                    mySubGraph.Elements.Add(nodes[i]);
                    DotEdge edge = new DotEdge(nodes[i], nodes[i + 1]);

                    edge.SetCustomAttribute("constraint", "false");
                    edge.SetCustomAttribute("minlen", "5");

                    mySubGraph.Elements.Add(edge);

                }
                mySubGraph.Elements.Add(nodes[nodes.Count - 1]);

                dotGraph.Elements.Add(mySubGraph);

                return nodes[0];

            }
            else if (head.type == NodeType.Statement)
            {
                return graphBuilder(head.children[0], ref dotGraph);
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
                for (int i = 0; i < head.children.Count; i++)
                {
                    if (head.children[i].type == NodeType.Expression || head.children[i].type == NodeType.StmtSequence)
                    {
                        DotEdge edge = new DotEdge(ifNode, graphBuilder(head.children[i], ref dotGraph));
                        dotGraph.Elements.Add(edge);
                    }
                }
                return ifNode;
            }
            else if (head.type == NodeType.AssignStmt)
            {
                DotNode assignNode = new DotNode("" + counter + "")
                {
                    Shape = DotNodeShape.Rectangle,
                    Label = "assign\n (" + head.children[0].value+")",
                    FillColor = Color.Coral,
                    FontColor = Color.Black,
                    Style = DotNodeStyle.Solid,
                    Width = 0.5f,
                    Height = 0.5f,
                    PenWidth = 1.5f
                };
                dotGraph.Elements.Add(assignNode);

                DotEdge edge1 = new DotEdge(assignNode, graphBuilder(head.children[2], ref dotGraph));
                dotGraph.Elements.Add(edge1);
                return assignNode;
            }
            else if (head.type == NodeType.ReadStmt)
            {
                DotNode readNode = new DotNode("" + counter + "")
                {
                    Shape = DotNodeShape.Rectangle,
                    Label = "read \n(" + head.children[1].value + ")",
                    FillColor = Color.White,
                    FontColor = Color.Black,
                    Style = DotNodeStyle.Solid,
                    Width = 0.5f,
                    Height = 0.5f,
                };
                dotGraph.Elements.Add(readNode);
                counter++;
                DotNode invNode = new DotNode("" + counter + "");
                invNode.SetCustomAttribute("style", "invis");
                
                dotGraph.Elements.Add(invNode);

                DotEdge invEdge = new DotEdge(readNode,invNode);
                invEdge.SetCustomAttribute("style", "invis");

                dotGraph.Elements.Add(invEdge);

                return readNode;
            }
            else if (head.type == NodeType.WriteStmt)
            {
                DotNode writeNode = new DotNode("" + counter + "")
                {
                    Shape = DotNodeShape.Rectangle,
                    Label = "write",
                    FillColor = Color.White,
                    FontColor = Color.Black,
                    Style = DotNodeStyle.Solid,
                    Width = 0.5f,
                    Height = 0.5f,
                };
                dotGraph.Elements.Add(writeNode);
                DotEdge edge = new DotEdge(writeNode, graphBuilder(head.children[1], ref dotGraph));
                dotGraph.Elements.Add(edge);
                return writeNode;
            }
            else if (head.type == NodeType.Expression)
            {
                
                return graphBuilder(head.children[0], ref dotGraph);
                
            }
            else if(head.type == NodeType.ComparisonOp)
            {
                DotNode comparisonNode = new DotNode("" + counter + "")
                {
                    Shape = DotNodeShape.Ellipse,
                    Label = "op \n(" + head.value + ")",
                    FillColor = Color.White,
                    FontColor = Color.Black,
                    Style = DotNodeStyle.Solid,
                    Width = 0.5f,
                    Height = 0.5f,
                };
                dotGraph.Elements.Add(comparisonNode);

                DotEdge edge1 = new DotEdge(comparisonNode, graphBuilder(head.children[0], ref dotGraph));
                dotGraph.Elements.Add(edge1);
                DotEdge edge2 = new DotEdge(comparisonNode, graphBuilder(head.children[1], ref dotGraph));
                dotGraph.Elements.Add(edge2);

                return comparisonNode;
            }
            else if (head.type == NodeType.SimpleExpression)
            {
                
                return graphBuilder(head.children[0], ref dotGraph);
               

            }
            else if(head.type == NodeType.Addop)
            {
                DotNode addopNode = new DotNode("" + counter + "")
                {
                    Shape = DotNodeShape.Ellipse,
                    Label = "op \n(" + head.value+ ")",
                    FillColor = Color.White,
                    FontColor = Color.Black,
                    Style = DotNodeStyle.Solid,
                    Width = 0.5f,
                    Height = 0.5f,
                };
                dotGraph.Elements.Add(addopNode);

                DotEdge edge1 = new DotEdge(addopNode, graphBuilder(head.children[0], ref dotGraph));
                dotGraph.Elements.Add(edge1);
                DotEdge edge2 = new DotEdge(addopNode, graphBuilder(head.children[1], ref dotGraph));
                dotGraph.Elements.Add(edge2);

                return addopNode;
            }
            else if (head.type == NodeType.Term)
            {
                
                return graphBuilder(head.children[0], ref dotGraph);
               
            }
            else if(head.type == NodeType.Mulop)
            {
                DotNode mulopNode = new DotNode("" + counter + "")
                {
                    Shape = DotNodeShape.Ellipse,
                    Label = "op \n(" + head.value + ")",
                    FillColor = Color.White,
                    FontColor = Color.Black,
                    Style = DotNodeStyle.Solid,
                    Width = 0.5f,
                    Height = 0.5f,
                };
                dotGraph.Elements.Add(mulopNode);
                DotEdge edge1 = new DotEdge(mulopNode, graphBuilder(head.children[0], ref dotGraph));
                dotGraph.Elements.Add(edge1);
                DotEdge edge2 = new DotEdge(mulopNode, graphBuilder(head.children[1], ref dotGraph));
                dotGraph.Elements.Add(edge2);

                return mulopNode;
            }
            else if(head.type == NodeType.Factor)
            {
                return graphBuilder(head.children[0], ref dotGraph);

            }
            else if (head.type == NodeType.Identifier)
            {
                DotNode indentifierNode = new DotNode("" + counter + "")
                {
                    Shape = DotNodeShape.Ellipse,
                    Label = "Identifier \n(" + head.value + ")",
                    FillColor = Color.Coral,
                    FontColor = Color.Black,
                    Style = DotNodeStyle.Solid,
                    Width = 0.5f,
                    Height = 0.5f,
                    PenWidth = 1.5f
                };
                dotGraph.Elements.Add(indentifierNode);
                return indentifierNode;

            }
            else if (head.type == NodeType.Number)
            {
                DotNode numberNode = new DotNode("" + counter + "")
                {
                    Shape = DotNodeShape.Ellipse,
                    Label = "Const \n(" + head.value+ ")",
                    FillColor = Color.Coral,
                    FontColor = Color.Black,
                    Style = DotNodeStyle.Solid,
                    Width = 0.5f,
                    Height = 0.5f,
                    PenWidth = 1.5f
                };
                dotGraph.Elements.Add(numberNode);
                return numberNode;

            }
            else if(head.type == NodeType.RepeatStmt)
            {
                DotNode repeatNode = new DotNode("" + counter + "")
                {
                    Shape = DotNodeShape.Rectangle,
                    Label = "repeat",
                    FillColor = Color.White,
                    FontColor = Color.Black,
                    Style = DotNodeStyle.Solid,
                    Width = 0.5f,
                    Height = 0.5f,
                };
                dotGraph.Elements.Add(repeatNode);
                for (int i = 0; i < head.children.Count; i++)
                {
                    if (head.children[i].type == NodeType.Expression || head.children[i].type == NodeType.StmtSequence)
                    {
                        DotEdge edge = new DotEdge(repeatNode, graphBuilder(head.children[i], ref dotGraph));
                        dotGraph.Elements.Add(edge);
                    }
                }
                return repeatNode;
            }
            else
            {
               DotNode enode = new DotNode("" + counter + "")
                {
                    Shape = DotNodeShape.Rectangle,
                    Label = "undefinedNode",
                    FillColor = Color.Coral,
                    FontColor = Color.Black,
                    Style = DotNodeStyle.Solid,
                    Width = 0.5f,
                    Height = 0.5f,
                    PenWidth = 1.5f
                };
                dotGraph.Elements.Add(enode);
                return graphBuilder(head.children[0], ref dotGraph);
            }

            DotNode node = new DotNode("" + counter + "")
            {
                Shape = DotNodeShape.Rectangle,
                Label = "md5lsh!",
                FillColor = Color.Coral,
                FontColor = Color.Black,
                Style = DotNodeStyle.Solid,
                Width = 0.5f,
                Height = 0.5f,
                PenWidth = 1.5f
            };
            dotGraph.Elements.Add(node);
            return node;


        }
    }
}
