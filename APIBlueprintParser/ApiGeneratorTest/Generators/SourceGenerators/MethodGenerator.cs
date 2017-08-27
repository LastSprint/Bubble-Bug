//
// MethodGenerator.cs
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
    
    public class MethodGenerator {

        private struct Tokens {
            public const string RoutPath = "$uri$";
            public const string HttpMethod = "$HttpMethod$";
            public const string ReturnType = "$ReturnType$";
            public const string MethodName = "$MethodName$";
            public const string Code = "$Code$";

        }

        private ResourceActionNode _node;

        public MethodGenerator(ResourceActionNode node) {
            this._node = node;
        }

        public string Generate() {

            var httpMethod = this._node.HttpMethod.MethodToString();

            var name = this._node.Identifier.Replace(" ", "").Trim();

            var returnType = "object";

            var route = this._node.Template.Template;

            var str = File.ReadAllText(SourceTemplatesPathes.PathToMethodTemplate);

            str = str.Replace(Tokens.RoutPath, route);
            str = str.Replace(Tokens.HttpMethod, httpMethod);
            str = str.Replace(Tokens.ReturnType, returnType);
            str = str.Replace(Tokens.MethodName, name);
            str = str.Replace(Tokens.Code, this.GenerateCode());

            return str;
        }

        private string GenerateCode() {

            var requsetDictionary = "request";

            var code = $"Dictionary<string, string> {requsetDictionary} = new Dictionary<string, string>(); {Environment.NewLine}";

            //var t = new Dictionary<string, string>();
            foreach (var pair in this._node.RequestPairs) {
                var reqBody = pair.Request.Body?.Replace(Environment.NewLine, "").Replace("\"", "\\\"") ?? "";
                var respBody = pair.Response.Body?.Replace(Environment.NewLine, "").Replace("\"", "\\\"") ?? "";

                code += $"{requsetDictionary}.Add(\"{reqBody}\", \"{respBody}\");{Environment.NewLine}";
            }

            code += $"return {requsetDictionary}[value];";

            return code;
        }
    }
}
