using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ2020
{
    public class DangerZone : MonoBehaviour
    {
        public enum Types
        {
            Normal,
            Fireball
        }
        public void Excute(Types type)
        {
            switch (type)
            {
                case Types.Normal:
                    break;
                case Types.Fireball:
                    break;
                default:
                    break;
            }
        }
    } 
}
