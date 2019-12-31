namespace AdventOfCode2019
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Linq;
    using System.Collections.Concurrent;

    public class IntcodeRunner
    {
        public enum Mode
        {
            normal = 0,
            feedback = 1,
        }

        private enum Opcode
        {
            Unknown = 0,
            Add = 1,
            Multiply = 2,
            Read = 3,
            Print = 4,
            JumpIfTrue = 5,
            JumpIfFalse = 6,
            LessThan = 7,
            Equals = 8,
            End = 99,
        }

        private readonly List<int> _outputBuffer;
        private int[] _instructions;
        private int _maxBuffer = 50;

        /// <summary>
        /// InputMode
        /// </summary>
        public Mode InputMode { get; set; }

        /// <summary>
        /// Represents user input
        /// </summary>
        public ConcurrentQueue<int> InputQueue { get; set; }
        
        /// <summary>
        /// Intcode output, for consumption during feedback mode
        /// </summary>
        public ConcurrentQueue<int> OutputQueue { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntcodeRunner"/> class.
        /// </summary>
        /// <param name="instructions">Program instructions.</param>
        /// <param name="userInput">Test hook - simulates command line input.</param>
        public IntcodeRunner(int[] instructions)
        {
            _instructions = instructions ?? throw new ArgumentNullException(nameof(instructions));
            InputQueue = new ConcurrentQueue<int>();
            OutputQueue = null;
            _outputBuffer = new List<int>();
            InputMode = Mode.normal;
        }


        /// <summary>
        /// Updates stored instructions and input, if given, and resets user input pointer.
        /// </summary>
        /// <param name="instructions"></param>
        /// <param name="userInput"></param>
        public void UpdateAndReset(int[] instructions = null, int[] userInput = null)
        {
            if (instructions != null)
            {
                _instructions = instructions;
            }
            
            if (userInput != null)
            {
                foreach (int i in userInput)
                {
                    InputQueue.Enqueue(i);
                }
            }
        }

        /// <summary>
        /// Executes the opcodes and returns the results.
        /// </summary>
        /// <returns>Intcode runner results.</returns>
        public int[] Execute()
        {
            return this.Execute(_instructions[1], _instructions[2]);
        }

        /// <summary>
        /// Executes the opcodes and returns the results.
        /// </summary>
        /// <param name="noun">Noun value.</param>
        /// <param name="verb">Verb value.</param>
        /// <returns>Intcode runner results.</returns>
        public int[] Execute(int noun, int verb)
        {
            var output = (InputMode == Mode.feedback ? _instructions : (int[])_instructions.Clone());
            int[] opcodeParams;
            int index = 0, outputIndex = -1, outputIndexOffset = 0, numParams = 0;

            output[1] = noun;
            output[2] = verb;                        
            var (opcode, operModes) = ParseOpcode(output[index]);
            //_outputBuffer.Clear();

            while (opcode != Opcode.End)
            {
                numParams = 2;
                outputIndexOffset = 0;

                switch (opcode)
                {                    
                    case Opcode.Add:
                    case Opcode.Multiply:
                    case Opcode.LessThan:
                    case Opcode.Equals:
                        outputIndexOffset = 1;
                        break;
                    case Opcode.Read:
                    case Opcode.Print:
                        numParams = 1;
                        break;
                    case Opcode.JumpIfTrue:
                    case Opcode.JumpIfFalse:
                    default:
                        break;
                }

                outputIndex = index + numParams + outputIndexOffset;

                opcodeParams = new int[numParams];
                Array.Copy(output, index + 1, opcodeParams, 0, numParams);

                if (!this.Execute(opcode, operModes, ref index, opcodeParams, output, output[outputIndex]))
                {
                    index = outputIndex + 1;
                }

                (opcode, operModes) = ParseOpcode(output[index]);
            }

            return output;
        }

        /// <summary>
        /// Returns the most recent output.
        /// </summary>
        /// <returns>int</returns>
        public int GetLastOutput()
        {
            _outputBuffer.TrimExcess();

            if (OutputQueue != null && !OutputQueue.IsEmpty)
            {
                OutputQueue.TryPeek(out int result);

                return result;
            }
            else
            {
                return _outputBuffer[^1];
            }
        }

        /// <summary>
        /// Returns output buffer.
        /// </summary>
        /// <returns>List</returns>
        public List<int> GetOutputBuffer()
        {
            _outputBuffer.TrimExcess();

            return _outputBuffer;
        }

        /// <summary>
        /// Parses the given opcode into instruction opcode and operand modes
        /// </summary>
        /// <param name="opcode"></param>
        /// <returns>
        ///    opcode  - the two right-most digits of parameter
        ///    opModes - the remaining digits of parameter
        /// </returns>
        private (Opcode opcode, int opermodes) ParseOpcode(int opcode)
        {
            return ((Opcode)(opcode % 100), opcode / 100);
        }

        /// <summary>
        /// Executes a single instruction, returns bool indicating whether or not instruction pointer was updated
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="operModes"></param>
        /// <param name="index"></param>
        /// <param name="numParams"></param>
        /// <param name="instructions"></param>
        /// <returns>True if instruction pointer was updated, false otherwise</returns>
        private bool Execute(Opcode opcode, int operModes, ref int index, int[] opcodeParams, int[] instructions, int outputIndex)
        {
            bool indexUpdated = false;
            int i = 0;

            for (i = 0; i < opcodeParams.Length; i++)
            {
                if (operModes % 10 == 0)
                {
                    opcodeParams[i] = instructions[opcodeParams[i]];
                }

                operModes /= 10;
            }

            switch (opcode)
            {
                case Opcode.Add:
                    instructions[outputIndex] = opcodeParams.Aggregate((int) 0, (x, y) => x + y);
                    break;

                case Opcode.Multiply:
                    instructions[outputIndex] = opcodeParams.Aggregate((int) 1, (x, y) => x * y);
                    break;

                case Opcode.Read:
                    var input = 0;

                    while (!InputQueue.TryDequeue(out input))
                    {
                        Thread.Sleep(1); // TODO: TOFIX: Should utilize wait instead
                    }

                    instructions[outputIndex] = input;

                    break;

                case Opcode.Print:
                    Print(instructions[outputIndex]);
                    break;

                case Opcode.JumpIfTrue:
                    if (opcodeParams[0] != 0)
                    {
                        index = (int) opcodeParams[^1];
                        indexUpdated = true;
                    }

                    break;

                case Opcode.JumpIfFalse:
                    if (opcodeParams[0] == 0)
                    {
                        index = (int) opcodeParams[^1];
                        indexUpdated = true;
                    }

                    break;

                case Opcode.LessThan:
                    instructions[outputIndex] = (int) (opcodeParams[0] < opcodeParams[1] ? 1 : 0);                    
                    break;

                case Opcode.Equals:
                    instructions[outputIndex] = (int) (opcodeParams[0] == opcodeParams[1] ? 1 : 0);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(opcode), $"Invalid opcode specified! -- {opcode}");
            }

            return indexUpdated;
        }

        /// <summary>
        /// Handles the output functionality.
        /// </summary>
        /// <param name="value"></param>
        private void Print(int value)
        {
            _outputBuffer.Add(value);

            if (InputMode == Mode.feedback)
            {
                OutputQueue.Enqueue(value);
            }

            while (_outputBuffer.Count > _maxBuffer)
            {
                _outputBuffer.RemoveAt(0);
            }

            Console.WriteLine(value);
        }
    }
}
