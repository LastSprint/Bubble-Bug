//
// ResponseParser.cs
// 22.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//

using System;
using NUnit.Framework;
using APIBlueprintParser.Parsers.Action.Request;
using APIBlueprintParser.Models;

namespace APIBlueprintTests {
   
    [TestFixture]
    public class ResponseParserTest {

		[Test]
		public void FullDeclarationTest()
		{
			var statusCode = 200;
			var mediaType = "application/json";
			var headerKey1 = "key";
			var headerValue1 = "value";
			var headerKey2 = "key1";
			var headerValue2 = "value2";
			var body = "body";

			var declaration = $"Response {statusCode} ({mediaType})";
			var bodyRequst = $"+ Headers {Environment.NewLine}" +
				$"{headerKey1}:{headerValue1}{Environment.NewLine}" +
				$"{headerKey2} : {headerValue2} {Environment.NewLine}" +
				$"+ Body {Environment.NewLine}" +
				$"{body}{Environment.NewLine}" +
				"###";

			var stream = Extensions.CreatFromString(bodyRequst);

            var result = new ResponseParser(stream, declaration).Parse().response;

			Assert.AreEqual(result.Code, statusCode);
			Assert.AreEqual(result.Body, body);
			Assert.AreEqual(result.BodyType, BodyType.Json);
			Assert.AreEqual(result.Headers[headerKey1], headerValue1);
			Assert.AreEqual(result.Headers[headerKey2], headerValue2);
		}

		[Test]
		public void WithoutHeadersTest()
		{
			var statusCode = 200;
			var mediaType = "application/json";
			var body = "body";

			var declaration = $"Response {statusCode} ({mediaType})";
			var bodyRequst = 
				$"+ Body {Environment.NewLine}" +
				$"{body}{Environment.NewLine}" +
				"+ Request";

			var stream = Extensions.CreatFromString(bodyRequst);

			var result = new ResponseParser(stream, declaration).Parse().response;

			Assert.AreEqual(result.Code, statusCode);
			Assert.AreEqual(result.Body, body);
			Assert.AreEqual(result.BodyType, BodyType.Json);
            Assert.IsNull(result.Headers);
		}

		[Test]
		public void WithoutBodyTest()
		{
			var statusCode = 200;
			var mediaType = "application/json";
			var headerKey1 = "key";
			var headerValue1 = "value";
			var headerKey2 = "key1";
			var headerValue2 = "value2";

			var declaration = $"Response {statusCode} ({mediaType})";
			var bodyRequst = $"+ Headers {Environment.NewLine}" +
				$"{headerKey1}:{headerValue1}{Environment.NewLine}" +
				$"{headerKey2} : {headerValue2} {Environment.NewLine}" +
				"+ Request";

			var stream = Extensions.CreatFromString(bodyRequst);

			var result = new ResponseParser(stream, declaration).Parse().response;

			Assert.AreEqual(result.Code, statusCode);
            Assert.IsNull(result.Body);
			Assert.AreEqual(result.BodyType, BodyType.Json);
			Assert.AreEqual(result.Headers[headerKey1], headerValue1);
			Assert.AreEqual(result.Headers[headerKey2], headerValue2);
		}

		[Test]
        public void EmptyResponseTest()
		{
			var statusCode = 200;
			var mediaType = "application/json";

			var declaration = $"Response {statusCode} ({mediaType})";
			var bodyRequst = 
				"+ Request";

			var stream = Extensions.CreatFromString(bodyRequst);

			var result = new ResponseParser(stream, declaration).Parse().response;

			Assert.AreEqual(result.Code, statusCode);
			Assert.IsNull(result.Body);
			Assert.AreEqual(result.BodyType, BodyType.Json);
            Assert.IsNull(result.Headers);
		}
    }
}
