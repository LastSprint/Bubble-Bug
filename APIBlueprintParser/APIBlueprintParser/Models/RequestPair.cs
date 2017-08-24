//
// RequestPair.cs
// 21.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//
using System;
namespace APIBlueprintParser.Models {

    /// <summary>
    /// Object, that contain request and response for it.
    /// </summary>
	public class RequestPair: BaseNode {

        public RequestNode Request { get; }
        public ResponseNode Response { get; }

        public RequestPair(RequestNode request, ResponseNode response) {
            this.Request = request;
            this.Response = response;
        }
    }
}
