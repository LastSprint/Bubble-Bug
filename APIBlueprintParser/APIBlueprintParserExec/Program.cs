//
// Program.cs
// 26.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//
using System;
using System.IO;
using APIBlueprintParser.Parsers.Action;

namespace APIBlueprintParserExec {
    
    class MainClass {
        
        public static void Main(string[] args) {
            var stream = new StreamReader(Console.OpenStandardInput());

            //var parser = new ResourceActionParser(stream).Parse();

            //Console.WriteLine(parser.ToString());
            Console.ReadKey();
        }
    }
}
