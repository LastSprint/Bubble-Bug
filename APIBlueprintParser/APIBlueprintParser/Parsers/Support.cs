//
// Extensions.cs
// 22.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//
using System;
using APIBlueprintParser.Models;

namespace APIBlueprintParser.Parsers {

    /// <summary>
    /// This class contains different support mehod.
    /// </summary>
    public static class Support {

        /// <summary>
        /// Try to convert string to <see cref="T:APIBlueprint.Models.BodyType"/>
        /// </summary>
        /// <returns>Type of the body content or null if parse failed.</returns>
        /// <param name="content">String view of content type.</param>
        public static BodyType? StringToBodyType(string content) {
            if (String.Compare(content, Constants.JsonContentType, true) == 0 ) {
                return BodyType.Json;
            }

            return null;
        }

    }
}
