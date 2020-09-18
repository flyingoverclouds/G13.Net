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

        static string ToBinaryString(byte b)
        {
            StringBuilder sb = new StringBuilder(8);
            for (int n = 0; n < 8; n++) {
                if ((b & 128) != 0)
                    sb.Append("1");
                else sb.Append("0");
                b = (byte)(b << 1);
            }
            return sb.ToString();
        }

        static void ListHidDevices(string vid)
        {
            Console.WriteLine("HID Devices : ");
            var usb = new USBHIDDRIVER.USBInterface(vid);
            var devices = usb.getDeviceList();
            foreach (var s in devices)
            {
                Console.WriteLine(" {0} ", s);
            }
            Console.WriteLine();
        }

        static void DisplayRawData(byte[] rawData)
        {
            Console.WriteLine("RAW: ");
            for (int n = 0; n < rawData.Length; n++)
            {
                Console.WriteLine("RAW[{0}] = {1}   :   0x{2}   {3}        ", n, ToBinaryString(rawData[n]), rawData[n].ToString("X"), rawData[n]); ;
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Test USB HID for Logitech G13");
            //ListHidDevices("0");

            // Logitech G13 :  \\?\hid#vid_046d&pid_c21c#7&372657bd&0&0000#{4d1e55b2-f16f-11cf-88cb-001111000030}
            var usb = new USBHIDDRIVER.USBInterface("vid_046d", "pid_c21c");
            if (usb == null)
            {
                Console.WriteLine("ERROR: Unable to instantiate USb interface. ");
                return;
            }

            var devices = usb.getDeviceList();
            Console.WriteLine("G13 device(s) : {0} detected", devices.Count());
            foreach (var devG13 in devices)
            {
                Console.WriteLine("      {0} ", devG13);
            }

            if (!usb.Connect())
            {
                Console.WriteLine("ERROR: Unable to connect to G13 Device!!");
                Console.ReadLine();
                return;
            }

            var g13engine = new G13GamingKeyboard.G13Engine();

            usb.enableUsbBufferEvent((obj, a) =>
            {
                var lst = (USBHIDDRIVER.List.ListWithEvent)obj;
                //Console.WriteLine("G13 EVENT ! : list size {0}",lst.Count);

                while (lst.Count > 0)
                {
                    byte[] rawData;
                    lock (lst)
                    {
                        rawData = lst[0] as byte[];
                        lst.RemoveAt(0);
                    }
                    
                    Console.SetCursorPosition(0, 6);
                    DisplayRawData(rawData);
                    G13Data decoded = g13engine.DecodeHidRawDataForG13(rawData);
                    Console.WriteLine();
                    Console.WriteLine("DECODED:");
                    Console.WriteLine($"   STICK({decoded.X},{decoded.Y})      ");
                    Console.WriteLine($"   LIGHT= {decoded.LightOn}     ");
                    Console.Write("   PRESSED KEYS: ");
                    foreach (var kn in decoded.GetPressedKeyNames())
                        Console.Write($" {kn} ");
                    Console.Write("                                                                                                      ");

                };
                
            });
            usb.startRead();
            Console.WriteLine("Listening G13 key event .... ");
            Console.WriteLine("Press [ENTER] on your main keyboard (not the G13) to exit.");
            Console.ReadLine();
            usb.stopRead();
            usb.Disconnect();
            
        }
    }
}
