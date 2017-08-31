//
// AttributeParser.cs
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

namespace APIBlueprintParser.Parsers.Action.Attributes {

	/// <summary>
	/// Parser for Attributes and Parameters.
	/// Really format: + <identifier>(<neededType>, <valueType>) - <descriptionText> <newline>
	/// Input format: (<neededType>, <valueType>) - <descriptionText> <newline>
    /// Identifier doesnt exist because it executed before.
	/// </summary>
    public class AttributeParser {

        #region Nested

        public struct Tokens {
            public const char DescriptionSeparator = '-';
            public const char TypeSeparator = ',';
            public const char StartOfTypeSection = '(';
            public const char EndOfTypeSection = ')';

            public const int TypesCount = 2;
            public const int NeededTypeIndex = 0;
            public const int ValueTypeIndex = 1;

            public const char FirstSymbol = '+';
        }

        #endregion

        public AttributeParser(): base() { }

        public AttributeNode Parse(string line) {

            var stringView = line.Replace(Tokens.FirstSymbol.ToString(), "").Trim();

			if (!stringView.Contains(Tokens.StartOfTypeSection)) {
				throw new FormatException($"Doesnt contains \'{Tokens.StartOfTypeSection}\'");
			}

			if (!stringView.Contains(Tokens.EndOfTypeSection.ToString())) {
				throw new FormatException($"Doesnt contains \'{Tokens.EndOfTypeSection}\'");
			}

            var words = stringView.Words().Where(x => x.Trim().Length != 0 && x.Trim() != Tokens.DescriptionSeparator.ToString()).ToArray();

            if (words.Length < 3) {
                throw new FormatException("Attrbite contains less then 2 sections");
            }

            var identifier = words.First();

            var neededType = Support.StringToNeededType(words[1]);

            if (!neededType.HasValue) {
                throw new FormatException($"Cant parse needed type");
			}

            var valueType = Support.StringToValueType(words[2]);

			if (!valueType.HasValue) {
				throw new FormatException($"Cant parse value type");
			}

            var indexOfEndTypeSection = stringView.IndexOf(Tokens.EndOfTypeSection);

            var description = words.Length == 4 ? words[3] : "";

            var result = new AttributeNode(identifier, description, neededType.Value, valueType.Value);
            return result;
        }

    }
}
