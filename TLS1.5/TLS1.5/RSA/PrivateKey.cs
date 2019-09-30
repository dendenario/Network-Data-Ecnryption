using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RSA_128
{
    class PrivateKey
    {
        BigInteger d;
        BigInteger n;

        public PrivateKey(BigInteger d, BigInteger n)
        {
            this.d = d;
            this.n = n;
        }
        public BigInteger Decrypt(BigInteger c)
        {
            BigInteger m = BigInteger.ModPow(c, d, n);
            return m;
        }
    }
}
