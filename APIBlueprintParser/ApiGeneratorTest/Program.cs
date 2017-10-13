//
// Program.cs
// 27.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//
using System;
using System.IO;
using APIBlueprintParser.Parsers;
using ApiGeneratorTest.Generators;
using ApiGeneratorTest.Generators.SourceGenerators;
using System.Diagnostics;

namespace ApiGeneratorTest {

    class MainClass {

		public static void Main(string[] args) {

			try
			{
				IntitPoint();
			}
			catch (Exception e)
			{
				Console.WriteLine();
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Has error occured");
				Console.ResetColor();
				Console.WriteLine(e);
			    Console.ReadKey();
			}
		}

		private static void IntitPoint()
		{
			Console.WriteLine("Input path to *.apib file");

			var path = Console.ReadLine();

			var stream = File.OpenRead(path);

			Console.WriteLine("Start parsing");

			var parser = new MainParser(new StreamReader(stream)).Parse();

			Console.WriteLine("Parsing end");

            Console.WriteLine("Start generation of folder structure");


            var projectName = parser.Projectname;

            if (parser.Projectname == null) {
                do {

                    projectName = Console.ReadLine().Trim();

                } while (projectName.Length == 0);
            }

            var generator = new FolderStructureGenerator(projectName);
            var descriptor = generator.Generate();
            var port = new Manager().AddNewServer(projectName);
            Console.WriteLine("Folder structure generation finished");
            Console.WriteLine("File strcture generation starting");

            new GenerateBaseFiles(descriptor, port).Generate();

            Console.WriteLine("File strcture generation finished");

            Console.WriteLine("Needs to generate default mocks? y/n");

            var readedFlag = Console.ReadLine().ToLower() == "y";

            Console.WriteLine("Start generate source code");

            new ApiGenerator(parser, descriptor).Generate(readedFlag);

            Console.WriteLine("Source code generation finished");

            Deploy(descriptor);
		}

        private static void Deploy(FolderStructureGenerator.FolderStrucureDescriptior descriptor) {
            
			Console.WriteLine("You want to build and deply this project? (y/n)");

			var answer = Console.ReadLine();

			if (answer.Trim().ToLower() != "y")
			{
				return;
			}

			var consoleStandartIn = Console.In;
			var consoleStandartOut = Console.Out;

			Process proc = new Process();
			proc.StartInfo = new ProcessStartInfo("dotnet");
			proc.StartInfo.WorkingDirectory = descriptor.MainDirectory;
			proc.StartInfo.CreateNoWindow = true;
			proc.StartInfo.UseShellExecute = false;
			proc.StartInfo.RedirectStandardInput = true;
			proc.StartInfo.Arguments = "restore";
			proc.Start();
			Console.SetOut(proc.StandardInput);

			proc.WaitForExit();

			Console.SetOut(consoleStandartOut);

			proc = new Process();
			proc.StartInfo = new ProcessStartInfo("dotnet");
			proc.StartInfo.WorkingDirectory = descriptor.MainDirectory;
			proc.StartInfo.CreateNoWindow = true;
			proc.StartInfo.UseShellExecute = false;
			proc.StartInfo.RedirectStandardInput = true;
			proc.StartInfo.Arguments = "build";
			proc.Start();
			Console.SetOut(proc.StandardInput);

			proc.WaitForExit();

			Console.SetOut(consoleStandartOut);

			proc = new Process();
			proc.StartInfo = new ProcessStartInfo("dotnet");
			proc.StartInfo.CreateNoWindow = false;
			proc.StartInfo.WorkingDirectory = descriptor.ProjectDirectory;
			proc.StartInfo.Arguments = $"run --urls http://192.168.0.221:50012/";
			proc.Start();

			Console.WriteLine("API service was deployed.");

			Console.ReadKey();
        }
    }
}
