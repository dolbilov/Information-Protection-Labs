using System.Text;

namespace InformationProtectionLab1
{
    public class CryptoManager
    {
        private readonly Dictionary<byte, byte> _encodingKey;
        private Dictionary<byte, byte> _decodingKey;

        public CryptoManager(string keyFileName)
        {
            if (!File.Exists(keyFileName))
            {
                CreateKey(keyFileName);
            }

            StreamReader sr = new(keyFileName);
            var originalBytes = sr.ReadLine().Split().Select(byte.Parse).ToList();
            var encodedBytes = sr.ReadLine().Split().Select(byte.Parse).ToList();
            sr.Close();

            _encodingKey = new Dictionary<byte, byte>();

            for (var i = 0; i < originalBytes.Count; i++)
            {
                var orig = originalBytes[i];
                var encoded = encodedBytes[i];

                _encodingKey[orig] = encoded;
            }
        }

        private void ChangeBytes(string filename, Dictionary<byte, byte> key)
        {
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException($"File {filename} not found");
            }

            var originalFile = new FileInfo(filename);
            var encodedFile = File.Create("../../../res/" + "_changed_" + originalFile.Name);

            var br = new BinaryReader(File.Open(filename, FileMode.Open));
            var bw = new BinaryWriter(encodedFile);
            
            // encoding
            while (br.BaseStream.Position != br.BaseStream.Length)
            {
                bw.Write(key[br.ReadByte()]);
            }
            
            br.Close();
            bw.Close();
            
            File.Delete(originalFile.FullName);
            Microsoft.VisualBasic.FileIO.FileSystem.RenameFile(encodedFile.Name, originalFile.Name);
        }
        
        public void EncodeFile(string filename)
        {
            ChangeBytes(filename, _encodingKey);
        }
        
        
        public void DecodeFile(string filename)
        {
            if (_decodingKey == null)
            {
                CreateDecodingKey();
            }
            
            ChangeBytes(filename, _decodingKey);
        }

        private void CreateDecodingKey()
        {
            _decodingKey = new Dictionary<byte, byte>();

            foreach (var (key, value) in _encodingKey)
            {
                _decodingKey[value] = key;
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
            
            // write numbers encoding key
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