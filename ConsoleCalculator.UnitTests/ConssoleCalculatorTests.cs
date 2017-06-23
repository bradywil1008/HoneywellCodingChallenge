using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Honeywell.CodingChallenge;


namespace Honeywell.CodingChallenge.UnitTests
{
    [TestClass]
    public class ConssoleCalculatorTests
    {
        [TestMethod]
        public void GetCalculatedResults_With2Ops_AdditionAndSubtraction()
        {
            // arrange
            double expected = 0;
            
            //act 
            ICalculator calculator = new ConsoleCalculator("1+2-3");
            double actual = calculator.GetCalculatedResults();

            // assert
            Assert.AreEqual<double>(expected, actual);
        }

        [TestMethod]
        public void GetCalculatedResults_With3Ops_MultiplicationAndSubtraction()
        {
            // arrange
            double expected = 16;

            //act 
            ICalculator calculator = new ConsoleCalculator("12*2-4*2");
            double actual = calculator.GetCalculatedResults();

            // assert
            Assert.AreEqual<double>(expected, actual);
        }

        [TestMethod]
        public void GetCalculatedResults_With2Ops_AdditionAndMultiplication()
        {
            // arrange
            double expected = 11;

            //act 
            ICalculator calculator = new ConsoleCalculator("5+2*3");
            double actual = calculator.GetCalculatedResults();

            // assert
            Assert.AreEqual<double>(expected, actual);
        }

        [TestMethod]
        public void GetCalculatedResults_With2Ops_Subtraction()
        {
            // arrange
            double expected = -18;

            //act 
            ICalculator calculator = new ConsoleCalculator("-3-6-9");
            double actual = calculator.GetCalculatedResults();

            // assert
            Assert.AreEqual<double>(expected, actual);
        }

        [TestMethod]
        public void GetCalculatedResults_With1Op_Division()
        {
            // arrange
            double expected = 3.3333;

            //act 
            ICalculator calculator = new ConsoleCalculator("10/3");
            double actual = calculator.GetCalculatedResults();

            // assert
            Assert.AreEqual<double>(expected, Math.Round(actual, 4));
        }

        [TestMethod]
        public void GetCalculatedResults_With1Op_AdditionWithPI()
        {
            // arrange
            double expected = 14.1416;

            //act 
            ICalculator calculator = new ConsoleCalculator("PI+11");
            double actual = calculator.GetCalculatedResults();

            // assert
            Assert.AreEqual<double>(expected, Math.Round(actual, 4));
        }

        [TestMethod]
        public void GetCalculatedResults_With1Op_AdditionWithE()
        {
            // arrange
            double expected = 8.1548;

            //act 
            ICalculator calculator = new ConsoleCalculator("E*3");
            double actual = calculator.GetCalculatedResults();

            // assert
            Assert.AreEqual<double>(expected, Math.Round(actual, 4));
        }
    }
}
