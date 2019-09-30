namespace AES___
{
    class GF
    {
        public const int m = 8;
        public static PolynomZ2 b = new PolynomZ2(0x10);
        public static PolynomZ2 f = new PolynomZ2(0x11b); //PolynomZ2.GenerateIrreducablePolynomZ2(m);
        public PolynomZ2 p;

        public byte Value
        {
            get
            {
                return (byte)p.p;
            }
        }
        public GF()
        {
            p = new PolynomZ2(0);
        }
        public GF(PolynomZ2 p)
        {
            this.p = p % f;
        }
        public GF(byte a)
        {
            this.p = new PolynomZ2(a);
        }
        public static GF operator +(GF a, GF b)
        {
            return new GF((a.p + b.p) % f);
        }
        public static GF operator *(GF a, GF b)
        {
            return new GF((a.p * b.p) % f);
        }
        public override string ToString()
        {
            return p.ToString();
        }
        public static void NextIrreducable()
        {
            f = PolynomZ2.GenerateIrreducablePolynomZ2(m);
        }

        public static implicit operator GF(byte a)
        {
            GF A = new GF(new PolynomZ2(a));
            return A;
        }
    }
}
