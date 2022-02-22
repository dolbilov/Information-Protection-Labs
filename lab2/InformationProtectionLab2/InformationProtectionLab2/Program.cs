using System;

namespace InformationProtectionLab2
{
    public static class Program
    {
        private static void Main()
        {
            try
            {
                RSAManager rsaManager = new(7, 13);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Some error occurred: {e.Message}");
                Environment.Exit(-1);
            }
        }
    }
}