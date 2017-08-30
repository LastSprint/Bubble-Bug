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

			var pairs = stringView.Split(Tokens.PairSeparator.ToArray()).Where( x=> x.Trim().Length != 0);

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

                value = value.Trim();

                if (value.Length != 0)
                {

                    if (value.First() == '\"' || value.First() == '\'')
                    {
                        value = value.Substring(1);
                    }

                    if (value.Last() == '\"' || value.Last() == '\'')
                    {
                        value = value.Substring(0, value.Length - 1);
                    }

                }

                dict.Add(key.Trim(), value.Trim());
            }

            return dict;
        }
    }
}
