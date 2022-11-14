using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Threading.Tasks;

namespace PowerSeries
{
    public class Series
    {
        public static ExtendedPolynomial Exp(ExtendedPolynomial p)
        {
            var a = p[0];
            var q = p - a;
            var ea = Math.Exp(a);

            return new ExtendedPolynomial(n =>
            {
                long fact = 1;
                double sum = 0;
                sum += (q ^ 0)[n];
                for (int i = 1; i <= n; i++)
                {
                    fact *= i;
                    sum += (q ^ i)[n] / fact;
                }
                return sum * ea;
            });
        }

        public static ComplexPolynomial ComplexExp(ComplexPolynomial p)
        {
            var a = p[0];
            var q = p - a;
            var ea = Complex.Exp(a);

            return new ComplexPolynomial(n =>
            {
                long fact = 1;
                Complex sum = 0;
                sum += (q ^ 0)[n];
                for (int i = 1; i <= n; i++)
                {
                    fact *= i;
                    sum += (q ^ i)[n] / fact;
                }
                return sum * ea;
            });
        }

        public static ExtendedPolynomial Log(ExtendedPolynomial p)
        {
            var a = p[0];
            var q = p / a - 1;
            var log_A = Math.Log(a);

            return log_A + new ExtendedPolynomial(n =>
            {
                double sum = 0;
                int c = 1;
                for (int i = 1; i <= n; i++)
                {
                    sum += c * ((q ^ i)[n] / i);
                    c = -c;
                }
                return sum;
            });
        }
        public static ExtendedPolynomial Ln(ExtendedPolynomial p)
            => (p.Derivative() / p).AntiDerivative(Math.Log(p[0]));
        // e^(ix) = cos(x) + i*sin(x)
        public static ExtendedPolynomial Sin(ExtendedPolynomial p)
            => ComplexExp(Complex.ImaginaryOne * (ComplexPolynomial)p).Imaginary;

        public static ExtendedPolynomial Cos(ExtendedPolynomial p)
            => ComplexExp(Complex.ImaginaryOne * (ComplexPolynomial)p).Real;

        public static ExtendedPolynomial Arctan(ExtendedPolynomial p)
            => (p.Derivative() / (1 + p.Pow(2))).AntiDerivative(Math.Atan(p[0]));
        public static ExtendedPolynomial Arcsin(ExtendedPolynomial p)
            => (p.Derivative() / (1 - p * p).Pow(0.5)).AntiDerivative(Math.Asin(p[0]));
        public static ExtendedPolynomial Arccos(ExtendedPolynomial p)
            => (-p.Derivative() / (1 - p * p).Pow(0.5)).AntiDerivative(Math.Acos(p[0]));
    }
}
