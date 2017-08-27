//
// MetadataParser.cs
// 17.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace APIBlueprintParser.Parsers {
    
    /// <summary>
    /// Parse metadata section.
    /// <see cref="https://github.com/apiaryio/api-blueprint/blob/master/API%20Blueprint%20Specification.md#metadata-section"/>
    /// </summary>
    public class MetadataParser: BaseParser {

        #region Nested

        internal struct Tokens {
            public const char EndOfSection = '#';
            public const char KeyValueSeparator = ':';
            public static char[] PairsSeparator = Environment.NewLine.ToCharArray();
            public const int CountOfContentInPair = 2;

            public enum PairTokens {
                Key,
                Value
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="T:APIBlueprintParser.MetadataParser"/> class.
        /// </summary>
        /// <param name="stream">Stream which contains API specifications writing on APIBlueprint</param>
        public MetadataParser(StreamReader stream): base(stream) { }

        public Dictionary<string, string> Parse() {

            var sectionCharArr = new List<char>();

            while (!base.streamReader.EndOfStream && base.streamReader.Peek() != Tokens.EndOfSection) {
                sectionCharArr.Add((char)streamReader.Read());
            }

            var stringView = new String(sectionCharArr.ToArray());

            // separate for array key:value, key: value ...
            var keyValuePairs = stringView.Split(Tokens.PairsSeparator);

            var result = new Dictionary<string, string>();

            foreach (string pair in keyValuePairs) {

                var trimmed = pair.Trim();
                if (trimmed == "") {
                    continue;
                }

                var indexOfSeparator = trimmed.IndexOf(Tokens.KeyValueSeparator);

                if (indexOfSeparator == -1 || indexOfSeparator == trimmed.Length - 1) {
                    throw new ArgumentException("MetadataParser cant parse content bacouse it contains bad key:value pairs");
                }

				var key = "";
				var value = "";

                for (int i = 0; i < indexOfSeparator; i++) {
                    key += trimmed[i];
                }

                for (int i = indexOfSeparator + 1 ; i < trimmed.Length; i++) {
                    value += trimmed[i];
				}

                result.Add(key.Trim(), value.Trim());
            }

            return result;
        }

    }
}
