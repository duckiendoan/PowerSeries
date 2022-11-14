using System;
using System.Numerics;
using System.Threading.Tasks;
using static PowerSeries.Series;

namespace PowerSeries
{
    class Program
    {
        static void Main(string[] args)
        {
            BeginEval().GetAwaiter().GetResult();
            /*
            var sw = System.Diagnostics.Stopwatch.StartNew();
            var x = ExtendedPolynomial.X;
            var i = Complex.ImaginaryOne;

            var cosx = new ExtendedPolynomial(n =>
            {
                if (n % 2 == 1) return 0;
                double res = 1.0 / Factorial(n);
                if ((n / 2) % 2 == 1) res = -res;
                return res;
            });

            var arctanx = new ExtendedPolynomial(n =>
            {
                if (n % 2 == 0) return 0;
                double res = 1.0 / n;
                if ((n - 1) / 2 % 2 == 1) res = -res;
                return res;
            });
            //var s = new ExtendedPolynomial(i => 1);
            double tmp = Math.Sqrt(3) / 3;
            var poly = ((4 + x)).Pow(0.5);
            var p2 = (1 + x).Pow(10);
            var p3 = (1 - 2 * x + 2 * x * x) / ((1 - x) * (1 - x) * (1 - 2 * x));

            Console.WriteLine(x.Derivative());
            Console.WriteLine(Arccos(x*x).ToPolyString());
            Console.WriteLine(Log(poly));

            Console.WriteLine(Sin(x));
            Console.WriteLine(Log(1 + x * x).Integrate(0, 0.9, 20));
            Console.WriteLine("(1+x)^10 = " + p2.ToPolyString(10));
            Console.WriteLine("sqrt(4+x) = " + poly.ToPolyString());
            Console.WriteLine("cos(x) = " + cosx.ToPolyString());

            var cp = new ComplexPolynomial(n => 1 + i);
            Console.WriteLine(cp.Pow(2));
            Console.WriteLine(cp.Real);
            Console.WriteLine((4 / (x*x + 1)).Integrate(0, 1, 80));
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds + "ms");
            */
        }

        static async Task BeginEval()
        {
            var service = new EvaluationModule();
            Console.Write("Setting up ...");
            await service.EvaluateAsync("x");
            Print("OK", ConsoleColor.Green);
            while(true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("> ");
                Console.ResetColor();

                string code = Console.ReadLine();
                var res = await service.EvaluateAsync(code);
                if (res.Success)
                    Print(res.Result, ConsoleColor.Green);
                else
                    Print(res.Result, ConsoleColor.Red);
                
            }
        }
        static void Print(object obj, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(obj);
            Console.ResetColor();
        }
        static Func<int, double> Derivative(Func<ExtendedPolynomial, ExtendedPolynomial> f, double c)
        {
            var fc = f(ExtendedPolynomial.X + c);
            return (i => fc[i] * Factorial(i));
        }
        static double Sqrt(double c, int n = 10)
            => (ExtendedPolynomial.X + c).Pow(0.5).Evaluate(0, n);

        static long Factorial(int n)
        {
            long res = 1;
            for (int i = 1; i <= n; i++)
                res *= i;
            return res;
        }
    }
}
