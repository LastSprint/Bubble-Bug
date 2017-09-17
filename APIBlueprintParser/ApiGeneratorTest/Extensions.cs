//
// Extensions.cs
// 27.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//
using System;
using APIBlueprintParser.Models;
using ValueType = APIBlueprintParser.Models.ValueType;

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

        public static string TypeToString(this ValueType valueType) {
            switch (valueType) {
                case ValueType.Bool:
                    return "bool";
                case ValueType.Number:
                    return "double";
                case ValueType.Object:
                    return "object";
                case ValueType.String:
                    return "string";
                case ValueType.Long:
                    return "long";
                default: throw new ArgumentOutOfRangeException(nameof(valueType), "Cant convert value type to string");
            }
        }

    }
}
