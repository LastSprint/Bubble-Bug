//
// OptionsParser.cs
// 10.09.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//
using System;
using System.IO;
using System.Collections.Generic;
using APIBlueprintParser.Models;
using System.Linq;

namespace APIBlueprintParser.Parsers.Action {
    
    public class OptionsParser: BaseParser {

		public struct Tokens {
			public static string[] EndKeyNodes = { "Parameters", "Request", "Response", "Attributes", "Options" };
			public const char EndOfSection = '+';
		}

        public OptionsParser(StreamReader reader): base(reader) { }

        public (IReadOnlyList<ActionOption> options, string lastReadedLine) Parse() {
            var options = new List<ActionOption>();
			var lastReadedString = "";

            do
            {
                lastReadedString = base.streamReader.ReadLine();

				if (lastReadedString == null)
				{
                    return (options, lastReadedString);
				}

                var current = lastReadedString.Replace(Tokens.EndOfSection.ToString(), "").Trim();

				if (Tokens.EndKeyNodes.Contains(current) || current.Contains("Request") || current.Contains("Response")) {

					return (options, lastReadedString);
				}

				if (lastReadedString.Contains("+")) {
                    var str = lastReadedString.Replace("+", "").Trim();
                    var casted = Support.StringToActionOption(str);

                    if (!casted.HasValue) {
                        throw new FormatException($"Undefind options - {str}");
                    }
                    options.Add(casted.Value);
				}
			} while (true);

		}

    }
}
