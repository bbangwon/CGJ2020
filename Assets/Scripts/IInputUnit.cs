﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ2020
{
    public interface IInputUnit
    {
        void Axis(Vector2 axis);
        void OnAttack();
        void OnTrebuchetChangeMode();
    } 
}
