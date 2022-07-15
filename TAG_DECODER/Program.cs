using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TAG_DECODER.Devices.Common.Helpers;
using VIPA_PARSER.Config.Extensions;
using VIPA_PARSER.Devices.Common;
using VIPA_PARSER.Devices.Common.Helpers;
using static VIPA_PARSER.Devices.Common.Types;

namespace VIPA_PARSER
{
    class Program
    {
        static DeviceLogHandler deviceLogHandler = DeviceLogger;

        static void Main(string[] args)
        {
            Console.WriteLine($"\r\n==========================================================================================");
            Console.WriteLine($"{Assembly.GetEntryAssembly().GetName().Name} - Version {Assembly.GetEntryAssembly().GetName().Version}");
            Console.WriteLine($"==========================================================================================\r\n");

            // TEST ONLY
            //LoadDebuggerAutomation();

            //int value = 0x01;
            //bool ischained = (value & 0x01) == 0x01;
            //value = 0x40;
            //bool isNotchained = (value & 0x01) == 0x00;
            //value = 0x41;
            //bool isNotKnown = (value & 0x00) == 0x00;

            ConfigurationLoad(0);
        }

        static void ConfigurationLoad(int index)
        {
            // Get appsettings.json config.
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            // VIPA DATA GROUP
            VIPADataGroup(configuration, index);
        }

        static void VIPADataGroup(IConfiguration configuration, int index)
        {
            Dictionary<string, string> vipaPayload = configuration.GetSection("CapturedTagData")
                .GetChildren()
                .ToDictionary(x => x.Key, x => x.Value);

            // Is there a matching item?
            if (vipaPayload.Count() > index)
            {
                foreach (var tlv in vipaPayload)
                {
                    if (Int32.TryParse(tlv.Key, out int numKey))
                    {
                        byte[] value = ConversionHelper.AsciiToByte(tlv.Value);
                        Console.WriteLine(string.Format("{0:X6}: [{1}]", numKey, ConversionHelper.ByteArrayToHexString(value)));
                    }
                }
            }
        }

        public static void DeviceLogger(LogLevel logLevel, string message)
        {
            Console.WriteLine($"[{logLevel}]: {message}");
        }
    }
}
