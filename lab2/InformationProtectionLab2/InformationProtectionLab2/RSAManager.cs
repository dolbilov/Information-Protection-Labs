using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace InformationProtectionLab2
{
    public class RSAManager
    {
        private const string OriginalFileName = "1.original.txt";
        private const string DecodedFileName = "2.decoded.txt";
        private const string EncodedFileName = "3.encoded.txt";
        
        # region Fields for math

        private long _p;
        private long _q;

        private long _n;
        private long _pn;
        private long _e;
        private long _d;

        # endregion

        #region Alphabets

        private static readonly Dictionary<int, char> OriginalIntToChar = new Dictionary<int, char>()
        {
            { 1, 'А' },
            { 2, 'Б' },
            { 3, 'В' },
            { 4, 'Г' },
            { 5, 'Д' },
            { 6, 'Е' },
            { 7, 'Ё' },
            { 8, 'Ж' },
            { 9, 'З' },
            { 10, 'И' },
            { 11, 'Й' },
            { 12, 'К' },
            { 13, 'Л' },
            { 14, 'М' },
            { 15, 'Н' },
            { 16, 'О' },
            { 17, 'П' },
            { 18, 'Р' },
            { 19, 'С' },
            { 20, 'Т' },
            { 21, 'У' },
            { 22, 'Ф' },
            { 23, 'Х' },
            { 24, 'Ц' },
            { 25, 'Ч' },
            { 26, 'Щ' },
            { 27, 'Щ' },
            { 28, 'Ъ' },
            { 29, 'Ы' },
            { 30, 'Ь' },
            { 31, 'Э' },
            { 32, 'Ю' },
            { 33, 'Я' },
            { 34, ' ' },
            { 35, '0' },
            { 36, '1' },
            { 37, '2' },
            { 38, '3' },
            { 39, '4' },
            { 40, '5' },
            { 41, '6' },
            { 42, '7' },
            { 43, '8' },
            { 44, '9' },
        };

        private static Dictionary<char, int> OriginalCharToInt;

        #endregion

        public RSAManager(int p, int q)
        {
            CalculateValues(p, q);
            CreateReversedDictionary();
            Decode();
            Encode();
        }

        private void CalculateValues(int p, int q)
        {
            _p = p;
            _q = q;

            _n = _p * _q;

            _pn = RsaMath.EulerFunction(p, q);
            _e = RsaMath.ChooseE(_n);

            Console.WriteLine($"Open key is ({_e}, {_n})");

            _d = RsaMath.CalculateD(_pn, _e);

            Console.WriteLine("Private key generated");
        }

        private void CreateReversedDictionary()
        {
            OriginalCharToInt = new Dictionary<char, int>();
            foreach (var (key, value) in OriginalIntToChar)
            {
                OriginalCharToInt[value] = key;
            }
        }

        private void Decode()
        {

            using (StreamReader sr = new(OriginalFileName))
            {
                string originalText = sr.ReadLine();
                StringBuilder sb = new();
                foreach (var letter in originalText)
                {
                    var ti = OriginalCharToInt[letter];
                    var ci = RsaMath.PowMod(ti, _e, _n);
                    sb.Append($"{ci} ");
                }
                
                var decodedText = sb.ToString().Trim();

                using (StreamWriter sw = new(DecodedFileName))
                {
                    sw.WriteLine(decodedText);
                }
            }
        }

        private void Encode()
        {
            using (StreamReader sr = new(DecodedFileName))
            {
                var nums = sr.ReadLine().Split(' ').Select(x => Convert.ToInt32(x)).ToList();
                StringBuilder sb = new();
                foreach (var num in nums)
                {
                    var codeOfOriginalLetter = (int)RsaMath.PowMod(num, _d, _n);
                    var originalLetter = OriginalIntToChar[codeOfOriginalLetter];
                    sb.Append(originalLetter);
                }

                var encodedText = sb.ToString();

                using (StreamWriter sw = new StreamWriter(EncodedFileName))
                {
                    sw.WriteLine(encodedText);
                }
            }
        }

        private void WriteToFile(string filename, string content)
        {
            StreamWriter sw = new(filename);
            sw.WriteLine(content);
            sw.Close();
        }
    }
}