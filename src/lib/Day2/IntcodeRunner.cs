namespace AdventOfCode2019.Day2
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class IntcodeRunner
    {
        private enum Opcode
        {
            Unknown = 0,
            Add = 1,
            Multiply = 2,
            End = 99,
        }

        private readonly int[] instructions;

        /// <summary>
        /// Initializes a new instance of the <see cref="IntcodeRunner"/> class.
        /// </summary>
        /// <param name="instructions">Program instructions.</param>
        public IntcodeRunner(int[] instructions)
        {
            this.instructions = instructions ?? throw new ArgumentNullException(nameof(instructions));
        }

        /// <summary>
        /// Executes the opcodes and returns the results.
        /// </summary>
        /// <returns>Intcode runner results.</returns>
        public int[] Execute()
        {
            return this.Execute(this.instructions[1], this.instructions[2]);
        }

        /// <summary>
        /// Executes the opcodes and returns the results.
        /// </summary>
        /// <param name="noun">Noun value.</param>
        /// <param name="verb">Verb value.</param>
        /// <returns>Intcode runner results.</returns>
        public int[] Execute(int noun, int verb)
        {
            var output = (int[])this.instructions.Clone();
            output[1] = noun;
            output[2] = verb;

            int index = 0;
            var opcode = (Opcode)output[index];
            while (opcode != Opcode.End)
            {
                this.Execute(opcode, output[index + 1], output[index + 2], output[index + 3], output);
                index += 4;
                opcode = (Opcode)output[index];
            }

            return output;
        }

        private void Execute(Opcode opcode, int inputPos1, int inputPos2, int outputPos, int[] instructions)
        {
            switch (opcode)
            {
                case Opcode.Add:
                    instructions[outputPos] = instructions[inputPos1] + instructions[inputPos2];
                    break;

                case Opcode.Multiply:
                    instructions[outputPos] = instructions[inputPos1] * instructions[inputPos2];
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(opcode), $"Invalid opcode specified! -- {opcode}");
            }
        }
    }
}
