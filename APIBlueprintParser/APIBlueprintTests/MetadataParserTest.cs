//
// Test.cs
// 17.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//
using NUnit.Framework;
using APIBlueprintParser.Parsers;
using System.Collections.Generic;
using System;

namespace APIBlueprintTests {
    [TestFixture]
    public class MetadataParserTest {
        [Test]
        public void ParseWithSmallValidString() {
            // given

            var stream = Extensions.CreatFromString($"VERSION: 1A{System.Environment.NewLine}HOST:www.dfsdf.sdfs/sdfsdf/sdf{System.Environment.NewLine}#");

            // when

            var dict = new MetadataParser(stream).Parse();

            // then

            Assert.IsTrue(dict.Count == 2);
            Assert.AreEqual(dict["VERSION"], "1A");
            Assert.AreEqual(dict["HOST"], "www.dfsdf.sdfs/sdfsdf/sdf");
        }

        [Test]
        public void ParseWithHugeValidString() {

			// given 
			var keys = new string[] {
                "VERSION", "HOST", "AUTHOR", "EMAIL", "PHONE"
            };

            var dict = new Dictionary<string, string>() {
                {keys[0],"A1"},
                {keys[1],"ASGFGHASFFASGBDYF"},
                {keys[2], "AHDFAG!5123e56rfshgf"},
                {keys[3], "ASFDAHG%^$!^%RFASFJKF"},
                {keys[4], "%^!@&TUAKSFJA"}
            };

            var str = "";

            foreach (var key in keys) {
                str += $"{key}: {dict[key]}{System.Environment.NewLine}";
            }

            str += "#";

            // when

            var result = new MetadataParser(Extensions.CreatFromString(str)).Parse();

            // then

            Assert.IsTrue(result.Count == keys.Length);

            foreach (var key in keys) {
                Assert.AreEqual(result[key], dict[key]);
            }

        }

        [Test]
        public void ParseEmptyString() {
            // given

            var stream = Extensions.CreatFromString("");

            // when

            var result = new MetadataParser(stream).Parse();

            // then

            Assert.IsTrue(result.Count == 0);
        }

        [Test]
        public void ParseInfinitySection() {
			// given

			var stream = Extensions.CreatFromString($"VERSION: 1A{System.Environment.NewLine}HOST:www.dfsdf.sdfs/sdfsdf/sdf{System.Environment.NewLine}");

			// when

			var dict = new MetadataParser(stream).Parse();

			// then

			Assert.IsTrue(dict.Count == 2);
			Assert.AreEqual(dict["VERSION"], "1A");
			Assert.AreEqual(dict["HOST"], "www.dfsdf.sdfs/sdfsdf/sdf");
        }

        [Test]
        public void ParseWithInvalidPairsSeparator() {
			// given

			var stream = Extensions.CreatFromString($"VERSION 1AHOSTwww.dfsdf.sdfs/sdfsdf/sdf{System.Environment.NewLine}#");

            // then

            Assert.Throws<ArgumentException>(() => new MetadataParser(stream).Parse());
        }

        [Test]
        public void ParseWithInvalidKeyValueSeparator() {
			// given

			var stream = Extensions.CreatFromString($"VERSION* 1A{System.Environment.NewLine}HOST%www.dfsdf.sdfs/sdfsdf/sdf{System.Environment.NewLine}#");

			// then

			Assert.Throws<ArgumentException>(() => new MetadataParser(stream).Parse());
        }

    }
}
