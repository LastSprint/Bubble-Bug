//
// Extensions.cs
// 22.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//
using System;
using APIBlueprintParser.Models;
using ValueType = APIBlueprintParser.Models.ValueType;

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

		/// <summary>
		/// Try to convert string to <see cref="T: APIBlueprintParser.Models.ValueType"/>
		/// </summary>
		/// <returns>Value type or null if parse failed.</returns>
		/// <param name="valueType">String view of value type.</param>
		public static ValueType? StringToValueType(string valueType) {
            var lower = valueType.ToLower();

            switch (lower) {
                case Constants.ValueTypes.Object: return ValueType.Object;
                case Constants.ValueTypes.Number: return ValueType.Number;
                case Constants.ValueTypes.String: return ValueType.String;
                case Constants.ValueTypes.Bool: return ValueType.Bool;
                default: return null;
            }
        }

        /// <summary>
        /// Try to convert string to <see cref="T:APIBlueprintParser.Models.NeededType"/>
        /// </summary>
        /// <returns>Needed type or null of parse failed.</returns>
        /// <param name="neededType">String view of needed type</param>
        public static NeededType? StringToNeededType(string neededType) {
			var lower = neededType.ToLower();

			switch (lower) {
                case Constants.NeededType.Optional: return NeededType.Optional;
                case Constants.NeededType.Required: return NeededType.Required;
				default: return null;
			}
        }

    }
}
