//
// GroupParser.cs
// 26.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//
using System;
using System.IO;
using APIBlueprintParser.Models;
using System.Linq;
using System.Collections.Generic;

namespace APIBlueprintParser.Parsers {
    
    public class GroupParser: BaseParser {

        private string _declaration;

        public GroupParser(StreamReader streamReader, string declaration): base(streamReader) {
            this._declaration = declaration;
        }

        public (GroupNode groupNode, string lastReaded) Parse() {

            var result = this.ParseDeclaration();

            var resources = new List<ResourceNode>();

            var line = base.streamReader.ReadLine();

            do {

                if (line == null || line.Contains("#") && !line.Contains("##")) {
                    result.Resources = resources;
                    return (result, line);
                }

                if (line.Contains("##")) {
                    var res = new ResourceParser(base.streamReader, line).Parse();
                    line = res.lastReaded;
                    resources.Add(res.resource);
                }

                if (line != null && !line.Contains("#")) {
                    line = streamReader.ReadLine().Trim(); 
                }

            } while (true);
        }

        private GroupNode ParseDeclaration() {

            var words = this._declaration.Words().Where(x => x.Trim() != "#" && x.Length != 0).ToList();

            if (words.Count() < 2) {
                throw new FormatException("Group declaration must contains 2 sections");
            }

            if (words[0] != "Group") {
                throw new FormatException("Group declaration doesnt contain Group keyword");
            }

            words.RemoveAt(0);

            var result = new GroupNode();
            result.Name = words.Aggregate((x,y) => x + " "+ y);

            return result;
        }

    }
}
