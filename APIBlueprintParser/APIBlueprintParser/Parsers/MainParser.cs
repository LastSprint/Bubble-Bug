//
// MainParser.cs
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
    
    public class MainParser: BaseParser {

		public const string ProjectNameKey = "NAME";

        public MainParser(StreamReader reader): base(reader) { }

        public ApiDescription Parse() {
            
            var metadata = new MetadataParser(base.streamReader).Parse();
            var overview = new OverviewParser(base.streamReader).Parse();

            var result = new ApiDescription();

            if (metadata.TryGetValue(ProjectNameKey, out string projectname)) {
                result.Projectname = projectname;
            }

            result.Metadata = new MetadataNode(metadata);
            result.Overview = overview;

            var groups = new List<GroupNode>();

            var line = base.streamReader.ReadLine();

            do
            {

                if (line == null)
                {
                    result.Groups = groups;
                    return result;
                }

                if (!line.Contains("#"))
                {
                    line = base.streamReader.ReadLine();
                }

                if (line.Contains("#"))
                {
                    var res = new GroupParser(base.streamReader, line).Parse();
                    line = res.lastReaded;
                    groups.Add(res.groupNode);
                }

            } while (true);
        }
    }
}
