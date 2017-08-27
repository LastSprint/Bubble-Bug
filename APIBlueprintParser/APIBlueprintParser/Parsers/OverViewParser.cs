//
// OverViewParser.cs
// 21.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using APIBlueprintParser.Models;

namespace APIBlueprintParser.Parsers {

	/// <summary>
	/// Parse overview section.
	/// <see cref="https://github.com/apiaryio/api-blueprint/blob/master/API%20Blueprint%20Specification.md#api-name--overview-section/>
	/// </summary>
    public class OverviewParser: BaseParser {

        #region Nested

        public struct Tokens {
            public const char StartOfSection = '#';
            public const char EndOfSection = '#';

            public static string Separator = Environment.NewLine;
        }

        #endregion

        public OverviewParser(StreamReader stream): base(stream) { }

        public OverviewNode Parse() {
			var sectionCharArr = new List<char>();

            if (!streamReader.EndOfStream && streamReader.Peek()!= Tokens.StartOfSection) {
                throw new FormatException("First element is not a \'#\'");
            }

            streamReader.Read();

			while (!streamReader.EndOfStream && streamReader.Peek() != Tokens.EndOfSection) {
				sectionCharArr.Add((char)streamReader.Read());
			}

            if (sectionCharArr.Count == 0) {
                return null;
            }

			var stringView = new String(sectionCharArr.ToArray());

            var splited = stringView.Split(Tokens.Separator.ToCharArray());

            if (splited.Length == 0) {
                return null;
            }

            var name = splited[0].Replace("#", "").Trim();
            var overview = new StringBuilder();

            for (int i = 1; i < splited.Length; i++) {
                overview.AppendLine(splited[i].Trim());
            }

            return new OverviewNode(name, overview.ToString().Trim());
        }
    }
}
