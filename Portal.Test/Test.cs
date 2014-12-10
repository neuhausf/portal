using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Portal.Test
{
    using System.Threading.Tasks;

    using Hafas;

    using Portal.Business;

    [TestClass]
    public class Test
    {
        private SbbService sbbService;

        [TestInitialize]
        public void Setup()
        {
            this.sbbService = new SbbService();
        }

        [TestMethod]
        public void CanGetStations()
        {
            var station = this.sbbService.GetFirstStationWithName("Bern").Wait(10000);
            Assert.IsNotNull(station);
        }

        [TestMethod]
        public void CanGetConnectionList()
        {
            var result = Task.Run(async () => await this.sbbService.GetConnections("Münsingen", "Bern", DateTime.Now)).Result;
           // result.ConRes.ConnectionList;

            Assert.IsNotNull(result);
        }

    }
}
