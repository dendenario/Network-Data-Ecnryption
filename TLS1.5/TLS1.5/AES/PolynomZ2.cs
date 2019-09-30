using System;

namespace AES___
{
    class PolynomZ2 : IComparable
    {
        public int p; // битовое представление много

        public PolynomZ2(int x = 0)
        {
            p = x;
        }
        public PolynomZ2(int[] x)
        {
            //
            p = 0;
            for (byte i = (byte)(x.Length - 1), j = 0; i >= 0; i--, j++)
            {
                p |= (x[i] << j);
            }
        }
        public PolynomZ2(bool[] x)
        {
            //
            p = 0;
            for (int i = x.Length - 1, j = 0; i >= 0; i--, j++)
            {
                if (x[i]) p |= 1 << j;
            }
        }
        public PolynomZ2(PolynomZ2 a)
        {
            p = a.p;
        }
        public override string ToString()
        {
            if (p == 0) return "0";
            bool first = true;
            string polinom_str = "";
            for (int i = 31; i > 0; --i)
            {
                if (((p >> i) & 1) == 1)
                {
                    if (i > 0 && !first) polinom_str += "+";
                    polinom_str += "x^" + i.ToString();
                    first = false;
                }
            }
            if ((p & 1) == 1)
            {
                if (!first) polinom_str += "+";
                polinom_str += "1";
                first = false;
            }
            return polinom_str;
        }

        static public PolynomZ2 operator *(PolynomZ2 a, PolynomZ2 b)
        {
            PolynomZ2 c = new PolynomZ2();
            for (int i = 0; i <= 31; i++)
            {
                if ((a.p >> i & 1) == 1)
                {
                    c.p ^= b.p << i;
                }
            }
            return c;
        }
        public void xtime()
        {
            PolynomZ2 c = new PolynomZ2();
            PolynomZ2 b = new PolynomZ2(0x02);
            for (int i = 0; i <= 31; i++)
            {
                if ((this.p >> i & 1) == 1)
                {
                    c.p ^= b.p << i;
                }
            }
            this.p = c.p;
        }
        static public PolynomZ2 operator +(PolynomZ2 a, PolynomZ2 b)
        {
            PolynomZ2 c = new PolynomZ2();
            c.p = a.p ^ b.p;
            return c;
        }
        static public PolynomZ2 operator -(PolynomZ2 a, PolynomZ2 b)
        {
            PolynomZ2 c = new PolynomZ2();
            c.p = a.p ^ b.p;
            return c;
        }
        public int Deg {
            get
            {
                if (p <= 1) return 0;
                int k = 31;
                while ((p >> k & 1) == 0)
                {
                    k--;
                }
                return k;
            }
        }
        public static PolynomZ2 operator /(PolynomZ2 a, PolynomZ2 b)
        {
            PolynomZ2 c = new PolynomZ2(a); // остаток
            PolynomZ2 d = new PolynomZ2(0); // частнок
            while (c.Deg >= b.Deg)
            {
                d.p |= 1 << (c.Deg - b.Deg);
                c.p ^= b.p << (c.Deg - b.Deg);
            }
            return d;
        }
        public static PolynomZ2 operator %(PolynomZ2 a, PolynomZ2 b)
        {
            PolynomZ2 c = new PolynomZ2(a); // остатое
            PolynomZ2 d = new PolynomZ2(0); // частнок
            while (c.CompareTo(new PolynomZ2(0))>0 && c.Deg >= b.Deg)
            {
                d.p |= 1 << (c.Deg - b.Deg);
                c.p ^= b.p << (c.Deg - b.Deg);
            }
            return c;
        }

        public int CompareTo(object x)
        {
            if (x is PolynomZ2) return (int)(p - (x as PolynomZ2).p);
            if (x is Int64) return (int)(p - (x as PolynomZ2).p);
            if (x is int) return (int)(p - (x as PolynomZ2).p);
            return 0;
        }
        public static PolynomZ2 GCD(PolynomZ2 a, PolynomZ2 b)
        {
            if (b.p == 0) return a;
            else return GCD(b, a % b);
        }
        public static PolynomZ2 GenerateIrreducablePolynomZ2(int m)
        {
            //random Polynom
            bool found = false;
            PolynomZ2 f;
            do
            {
                Random r = new Random();
                f = new PolynomZ2((1 + (1 << m)));
                for (int i = 1; i < m; i++)
                {
                    f.p |= r.Next(2) << i;
                }
                PolynomZ2 u = new PolynomZ2(2);
                PolynomZ2 d;
                bool Irreducable = true;
                for (int i=1; i<=m/2; i++)
                {
                    u = u * u % f;
                    d = GCD(f, u - new PolynomZ2(2));
                    if (d.CompareTo(new PolynomZ2(1)) != 0) Irreducable = false;
                }
                if (Irreducable) found = true;
            } while (!found);
            return f;
        }
    }
}
