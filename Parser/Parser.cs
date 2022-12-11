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
            program.AddChild(stmt_sequence());
            return program;
        }
        private Node stmt_sequence()
        {
            Node stmt_sequence = new Node(NodeType.StmtSequence);
            stmt_sequence.AddChild(statement());
            while (peek()?.tokenValue == ";")
            {
                match();
                stmt_sequence.AddChild(new Node(value:";"));
                stmt_sequence.AddChild(statement());
            }
            return stmt_sequence;
        }

        private Node statement()
        {

            Node statement = new Node(NodeType.Statement);
            var top = peek();
            switch (top?.tokenValue)
            {
                case "if":
                    statement.AddChild(if_stmt());
                    break;
                case "repeat":
                    statement.AddChild(repeat_stmt());
                    break;
                case "read":
                    statement.AddChild(read_stmt());
                    break;
                case "write":
                    statement.AddChild(write_stmt());
                    break;
                default:
                    switch (top?.tokenType)
                    {
                        case TokenType.Identifier:
                            statement.AddChild(assign_stmt());
                            break;
                        default:
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
        private Node exp();
        private Node term() { 
            //term --> factor {mulop factor }
            Node term = new Node(NodeType.Term);
            term.AddChild(factor());
            while (peek()?.tokenValue == "*"|| peek()?.tokenValue == "/")
            {
                term.AddChild(mulop());
                term.AddChild(factor());
            }
            return term;
        }
        private Node simple_exp() { 
            //simple exp--> term {addop term}
            Node simple_exp = new Node(NodeType.SimpleExpression );
            simple_exp.AddChild(term());
            while (peek()?.tokenValue == "+"|| peek()?.tokenValue == "-") { 
                simple_exp.AddChild(addop());
                simple_exp.AddChild(term());
            }
            return simple_exp;
        }
        private Node factor() { 
            //factor -->(exp)|number|identifier
            Node factor = new Node(NodeType.Factor);
            var checktop = peek();
            switch (checktop?.tokenValue) {
                case "(":
                    factor.AddChild(new Node(value: "("));
                    match();
                    factor.AddChild(exp());
                    if (peek()?.tokenValue == ")")
                    {
                        factor.AddChild(new Node(value: ")"));
                        match();
                    }
                    else {
                        throw new Exception("Error at line " + checktop?.line + " near column " + checktop?.column + " Syntax error");
                    }
                    break;

				default:
                    switch (peek()?.tokenType)
                    {
                        case TokenType.Number:
                            if (checktop?.tokenType == TokenType.Number)
                            {
                                factor.AddChild(new Node(value: "Number" + "(" + checktop.tokenValue + ")"));
                                match();
                            }
                            else
                            {
                                throw new Exception("Error at line " + checktop?.line + " near column " + checktop?.column + " Syntax error");

                            }
                            break;

                        case TokenType.Identifier:
                            if (checktop?.tokenType == TokenType.Identifier)
                            {
                                factor.AddChild(new Node(value: "Identifier" + "(" + checktop.tokenValue + ")"));
                                match();
                            }
                            else
                            {
                                throw new Exception("Error at line " + checktop?.line + " near column " + checktop?.column + " Syntax error");

                            }
                            break;
                        default:
                            throw new Exception("Error at line " + checktop?.line + " near column " + checktop?.column + " Syntax Error");

                    }
                    break;

            }
            return factor;
        }
        private Node mulop() {
            //mulop --> *|/
            Node mulop = new Node(NodeType.Mulop);
            var checktop = peek(); 
            switch (checktop?.tokenValue)
            {
                case "*":
                    mulop.AddChild(new Node(value: "*"));
                    match();
                    break;
                case "/":
                    mulop.AddChild(new Node(value: "/"));
                    match();
                    break;
                default:
                    throw new Exception("Error at line " + checktop?.line + " near column " + checktop?.column + " expected (* or /)");

            }
            return mulop;
        }
        private Node addop() { 
            //addop --> +|-
            Node addop = new Node(NodeType.Addop);
            var checktop = peek(); //check next token 
            switch (checktop?.tokenValue) {
                case "+":
                    addop.AddChild(new Node(value:"+"));
                    match();
                    break;
                case "-":
                    addop.AddChild(new Node(value: "-"));
                    match();
                    break;
                default:
                    throw new Exception("Error at line " + checktop?.line + " near column " + checktop?.column + " expected (+ or -)");

            }
            return addop;
        }
        private Node comparisonop();
    }
}
