namespace AES___
{
    partial class AES
    {
        public byte[] InvCipher(byte[] Block)
        {
            In = Block;
            State = new GF[4, Nb];
            for (int r = 0; r < 4; r++)
                for (int c = 0; c < Nb; c++)
                {
                    State[r, c] = new GF();
                    State[r, c] = In[r + (4 * c)];
                }
            AddRoundKey(Nr);
            for (int round = Nr-1; round != 0; round--)
            {
                InvShiftRows();
                InvSubBytes();
                AddRoundKey(round);
                InvMixColumns();
            }

            InvShiftRows();
            InvSubBytes();
            AddRoundKey(0);
            Out = new byte[In.Length];
            for (int r = 0; r < 4; r++)
                for (int c = 0; c < Nb; c++)
                {
                    Out[r + (4 * c)] = State[r, c].Value;
                }
            //Print(State);
            return Out;
        }
        private void InvShiftRows()
        {
            GF[,] help = (GF[,])State.Clone();
            for (int r = 0; r < 4; r++)
                for (int c = 0; c < Nb; c++)
                {
                    help[r, (c+Shift(r,Nb))%Nb] = State[r, c];
                }
            State = help;
        }

        private void InvSubBytes()
        {
            for (int r = 0; r < 4; r++)
                for (int c = 0; c < Nb; c++)
                {
                    byte x = (byte)(State[r, c].p.p >> 4);
                    byte y = (byte)(State[r, c].p.p % 16);
                    State[r, c].p.p = InvSbox[x, y];
                }
        }

        private void InvMixColumns()
        {
            GF[,] help = (GF[,])State.Clone();
            for (int c = 0; c < Nb; c++)
            {
                help[0, c] = 0x0e * State[0, c] + 0x0b * State[1, c] + 0x0d * State[2, c] + 0x09 * State[3, c];
                help[1, c] = 0x09 * State[0, c] + 0x0e * State[1, c] + 0x0b * State[2, c] + 0x0d * State[3, c];
                help[2, c] = 0x0d * State[0, c] + 0x09 * State[1, c] + 0x0e * State[2, c] + 0x0b * State[3, c];
                help[3, c] = 0x0b * State[0, c] + 0x0d * State[1, c] + 0x09 * State[2, c] + 0x0e * State[3, c];
            }
            State = help;
        }
    }
}
