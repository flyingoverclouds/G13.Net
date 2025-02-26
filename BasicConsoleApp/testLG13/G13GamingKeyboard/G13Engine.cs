﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testLG13.G13GamingKeyboard;

namespace G13GamingKeyboard
{


    /// <summary>
    /// Allow to acces and managed a Logitech G13 gaming keyboard
    /// </summary>
    public class G13Engine
    {
        //Func<Task,G13Engine,G13State> currentStateChangeHandler;
        Action<G13Engine, G13State> currentStateChangeHandler;
        Action<G13Engine, G13KeyState> currentKeyStateChangeHandler;
        Action<G13Engine, G13StickState> currentStickStateChangeHandler;


        USBHIDDRIVER.USBInterface usbInterface=null; 

        /// <summary>
        /// Print on console the HID data packet in a readable form (binary, hex and decimal)
        /// </summary>
        /// <param name="rawData">byte arry to print</param>
        public static void DebugDisplayRawData(byte[] rawData)
        {
            Console.SetCursorPosition(0, 6);
            Console.WriteLine("RAW: ");
            for (int n = 0; n < rawData.Length; n++)
            {
                Console.WriteLine("RAW[{0}] = {1}   :   0x{2}   {3}        ", n, rawData[n].ToBinaryString(), rawData[n].ToString("X"), rawData[n]); ;
            }
        }


        public static string[] GetDevices()
        {
            var usb = new USBHIDDRIVER.USBInterface(G13UsbVendorID,G13UsbProductID);
            return usb.getDeviceList();
        }


        public G13Engine()
        {
            
        }
        

        /// <summary>
        /// If connected, Contains the winusbapi compatible device path to the connected devices,otherwiser string.Empty;
        /// </summary>
        public string DevicePath { get { return usbInterface?.Usbdevice?.DevicePath;  } } 

        /// <summary>
        /// Logitech USB Vendor ID 
        /// </summary>
        const string G13UsbVendorID = "vid_046d";

        /// <summary>
        /// Logitech G13 USB Gaming keyboard product ID
        /// </summary>
        const string G13UsbProductID = "pid_c21c";

        /// <summary>
        /// Connect to the first G13 device available and start listening HID event. 
        /// If connexion succeeded, the property DevicePath is valued
        /// </summary>
        /// <returns>true if connection succeeded, false in case of failure</returns>
        public bool Connect()
        {
            usbInterface = new USBHIDDRIVER.USBInterface(G13UsbVendorID,G13UsbProductID);
            if (usbInterface == null)
            {
                Console.WriteLine($"ERROR: Unable to instantiate USb interface for {G13UsbVendorID} {G13UsbProductID} "); 
                return false;
            }
            
            if (!usbInterface.Connect())
            {
                Console.WriteLine("ERROR: Unable to connect to G13 Device!!");
                usbInterface = null;
                return false;
            }

            usbInterface.EnableUsbBufferEvent(UsbHidEventHandler); // Register event handler to receive HID event
            usbInterface.StartRead();
            return true;
        }

        /// <summary>
        /// Stop Usb HIV event handling, and close usb channel the device.
        /// </summary>
        public void Disconnect()
        {
            usbInterface.StopRead();
            usbInterface.Disconnect();
            usbInterface = null;
        }

        public void RegisterStateChangeHandler(Action<G13Engine, G13State> chgHandler)
        {
            this.currentStateChangeHandler = chgHandler;
        }

        public void RegisterKeyStateChangeHandler(Action<G13Engine, G13KeyState> kchgHandler)
        {
            this.currentKeyStateChangeHandler = kchgHandler;
        }

        public void RegisterStickStateChangeHandler(Action<G13Engine, G13StickState> schgHandler)
        {
            this.currentStickStateChangeHandler = schgHandler;
        }

        void UsbHidEventHandler(object sender, EventArgs e)
        {
            var lst = (USBHIDDRIVER.List.ListWithEvent)sender;
            //Console.WriteLine("G13 EVENT ! : list size {0}",lst.Count);

            while (lst.Count > 0) // iterate on each HID event.
            {
                byte[] rawHidData;
                lock (lst)
                {
                    rawHidData = lst[0] as byte[];
                    lst.RemoveAt(0);
                }

                
                //DebugDisplayRawData(rawHidData); // for testing and debugging HID data packet

                G13State currentState = DecodeHidRawDataForG13(rawHidData);

                if (this.currentStateChangeHandler != null)
                    this.currentStateChangeHandler(this, currentState);
            };
        }

