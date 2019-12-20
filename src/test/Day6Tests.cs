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
        [DataRow(new string[] { "COM)B", "A)B", "B)C", "C)D", "A)E", "B)F" })]
        [DataRow(new string[] { "COM)B", "B)C", "C)D", "D)E", "E)F", "B)G", "G)H", "D)I", "E)J", "J)K", "K)L" })]
        public void TestBuildMap(string[] inputOrbitStrings)
        {
            var map = new OrbitMap(inputOrbitStrings);

            map.PrintMap();
            Console.WriteLine($"DirectOrbitCount: {map.DirectOrbitCount()}");
            Console.WriteLine($"IndirectOrbitCount: {map.TotalOrbitCount()}");
        }

        /// <summary>
        /// Day6 Part2 Input Tests
        /// </summary>
        /// <param name="inputOrbitStrings"></param>
        /// <param name="inputPlanet"></param>
        /// <param name="expected"></param>
        [DataTestMethod]
        [DataRow(new string[] { "COM)B", "B)C", "C)D", "D)E", "E)F", "B)G", "G)H", "D)I", "E)J", "J)K", "K)L", "K)YOU", "L)SANTA" }, "YOU", "K")]
        [DataRow(new string[] { "COM)B", "B)C", "C)D", "D)E", "E)F", "B)G", "G)H", "D)I", "E)J", "J)K", "K)L", "K)YOU", "L)SANTA" }, "SANTA", "L")]
        [DataRow(new string[] { "COM)B", "B)C", "C)D", "D)E", "E)F", "B)G", "G)H", "D)I", "E)J", "J)K", "K)L", "K)YOU", "L)SANTA" }, "B", "COM")]
        [DataRow(new string[] { "COM)B", "B)C", "C)D", "D)E", "E)F", "B)G", "G)H", "D)I", "E)J", "J)K", "K)L", "K)YOU", "L)SANTA" }, "G", "B")]
        [DataRow(new string[] { "COM)B", "B)C", "C)D", "D)E", "E)F", "B)G", "G)H", "D)I", "E)J", "J)K", "K)L", "K)YOU", "L)SANTA" }, "L", "K")]
        public void TestFindPlanetFromOrbit(string[] inputOrbitStrings, string inputPlanet, string expected)
        {
            var map = new OrbitMap(inputOrbitStrings);

            var mapResults = map.FindPlanetFromOrbit(inputPlanet);
            mapResults.Should().Equals(expected);
        }

        [DataTestMethod]
        //[DataRow(new string[] { "COM)B", "A)B", "B)C", "C)D", "A)E", "B)F" }, "F", "D", 1)]
        [DataRow(new string[] { "COM)B", "B)C", "C)D", "D)E", "E)F", "B)G", "G)H", "D)I", "E)J", "J)K", "K)L", "K)YOU", "L)SAN" }, "YOU", "SAN", 1)]
        [DataRow(new string[] { "COM)B", "B)C", "C)D", "D)E", "E)F", "B)G", "G)H", "D)I", "E)J", "J)K", "K)L", "K)YOU", "I)SAN" }, "YOU", "SAN", 4)]
        public void TestFindMinOrbitPath(string[] inputOrbitStrings, string orbitA, string orbitB, int expected)
        {
            var map = new OrbitMap(inputOrbitStrings);
            var mapResults = map.FindMinOrbitPath(orbitA, orbitB);

            mapResults.Should().Be(expected);
                
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

        /// <summary>
        /// Day6 Part2
        /// </summary>
        [TestMethod]
        public void Part2Tests_File()
        {
            OrbitMap map = new OrbitMap(InputFile1);

            Console.WriteLine($"OrbitTransferCount: {map.FindMinOrbitPath("YOU", "SAN")}");
        }
    }
}
