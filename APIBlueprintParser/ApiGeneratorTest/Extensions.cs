//
// Extensions.cs
// 27.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//
using System;
using APIBlueprintParser.Models;

namespace ApiGeneratorTest {
    public static class Extensions {
        public static string MethodToString(this HttpMethod method) {
            switch (method) {
                case HttpMethod.Get:
                    return "HttpGet";
                case HttpMethod.Put:
                    return "HttpPut";
                case HttpMethod.Delete:
                    return "HttpDelete";
                case HttpMethod.Post:
                    return "HttpPost";
                default:
                    throw new ArgumentOutOfRangeException(nameof(method), "Cant convert current http method to string");
            }
        }
    }
}
