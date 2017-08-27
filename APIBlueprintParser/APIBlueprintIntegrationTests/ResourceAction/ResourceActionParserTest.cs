//
// Test.cs
// 26.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//
using NUnit.Framework;
using System;
using APIBlueprintParser.Parsers.Action;
using APIBlueprintParser.Models;
using System.IO;

namespace APIBlueprintIntegrationTests.ResourceAction {

    // TODO: Write full integration test

    [TestFixture]
    public class ResourceActionParserTest {
        
        [Test]
        public void ValidDataParsingTest() {

            var stream = File.OpenRead("/Users/aleksandrkravcenkov/Repo/APIBlueprintParser/APIBlueprintParser/APIBlueprintIntegrationTests/ResourceAction/valid.apib");

            var result = new ResourceActionParser(new StreamReader(stream), "### sample [get, /name]").Parse();

            Assert.IsNull(null);
        }

		[Test]
		public void MultiWordsIdentifierTest(){

			var stream = File.OpenRead("/Users/aleksandrkravcenkov/Repo/APIBlueprintParser/APIBlueprintParser/APIBlueprintIntegrationTests/ResourceAction/valid.apib");

			var result = new ResourceActionParser(new StreamReader(stream), "### sample fucking name [get, /name]").Parse();

            Assert.AreEqual(result.resourceAction.Identifier.Trim(), "sample fucking name");
		}
    }
}
