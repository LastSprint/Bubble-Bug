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
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;

namespace ApiGeneratorTest.Generators {
    public class GenerateBaseFiles {

        private struct Tokens {
            public const string ProjectName = "$projectname$";
            public const string Ip = "$ip$";
            public const string Port = "$port$";
        }

        private FolderStructureGenerator.FolderStrucureDescriptior _descriptor;
        private int Port;

        public GenerateBaseFiles(FolderStructureGenerator.FolderStrucureDescriptior descriptor, int port) {
            this._descriptor = descriptor;
            this.Port = port;
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

			var modelPath = $"{this._descriptor.ProjectDirectory}/EquatableRequaest.cs";
            var jsonComparerPath = $"{this._descriptor.ProjectDirectory}/JsonCommparer.cs";
            var supportPath = $"{this._descriptor.ProjectDirectory}/Support.cs";

            Dictionary<string, string> pathes = new Dictionary<string, string>() {
                {TemplateResource.Solution, solutionPath},
                {TemplateResource.project, projectPath},
                {TemplateResource.appsettings, appsetPath},
                {TemplateResource.appsettings_Development, appsetDevPath},
                {TemplateResource.Startups, startupPath},
                {TemplateResource.Program, programPath},
                {TemplateResource.userprefs, userprefsPath},
                {TemplateResource.Model, modelPath},
                {TemplateResource.JsonComparer, jsonComparerPath}
            };

            foreach (var pair in pathes) {
                this.ReadFromFileAndWriteInAnotherFile(pair.Key, pair.Value, Tokens.ProjectName, this._descriptor.ProjectName);
            }

            string content = TemplateResource.Support;
			
			content = content.Replace(Tokens.ProjectName, this._descriptor.ProjectName);
            content = content.Replace("$PathToController$", this._descriptor.ControllerDirectory);

			File.WriteAllText(supportPath, content);

            content = TemplateResource.launchSettings;
            content = content.Replace(Tokens.Ip, "192.168.0.201");
            content = content.Replace(Tokens.Port, this.Port.ToString());

            File.WriteAllText(propertiesPath, content);
        }


        private void ReadFromFileAndWriteInAnotherFile(string content, string writeTo, string replacePattern = null, string stringForReplace = null) {

            if (replacePattern != null) {
                content = content.Replace(replacePattern, stringForReplace);
            }

            File.WriteAllText(writeTo, content);
        }
    }
}
