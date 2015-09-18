﻿// Copyright (c) 2014 Augie R. Maddox, Guavaman Enterprises. All rights reserved.

namespace Rewired.Editor {

    using UnityEngine;
    using UnityEditor;
    using Rewired;
    using Rewired.Data.Mapping;

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    [CustomEditor(typeof(HardwareJoystickMap))]
    public sealed class HardwareJoystickMapInspector : CustomInspector_External {
        
        private void OnEnable() {
            internalEditor = new HardwareJoystickMapInspector_Internal(this);
            base.Enabled();
        }
    }
}
