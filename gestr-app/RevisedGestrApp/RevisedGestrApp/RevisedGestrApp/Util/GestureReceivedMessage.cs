﻿/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using System;
using System.Collections.Generic;
using System.Text;

namespace RevisedGestrApp.Util
{
    public class GestureReceivedMessage
    {
        public string Gesture { get; set; }
        public GestureReceivedMessage(string gesture)
        {
            Gesture = gesture;
        }
    }
}
