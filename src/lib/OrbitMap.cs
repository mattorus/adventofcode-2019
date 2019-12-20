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
        private readonly Dictionary<string, string> _orbitCenters;
        private static readonly string _centerOfMass = "COM";

        /// <summary>
        /// Initializes a new instance of the <see cref="OrbitMap"/> class.
        /// </summary>
        /// <param name="path"></param>
        public OrbitMap(string path)
        {
            _adjList = new Dictionary<string, List<string>>();
            _orbitCenters = new Dictionary<string, string>();

            BuildMap(File.ReadAllLines(path));
            Validate();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrbitMap"/> class.
        /// </summary>
        /// <param name="orbitStrings"></param>
        public OrbitMap(string[] orbitStrings)
        {
            _adjList = new Dictionary<string, List<string>>();
            _orbitCenters = new Dictionary<string, string>();

            BuildMap(orbitStrings);
            Validate();            
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
        /// Finds the subject planet of the given orbit.
        /// </summary>
        /// <param name="orbit"></param>
        /// <returns>The respective orbit's planet.</returns>
        public string FindPlanetFromOrbit(string orbit)
        {
            foreach (KeyValuePair<string, List<string>> kvPair in _adjList)
            {
                if (kvPair.Value.Contains(orbit))
                {
                    return kvPair.Key;
                }
            }

            throw new ArgumentNullException($"Planet not found!-----{orbit}");
        }

        /// <summary>
        /// Finds the minimum number of orbital transfers between the given orbits.
        /// </summary>
        /// <param name="planetA"></param>
        /// <param name="planetB"></param>
        /// <returns>Orbital transfer count.</returns>
        public int FindMinOrbitPath(string orbitA, string orbitB)
        {
            // Perform DFS from LowestCommonAncestor of planetA & planetB to find path
            var lca = LowestCommonAncestor(orbitA, orbitB);

            return OrbitalTransferCount(lca, orbitA, orbitB);
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
        /// Finds the total number of orbits in the map.
        /// </summary>
        /// <returns>Count of total orbits.</returns>
        public int TotalOrbitCount()
        {
            int numOrbits = 0;
            
            foreach (string planet in _adjList.Keys)
            {
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
            }

            return numOrbits;
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
                _orbitCenters[value] = key;
            }
        }
        
        /// <summary>
        /// Ensures map contains a center of mass.
        /// </summary>
        private void Validate()
        {
            if (!_adjList.ContainsKey(_centerOfMass))
            {
                throw new ArgumentException($"Invalid map! Must contain {_centerOfMass}");
            }
        }

        /// <summary>
        /// Searches for the lowest common ancestor in the map.
        /// </summary>
        /// <param name="orbitA"></param>
        /// <param name="orbitB"></param>
        /// <returns>The lowest common ancestor if successful, empty string otherwise</returns>
        private string LowestCommonAncestor(string orbitA, string orbitB)
        {
            HashSet<string> ancestors = new HashSet<string>();

            var planetA = _orbitCenters[orbitA];
            var planetB = _orbitCenters[orbitB];

            while (_orbitCenters.ContainsKey(planetA))
            {
                ancestors.Add(planetA);
                planetA = _orbitCenters[planetA];
            }

            while (_orbitCenters.ContainsKey(planetB))
            {
                if (ancestors.Contains(_orbitCenters[planetB]))
                {
                    return _orbitCenters[planetB];                    
                }

                ancestors.Add(planetB);
                planetB = _orbitCenters[planetB];
            }

            return "";
        }

        /// <summary>
        /// Search for the path length between orbitA and OrbitB
        /// </summary>
        /// <param name="ancestor"></param>
        /// <param name="orbitA"></param>
        /// <param name="orbitB"></param>
        /// <returns>The path length if successful, -1 otherwise.</returns>
        private int OrbitalTransferCount(string ancestor, string orbitA, string orbitB)
        {
            return PathLength(ancestor, orbitA) + PathLength(ancestor, orbitB);
        }

        /// <summary>
        /// Search for the path between ancestor and given orbit's center.
        /// </summary>
        /// <param name="ancestor"></param>
        /// <param name="orbit"></param>
        /// <returns>The path length.</returns>
        private int PathLength(string ancestor, string orbit)
        {
            if (_orbitCenters[orbit] == ancestor)
            {
                return 0;
            }

            return 1 + PathLength(ancestor, _orbitCenters[orbit]);
        }
        
    }
}
