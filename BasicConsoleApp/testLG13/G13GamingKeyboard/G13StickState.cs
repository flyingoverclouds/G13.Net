using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testLG13.G13GamingKeyboard
{

    /// <summary>
    /// Event data for a G13 joystick when its position changed or Click/PgUp/PgDn change (released to pressed, pressed to released).
    /// Not fired when other keys changed (see G13KeyState)
    /// </summary>
    public class G13StickState
    {
        public byte X { get; internal set; }
        public byte Y { get; internal set; }
        public bool StickClickPressed { get; internal set; }
        public bool PageUpPressed { get; internal set; }
        public bool PageDownPressed { get; internal set; }
    }
}
