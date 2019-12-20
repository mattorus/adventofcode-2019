﻿namespace AdventOfCode2019.Tests
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
        [DataRow(new string[] { "COM)B", "A)B", "B)C", "C)D", "A)E", "B)F" })]
        [DataRow(new string[] { "COM)B", "B)C", "C)D", "D)E", "E)F", "B)G", "G)H", "D)I", "E)J", "J)K", "K)L" })]
        public void TestBuildMap(string[] orbitStrings)
        {
            OrbitMap map = new OrbitMap(orbitStrings);

            map.PrintMap();
            Console.WriteLine($"DirectOrbitCount: {map.DirectOrbitCount()}");
            Console.WriteLine($"IndirectOrbitCount: {map.TotalOrbitCount()}");
        }

        /// <summary>
        /// Day6 Part1
        /// </summary>
        [TestMethod]
        public void Part1Tests_File()
        {
            OrbitMap map = new OrbitMap(InputFile1);

            Console.WriteLine($"OrbitCount: {map.TotalOrbitCount()}");
        }
    }
}
