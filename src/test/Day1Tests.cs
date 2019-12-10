namespace AdventOfCode2019.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    using AdventOfCode2019;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Test class for Day1
    /// </summary>
    [TestClass]
    public class Day1Tests
    {
        private const string InputFile = @".\input\Day1.txt";

        /// <summary>
        /// Day1 Part1
        /// </summary>
        [TestMethod]
        public void Part1()
        {
            Console.WriteLine($"Fuel Required: {Day1.GetFuelRequirements(new FileInfo(InputFile))}");
        }

        /// <summary>
        /// Day1 Part2
        /// </summary>
        [TestMethod]
        public void Part2()
        {
            Console.WriteLine($"Fuel Required: {Day1.GetFuelRequirements(new FileInfo(InputFile), includeFuelMass: true)}");
        }
    }
}
