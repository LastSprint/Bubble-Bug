//
// ResponseParser.cs
// 22.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//

using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using APIBlueprintParser.Models;

namespace APIBlueprintParser.Parsers {

	/// <summary>
	/// Response parser.
	/// Really format: ... + Response <number> (<type>) <newline> Body + ...
	/// Input format: <number> (<type>) <newline> Body + ...
	/// </summary>
	public class ResponseParser: BaseParser {

        #region Nested

        public struct Tokens {
            public const char StartOfSection = '+';
            public const char EndOfSection = '+';

            public const char StartOfTypeSection = '(';
            public const char EndOfTypeSection = ')';

            public static string Separator = Environment.NewLine;
        }

        #endregion

        public ResponseParser(StreamReader stream, ): base(stream) { }

        public ResponseNode Parse() {

			var sectionCharArr = new List<char>();

			while (!streamReader.EndOfStream && streamReader.Peek() != Tokens.EndOfSection) {
				sectionCharArr.Add((char)streamReader.Read());
			}

			var stringView = new String(sectionCharArr.ToArray());

			// separate  <number> (<type>) {NewLine} Body by {NewLine}
			var indexOfSeparator = stringView.IndexOf(Tokens.Separator, StringComparison.InvariantCulture);

			if (indexOfSeparator == -1) {
				throw new FormatException($"Cant separate request header form it body");
			}

			var header = "";

			// read all symbols before {NewLine}

			for (int i = 0; i < indexOfSeparator; i++) {
				header += stringView[i];
			}

			header = header.Trim();

			// if last symbol of Request (<type>) not a ')' than throws
			if (header.Last() != Tokens.EndOfTypeSection) {
				throw new FormatException($"Header of this section not end on the \'{Tokens.EndOfTypeSection}\'");
			}

			// if header deasnt contains '(' then throws
			if (!header.Contains(Tokens.StartOfTypeSection.ToString())) {
				throw new FormatException($"Header of this section not contains \'{Tokens.StartOfTypeSection}\'");
			}

			var type = "";

			// read <number>
			var stringCode = "";
			for (int i = 0; header[i] != Tokens.StartOfTypeSection && i < header.Length; i++) {
				stringCode += header[i];
			}

			stringCode = stringCode.Trim();

			if (!Int32.TryParse(stringCode, out int code)) {
				throw new FormatException($"Header doesnt contains code");
			}

			// read string between '(' and ')'
			for (int i = header.Length - 2; i >= 0 && header[i] != Tokens.StartOfTypeSection; i--) {
				type += header[i];
			}

			type = new String(type.Reverse().ToArray());
			var bodyType = Support.StringToBodyType(type.Trim());

            if (!bodyType.HasValue) {
				throw new FormatException($"Content type string have invalid format");
			}

			// execute request body - its a string after {NewLine}
			var body = stringView.Substring(indexOfSeparator).Trim();

            return new ResponseNode(code, bodyType.Value, body);
        }
    }
}
