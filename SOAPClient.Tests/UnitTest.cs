using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SOAPClient.Core.Serializer;

namespace SOAPClient.Tests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestMethod()
        {
            var response = PlayerSerializer.GetPlayers("Brazil");
        }
    }
}
