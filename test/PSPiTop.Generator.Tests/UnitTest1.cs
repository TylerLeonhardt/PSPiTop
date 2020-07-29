using System;
using PSPiTop.Generated;
using Xunit;
using Xunit.Abstractions;

using PiTopMakerArchitecture.Foundation;

namespace PSPiTop.Generator.Tests
{
    public class UnitTest1
    {
        private readonly ITestOutputHelper _output;
        public UnitTest1(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test1()
        {
            Console.WriteLine(DigitalPort.D0);
            HelloWorldGenerated.HelloWorld.SayHello();
            foreach(string i in Enum.GetNames(typeof(DigitalDevices)))
                Console.WriteLine(i);
            foreach(string i in Enum.GetNames(typeof(AnalogueDevices)))
                Console.WriteLine(i);
        }
    }
}
