using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApp2
{


    class Md5
    {
        //private static int MD5_DIGEST_SIZE = 16;
        //private static int MD5_HMAC_BLOCK_SIZE = 64;
        private static int MD5_BLOCK_WORDS = 16;
        private static int MD5_HASH_WORDS = 4;

        private  UInt32[] hash = new UInt32[MD5_HASH_WORDS];
        private  UInt32[] block = new UInt32[MD5_BLOCK_WORDS];
        //private UInt32 byte_count;
        private int BlocksCount;

        private UInt32[] Input;
        private UInt32[] Output;
        private UInt32[] Block;
        private UInt32[] InputHelp;

        public Md5()
        {

        }

        public string GetHash(string text)
        {
            byte[]help = Encoding.UTF8.GetBytes(text);            
            Input = new UInt32[(int)(Math.Floor((double)help.Length/4))]; 
            for (int i = 0; i < Input.Length; i++)
            {
                Input[i] = BitConverter.ToUInt32(help,i*4);
            }

            if (Input.Length % 16 > 0)
            {
                InputHelp = new UInt32[Input.Length + (16 - Input.Length % 16)];
                for (int i = 0; i < InputHelp.Length; i++)
                {
                    if (i < Input.Length)
                    {
                        InputHelp[i] = Input[i];
                    }
                    else
                    {
                        InputHelp[i] = 0;
                    }
                }
            }

            BlocksCount = InputHelp.Length / 16;
            if (Input.Length % 16 > 0)
            {
                BlocksCount++;
            }
            Output = new UInt32[4];
            for (int i = 0; i < BlocksCount; i++)
            {
                SeparateBlocks();
                md5_transform(Block);
                for (int j = 0; j < 4; j++)
                {
                    Output[j] ^= hash[j];
                }
            }
            byte[] finalhash = new byte[16];
            byte[] buffer = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                buffer = BitConverter.GetBytes(Output[i]);
                for (int j = 0; j < 4; j++)
                {
                    finalhash[i * 4 + j] = buffer[j];
                }
            }
            return Encoding.UTF8.GetString(finalhash);
        }

        private void SeparateBlocks()
        {
            Block = new UInt32[16];
            if (Input.Length <= 16)
            {
                for (int i = 0; i < Input.Length; i++)
                {
                    Block[i] = Input[i];
                }
            }
            else
            {
                UInt32[] help = new UInt32[Input.Length - 16];
                for (int i = 0; i < 16; i++)
                {
                    Block[i] = Input[i];
                }
                int a = 16;
                for (int i = 0; i < help.Length; i++)
                {
                    help[i] = Input[a++];
                }
                Input = help;
            }
        }
        private  UInt32 F1(UInt32 x, UInt32 y, UInt32 z)
        {
            return (z ^ (x & (y ^ z)));
        }
        private  UInt32 F2(UInt32 x, UInt32 y, UInt32 z)
        {
            return F1(z, x, y);
        }

        private  UInt32 F3(UInt32 x, UInt32 y, UInt32 z)
        {
            return x ^ y ^ z;
        }
        private  UInt32 F4(UInt32 x, UInt32 y, UInt32 z)
        {
            return (y ^ (x | ~z));
        }

        private  void MD5STEP(Func<UInt32, UInt32, UInt32, UInt32> f, ref UInt32 w, UInt32 x, UInt32 y, UInt32 z, UInt32 input, byte s)
        {
            w += f(x, y, z) + input;
            w = (w << s | w >> (32 - s)) + x;
        }


         void md5_transform(UInt32[] input)
        {

            UInt32 a, b, c, d;

            a = hash[0];
            b = hash[1];
            c = hash[2];
            d = hash[3];

            MD5STEP(F1, ref a, b, c, d, input[0] + 0xd76aa478, 7);
            MD5STEP(F1, ref d, a, b, c, input[1] + 0xe8c7b756, 12);
            MD5STEP(F1, ref c, d, a, b, input[2] + 0x242070db, 17);
            MD5STEP(F1, ref b, c, d, a, input[3] + 0xc1bdceee, 22);
            MD5STEP(F1, ref a, b, c, d, input[4] + 0xf57c0faf, 7);
            MD5STEP(F1, ref d, a, b, c, input[5] + 0x4787c62a, 12);
            MD5STEP(F1, ref c, d, a, b, input[6] + 0xa8304613, 17);
            MD5STEP(F1, ref b, c, d, a, input[7] + 0xfd469501, 22);
            MD5STEP(F1, ref a, b, c, d, input[8] + 0x698098d8, 7);
            MD5STEP(F1, ref d, a, b, c, input[9] + 0x8b44f7af, 12);
            MD5STEP(F1, ref c, d, a, b, input[10] + 0xffff5bb1, 17);
            MD5STEP(F1, ref b, c, d, a, input[11] + 0x895cd7be, 22);
            MD5STEP(F1, ref a, b, c, d, input[12] + 0x6b901122, 7);
            MD5STEP(F1, ref d, a, b, c, input[13] + 0xfd987193, 12);
            MD5STEP(F1, ref c, d, a, b, input[14] + 0xa679438e, 17);
            MD5STEP(F1, ref b, c, d, a, input[15] + 0x49b40821, 22);

            MD5STEP(F2, ref a, b, c, d, input[1] + 0xf61e2562, 5);
            MD5STEP(F2, ref d, a, b, c, input[6] + 0xc040b340, 9);
            MD5STEP(F2, ref c, d, a, b, input[11] + 0x265e5a51, 14);
            MD5STEP(F2, ref b, c, d, a, input[0] + 0xe9b6c7aa, 20);
            MD5STEP(F2, ref a, b, c, d, input[5] + 0xd62f105d, 5);
            MD5STEP(F2, ref d, a, b, c, input[10] + 0x02441453, 9);
            MD5STEP(F2, ref c, d, a, b, input[15] + 0xd8a1e681, 14);
            MD5STEP(F2, ref b, c, d, a, input[4] + 0xe7d3fbc8, 20);
            MD5STEP(F2, ref a, b, c, d, input[9] + 0x21e1cde6, 5);
            MD5STEP(F2, ref d, a, b, c, input[14] + 0xc33707d6, 9);
            MD5STEP(F2, ref c, d, a, b, input[3] + 0xf4d50d87, 14);
            MD5STEP(F2, ref b, c, d, a, input[8] + 0x455a14ed, 20);
            MD5STEP(F2, ref a, b, c, d, input[13] + 0xa9e3e905, 5);
            MD5STEP(F2, ref d, a, b, c, input[2] + 0xfcefa3f8, 9);
            MD5STEP(F2, ref c, d, a, b, input[7] + 0x676f02d9, 14);
            MD5STEP(F2, ref b, c, d, a, input[12] + 0x8d2a4c8a, 20);

            MD5STEP(F3, ref a, b, c, d, input[5] + 0xfffa3942, 4);
            MD5STEP(F3, ref d, a, b, c, input[8] + 0x8771f681, 11);
            MD5STEP(F3, ref c, d, a, b, input[11] + 0x6d9d6122, 16);
            MD5STEP(F3, ref b, c, d, a, input[14] + 0xfde5380c, 23);
            MD5STEP(F3, ref a, b, c, d, input[1] + 0xa4beea44, 4);
            MD5STEP(F3, ref d, a, b, c, input[4] + 0x4bdecfa9, 11);
            MD5STEP(F3, ref c, d, a, b, input[7] + 0xf6bb4b60, 16);
            MD5STEP(F3, ref b, c, d, a, input[10] + 0xbebfbc70, 23);
            MD5STEP(F3, ref a, b, c, d, input[13] + 0x289b7ec6, 4);
            MD5STEP(F3, ref d, a, b, c, input[0] + 0xeaa127fa, 11);
            MD5STEP(F3, ref c, d, a, b, input[3] + 0xd4ef3085, 16);
            MD5STEP(F3, ref b, c, d, a, input[6] + 0x04881d05, 23);
            MD5STEP(F3, ref a, b, c, d, input[9] + 0xd9d4d039, 4);
            MD5STEP(F3, ref d, a, b, c, input[12] + 0xe6db99e5, 11);
            MD5STEP(F3, ref c, d, a, b, input[15] + 0x1fa27cf8, 16);
            MD5STEP(F3, ref b, c, d, a, input[2] + 0xc4ac5665, 23);

            MD5STEP(F4, ref a, b, c, d, input[0] + 0xf4292244, 6);
            MD5STEP(F4, ref d, a, b, c, input[7] + 0x432aff97, 10);
            MD5STEP(F4, ref c, d, a, b, input[14] + 0xab9423a7, 15);
            MD5STEP(F4, ref b, c, d, a, input[5] + 0xfc93a039, 21);
            MD5STEP(F4, ref a, b, c, d, input[12] + 0x655b59c3, 6);
            MD5STEP(F4, ref d, a, b, c, input[3] + 0x8f0ccc92, 10);
            MD5STEP(F4, ref c, d, a, b, input[10] + 0xffeff47d, 15);
            MD5STEP(F4, ref b, c, d, a, input[1] + 0x85845dd1, 21);
            MD5STEP(F4, ref a, b, c, d, input[8] + 0x6fa87e4f, 6);
            MD5STEP(F4, ref d, a, b, c, input[15] + 0xfe2ce6e0, 10);
            MD5STEP(F4, ref c, d, a, b, input[6] + 0xa3014314, 15);
            MD5STEP(F4, ref b, c, d, a, input[13] + 0x4e0811a1, 21);
            MD5STEP(F4, ref a, b, c, d, input[4] + 0xf7537e82, 6);
            MD5STEP(F4, ref d, a, b, c, input[11] + 0xbd3af235, 10);
            MD5STEP(F4, ref c, d, a, b, input[2] + 0x2ad7d2bb, 15);
            MD5STEP(F4, ref b, c, d, a, input[9] + 0xeb86d391, 21);

            hash[0] += a;
            hash[1] += b;
            hash[2] += c;
            hash[3] += d;
        }

    }

}
