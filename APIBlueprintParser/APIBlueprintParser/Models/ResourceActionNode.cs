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

        public string Identifier { get; internal set;}
		public UriTemplate.Core.UriTemplate Template { get; internal set; }
		public HttpMethod HttpMethod { get; internal set; }

        public IReadOnlyCollection<AttributeNode> Attributes { get; internal set; }
        public IReadOnlyCollection<AttributeNode> Parameters { get; internal set; }

        public IReadOnlyCollection<RequestPair> RequestPairs { get; internal set; }

        public IReadOnlyCollection<ActionOption> Options { get; internal set; }

		internal ResourceActionNode(string identifier, UriTemplate.Core.UriTemplate template, HttpMethod httpMethod) {
			this.Identifier = identifier;
			this.Template = template;
			this.HttpMethod = httpMethod;
            this.Options = new List<ActionOption>();
		}

		public ResourceActionNode(string identifier, UriTemplate.Core.UriTemplate template, 
		                          HttpMethod httpMethod, ICollection<AttributeNode> attributes, 
		                          ICollection<AttributeNode> parameters, 
		                          ICollection<RequestPair> requestPairs): this(identifier, template, httpMethod) {
			this.Attributes = new List<AttributeNode>(attributes);
			this.Parameters = new List<AttributeNode>(parameters);
			this.RequestPairs = new List<RequestPair>(requestPairs);
		}

        internal ResourceActionNode() { } 
    }
}
