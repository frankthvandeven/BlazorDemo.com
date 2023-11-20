using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Kenova.SourceGenerators
{

    internal class ViewModelToGenerate
    {

        public INamedTypeSymbol ViewModelClassSymbol { get; set; }

        public List<PropertyToGenerate> PropertiesToGenerate { get; } = new List<PropertyToGenerate>();

    }
}
