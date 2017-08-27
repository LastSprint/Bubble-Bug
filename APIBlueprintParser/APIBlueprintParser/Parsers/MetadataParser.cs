﻿//
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

                // need reed to first KeyValueSeparator
                var keyValue = trimmed.Split(Tokens.KeyValueSeparator).ToList();

                var resultSplited = new List<string>();

                if (keyValue.Count < Tokens.CountOfContentInPair) {
                    throw new ArgumentException("MetadataParser cant parse content bacouse it contains bad key:value pairs");
                }

                resultSplited.Add(keyValue.First());
                keyValue.RemoveAt(0);
                resultSplited.Add(keyValue.Aggregate( (x,y) => x + y));


                result.Add(resultSplited[(int)Tokens.PairTokens.Key].Trim(), resultSplited[(int)Tokens.PairTokens.Value].Trim());
            }

            return result;
        }

    }
}
