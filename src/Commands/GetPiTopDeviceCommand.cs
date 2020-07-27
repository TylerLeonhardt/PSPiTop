using System.Management.Automation;
using PiTop;
using PiTopMakerArchitecture.Foundation;
using PiTopMakerArchitecture.Foundation.Components;

namespace PSPiTop
{
    [Cmdlet(VerbsCommon.Get,"PiTopDevice")]
    [OutputType(typeof(IConnectedDevice))]
    public class GetPiTopDeviceCommand : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 1)]
        public DigitalPort DigitalPort {get; set;}

        [Parameter(Position = 1)]
        public FoundationPlate PiTopPlate {get; set;} = PiTopModuleState.PiTopPlate;

        protected override void EndProcessing()
        {
            if (PiTopModuleState.PiTopPlate == null)
            {
                ThrowTerminatingError(new ErrorRecord(
                    new PSInvalidOperationException("PiTop Plate not initialized. Run Get-PiTopPlate first."),
                    null,
                    ErrorCategory.InvalidData,
                    null));
                return;
            }

            var device = PiTopPlate.GetOrCreateDevice<Led>(DigitalPort);
            WriteObject(device);
        }
    }
}
