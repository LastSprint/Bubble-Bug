//
// Program.cs
// 26.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//
using System;
using System.IO;
using APIBlueprintParser.Parsers;
using Newtonsoft.Json;

namespace APIBlueprintParserExec {
    
    class MainClass {
        
        public static void Main(string[] args) {

            try {
                IntitPoint();
            } catch (Exception e) {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Has error occured");
                Console.ResetColor();
                Console.WriteLine(e);
            }

        }

        private static void IntitPoint() {
			Console.WriteLine("Input path to *.apib file");

			var path = Console.ReadLine();

			var stream = File.OpenRead(path);

			Console.WriteLine("Start parsing");

			var parser = new MainParser(new StreamReader(stream)).Parse();
			var str = JsonConvert.SerializeObject(parser);

			Console.WriteLine("Pasing end");
			Console.WriteLine("Input path to output file");

			var outPath = Console.ReadLine();

			File.WriteAllText(outPath, str);

			Console.WriteLine("Writing succesfully ended");

			Console.ReadKey();
        }
    }
}
