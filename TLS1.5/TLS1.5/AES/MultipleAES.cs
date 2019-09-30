using System;
using System.Text;

namespace AES___
{
    partial class MultipleAES
    {
        private byte[] Input;
        private byte[] Output;
        private byte[] Block;
        private byte[] HelpBlock;
        private byte[] Encrypted;
        private int BlocksCount;
        private AES Crypto;
        private byte[] VI;
        public MultipleAES()
        {
            VI = new byte[16];
            Random rand = new Random();
            for (int i = 0; i < VI.Length; i++)
            {
                VI[i] = (byte)rand.Next(0,255);
            }
        }

        public byte[] XOR(byte[] OpenedBlock, byte[] CypheredBlock)
        {
            byte[] help = new byte[OpenedBlock.Length];
            for (int i = 0; i < 16; i++)
            {
                help[i] = (byte)(CypheredBlock[i] ^ OpenedBlock[i]);
            }
            return help;
        }

        public byte[] EncryptOFB(byte[] Text, byte[] Password)
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

        public byte[] EncryptCBC(byte[] Text, byte[] Password)
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
                    Block = XOR(VI, Block);
                }
                else
                {
                    Block = XOR(Block, Encrypted);
                }
                Encrypted = Crypto.Cipher(Block);
                a = i * 16;
                for (int j = 0; j < 16; j++)
                {
                    Output[a++] = Encrypted[j];
                }
            }
            return Output;
        }

       
        public byte[] EncryptECB(string Text, string Password)
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
                byte[] Encrypted = Crypto.Cipher(Block);
                a = i * 16;
                for (int j = 0; j < 16; j++)
                {
                    Output[a++] = Encrypted[j];
                }
            }
            return Output;
        }
        public byte[] EncryptECB(byte[] Text, byte[] Password)
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
                byte[] Encrypted = Crypto.Cipher(Block);
                a = i * 16;
                for (int j = 0; j < 16; j++)
                { 
                    Output[a++] = Encrypted[j];
                }
            }
            return Output;
        }

        public byte[] EncryptECB(byte[] Text, string Password)
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
                byte[] Encrypted = Crypto.Cipher(Block);
                a = i * 16;
                for (int j = 0; j < 16; j++)
                {
                    Output[a++] = Encrypted[j];
                }
            }
            return Output;
        }
        private void SeparateBlocks()
        {
            Block = new byte[16];
            if (Input.Length <= 16)
            {
                for (int i = 0; i < Input.Length; i++)
                {
                    Block[i] = Input[i];
                }
            }
            else
            {
                byte[] help = new byte[Input.Length - 16];
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
    }
}
