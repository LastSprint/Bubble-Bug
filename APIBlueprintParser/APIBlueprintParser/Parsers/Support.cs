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
        public static BodyType StringToBodyType(string content) {
            if (String.Compare(content, Constants.JsonContentType, true) == 0 ) {
                return BodyType.Json;
            }
            return BodyType.Empty;
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
                case Constants.ValueTypes.Long: return ValueType.Long;
                default: return null;
            }
        }

        /// <summary>
        /// Try to convert string to <see cref="T:APIBlueprintParser.Models.NeededType"/>
        /// </summary>
        /// <returns>Needed type or null if parse failed.</returns>
        /// <param name="neededType">String view of needed type.</param>
        public static NeededType? StringToNeededType(string neededType) {
			var lower = neededType.ToLower();

			switch (lower) {
                case Constants.NeededTypes.Optional: return NeededType.Optional;
                case Constants.NeededTypes.Required: return NeededType.Required;
				default: return null;
			}
        }

		/// <summary>
		/// Try to convert string to <see cref="T:APIBlueprintParser.Models.HttpMethod"/>
		/// </summary>
		/// <returns>Http method type or null if parse failed.</returns>
		/// <param name="httpMethod">String view of http method.</param>
		public static HttpMethod? StringToHttpMethod(string httpMethod) {
			var lower = httpMethod.ToLower();

			switch (lower) {
				case Constants.HttpMethods.Get: return HttpMethod.Get;
				case Constants.HttpMethods.Post: return HttpMethod.Post;
				case Constants.HttpMethods.Put: return HttpMethod.Put;
				case Constants.HttpMethods.Delete: return HttpMethod.Delete;
				default: return null;
			}
		}

        public static ActionOption? StringToActionOption(string str) {
            switch(str.Trim().ToLower()) {
                case "iterative":
                    return ActionOption.Iterative;
                default: return null;
            }
        }

		public static bool IsURI(string str) {
			if (str[0] != '/') {
				return false;
			}

			try {
				var t = new UriTemplate.Core.UriTemplate(str);
			}
			catch {
				return false;
			}
			return true;
		}
    }
}
