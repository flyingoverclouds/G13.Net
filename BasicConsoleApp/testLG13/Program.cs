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
        static void PrintHidDevices()
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
            PrintHidDevices();

            // Logitech G13 on my dev system :  \\?\hid#vid_046d&pid_c21c#7&372657bd&0&0000#{4d1e55b2-f16f-11cf-88cb-001111000030}

            var g13engine = new G13Engine();

            g13engine.RegisterStateChangeHandler((g, g13state) =>
            {
                Console.WriteLine();
                Console.WriteLine("DECODED:");
                Console.WriteLine($"   STICK({g13state.X},{g13state.Y})      ");
                Console.WriteLine($"   LIGHT= {g13state.LightOn}     ");
                Console.Write("   PRESSED KEYS: ");
                foreach (var kn in g13state.GetPressedKeyNames())
                    Console.Write($" {kn} ");
                Console.WriteLine("                                                                                                                  ");
                if (g13state.IsKeyPressed(G13KeyMapping.MAP_KEY_G1))
                    g.SetBacklightColor(255, 0, 0);
                if (g13state.IsKeyPressed(G13KeyMapping.MAP_KEY_G2))
                    g.SetBacklightColor(0,255, 0);
                if (g13state.IsKeyPressed(G13KeyMapping.MAP_KEY_G3))
                    g.SetBacklightColor(0, 0, 255);
                if (g13state.IsKeyPressed(G13KeyMapping.MAP_KEY_M1))
                    g.SetMKeyLight(G13KeyMapping.MAP_KEY_M1,true);
                if (g13state.IsKeyPressed(G13KeyMapping.MAP_KEY_M2))
                    g.SetMKeyLight(G13KeyMapping.MAP_KEY_M2, true);
                if (g13state.IsKeyPressed(G13KeyMapping.MAP_KEY_M3))
                    g.SetMKeyLight(G13KeyMapping.MAP_KEY_M3, true);
                if (g13state.IsKeyPressed(G13KeyMapping.MAP_KEY_MR))
                    g.SetMKeyLight(G13KeyMapping.MAP_KEY_MR, true);
            });


            g13engine.Connect();

            Console.WriteLine("Listening G13 events .... ");
            Console.WriteLine("Press [ENTER] on your main keyboard (not the G13) to exit.");
            Console.ReadLine();

            g13engine.Disconnect();
            
        }
    }
}
