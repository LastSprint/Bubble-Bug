//
// Response.cs
// 21.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//
using System.Collections.Generic;
namespace APIBlueprintParser.Models {

	public class ResponseNode: BaseNode {

        /// <summary>
        /// Response code.
        /// </summary>
        public int Code { get; internal set; }

        /// <summary>
        /// Type of the response body. For example 'application/json'
        /// </summary>
        public BodyType BodyType { get; internal set; }

        /// <summary>
        /// Body of the response.
        /// </summary>
        public string Body { get; internal set; }

        public IReadOnlyDictionary<string, string> Headers { get; internal set; }

        public ResponseNode(int code, BodyType bodyType, string body) {
            this.Code = code;
            this.BodyType = bodyType;
            this.Body = body;
        }

        public ResponseNode() { }
    }
}
