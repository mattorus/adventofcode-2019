namespace AdventOfCode2019.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using AdventOfCode2019;
    using AdventOfCode2019.Day8;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using static AdventOfCode2019.IntcodeRunner;

    [TestClass]
    public class Day9Tests
    {
        private const string InputFile1 = @".\input\Day9.txt";

        [DataTestMethod]
        [DataRow(new long[] { 109, 1, 204, -1, 1001, 100, 1, 100, 1008, 100, 16, 101, 1006, 101, 0, 99 }, 0, new long[] { 109, 1, 204, -1, 1001, 100, 1, 100, 1008, 100, 16, 101, 1006, 101, 0, 99 })]
        [DataRow(new long[] { 1102, 34915192, 34915192, 7, 4, 7, 99, 0 }, 1219070632396864, null)]
        [DataRow(new long[] { 104, 1125899906842624, 99 }, 1125899906842624, null)]
        public void Part1Tests(long[] instructions, long expected, long[] expectedArr)
        {
            var intcodeRunner = new IntcodeRunner(instructions);
            var runnerResults = intcodeRunner.Execute();

            if (expectedArr != null)
            {
                runnerResults.Should().BeEquivalentTo(expectedArr);
            }
            else
            {
                intcodeRunner.GetLastOutput().Should().Be(expected);
            }
        }

        [TestMethod]
        public void Part1Tests_File()
        {
            var instructions = File.ReadAllText(InputFile1)
                .Split(",")
                .Select(x => long.Parse(x))
                .ToArray();
            var intcodeRunner = new IntcodeRunner(instructions);

            intcodeRunner.InputQueue.Enqueue(1);

            var runnerResults = intcodeRunner.Execute();
        }

        [TestMethod]
        public void Part2Tests_File()
        {
            var instructions = File.ReadAllText(InputFile1)
                .Split(",")
                .Select(x => long.Parse(x))
                .ToArray();
            var intcodeRunner = new IntcodeRunner(instructions);

            intcodeRunner.InputQueue.Enqueue(2);

            var runnerResults = intcodeRunner.Execute();
        }
    }
}
