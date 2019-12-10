namespace AdventOfCode2019.Day1
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class FuelCalculator
    {
        /// <summary>
        /// Gets the total fuel requirements of all of the given modules.
        /// </summary>
        /// <param name="moduleMasses">Input file containing all module masses.</param>
        /// <param name="includeFuelMass">Whether to include the fuel as part of the mass.</param>
        /// <returns>Total fuel requirements.</returns>
        public static int GetFuelRequirements(FileInfo moduleMasses, bool includeFuelMass = false)
        {
            _ = moduleMasses ?? throw new ArgumentNullException(nameof(moduleMasses));
            if (!moduleMasses.Exists)
            {
                throw new FileNotFoundException($"{nameof(moduleMasses)} was not found!", moduleMasses.FullName);
            }

            var totalFuelRequired = 0;
            foreach (var mass in File.ReadAllLines(moduleMasses.FullName).Select(x => int.Parse(x)))
            {
                totalFuelRequired += GetModuleFuelRequirement(mass, includeFuelMass);
            }

            return totalFuelRequired;
        }

        /// <summary>
        /// Gets fuel required to launch a given module based on its mass.
        /// </summary>
        /// <param name="mass">Mass of module.</param>
        /// <param name="includeFuelMass">Whether to include the fuel as part of the mass.</param>
        /// <returns>Fuel required to launch module.</returns>
        /// <remarks>To find the fuel required for a module, take its mass, divide by three, round down, and subtract 2.</remarks>
        internal static int GetModuleFuelRequirement(int mass, bool includeFuelMass)
        {
            var fuelRequired = mass / 3 - 2;
            if (includeFuelMass)
            {
                var additionalFuel = fuelRequired;
                while (additionalFuel > 0)
                {
                    additionalFuel = GetModuleFuelRequirement(additionalFuel, includeFuelMass: false);
                    if (additionalFuel > 0)
                    {
                        fuelRequired += additionalFuel;
                    }
                }
            }

            return fuelRequired;
        }
    }
}
