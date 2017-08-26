//
// HeaderParserTest.cs
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
    public class HeaderParserTest {

        [Test]
        public void ValidHeadersSectionTest() {
            string[] keys = { "key1", "key2", "key3" };
            string[] values = { "value1", "value2", "value3" };

            var str = "";

            for (int i = 0; i < keys.Length; i++) {
                str += $"{keys[i]}:{values[i]} {Environment.NewLine}";
            }

            var stream = Extensions.CreatFromString(str);

            var result = new HeadersParser(stream).Parse();

            Assert.AreEqual(result.Count, keys.Length);

			for (int i = 0; i < keys.Length; i++) {
                Assert.AreEqual(result[keys[i]], values[i]);
			}
        }

        [Test]
        public void InvalidKeyValueHeaderTest() {
            var str = "key value";

			var stream = Extensions.CreatFromString(str);

            Assert.Throws<FormatException>( () => new HeadersParser(stream).Parse());
        }

        [Test]
		public void EmptyHeaderTest() {
			var str = "";

			var stream = Extensions.CreatFromString(str);

			Assert.Throws<FormatException>(() => new HeadersParser(stream).Parse());
		}

    }
}
