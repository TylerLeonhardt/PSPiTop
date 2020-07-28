using System;
using System.IO;
using System.Management.Automation;
using System.Reflection;
using PiTopMakerArchitecture.Foundation;

namespace PSPiTop
{
    [Cmdlet(VerbsLifecycle.Enable,"PiTopDotnetInteractiveFormatters")]
    [OutputType(typeof(FoundationPlate))]
    public class EnablePiTopDotnetInteractiveFormatters : PSCmdlet
    {
        private static string s_assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static string s_extensionAsmPath = Path.Combine(s_assemblyFolder,"PiTopMakerArchitecture.Foundation.InteractiveExtension.dll");
        private static Lazy<Assembly> s_extensionAsm = new Lazy<Assembly>(() => Assembly.LoadFile(s_assemblyFolder));

        protected override void EndProcessing()
        {
            var type = s_extensionAsm.Value.GetType("PiTopMakerArchitecture.Foundation.InteractiveExtension.KernelExtension");
            var instance = type.GetConstructor(Array.Empty<Type>()).Invoke(null);
            type.GetMethod("OnLoadAsync").Invoke(instance, new [] { instance });
        }
    }
}
