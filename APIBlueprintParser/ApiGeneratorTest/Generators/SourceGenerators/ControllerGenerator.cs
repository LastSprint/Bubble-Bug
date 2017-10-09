//
// ControllerGenerator.cs
// 27.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//

using System;
using APIBlueprintParser.Models;
using System.IO;
using System.Collections.Generic;

namespace ApiGeneratorTest.Generators.SourceGenerators {
    public class ControllerGenerator {

        private struct Tokens {
            public const string ProjectName = "$ProjectName$";
            public const string ControllerName = "$Name$";
            public const string Code = "$Code$";
        }

        private ResourceNode _node;
        private string _projectName;
        private FolderStructureGenerator.FolderStrucureDescriptior _descriptor;

        public ControllerGenerator(ResourceNode node, string projectName, FolderStructureGenerator.FolderStrucureDescriptior descriptor) {
            this._node = node;
            this._projectName = projectName;
            this._descriptor = descriptor;
        }

        public (string code, string controllerName) Generate(bool generateMocks) {

            var controllerName = this._node.Identifier.Replace(" ", "").Trim();

            var code = "";

            foreach (var action in this._node.Actions) {
                code += new MethodGenerator(action, this._descriptor).Generate(generateMocks) + Environment.NewLine;
            }

            var str = SourceTemplatesPathes.PathToControllerTemplate;

            str = str.Replace(Tokens.ProjectName, this._projectName);
            str = str.Replace(Tokens.ControllerName, controllerName);
            str = str.Replace(Tokens.Code, code);

            return (str, controllerName+"Controller");
        }

    }
}
