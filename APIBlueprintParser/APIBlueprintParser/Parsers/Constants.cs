//
// Constants.cs
// 22.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//
using System;
namespace APIBlueprintParser.Parsers {
    public static class Constants {
        public const string JsonContentType = "Application/json";

        public struct ValueTypes {
            public const string Object = "object";
            public const string String = "string";
            public const string Number = "number";
            public const string Bool = "bool";
        }

        public struct NeededType {
            public const string Required = "required";
            public const string Optional = "optional";
        }
    }
}
