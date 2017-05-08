using SleekSurf.Manager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SleekSurf.Entity;
using SleekSurf.FrameWork;

namespace ClientManagerTest
{
    
    
    /// <summary>
    ///This is a test class for ClientManagerTest and is intended
    ///to contain all ClientManagerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ClientManagerTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for InsertClient
        ///</summary>
        [TestMethod()]
        public void InsertClientTest()
        {
            ClientDetails clientDetail = null; // TODO: Initialize to an appropriate value
            Result<ClientDetails> expected = null; // TODO: Initialize to an appropriate value
            Result<ClientDetails> actual;
            actual = ClientManager.InsertClient(clientDetail);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetCategories
        ///</summary>
        [TestMethod()]
        public void GetCategoriesTest()
        {
            Result<CategoryDetails> expected = null; // TODO: Initialize to an appropriate value
            Result<CategoryDetails> actual;
            actual = ClientManager.GetCategories();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        ///// <summary>
        /////A test for HostIpToLocation
        /////</summary>
        //[TestMethod()]
        //public void HostIpToLocationTest()
        //{
        //    string ip = string.Empty; // TODO: Initialize to an appropriate value
        //    LocationDetails expected = null; // TODO: Initialize to an appropriate value
        //    LocationDetails actual;
        //    actual = ClientManager.HostIpToLocation(ip);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for UnPublishClient
        /////</summary>
        //[TestMethod()]
        //public void UnPublishClientTest()
        //{
        //    string clientID = string.Empty; // TODO: Initialize to an appropriate value
        //    string comment = string.Empty; // TODO: Initialize to an appropriate value
        //    bool expected = false; // TODO: Initialize to an appropriate value
        //    bool actual;
        //    actual = ClientManager.UnPublishClient(clientID, comment);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}
    }
}
