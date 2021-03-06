using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Channels;

namespace InformationProtectionLab1
{
    public static class Program
    {
        private const string Filename = "../../../res/text.doc";
        
        private static void Main()
        {
            try
            {
                Console.WriteLine("Task 1.1");
                DocManager docManager = new(Filename);
                var fileLength = docManager.GetFileLength();
                Console.WriteLine($"File length: {fileLength} bytes");

                Console.WriteLine("\n\nTask 1.2");
                BinaryFileManager binaryFileManager = new(Filename);
                var dict = binaryFileManager.CountBytes();
                var relativeFrequencies = binaryFileManager.CalculateRelativeFrequency(dict);
                foreach (var (key, value) in relativeFrequencies)
                {
                    Console.WriteLine($"{key}: {value * 100:f2}%");
                }
                
                Console.WriteLine("\n\nTask 2");
                var cm = new CryptoManager("../../../res/key.txt");
                int command;
                do
                {
                    Console.WriteLine("What would you do?\n1 - Decode\n2 - Encode\n0 - Exit");
                    command = Convert.ToInt32(Console.ReadLine());
                    if (command == 1)
                    {
                        cm.DecodeFile(Filename);
                    }
                    else if (command == 2)
                    {
                        cm.EncodeFile(Filename);
                    }
                } while (command is 1 or 2);

            }
            catch (Exception e)
            {
                Console.WriteLine($"Some error occurred: {e.Message}");
                Environment.Exit(-1);
            }
        }
    }
}