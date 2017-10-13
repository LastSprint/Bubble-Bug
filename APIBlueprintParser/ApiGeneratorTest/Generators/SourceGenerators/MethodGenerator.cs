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
            public const string Body = "$Body$";
        }

        private ResourceActionNode _node;
        private FolderStructureGenerator.FolderStrucureDescriptior _descriptor;
        private string _methodName;
        private bool isEmptyRequestContent;

        public MethodGenerator(ResourceActionNode node, FolderStructureGenerator.FolderStrucureDescriptior descriptor) {
            this._node = node;
            this._descriptor = descriptor;
            this._methodName = this._node.Identifier.Replace(" ", "").Trim();
            this.isEmptyRequestContent = false;
        }

        public string Generate(bool generateMocks) {

            var httpMethod = this._node.HttpMethod.MethodToString();

            var returnType = "object";

            var route = this._node.Template.Template;

            var isIterative = this._node.Options.Contains(ActionOption.Iterative);

            var str = isIterative ? SourceTemplatesPathes.PathToIterativeMethodTemplate : SourceTemplatesPathes.PathToMethodTemplate;

            var parameters = "";

            var body = "";

            switch (this._node.RequestPairs.First().Request.BodyType) {
                case BodyType.Json:
                    body = "[FromBody]object value";
                    break;
                case BodyType.Empty:
					body = "";
                    this.isEmptyRequestContent = true;
                    break;
            }

            if (this._node.Parameters != null) {
				foreach (var param in this._node.Parameters) {
					parameters += $"{param.ValueType.TypeToString()} {param.Name},";
				}
            }

			if (body.Length == 0 && parameters.Length != 0) {
                parameters = parameters.Substring(0, parameters.Length - 1);
            }

            str = str.Replace(Tokens.RoutPath, route);
            str = str.Replace(Tokens.HttpMethod, httpMethod);
            str = str.Replace(Tokens.ReturnType, returnType);
            str = str.Replace(Tokens.MethodName, this._methodName);
            if (!isIterative) {
                str = str.Replace(Tokens.Code, this.CodeGeneration());
            }
            str = str.Replace(Tokens.Parameters, parameters);
            str = str.Replace(Tokens.Body, body);

            this.GenerateMockJSON(generateMocks);

            return str;
        }

		private string CodeGeneration() {
			var parameters = this.GenerateParametersCode();
			//var body = this.GenerateBodyCode();

			var code = $"HttpContext.Response.ContentType = \"Application/json\";{Environment.NewLine}";
            code += "object result;" + Environment.NewLine;
            code += "try { " + Environment.NewLine;
			code += parameters + Environment.NewLine;
            code += $"var mocks = Support.ReadAllMocks(\"{this._methodName}\");";
            code += "foreach(var mock in mocks) {\n                if (mock.Equals(convertedRequest)) {\n                    HttpContext.Response.StatusCode = mock.ResponseCode;if (mock.ResponseHeaders != null) {\n\t\t\t\t\t\tforeach (var header in mock.ResponseHeaders) {\n\t\t\t\t\t\t\tHttpContext.Response.Headers.Add(header.Key, header.Value);\n\t\t\t\t\t\t}\n\t\t\t\t\t}return mock.ResponseBody;\n                }\n            }";
            code += "throw new MockNotFoundException();";
            code += Environment.NewLine + "} catch (Exception e) {" + Environment.NewLine;
            code += "HttpContext.Response.StatusCode = 500;" + Environment.NewLine;
            code += "result = e;}" + Environment.NewLine;
            code += "return result;" + Environment.NewLine;
            return code;

		}

        private string GenerateParametersCode() {

            var bodyValue = this.isEmptyRequestContent ? "null" : "value";

            var code = $"var convertedRequest = new EquatableRequaest({bodyValue}, new List<RequestParameter>()); {Environment.NewLine}";

			if (this._node.Parameters == null) {
				return code;
			}

			// create EquatableRequestParameter
			// public RequestParameter(object value, string name, ParameterType type)
			//EquatableRequaest(object requestBody, IList < RequestParameter > parameters)

			foreach (var param in this._node.Parameters) {

				var paramtype = param.NeededType == NeededType.Optional ? "ParameterType.Optional" : "ParameterType.Required";
                var paramString = param.ValueType == APIBlueprintParser.Models.ValueType.String ? $"{param.Name} != null ? Uri.UnescapeDataString({param.Name}) : null" : param.Name;
                code += $"convertedRequest.AddParameter(new RequestParameter({paramString}, \"{param.Name}\", {paramtype})); {Environment.NewLine}";
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

        private void GenerateMockJSON(bool generateMocks) {
            var dirName = this._methodName;
            var pathToMock = $"{this._descriptor.MocksDirectory}/{dirName}";
            Directory.CreateDirectory(pathToMock);
            if (!generateMocks) {
                return;
            }
            var list = this._node.RequestPairs.ToList();
            for (var i = 0; i < this._node.RequestPairs.Count(); i++) {
                var pair = list[i];

				var lstr = this._node.Parameters != null ? this._node.Parameters.ToList() : new List<AttributeNode>();

                var req = new EquatableRequaest(pair, lstr);

                if (req.RequestBody == null && req.ResponseBody == null) {
                    break;
                }

                var str = JsonConvert.SerializeObject(req);
                File.WriteAllText($"{pathToMock}/{i}mock.json", str);
            }
        }
    }
}
