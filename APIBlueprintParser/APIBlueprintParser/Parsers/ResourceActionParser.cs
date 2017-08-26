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

namespace APIBlueprintParser.Parsers {

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
            public const char EndOfHeader = '+';
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

            while (!streamReader.EndOfStream && streamReader.Peek() != Tokens.EndOfHeader) {
                sectionCharArr.Add((char)streamReader.Read());
            }

            // <identifier> [<httpmeth>, <URITempl>] ....
            var stringView = new String(sectionCharArr.ToArray()).Trim();

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

        public void ParseSubnodes() {

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
            var parameter = new List<AttributeNode>();
            var attributes = new List<AttributeNode>();
            var requesrPairs = new List<RequestPair>();

            RequestNode currentRequest = null;

            var parseState = State.None;
            while (!streamReader.EndOfStream && streamReader.Peek() != Tokens.EndOfSection) {
                var readed = (char)streamReader.Read();

                if (readed == Tokens.EndOfHeader) {
                    var str = streamReader.ReadLine();

                    switch (str) {
                        case Tokens.Parameters: 
                            parseState = State.Parameters;
                            continue;
                        case Tokens.Attributes:
                            parseState = State.Attributes;
                            continue;
                    }

                    if (str.Contains(Tokens.Request)) {

                        if (parseState == State.Request) {
                            throw new FormatException("After request need implement response for this request");
                        }

                        //currentRequest = new RequestParser(base.stream).Parse();
                        parseState = State.Request;
                        continue;

                    } else if (str.Contains(Tokens.Response)) {

                        if (parseState == State.Response) {
                            throw new FormatException("After response need implement new request");
                        }

                        var response = new ResponseParser(base.streamReader).Parse();
                        requesrPairs.Add(new RequestPair(currentRequest, response));
                        parseState = State.Response;
                        continue;
                    }

                    switch (parseState) {
                        case State.Attributes:
                            attributes.Add(new AttributeParser(base.streamReader).Parse());
                            continue;
                        case State.Parameters:
                            parameter.Add(new AttributeParser(base.streamReader).Parse());
                            continue;

                    }
                }
            }

            //return new ResourceActionNode("",null, HttpMethod.Get)

        }
    }
}
