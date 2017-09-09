//
// RequestParserTest.cs
// 22.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//
using System;
using NUnit.Framework;
using APIBlueprintParser.Models;
using APIBlueprintParser.Parsers.Action.Request;

namespace APIBlueprintTests {

    [TestFixture]
    public class RequestParserTest {

        [Test]
        public void FullDeclarationTest() {
            var identifier = "identifier";
            var mediaType = "application/json";
            var headerKey1 = "key";
            var headerValue1 = "value";
			var headerKey2 = "key1";
			var headerValue2 = "value2";
            var body = "body";
            var schema = "schema";

            var declaration = $"Request {identifier} ({mediaType})";
            var bodyRequst = $"+ Headers {Environment.NewLine}" +
                $"{headerKey1}:{headerValue1}{Environment.NewLine}" +
                $"{headerKey2} : {headerValue2} {Environment.NewLine}" +
                $"+ Body {Environment.NewLine}" +
                $"{body}{Environment.NewLine}" +
                $"+ Schema {Environment.NewLine}" +
                $"{schema}{Environment.NewLine}" +
                "+ Response";

            var stream = Extensions.CreatFromString(bodyRequst);

            var result = new RequestParser(stream, declaration).Parse().request;

            Assert.AreEqual(result.Identifier, identifier);
            Assert.AreEqual(result.Body, body);
            Assert.AreEqual(result.Schema, schema);
            Assert.AreEqual(result.BodyType, BodyType.Json);
            Assert.AreEqual(result.Headers[headerKey1], headerValue1);
            Assert.AreEqual(result.Headers[headerKey2], headerValue2);
        }

        [Test]
        public void WithoutIdentifierTest() {
			var mediaType = "application/json";
			var headerKey1 = "key";
			var headerValue1 = "value";
			var headerKey2 = "key1";
			var headerValue2 = "value2";
			var body = "body";
			var schema = "schema";

			var declaration = $"Request({mediaType})";
			var bodyRequst = $"+ Headers {Environment.NewLine}" +
				$"{headerKey1}:{headerValue1}{Environment.NewLine}" +
				$"{headerKey2} : {headerValue2} {Environment.NewLine}" +
				$"+ Body {Environment.NewLine}" +
				$"{body}{Environment.NewLine}" +
				$"+ Schema {Environment.NewLine}" +
				$"{schema}{Environment.NewLine}" +
				"+ Response";

			var stream = Extensions.CreatFromString(bodyRequst);

			var result = new RequestParser(stream, declaration).Parse().request;

			Assert.AreEqual(result.Identifier, "");
			Assert.AreEqual(result.Body, body);
			Assert.AreEqual(result.Schema, schema);
			Assert.AreEqual(result.BodyType, BodyType.Json);
			Assert.AreEqual(result.Headers[headerKey1], headerValue1);
			Assert.AreEqual(result.Headers[headerKey2], headerValue2);
        }

		[Test]
		public void WithoutheadersTest() {
			var identifier = "identifier";
			var mediaType = "application/json";
			var body = "body";
			var schema = "schema";

			var declaration = $"Request {identifier} ({mediaType})";
			var bodyRequst = 
				$"+ Body {Environment.NewLine}" +
				$"{body}{Environment.NewLine}" +
				$"+ Schema {Environment.NewLine}" +
				$"{schema}{Environment.NewLine}" +
				"+ Response";

			var stream = Extensions.CreatFromString(bodyRequst);

			var result = new RequestParser(stream, declaration).Parse().request;

			Assert.AreEqual(result.Identifier, identifier);
			Assert.AreEqual(result.Body, body);
			Assert.AreEqual(result.Schema, schema);
			Assert.AreEqual(result.BodyType, BodyType.Json);
            Assert.IsNull(result.Headers);
		}

		[Test]
		public void WithoutBodyTest() {
			var identifier = "identifier";
			var mediaType = "application/json";
			var headerKey1 = "key";
			var headerValue1 = "value";
			var headerKey2 = "key1";
			var headerValue2 = "value2";
			var schema = "schema";

			var declaration = $"Request {identifier} ({mediaType})";
			var bodyRequst = $"+ Headers {Environment.NewLine}" +
				$"{headerKey1}:{headerValue1}{Environment.NewLine}" +
				$"{headerKey2} : {headerValue2} {Environment.NewLine}" +
				$"+ Schema {Environment.NewLine}" +
				$"{schema}{Environment.NewLine}" +
				"+ Response";

			var stream = Extensions.CreatFromString(bodyRequst);

			var result = new RequestParser(stream, declaration).Parse().request;

			Assert.AreEqual(result.Identifier, identifier);
            Assert.IsNull(result.Body);
			Assert.AreEqual(result.Schema, schema);
			Assert.AreEqual(result.BodyType, BodyType.Json);
			Assert.AreEqual(result.Headers[headerKey1], headerValue1);
			Assert.AreEqual(result.Headers[headerKey2], headerValue2);
		}

