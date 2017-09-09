//
// ResponseParser.cs
// 26.08.2017
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
    public class ResponseParser: BaseParser {
		#region Nested

		public struct Tokens {
			public static string Separator = Environment.NewLine;
			public const char EndOfSection = '+';
			public const char StartOfSection = '+';
			public const char StartOfTypeSection = '(';
			public const char EndOfTypeSection = ')';

			public const string KeyWord = "Response";

			public const string HeadersKeyword = "Headers";
			public const string BodyKeyword = "Body";

			public static string[] NestedKeywords = { HeadersKeyword, BodyKeyword };
		}

		#endregion

        private string _declaration;

		public ResponseParser(StreamReader stream, string decl): base(stream) {
			this._declaration = decl;
		}

        public (ResponseNode response, string lastReaded)Parse()
		{

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

            var response = new ResponseNode();
            response.BodyType = parsedDeclaration.bodyType;
            response.Code = parsedDeclaration.statusCode;

			do
			{
                if (streamReader.Peek() == '#') {
                    return (response, "");
                }
                
				lastReadedString = streamReader.ReadLine();

                if (lastReadedString == null) {
                    return (response, null);
                }

                var current = lastReadedString.Replace(Tokens.EndOfSection.ToString(), "").Trim();

				if (!Tokens.NestedKeywords.Contains(current))
				{
                    return (response, lastReadedString);
				}

				switch (current) {
					case Tokens.BodyKeyword:
						response.Body = new BodyParser(base.streamReader).Parse();
						break;
					case Tokens.HeadersKeyword:
						response.Headers = new Dictionary<string, string>(new HeadersParser(base.streamReader).Parse());
						break;
				}
			} while (true);
		}

		private (int statusCode, BodyType bodyType) ParseDeclaration()
		{

			if (!this._declaration.Contains(Tokens.StartOfTypeSection) && !this._declaration.Contains(Tokens.EndOfTypeSection))
			{
				throw new FormatException($"Response parser cant find media type section");
			}

            var words = this._declaration.Words().Where(x => x.Length != 0 && x != Tokens.EndOfSection.ToString()).ToArray();

            if (words.Length != 3) {
                throw new FormatException($"Response declaration cant contains 3 section");
            }

			if (words[0].Trim() != Tokens.KeyWord)
			{
				throw new FormatException($"Response parser cant find {Tokens.KeyWord} keyword");
			}

			var bodyType = Support.StringToBodyType(words.Last().Trim());

            if (!int.TryParse(words[1], out int code)) {
                throw new FormatException("Response parser cant parse statuc code");
            }

			return (code, bodyType);

		}
	}
}
