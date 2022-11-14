using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace PowerSeries
{
    public class EvaluationContext
    {
        public ExtendedPolynomial x => ExtendedPolynomial.X;
    }
    public class EvaluationResult
    {
        public object Result { get; set; }
        public bool Success { get; set; }
        public long ElapsedTime { get; set; }
        public string Error { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }

    }
    public class EvaluationModule
    {
        private readonly string[] Namespaces
            = {
            "System",
            "System.Threading.Tasks",
            "System.Linq",
            "System.Collections.Generic",
            "System.Numerics",
            "PowerSeries",
            "PowerSeries.Series"
        };

        private readonly Assembly[] References
            = {
            typeof(string).GetTypeInfo().Assembly,
            typeof(Enumerable).GetTypeInfo().Assembly,
            typeof(Series).GetTypeInfo().Assembly
        };

        private readonly ScriptOptions _options;
        private readonly EvaluationContext _context;
        public EvaluationModule()
        {
            _options = ScriptOptions.Default.AddImports(Namespaces)
                .AddReferences(References);
            _context = new EvaluationContext();
        }

        public async Task<EvaluationResult> EvaluateAsync(string code)
        {
            bool success = false;
            object result;
            string error = null;
            var sw = Stopwatch.StartNew();
            try
            {
                result = await CSharpScript.EvaluateAsync(code, _options, _context);
                success = true;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                error = ex.GetType().FullName;
                if (ex is CompilationErrorException e)
                {
                    result = string.Join("\n", e.Diagnostics);
                    error = "Compilation Error";
                }
            }
            sw.Stop();
            return new EvaluationResult()
            {
                Result = result,
                ElapsedTime = sw.ElapsedMilliseconds,
                Success = success,
                Error = error
            };
        }

    }
}
