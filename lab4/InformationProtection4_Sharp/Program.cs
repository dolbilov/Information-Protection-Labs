using System.Data;
using System.Text;

namespace InformationProtection4_Sharp;

public static class Program
{
    private const string Prefix = "../../../res/";
    private const string KeyFileName = Prefix + "key.txt";
    private const string TextFileName = Prefix + "text.txt";

    private static void Main()
    {
        try
        {
            string key = "This is a crypto blowfish 448 bits key and 64 bits text!";
            if (!File.Exists(Prefix + "key.txt"))
            {
                Console.WriteLine("Key file not found. Key was set to default.");
            }
            else
            {
                using StreamReader sr = new(KeyFileName);
                key = sr.ReadLine() ?? key;
                sr.Close();
            }

            

            string msg;
            using (StreamReader sr = new(TextFileName))
            {
                msg = sr.ReadLine() ?? "Default message to encode";
                sr.Close();
            }

            BlowfishCoder coder = new(key);
            byte[] bytes = coder.EncodeMessage(msg);
            Console.WriteLine($"Message encoded ({bytes.Length} bytes). Coded bytes:");
            foreach (byte b in bytes)
            {
                Console.Write($"{b} ");
            }
            Console.WriteLine();


            string decodedMessage = coder.DecodeMessage(bytes);
            Console.WriteLine("\nMessage decoded. Decoded text:");
            Console.WriteLine(decodedMessage);

        }
        catch (Exception e)
        {
            Console.WriteLine($"Some error occurred: {e.Message}");
            Environment.Exit(-1);
        }
        
    }
}