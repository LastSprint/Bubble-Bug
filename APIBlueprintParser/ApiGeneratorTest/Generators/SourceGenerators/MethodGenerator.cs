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
using ApiGeneratorTest.ThirdParty;
using System.Linq;

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
        private FolderStructureGenerator.FolderStrucureDescriptior _descriptor;
        private string _methodName;


        public MethodGenerator(ResourceActionNode node, FolderStructureGenerator.FolderStrucureDescriptior descriptor) {
            this._node = node;
            this._descriptor = descriptor;
            this._methodName = this._node.Identifier.Replace(" ", "").Trim();
        }

        public string Generate() {

            var httpMethod = this._node.HttpMethod.MethodToString();

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
            str = str.Replace(Tokens.MethodName, this._methodName);
            str = str.Replace(Tokens.Code, this.CodeGeneration());
            str = str.Replace(Tokens.Parameters, parameters);

            this.GenerateMockJSON();
            return str;
        }

		private string CodeGeneration() {
			var parameters = this.GenerateParametersCode();
			//var body = this.GenerateBodyCode();

			var code = $"HttpContext.Response.ContentType = \"Application/json\";{Environment.NewLine}";
			code += parameters + Environment.NewLine;
            /*
                var mocks = Support.ReadAllMocks("");

            foreach(var mock in mocks) {
                if (mock.Equals(convertedRequest)) {
                    return mock.RequestBody;
                }
            }

            throw new ArgumentOutOfRangeException("Cant find mock for current request :(");
            */
            code += $"var mocks = Support.ReadAllMocks(\"{this._methodName}\");";
            code += "foreach(var mock in mocks) {\n                if (mock.Equals(convertedRequest)) {\n                    HttpContext.Response.StatusCode = mock.ResponseCode;if (mock.ResponseHeaders != null) {\n\t\t\t\t\t\tforeach (var header in mock.ResponseHeaders) {\n\t\t\t\t\t\t\tHttpContext.Response.Headers.Add(header.Key, header.Value);\n\t\t\t\t\t\t}\n\t\t\t\t\t}return mock.ResponseBody;\n                }\n            }";
            code += "throw new ArgumentOutOfRangeException(\"Cant find mock for current request :(\");";
            return code;

		}

        private string GenerateParametersCode() {

			if (this._node.Parameters == null) {
				return "";
			}

			// create EquatableRequestParameter
			// public RequestParameter(object value, string name, ParameterType type)
			//EquatableRequaest(object requestBody, IList < RequestParameter > parameters)

			var code = $"var convertedRequest = new EquatableRequaest(value, new List<RequestParameter>()); {Environment.NewLine}";
			foreach (var param in this._node.Parameters) {

				var paramtype = param.NeededType == NeededType.Optional ? "ParameterType.Optional" : "ParameterType.Required";

				code += $"convertedRequest.AddParameter(new RequestParameter({param.Name}, \"{param.Name}\", {paramtype})); {Environment.NewLine}";
			}

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

        private void GenerateMockJSON() {
            var dirName = this._methodName;
            var pathToMock = $"{this._descriptor.ControllerDirectory}/{dirName}";
            Directory.CreateDirectory(pathToMock);
            var list = this._node.RequestPairs.ToList();
            for (var i = 0; i < this._node.RequestPairs.Count(); i++) {
                var pair = list[i];
                var req = new EquatableRequaest(pair, this._node.Parameters.ToList());
                var str = JsonConvert.SerializeObject(req);
                File.WriteAllText($"{pathToMock}/{i}mock.json", str);
            }
        }
    }
}
