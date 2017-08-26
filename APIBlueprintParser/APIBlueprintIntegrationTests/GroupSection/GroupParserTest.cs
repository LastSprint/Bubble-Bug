//
// GroupParserTest.cs
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
namespace APIBlueprintIntegrationTests.GroupSection
{
    [TestFixture]
    public class GroupParserTest
    {
        [Test]
        public void FullTest() {
			var stream = File.OpenRead("/Users/aleksandrkravcenkov/Repo/APIBlueprintParser/APIBlueprintParser/APIBlueprintIntegrationTests/GroupSection/valid.apib");

            var result = new GroupParser(new StreamReader(stream), "# Group name").Parse();

			Assert.IsNull(null);
        }
    }
}
