using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace SourceGeneratorSamples
{
    [Generator]
    public class DeviceEnumGenerator : ISourceGenerator
    {
        public void Execute(SourceGeneratorContext context)
        {
            // begin creating the source we'll inject into the users compilation
            var sourceBuilder = new StringBuilder(@"
namespace PSPiTop.Generated
{
    public enum DigitalDevices
    {
");

            // var digitalTypeNames =
            //     typeof(DigitalPortDeviceBase).Assembly.GetTypes()
            //         .Where(type => type.IsSubclassOf(typeof(DigitalPortDeviceBase)))
            //         .Select(type => type.Name);
            INamedTypeSymbol symbol = context.Compilation.GetTypeByMetadataName("PiTopMakerArchitecture.Foundation.DigitalPortDeviceBase");
            // IEnumerable<SyntaxNode> allNodes = context.Compilation.SyntaxTrees.SelectMany(s => s.GetRoot().DescendantNodes());
            // IEnumerable<ClassDeclarationSyntax> allClassDeclarations = allNodes.Where(d => d.IsKind(SyntaxKind.ClassDeclaration)).OfType<ClassDeclarationSyntax>();
            // allClassDeclarations.Where(c => {
            //     if(c.BaseList == null)
            //     {
            //         return false;
            //     }

            //     c.BaseList?.Contains(symbol.);
            // });
            // sourceBuilder.AppendLine(symbol.Name);
           var a = context.Compilation.GlobalNamespace.GetTypeMembers().Where(n => n.CanBeReferencedByName);//.Where(t =>
                //t.BaseType != null && t.BaseType.Name == symbol.Name);

            var b = a.Select(x => x.Name).Distinct();// .Take(1);
            foreach(var t in b)
            {
                sourceBuilder.AppendLine(t + ",\n");
            }

            // var a = allClassDeclarations.Where(c => {
            //     if(c.BaseList == null)
            //     {
            //         return false;
            //     }

            //     return c.BaseList.Types.Any(t => t.ToFullString() == "DigitalPortDeviceBase");
            // });

            // using the context, get a list of syntax trees in the users compilation
            // IEnumerable<SyntaxTree> syntaxTrees = context.Compilation.SyntaxTrees.Where(tree => {
            //     tree.
            // });

            // // add the filepath of each tree to the class we're building
            // foreach (SyntaxTree tree in syntaxTrees)
            // {
            //     sourceBuilder.AppendLine($@"Console.WriteLine(@"" - {tree.FilePath}"");");
            // }

            // sourceBuilder.AppendLine(a.First().ToFullString());
            // sourceBuilder.AppendLine(string.Join(',', digitalTypeNames));
            // sourceBuilder.AppendLine(digitalTypeNames.First());

            // finish creating the source to inject
            sourceBuilder.Append(@"
    }
}");

            // inject the created source into the users compilation
            context.AddSource("DeviceEnumGenerator", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));

//             // begin creating the source we'll inject into the users compilation
//             sourceBuilder = new StringBuilder(@"
// namespace PSPiTop.Generated
// {
//     public enum AnalogDevices
//     {
// ");

//             IEnumerable<string> analogueTypeNames =
//                 typeof(AnaloguePortDeviceBase).Assembly.GetTypes()
//                     .Where(type => type.IsSubclassOf(typeof(DigitalPortDeviceBase)))
//                     .Select(type => type.Name);

//             sourceBuilder.AppendLine(string.Join(',', analogueTypeNames));

//             // finish creating the source to inject
//             sourceBuilder.Append(@"
//     }
// }");

//             // inject the created source into the users compilation
//             context.AddSource("DeviceEnumGenerator", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
        }

        public void Initialize(InitializationContext context)
        {
            // No initialization required for this one
            // var digitalTypeNames =
            //     typeof(DigitalPortDeviceBase).Assembly.GetTypes();
        }
    }
}
