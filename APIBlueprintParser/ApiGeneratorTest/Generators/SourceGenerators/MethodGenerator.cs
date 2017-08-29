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
using Newtonsoft.Json;

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

            var str = SourceTemplatesPathes.PathToMethodTemplate;

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
            str = str.Replace(Tokens.Code, this.CodeGeneration());
            str = str.Replace(Tokens.Parameters, parameters);

            return str;
        }

		private string CodeGeneration() {
			var parameters = this.GenerateParametersCode();
			//var body = this.GenerateBodyCode();

			var code = $"HttpContext.Response.ContentType = \"Application/json\";{Environment.NewLine}";
			code += parameters + Environment.NewLine;
			code += $"if (paramResult != null) return paramResult as object; {Environment.NewLine}";
			code += $" return new ArgumentOutOfRangeException(\"Bad requet\"); {Environment.NewLine}";

			return code;

		}

        private string GenerateParametersCode() {

			if (this._node.Parameters == null) {
				return "";
			}

			// create EquatableRequestParameter
			// public RequestParameter(object value, string name, ParameterType type)
			//EquatableRequaest(object requestBody, IList < RequestParameter > parameters)

			var code = $"var list = new List<RequestParameter>(); {Environment.NewLine}";
			foreach (var param in this._node.Parameters) {

				var paramtype = param.NeededType == NeededType.Optional ? "ParameterType.Optional" : "ParameterType.Required";

				code += $"list.Add(new RequestParameter({param.Name}, \"{param.Name}\", {paramtype})); {Environment.NewLine}";
			}

			code += $"var convertedRequest = new EquatableRequaest(value, list); {Environment.NewLine}";

			return code;
        }

        public string GenerateBodyCode() {

			var flag = "bodyEquls";
			var requestBody = "requestBody";
			var responseBody = "responseBody";
			var bodyResult = "bodyResult";

			var code = $"bool {flag} = false; {Environment.NewLine}";
			code += $"object {requestBody} = \"\";{Environment.NewLine}";
			code += $"object {responseBody} = \"\";{Environment.NewLine}";
			code += $"object {bodyResult} = \"\";{Environment.NewLine}";

            foreach (var pair in this._node.RequestPairs) {

				var reqBody = pair.Request.Body ?? "null";
				var respBody = pair.Response.Body ?? "null";

				code += $"{requestBody} = {reqBody}; {Environment.NewLine}";
				code += $"{responseBody} = {respBody}; {Environment.NewLine}";

				code += $"{flag} = value?.ToString() == \"{JsonConvert.SerializeObject(requestBody).ToString()}\"; {Environment.NewLine}";
				code += $"if ({flag} && flag) {bodyResult} = \"{JsonConvert.SerializeObject(responseBody).ToString()}\"; {Environment.NewLine}";
				 
            }
			return code;
        }
    }
}
