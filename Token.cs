using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace compiler
{
        public class Token
        {
        private TokenType ıNT;
        private int intValue;
        private TokenType fLOAT;
        private float floatValue;

        public TokenType Type { get; private set; }
            public string Value { get; private set; }

        //Type özelliği, tokenin türünü temsil eden bir TokenType değeri döndürür.
        //Bu, sayı, operatör, parantez, döngü anahtar kelimesi gibi farklı türleri belirtmek için kullanılır.
        //Value özelliği, tokenin değerini temsil eden bir metin dizesi döndürür.
        //Örneğin, bir sayı için değer "42", bir operatör için değer "+" olabilir.
        public Token(TokenType type, string value)
            {
                Type = type;
                Value = value;
            }

        public Token(TokenType ıNT, int intValue)
        {
            this.ıNT = ıNT;
            this.intValue = intValue;
        }

        public Token(TokenType fLOAT, float floatValue)
        {
            this.fLOAT = fLOAT;
            this.floatValue = floatValue;
        }

        //ToString metodunda, Token nesnesinin türünü ve değerini içeren bir dize döndürülür.
        //Örneğin, "Token(Number, 42)" veya "Token(Plus, +)" gibi bir çıktı elde edilebilir.
        public override string ToString()
            {
                return $"Token({Type}, {Value})";
            }
        }
}
