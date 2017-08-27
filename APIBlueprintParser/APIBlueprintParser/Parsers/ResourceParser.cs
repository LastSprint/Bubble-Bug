//
// ResourceParser.cs
// 26.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using APIBlueprintParser.Models;
using APIBlueprintParser.Parsers.Action;

namespace APIBlueprintParser.Parsers {
    
    public class ResourceParser: BaseParser {

        private string _declaration;

        public ResourceParser(StreamReader streamReader, string decalration): base(streamReader) {
            this._declaration = decalration;
        }

        public (ResourceNode resource, string lastReaded) Parse() {

            var resource = this.ParseDeclaration();
            var actions = new List<ResourceActionNode>();
            var line = base.streamReader.ReadLine();

            while (true) {

                if (line.Contains("###")) {
                    var result = new ResourceActionParser(base.streamReader, line).Parse();
                    line = result.lastReaded;
                    actions.Add(result.resourceAction);
                }

				if (line == null)
				{
					resource.Actions = actions;
					return (resource, line);
				}


                if ((line.Contains("##") || line.Contains("#")) && !line.Contains("###")) {
                    resource.Actions = actions;
                    return (resource, line);
                }

                if (!line.Contains("###") || !line.Contains("##") || !line.Contains("#")) {
                    line = streamReader.ReadLine().Trim();
                }
            }
        }

        private ResourceNode ParseDeclaration() {

            var words = this._declaration.Words().Where(x => x.Length != 0 && x != "##").ToList();

            if (words.Count() < 2) {
                throw new FormatException("Resource declaration doesnt contains 2 sections");
            }

            var templ = words.Last();

            if (!Support.IsURI(templ)) {
                throw new FormatException("Resource parser cant parse url template");
            }

			words.Remove(words.Last());

			var identifier = words.Aggregate((x, y) => x + " " + y);

            var result = new ResourceNode();
            result.Identifier = identifier;
            result.Template = new UriTemplate.Core.UriTemplate(templ);

            return result;
        }
    }
}
