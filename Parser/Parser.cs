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
                throw new Exception("Error at line " + top?.line + " near column " + top?.column + ". Syntax Error");
                    }
                    break;
            }
            return statement;
        }

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

        private Node comparisonop()
        {
            Node comparisonop = new Node(NodeType.ComparisonOp);
            var top = peek();

            // ComparisonOp -> < | =
            switch (top?.tokenValue)
            {
                case "<":
                    comparisonop.AddChild(new Node(value: "<"));
                    match();
                    break;
                case "=":
                    comparisonop.AddChild(new Node(value: "="));
                    match();
                    break;
                default:
                    // actually this line is impossible to reach, as we checked top value in exp() before calling compariosnop()
                    // but you know IT'S THE LAW
                    throw new Exception("Error at line " + top?.line + " near column " + top?.column);
            }
            return comparisonop;
        }
        private Node assign_stmt()
        {
            Node assign_stmt = new Node(NodeType.AssignStmt);
            var top = peek();
            if (top?.tokenType == TokenType.Identifier)
            {
                assign_stmt.AddChild(new Node(NodeType.Identifier, top.tokenValue));
                match();
            }
            else
            {
                throw new Exception("Error at line " + top?.line + " near column " + top?.column + ". Syntax Error");

            }
            top = peek();
            if (top.tokenType == TokenType.Symbol && top.tokenValue == ":=")
            {
                assign_stmt.AddChild(new Node(value: ":="));
                match();
            }
            else
            {
                throw new Exception("Error at line " + top?.line + " near column " + top?.column + ". Syntax Error");
            }
            /*maybe a bug*/
            assign_stmt.AddChild(exp());

            return assign_stmt;
        }

        private Node write_stmt()
        {
            Node write_node = new Node(NodeType.WriteStmt);

            var top = peek();

            if (top.tokenValue == "write")
            {
                write_node.AddChild(new Node(value: "write"));
                match();
            }
            else
            {
                throw new Exception("Error at line " + top?.line + " near column " + top?.column + ". Syntax Error");
            }
            write_node.AddChild(exp());

            return write_node;
        }
        private Node read_stmt()
        {
            Node read_node = new Node(NodeType.ReadStmt);

            var top = peek();

            if (top.tokenValue == "read")
            {
                read_node.AddChild(new Node(value: "read"));
                match();
            }
            else
            {
                throw new Exception("Error at line " + top?.line + " near column " + top?.column + ". Syntax Error");

            }
            top = peek();

            if (top.tokenType == TokenType.Identifier)
            {
                read_node.AddChild(new Node(NodeType.Identifier, top.tokenValue));
                match();
            }
            else
            {
                throw new Exception("Error at line " + top?.line + " near column " + top?.column + ". Syntax Error");

            }
            return read_node;
        }
        private Node repeat_stmt()
        {
            Node repeat_node = new Node(NodeType.RepeatStmt);
            var top = peek();
            if (top.tokenValue == "repeat")
            {
                repeat_node.AddChild(new Node(value: "repeat"));
                match();
            }
            else
            {
                throw new Exception("Error at line " + top?.line + " near column " + top?.column + ". Syntax Error");

            }
            repeat_node.AddChild(stmt_sequence());

            top = peek();
            if (top.tokenValue == "until")
            {
                repeat_node.AddChild(new Node(value: "until"));
                match();
            }
            else
            {
                throw new Exception("Error at line " + top?.line + " near column " + top?.column + ". Syntax Error");

            }
            repeat_node.AddChild(exp());

            return repeat_node;

        }
        private Node if_stmt()
        {
            Node if_node = new Node(NodeType.IfStmt);
            var top = peek();
            //if part
            if (top.tokenValue == "if")
            {
                match();
                if_node.AddChild(new Node(value: "if"));
            }
            else
            {
                throw new Exception("Error at line " + top?.line + " near column " + top?.column + ". Syntax Error");

            }
            //call exp procedure
            if_node.AddChild(exp());

            //then part
            top = peek();
            if (top.tokenValue == "then")
            {
                match();
                if_node.AddChild(new Node(value: "then"));
            }
            else
            {
                throw new Exception("Error at line " + top?.line + " near column " + top?.column + ". Syntax Error");

            }
            //stmt-seq call
            if_node.AddChild(stmt_sequence());
            //else part is optional either else or end 
            top = peek();
            if (top.tokenValue == "else")
            {
                if_node.AddChild(new Node(value: "else"));
                match();
                if_node.AddChild(stmt_sequence());
            }
            top = peek(); //redundant if we dont have an else
            //end part
            if (top.tokenValue == "end")
            {
                if_node.AddChild(new Node(value: "end"));
                match();
            }
            else
            {
                throw new Exception("Error at line " + top?.line + " near column " + top?.column + ". Syntax Error");


            }
            return if_node;

        }
        private Node term()
        {
            //term --> factor {mulop factor }
            Node term = new Node(NodeType.Term);
            term.AddChild(factor());
            while (peek()?.tokenValue == "*" || peek()?.tokenValue == "/")
            {
                term.AddChild(mulop());
                term.AddChild(factor());
            }
            return term;
        }
        private Node simple_exp()
        {
            //simple exp--> term {addop term}
            Node simple_exp = new Node(NodeType.SimpleExpression);
            simple_exp.AddChild(term());
            while (peek()?.tokenValue == "+" || peek()?.tokenValue == "-")
            {
                simple_exp.AddChild(addop());
                simple_exp.AddChild(term());
            }
            return simple_exp;
        }
        private Node factor()
        {
            //factor -->(exp)|number|identifier
            Node factor = new Node(NodeType.Factor);
            var top = peek();
            switch (top?.tokenValue)
            {
                case "(":
                    factor.AddChild(new Node(value: "("));
                    match();
                    factor.AddChild(exp());
                    if (peek()?.tokenValue == ")")
                    {
                        factor.AddChild(new Node(value: ")"));
                        match();
                    }
                    else
                    {
                        throw new Exception("Error at line " + top?.line + " near column " + top?.column + ". Syntax Error");

                    }
                    break;

                default:
                    switch (peek()?.tokenType)
                    {
                        case TokenType.Number:
                            if (top?.tokenType == TokenType.Number)
                            {
                                factor.AddChild(new Node(type: NodeType.Number, value: top.tokenValue));
                                match();
                            }
                            else
                            {
                                throw new Exception("Error at line " + top?.line + " near column " + top?.column + ". Syntax Error");


                            }
                            break;

                        case TokenType.Identifier:
                            if (top?.tokenType == TokenType.Identifier)
                            {
                                factor.AddChild(new Node(NodeType.Identifier, top.tokenValue));
                                match();
                            }
                            else
                            {
                                throw new Exception("Error at line " + top?.line + " near column " + top?.column + ". Syntax Error");


                            }
                            break;
                        default:
                            throw new Exception("Error at line " + top?.line + " near column " + top?.column + ". Syntax Error");


                    }
                    break;

            }
            return factor;
        }
        private Node mulop()
        {
            //mulop --> *|/
            Node mulop = new Node(NodeType.Mulop);
            var top = peek();
            switch (top?.tokenValue)
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
                    throw new Exception("Error at line " + top?.line + " near column " + top?.column + ". Syntax Error");


            }
            return mulop;
        }
        private Node addop()
        {
            //addop --> +|-
            Node addop = new Node(NodeType.Addop);
            var top = peek(); //check next token 
            switch (top?.tokenValue)
            {
                case "+":
                    addop.AddChild(new Node(value: "+"));
                    match();
                    break;
                case "-":
                    addop.AddChild(new Node(value: "-"));
                    match();
                    break;
                default:
                    throw new Exception("Error at line " + top?.line + " near column " + top?.column + ". Syntax Error");


            }
            return addop;
        }
    }
}
