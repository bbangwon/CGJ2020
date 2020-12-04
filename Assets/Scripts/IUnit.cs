using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ2020
{
    public interface IUnit
    {
        void SetPlayer(Player player);
        void Axis(Vector2 axis);
        void OnAttack();
        void OnTrebuchetChangeMode();
        void OnDie();
        
    } 
}
