using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G13GamingKeyboard
{
    class G13Engine
    {
        public KeyStateEnum GetKeyState(ushort keyscancode)
        {
            return KeyStateEnum.RELEASED;
        }

        public G13Data DecodeHidRawDataForG13(byte[] rawData)
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
            //           |B_LIGHT    |   |   |CLIC_LEFT
            //                       |   |CLIC_BOTTOM
            //                       |CLICK_STICK

            G13Data g = new G13Data();
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

    }
}
