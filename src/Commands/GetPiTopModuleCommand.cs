using System.Management.Automation;
using PiTop;

namespace PSPiTop
{
    [Cmdlet(VerbsCommon.Get,"PiTopModule")]
    [OutputType(typeof(PiTopModule))]
    public class GetPiTopModuleCommand : PSCmdlet
    {
        [Parameter]
        public SwitchParameter NewInstance {get; set;}
        protected override void EndProcessing()
        {
            if (PiTopModuleState.PiTopModule == null || NewInstance.IsPresent)
            {
                PiTopModuleState.PiTopModule = new PiTopModule();
            }
            WriteObject(PiTopModuleState.PiTopModule);
        }
    }
}
