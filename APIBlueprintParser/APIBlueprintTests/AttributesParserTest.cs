//
// AttributesParser.cs
// 26.08.2017
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
    public class AttributesParserTest {

        [Test]
        public void FullSectionTest() {
            var attr1 = new AttributeNode("attr1", "description1", NeededType.Optional, ValueType.Bool);
            var attr2 = new AttributeNode("attr2", "description2", NeededType.Required, ValueType.Object);
            var attr3 = new AttributeNode("attr3", "description3", NeededType.Optional, ValueType.Number);
            var attr4 = new AttributeNode("attr4", "description4", NeededType.Required, ValueType.String);

            AttributeNode[] attrs = { attr1, attr2, attr3, attr4 };

            var str = "";

            foreach (var attr in attrs) {
                str += $"+ {attr.Name} ({attr.NeededType}, {attr.ValueType}) {attr.Description} {Environment.NewLine}";
            }
            str += "+ Parameters";

            var stream = Extensions.CreatFromString(str);

            var result = new AttributesParser(stream).Parse().attributes;

            Assert.AreEqual(result.Count, attrs.Length, "Result length != attrs length");

            for (int i = 0; i < result.Count; i++) {
                Assert.AreEqual(result[i].Name, attrs[i].Name);
                Assert.AreEqual(result[i].Description, attrs[i].Description);
                Assert.AreEqual(result[i].NeededType, attrs[i].NeededType);
                Assert.AreEqual(result[i].ValueType, attrs[i].ValueType);
            }
        }

		[Test]
		public void WithoutDescriptionSectionTest()
		{
			var attr1 = new AttributeNode("attr1", "", NeededType.Optional, ValueType.Bool);
			var attr2 = new AttributeNode("attr2", "", NeededType.Required, ValueType.Object);
			var attr3 = new AttributeNode("attr3", "", NeededType.Optional, ValueType.Number);
			var attr4 = new AttributeNode("attr4", "", NeededType.Required, ValueType.String);

			AttributeNode[] attrs = { attr1, attr2, attr3, attr4 };

			var str = "";

			foreach (var attr in attrs)
			{
				str += $"+ {attr.Name} ({attr.NeededType}, {attr.ValueType}){Environment.NewLine}";
			}
			str += "+ Parameters";

			var stream = Extensions.CreatFromString(str);

			var result = new AttributesParser(stream).Parse().attributes;

			Assert.AreEqual(result.Count, attrs.Length, "Result length != attrs length");

			for (int i = 0; i < result.Count; i++)
			{
				Assert.AreEqual(result[i].Name, attrs[i].Name);
				Assert.AreEqual(result[i].Description, attrs[i].Description);
				Assert.AreEqual(result[i].NeededType, attrs[i].NeededType);
				Assert.AreEqual(result[i].ValueType, attrs[i].ValueType);
			}
		}
    }
}