		[Test]
		public void WithoutSchemaTest() {
			var identifier = "identifier";
			var mediaType = "application/json";
			var headerKey1 = "key";
			var headerValue1 = "value";
			var headerKey2 = "key1";
			var headerValue2 = "value2";
			var body = "body";

			var declaration = $"Request {identifier} ({mediaType})";
			var bodyRequst = $"+ Headers {Environment.NewLine}" +
				$"{headerKey1}:{headerValue1}{Environment.NewLine}" +
				$"{headerKey2} : {headerValue2} {Environment.NewLine}" +
				$"+ Body {Environment.NewLine}" +
				$"{body}{Environment.NewLine}" +
				"+ Response";

			var stream = Extensions.CreatFromString(bodyRequst);

			var result = new RequestParser(stream, declaration).Parse().request;

			Assert.AreEqual(result.Identifier, identifier);
			Assert.AreEqual(result.Body, body);
            Assert.IsNull(result.Schema);
			Assert.AreEqual(result.BodyType, BodyType.Json);
			Assert.AreEqual(result.Headers[headerKey1], headerValue1);
			Assert.AreEqual(result.Headers[headerKey2], headerValue2);
		}

        [Test]
        public void EmptyRequestTest() {
			var identifier = "identifier";
			var mediaType = "application/json";

			var declaration = $"Request {identifier} ({mediaType})";
			var bodyRequst =
				"+ Response";

			var stream = Extensions.CreatFromString(bodyRequst);

			var result = new RequestParser(stream, declaration).Parse().request;

			Assert.AreEqual(result.Identifier, identifier);
			Assert.AreEqual(result.Body, null);
			Assert.IsNull(result.Schema);
			Assert.AreEqual(result.BodyType, BodyType.Json);
            Assert.IsNull(result.Headers);
        }

		[Test]
		public void WithParametersTest()
		{
			var identifier = "identifier";
			var mediaType = "application/json";
			var body = "body";
			var schema = "schema";

			var declaration = $"Request {identifier} ({mediaType})";
			var bodyRequst =
                $"+ Parameters {Environment.NewLine}"+
                $"val:val{Environment.NewLine}"+
				$"+ Body {Environment.NewLine}" +
				$"{body}{Environment.NewLine}" +
				$"+ Schema {Environment.NewLine}" +
				$"{schema}{Environment.NewLine}" +
				"+ Response";

			var stream = Extensions.CreatFromString(bodyRequst);

			var result = new RequestParser(stream, declaration).Parse().request;

			Assert.AreEqual(result.Identifier, identifier);
			Assert.AreEqual(result.Body, body);
			Assert.AreEqual(result.Schema, schema);
			Assert.AreEqual(result.BodyType, BodyType.Json);
            Assert.IsNull(result.Headers);
            Assert.AreEqual(result.Parameters["val"], "val");
		}

		[Test]
		public void WithEmptyBodyTypeTest()
		{
			var identifier = "";
			var mediaType = "";
			var body = "body";
			var schema = "schema";

			var declaration = $"Request {identifier} {mediaType}";
			var bodyRequst =
				$"+ Parameters {Environment.NewLine}" +
				$"val:val{Environment.NewLine}" +
				$"+ Body {Environment.NewLine}" +
				$"{body}{Environment.NewLine}" +
				$"+ Schema {Environment.NewLine}" +
				$"{schema}{Environment.NewLine}" +
				"+ Response";

			var stream = Extensions.CreatFromString(bodyRequst);

			var result = new RequestParser(stream, declaration).Parse().request;

			Assert.AreEqual(result.Identifier, identifier);
			Assert.AreEqual(result.Body, body);
			Assert.AreEqual(result.Schema, schema);
			Assert.AreEqual(result.BodyType, BodyType.Empty);
			Assert.IsNull(result.Headers);
			Assert.AreEqual(result.Parameters["val"], "val");
		}
    }
}
