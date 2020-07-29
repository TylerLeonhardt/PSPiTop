using System.Management.Automation;
using PiTop;
using PiTopMakerArchitecture.Foundation;
using PiTopMakerArchitecture.Foundation.Components;
using PSPiTop.Generated;

namespace PSPiTop
{
    [Cmdlet(VerbsCommon.Get,"PiTopDevice")]
    [OutputType(typeof(IConnectedDevice))]
    public class GetPiTopDeviceCommand : PSCmdlet
    {
        [Parameter(
            Mandatory = true,
            Position = 0,
            ParameterSetName = "DigitalPort",
            ValueFromPipeline = true)]
        public DigitalPort DigitalPort {get; set;}

        [Parameter(
            Mandatory = true,
            Position = 0,
            ParameterSetName = "AnaloguePort",
            ValueFromPipeline = true)]
        public AnaloguePort AnaloguePort {get; set;}

        [Parameter(
            Mandatory = true,
            Position = 1,
            ParameterSetName = "DigitalPort",
            ValueFromPipeline = true)]
        public DigitalDevices DigitalDevice {get; set;}

        [Parameter(
            Mandatory = true,
            Position = 1,
            ParameterSetName = "AnaloguePort",
            ValueFromPipeline = true)]
        public AnalogueDevices AnalogueDevice {get; set;}

        // [Parameter(
        //     Mandatory = true,
        //     Position = 0,
        //     ParameterSetName = "AnaloguePort",
        //     ValueFromPipeline = true)]
        // public AnalogDevices AnalogueDevices {get; set;}

        [Parameter(Position = 2)]
        public DisplayPropertyBase[] DisplayProperties {get; set;}

        [Parameter(Position = 3)]
        public FoundationPlate PiTopPlate {get; set;} = PiTopModuleState.PiTopPlate;

        protected override void ProcessRecord()
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

            IConnectedDevice device = null;
            switch (ParameterSetName)
            {
                case "DigitalPort":
                    device = PiTopPlate.GetOrCreateDevice<DigitalPortDeviceBase>(DigitalPort);
                    break;
                case "AnalogPort":
                    device = PiTopPlate.GetOrCreateDevice<AnaloguePortDeviceBase>(AnaloguePort);
                    break;
            }

            if(device != null)
            {
                WriteObject(device);
            }
        }
    }
}
