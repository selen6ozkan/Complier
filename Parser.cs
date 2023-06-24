using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace compiler
{
    public class Parser
    {
        public Lexer lexer;// dilbilgisel analiz işlemlerini gerçekleştirmek için
        public Token currentToken;//Token türünde currentToken adlı özel bir değişken tanımlanır. Bu değişken, işlenen ifadenin mevcut sembolünü temsil eder. 
        public int tok_idx=0;
        public Dictionary<string, int> variables;//Bu sözlük, değişken adlarını ve bu değişkenlere atanmış değerleri içerir.
        public List<Token> tokens;

        public Parser(Lexer lexer)
        {
            // sınıfının diğer metodlarında kullanılabilir 
            this.lexer = lexer;
            //Bu, syntax analizi ve ifadeyi parçalama işlemlerinde mevcut sembolün takibini sağlar.
            currentToken = lexer.GetNextToken();
            //ifadedeki değişken adlarını ve atanan değerleri saklamak için kullanılan bir veri yapısıdır.
            variables = new Dictionary<string, int>();
        }

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
            this.tok_idx = -1;
        }

      
        private Token Eat(TokenType tokenType) //Eat metodu, currentToken'ın belirtilen tokenType ile eşleşip eşleşmediğini kontrol eder.
        {
            //Eşleşme durumunda, mevcut sembolü alır, bir sonraki sembole geçer ve alınan sembolü geri döndürür.
            if (currentToken.Type == tokenType)
            {
                Token token = currentToken;
                currentToken = lexer.GetNextToken();
                return token;
            }
            //Eşleşme olmazsa, bir hata fırlatılır. 
            else
            {
                throw new Exception("Invalid syntax");
            }
        }



        //Bu metod, matematiksel ifadelerin toplama ve çıkarma işlemlerini gerçekleştirmek için kullanılır.
        //İfadeyi terimlere bölerek her bir terimi analiz eder ve ardından toplama veya çıkarma sembolüne göre işlem yapar.
        //Döngüyü kullanarak ifadenin tamamını analiz eder ve sonuç olarak toplamı veya farkı elde eder
        public int ParseExpression()
        {
            int result = ParseTerm();// bir terimi analiz eder ve bu terimin sonucunu result değişkenine atar.

            //currentToken'ın TokenType.Plus veya TokenType.Minus sembolüne eşit olup olmadığını kontrol eder.
            while (currentToken.Type == TokenType.Plus || currentToken.Type == TokenType.Minus)
            {
                Token op = currentToken;//op adlı bir Token değişkeni oluşturulur ve değeri currentToken olarak atanır.

                if (op.Type == TokenType.Plus)
                {
                    //currentToken'ın TokenType.Plus sembolüne eşit olduğu doğrulanır.
                    Eat(TokenType.Plus);
                    //bir sonraki terim analiz edilir ve bu terimin değeri result'a eklenir.
                    result += ParseTerm();
                }
                else if (op.Type == TokenType.Minus)
                {
                    //currentToken'ın TokenType.Minus sembolüne eşit olduğu doğrulanır.
                    Eat(TokenType.Minus);
                    //bir sonraki terim analiz edilir ve bu terimin değeri result'dan çıkarılır.
                    result -= ParseTerm();
                }
            }

            return result;
        }


        //Bu metod, matematiksel ifadelerdeki çarpma, bölme ve mod alma işlemlerini gerçekleştirir.
        //İfadeyi faktörlere bölerek her bir faktörü analiz eder ve ardından çarpma, bölme veya mod sembolüne göre ilgili işlemi yapar.
        //Döngüyü kullanarak ifadenin tamamını analiz eder ve sonuç olarak çarpım, bölüm veya mod sonucunu elde eder.

        private int ParseTerm()
        {
            int result = ParseFactor();

            while (currentToken.Type == TokenType.Multiply || currentToken.Type == TokenType.Divide || currentToken.Type == TokenType.Modulo)
            {
                Token op = currentToken;

                if (op.Type == TokenType.Multiply)
                {
                    Eat(TokenType.Multiply);
                    result *= ParseFactor();
                }
                else if (op.Type == TokenType.Divide)
                {
                    Eat(TokenType.Divide);
                    result /= ParseFactor();
                }
                else if (op.Type == TokenType.Modulo)
                {
                    Eat(TokenType.Modulo);
                    result %= ParseFactor();
                }
            }

            return result;
        }


        //matematiksel ifadelerdeki faktörleri analiz etmek için kullanılır.
        //Faktör, bir sayı, negatif bir faktör veya parantez içinde bir ifade olabilir.
        //Sembolün türüne göre ilgili işlem yapılır ve sonuç döndürülür.
        private int ParseFactor()
        {
            Token token = currentToken;

            if (token.Type == TokenType.Number)
            {
                Eat(TokenType.Number);
                return int.Parse(token.Value);
            }
            else if (token.Type == TokenType.Minus)
            {
                Eat(TokenType.Minus);
                return -ParseFactor();//bir sonraki faktör negatif olarak analiz edilir ve negatif değeri döndürülür.
            }
            else if (token.Type == TokenType.LeftParen)
            {
                Eat(TokenType.LeftParen);
                int result = ParseExpression();
                Eat(TokenType.RightParen);
                return result;
            }

            throw new Exception("Invalid syntax");
        }


        //Bu metod, belirli bir karşılaştırma operatörü ile iki tamsayı değeri arasındaki koşulu değerlendirir.
        //Karşılaştırma operatörüne bağlı olarak, karşılaştırma sonucu true veya false olarak döndürülür.


        //private bool EvaluateCondition(int leftValue, TokenType comparisonOperator, int rightValue)
        //{
        //    switch (comparisonOperator)
        //    {
        //        case TokenType.LessThan:
        //            return leftValue < rightValue;
        //        case TokenType.GreaterThan:
        //            return leftValue > rightValue;
        //        case TokenType.LessThanOrEqual:
        //            return leftValue <= rightValue;
        //        case TokenType.GreaterThanOrEqual:
        //            return leftValue >= rightValue;
        //        case TokenType.Equal:
        //            return leftValue == rightValue;
        //        case TokenType.NotEqual:
        //            return leftValue != rightValue;
        //        default:
        //            throw new Exception("Invalid comparison operator");
        //    }
        //}






        //Bu metod, belirli bir karşılaştırma operatörü ile iki ifade arasındaki koşulu ayrıştırmak ve değerlendirmek için kullanılır.
        //Sol ve sağ ifadeleri ayrıştırarak, karşılaştırma operatörüne göre ilgili karşılaştırmayı yapar ve sonucu döndürür.
        private bool ParseCondition()
        {
            //sol ifadeyi ParseExpression metoduyla analiz eder ve değeri leftValue değişkenine atar.
            int leftValue = ParseExpression();
            Token comparisonOperator = currentToken;

            if (comparisonOperator.Type == TokenType.LessThan)
            {
                Eat(TokenType.LessThan);
                int rightValue = ParseExpression();
                return leftValue < rightValue;
            }
            else if (comparisonOperator.Type == TokenType.GreaterThan)
            {
                Eat(TokenType.GreaterThan);
                int rightValue = ParseExpression();
                return leftValue > rightValue;
            }
            else if (comparisonOperator.Type == TokenType.LessThanOrEqual)
            {
                Eat(TokenType.LessThanOrEqual);
                int rightValue = ParseExpression();
                return leftValue <= rightValue;
            }
            else if (comparisonOperator.Type == TokenType.GreaterThanOrEqual)
            {
                Eat(TokenType.GreaterThanOrEqual);
                int rightValue = ParseExpression();
                return leftValue >= rightValue;
            }
            else if (comparisonOperator.Type == TokenType.Equal)
            {
                Eat(TokenType.Equal);
                int rightValue = ParseExpression();
                return leftValue == rightValue;
            }
            else if (comparisonOperator.Type == TokenType.NotEqual)
            {
                Eat(TokenType.NotEqual);
                int rightValue = ParseExpression();
                return leftValue != rightValue;
            }
            else
            {
                throw new Exception("Invalid syntax: comparison operator expected.");
            }
        }

        //Bu metod, belirli bir ikili operatör ile sol ve sağ operandlar arasındaki işlemi uygulamak için kullanılır.
        //İkili operatöre bağlı olarak, ilgili işlem gerçekleştirilir ve sonuç döndürülür.




        //private int ApplyBinaryOperation(int leftValue, string operatorType, int rightValue)
        //{
        //    switch (operatorType)
        //    {
        //        case "+":
        //            return leftValue + rightValue;
        //        case "-":
        //            return leftValue - rightValue;
        //        case "*":
        //            return leftValue * rightValue;
        //        case "/":
        //            return leftValue / rightValue;
        //        case "%":
        //            return leftValue % rightValue;
        //        default:
        //            throw new Exception("Invalid operator: " + operatorType);
        //    }
        //} 








        //private void ParseForLoop()
        //{
        //    Eat(TokenType.For);//"for" anahtar kelimesi

        //    Eat(TokenType.LeftParen);//döngünün başlangıcını belirtir.

        //    // Döngü değişkeni
        //    string variableName = Eat(TokenType.Identifier).Value;

        //    Eat(TokenType.Assign);

        //    // Döngü başlangıç değeri
        //    int startValue = ParseExpression();

        //    Eat(TokenType.Semicolon);

        //    // Döngü bitiş koşulu
        //    bool condition = ParseCondition();

        //    Eat(TokenType.Semicolon);

        //    // Döngü adımı
        //    string stepOperation = Eat(TokenType.Identifier).Value;

        //    Eat(TokenType.Assign);

        //    int stepValue = ParseExpression();

        //    Eat(TokenType.RightParen);

        //    // Döngü gövdesi
        //    Eat(TokenType.LeftBrace);

        //    while (condition)
        //    {
        //        // Döngü değişkenini güncelle
        //        variables[variableName] = startValue;

        //        // Döngü gövdesini derle
        //        ParseStatement();

        //        // Döngü adımını uygula
        //        int result = ApplyBinaryOperation(variables[variableName], stepOperation, stepValue);
        //        variables[variableName] = result;

        //        // Döngü koşulunu kontrol et
        //        condition = EvaluateCondition(variables[variableName], TokenType.LessThanOrEqual, startValue);
        //    }

        //    Eat(TokenType.RightBrace);
        //}




        public void ParseForLoop()
        {

          
            // "for" anahtar kelimesini kontrol et
            Token token = currentToken; // Değişiklik: lexer.GetNextToken() -> GetNextToken()
            if (token.Type != TokenType.Identifier || token.Value != "for")
            {
                throw new Exception("Syntax Error: 'for' keyword expected.");
            }
            token = currentToken;

            // "(" parantezini kontrol et

            if (token.Type != TokenType.LeftParen)
            {
                throw new Exception("Syntax Error: '(' expected.");
            }



            //token = currentToken;
            //if (token.Type != TokenType.dataType || token.Value != "int")
            //{
            //    throw new Exception("Syntax Error: 'int' keyword expected.");
            //}



            // İlk atama ifadesini işle

            if (token.Type != TokenType.Identifier)
            {
                throw new Exception("Syntax Error: Variable name expected.");
            }
            

            if (token.Type != TokenType.Assign)
            {
                throw new Exception("Syntax Error: '=' expected.");
            }

            // Başlangıç değerini işle

            if (token.Type != TokenType.Number)
            {
                throw new Exception("Syntax Error: Numeric value expected.");
            }
          

            // ";" noktalı virgülü kontrol et

            if (token.Type != TokenType.Semicolon)
            {
                throw new Exception("Syntax Error: ';' expected.");
            }

            // Koşul ifadesini işle

            if (token.Type != TokenType.Identifier)
            {
                throw new Exception("Syntax Error: Variable name expected.");
            }
          

            if (token.Type != TokenType.LessThan && token.Type != TokenType.LessThanOrEqual && token.Type != TokenType.GreaterThan && token.Type != TokenType.GreaterThanOrEqual)
            {
                throw new Exception("Syntax Error: Comparison operator expected.");
            }
            //string comparisonOperator = token.Value;


            if (token.Type != TokenType.Number)
            {
                throw new Exception("Syntax Error: Numeric value expected.");
            }
           

            // ";" noktalı virgülü kontrol et

            if (token.Type != TokenType.Semicolon)
            {
                throw new Exception("Syntax Error: ';' expected.");
            }

            // Artış ifadesini işle
            if (token.Type != TokenType.Identifier)
            {
                throw new Exception("Syntax Error: Variable name expected.");
            }
            //string incrementVariable = token.Value;

            if (token.Type != TokenType.Assign)
            {
                throw new Exception("Syntax Error: '=' expected.");
            }

            // Artış değerini işle
            if (token.Type != TokenType.Number)
            {
                throw new Exception("Syntax Error: Numeric value expected.");
            }
            //int incrementValue = int.Parse(token.Value);

            // ")" parantezini kontrol et
            if (token.Type != TokenType.RightParen)
            {
                throw new Exception("Syntax Error: ')' expected.");
            }

            // "{" süslü parantezi kontrol et
            if (token.Type != TokenType.LeftBrace)
            {
                throw new Exception("Syntax Error: '{' expected.");
            }

            // For döngüsü içerisindeki ifadeleri işle
            // Burada for döngüsünün içeriğini nasıl işleyeceğiniz size bağlıdır.

            // "}" süslü parantezi kontrol et
            if (token.Type != TokenType.RightBrace)
            {
                throw new Exception("Syntax Error: '}' expected.");
            }
        }

      



        //Bu metod, mevcut tokenin türüne göre farklı işlemleri yönlendirerek ifadeleri ayrıştırır ve çalıştırır.
        //Bu işlemler arasında "for" döngüsü, "while" döngüsü ve atama ifadesi bulunmaktadır.
        private void ParseStatement()
        {
            //mevcut tokenin türü TokenType.For ise,
            //ParseForLoop metodunu çağırarak bir "for" döngüsünü ayrıştırır ve çalıştırır.
            if (currentToken.Type == TokenType.For)
            {
                ParseForLoop();
            }
            //Eğer mevcut tokenin türü TokenType.While ise,
            //ParseWhileLoop metodunu çağırarak bir "while" döngüsünü ayrıştırır ve çalıştırır.
            else if (currentToken.Type == TokenType.While)
            {
                ParseWhileLoop();
            }
            //Eğer mevcut tokenin türü TokenType.Identifier ise,
            //ParseAssignment metodunu çağırarak bir atama ifadesini ayrıştırır ve çalıştırır.
            else if (currentToken.Type == TokenType.Identifier)
            {
                ParseAssignment();
            }
            else
            {
                throw new Exception("Invalid statement");
            }
        }

        //Bu metod, bir atama ifadesini ayrıştırır ve değişkenin değerini belirlenen ifadeye eşitler.
        private int ParseAssignment()
        {
            string variableName = Eat(TokenType.Identifier).Value;

            //Eşittir işaretini (TokenType.Assign) ayrıştırır ve yutar.
            Eat(TokenType.Assign);

            int value = ParseExpression();
            
            //Değişken adı ile ifadenin değerini variables adlı sözlüğe atar.
            variables[variableName] = value;

            return value;
        }


        //Bu metod, "while" döngüsünü ayrıştırır ve belirtilen koşul doğru olduğu sürece döngü gövdesini tekrar tekrar çalıştırır.
        private void ParseWhileLoop()
        {
            Eat(TokenType.While);

            Eat(TokenType.LeftParen);

            // Döngü koşulu
            bool condition = ParseCondition();

            Eat(TokenType.RightParen);

            // Döngü gövdesi
            Eat(TokenType.LeftBrace);

            while (condition)
            {
                // Döngü gövdesini derle
                ParseStatement();

                // Döngü koşulunu kontrol et
                condition = ParseCondition();
            }

            Eat(TokenType.RightBrace);
        }

    }
}

