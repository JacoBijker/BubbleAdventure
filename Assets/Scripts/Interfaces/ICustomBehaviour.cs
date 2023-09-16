﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Behavior
{
    public interface ICustomBehaviour
    {
        bool IsActive { get; }
        void Update();
        void Start();
        void Stop();
    }
}
