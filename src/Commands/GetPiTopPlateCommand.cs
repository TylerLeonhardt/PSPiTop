using System.Management.Automation;
using PiTop;
using PiTopMakerArchitecture.Foundation;

namespace PSPiTop
{
    [Cmdlet(VerbsCommon.Get,"PiTopPlate")]
    [OutputType(typeof(FoundationPlate))]
    public class GetPiTopPlateCommand : PSCmdlet
    {
        [Parameter]
        public SwitchParameter NewInstance {get; set;}

        [Parameter(Position = 0)]
        public PiTopModule PiTopModule {get; set;} = PiTopModuleState.PiTopModule;

        protected override void EndProcessing()
        {
            if (PiTopModule == null)
            {
                ThrowTerminatingError(new ErrorRecord(
                    new PSInvalidOperationException("PiTop Module not initialized. Run Get-PiTopModule first."),
                    null,
                    ErrorCategory.InvalidData,
                    null));
                return;
            }

            if (PiTopModuleState.PiTopPlate == null || NewInstance.IsPresent)
            {
                PiTopModuleState.PiTopPlate = PiTopModule.GetOrCreatePlate<FoundationPlate>();
            }
            WriteObject(PiTopModuleState.PiTopPlate);
        }
    }
}
