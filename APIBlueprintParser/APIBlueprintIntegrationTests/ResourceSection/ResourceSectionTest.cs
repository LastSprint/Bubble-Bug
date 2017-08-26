//
// ResourceSectionTest.cs
// 26.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//

using NUnit.Framework;
using System;
using APIBlueprintParser.Parsers;
using APIBlueprintParser.Models;
using System.IO;

namespace APIBlueprintIntegrationTests {

    [TestFixture]
    public class ResourceSectionTest {

        [Test]
        public void FullTest() {
			var stream = File.OpenRead("/Users/aleksandrkravcenkov/Repo/APIBlueprintParser/APIBlueprintParser/APIBlueprintIntegrationTests/ResourceSection/valid.apib");

            var result = new ResourceParser(new StreamReader(stream), "## identifier [/resource]").Parse();

			Assert.IsNull(null);
        }
    }
}
