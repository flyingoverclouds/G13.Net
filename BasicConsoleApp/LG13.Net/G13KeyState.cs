﻿using System;

namespace G13GamingKeyboard
{

    /// <summary>
    /// Event data for a G13 Key when its state change (released to pressed, pressed to released).
    /// Not fired when stick move (see G13StickState)
    /// </summary>
    public class G13KeyState
    {
        public ushort KeyCode { get; internal set; }
        public bool Pressed { get; internal set; }
        public bool LightOn { get; internal set; }
    }

}