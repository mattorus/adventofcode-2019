using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2019
{
    public class OrbitMap
    {
        private readonly Dictionary<string, List<string>> _adjList; 

        /// <summary>
        /// Initializes a new instance of the <see cref="OrbitMap"/> class.
        /// </summary>
        /// <param name="path"></param>
        public OrbitMap(string path)
        {
            _adjList = new Dictionary<string, List<string>>();
            BuildMap(File.ReadAllLines(path));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrbitMap"/> class.
        /// </summary>
        /// <param name="orbitStrings"></param>
        public OrbitMap(string[] orbitStrings)
        {
            _adjList = new Dictionary<string, List<string>>();
            BuildMap(orbitStrings);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int DirectOrbitCount()
        {
            int numOrbits = 0;

            foreach (List<string> orbits in _adjList.Values)
            {
                numOrbits += orbits.Count;
            }

            return numOrbits;
        }

        /// <summary>
        /// Finds the total number of orbits in the map.
        /// </summary>
        /// <returns>Count of total orbits.</returns>
        public int TotalOrbitCount()
        {
            int numOrbits = 0;
            
            foreach (string planet in _adjList.Keys)
            {
                // Console.WriteLine($"IndirectOrbitCount for {planet} -->");
                numOrbits += IndirectOrbitCount(planet, new HashSet<string>());               
            }

            return numOrbits;
        }

        /// <summary>
        /// Finds the total number of orbits for the given planet.
        /// </summary>
        /// <param name="planet"></param>
        /// <param name="visited"></param>
        /// <returns>Count of total orbits.</returns>
        private int IndirectOrbitCount(string planet, HashSet<string> visited)
        {
            // Perform DFS of adjList
            int numOrbits = 0;

            if (!_adjList.ContainsKey(planet))
            {
                return 0;
            }

            foreach (string orbit in _adjList[planet])
            {
                if (visited.Contains(orbit))
                {
                    continue;
                }

                visited.Add(orbit);
                numOrbits += IndirectOrbitCount(orbit, new HashSet<string>(visited)) + 1;
                // Console.WriteLine($"\t\t--> {orbit} = {numOrbits}");
            }

            return numOrbits;
        }

        /// <summary>
        /// Prints a visual representation of Map's adjacency list.
        /// </summary>
        public void PrintMap()
        {
            foreach (KeyValuePair<string, List<string>> kv in _adjList)
            {
                string str = "";

                if (kv.Value.Count == 1)
                {
                    str = kv.Value[^1];
                }

                else if (kv.Value.Count > 1)
                {
                    str = String.Join(", ", kv.Value);
                }

                Console.WriteLine($"{kv.Key} : [ {str} ]");
            }
        }

        /// <summary>
        /// Populates Map's adjacency list.
        /// </summary>
        /// <param name="orbitStrings"></param>
        private void BuildMap(string[] orbitStrings)
        {
            // Populate AdjList from each line
            foreach (string orbit in orbitStrings)
            {
                var orbitParams = orbit.Split(')');
                var key = orbitParams[0];
                var value = orbitParams[1];

                if (!_adjList.ContainsKey(key))
                {
                    _adjList[key] = new List<string>();
                }

                _adjList[key].Add(value);
            }
        }

        
    }
}
