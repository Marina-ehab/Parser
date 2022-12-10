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

        private Node assign_stmt()
        { 
            Node assign_stmt = new Node(NodeType.AssignStmt);
            var top=peek();
            if(top?.tokenType == TokenType.Identifier)
            { 
                assign_stmt.AddChild(new Node(value:"Identifier"+"("+top.tokenValue)+")");
                match();
            }
            else
            { 
               throw new Exception("Error at line " + top?.line + " near column " + top?.column + ". Assigning non identifier");
                   
            }
            top = peek();
            if(top.tokenType==TokenType.Symbol && top.tokenValue==":=")
            { 
                assign_stmt.AddChild(new Node(value:":="));
                match();
            }
            else
            { 
                throw new Exception("Error at line " + top?.line + " near column " + top?.column + ". use := for assignment");             
            }
            /*maybe a bug*/
            assign_stmt.AddChild(exp());

            return assign_stmt;
        }

        private Node write_stmt()
        { 
            Node write_node=new Node(NodeType.WriteStmt);

            var top=peek();
            
            if(top.tokenValue=="write")
            { 
                write_node.AddChild(new Node(value:"write"));
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
            Node read_node=new Node(NodeType.ReadStmt);

            var top=peek();

            if(top.tokenValue=="read")
            { 
                read_node.AddChild(new Node(value:"read"));
                match();
            }
            else
            { 
                throw new Exception("Error at line " + top?.line + " near column " + top?.column + ". Syntax Error");
                   
            }
            top=peek();

            if(top.tokenType==TokenType.Identifier)
            { 
                read_node.AddChild(new Node(value:("Identifier ("+top.tokenValue+")")));
                match();
            }
            else
            { 
                throw new Exception("Error at line " + top?.line + " near column " + top?.column + ". Can't read Non-identifiers");
                   
            }
            return read_node;
        }
        private Node repeat_stmt()
        { 
            Node repeat_node=new Node(NodeType.RepeatStmt);
            var top=peek();
            if(top.tokenValue=="repeat")
            { 
                repeat_node.AddChild(new Node(value:"repeat"));
                match();
            }
            else
            {
                throw new Exception("Error at line " + top?.line + " near column " + top?.column + ". Syntax Error");
                   
            }
            repeat_node.AddChild(stmt_sequence());

            top=peek();
            if(top.tokenValue=="until")
            { 
                repeat_node.AddChild(new Node(value:"until");
                match();
            }
            else
            { 
                throw new Exception("Error at line " + top?.line + " near column " + top?.column + ". Syntax Error(expected until)");
            }
            repeat_node.AddChild(exp());

            return repeat_node;

        }
        private Node if_stmt()
        {
            Node if_node =new Node(NodeType.IfStmt);
            var top=peek();
            //if part
            if(top.tokenValue=="if")
            { 
                match();
                if_node.AddChild(new Node(value:"if"));
            }
            else
            { 
              throw new Exception("Error at line " + top?.line + " near column " + top?.column + ". Syntax Error");
            }
            //call exp procedure
            if_node.AddChild(exp());  

            //then part
            top.peek();
            if(top.tokenValue=="then")
            { 
                match();
                if_node.AddChild(new Node(value:"then"));
            }
            else
            {
                throw new Exception("Error at line " + top?.line + " near column " + top?.column + ". Syntax Error");
            }
            //stmt-seq call
            if_node.AddChild(stmt_sequence());
            //else part is optional either else or end 
            top=peek();
            if(top.tokenValue=="else")
            { 
                if_node.AddChild(new Node(value:"else"));
                match();
                if_node.AddChild(stmt_sequence());
            }
            top.peek(); //redundant if we dont have an else
            //end part
            if(top.tokenValue=="end")
            {
                if_node.AddChild(new Node(value:"end"));
                match();
            }
            else
            { 
                throw new Exception("Error at line " + top?.line + " near column " + top?.column + ". Syntax Error(missing end)");
            
            }
            return if_node;

        }
        private Node exp();
        private Node term();
        private Node simple_exp();
        private Node factor();
        private Node mulop();
        private Node addop();
        private Node comparisonop();
    }
}
