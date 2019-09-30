using System.Text;

namespace AES___
{
    partial class MultipleAES
    {
        public byte[] DecryptCBC(byte[] Text, byte[] Password)
        {
            Input = Text;
            BlocksCount = Input.Length / 16;
            if (Input.Length % 16 > 0)
            {
                BlocksCount++;
            }
            Crypto = new AES(Password);
            Output = new byte[BlocksCount * 16];

            int a = 0;
            for (int i = 0; i < BlocksCount; i++)
            {
                if (i == 0)
                {
                    SeparateBlocks();
                    HelpBlock = Block;
                    Encrypted = Crypto.InvCipher(Block);
                    Encrypted = XOR(VI, Encrypted);
                }
                else
                {
                    SeparateBlocks();
                    Encrypted = Crypto.InvCipher(Block);
                    Encrypted = XOR(HelpBlock, Encrypted);
                    HelpBlock = Block;
                }
                a = i * 16;
                for (int j = 0; j < 16; j++)
                {
                    Output[a++] = Encrypted[j];
                }
            }
            return Output;
        }   

        public byte[] DecryptOFB(byte[] Text, byte[] Password)
        {
            Input = Text;
            BlocksCount = Input.Length / 16;
            if (Input.Length % 16 > 0)
            {
                BlocksCount++;
            }
            Crypto = new AES(Password);
            Output = new byte[BlocksCount * 16];

            int a = 0;
            for (int i = 0; i < BlocksCount; i++)
            {
                SeparateBlocks();
                if (i == 0)
                {
                    Encrypted = Crypto.Cipher(VI);
                }
                else
                {
                    Encrypted = Crypto.Cipher(Encrypted);
                }
                Block = XOR(Block, Encrypted);
                a = i * 16;
                for (int j = 0; j < 16; j++)
                {
                    Output[a++] = Block[j];
                }
            }
            return Output;
        }

        public byte[] DecryptECB(byte[] Text, byte[] Password)
        {
            Input = Text;
            BlocksCount = Input.Length / 16;
            if (Input.Length % 16 > 0)
            {
                BlocksCount++;
            }
            Crypto = new AES(Password);
            Output = new byte[BlocksCount * 16];

            int a = 0;
            for (int i = 0; i < BlocksCount; i++)
            {
                SeparateBlocks();
                byte[] Encrypted = Crypto.InvCipher(Block);
                a = i * 16;
                for (int j = 0; j < 16; j++)
                {
                    Output[a++] = Encrypted[j];
                }
            }
            return Output;
        }

        public byte[] DecryptECB(byte[] Text, string Password)
        {
            Input = Text;
            BlocksCount = Input.Length / 16;
            if (Input.Length % 16 > 0)
            {
                BlocksCount++;
            }
            Crypto = new AES(Encoding.ASCII.GetBytes(Password));
            Output = new byte[BlocksCount * 16];

            int a = 0;
            for (int i = 0; i < BlocksCount; i++)
            {
                SeparateBlocks();
                byte[] Decrypted = Crypto.InvCipher(Block);
                a = i * 16;
                for (int j = 0; j < 16; j++)
                {
                    Output[a++] = Decrypted[j];
                }
            }
            return Output;
        }

        public byte[] DecryptECB(string Text, string Password)
        {
            Input = Encoding.ASCII.GetBytes(Text);
            BlocksCount = Input.Length / 16;
            if (Input.Length % 16 > 0)
            {
                BlocksCount++;
            }
            Crypto = new AES(Encoding.ASCII.GetBytes(Password));
            Output = new byte[BlocksCount * 16];

            int a = 0;
            for (int i = 0; i < BlocksCount; i++)
            {
                SeparateBlocks();
                byte[] Decrypted = Crypto.InvCipher(Block);
                a = i * 16;
                for (int j = 0; j < 16; j++)
                {
                    Output[a++] = Decrypted[j];
                }
            }
            return Output;
        }
    }
}
