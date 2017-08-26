//
// GroupNode.cs
// 21.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//
using System.Collections.Generic;

namespace APIBlueprintParser.Models {
    
    /// <summary>
    /// Present resource group section.
    /// </summary>
	public class GroupNode: BaseNode {

        /// <summary>
        /// Name of group.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Collection of the resources of this group.
        /// </summary>
        public IReadOnlyCollection<ResourceNode> Resources { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:APIBlueprintParser.Models.GroupNode"/> class.
        /// </summary>
        /// <param name="resources">Resources collection</param>
        public GroupNode(ICollection<ResourceNode> resources) {
            this.Resources = new List<ResourceNode>(resources);
        }

        internal GroupNode() { }
    }
}
