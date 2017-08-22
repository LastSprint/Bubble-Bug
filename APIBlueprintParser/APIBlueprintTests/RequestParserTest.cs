//
// RequestParserTest.cs
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
    public class RequestParserTest {

        [Test]
        public void ValidRequestTest() {
            // given
            var stream = Extensions.CreatFromString($"+ Request( application/json) {Environment.NewLine} body+");

            // when
            var result = new RequestParser(stream).Parse();

            // then
            Assert.AreEqual(result.BodyType, BodyType.Json);
            Assert.AreEqual(result.Body, "body");
        }

        [Test]
        public void InvalidRequestTypeTest() {
			// given
			var stream = Extensions.CreatFromString($"+ Request( applic) {Environment.NewLine} body+");

            // when then
            Assert.Throws<FormatException>(() => new RequestParser(stream).Parse());
        }

        [Test]
        public void InvalidFormatTypeDeclTest() {
			// given
			var stream1 = Extensions.CreatFromString($"+ Request (/json){Environment.NewLine} body+");
            var stream2 = Extensions.CreatFromString($"+ Request( apicat) {Environment.NewLine} body+");

			// when then
			Assert.Throws<FormatException>(() => new RequestParser(stream1).Parse());
            Assert.Throws<FormatException>(() => new RequestParser(stream2).Parse());
        }

        [Test]
        public void InvalidFormatWithoutBodyTest() {
			// given
			var stream = Extensions.CreatFromString($"+ Request (application/json){Environment.NewLine}+");

            // when
            var result = new RequestParser(stream).Parse();

            //then
            Assert.AreEqual(result.BodyType, BodyType.Json);
            Assert.AreEqual(result.Body, "");
        }

    }
}
