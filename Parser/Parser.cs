using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    internal class Parser
    {
        public Node? parseTree;
        private Scanner scanner;

        public Parser(String tinyCode)
        {
            this.scanner = new Scanner(tinyCode);
            // the final parse tree
            parseTree = program();
        }

        private Token? peek()
        {
            Token? top = scanner.GetNextToken();
            scanner.GoBackTokens(1);
            return top;
        }
        private void match()
        {
            scanner.GetNextToken();
        }

        private Node? program()
        {
            Node program = new Node(NodeType.Program);

            // program -> stmt_sequence
            program.AddChild(stmt_sequence());
            return program;
        }
        private Node stmt_sequence()
        {
            Node stmt_sequence = new Node(NodeType.StmtSequence);

            // stmt-sequence -> statement
            stmt_sequence.AddChild(statement());

            // stmt-sequence -> statement {; statement}
            while (peek()?.tokenValue == ";")
            {
                match();
                stmt_sequence.AddChild(new Node(value: ";"));
                stmt_sequence.AddChild(statement());
            }
            return stmt_sequence;
        }

        private Node statement()
        {

            Node statement = new Node(NodeType.Statement);

            // stmt -> if_stmt | repeat_stmt | read_stmt | write_stmt | assign_stmt
            var top = peek();
            switch (top?.tokenValue)
            {
                case "if":
                    // stmt -> if_stmt
                    statement.AddChild(if_stmt());
                    break;
                case "repeat":
                    // stmt -> repeat_stmt
                    statement.AddChild(repeat_stmt());
                    break;
                case "read":
                    // stmt -> read_stmt
                    statement.AddChild(read_stmt());
                    break;
                case "write":
                    // stmt -> write_stmt
                    statement.AddChild(write_stmt());
                    break;
                default:
                    switch (top?.tokenType)
                    {
                        case TokenType.Identifier:
                            // stmt -> assign_stmt
                            statement.AddChild(assign_stmt());
                            break;
                        default:
                            // an Exception is thrown when there's a syntax error 
                            throw new Exception("Error at line " + top?.line + " near column " + top?.column + ". Colon without equal sign");
                    }
                    break;
            }
            return statement;
        }

        private Node assign_stmt();
        private Node write_stmt();
        private Node read_stmt();
        private Node repeat_stmt();
        private Node if_stmt();
        private Node exp()
        {
            Node exp = new Node(NodeType.Expression);
            // exp -> simple_exp
            exp.AddChild(simple_exp());
            
            // exp -> simple_exp [comparisonOp simple_exp]
            var top = peek();
            if (top?.tokenValue == "<" || top?.tokenValue == "=")
            {   
                // we can make the implementation of comparisonOp here
                // but to keep up with the convention, It's implemented in a dedicated function
                exp.AddChild(comparisonop());
                exp.AddChild(simple_exp());
            }
                
            return exp;
        }

        private Node term();
        private Node simple_exp();
        private Node factor();
        private Node mulop();
        private Node addop();
        
        private Node comparisonop()
        {
            Node comparisonop = new Node(NodeType.ComparisonOp);
            var top = peek();

            // ComparisonOp -> < | =
            switch(top?.tokenValue)
            {
                case ">":
                    comparisonop.AddChild(new Node(value: ">"));
                    match();
                    break;
                case "=":
                    comparisonop.AddChild(new Node(value: "="));
                    match();
                    break;
                default:
                    // actually this line is impossible to reach, as we checked top value in exp() before calling compariosnop()
                    // but you know IT'S THE LAW
                    throw new Exception("Error at line " + top?.line + " near column " + top?.column + ". Colon without equal sign");
            }
            return comparisonop;
        }
    }
}
