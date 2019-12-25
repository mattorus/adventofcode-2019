using AdventOfCode2019.Tools;
using System;
using System.Collections.Generic;
using System.Text;
using static AdventOfCode2019.IntcodeRunner;

namespace AdventOfCode2019
{
    public class Amplifier
    {
        public int PhaseSetting { get; private set; }
        public int Input { get; private set; }
        public int ThrusterOutput { get; set; }

        private readonly IntcodeRunner _intcodeRunner;

        public Amplifier(int phaseSetting, int ampInput, int[] instructions, Mode inputMode = Mode.normal)
        {
            this.PhaseSetting = phaseSetting;
            this.Input = ampInput;

            _intcodeRunner = new IntcodeRunner(instructions, new int[] { PhaseSetting, Input });
            _intcodeRunner.InputMode = inputMode;
        }

        public void UpdateAndReset(int phaseSetting, int ampInput, int[] instructions = null, int[] userInput = null)
        {
            this.PhaseSetting = phaseSetting;
            this.Input = ampInput;

            _intcodeRunner.UpdateAndReset(instructions, new int[] { PhaseSetting, Input });
        }

        public int GetMaxAmpChainOutput(int numAmps, Mode inputMode)
        {
            int start = 0, end = 0, maxOutput = 0;

            _intcodeRunner.InputMode = inputMode;

            switch (inputMode)
            {
                case Mode.normal:
                    start = 0;
                    end = numAmps;
                    break;
                case Mode.feedback:
                    start = numAmps;
                    end = numAmps * 2;
                    break;
                default:
                    break;
            }

            var sequences = Generator.GeneratePhaseSequences(start, end);
            
            foreach (var sequence in sequences)
            {
                var output = this.ExecuteChainSequence(sequence.ToArray());
                maxOutput = Math.Max(maxOutput, output);
            }

            return maxOutput;
        }

        public int ExecuteChainSequence(int[] phaseSequence, int ampInput = 0)
        {
            HashSet<int> phaseSetting = new HashSet<int>();

            for (int i = 0; i < phaseSequence.Length; i++)
            {
                this.UpdateAndReset(phaseSequence[i], ampInput);

                ampInput = this.Execute();
            }

            return ampInput;
        }

        public int Execute()
        {
            _intcodeRunner.Execute();
            ThrusterOutput = _intcodeRunner.GetLastOutput();

            return ThrusterOutput;
        }

    }
}
