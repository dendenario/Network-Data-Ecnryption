using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Collections;

namespace RSA_128
{
    class RSA
    {


        private Random random = new Random();

        public PublicKey pub;
        public PrivateKey priv;


        public RSA()
        {
            GenerateKeys(128);
        }
        public  BigInteger Encrypt(byte[] text)
        {
            BigInteger m = new BigInteger(text);
            return pub.Encrypt(m); 
        }

        public  BigInteger Decrypt(BigInteger c)
        {
            return priv.Decrypt(c);
        }
        private void GenerateKeys(int size)
        {
            BigInteger p = 0;
            BigInteger q = 0;
            for (int i = 0; i < 100; i++)
            {
                q = GeneratePrime(BigInteger.Pow(2, (size / 2) + 5));
                p = GeneratePrime(BigInteger.Pow(2, (size / 2) + 5));
                if ((p * q) > BigInteger.Pow(2, size))
                {
                    break;
                }
            }
            //Console.WriteLine(p);
            //Console.WriteLine(q);
            BigInteger n = q * p;
            BigInteger fi = (p - 1) * (q - 1);
            BigInteger e = Generate(fi);
            while (BigInteger.GreatestCommonDivisor(e, fi) > 1)
            {
                e = Generate(fi);
            }
            BigInteger d = Inverse(e,fi);
            pub = new PublicKey(e, n);
            priv = new PrivateKey(d, n);
        }

         private BigInteger Inverse(BigInteger a, BigInteger n)
        {
            BigInteger i = n, v = 0, d = 1;
            while (a > 0)
            {
                BigInteger t = i / a, x = a;
                a = i % x;
                i = x;
                x = d;
                d = v - t * x;
                v = x;
            }
            v %= n;
            if (v < 0) v = (v + n) % n;
            return v;
        }

        private BigInteger GeneratePrime(BigInteger N)  //Генерация простого числа 
        {
            for (int i = 0; i < 10000; i++)
            {
                BigInteger temp = Generate(N);
                if (IsPrimeMillerRabin(temp, 100))
                {
                    return temp;
                }
            }
            return -1;
        }

        private  bool IsPrimeMillerRabin(BigInteger n, int s) //Проверка на простоту с помощью Миллера-Рабина
        {
            for (int j = 0; j < s; j++)
            {
                BigInteger a = Generate(n);
                if (Witness(a, n))
                {
                    return false;
                }
            }
            return true;
        }

        private  bool Witness(BigInteger a, BigInteger n) // Является ли число составным
        {
            BigInteger u = n - 1;
            int t = 0;
            while ((u % 2) == 0)
            {
                t++;
                u = u / 2;
            }
            BigInteger[] x = new BigInteger[t + 1];
            x[0] = BigInteger.ModPow(a, u, n);
            for (int i = 1; i <= t; i++)
            {
                x[i] = BigInteger.ModPow(x[i - 1], 2, n);
                if (x[i] == 1 && x[i - 1] != 1 && x[i - 1] != (n - 1))
                {
                    return true;
                }
            }
            if (x[t] != 1)
            {
                return true;
            }
            return false;
        }

        private  BigInteger Generate(BigInteger N)
        {
            byte[] bytes = N.ToByteArray();
            BigInteger R;

            do
            {
                random.NextBytes(bytes);
                bytes[bytes.Length - 1] &= (byte)0x7F; //force sign bit to positive
                R = new BigInteger(bytes);
            } while (R >= N);

            return R;
        }

    }
}
