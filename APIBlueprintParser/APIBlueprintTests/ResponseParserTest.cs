//
// ResponseParser.cs
// 22.08.2017
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
    public class ResponseParserTest {

		[Test]
		public void ValidResponseTest() {
			// given
			var stream = Extensions.CreatFromString($"1234( application/json) {Environment.NewLine} body+");

			// when
			var result = new ResponseParser(stream).Parse();

			// then
            Assert.AreEqual(result.Code, 1234);
			Assert.AreEqual(result.BodyType, BodyType.Json);
			Assert.AreEqual(result.Body, "body");
		}

		[Test]
		public void InvalidResponseTypeTest() {
			// given
			var stream = Extensions.CreatFromString($"123( applic) {Environment.NewLine} body+");

			// when then
			Assert.Throws<FormatException>(() => new ResponseParser(stream).Parse());
		}

		[Test]
		public void InvalidFormatTypeDeclTest() {
			// given
			var stream1 = Extensions.CreatFromString($"123 (/json){Environment.NewLine} body+");
			var stream2 = Extensions.CreatFromString($"123 ( apicat) {Environment.NewLine} body+");

			// when then
			Assert.Throws<FormatException>(() => new ResponseParser(stream1).Parse());
			Assert.Throws<FormatException>(() => new ResponseParser(stream2).Parse());
		}

		[Test]
		public void InvalidFormatWithoutBodyTest() {
			// given
			var stream = Extensions.CreatFromString($"123 (application/json){Environment.NewLine}+");

			// when
			var result = new ResponseParser(stream).Parse();

			//then
			Assert.AreEqual(result.BodyType, BodyType.Json);
			Assert.AreEqual(result.Body, "");
		}

        [Test]
        public void InvalidFormatWitoutCode() {
			// given
			var stream = Extensions.CreatFromString($"(application/json){Environment.NewLine} body+");

			// when then
			Assert.Throws<FormatException>(() => new ResponseParser(stream).Parse());
        }

        [Test]
        public void InvalidFormatWithIvalidCode() {
			// given
			var stream = Extensions.CreatFromString($"fr45fet(application/json){Environment.NewLine} body+");

			// when then
			Assert.Throws<FormatException>(() => new ResponseParser(stream).Parse());
        }
    }
}
