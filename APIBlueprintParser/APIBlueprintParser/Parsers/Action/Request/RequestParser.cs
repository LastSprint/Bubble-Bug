//
// RequestParser.cs
// 21.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using APIBlueprintParser.Models;

namespace APIBlueprintParser.Parsers.Action.Request {

	/// <summary>
	/// Request parser.
	/// Really format: ... + Request (<type>) <newline> Body + ...
	/// Input format: (<type>) <newline> Body + ...
	/// </summary>
	public class RequestParser: BaseParser {

        #region Nested

        public struct Tokens {
            public static string Separator = Environment.NewLine;
            public const char EndOfSection = '+';
            public const char StartOfSection = '+';
            public const char StartOfTypeSection = '(';
            public const char EndOfTypeSection = ')';

            public const string KeyWord = "Request";

            public const string HeadersKeyword = "Headers";
            public const string BodyKeyword = "Body";
            public const string SchemaKeyword = "Schema";
            public const string ParameterKeyWord = "Parameters";

            public static string[] NestedKeywords = { HeadersKeyword, BodyKeyword, SchemaKeyword, ParameterKeyWord };
        }

        #endregion

        private string _declaration;

        public RequestParser(StreamReader stream, string decl): base(stream) {
            this._declaration = decl;
        }

        public (RequestNode request, string lastReaded) Parse() {

            // Request <identifier>? (<media type>) <new line> -> this._declaration
            //  + Headers?
            //      <key>: <value>
            //
            //  + Body?
            //      <string>
            //
            //  + Schema?
            //      <string>

            var parsedDeclaration = this.ParseDeclaration();

            var lastReadedString = "";

            var request = new RequestNode();
            request.BodyType = parsedDeclaration.bodyType;
            request.Identifier = parsedDeclaration.indentifier;

            do {
                lastReadedString = this.streamReader.ReadLine();

				if (lastReadedString == "") {
					continue;
				}

                var current = lastReadedString.Replace(Tokens.EndOfSection.ToString(), "").Trim();

                if (!Tokens.NestedKeywords.Contains(current)) {
                    return (request, lastReadedString);
                }

                switch (current) {
                    case Tokens.BodyKeyword:
                        request.Body = new BodyParser(base.streamReader).Parse();
                        break;
                    case Tokens.SchemaKeyword:
                        request.Schema = new BodyParser(base.streamReader).Parse();
                        break;
                    case Tokens.HeadersKeyword:
                        request.Headers = new Dictionary<string, string>(new HeadersParser(base.streamReader).Parse());
                        break;
                    case Tokens.ParameterKeyWord:
                        request.Parameters = new Dictionary<string, string>(new ParametersParser(base.streamReader).Parse());
                        break;
                }
            } while (true);
        }

        private (string indentifier, BodyType bodyType) ParseDeclaration() {

			if (!this._declaration.Contains(Tokens.StartOfTypeSection) && !this._declaration.Contains(Tokens.EndOfTypeSection)) {
				throw new FormatException($"Request parser cant find media type section");
			}

            var words = this._declaration.Words().Where(x => x.Length != 0 && x.Length != 1).ToArray();

            if (words[0].Trim() != Tokens.KeyWord) {
                throw new FormatException($"Request parser cant find {Tokens.KeyWord} keyword");
            }

            var bodyType = Support.StringToBodyType(words.Last().Trim());

            if (!bodyType.HasValue) {
                throw new FormatException($"Request parser cant parse media type");
            }

            var identifier = "";

            if (words.Length == 3) {
                identifier = words[1];
            }

            return (identifier, bodyType.Value);

        }
    }
}
