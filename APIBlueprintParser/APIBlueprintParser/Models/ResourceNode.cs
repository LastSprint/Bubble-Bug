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
	public class ResourceNode: BaseNode {

        /// <summary>
        /// Gets the identifier of this resource
        /// </summary>
        public string Identifier { get; internal set; }

        /// <summary>
        /// URI template for this resource.
        /// </summary>
        public UriTemplate.Core.UriTemplate Template { get; internal set; }

        /// <summary>
        /// Actions of this resource.
        /// </summary>
        public IReadOnlyCollection<ResourceActionNode> Actions { get; internal set; }

        public ResourceNode(string identifier, UriTemplate.Core.UriTemplate template, ICollection<ResourceActionNode> actions) {
            this.Identifier = identifier;
            this.Template = template;
            this.Actions = new List<ResourceActionNode>(actions);
        }

        internal ResourceNode() { }
    }
}
