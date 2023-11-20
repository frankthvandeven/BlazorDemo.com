using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Kenova.SourceGenerators
{

    internal class SyntaxReceiver : ISyntaxContextReceiver
    {

        public List<ViewModelToGenerate> ViewModelsToGenerate { get; } = new List<ViewModelToGenerate>();

        /// <summary>
        /// Called for every syntax node in the compilation, we can inspect the nodes and save any information useful for generation
        /// </summary>
        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            if (context.Node is not ClassDeclarationSyntax cds)
                return;

            if (cds.AttributeLists.Count == 0)
                return;

            if (cds.AttributeLists.ToString().Contains("ViewModel") == false)
                return;

            // At this point we have a candidate viewmodel. We look deeper into the class definition

            var classSymbol = context.SemanticModel.GetDeclaredSymbol(cds) as INamedTypeSymbol;

            if (classSymbol == null)
                return;

            //var attributeData = classSymbol.GetAttributes().SingleOrDefault(x => x.AttributeClass.ToDisplayString().Contains("ViewModel"));

            const string VM_ATTRIB = "Kenova.Client.Components.ViewModelAttribute";

            AttributeData attributeData = classSymbol.GetAttributes().SingleOrDefault(x => x.AttributeClass?.ToDisplayString() == VM_ATTRIB);

            if (attributeData == null)
                return;

            var vmtg = new ViewModelToGenerate();
            vmtg.ViewModelClassSymbol = classSymbol;

            findFields(classSymbol, vmtg);

            ViewModelsToGenerate.Add(vmtg);
        }

        private void findFields(INamedTypeSymbol classSymbol, ViewModelToGenerate vmtg)
        {
            var viewModelMembers = classSymbol.GetMembers();

            foreach (var memberSymbol in viewModelMembers)
            {
                if (memberSymbol is IFieldSymbol fieldSymbol && fieldSymbol.Name.StartsWith("__") && fieldSymbol.Name.Length > 2)
                {
                    preparePropertyData(fieldSymbol, vmtg);
                }
            }

        }

        private void preparePropertyData(IFieldSymbol fieldSymbol, ViewModelToGenerate vmtg)
        {
            var prtg = new PropertyToGenerate();

            prtg.FieldSymbol = fieldSymbol;

            vmtg.PropertiesToGenerate.Add(prtg);
        }

    }
}