//
// HeadersParser.cs
// 26.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace APIBlueprintParser.Parsers.Action.Request {
    
    public class HeadersParser: BaseParser {

        public struct Tokens {
            public const char EndOfSection = '+';
            public static string PairSeparator = Environment.NewLine;
            public const char KeyValueSeparator = ':';

            public const int KeyIndex = 0;
            public const int ValueIndex = 1;
        }


        public HeadersParser(StreamReader stream): base(stream) {}

        public IDictionary<string, string> Parse() {

			// <key> : <value>
			// <key> : <value>
			// ....
			// +

			var sectionCharArr = new List<char>();

            while (!this.streamReader.EndOfStream && this.streamReader.Peek() != Tokens.EndOfSection && this.streamReader.Peek() != '#')
			{
				sectionCharArr.Add((char) this.streamReader.Read());
			}

			var stringView = new String(sectionCharArr.ToArray());
            stringView = stringView.Trim();

            if (stringView.Length == 0) {
                throw new FormatException("If header section declared then it cant be empty");
            }

            var pairs = stringView.Split(Tokens.PairSeparator.ToCharArray()).Where( x => x.Length != 0);
            var result = new Dictionary<string, string>();
            foreach (var pair in pairs) {

                var splited = pair.Split(Tokens.KeyValueSeparator);

                if (splited.Length != 2) {
                    throw new FormatException("Header must be presented as key: value");
                }

                result.Add(splited[Tokens.KeyIndex].Trim(), splited[Tokens.ValueIndex].Trim());
            }

            return result;

        }

    }
}
