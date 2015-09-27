using EasyPacketSharp.Exceptions;
using EasyPacketSharp.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EasyPacketSharpTests
{
    [TestClass]
    public class TestBaseConversion
    {
        [TestMethod]
        public void BasicBaseUp()
        {
            var before = "11011"; //2
            var expected = "1B"; //16
            Assert.AreEqual(expected, before.ToBase(2, 16).TrimStart('0'), "Failed base 2 to base 16 conversion.");
            before = "101001"; //2
            expected = "41"; //10
            Assert.AreEqual(expected, before.ToBase(2, 10).TrimStart('0'), "Failed base 2 to base 10 conversion.");
            before = "FACB00";
            expected = "+ssA";
            Assert.AreEqual(expected, before.ToBase(16, 64).TrimStart('A'), "Failed base 16 to base 64 conversion.");
            before = "FFFF";
            expected = "B777";
            Assert.AreEqual(expected, before.ToBase(16, 32).TrimStart('A'), "Failed base 16 to base 32 conversion.");
        }

        [TestMethod]
        public void EnsureThrowsOnInvalidCharacter()
        {
            var before = "101101Å";
            try
            {
                var s = before.ToBase(2, 15); //Should throw InvalidCharacterException
                Assert.Fail("ToBase should throw InvalidCharacterException when an invalid character for the base is passed.");
            }
            catch (InvalidCharacterException) { }


            before = "101101A";
            try
            {
                var s = before.ToBase(6, 30); //Should throw InvalidCharacterException
                Assert.Fail("ToBase should throw InvalidCharacterException when an invalid character for the base is passed.");
            }
            catch (InvalidCharacterException) { }
        }
    }
}
