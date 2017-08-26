//
// MetadataNode.cs
// 21.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//

using System;
using System.Collections.Generic;

namespace APIBlueprintParser.Models {

    /// <summary>
    /// Present metadata section.
    /// </summary>
	public class MetadataNode: BaseNode {

        public IReadOnlyDictionary<string, string> Metadata;

        internal MetadataNode(IDictionary<string, string> metadata) {
            this.Metadata = new Dictionary<string, string>(metadata);
        }

        public static implicit operator MetadataNode(Dictionary<string, string> v)
        {
            throw new NotImplementedException();
        }
    }
}
