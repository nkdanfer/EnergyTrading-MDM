﻿namespace EnergyTrading.MDM.Test.Extensions
{
    using System;

    using EnergyTrading;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.MDM.Extensions;

    [TestClass]
    public class DateRangeExtensionsFixture
    {
        [TestMethod]
        public void ToContractCopiesValue()
        {
            var start = new DateTime(2011, 1, 1);
            var finish = new DateTime(2011, 12, 31);
            var value = new DateRange(start, finish);

            var candidate = value.ToContract();
            Assert.AreEqual(start, candidate.StartDate, "StartDate differs");
            Assert.AreEqual(finish, candidate.EndDate, "EndDate differs");
        }

        [TestMethod]
        public void ToContractWithNullValueCreatesDefault()
        {
            DateRange value = null;

            var candidate = value.ToContract();
            Assert.IsFalse(candidate.StartDate.HasValue, "StartDate has value");
            Assert.IsFalse(candidate.EndDate.HasValue, "EndDate has value");
        }
    }
}