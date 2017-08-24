//
// OverviewNode.cs
// 21.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//

namespace APIBlueprintParser.Models {

    /// <summary>
    /// Present overview section
    /// </summary>
	public class OverviewNode: BaseNode {

        /// <summary>
        /// Name of API.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Overviw for this API.
        /// </summary>
        public string Overview { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:APIBlueprintParser.Models.OverviewNode"/> class.
        /// </summary>
        /// <param name="name">Name of this API.</param>
        /// <param name="overview">Overview of this API.</param>
        public OverviewNode(string name, string overview) {
            this.Name = name;
            this.Overview = overview;
        }
    }
}
