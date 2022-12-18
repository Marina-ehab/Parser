using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    internal class Scanner
    {
        List<Token> tokens = new List<Token>();
        private String tinyCode;
        private int index;
        public int line = 1;
        public int column = 1;
        private int returnIndex = 0;
        private static Dictionary<String, String>? symbolNameDict = null;
        private static HashSet<String>? reservedWords = null;

        public Scanner(String tinyCode)
        {
            this.tinyCode = tinyCode;
            index = 0;
        }

        public void GoBackTokens(int returnIndex)
        {
            this.returnIndex += returnIndex;
        }

        public Token? GetNextToken()
        {
            if (this.returnIndex > 0)
                return this.tokens[tokens.Count - returnIndex--];
            String state = "START";
            String lexeme = "";
            while ((index < tinyCode.Length) || state != "START")
            {
                switch (state)
                {
                    case "START":
                        if (tinyCode[index] == '{')
                        {
                            while (tinyCode[index] != '}')
                            {
                                if (tinyCode[index] == '\n')
                                {
                                    ++line;
                                    column = 1;
                                }
                                else
                                {
                                    ++column;
                                }
                                ++index;
                                if (index >= tinyCode.Length)
                                    throw new Exception("Error at line " + line + " near column " + column + ". Unclosed comment");
                            }
                            ++index;
                            ++column;
                        }
                        else if (tinyCode[index] == ' ' || tinyCode[index] == '\t' || tinyCode[index] == '\r')
                        {
                            //do nothing
                        }
                        else if (tinyCode[index] == '\n')
                        {
                            ++line;
                            column = 1;
                        }
                        else if (char.IsDigit(tinyCode[index]))
                        {
                            lexeme += tinyCode[index];
                            state = "INNUM";
                        }
                        else if ((tinyCode[index] >= 'A' && tinyCode[index] <= 'Z') || (tinyCode[index] >= 'a' && tinyCode[index] <= 'z'))
                        {
                            lexeme += tinyCode[index];
                            state = "INID";
                        }
                        else if (tinyCode[index] == ':')
                        {
                            lexeme += tinyCode[index];
                            state = "INASSIGN";
                        }
                        else if (GetSymbolDict().ContainsKey(tinyCode[index].ToString()))
                        {
                            ++index;
                            ++column;
                            tokens.Add(new Token(TokenType.Symbol, tinyCode[index - 1].ToString(), GetSymbolDict()[tinyCode[index - 1].ToString()], line, column - 1));
                            return tokens.Last();
                        }
                        else
                        {
                            throw new Exception("Error at line " + line + " near column " + column + ". Unknown character");
                        }
                        ++index;
                        ++column;
                        break;
                    case "INNUM":
                        while (index < tinyCode.Length && char.IsDigit(tinyCode[index]))
                        {
                            lexeme += tinyCode[index];
                            ++index;
                            ++column;
                        }
                        tokens.Add(new Token(TokenType.Number, lexeme, "Number", line, column - 1));
                        return tokens.Last();
                    case "INID":
                        while (index < tinyCode.Length && ((tinyCode[index] >= 'A' && tinyCode[index] <= 'Z') || (tinyCode[index] >= 'a' && tinyCode[index] <= 'z')))
                        {
                            lexeme += tinyCode[index];
                            ++index;
                            ++column;
                        }
                        if (GetReservedSet().Contains(lexeme))
                        {
                            tokens.Add(new Token(TokenType.Reserved, lexeme, lexeme.ToUpper(), line, column - 1));
                            return tokens.Last();
                        }
                        else
                        {
                            tokens.Add(new Token(TokenType.Identifier, lexeme, "Identifier", line, column - 1));
                            return tokens.Last();
                        }
                    case "INASSIGN":
                        if (index < tinyCode.Length && tinyCode[index] == '=')
                        {
                            lexeme += tinyCode[index];
                            ++index;
                            ++column;
                            tokens.Add(new Token(TokenType.Symbol, lexeme, GetSymbolDict()[lexeme], line, column - 1));
                            return tokens.Last();
                        }
                        else
                        {
                            throw new Exception("Error at line " + line + " near column " + column + ". Colon without equal sign");
                        }
                }
            }
            return null;
        }

        public static Dictionary<String, String> GetSymbolDict()
        {
            if (symbolNameDict == null)
            {
                symbolNameDict = new Dictionary<String, String>();
                symbolNameDict.Add(";", "SEMICOLON");
                symbolNameDict.Add(":=", "ASSIGN");
                symbolNameDict.Add("<", "LESSTHAN");
                symbolNameDict.Add("=", "EQUAL");
                symbolNameDict.Add("+", "PLUS");
                symbolNameDict.Add("-", "MINUS");
                symbolNameDict.Add("*", "MULT");
                symbolNameDict.Add("/", "DIV");
                symbolNameDict.Add("(", "OPENBRACKET");
                symbolNameDict.Add(")", "CLOSEDBRACKET");
            }
            return symbolNameDict;
        }

        public static HashSet<String> GetReservedSet()
        {
            if (reservedWords == null)
            {
                reservedWords = new HashSet<String>();
                reservedWords.Add("if");
                reservedWords.Add("then");
                reservedWords.Add("else");
                reservedWords.Add("end");
                reservedWords.Add("repeat");
                reservedWords.Add("until");
                reservedWords.Add("read");
                reservedWords.Add("write");
            }
            return reservedWords;
        }
    }
}
