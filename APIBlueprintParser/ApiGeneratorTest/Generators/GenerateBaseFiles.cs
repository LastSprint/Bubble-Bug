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

			var modelPath = $"{this._descriptor.ProjectDirectory}/EquatableRequaest.cs";

            var supportPath = $"{this._descriptor.ProjectDirectory}/Support.cs";

            Dictionary<string, string> pathes = new Dictionary<string, string>() {
                {TemplateResource.Solution, solutionPath},
                {TemplateResource.project, projectPath},
                {TemplateResource.appsettings, appsetPath},
                {TemplateResource.appsettings_Development, appsetDevPath},
                {TemplateResource.Startups, startupPath},
                {TemplateResource.Program, programPath},
                {TemplateResource.userprefs, userprefsPath},
                {TemplateResource.launchSettings, propertiesPath},
				{TemplateResource.Model, modelPath}
            };

            foreach (var pair in pathes) {
                this.ReadFromFileAndWriteInAnotherFile(pair.Key, pair.Value, Tokens.ProjectName, this._descriptor.ProjectName);
            }

            string content = File.ReadAllText(Pathes.Support);
			
			content = content.Replace(Tokens.ProjectName, this._descriptor.ProjectName);
            content = content.Replace("$PathToController$", this._descriptor.ControllerDirectory);

			File.WriteAllText(supportPath, content);
        }


        private void ReadFromFileAndWriteInAnotherFile(string content, string writeTo, string replacePattern = null, string stringForReplace = null) {

            if (replacePattern != null) {
                content = content.Replace(replacePattern, stringForReplace);
            }

            File.WriteAllText(writeTo, content);
        }
    }
}
