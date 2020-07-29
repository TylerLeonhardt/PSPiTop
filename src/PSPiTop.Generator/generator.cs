using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace SourceGeneratorSamples
{
    [Generator]
    public class DeviceEnumGenerator : ISourceGenerator
    {
        public void Execute(SourceGeneratorContext context)
        {
            var allTypesThatHaveBaseTypes = GetAllNamedTypes(context.Compilation.GlobalNamespace).Where(
                t => t.CanBeReferencedByName &&
                t.BaseType != null);

            // begin creating the source we'll inject into the users compilation
            var digitalDevicesSourceBuilder = new StringBuilder(@"
namespace PSPiTop.Generated
{
    public enum DigitalDevices
    {
");

            INamedTypeSymbol digitalPortDeviceBasesSymbol = context.Compilation.GetTypeByMetadataName("PiTopMakerArchitecture.Foundation.DigitalPortDeviceBase");
            var digitalPortDeviceBaseExtendedTypes = allTypesThatHaveBaseTypes.Where(t => t.BaseType.Name == digitalPortDeviceBasesSymbol.Name);

            foreach(var t in digitalPortDeviceBaseExtendedTypes.Select(x => x.Name).Distinct())
            {
                digitalDevicesSourceBuilder.AppendLine($"{t},\n");
            }
            digitalDevicesSourceBuilder.Append(@"
    }
}");

            // inject the created source into the users compilation
            context.AddSource("DigitalDevicesEnumGenerator", SourceText.From(digitalDevicesSourceBuilder.ToString(), Encoding.UTF8));

            // Now add Analogue Enum
            var analogueDevicesSourceBuilder = new StringBuilder(@"
namespace PSPiTop.Generated
{
    public enum AnalogueDevices
    {
");

            INamedTypeSymbol analoguePortDeviceBasesSymbol = context.Compilation.GetTypeByMetadataName("PiTopMakerArchitecture.Foundation.AnaloguePortDeviceBase");
            var analoguePortDeviceBaseExtendedTypes = allTypesThatHaveBaseTypes.Where(t => t.BaseType.Name == analoguePortDeviceBasesSymbol.Name);

            foreach(var t in analoguePortDeviceBaseExtendedTypes.Select(x => x.Name).Distinct())
            {
                analogueDevicesSourceBuilder.AppendLine($"{t},\n");
            }

            analogueDevicesSourceBuilder.Append(@"
    }
}");

            // inject the created source into the users compilation
            context.AddSource("AnalogueDevicesEnumGenerator", SourceText.From(analogueDevicesSourceBuilder.ToString(), Encoding.UTF8));
        }

        public void Initialize(InitializationContext context)
        {
            // No initialization required for this one
        }

        public static IEnumerable<INamedTypeSymbol> GetAllNamedTypes(INamespaceSymbol @namespace)
        {
            if (@namespace == null)
            {
                yield break;
            }

            foreach (var typeMember in @namespace.GetTypeMembers().SelectMany(t => GetAllNamedTypes(t)))
            {
                yield return typeMember;
            }

            foreach (var typeMember in @namespace.GetNamespaceMembers().SelectMany(t => GetAllNamedTypes(t)))
            {
                yield return typeMember;
            }
        }

        public static IEnumerable<INamedTypeSymbol> GetAllNamedTypes(INamedTypeSymbol type)
        {
            if (type == null)
            {
                yield break;
            }

            yield return type;

            foreach (var nestedType in type.GetTypeMembers().SelectMany(t => GetAllNamedTypes(t)))
            {
                yield return nestedType;
            }
        }
    }
}
