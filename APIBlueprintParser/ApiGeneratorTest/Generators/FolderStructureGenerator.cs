//
// FolderStrucctureGenerator.cs
// 27.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//
using System;
using System.IO;

namespace ApiGeneratorTest.Generators {
    
    public class FolderStructureGenerator {

        public struct FolderStrucureDescriptior {

            /// <summary>
            ///  Name of the curernt project
            /// </summary>
            public string ProjectName { get; }

            /// <summary>
            /// Directory that contains *.sln and *.userprefs files
            /// </summary>
            public string MainDirectory { get; }

            /// <summary>
            /// Directory that included in the root project directiry
            /// </summary>
            public string ProjectDirectory { get; }

            /// <summary>
            /// Directory that contains Api controllers
            /// </summary>
            public string ControllerDirectory { get; }

            /// <summary>
            /// Directory that contains solution properties
            /// </summary>
            public string PropertiesDirectory { get; }

            /// <summary>
            /// Path to wwwroot directory.
            /// </summary>
            /// <value>The www root directory.</value>
            public string WwwRootDirectory { get; }

			public string MocksDirectory { get; }

            public FolderStrucureDescriptior(string projectName, string mainDirectory, string projectDir, string controllersDir, string propertiesDir, string wwwrootDir, string mocksDirectory) {
                this.ProjectName = projectName;
                this.MainDirectory = mainDirectory;
                this.ProjectDirectory = projectDir;
                this.ControllerDirectory = controllersDir;
                this.PropertiesDirectory = propertiesDir;
                this.WwwRootDirectory = wwwrootDir;
				this.MocksDirectory = mocksDirectory;
            }

        }

        private struct Consts {
            public const string ProjectName = "$projectnname$";

            public const string ControllersDiretoryName = "/Controllers";
            public const string PropertiesDirectoryName = "/Properties";
            public const string WwwRootDirectoryName = "/wwwroot";
            public const string MocksFolder = "/Mocks";
        }

        private string _projectName;

        public FolderStructureGenerator(string projectName) {
            this._projectName = projectName;
        }

        public FolderStrucureDescriptior Generate() {

            var mainFolder = Constants.RootDirectory + Constants.ServersProjectDir + $"/{this._projectName}_Api";
            var projectFolder = mainFolder + $"/{this._projectName}";
            var controllerDir = projectFolder + Consts.ControllersDiretoryName;
            var propertiesDir = projectFolder + Consts.PropertiesDirectoryName;
            var wwwrootDir = projectFolder + Consts.WwwRootDirectoryName;
            var mockDir = projectFolder + Consts.ControllersDiretoryName + Consts.MocksFolder;

            Directory.CreateDirectory(Constants.RootDirectory + Constants.ServersProjectDir);
            Directory.CreateDirectory(mainFolder);
            Directory.CreateDirectory(projectFolder);
            Directory.CreateDirectory(controllerDir);
            Directory.CreateDirectory(propertiesDir);
            Directory.CreateDirectory(wwwrootDir);
            Directory.CreateDirectory(mockDir);

            return new FolderStrucureDescriptior(
                this._projectName,
                mainFolder,
                projectFolder,
                controllerDir,
                propertiesDir,
                wwwrootDir,
                mockDir
            );
        }
    }
}
