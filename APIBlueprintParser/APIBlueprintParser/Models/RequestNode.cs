//
// RequestNode.cs
// 21.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//

using System.Collections.Generic;

namespace APIBlueprintParser.Models {

    public class RequestNode : BaseNode {

        public IReadOnlyDictionary<string, string> Headers { get; internal set; }

        public BodyType BodyType { get; internal set; }

        public string Identifier { get; internal set; }

        public string Body { get; internal set; }

        public string Schema { get; internal set; }

        public IReadOnlyDictionary<string, string> Parameters { get; internal set; }

        public RequestNode(BodyType bodyType, string body, IDictionary<string, string> headers, string schema, string identifier) {
            this.BodyType = bodyType;
            this.Body = body;
            this.Headers = new Dictionary<string, string>(headers);
            this.Schema = schema;
            this.Identifier = identifier;
        }

        internal RequestNode() {}
    }
}
