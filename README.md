# PowerSeries

A REPL environment for evaluating Maclaurin series of functions, based on

- [Infinite lazy polynomials (jliszka.org)](http://blog.jliszka.org/2013/10/31/infinite-lazy-polynomials.html)

# Usage

Run the program and type an expression to evaluate its Maclaurin series. The variable `x` denotes the polynomial `x`. The output will the coefficients.

```csharp
// Examples
> x ^ 2
{ 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, ... }

// Print the first 30 coefficient
> Sin(x).ToString(30)
{ 0, 1, 0, -0.16666666666666666, 0, 0.008333333333333333, 0, -0.0001984126984126984, 0, 2.7557319223985893E-06, 0, -2.505210838544172E-08, 0, 1.6059043836821613E-10, 0, -7.647163731819816E-13, 0, 2.8114572543455206E-15, 0, -8.22063524662433E-18, 0, -2.353334294364486E-19, 0, -1.2302708207447328E-19, 0, 1.4215580141988783E-19, 0, 1.8236039240670545E-19, 0, -1.4172418368442757E-19, ... }
```