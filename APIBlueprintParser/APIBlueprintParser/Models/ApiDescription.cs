//
// ApiDescription.cs
// 26.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//
using System;
using System.Collections.Generic;
using System.Linq;

namespace APIBlueprintParser.Models
{
    public class ApiDescription : BaseNode
    {
        public MetadataNode Metadata { get; internal set; }
        public OverviewNode Overview { get; internal set; }
        public string Projectname { get; internal set; }

        public IReadOnlyCollection<GroupNode> Groups { get; internal set; }

        internal ApiDescription() { }
    }
}