        G13State DecodeHidRawDataForG13(byte[] rawData)
        {
            // G13 HID keyboard map
            // [0] : 0   0   0   0   0   0   0   1
            //       ?   ?   ?   ?   ?   ?   ?   always1

            // [1] : JOYSTICK HORIZONTAL (0->255)
            // [2] : JOYSTICK VERTICAL (0->255)

            // [3] : 0   0   0   0   0   0   0   0
            //       G8  G7  G6  G5  G4  G3  G2  G1

            // [4] : 0   0   0   0   0   0   0   0
            //       G16 G15 G14 G13 G12 G11 G10 G9

            // [5] : 0   0   0   0   0   0   0   0
            //       |   ?   G22 G21 G20 G19 G18 G17
            //       |LIGHT_STATUS

            // [6] : 0   0   0   0   0   0   0   0
            //       M3  M2  M1  US4 US3 US2 US1 RBUT_LEFT

            // [7] : 0   0   0   0   0   0   0   0
            //       1?  |__/        |   |   |   M4
            //           |B_LIGHT    |   |   |PAGE_UP
            //                       |   |PAGE_DOWN
            //                       |CLICK_STICK

            G13State g = new G13State();
            g.X = rawData[1];
            g.Y = rawData[2];
            g.LightOn = (rawData[5] & 128) != 0;

            foreach(var keyscan in G13KeyMapping.ALL_KEYS_MAPPING)
            {
                if (( rawData[(keyscan &0xFF00) >> 8] & (keyscan&0x00FF)) !=0 )
                {
                    g.PressedKey.Add(keyscan);
                }
            }

            return g;
        }


        /// <summary>
        /// switch on/off led light for key M1, M2, M3 or MR
        /// </summary>
        /// <param name="MKeyCode"></param>
        /// <param name="enlighted"></param>
        public void SetMKeyLight(ushort MKeyCode,bool enlighted)
        {
            Console.WriteLine("SetMKeyLight() Not supported.");
            return;
            // TODO : add support for MKey light
        }



        /// <summary>
        /// Change the color of keyboard led
        /// </summary>
        /// <param name="red">red level</param>
        /// <param name="green">red level</param>
        /// <param name="blue">red level</param>
        public void SetBacklightColor(byte red, byte green,byte blue)
        {
            Console.WriteLine("SetBacklightColor() Not supported.");
            return;

            if (usbInterface == null) // TODO : return exception if not connected.
            {
                Console.WriteLine("Not connected to G13 keyboard.");
                return;
            }
            var g13command = new byte[5] { 5, red, green, blue, 0 };

            if (!usbInterface.write(g13command))
            {
                // TODO : return exception if send fail.
                Console.WriteLine("Unable to send command to G13 device");
            }

            /*
                int error;
	            unsigned char usb_data[] = { 5, 0, 0, 0, 0 };
             	usb_data[1] = red;
	            usb_data[2] = green;
	            usb_data[3] = blue;

	            error = libusb_control_transfer(
                        handle,
			            LIBUSB_REQUEST_TYPE_CLASS | LIBUSB_RECIPIENT_INTERFACE, // the request type field for the setup packet
                            // from libusb.h : LIBUSB_REQUEST_TYPE_CLASS = (0x01 << 5) 
                            // from libusb.h : LIBUSB_RECIPIENT_INTERFACE = 0x01
                        9,          // the request field for the setup packet
                        0x307,      // the value field for the setup packet
                        0,          // the index field for the setup packet
			            usb_data,   // a suitably-sized data buffer for either input or output (depending on direction bits within bmRequestType)
                        5,          // the length field for the setup packet. The data buffer should be at least this size.
                        1000        // timeout (in millseconds) that this function should wait before giving up due to no response being received. For an unlimited timeout, use value 0.
                );
	            if (error != 5) {
		            G13_LOG( error, "Problem sending data" );
		            return;
	            }
             * 
             */

        }

    }
}
