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

        public OrbitMap(string path)
        {
            _adjList = new Dictionary<string, List<string>>();
            BuildMap(File.ReadAllLines(path));
        }

        public OrbitMap(string[] orbitStrings)
        {
            _adjList = new Dictionary<string, List<string>>();
            BuildMap(orbitStrings);
        }

        public int GetDirectOrbitCount()
        {
            return (_adjList != null ? _adjList.Keys.Count : 0);
        }

        public int GetIndirectOrbitCount()
        {
            return 0;
        }

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
