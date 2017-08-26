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
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace APIBlueprintParserExec {
    
    class MainClass {
        
        public static void Main(string[] args) {
            var stream = File.OpenRead("/Users/aleksandrkravcenkov/Repo/APIBlueprintParser/APIBlueprintParser/APIBlueprintIntegrationTests/ApiDescription/valid.apib");

            var parser = new MainParser(new StreamReader(stream)).Parse();

			Console.WriteLine(parser.ToString());

			var str = JsonConvert.SerializeObject(parser);

            Console.WriteLine();
            Console.WriteLine(str);

            File.WriteAllText("/Users/aleksandrkravcenkov/test.json", str);

            Console.ReadKey();
        }
    }
}
