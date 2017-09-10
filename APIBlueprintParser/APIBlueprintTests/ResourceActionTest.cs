//
// ResourceActionTest.cs
// 23.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//

using System;
using NUnit.Framework;
using APIBlueprintParser.Parsers.Action;
using APIBlueprintParser.Models;
using UriTemplate.Core;
using System.Linq;

namespace APIBlueprintTests {

	[TestFixture]
	public class ResourceActionTest {

		[Test]
		public void FullValidActionResourceTest() {
			
			// given
			var httpMethod = HttpMethod.Get;
			var template = "/samplePath/method";
			var identifier = "SampleResourceAction";
			var action = 
				$"This is sample action for unit test. Fuck off {Environment.NewLine}" +
				$"+ Attributes {Environment.NewLine}\t" +
				"+ la-la-la";
			var stream = Extensions.CreatFromString(action);

			// when
            var result = new ResourceActionParser(stream, $"{identifier} [GET, {template}]").ParseWithoutNestenNodes();

			// then

			Assert.AreEqual(result.Identifier, identifier);
			Assert.AreEqual(result.HttpMethod, httpMethod);
			Assert.AreEqual(result.Template.Template, template);

		}

        [Test]
        public void ActionWithOptionsTest() {
			// given
			var httpMethod = HttpMethod.Get;
			var template = "/samplePath/method";
			var identifier = "SampleResourceAction";
            var options = $"+ Options {Environment.NewLine} + Iterative";
			var action =
				$"+ Attributes {Environment.NewLine}\t" +
                $"+ lalala (required, object){Environment.NewLine} {options}";
			var stream = Extensions.CreatFromString(action);

            // when
            var result = new ResourceActionParser(stream, $"{identifier} [GET, {template}]").Parse().resourceAction;

			// then

			Assert.AreEqual(result.Identifier, identifier);
			Assert.AreEqual(result.HttpMethod, httpMethod);
			Assert.AreEqual(result.Template.Template, template);
            Assert.AreEqual(result.Options.First(), ActionOption.Iterative);
        }

		[Test]
		public void IvalidHttpMethodTest() {

			// given
			var template = "/samplePath/method";
			var identifier = "SampleResourceAction";
			var action =
				$"This is sample action for unit test. Fuck off {Environment.NewLine}" +
				$"+ Attributes {Environment.NewLine}\t" +
				"+ la-la-la";
			var stream = Extensions.CreatFromString(action);

			// when then
			Assert.Throws<FormatException>(() => new ResourceActionParser(stream, $"{identifier} [NuGet, {template}]").ParseWithoutNestenNodes());
		}

		[Test]
		public void IvalidTemplateTest() {

			// given
			var template = "/samplePath/met hod";
			var identifier = "SampleResourceAction";
			var action =
				$"This is sample action for unit test. Fuck off {Environment.NewLine}" +
				$"+ Attributes {Environment.NewLine}\t" +
				"+ la-la-la";
			var stream = Extensions.CreatFromString(action);

			// when then
			Assert.Throws<FormatException>(() => new ResourceActionParser(stream, $"{identifier} [NuGet, {template}]").ParseWithoutNestenNodes());
		}

	}
}
