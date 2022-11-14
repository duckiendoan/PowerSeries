using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Threading.Tasks;

namespace PowerSeries
{
    public class ComplexPolynomial
    {
        public static readonly ComplexPolynomial X = new ComplexPolynomial(i => (i == 1) ? 1 : 0);

        private readonly Dictionary<int, Complex> memo = new Dictionary<int, Complex>();

        private readonly Dictionary<int, ComplexPolynomial> PowMemo
            = new Dictionary<int, ComplexPolynomial>();

        private readonly Func<int, Complex> _map;

        public ExtendedPolynomial Real { get; }
        public ExtendedPolynomial Imaginary { get; set; }

        public ComplexPolynomial(Func<int, Complex> map)
        {
            _map = map;
            Real = new ExtendedPolynomial(n => this[n].Real);
            Imaginary = new ExtendedPolynomial(n => this[n].Imaginary);
        }

        public Complex this[int index]
        {
            get
            {
                if (!memo.ContainsKey(index))
                    memo[index] = _map(index);
                return memo[index];
            }
        }

        public Complex Evaluate(Complex x, int deg = 10)
        {
            Complex res = 0;
            Complex t = 1;
            for (int i = 0; i <= deg; i++)
            {
                res += this[i] * t;
                t *= x;
            }
            return res;
        }

        public ComplexPolynomial Pow(int n)
        {
            if (n == 0) return new Complex(1, 0);
            if (PowMemo.ContainsKey(n)) return PowMemo[n];
            var p1 = Pow(n / 2);
            if (n % 2 == 0) return PowMemo[n] = p1 * p1;
            return PowMemo[n] = p1 * p1 * this;
        }

        public ComplexPolynomial Pow(Complex r)
        {
            var a = this[0];
            var q = (this / a) - new Complex(1, 0);
            // p(x)^r = a^r(1+q(x))^r
            var ar = Complex.Pow(a, r);

            return new ComplexPolynomial(n =>
            {
                Complex sum = 0;
                Complex c = 1;
                sum += c * (q ^ 0)[n];
                for (int i = 1; i <= n; i++)
                {
                    c *= (r - i + 1) / i;
                    sum += c * (q ^ i)[n];
                }
                return sum * ar;
            });
        }

        public ComplexPolynomial Inverse()
        {
            var a = this[0];
            var q = (Complex)1 - (this / a);
            return new ComplexPolynomial(n =>
            {
                Complex sum = 0;
                for (int i = 0; i <= n; i++)
                    sum += (q ^ i)[n];
                return sum / a;
            });
        }

        public static ComplexPolynomial operator +(ComplexPolynomial p1, ComplexPolynomial p2)
            => new ComplexPolynomial(i => p1[i] + p2[i]);
        public static ComplexPolynomial operator -(ComplexPolynomial p1, ComplexPolynomial p2)
            => new ComplexPolynomial(i => p1[i] - p2[i]);
        public static ComplexPolynomial operator *(ComplexPolynomial p1, ComplexPolynomial p2)
            => new ComplexPolynomial(i =>
            {
                Complex s = 0;
                for (int j = 0; j <= i; j++)
                    s += p1[j] * p2[i - j];
                return s;
            });
        public static ComplexPolynomial operator ^(ComplexPolynomial p1, int n)
            => p1.Pow(n);
        public static ComplexPolynomial operator /(ComplexPolynomial p1, ComplexPolynomial p2)
            => p1 * p2.Inverse();
        public static ComplexPolynomial operator -(ComplexPolynomial p1)
            => new ComplexPolynomial(i => -p1[i]);
        public static ComplexPolynomial operator *(ComplexPolynomial p1, Complex k)
            => new ComplexPolynomial(i => p1[i] * k);
        public static ComplexPolynomial operator /(ComplexPolynomial p1, Complex k)
            => new ComplexPolynomial(i => p1[i] / k);

        public static implicit operator ComplexPolynomial(Complex a)
            => new ComplexPolynomial(i => (i == 0) ? a : 0);

        public static implicit operator ComplexPolynomial(ExtendedPolynomial p)
            => new ComplexPolynomial(i => p[i]);

        public override string ToString()
        {
            return "{ " + string.Join(", ", Enumerable.Range(0, 10).Select(x => this[x])) + ", ... }";
        }

        public string ToPolyString(int deg = 10)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i <= deg; i++)
            {
                var e = this[i];
                if (e == 0) continue;
                if (i == 0) builder.Append(e.ToString("0.#####"));
                else
                {
                    builder.Append(FormatCoefficient(e));
                    if (i > 1) builder.Append($"x^{i}");
                    else if (i == 1) builder.Append("x");
                }
                builder.Append(' ');
            }
            builder.Append("+ ...");
            return builder.ToString();
        }

        private string FormatCoefficient(Complex effi)
        {
            return effi.ToString();
        }
    }
}
