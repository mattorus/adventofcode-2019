namespace AdventOfCode2019.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using AdventOfCode2019;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Test class for Day5
    /// </summary>
    [TestClass]
    public class Day5Tests
    {
        private const string InputFile1 = @".\input\Day5.txt";

        /// <summary>
        /// Day5 Part1 Input Tests
        /// </summary>
        [DataTestMethod]
        [DataRow(new long[] { 3, 0, 4, 0, 99 }, new long[] { 1 }, new long[] { 1, 0, 4, 0, 99 })]
        [DataRow(new long[] { 3, 3, 99, 13 }, new long[] { 1 }, new long[] { 3, 3, 99, 1 })]
        [DataRow(new long[] { 1, 0, 0, 0, 99 }, null, new long[] { 2, 0, 0, 0, 99 })]
        [DataRow(new long[] { 2, 3, 0, 3, 99 }, null, new long[] { 2, 3, 0, 6, 99 })]
        [DataRow(new long[] { 2, 4, 4, 5, 99, 0 }, null, new long[] { 2, 4, 4, 5, 99, 9801 })]
        [DataRow(new long[] { 1101, 100, -1, 4, 0 }, null, new long[] { 1101, 100, -1, 4, 99 })]
        public void Part1Tests(long[] instructions, long[] userInput, long[] expectedOutput)
        {
            var intcodeRunner = new IntcodeRunner(instructions);
            if (userInput != null)
            {
                foreach (var item in userInput)
                {
                    intcodeRunner.InputQueue.Enqueue(item);
                }
            }

            var runnerResults = intcodeRunner.Execute();

            runnerResults.Should().BeEquivalentTo(expectedOutput);
        }

        /// <summary>
        /// Day5 Part1
        /// </summary>
        [TestMethod]
        public void Part1Tests_File()
        {
            var instructions = File.ReadAllText(InputFile1)
                .Split(',')
                .Select(x => long.Parse(x))
                .ToArray();

            var intcodeRunner = new IntcodeRunner(instructions);
            intcodeRunner.InputQueue.Enqueue(1);
            var runnerResults = intcodeRunner.Execute();
            intcodeRunner.GetLastOutput().Should().Be(6745903);
        }

        [DataTestMethod]
        [DataRow(new long[] { 3, 9, 8, 9, 10, 9, 4, 9, 99, -1, 8 }, new long[] { 1 }, new long[] { 3, 9, 8, 9, 10, 9, 4, 9, 99, 0, 8 })]
        [DataRow(new long[] { 3, 9, 8, 9, 10, 9, 4, 9, 99, -1, 8 }, new long[] { 8 }, new long[] { 3, 9, 8, 9, 10, 9, 4, 9, 99, 1, 8 })]
        [DataRow(new long[] { 3, 9, 7, 9, 10, 9, 4, 9, 99, -1, 8 }, new long[] { 1 }, new long[] { 3, 9, 7, 9, 10, 9, 4, 9, 99, 1, 8 })]
        [DataRow(new long[] { 3, 9, 7, 9, 10, 9, 4, 9, 99, -1, 8 }, new long[] { 8 }, new long[] { 3, 9, 7, 9, 10, 9, 4, 9, 99, 0, 8 })]
        [DataRow(new long[] { 3, 3, 1108, -1, 8, 3, 4, 3, 99 }, new long[] { 8 }, new long[] { 3, 3, 1108, 1, 8, 3, 4, 3, 99 })]
        [DataRow(new long[] { 3, 3, 1108, -1, 8, 3, 4, 3, 99 }, new long[] { 15 }, new long[] { 3, 3, 1108, 0, 8, 3, 4, 3, 99 })]
        [DataRow(new long[] { 3, 3, 1107, -1, 8, 3, 4, 3, 99 }, new long[] { 9 }, new long[] { 3, 3, 1107, 0, 8, 3, 4, 3, 99 })]
        [DataRow(new long[] { 3, 3, 1107, -1, 8, 3, 4, 3, 99 }, new long[] { 7 }, new long[] { 3, 3, 1107, 1, 8, 3, 4, 3, 99 })]
        [DataRow(new long[] { 3, 12, 6, 12, 15, 1, 13, 14, 13, 4, 13, 99, -1, 0, 1, 9 }, new long[] { 0 }, new long[] { 3, 12, 6, 12, 15, 1, 13, 14, 13, 4, 13, 99, 0, 0, 1, 9 })]
        [DataRow(new long[] { 3, 12, 6, 12, 15, 1, 13, 14, 13, 4, 13, 99, -1, -1, 1, 9 }, new long[] { 1 }, new long[] { 3, 12, 6, 12, 15, 1, 13, 14, 13, 4, 13, 99, 1, 0, 1, 9 })]
        [DataRow(new long[] { 3, 3, 1105, -1, 9, 1101, 0, 0, 12, 4, 12, 99, 1 }, new long[] { 0 }, new long[] { 3, 3, 1105, 0, 9, 1101, 0, 0, 12, 4, 12, 99, 0 })]
        [DataRow(new long[] { 3, 3, 1105, -1, 9, 1101, 0, 0, 12, 4, 12, 99, 1 }, new long[] { 5 }, new long[] { 3, 3, 1105, 5, 9, 1101, 0, 0, 12, 4, 12, 99, 1 })]
        public void Part2Tests(long[] instructions, long[] userInput, long[] expectedOutput)
        {
            var intcodeRunner = new IntcodeRunner(instructions);
            foreach (var item in userInput)
            {
                intcodeRunner.InputQueue.Enqueue(item);
            }

            var runnerResults = intcodeRunner.Execute();

            runnerResults.Should().BeEquivalentTo(expectedOutput);
        }

        /// <summary>
        /// Day5 Part2
        /// </summary>
        [TestMethod]
        public void Part2Tests_File()
        {
            var instructions = File.ReadAllText(InputFile1)
                .Split(',')
                .Select(x => long.Parse(x))
                .ToArray();

            var intcodeRunner = new IntcodeRunner(instructions);
            intcodeRunner.InputQueue.Enqueue(5);
            var runnerResults = intcodeRunner.Execute();
            intcodeRunner.GetLastOutput().Should().Be(9168267);
        }
    }
}
