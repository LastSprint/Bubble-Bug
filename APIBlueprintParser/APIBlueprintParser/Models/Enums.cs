//
// Enums.cs
// 22.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//

namespace APIBlueprintParser.Models {
	/// <summary>
	/// Type of the attribute.
	/// </summary>
	public enum NeededType {
		Optional,
		Required
	}

	/// <summary>
	/// Value type of the attribute.
	/// </summary>
	public enum ValueType {
		Object,
		String,
		Number,
		Bool
	}

    /// <summary>
    /// Type of content that contains in the request/response body
    /// </summary>
	public enum BodyType {
		Json
	}
}
