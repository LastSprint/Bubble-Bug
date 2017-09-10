//
// SupportTest.cs
// 23.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//

using System;
using System.Collections.Generic;
using NUnit.Framework;
using APIBlueprintParser.Parsers;
using APIBlueprintParser.Models;
using ValueType = APIBlueprintParser.Models.ValueType;

namespace APIBlueprintTests {

    [TestFixture]
    public class SupportTest
    {

        #region StringToBodyType Tests

        [Test]
        public void StringToBodyTypeJsonTest()
        {

            // given
            string[] jsons = { "application/json", "AppLicatiOn/Json", "APPLICatioN/JSoN", "APPLICATION/JSON" };

            // when
            var results = new List<BodyType?>();

            foreach (var json in jsons)
            {
                results.Add(Support.StringToBodyType(json));
            }

            // then
            foreach (var result in results)
            {
                Assert.IsTrue(result.HasValue);
                Assert.AreEqual(result.Value, BodyType.Json);
            }
        }

        [Test]
        public void StringToBodyTypeIvalidTest()
        {

            // given
            string[] jsons = { "276t3igyh", "asda", "2893kj", "xml" };

            // when
            var results = new List<BodyType>();

            foreach (var json in jsons)
            {
                results.Add(Support.StringToBodyType(json));
            }

            // then
            foreach (var result in results)
            {
                Assert.IsTrue(result == BodyType.Empty);
            }
        }

        #endregion

        #region StringToValueType Tests

        [Test]
        public void StringToValueTypeObjectTest()
        {
            // given
            string[] jsons = { "Object", "object", "OBJeCt", "OBJECT" };

            // when
            var results = new List<ValueType?>();

            foreach (var json in jsons)
            {
                results.Add(Support.StringToValueType(json));
            }

            // then
            foreach (var result in results)
            {
                Assert.IsTrue(result.HasValue);
                Assert.AreEqual(result.Value, ValueType.Object);
            }
        }

        [Test]
        public void StringToValueTypeStringTest()
        {
            // given
            string[] jsons = { "String", "string", "StrInG", "STRING" };

            // when
            var results = new List<ValueType?>();

            foreach (var json in jsons)
            {
                results.Add(Support.StringToValueType(json));
            }

            // then
            foreach (var result in results)
            {
                Assert.IsTrue(result.HasValue);
                Assert.AreEqual(result.Value, ValueType.String);
            }
        }

        [Test]
        public void StringToValueTypeNumberTest()
        {
            // given
            string[] jsons = { "Number", "number", "NuMbeR", "NUMBER" };

            // when
            var results = new List<ValueType?>();

            foreach (var json in jsons)
            {
                results.Add(Support.StringToValueType(json));
            }

            // then
            foreach (var result in results)
            {
                Assert.IsTrue(result.HasValue);
                Assert.AreEqual(result.Value, ValueType.Number);
            }
        }

        [Test]
        public void StringToValueTypeBoolTest()
        {
            // given
            string[] jsons = { "Bool", "bool", "bOoL", "BOOL" };

            // when
            var results = new List<ValueType?>();

            foreach (var json in jsons)
            {
                results.Add(Support.StringToValueType(json));
            }

            // then
            foreach (var result in results)
            {
                Assert.IsTrue(result.HasValue);
                Assert.AreEqual(result.Value, ValueType.Bool);
            }
        }

        [Test]
        public void StringToValueTypeInvalidTest()
        {
            // given
            string[] jsons = { "67tyghn", "sihreib", "GR Ds", "OBJe T" };

            // when
            var results = new List<ValueType?>();

            foreach (var json in jsons)
            {
                results.Add(Support.StringToValueType(json));
            }

            // then
            foreach (var result in results)
            {
                Assert.IsFalse(result.HasValue);
            }
        }

        #endregion

        #region StringToNeededType Tests

        [Test]
        public void StringToNeededTypeRequiredTest()
        {
            // given
            string[] jsons = { "Required", "required", "reQuIReD", "REQUIRED" };

            // when
            var results = new List<NeededType?>();

            foreach (var json in jsons)
            {
                results.Add(Support.StringToNeededType(json));
            }

            // then
            foreach (var result in results)
            {
                Assert.IsTrue(result.HasValue);
                Assert.AreEqual(result.Value, NeededType.Required);
            }
        }

        [Test]
        public void StringToNeededTypeOptionalTest()
        {
            // given
            string[] jsons = { "Optional", "optional", "OptIoNal", "OPTIONAL" };

            // when
            var results = new List<NeededType?>();

            foreach (var json in jsons)
            {
                results.Add(Support.StringToNeededType(json));
            }

            // then
            foreach (var result in results)
            {
                Assert.IsTrue(result.HasValue);
                Assert.AreEqual(result.Value, NeededType.Optional);
            }
        }

        [Test]
        public void StringToNeededTypeInvalidTest() {
            // given
            string[] jsons = { "Optio nal", "optiodnal", "OptIo3Nal", "OPTIqONAL" };

            // when
            var results = new List<NeededType?>();

            foreach (var json in jsons) {
                results.Add(Support.StringToNeededType(json));
            }

            // then
            foreach (var result in results) {
                Assert.IsFalse(result.HasValue);
            }
        }

        #endregion

        #region StringToactionOption

        [Test]
        public void ParseIterativeOPtionTest() {
            // given
            var str = "Iterative";

            // when
            var res = Support.StringToActionOption(str);

            // then

            Assert.AreEqual(res.Value, ActionOption.Iterative);
        }

        #endregion

    }
}
