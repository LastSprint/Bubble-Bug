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

            public const string Attributes = "Attributes";
            public const string Parameters = "Parameters";

            public const string Request = "Request";
            public const string Response = "Response";
        }

          #endregion

        public ResourceActionParser(StreamReader stream): base(stream) { }

        /// <summary>
        /// Parses the without nesten nodes.
        /// Implement for testable.
        /// </summary>
        /// <returns>ResourceActionNode without nested nodes</returns>
        public ResourceActionNode ParseWithoutNestenNodes()
        {

            // <identifier> [<httpmeth>, <URITempl>] .... +
            // read to '+'
            var sectionCharArr = new List<char>();

            while (!streamReader.EndOfStream && streamReader.Peek() != Tokens.StartOfSubsection) {
                sectionCharArr.Add((char)streamReader.Read());
            }

            // <identifier> [<httpmeth>, <URITempl>] ....
            var stringView = new String(sectionCharArr.ToArray()).Replace("#","").Trim();

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

        public ResourceActionNode Parse() {

            // TODO: Make commants

            // + Attributes
            //        + parameter1
            //        + parameter2
            // + Parameters
            //        + parameter1
            //        + parameter2
            // + Request
            // + Response
            // + Request
            // + Response
            // ...
            // #...
            var requesrPairs = new List<RequestPair>();

            var result = this.ParseWithoutNestenNodes();

            RequestNode currentRequest = null;

            var line = "";

			do {

                if (base.streamReader.Peek() == Tokens.EndOfSection) {
                    return result;
                }

                line = base.streamReader.ReadLine().Replace(Tokens.StartOfSubsection.ToString(), "").Trim();

                if (line.Contains("Attributes")) {
					var res = new AttributesParser(base.streamReader).Parse();
					line = res.lastReadedLine;
					result.Attributes = res.attributes.ToList();
                }

                if (line.Contains("Parameters")) {
					var res = new AttributesParser(base.streamReader).Parse();
					line = res.lastReadedLine;
                    result.Parameters = res.attributes.ToList();
                }

                while (true)
                {

                    if (line.Trim() == "") {
                        result.RequestPairs = requesrPairs.ToList();
                        return result;
                    }

                    if (line.Contains("Request"))
                    {
                        var res = new RequestParser(base.streamReader, line).Parse();
                        currentRequest = res.request;
                        line = res.lastReaded;
                    }

                    if (line.Contains("Response"))
                    {
                        var res = new ResponseParser(base.streamReader, line).Parse();
                        var response = res.response;
                        line = res.lastReaded;
                        requesrPairs.Add(new RequestPair(currentRequest, response));
                    }
                }

            } while (true);
        }
    }
}
