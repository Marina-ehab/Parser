﻿using System;
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
        private Node write_stmt();
        private Node read_stmt();
        private Node repeat_stmt();
        private Node if_stmt();
        private Node exp();
        private Node term();
        private Node simple_exp();
        private Node factor();
        private Node mulop();
        private Node addop();
        private Node comparisonop();
    }
}
