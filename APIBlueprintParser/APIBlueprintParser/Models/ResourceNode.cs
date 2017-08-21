//
// ResourceNode.cs
// 21.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//

using System.Collections.Generic;

namespace APIBlueprintParser.Models {

    /// <summary>
    /// Present resource section.
    /// </summary>
    public class ResourceNode {

        /// <summary>
        /// Gets the identifier of this resource
        /// </summary>
        public string Identifier { get; }

        /// <summary>
        /// URI template for this resource.
        /// </summary>
        public UriTemplate.Core.UriTemplate Template { get; }

        /// <summary>
        /// Actions of this resource.
        /// </summary>
        public IReadOnlyCollection<ResourceActionNode> Actions { get; }

        public ResourceNode(string identifier, UriTemplate.Core.UriTemplate template, ICollection<ResourceActionNode> actions) {
            this.Identifier = identifier;
            this.Template = template;
            this.Actions = new List<ResourceActionNode>(actions);
        }
    }
}
