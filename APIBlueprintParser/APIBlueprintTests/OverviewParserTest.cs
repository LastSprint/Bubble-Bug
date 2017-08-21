//
// OverviewParserTest.cs
// 21.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//
using System;
using NUnit.Framework;
using APIBlueprintParser.Parsers;
using APIBlueprintParser.Models;

namespace APIBlueprintTests {

    [TestFixture]
    public class OverviewParserTest {

        [Test]
        public void ValidOverviewTest() {
            //given
            var stream = Extensions.CreatFromString($"# This is sample API {Environment.NewLine}This API example demonstrates how to define a resource with multiple actions.{Environment.NewLine}#");
            var must = new OverviewNode("This is sample API", "This API example demonstrates how to define a resource with multiple actions.");

            // when
            var parsed = new OverviewParser(stream).Parse();

            // then

            Assert.AreEqual(must.Name, parsed.Name);
            Assert.AreEqual(must.Overview, parsed.Overview);
        }

        [Test]
        public void InvalidViewviewTest() {
			//given
			var stream = Extensions.CreatFromString($"This is sample API {Environment.NewLine}This API example demonstrates how to define a resource with multiple actions.{Environment.NewLine}#");

            // when then
            Assert.Throws<FormatException>(() => new OverviewParser(stream).Parse());
        }
    }
}
