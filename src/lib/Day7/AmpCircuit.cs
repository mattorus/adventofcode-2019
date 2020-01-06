using AdventOfCode2019.Tools;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using static AdventOfCode2019.IntcodeRunner;
using System.Collections.Concurrent;

namespace AdventOfCode2019
{
    public class AmpCircuit
    {
        public Amplifier[] Amps { get; set; }
        public int[] PhaseSequence { get; set; }
        public long[] Instructions { get; set; }
        public Mode InputMode { get; set; }


        public AmpCircuit(int[] phaseSequence, long[] instructions, Mode inputMode = Mode.normal)
        {
            if (phaseSequence == null || phaseSequence.Length < 1)
            {
                throw new ArgumentException($"phaseSequence must have length >= 1.");
            }

            this.PhaseSequence = phaseSequence;
            this.Instructions = instructions;
            this.InputMode = inputMode;
            ResetAmps();
        }

        private void ResetAmps()
        {
            Amps = new Amplifier[PhaseSequence.Length];

            for (int i = 0; i < PhaseSequence.Length; i++)
            {
                Amps[i] = new Amplifier(PhaseSequence[i], (long[]) Instructions.Clone(), InputMode);
                Amps[i].UpdateInput(PhaseSequence[i]);
            }

            if (InputMode == Mode.feedback)
            {
                ResetFeedbackAmps();
            }
        }

        private void ResetFeedbackAmps()
        {
            for (int i = 0; i < Amps.Length; i++)
            {
                var next = (i == Amps.Length - 1 ? 0 : i + 1);
                ConcurrentQueue<long> nextInputQueue = null;

                Amps[next].GetInputQueue(ref nextInputQueue);
                Amps[i].RegisterOutputQueue(ref nextInputQueue);
            }
        }

        public void UpdatePhaseSequence(int[] phaseSequence)
        {
            this.PhaseSequence = phaseSequence;
            ResetAmps();
        }

        public static long GetMaxAmpChainOutput(AmpCircuit ampCircuit)
        {
            int start = 0, end = 0;
            long maxOutput = 0;
            var numAmps = ampCircuit.Amps.Length;

            switch (ampCircuit.InputMode)
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
                ampCircuit.UpdatePhaseSequence(sequence.ToArray());
                var output = ampCircuit.Execute();
                maxOutput = Math.Max(maxOutput, output);
            }

            return maxOutput;
        }

        public long Execute()
        {
            long result = 0;

            switch (InputMode)
            {
                case Mode.feedback:
                    result = ExecuteFeedbackCircuit();
                    break;
                case Mode.normal:
                    result = ExecuteNormalCircuit();
                    break;
                default:
                    break;
            }

            return result;
        }

        private long ExecuteNormalCircuit()
        {
            long ampOutput = 0;      

            for (int i = 0; i < Amps.Length; i++)
            {
                var curAmp = Amps[i];
                curAmp.UpdateInput(ampOutput);
                ampOutput = curAmp.Execute();
            }

            return ampOutput;
        }
                
        private long ExecuteFeedbackCircuit()
        {
            // Create task array which runs execute on each amp
            Task<long>[] tasks = new Task<long>[Amps.Length];
            
            Amps[0].UpdateInput(0);

            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task<long>.Factory.StartNew(Amps[i].Execute);
            }

            Task.WaitAll(tasks);

            return tasks[^1].Result;
        }
    }
}
