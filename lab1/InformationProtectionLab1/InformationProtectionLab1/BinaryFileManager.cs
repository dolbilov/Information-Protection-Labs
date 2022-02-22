using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace InformationProtectionLab1
{
    public class BinaryFileManager
    {
        private BinaryReader _binaryReader;
        
        public BinaryFileManager(string filename)
        {
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException($"File {filename} not found");
            }

            _binaryReader = new BinaryReader(File.Open(filename, FileMode.Open));
        }

        public Dictionary<byte, int> CountBytes()
        {
            var dict = new Dictionary<byte, int>();
            
            while (_binaryReader.BaseStream.Position != _binaryReader.BaseStream.Length)
            {
                var b = _binaryReader.ReadByte();
                if (dict.ContainsKey(b))
                {
                    dict[b]++;
                }
                else
                {
                    dict[b] = 1;
                }
            }

            _binaryReader.Close();

            return dict;
        }

        public Dictionary<byte, float> CalculateRelativeFrequency(Dictionary<byte, int> dict)
        {
            var totalBytes = dict.Values.Sum();
            Console.WriteLine(totalBytes);

            var relativeFrequencies = new Dictionary<byte, float>();

            foreach (var (key, value) in dict)
            {
                relativeFrequencies[key] = (float) value / totalBytes;
            }

            return relativeFrequencies;
        }
    }
}