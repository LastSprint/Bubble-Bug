//
// RequestParser.cs
// 21.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//
using System;
using System.IO;
using System.Collections.Generic;
using APIBlueprintParser.Models;
using APIBlueprintParser.Constants;

namespace APIBlueprintParser.Parsers {
    
    public class RequestParser: BaseParser {

        #region Nested

        public struct Tokens {
            public static string Separator = Environment.NewLine;
            public const char EndOfSection = '+';
            public const char StartOfSection = '+';
        }

        #endregion

        public RequestParser(Stream stream): base(stream) { }

        public RequestNode Parse() {

            // + Request (<type>) {NewLine} Body

			var streamReader = new StreamReader(base.stream);
			var sectionCharArr = new List<char>();

			if (!streamReader.EndOfStream && streamReader.Peek() != Tokens.StartOfSection) {
                throw new FormatException($"First element is not a \'{Tokens.StartOfSection}\'");
			}

			streamReader.Read();

			while (!streamReader.EndOfStream && streamReader.Peek() != Tokens.EndOfSection) {
				sectionCharArr.Add((char)streamReader.Read());
			}

			var stringView = new String(sectionCharArr.ToArray());

            var indexOfSeparator = stringView.IndexOf(Tokens.Separator, StringComparison.InvariantCulture);

            if (indexOfSeparator == -1) {
                throw new FormatException($"Cant separate request header form it body");
            }

            var header = "";

            for (int i = 0; i < indexOfSeparator; i++) {
                header += stringView[i];
            }

            if (!header.Contains(Keywords.Request)) {
                throw new FormatException($"Request must contains keyword {Keywords.Request}");
            }

        }
    }
}
