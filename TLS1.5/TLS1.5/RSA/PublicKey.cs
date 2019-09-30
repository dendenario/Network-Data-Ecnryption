using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RSA_128
{
    class PublicKey
    {
        public BigInteger e { get; }
        public BigInteger n { get; }

    public PublicKey(BigInteger e, BigInteger n)
        {
            this.e = e;
            this.n = n;
        }

        public PublicKey(byte[] bytes)
        {
            byte len = bytes[0];
            byte[] e_help = new byte[len];
            byte[] n_help = new byte[bytes.Length - len];
            int a = 0;
            for (int i = 1; i < bytes.Length; i++)
            {
                if (i < len)
                {
                    e_help[i-1] = bytes[i];
                }
                else
                {
                    n_help[a++] = bytes[i];
                }
            }
            this.e = new BigInteger(e_help);
            this.n = new BigInteger(n_help);
        }


        public BigInteger Encrypt(BigInteger m)
        {
            return BigInteger.ModPow(m,e,n);
        }

        public byte[] ToBytes()
        {
            
            byte[] e_help = e.ToByteArray();
            byte[] n_help = n.ToByteArray();
            byte[] help = new byte[e_help.Length + n_help.Length+1];
            help[0] = (byte)e_help.Length;
            int a = 0;
            for (int i = 1; i < help.Length; i++)
            {
                if (i <= e_help.Length)
                {
                    help[i] = e_help[i-1];
                }
                else
                {
                    help[i] = n_help[a];
                    a++;
                }
            }
            return help;
        }
    }
}
