//
// ParametersParser.cs
// 27.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace APIBlueprintParser.Parsers.Action.Request {
    public class ParametersParser: BaseParser {

		public struct Tokens
		{
			public const char EndOfSection = '+';
			public static string PairSeparator = Environment.NewLine;
			public const char KeyValueSeparator = ':';

			public const int KeyIndex = 0;
			public const int ValueIndex = 1;
		}

        public ParametersParser(StreamReader reader): base(reader) { }

        public IDictionary<string, string> Parse() {
			var sectionCharArr = new List<char>();

			while (!this.streamReader.EndOfStream && this.streamReader.Peek() != Tokens.EndOfSection && this.streamReader.Peek() != '#')
			{
				sectionCharArr.Add((char) this.streamReader.Read());
			}

            var dict = new Dictionary<string, string>();

			var stringView = new String(sectionCharArr.ToArray());

            var pairs = stringView.Split(Tokens.PairSeparator.ToArray()).Where( x=> x.Length != 0);

            foreach (var pair in pairs) {
                var sepIndex = pair.IndexOf(Tokens.KeyValueSeparator) ;

                var key = "";

                for (int i = 0; i < sepIndex; i++) {
                    key += pair[i];
                }

                var value = "";

                for (int i = sepIndex + 1; i < pair.Length; i++) {
                    value += pair[i];
                }

                dict.Add(key.Trim(), value.Trim());
            }

            return dict;
        }
    }
}
