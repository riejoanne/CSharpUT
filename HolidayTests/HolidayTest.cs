using System;
using Lib;
using NUnit.Framework;

namespace HolidayTests
{
    [TestFixture]
    public class HolidayTest
    {
        [SetUp]
        public void SetUp()
        {
            _holiday = new HolidayForTest();
        }

        private HolidayForTest _holiday;

        [Test]
        public void today_is_xmas()
        {
            GivenToday(12, 25);
            SayHelloShouldBe("Merry Xmas");
        }

        [Test]
        public void today_is_xmas_when_Dec_24()
        {
            GivenToday(12, 24);
            SayHelloShouldBe("Merry Xmas");
        }

        [Test]
        public void today_is_xmas_when_Nov_24()
        {
            GivenToday(11, 24);
            SayHelloShouldBe("Today is not Xmas");
        }

        [Test]
        public void today_is_not_xmas()
        {
            GivenToday(12, 1);
            SayHelloShouldBe("Today is not Xmas");
        }


        private void SayHelloShouldBe(string expected)
        {
            Assert.AreEqual(expected, _holiday.SayHello());
        }

        private void GivenToday(int month, int day)
        {
            _holiday.Today = new DateTime(2000, month, day);
        }
    }

    internal class HolidayForTest : Holiday
    {
        private DateTime _today;

        public DateTime Today
        {
            set => _today = value;
        }

        protected override DateTime GetToday()
        {
            return _today;
        }
    }
}