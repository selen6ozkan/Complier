using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace compiler
{

    //TokenType adında bir numaralandırma (enum) tanımlar.
    //Bu enum, farklı token türlerini temsil etmek için kullanılır.
    public enum TokenType
    {
        Number,//sayı
        Plus,//artı 
        Minus,//eksi 
        Multiply,//çarpı 
        Divide,//bölü 
        Modulo,//mod  
        Power,//kuvvet
        LeftParen,//sol parantez
        RightParen,//sağ parantez
        ComparisonOperator, //Karşılaştırma operatörü
        Semicolon,//noktalı virgül 
        Comment,//acıklama
        While,//while döngüsü 
        For,// for döngüsü 
        KeyWord,//anahtar kelime
        Identifier,//tanımlayıcı 
        Assign,//atama işareti
        LeftBrace,//sol süslü parantez 
        RightBrace,//sağ süslü parantez 
        LessThan,//küçüktür
        GreaterThan,//büyüktür 
        LessThanOrEqual,//küçük eşittir 
        GreaterThanOrEqual,//büyük eşittir
        Equal,//eşit
        NotEqual,//eşit değil 
        EndOfFile,//dosya sonu
        If,
        Else,
        StartsWith,
        EndsWith,
        Int,
        Float,
        EOF,//dosya sonu
    

    }
}
