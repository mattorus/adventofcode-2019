namespace AdventOfCode2019.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using AdventOfCode2019.Day2;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    [TestClass]
    public class Day6Tests
    {

        private const string InputFile1 = @".\input\Day6.txt";

        /// <summary>
        /// Day6 Part1 Input Tests
        /// </summary>
        /// <param name="orbitStrings"></param>
        [DataTestMethod]
        [DataRow(new string[] { "A)B", "B)C", "C)D", "A)E", "B)F" })]
        public void TestBuildMap(string[] orbitStrings)
        {
            OrbitMap map = new OrbitMap(orbitStrings);

            map.PrintMap();
        }

        /// <summary>
        /// Day6 Part1
        /// </summary>
        [TestMethod]
        public void Part1Tests_File()
        {
            OrbitMap map = new OrbitMap(InputFile1);

            map.PrintMap();
        }
    }
}
