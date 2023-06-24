using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using compiler;

namespace compiler
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter an expression:");
            string input = Console.ReadLine();

            //ifadenin dilbilgisel analizinin yapılmasını sağlar.
            //gelen ifadeyi sembollerine ayırır ve belirli dilbilgisi kurallarına göre
            //ifadenin bileşenlerini tanımlar.
            Lexer lexer = new Lexer(input);
            
            //fadenin syntax analizinin yapılmasını sağlar.
            Parser parser = new Parser(lexer);
         

            try
            {
                int result = parser.ParseExpression();
                Console.WriteLine("Result: " + result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            Console.ReadLine();
        }
    }
}
