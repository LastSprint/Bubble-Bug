//
// ValidationTest.cs
// 20.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using APIBlueprintParser;

namespace APIBlueprintTests {

    [TestFixture]
    public class ValidationTest {

        [Test]
        public void IsUriWithRelativePath() {
            // given
            var uri = "/temp/data/{id}";

            // when
            var result = HeaderKeywords.IsURI(uri);

            // then
            Assert.IsTrue(result);
        }

        [Test]
        public void IsUriWithIvalidUri() {
            // given
            var str = "this is not a any URI";

            // when
            var result = HeaderKeywords.IsURI(str);

            // then
            Assert.IsFalse(result);
        }

        [Test]
        public void IsHeaderKeywordWithGroup() {
            // given
            var keywod = HeaderKeywords.Group;
            var nonKeywordWithLastUp = "GroUP";
            var nonKeywordWithAllSub = "group";

            // when
            var keywordResult = HeaderKeywords.IsNonKeyword(keywod);
            var nonKeywordWithlastUpResult = HeaderKeywords.IsNonKeyword(nonKeywordWithLastUp);
            var nonKeywordWithAllSubResult = HeaderKeywords.IsNonKeyword(nonKeywordWithAllSub);

            // then
            Assert.IsFalse(keywordResult);
            Assert.IsTrue(nonKeywordWithlastUpResult);
            Assert.IsTrue(nonKeywordWithAllSubResult);
        }

        [Test]
        public void IsHeaderKeywordWithDataStructer() {
            // given
            var keywod = HeaderKeywords.DataStructures;
			var nonKeyWordWithCamel = "DataStructures";
			var nonKeywordWithAllSub = "data structures";

			// when
			var keywordResult = HeaderKeywords.IsNonKeyword(keywod);
			var nonKeywordWithlastUpResult = HeaderKeywords.IsNonKeyword(nonKeyWordWithCamel);
			var nonKeywordWithAllSubResult = HeaderKeywords.IsNonKeyword(nonKeywordWithAllSub);

			// then
			Assert.IsFalse(keywordResult);
			Assert.IsTrue(nonKeywordWithlastUpResult);
			Assert.IsTrue(nonKeywordWithAllSubResult);
        }

        [Test]
        public void IsHeaderKeywordWithHttpMthods() {
            // given
            var keywords = HeaderKeywords.HttpMethods;
            var nonKeywords = new string[] { "lol", "76r3ewgef", "put", "stub" };

            // when - then

            foreach (var value in keywords) {
                Assert.IsFalse(HeaderKeywords.IsNonKeyword(value));
            }

            foreach (var value in nonKeywords) {
                Assert.IsTrue(HeaderKeywords.IsNonKeyword(value));
            }
        }
    }
}
