using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace compiler
{
    public class Lexer
    {
        public string input;
        private int position;
        private char currentChar;
 

        //giriş ifadesini (input) ve karakter işleme konumunu (position) başlangıç değerleriyle ayarlar.
        public Lexer(string input)//ayrıştırılacak olan giriş ifadesini temsil eder.    
        {
            this.input = input;
            position = 0;//ayrıştırma işlemi sırasında hangi karakterin işlendiğini takip etmek için kullanılır.
            currentChar = input[position];
         

        }

        //ayrıştırma işlemi sırasında karakter işleme konumunu bir sonraki karaktere ilerletmek için kullanılır.
        private void Advance()
        {
            position++;// değişkenini bir artırarak karakter işleme konumunu bir sonraki karaktere geçirir.
            //position değeri, giriş metninin uzunluğundan küçükse,
            if (position < input.Length)
                currentChar = input[position];  //currentChar değişkenini input içindeki position indeksine karşılık gelen karakterle günceller.
            //Eğer position değeri, giriş metninin sonuna ulaşıldıysa (position değeri giriş metninin uzunluğuna eşitse), currentChar'ı boş karakter ('\0') olarak ayarlar.
            else
                currentChar = '\0'; // Eğer giriş metninin sonuna ulaşıldıysa, currentChar'ı boş karakter ('\0') olarak ayarla
        }
        private void SkipWhitespace()
        {
            while (currentChar != '\0' && char.IsWhiteSpace(currentChar))
            {
                Advance();
            }
        }

        private Token ParseNumber()
        {
            string number = "";

            while (position < input.Length && char.IsDigit(input[position]))
            {
                number += input[position];
                Advance();
            }

            return new Token(TokenType.Number, number);
        }

        private Token ParseIdentifier()
        {
            StringBuilder sb = new StringBuilder();

            while (currentChar != '\0' && (char.IsLetter(currentChar) || char.IsDigit(currentChar)))
            {
                sb.Append(currentChar);
                Advance();
            }

            return new Token(TokenType.Identifier, sb.ToString());
        }
    

        public Token GetNextToken()
        {
            while (currentChar != '\0')
            {
                if (char.IsWhiteSpace(currentChar))
                {
                    SkipWhitespace();
                    continue;
                }

                if (char.IsDigit(currentChar))
                {
                    return ParseNumber();
                }

                if (char.IsLetter(currentChar))
                {
                    return ParseIdentifier();
                }

                switch (currentChar)
                {
                    case '+':
                        Advance();
                        return new Token(TokenType.Plus, "+");
                    case '-':
                        Advance();
                        return new Token(TokenType.Minus, "-");
                    case '*':
                        Advance();
                        return new Token(TokenType.Multiply, "*");
                    case '/':
                        Advance();
                        if (currentChar == '/')
                        {
                            Advance();
                            return GenerateComment();
                        }
                        return new Token(TokenType.Divide, "/");
                    case '^':
                        Advance();
                        return new Token(TokenType.Power, "^");
                    case '%':
                        Advance();
                        return new Token(TokenType.Modulo, "%");
                    case '(':
                        Advance();
                        return new Token(TokenType.LeftParen, "(");
                    case ')':
                        Advance();
                        return new Token(TokenType.RightParen, ")");
                    case '{':
                        Advance();
                        return new Token(TokenType.LeftBrace, "{");
                    case '}':
                        Advance();
                        return new Token(TokenType.RightBrace, "}");
                    case '<':
                        Advance();
                        if (currentChar == '=')
                        {
                            Advance();
                            return new Token(TokenType.LessThanOrEqual, "<=");
                        }
                        return new Token(TokenType.LessThan, "<");
                    case '>':
                        Advance();
                        if (currentChar == '=')
                        {
                            Advance();
                            return new Token(TokenType.GreaterThanOrEqual, ">=");
                        }
                        return new Token(TokenType.GreaterThan, ">");
                    case '=':
                        Advance();
                        if (currentChar == '=')
                        {
                            Advance();
                            return new Token(TokenType.Equal, "==");
                        }
                        return new Token(TokenType.Assign, "=");
                    case '!':
                        Advance();
                        if (currentChar == '=')
                        {
                            Advance();
                            return new Token(TokenType.NotEqual, "!=");
                        }
                        throw new Exception("Invalid character: " + currentChar);
                    case ';':
                        Advance();
                        return new Token(TokenType.Semicolon, ";");
                   
                    default:
                        throw new Exception("Invalid character: " + currentChar);
                }
            }

            return new Token(TokenType.EOF, "");
        }


        private readonly Dictionary<string, dynamic> variables = new Dictionary<string, dynamic>();

      
        private Token GenerateComment()
        {
            string comment = "";

            while (currentChar != '\0' && currentChar != '\n')
            {
                comment += currentChar;
                Advance();
            }

            return new Token(TokenType.Comment, comment);
        }

        //private string ReadKeyword()
        //{
        //    StringBuilder builder = new StringBuilder();//anahtar kelimeyi birleştirmek için kullanılır.

        //    while (Char.IsLetter(currentChar))//Metod, currentChar değişkeninin bir harf karakteri olduğu sürece döngüye girer.
        //    {
        //        //Döngü her döndüğünde, currentChar karakterini builder nesnesine ekler ve bir sonraki karaktere geçmek için Advance yöntemini çağırır.
        //        builder.Append(currentChar);
        //        Advance();
        //    }

        //    return builder.ToString();
        //}



    }
}

