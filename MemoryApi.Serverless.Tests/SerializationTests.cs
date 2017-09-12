using System;
using FaunaDB.Query;
using MemoryApi.DbModels;
using MemoryApi.Storage;
using MemoryApi.Util;
using MemoryCore.JsonModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MemoryApi.Tests
{
    [TestClass]
    public class SerializationTests
    {
        private class SerializationTestObj
        {
            public string TestString { get; set; } = "testString";
            public int TestNumber { get; set; } = -2;
            public LoginModel TestObject { get; set; } = new LoginModel
            {
                Identifier = "testIdent",
                Password = "testPwd"
            };

            public bool TestBoolean { get; set; } = true;
            public char TestChar { get; set; } = 'b';
            public DateTime TestDateTime { get; set; } = new DateTime(1970, 1, 1);
        }

        [TestMethod]
        public void FaunaDbObjSerializer()
        {
            var testObj = new SerializationTestObj();
            var autoSerialization = testObj.ToFaunaObj();
            var manualSerialization = Language.Obj("testString", testObj.TestString, "testNumber", testObj.TestNumber,
                "testObject", Language.Obj("identifier", "testIdent", "password", "testPwd"), "testBoolean",
                testObj.TestBoolean, "testChar", testObj.TestChar.ToString(), "testDateTime",
                testObj.TestDateTime.ToUnixTimeStamp());
            Assert.AreEqual(autoSerialization, manualSerialization);
        }

        [TestMethod]
        public void FaunaDbObjSerializerRealWorld()
        {
            var testUser = new User {Email = "test@test.test", Username = "testuser", PasswordHash = "asxaw"};
            var autoSerialization = testUser.ToFaunaObj();
            // Manual serialization would be massive - only checking for exceptions.
            Assert.IsNotNull(autoSerialization);
        }
    }
}
