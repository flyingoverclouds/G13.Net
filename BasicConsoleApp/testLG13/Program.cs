using G13GamingKeyboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace testLG13
{
    class Program
    {

      

        /// <summary>
        /// print on the console all the G13 devices detected on the system
        /// </summary>
        static void ListHidDevices()
        {
            Console.WriteLine("HID Devices : ");
            var devices = G13Engine.GetDevices();
            foreach (var s in devices)
            {
                Console.WriteLine(" {0} ", s);
            }
            Console.WriteLine();
        }

      
        static void Main(string[] args)
        {
            Console.WriteLine("Test USB HID for Logitech G13");
            //ListHidDevices("0");

            // Logitech G13 :  \\?\hid#vid_046d&pid_c21c#7&372657bd&0&0000#{4d1e55b2-f16f-11cf-88cb-001111000030}

            //var usb = new USBHIDDRIVER.USBInterface("vid_046d", "pid_c21c");
            //if (usb == null)
            //{
            //    Console.WriteLine("ERROR: Unable to instantiate USb interface. ");
            //    return;
            //}

            //var devices = usb.getDeviceList();
            //Console.WriteLine("G13 device(s) : {0} detected", devices.Count());
            //foreach (var devG13 in devices)
            //{
            //    Console.WriteLine("      {0} ", devG13);
            //}

            //if (!usb.Connect())
            //{
            //    Console.WriteLine("ERROR: Unable to connect to G13 Device!!");
            //    Console.ReadLine();
            //    return;
            //}

            var g13engine = new G13GamingKeyboard.G13Engine();

            //usb.EnableUsbBufferEvent((obj, a) =>
            //{
            //    var lst = (USBHIDDRIVER.List.ListWithEvent)obj;
            //    //Console.WriteLine("G13 EVENT ! : list size {0}",lst.Count);

            //    while (lst.Count > 0)
            //    {
            //        byte[] rawData;
            //        lock (lst)
            //        {
            //            rawData = lst[0] as byte[];
            //            lst.RemoveAt(0);
            //        }

            //        Console.SetCursorPosition(0, 6);
            //        G13Engine.DisplayRawData(rawData);
            //        G13Data decoded = g13engine.DecodeHidRawDataForG13(rawData);
            //        Console.WriteLine();
            //        Console.WriteLine("DECODED:");
            //        Console.WriteLine($"   STICK({decoded.X},{decoded.Y})      ");
            //        Console.WriteLine($"   LIGHT= {decoded.LightOn}     ");
            //        Console.Write("   PRESSED KEYS: ");
            //        foreach (var kn in decoded.GetPressedKeyNames())
            //            Console.Write($" {kn} ");
            //        Console.Write("                                                                                                      ");

            //    };

            //});
            //usb.StartRead();

            g13engine.Connect();

            Console.WriteLine("Listening G13 key event .... ");
            Console.WriteLine("Press [ENTER] on your main keyboard (not the G13) to exit.");
            Console.ReadLine();

            g13engine.Disconnect();
            
        }
    }
}
