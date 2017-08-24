//
// AttributeNode.cs
// 21.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//
using System;
namespace APIBlueprintParser.Models {

    /// <summary>
    /// Attribute node.
    /// </summary>
	public class AttributeNode: BaseNode {

        /// <summary>
        /// Attribute name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Neededtype of the attribute.
        /// </summary>
        public NeededType NeededType { get; }

        /// <summary>
        /// Value type of the attribute.
        /// </summary>
        public ValueType ValueType { get; }

        /// <summary>
        /// Description of the attribute.
        /// </summary>
        public string Description { get; }

        public AttributeNode(string name, string description, NeededType neededType = NeededType.Required, ValueType valueType = ValueType.Object) {
            this.Name = name;
            this.NeededType = neededType;
            this.ValueType = valueType;
            this.Description = description;
        }
    }
}
