using System;

namespace InformationProtectionLab2
{
    public static class RsaMath
    {
        public static long EulerFunction(long p, long q)
        {
            return Convert.ToInt64((p - 1) * (q - 1));
        }

        public static bool IsCoPrime(long num1, long num2)
        {
            if (num1 == num2)
            {
                return num1 == 1;
            }

            return num1 > num2 ? IsCoPrime(num1 - num2, num2) : IsCoPrime(num2 - num1, num1);
        }

        public static long ChooseE(long n)
        {
            for (long i = 1; i < n; i++)
            {
                if (IsCoPrime(i, n))
                {
                    return i;
                }
            }

            throw new ArgumentException($"Can't find co prime number for {n}");
        }

        public static long CalculateD(long pn, long e)
        {
            for (long k = 1;; k++)
            {
                var d = (double)(k * pn + 1) / e;
                if (d % 1 == 0)
                {
                    return Convert.ToInt64(d);
                }
            }
        }
        
        public static long PowMod(long x, long y, long N)
        {
            if (y == 0) return 1;
            var z = PowMod(x, y / 2, N);
            if (y % 2 == 0)
                return (z*z) % N;
            
            return (x*z*z) % N;
        }
    }
}