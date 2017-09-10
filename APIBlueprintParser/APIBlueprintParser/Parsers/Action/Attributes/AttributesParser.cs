//
// AttributesParser.cs
// 26.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//

using System.Linq;
using System.IO;
using System.Collections.Generic;
using APIBlueprintParser.Models;

namespace APIBlueprintParser.Parsers.Action.Attributes {
    public class AttributesParser : BaseParser{

        public struct Tokens {
            public static string[] EndKeyNodes = { "Parameters", "Request", "Response", "Attributes", "Options" };
            public const char EndOfSection = '+';
        }

        public AttributesParser(StreamReader streamReader): base(streamReader) { }


        public (IList<AttributeNode> attributes, string lastReadedLine) Parse() {

            var attributes = new List<AttributeNode>();

            var lastReadedString = "";

            do {
                lastReadedString = base.streamReader.ReadLine();
                var current = lastReadedString.Replace(Tokens.EndOfSection.ToString(), "").Trim();

                if (Tokens.EndKeyNodes.Contains(current) || current.Contains("Request") || current.Contains("Response")) {

                    return (attributes, lastReadedString);
				}

                if (lastReadedString.Contains("+")) {
					var attr = new AttributeParser().Parse(current);
					attributes.Add(attr);   
                }

            } while (true);

        }

    }
}
