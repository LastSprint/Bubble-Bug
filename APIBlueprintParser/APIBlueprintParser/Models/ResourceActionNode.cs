//
// ResourceActionNode.cs
// 21.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//

using System.Collections.Generic;

namespace APIBlueprintParser.Models {

	public class ResourceActionNode: BaseNode {

        public string Identifier { get; }
		public UriTemplate.Core.UriTemplate Template { get; }
		public HttpMethod HttpMethod { get; }

        public IReadOnlyCollection<AttributeNode> Attributes { get; }
        public IReadOnlyCollection<AttributeNode> Parameters { get; }

        public IReadOnlyCollection<RequestPair> RequestPairs { get; }

		internal ResourceActionNode(string identifier, UriTemplate.Core.UriTemplate template, HttpMethod httpMethod) {
			this.Identifier = identifier;
			this.Template = template;
			this.HttpMethod = HttpMethod;
		}

		public ResourceActionNode(string identifier, UriTemplate.Core.UriTemplate template, 
		                          HttpMethod httpMethod, ICollection<AttributeNode> attributes, 
		                          ICollection<AttributeNode> parameters, 
		                          ICollection<RequestPair> requestPairs): this(identifier, template, httpMethod) {
			this.Attributes = new List<AttributeNode>(attributes);
			this.Parameters = new List<AttributeNode>(parameters);
			this.RequestPairs = new List<AttributeNode>(requestPairs);
		}
    }
}
