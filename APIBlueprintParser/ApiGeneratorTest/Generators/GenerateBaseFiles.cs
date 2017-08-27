//
// GenerateBaseFiles.cs
// 27.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace ApiGeneratorTest.Generators {
    
    public class GenerateBaseFiles {

        private struct Pathes {
            public static string TemplatesPath = Constants.RootDirectory + "/Templates/MainTemplates/";
            public static string AppsettingsDotDevelopes = TemplatesPath + "appsettings.Development.json";
            public static string Appsettings = TemplatesPath + "appsettings.json";
            public static string Program = TemplatesPath + "ProgramTeplate.cs";
            public static string Project = TemplatesPath + "project.template";
            public static string Solution = TemplatesPath + "Solution.template";
            public static string Stratups = TemplatesPath + "StartupsTemplate.cs";
            public static string Userprefs = TemplatesPath + "userprefs.template";
            public static string LaunchSettings = TemplatesPath + "launchSettings.json";
        }

        private struct Tokens {
            public const string ProjectName = "$projectname$";
        }

        private FolderStructureGenerator.FolderStrucureDescriptior _descriptor;

        public GenerateBaseFiles(FolderStructureGenerator.FolderStrucureDescriptior descriptor) {
            this._descriptor = descriptor;
        }

        public void Generate() {

            var solutionPath = $"{this._descriptor.MainDirectory}/{this._descriptor.ProjectName}.sln";
            var projectPath = $"{this._descriptor.ProjectDirectory}/{this._descriptor.ProjectName}.csproj";

            var appsetDevPath = $"{this._descriptor.ProjectDirectory}/appsettings.Development.json";
            var appsetPath = $"{this._descriptor.ProjectDirectory}/appsettings.json";

            var programPath = $"{this._descriptor.ProjectDirectory}/Program.cs";
            var startupPath = $"{this._descriptor.ProjectDirectory}/Startup.cs";

            var userprefsPath = $"{this._descriptor.MainDirectory}/{this._descriptor.ProjectName}.userprefs";

            var propertiesPath = $"{this._descriptor.PropertiesDirectory}/launchSettings.json";

            Dictionary<string, string> pathes = new Dictionary<string, string>() {
                {Pathes.Solution, solutionPath},
                {Pathes.Project, projectPath},
                {Pathes.Appsettings, appsetPath},
                {Pathes.AppsettingsDotDevelopes, appsetDevPath},
                {Pathes.Stratups, startupPath},
                {Pathes.Program, programPath},
                {Pathes.Userprefs, userprefsPath},
                {Pathes.LaunchSettings, propertiesPath}
            };

            foreach (var pair in pathes) {
                this.ReadFromFileAndWriteInAnotherFile(pair.Key, pair.Value, Tokens.ProjectName, this._descriptor.ProjectName);
            }
        }


        private void ReadFromFileAndWriteInAnotherFile(string readFrom, string writeTo, string replacePattern = null, string stringForReplace = null) {
            string content = File.ReadAllText(readFrom);

            if (replacePattern != null) {
                content = content.Replace(replacePattern, stringForReplace);
            }

            File.WriteAllText(writeTo, content);
        }
    }
}
