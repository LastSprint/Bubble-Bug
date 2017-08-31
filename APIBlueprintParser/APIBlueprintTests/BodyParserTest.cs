//
// BodyParserTest.cs
// 26.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//

using System;
using NUnit.Framework;
using APIBlueprintParser.Parsers.Action.Request;

namespace APIBlueprintTests {

    [TestFixture]
    public class BodyParserTest {

        [Test]
        public void ValidBodyTest() {
			var str = "key value";

			var stream = Extensions.CreatFromString(str);

            var result = new BodyParser(stream).Parse();

            Assert.AreEqual(str, result);
        }

        [Test]
        public void EmptyBodyTest() {
			var str = "";

			var stream = Extensions.CreatFromString(str);

			Assert.Throws<FormatException>(() => new HeadersParser(stream).Parse());
        }

        [Test]
        public void TestBodyWithSpecialCharacters() {

            var str = "{\"name\": \"Килька: в- тома+тном соу$се #00г\"}";

			var stream = Extensions.CreatFromString(str);

			var result = new BodyParser(stream).Parse();

			Assert.AreEqual(str, result);
		}
    }
}
