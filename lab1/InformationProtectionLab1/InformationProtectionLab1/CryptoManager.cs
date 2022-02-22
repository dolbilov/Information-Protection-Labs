using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic;

namespace InformationProtectionLab1
{
    public class CryptoManager
    {
        private readonly Dictionary<byte, byte> _decodingKey;
        private Dictionary<byte, byte> _encodingKey;

        public CryptoManager(string keyFileName)
        {
            if (!File.Exists(keyFileName))
            {
                CreateKey(keyFileName);
            }

            StreamReader sr = new(keyFileName);
            var originalBytes = sr.ReadLine().Split().Select(byte.Parse).ToList();
            var decodedBytes = sr.ReadLine().Split().Select(byte.Parse).ToList();
            sr.Close();

            _decodingKey = new Dictionary<byte, byte>();

            for (var i = 0; i < originalBytes.Count; i++)
            {
                var orig = originalBytes[i];
                var decoded = decodedBytes[i];

                _decodingKey[orig] = decoded;
            }
        }

        private void ChangeBytes(string filename, Dictionary<byte, byte> key)
        {
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException($"File {filename} not found");
            }

            var originalFile = new FileInfo(filename);
            var decodedFile = File.Create("_changed_" + originalFile.Name);

            var br = new BinaryReader(File.Open(filename, FileMode.Open));
            var bw = new BinaryWriter(decodedFile);
            
            // decoding
            while (br.BaseStream.Position != br.BaseStream.Length)
            {
                bw.Write(key[br.ReadByte()]);
            }
            
            br.Close();
            bw.Close();
            
            File.Delete(originalFile.Name);
            FileSystem.Rename(decodedFile.Name, originalFile.Name);
        }
        
        public void DecodeFile(string filename)
        {
            ChangeBytes(filename, _decodingKey);
        }
        
        
        public void EncodeFile(string filename)
        {
            if (_encodingKey == null)
            {
                CreateDecodingKey();
            }
            
            ChangeBytes(filename, _encodingKey);
        }

        private void CreateDecodingKey()
        {
            _encodingKey = new Dictionary<byte, byte>();

            foreach (var (key, value) in _decodingKey)
            {
                _encodingKey[value] = key;
            }
        }

        private void CreateKey(string keyFileName)
        {
            StreamWriter sw = new(keyFileName);
            List<int> nums = Enumerable.Range(0, 256).ToList();
            
            // write string "1 2 ... 255" (original key)
            sw.WriteLine(string.Join(" ", nums));

            Random rnd = new();
            StringBuilder sb = new();
            
            // write numbers decoding key
            for (var i = 0; i < 256; i++)
            {
                if (i != 0)
                {
                    sb.Append(' ');
                }

                var ind = rnd.Next(0, nums.Count);
                sb.Append($"{nums[ind]}");
                nums.RemoveAt(ind);
            }
            
            sw.WriteLine(sb.ToString());
            sw.Close();
            
            Console.WriteLine("Key was created by program");
        }
    }
}