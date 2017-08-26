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

namespace APIBlueprintParser.Parsers {

	/// <summary>
	/// Parser for Attributes and Parameters.
	/// Really format: + <identifier>(<neededType>, <valueType>) - <descriptionText> <newline>
	/// Input format: (<neededType>, <valueType>) - <descriptionText> <newline>
    /// Identifier doesnt exist because it executed before.
	/// </summary>
    public class AttributeParser: BaseParser {

        #region Nested

        public struct Tokens {
            public const char DescriptionSeparator = '-';
            public const char TypeSeparator = ',';
            public const char StartOfTypeSection = '(';
            public const char EndOfTypeSection = ')';

            public const int TypesCount = 2;
            public const int NeededTypeIndex = 0;
            public const int ValueTypeIndex = 1;
        }

        #endregion

        public AttributeParser(StreamReader stream): base(stream) { }

        public AttributeNode Parse() {

			// <identifier> (<neededType>, <valueType>) - <descriptionText> <newline>

            // now: <identifier> (<neededType>, <valueType>) - <descriptionText>

            var stringView = streamReader.ReadLine().Trim();

			var identifier = stringView.Words().First();

			stringView = stringView.Substring(identifier.Length).Trim();

            if (stringView.First() != Tokens.StartOfTypeSection) {
                throw new FormatException($"Doesnt contains \'{Tokens.StartOfTypeSection}\'");
            }

            if (!stringView.Contains(Tokens.EndOfTypeSection.ToString())) {
				throw new FormatException($"Doesnt contains \'{Tokens.EndOfTypeSection}\'");
			}

            // get <neededType>,<valueType>
            var content = "";
            for (int i = 1; i < stringView.Length && stringView[i] != Tokens.EndOfTypeSection; i++ ) {
                content += stringView[i];
            }

            content = content.Trim();

			// split <neededType>, <valueType> by ','
            var splited = content.Split(Tokens.TypeSeparator);

            if (splited.Length != Tokens.TypesCount) {
                throw new FormatException($"Cant parse types"); 
            }

            var neededType = Support.StringToNeededType(splited[Tokens.NeededTypeIndex].Trim());

            if (!neededType.HasValue) {
                throw new FormatException($"Cant parse needed type");
			}

            var valueType = Support.StringToValueType(splited[Tokens.ValueTypeIndex].Trim());

			if (!valueType.HasValue) {
				throw new FormatException($"Cant parse value type");
			}

            var indexOfEndTypeSection = stringView.IndexOf(Tokens.EndOfTypeSection);

            var description = stringView.Substring(indexOfEndTypeSection + 1).Trim();

            var result = new AttributeNode(identifier, description, neededType.Value, valueType.Value);
            return result;
        }

    }
}
