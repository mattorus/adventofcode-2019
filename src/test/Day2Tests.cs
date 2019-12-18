namespace AdventOfCode2019.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using AdventOfCode2019.Day2;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Test class for Day2
    /// </summary>
    [TestClass]
    public class Day2Tests
    {
        private const string InputFile = @".\input\Day2.txt";

        /// <summary>
        /// Day2 Part1 Input Tests
        /// </summary>
        [DataTestMethod]
        [DataRow(new int[] { 1, 0, 0, 0, 99 }, new int[] { 2, 0, 0, 0, 99 })]
        [DataRow(new int[] { 2, 3, 0, 3, 99 }, new int[] { 2, 3, 0, 6, 99 })]
        [DataRow(new int[] { 2, 4, 4, 5, 99, 0 }, new int[] { 2, 4, 4, 5, 99, 9801 })]
        [DataRow(new int[] { 1, 1, 1, 4, 99, 5, 6, 0, 99 }, new int[] { 30, 1, 1, 4, 2, 5, 6, 0, 99 })]
        public void Part1Tests(int[] instructions, int[] expectedOutput)
        {
            var intcodeRunner = new IntcodeRunner(instructions);
            var runnerResults = intcodeRunner.Execute();

            runnerResults.Should().BeEquivalentTo(expectedOutput);
        }

        /// <summary>
        /// Day2 Part1
        /// </summary>
        [TestMethod]
        public void Part1Tests_File()
        {
            var instructions = File.ReadAllText(InputFile)
                .Split(',')
                .Select(x => int.Parse(x))
                .ToArray();

            // Restore gravity assist.
            var noun = 12;
            var verb = 2;

            var intcodeRunner = new IntcodeRunner(instructions);
            var runnerResults = intcodeRunner.Execute(noun, verb);
            Console.WriteLine($"Position 0 Result: {runnerResults[0]}");
        }

        /// <summary>
        /// Day2 Part2
        /// </summary>
        [TestMethod]
        public void Part2Tests_File()
        {
            const int outputToFind = 19690720;
            var instructions = File.ReadAllText(InputFile)
                .Split(',')
                .Select(x => int.Parse(x))
                .ToArray();

            var intcodeRunner = new IntcodeRunner(instructions);
            for (int noun = 0; noun <= 99; noun++)
            {
                for (int verb = 0; verb <= 99; verb++)
                {
                    try
                    {
                        var runnerResults = intcodeRunner.Execute(noun, verb);
                        if (runnerResults[0] == outputToFind)
                        {
                            Console.WriteLine($"Found Matching Result: Noun={noun}, Verb={verb}");
                            Console.WriteLine($"  Output: {noun * 100 + verb}");
                        }
                    }
                    catch
                    {
                        // If the program encounters an error, ignore it as it means the values didn't result in the desired output.
                    }
                }
            }
        }
    }
}
