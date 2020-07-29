using System;
using System.Management.Automation;
using System.Reflection;
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
        private static readonly MethodInfo _getOrCreateDeviceInfo = typeof(FoundationPlate).GetMethod("GetOrCreateDevice");

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
                    string digitalName = Enum.GetName(typeof(DigitalDevices), DigitalDevice);
                    device = _getOrCreateDeviceInfo
                        .MakeGenericMethod(typeof(DigitalPortDeviceBase).Assembly
                        .GetType(digitalName)).Invoke(PiTopPlate, new object[] { DigitalPort }) as IConnectedDevice;
                    break;
                case "AnalogPort":
                    string analogueName = Enum.GetName(typeof(AnalogueDevices), AnalogueDevice);
                    device = _getOrCreateDeviceInfo
                        .MakeGenericMethod(typeof(AnaloguePortDeviceBase).Assembly
                        .GetType(analogueName)).Invoke(PiTopPlate, new object[] { AnaloguePort }) as IConnectedDevice;
                    break;
            }

            if(device != null)
            {
                WriteObject(device);
            }
        }
    }
}
