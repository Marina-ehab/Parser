using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    internal class Scanner
    {
        private String tinyCode;
        private int index;
        private int line = 1;
        private int column = 1;
        private static Dictionary<String, String>? symbolNameDict = null;
        private static HashSet<String>? reservedWords = null;

        public Scanner(String tinyCode)
        {
            this.tinyCode = tinyCode;
            index = 0;
        }

        public Token? GetNextToken()
        {
            String state = "START";
            String lexeme = "";
            while (index < tinyCode.Length)
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
                            }
                            ++index;
                            ++column;
                        }
                        if (tinyCode[index] == ' ' || tinyCode[index] == '\t' || tinyCode[index] == '\r')
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
                            return new Token(TokenType.Symbol, tinyCode[index - 1].ToString(), GetSymbolDict()[tinyCode[index - 1].ToString()]);
                        }
                        else
                        {
                            throw new Exception("Error at line " + line + " near column " + column);
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
                        return new Token(TokenType.Number, lexeme, "Number");
                    case "INID":
                        while (index < tinyCode.Length && ((tinyCode[index] >= 'A' && tinyCode[index] <= 'Z') || (tinyCode[index] >= 'a' && tinyCode[index] <= 'z')))
                        {
                            lexeme += tinyCode[index];
                            ++index;
                            ++column;
                        }
                        if (GetReservedSet().Contains(lexeme)) return new Token(TokenType.Reserved, lexeme, lexeme.ToUpper());
                        else return new Token(TokenType.Identifier, lexeme, "Identifier");
                    case "INASSIGN":
                        if (tinyCode[index] == '=')
                        {
                            lexeme += tinyCode[index];
                            ++index;
                            ++column;
                            return new Token(TokenType.Symbol, lexeme, GetSymbolDict()[lexeme]);
                        }
                        else
                        {
                            throw new Exception("Error at line " + line + " near column " + column);
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
