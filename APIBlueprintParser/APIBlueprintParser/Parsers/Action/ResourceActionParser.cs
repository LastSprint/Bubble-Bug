//
// GroupParser.cs
// 23.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//

using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using APIBlueprintParser.Models;
using APIBlueprintParser.Parsers.Action.Request;
using APIBlueprintParser.Parsers.Action.Attributes;

namespace APIBlueprintParser.Parsers.Action {

    /// <summary>
    /// Realy format: ### <identifier> [<httpMethod>,<URITemplate>] ...
    /// Input foramt: <identifier> [<httpMethod>,<URITemplate>] ...
    /// </summary>
    public class ResourceActionParser: BaseParser {

        #region Nested

        private enum State {
            None,
            Attributes,
            Parameters,
            Response,
            Request
        }


        public struct Tokens {
            public const char EndOfSection = '#';
            public const char StartOfSubsection = '+';
            public const char StartOfTypeSection = '[';
            public const char EndOfTypeSection = ']';
            public const char TypeSectionSeparator = ',';

            public const int HttpMethodTypeIndex = 0;
            public const int URITemplateTypeIndex = 1;
        }

        public struct NestedSections {
            public const string Attributes = "Attributes";
            public const string Parameters = "Parameters";
			public const string Request = "Request";
			public const string Response = "Response";
            public const string Options = "Options";
        }

        #endregion

        private string _declaration;

        public ResourceActionParser(StreamReader stream, string declaration): base(stream) {
            this._declaration = declaration;
        }

        /// <summary>
        /// Parses the without nesten nodes.
        /// Implement for testable.
        /// </summary>
        /// <returns>ResourceActionNode without nested nodes</returns>
        public ResourceActionNode ParseWithoutNestenNodes()
        {

            // <identifier> [<httpmeth>, <URITempl>] ....
            var stringView = this._declaration.Replace("#","").Trim();

            // check format

            if (!stringView.Contains(Tokens.StartOfTypeSection)) {
                throw new FormatException($"Header doesnt contains \'{Tokens.StartOfTypeSection}\'");
            }

            if (!stringView.Contains(Tokens.EndOfTypeSection)) {
                throw new FormatException($"Header doesnt contains \'{Tokens.StartOfTypeSection}\'");
            }

            // read <identifier>

            var identifier = "";

            for (int i = 0; i < stringView.Length && stringView[i] != Tokens.StartOfTypeSection; i++) {
                identifier += stringView[i];
            }

            identifier = identifier.Trim();

            // read [<httpMeth>, <URITempl>]

            var typeSection = "";

            var indexOfStartTypeSection = stringView.IndexOf(Tokens.StartOfTypeSection);

            for (int i = indexOfStartTypeSection + 1; i < stringView.Length && stringView[i] != Tokens.EndOfTypeSection; i++) {
                typeSection += stringView[i];
            }

            typeSection = typeSection.Trim();
            // validation

            if (!typeSection.Contains(Tokens.TypeSectionSeparator)) {
                throw new FormatException($"Type section doesnt contains \'{Tokens.TypeSectionSeparator}\'");
            }

            // split <HttpMeth>, <URITempl> by ','

            var splited = typeSection.Split(Tokens.TypeSectionSeparator);

            if (splited.Length != 2) {
                throw new FormatException("Type section doesnt contains HttpMethod and/or URITemplate or contains too much arguments");
            }

            var httpMethStringView = splited[Tokens.HttpMethodTypeIndex].Trim();
            var URITemplStringView = splited[Tokens.URITemplateTypeIndex].Trim();

            var httpMeth = Support.StringToHttpMethod(httpMethStringView);

            if (!httpMeth.HasValue) {
                throw new FormatException("Cant parse http method");
            }

            if (!HeaderKeywords.IsURI(URITemplStringView)) {
                throw new FormatException("Cant parse URI template");
            }

            return new ResourceActionNode(identifier, new UriTemplate.Core.UriTemplate(URITemplStringView), httpMeth.Value);

        }

        public (ResourceActionNode resourceAction, string lastReaded) Parse() {

            var requesrPairs = new List<RequestPair>();

            var result = this.ParseWithoutNestenNodes();

            RequestNode currentRequest = null;

            var line = "";

			do {

                if (base.streamReader.Peek() == Tokens.EndOfSection) {
                    return (result, streamReader.ReadLine());
                }

                do {
                    line = base.streamReader.ReadLine().Replace(Tokens.StartOfSubsection.ToString(), "").Trim();
                } while (line == "");

                if (line.Contains(NestedSections.Attributes)) {
					var res = new AttributesParser(base.streamReader).Parse();
					line = res.lastReadedLine;
					result.Attributes = res.attributes.ToList();
                }

				if (line == null) {
					return (result, line);
				}

                while (line.Trim() == "") {
                    line = base.streamReader.ReadLine().Replace(Tokens.StartOfSubsection.ToString(), "").Trim();
                }

				if (line.Contains(NestedSections.Options)) {
                    var res = new OptionsParser(base.streamReader).Parse();
					line = res.lastReadedLine;
                    result.Options = res.options.ToList();
				}

				if (line == null) {
					return (result, line);
				}

				while (line.Trim() == "") {
					line = base.streamReader.ReadLine()?.Replace(Tokens.StartOfSubsection.ToString(), "").Trim();
				}

                if (line.Contains(NestedSections.Parameters)) {
					var res = new AttributesParser(base.streamReader).Parse();
					line = res.lastReadedLine;
                    result.Parameters = res.attributes.ToList();
                }


                while (true) {

					if (line == null) {
						result.RequestPairs = requesrPairs;
						return (result, line);
					}

                    if (line.Contains(NestedSections.Request))
                    {
                        var res = new RequestParser(base.streamReader, line).Parse();
                        currentRequest = res.request;
                        line = res.lastReaded;
                    }

                    if (line.Contains(NestedSections.Response))
                    {
                        var res = new ResponseParser(base.streamReader, line).Parse();
                        var response = res.response;
                        line = res.lastReaded;
                        requesrPairs.Add(new RequestPair(currentRequest, response));
                    }

                    if (line == null) {
                        result.RequestPairs = requesrPairs;
                        return (result, line);
                    }

                    if (line.Contains("#"))
					{
						result.RequestPairs = requesrPairs.ToList();
                        return (result, line);
                    } else if (!line.Contains(NestedSections.Response) && !line.Contains(NestedSections.Request)) {
                        line = streamReader.ReadLine();
                    }
                }

            } while (true);
        }
    }
}
