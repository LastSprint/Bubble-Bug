//
// ApiDescriptionParserTest.cs
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
namespace APIBlueprintIntegrationTests.ApiDescription
{
    [TestFixture]
    public class ApiDescriptionParserTest
    {
        [Test]
        public void FullTest() {
			var stream = File.OpenRead("/Users/aleksandrkravcenkov/Repo/APIBlueprintParser/APIBlueprintParser/APIBlueprintIntegrationTests/ApiDescription/valid.apib");

            var result = new MainParser(new StreamReader(stream)).Parse();

			Assert.IsNull(null);
		}
    }
}
