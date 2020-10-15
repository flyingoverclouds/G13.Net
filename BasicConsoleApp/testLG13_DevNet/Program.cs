using Device.Net;
using Hid.Net.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using G13GamingKeyboard;
using Hid.Net;

namespace testLG13_DevNet
{
    class Program
    {

        // Logitech G13 on my dev system :  \\?\hid#vid_046d&pid_c21c#7&372657bd&0&0000#{4d1e55b2-f16f-11cf-88cb-001111000030}

        /// <summary>
        /// Logitech USB Vendor ID "vid_046d"
        /// </summary>
        const uint G13UsbVendorID = 0x046d; 

        /// <summary>
        /// Logitech G13 USB Gaming keyboard product ID : "pid_c21c"
        /// </summary>
        const uint G13UsbProductID = 0xc21c; 

        static void Main(string[] args)
        {
            Console.WriteLine("** LG13 test with Device.net libraries");

            MainAsync(args).GetAwaiter().GetResult();

            Console.WriteLine("Press enter to exit ....");
            Console.ReadLine();
        }

        static async Task MainAsync(string[] args)
        {
            WindowsHidDeviceFactory.Register(new DebugLogger() { LogToConsole = true } , new DebugTracer() ) ;

            await ListDevices();

            await ConnectToG13();
                

        }

        static async Task ConnectToG13()
        {
            Console.WriteLine("*** Connect to G13 keyboard");
            var _DeviceDefinitions = new List<FilterDeviceDefinition> { new FilterDeviceDefinition { DeviceType = DeviceType.Hid, VendorId = G13UsbVendorID, ProductId = G13UsbProductID}, };


            var devices = await DeviceManager.Current.GetDevicesAsync(_DeviceDefinitions);
            IHidDevice hidG13 = devices.FirstOrDefault() as IHidDevice;

            await hidG13.InitializeAsync();

            

            Console.WriteLine("G13 should be initialized.");

            var ledModUsbData = new byte[] { 5, 0, 0, 0, 0 , 0, 0, 0};
            for (int n = 0; n < 20; n++) // wait 20 read from G13
            {
                var rr = await hidG13.ReadAsync();
                //rr.Data.RawWriteLine();
                
                switch (rr.Data[3])
                {
                    case 1: // G1 Key pressed
                        Console.WriteLine("[ G1 ]");


                        ledModUsbData[1] = 1 + 2 + 4 + 8; // ALL M* keys light on
                        await hidG13.WriteAsync(ledModUsbData);
                        break;
                    case 2: // G2 Key pressed
                        Console.WriteLine("[ G2 ]");

                        ledModUsbData[1] = 0; // ALL M* keys light off
                        await hidG13.WriteAsync(ledModUsbData);
                        break;
                    case 4: // G3 Key pressed
                        Console.WriteLine("[ G3 ]");
                        var d = new byte[] { 255,255,255,255,255,255,255,255};
                        await hidG13.WriteAsync(ledModUsbData);
                        break;
                }

            }

            Console.WriteLine("Press enter to exit test sequence");
            Console.ReadLine();
            hidG13.Dispose();
            Console.ReadLine();
        }

        


        /*
       
        
void G13_Device::set_mode_leds(int leds) {

	unsigned char usb_data[] = { 5, 0, 0, 0, 0 };
	usb_data[1] = leds;
	int r = libusb_control_transfer(handle,
			LIBUSB_REQUEST_TYPE_CLASS | LIBUSB_RECIPIENT_INTERFACE, 9, 0x305, 0,
			usb_data, 5, 1000);
	if (r != 5) {
		G13_LOG( error, "Problem sending data" );
		return;
	}
}
void G13_Device::set_key_color(int red, int green, int blue) {
	int error;
	unsigned char usb_data[] = { 5, 0, 0, 0, 0 };
	usb_data[1] = red;
	usb_data[2] = green;
	usb_data[3] = blue;

	error = libusb_control_transfer(handle,
			LIBUSB_REQUEST_TYPE_CLASS | LIBUSB_RECIPIENT_INTERFACE, 9, 0x307, 0,
			usb_data, 5, 1000);
	if (error != 5) {
		G13_LOG( error, "Problem sending data" );
		return;
	}
}

         * 
         */

        static async Task ListDevices()
        {
            Console.WriteLine("*** HID devices found on system (green is Logitech G13)");
            var dl = await DeviceManager.Current.GetConnectedDeviceDefinitionsAsync(null);
            var defaultColor = Console.ForegroundColor;
            foreach (var l in dl)
            {
                Console.ForegroundColor = defaultColor;
                if (l.VendorId == G13UsbVendorID && l.ProductId == G13UsbProductID)
                    Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{l.DeviceId}");
            }
        }
    }
}
