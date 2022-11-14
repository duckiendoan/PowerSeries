using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerSeries
{
    struct ComplexTest
    {
        public static readonly ComplexTest I = new(0, 1);
        public double Real { get; set; }
        public double Imaginary { get; set; }

        public ComplexTest(double real = 0, double imaginary = 0)
        {
            Real = real;
            Imaginary = imaginary;
        }

        public static bool operator ==(ComplexTest z1, ComplexTest z2)
            => (z1.Real == z2.Real) && (z1.Imaginary == z2.Imaginary);
        public static bool operator !=(ComplexTest z1, ComplexTest z2)
            => !(z1 == z2);



        public override string ToString()
        {
            if (Real == 0 && Imaginary == 0) return "0";
            if (Imaginary == 0) return Real.ToString();
            string res = "";
            if (Real != 0) res += Real;
            if (Imaginary == 1) res += "+i";
            else if (Imaginary == -1) res += "-i";
            else if (Imaginary > 0) res += $"+{Imaginary}i";
            else res += $"{Imaginary}i";
            return res;
        }

        public override bool Equals(object obj)
        {
            ComplexTest? z = obj as ComplexTest?;
            if (z == null) return false;
            return z.Value == this;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = hash * 7 + Real.GetHashCode();
            hash = hash * 7 + Imaginary.GetHashCode();
            return hash;
        }
    }
}
