//
// ApiGenerator.cs
// 27.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//
using System;
using APIBlueprintParser.Models;
using System.IO;

namespace ApiGeneratorTest.Generators.SourceGenerators {
    
    public class ApiGenerator {

        private ApiDescription _apiDescription;
        private FolderStructureGenerator.FolderStrucureDescriptior _folderDescriptor;

        public ApiGenerator(ApiDescription description, FolderStructureGenerator.FolderStrucureDescriptior folderDescriptor) {
            this._apiDescription = description;
            this._folderDescriptor = folderDescriptor;
        }

        public void Generate(bool generateMocks) {

            var pathToControllersFolser = this._folderDescriptor.ControllerDirectory;

            foreach (var group in this._apiDescription.Groups) {
                foreach (var resource in group.Resources) {
                    var code = new ControllerGenerator(resource, this._folderDescriptor.ProjectName, this._folderDescriptor).Generate(generateMocks);
                    File.WriteAllText(pathToControllersFolser + "/" + code.controllerName + ".cs", code.code);
                }
            }

        }
    }
}
