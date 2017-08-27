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
            public const string Parameters = "$Params$";
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

            var parameters = "";

            if (this._node.Parameters != null) {
				foreach (var param in this._node.Parameters) {
					parameters += $"{param.ValueType.TypeToString()} {param.Name},";
				}
            }

            str = str.Replace(Tokens.RoutPath, route);
            str = str.Replace(Tokens.HttpMethod, httpMethod);
            str = str.Replace(Tokens.ReturnType, returnType);
            str = str.Replace(Tokens.MethodName, name);
            str = str.Replace(Tokens.Code, this.GenerateCode());
            str = str.Replace(Tokens.Parameters, parameters);

            return str;
        }

        private string GenerateCode() {

            if (this._node.Parameters == null) {
                return "";
            }

            var code = $"bool flag = false;{Environment.NewLine}";

            foreach (var pair in this._node.RequestPairs) {

                if (pair.Request.Parameters == null) {
                    continue;
                }

                var expressions = "";

                foreach (var param in this._node.Parameters) {
                    expressions += $" && {param.Name} == {pair.Request.Parameters[param.Name]}";
                }

                expressions = expressions.Remove(0, " &&".Length);

                expressions = $"flag = {expressions};{Environment.NewLine}";

                code += expressions;

                code += $"if (flag) return {pair.Response.Body}; {Environment.NewLine}";

            }

            //var requsetDictionary = "request";
            //var code = $"Dictionary<string, string> {requsetDictionary} = new Dictionary<string, string>(); {Environment.NewLine}";
            //foreach (var pair in this._node.RequestPairs) {
            //    var reqBody = pair.Request.Body?.Replace(Environment.NewLine, "").Replace("\"", "\\\"") ?? "";
            //    var respBody = pair.Response.Body?.Replace(Environment.NewLine, "").Replace("\"", "\\\"") ?? "";
            //    code += $"{requsetDictionary}.Add(\"{reqBody}\", \"{respBody}\");{Environment.NewLine}";
            //}
            //code += $"return {requsetDictionary}[value];";
            code += $" throw new ArgumentOutOfRangeException(\"Bad requet\"); {Environment.NewLine}";
            return code;
        }
    }
}
