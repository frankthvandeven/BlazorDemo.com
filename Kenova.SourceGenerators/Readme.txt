
Resources
=========

Thomas Claudius Huber's MvvmGen on GitHub.

https://www.infoq.com/articles/CSharp-Source-Generator/ 
https://www.cazzulino.com/source-generators.html 
https://nicksnettravels.builttoroam.com/debug-code-gen/ 

Install "Syntax Viewer" in Visual Studio (Compilation SDK)

Debugging. Compile in a secondary project (separate VS instance), and the reload the main project my exiting and restarting
Visual Studio and then rebuild the project using the source generator. Rebuild will force the source generator to run.




TEST RUN THE GENERATOR FROM CONSOLE PROJECT.
The problems I had is that the symbols from Kenova.Client were not fully loaded.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Kenova.SourceGenerators;

namespace TestConsoleApp
{
    
    class Program
    {
        static void Main(string[] args)
        {
            string source = @"
using Kenova.Client;

namespace Foo
{
    [ultraModel]
    [ViewModel]
    class Customer
    {

        [AlsoNotify(nameof(FullDetails))]
        private string __Name;

        [AlsoNotify(nameof(FullDetails))]
        private string __Address;

        [AlsoNotify(""FullDetails"")]
        private string __Street;

        public string FullDetails
        {
            get
            {
                return __Name + __Address + __Street;
            }
        }


        private string __abc;


        void M()
        {
        }
    }
}";

            var (diagnostics, output) = GetGeneratedOutput(source);

            if (diagnostics.Length > 0)
            {
                Console.WriteLine("Diagnostics:");
                foreach (var diag in diagnostics)
                {
                    Console.WriteLine("   " + diag.ToString());
                }
                Console.WriteLine();
                Console.WriteLine("Output:");
            }

            Console.WriteLine(output);
        }

        private static (ImmutableArray<Diagnostic>, string) GetGeneratedOutput(string inputCode)
        {
            System.Reflection.Assembly.Load("Kenova.Client");

            var metadataReferences = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic)
                .Select(a => MetadataReference.CreateFromFile(a.Location))
                .ToArray();

            Compilation inputCompilation = CreateCompilation(inputCode, metadataReferences);

            Generator generator = new Generator();

            GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

            driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out var diagnostics);

            var runResult = driver.GetRunResult();

            //Assert.Equal(expectedGeneratedCode.Length, runResult.GeneratedTrees.Length);
            //Assert.True(runResult.Diagnostics.IsEmpty);

            var generatorResult = runResult.Results[0];

            return (diagnostics, outputCompilation.SyntaxTrees.Last().ToString());
        }

        protected static Compilation CreateCompilation(string source, MetadataReference[] metadataReferences)
        => CSharpCompilation.Create("compilation",
            new[] { CSharpSyntaxTree.ParseText(source) },
            metadataReferences,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

    }
}
