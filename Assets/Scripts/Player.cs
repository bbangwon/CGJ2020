using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ2020
{
    public class Player : MonoBehaviour
    {
        List<IInputUnit> inputUnitList = null;

        private void Awake()
        {
            var inputUnits = GetComponentsInChildren<IInputUnit>();

            foreach (var inputUnit in inputUnits)
            {
                RegistInputUnit(inputUnit);
            }
        }

        void RegistInputUnit(IInputUnit unit)
        {
            if (inputUnitList == null)
                inputUnitList = new List<IInputUnit>();

            inputUnitList.Add(unit);
        }

        private void Update()
        {
            if(inputUnitList.Count == 1)
            {
                var inputUnit = inputUnitList[0];

                inputUnit.Axis(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));

                if (Input.GetButtonDown("Fire1"))
                    inputUnit.OnAttack();

                if (Input.GetButtonDown("Fire2"))
                    inputUnit.OnTrebuchetChangeMode();
            }
        }
    } 
}
