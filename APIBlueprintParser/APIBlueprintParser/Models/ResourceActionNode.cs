//
// ResourceActionNode.cs
// 21.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//

using System.Collections.Generic;

namespace APIBlueprintParser.Models {

    public class ResourceActionNode {

        public string Identifier { get; }
        public IReadOnlyCollection<AttributeNode> Attributes { get; }
        public IReadOnlyCollection<AttributeNode> Parameters { get; }

        public IReadOnlyCollection<RequestPair> RequestPairs { get; }
    }
}
