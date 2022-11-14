using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerSeries
{
    public class ExtendedPolynomial
    {
        public static readonly ExtendedPolynomial X = new ExtendedPolynomial(i => (i == 1) ? 1 : 0);

        private readonly Dictionary<int, double> memo = new Dictionary<int, double>();

        private readonly Dictionary<int, ExtendedPolynomial> PowMemo
            = new Dictionary<int, ExtendedPolynomial>();

        private readonly Func<int, double> _map;
        public ExtendedPolynomial(Func<int, double> map) => _map = map;
        public double this[int index]
        {
            get
            {
                if (!memo.ContainsKey(index))
                    memo[index] = _map(index);
                return memo[index];
            }
        }

        public double Evaluate(double x, int deg = 10)
        {
            double res = 0;
            double t = 1;
            for (int i = 0; i <= deg; i++)
            {
                res += this[i] * t;
                t *= x;
            }
            return res;
        }

        public ExtendedPolynomial Pow(int n)
        {
            if (n == 0) return 1;
            if (PowMemo.ContainsKey(n)) return PowMemo[n];
            var p1 = Pow(n / 2);
            if (n % 2 == 0) return PowMemo[n] = p1 * p1;
            return PowMemo[n] = p1 * p1 * this;
        }
        public ExtendedPolynomial Pow(double r)
        {
            var a = this[0];
            var q = (this / a) - 1;
            // p(x)^r = a^r(1+q(x))^r
            var ar = Math.Pow(a, r);

            return new ExtendedPolynomial(n =>
            {
                double sum = 0;
                double c = 1;
                sum += c * (q ^ 0)[n];
                for (int i = 1; i <= n; i++)
                {
                    c *= (r - i + 1) / i;
                    sum += c * (q ^ i)[n];
                }
                return sum * ar;
            });
        }
        public ExtendedPolynomial Inverse()
        {
            var a = this[0];
            var q = 1 - (this / a);
            return new ExtendedPolynomial(n =>
            {
                double sum = 0;
                for (int i = 0; i <= n; i++)
                    sum += (q ^ i)[n];
                return sum / a;
            });
        }
        public ExtendedPolynomial Derivative()
        {
            return new ExtendedPolynomial(k => (k + 1) * this[k + 1]);
        }
        public ExtendedPolynomial AntiDerivative(double constant = 0)
        {
            return new ExtendedPolynomial(k =>
            {
                if (k == 0) return constant;
                return this[k - 1] / k;
            });
        }
        public double Integrate(double a, double b, int deg = 10)
        {
            double sum = 0;
            double b_n = b;
            double a_n = a;

            for (int i = 0; i <= deg; i++)
            {
                sum += (this[i] / (i + 1)) * (b_n - a_n);
                b_n *= b;
                a_n *= a;
            }
            return sum;
        }

        public double Integrate(double a, double b, double epsilon)
        {
            double sum = 0;
            double b_n = b;
            double a_n = a;
            for (int i = 0; ; i++)
            {
                double u = (this[i] / (i + 1)) * (b_n - a_n);
                sum += u;
                if (u != 0 && Math.Abs(u) <= epsilon) break;
                b_n *= b;
                a_n *= a;
            }
            return sum;
        }

        public static ExtendedPolynomial operator +(ExtendedPolynomial p1, ExtendedPolynomial p2)
            => new ExtendedPolynomial(i => p1[i] + p2[i]);
        public static ExtendedPolynomial operator -(ExtendedPolynomial p1, ExtendedPolynomial p2)
            => new ExtendedPolynomial(i => p1[i] - p2[i]);
        public static ExtendedPolynomial operator *(ExtendedPolynomial p1, ExtendedPolynomial p2)
            => new ExtendedPolynomial(i =>
            {
                double s = 0;
                for (int j = 0; j <= i; j++)
                    s += p1[j] * p2[i - j];
                return s;
            });
        public static ExtendedPolynomial operator ^(ExtendedPolynomial p1, int n)
            => p1.Pow(n);
        public static ExtendedPolynomial operator /(ExtendedPolynomial p1, ExtendedPolynomial p2)
            => p1 * p2.Inverse();
        public static ExtendedPolynomial operator -(ExtendedPolynomial p1)
            => new ExtendedPolynomial(i => 0 - p1[i]);
        public static ExtendedPolynomial operator *(ExtendedPolynomial p1, double k)
            => new ExtendedPolynomial(i => p1[i] * k);
        public static ExtendedPolynomial operator /(ExtendedPolynomial p1, double k)
            => new ExtendedPolynomial(i => p1[i] / k);

        public static implicit operator ExtendedPolynomial(double a)
            => new ExtendedPolynomial(i => (i == 0) ? a : 0);


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

        private string FormatCoefficient(double effi)
        {
            if (effi == 1) return "+";
            if (effi == -1) return "-";
            if (effi > 0) return "+" + effi.ToString("0.#####");
            return effi.ToString("0.#####");
        }
    }
}
