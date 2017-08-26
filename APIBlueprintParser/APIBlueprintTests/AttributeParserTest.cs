//
// AttributeParserTest.cs
// 22.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//

using System;
using NUnit.Framework;
using APIBlueprintParser.Models;
using ValueType = APIBlueprintParser.Models.ValueType;
using APIBlueprintParser.Parsers.Action.Attributes;

namespace APIBlueprintTests {
	
	[TestFixture]
    public class AttributeParserTest {

        [Test]
        public void TestWithValidAttribute() {
            // given
            var name = "sample";
			var stream = $"{name}(optional, object) - description {Environment.NewLine}";

			// when
            var result = new AttributeParser().Parse(stream);

            // then
            Assert.AreEqual(result.Name, name);
            Assert.AreEqual(result.Description, "description");
            Assert.AreEqual(result.ValueType, ValueType.Object);
            Assert.AreEqual(result.NeededType, NeededType.Optional);
        }

		[Test]
		public void InvalidNeededTypeTest() {
			// given
			var name = "sample";
			var stream = $"{name}(6rtyw, string) - description {Environment.NewLine}";

			// when then
            Assert.Throws<FormatException>(() => new AttributeParser().Parse(stream));
		}

		[Test]
		public void InvalidValueTypeTest() {
			// given
			var name = "sample";
			var stream = $" {name}(optional, 23wef) - description {Environment.NewLine}";

			// when then
			Assert.Throws<FormatException>(() => new AttributeParser().Parse(stream));
		}

		[Test]
		public void WithoutDescriptionTest() {
			// given
			var name = "sample";
			var stream = $" {name} (Required, bool) {Environment.NewLine}";

			// when
			var result = new AttributeParser().Parse(stream);

			// then
			Assert.AreEqual(result.Name, name);
			Assert.AreEqual(result.Description, "");
			Assert.AreEqual(result.ValueType, ValueType.Bool);
			Assert.AreEqual(result.NeededType, NeededType.Required);
		}
    }
}
