using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Swagger.Net.Tests
{
    [TestClass]
    public class SwaggerSpecTests
    {
        [TestMethod]
        public void TestNonGenericTypes()
        {
            Assert.AreEqual("boolean", SwaggerSpec.GetDataTypeName(typeof(bool)));
            Assert.AreEqual("byte", SwaggerSpec.GetDataTypeName(typeof(byte)));
            Assert.AreEqual("int", SwaggerSpec.GetDataTypeName(typeof(int)));
            Assert.AreEqual("long", SwaggerSpec.GetDataTypeName(typeof(long)));
            Assert.AreEqual("float", SwaggerSpec.GetDataTypeName(typeof(float)));
            Assert.AreEqual("double", SwaggerSpec.GetDataTypeName(typeof(double)));
            Assert.AreEqual("Date", SwaggerSpec.GetDataTypeName(typeof(DateTime)));
            Assert.AreEqual("string", SwaggerSpec.GetDataTypeName(typeof(char)));
            Assert.AreEqual("string", SwaggerSpec.GetDataTypeName(typeof(string)));
            Assert.AreEqual("object", SwaggerSpec.GetDataTypeName(typeof(object)));
            Assert.AreEqual("MockEnum1", SwaggerSpec.GetDataTypeName(typeof(MockEnum1)));
            Assert.AreEqual("MockStruct1", SwaggerSpec.GetDataTypeName(typeof(MockStruct1)));
            Assert.AreEqual("MockClass1", SwaggerSpec.GetDataTypeName(typeof(MockClass1)));
        }

        [TestMethod]
        public void TestNullable()
        {
            Assert.AreEqual("int?", SwaggerSpec.GetDataTypeName(typeof(int?)));
            Assert.AreEqual("int?", SwaggerSpec.GetDataTypeName(typeof(Nullable<int>)));
            Assert.AreEqual("MockEnum1?", SwaggerSpec.GetDataTypeName(typeof(Nullable<MockEnum1>)));
            Assert.AreEqual("MockStruct1?", SwaggerSpec.GetDataTypeName(typeof(Nullable<MockStruct1>)));
        }

        [TestMethod]
        public void TestContainers()
        {
            Assert.AreEqual("Array[object]", SwaggerSpec.GetDataTypeName(typeof(List<object>)));
            Assert.AreEqual("Array[MockClass1]", SwaggerSpec.GetDataTypeName(typeof(List<MockClass1>)));
            Assert.AreEqual("Array[object]", SwaggerSpec.GetDataTypeName(typeof(IEnumerable)));
            Assert.AreEqual("Array[MockClass1]", SwaggerSpec.GetDataTypeName(typeof(IEnumerable<MockClass1>)));
            Assert.AreEqual("Array[object]", SwaggerSpec.GetDataTypeName(typeof(Collection<object>)));
            Assert.AreEqual("Array[MockClass1]", SwaggerSpec.GetDataTypeName(typeof(Collection<MockClass1>)));
        }
    }

    public enum MockEnum1
    {
        Value1,
        Value2,
        Value3,
    }

    public struct MockStruct1
    {
        int x;
    }

    public class MockClass1
    {
    }
}