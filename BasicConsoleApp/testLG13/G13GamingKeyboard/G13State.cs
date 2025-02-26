﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G13GamingKeyboard
{
    /*
    // G13 HID message (8 bytes)
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
    //       1?  |__/        |   |   |   MR
    //           |B_LIGHT    |   |   |CLIC_LEFT
    //                       |   |CLIC_BOTTOM
    //                       |CLICK_STICK
    */

    class G13KeyMapping
    {
       

        // high byte : index in raw data
        // low byte : binary mask for key bit
        public const ushort MAP_KEY_G1  = 0x0301;
        public const ushort MAP_KEY_G2  = 0x0302;
        public const ushort MAP_KEY_G3  = 0x0304;
        public const ushort MAP_KEY_G4  = 0x0308;
        public const ushort MAP_KEY_G5  = 0x0310;
        public const ushort MAP_KEY_G6  = 0x0320;
        public const ushort MAP_KEY_G7  = 0x0340;
        public const ushort MAP_KEY_G8  = 0x0380;
        public const ushort MAP_KEY_G9  = 0x0401;
        public const ushort MAP_KEY_G10 = 0x0402;
        public const ushort MAP_KEY_G11 = 0x0404;
        public const ushort MAP_KEY_G12 = 0x0408;
        public const ushort MAP_KEY_G13 = 0x0410;
        public const ushort MAP_KEY_G14 = 0x0420;
        public const ushort MAP_KEY_G15 = 0x0440;
        public const ushort MAP_KEY_G16 = 0x0480;
        public const ushort MAP_KEY_G17 = 0x0501;
        public const ushort MAP_KEY_G18 = 0x0502;
        public const ushort MAP_KEY_G19 = 0x0504;
        public const ushort MAP_KEY_G20 = 0x0508;
        public const ushort MAP_KEY_G21 = 0x0510;
        public const ushort MAP_KEY_G22 = 0x0520;
        public const ushort MAP_KEY_M1  = 0x0620; // memory button M1
        public const ushort MAP_KEY_M2  = 0x0640; // memory button M2
        public const ushort MAP_KEY_M3  = 0x0680; // memory button M3
        public const ushort MAP_KEY_MR  = 0x0701; // memory button M4
        public const ushort MAP_KEY_US1 = 0x0602; // under screen button 1
        public const ushort MAP_KEY_US2 = 0x0604; // under screen button 2
        public const ushort MAP_KEY_US3 = 0x0608; // under screen button 3
        public const ushort MAP_KEY_US4 = 0x0610; // under screen button 4
        public const ushort MAP_KEY_RBLIGHT = 0x0760; // Rond button right (Light switch)
        public const ushort MAP_KEY_RBLEFT  = 0x0601; // Rond button left
        public const ushort MAP_KEY_PAGEUP   = 0x0702; // Button left of joystick
        public const ushort MAP_KEY_PAGEDOWN = 0x0704; // Button bottom of joystick
        public const ushort MAP_KEY_CLICKSTICK  = 0x0708; // Button Stick pressed

        static readonly ushort[] _ALL_KEYS_MAPPING;
        static public ushort[] ALL_KEYS_MAPPING
        {
            get { return _ALL_KEYS_MAPPING; }
        }
        
        static readonly Dictionary<string, ushort> g13KeyMap;
        static public Dictionary<string, ushort> KeyMapping
        {
            get { return g13KeyMap; }
        }

        static G13KeyMapping()
        {
            _ALL_KEYS_MAPPING = new ushort[]
            {
                MAP_KEY_G1,
                MAP_KEY_G2,
                MAP_KEY_G3,
                MAP_KEY_G4,
                MAP_KEY_G5,
                MAP_KEY_G6,
                MAP_KEY_G7,
                MAP_KEY_G8,
                MAP_KEY_G9,
                MAP_KEY_G10,
                MAP_KEY_G11,
                MAP_KEY_G12,
                MAP_KEY_G13,
                MAP_KEY_G14,
                MAP_KEY_G15,
                MAP_KEY_G16,
                MAP_KEY_G17,
                MAP_KEY_G18,
                MAP_KEY_G19,
                MAP_KEY_G20,
                MAP_KEY_G21,
                MAP_KEY_G22,
                MAP_KEY_M1,
                MAP_KEY_M2,
                MAP_KEY_M3,
                MAP_KEY_MR,
                MAP_KEY_US1,
                MAP_KEY_US2,
                MAP_KEY_US3,
                MAP_KEY_US4,
                MAP_KEY_RBLIGHT,
                MAP_KEY_RBLEFT,
                MAP_KEY_PAGEUP,
                MAP_KEY_PAGEDOWN,
                MAP_KEY_CLICKSTICK
            };
            g13KeyMap = new Dictionary<string, ushort>();
            g13KeyMap.Add(nameof(MAP_KEY_G1), MAP_KEY_G1);
            g13KeyMap.Add(nameof(MAP_KEY_G2), MAP_KEY_G2);
            g13KeyMap.Add(nameof(MAP_KEY_G3), MAP_KEY_G3);
            g13KeyMap.Add(nameof(MAP_KEY_G4), MAP_KEY_G4);
            g13KeyMap.Add(nameof(MAP_KEY_G5), MAP_KEY_G5);
            g13KeyMap.Add(nameof(MAP_KEY_G6), MAP_KEY_G6);
            g13KeyMap.Add(nameof(MAP_KEY_G7), MAP_KEY_G7);
            g13KeyMap.Add(nameof(MAP_KEY_G8), MAP_KEY_G8);
            g13KeyMap.Add(nameof(MAP_KEY_G9), MAP_KEY_G9);
            g13KeyMap.Add(nameof(MAP_KEY_G10), MAP_KEY_G10);
            g13KeyMap.Add(nameof(MAP_KEY_G11), MAP_KEY_G11);
            g13KeyMap.Add(nameof(MAP_KEY_G12), MAP_KEY_G12);
            g13KeyMap.Add(nameof(MAP_KEY_G13), MAP_KEY_G13);
            g13KeyMap.Add(nameof(MAP_KEY_G14), MAP_KEY_G14);
            g13KeyMap.Add(nameof(MAP_KEY_G15), MAP_KEY_G15);
            g13KeyMap.Add(nameof(MAP_KEY_G16), MAP_KEY_G16);
            g13KeyMap.Add(nameof(MAP_KEY_G17), MAP_KEY_G17);
            g13KeyMap.Add(nameof(MAP_KEY_G18), MAP_KEY_G18);
            g13KeyMap.Add(nameof(MAP_KEY_G19), MAP_KEY_G19);
            g13KeyMap.Add(nameof(MAP_KEY_G20), MAP_KEY_G20);
            g13KeyMap.Add(nameof(MAP_KEY_G21), MAP_KEY_G21);
            g13KeyMap.Add(nameof(MAP_KEY_G22), MAP_KEY_G22);
            g13KeyMap.Add(nameof(MAP_KEY_M1), MAP_KEY_M1);
            g13KeyMap.Add(nameof(MAP_KEY_M2), MAP_KEY_M2);
            g13KeyMap.Add(nameof(MAP_KEY_M3), MAP_KEY_M3);
            g13KeyMap.Add(nameof(MAP_KEY_MR), MAP_KEY_MR);
            g13KeyMap.Add(nameof(MAP_KEY_US1), MAP_KEY_US1);
            g13KeyMap.Add(nameof(MAP_KEY_US2), MAP_KEY_US2);
            g13KeyMap.Add(nameof(MAP_KEY_US3), MAP_KEY_US3);
            g13KeyMap.Add(nameof(MAP_KEY_US4), MAP_KEY_US4);
            g13KeyMap.Add(nameof(MAP_KEY_RBLIGHT), MAP_KEY_RBLIGHT);
            g13KeyMap.Add(nameof(MAP_KEY_RBLEFT), MAP_KEY_RBLEFT);
            g13KeyMap.Add(nameof(MAP_KEY_PAGEUP), MAP_KEY_PAGEUP);
            g13KeyMap.Add(nameof(MAP_KEY_PAGEDOWN), MAP_KEY_PAGEDOWN);
            g13KeyMap.Add(nameof(MAP_KEY_CLICKSTICK), MAP_KEY_CLICKSTICK);
        }
    }

    public class G13State
    {
        public int X { get; internal set; }
        public int Y { get; internal set; }
        public bool LightOn { get; internal set; }

        public List<ushort> PressedKey { get; private set; }

        /// <summary>
        /// return the pressed state of a key
        /// </summary>
        /// <param name="g13KeyScanCode">Key code (G13KeyMapping.MAP_KEY_* constant) </param>
        /// <returns>true if key is pressed, false, otherwise</returns>
        public bool IsKeyPressed(ushort g13KeyScanCode)
        {
            for (int n = 0; n < PressedKey.Count; n++)
                if (PressedKey[n] == g13KeyScanCode)
                    return true;
            return false;
        }

        public List<string> GetPressedKeyNames()
        {
            var l = new List<string>();
            foreach(var ksc in PressedKey)
            {
                foreach(var kp in G13KeyMapping.KeyMapping)
                {
                    if (kp.Value == ksc)
                        l.Add(kp.Key);
                }
            }
            return l;
        }

        public G13State()
        {
            PressedKey = new List<ushort>();
        }
    }

}
