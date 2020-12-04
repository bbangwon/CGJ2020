using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ2020
{
    public class Player : MonoBehaviour
    {
        List<IUnit> inputUnitList = null;
        public Item_Controller item_Controller;

        private void Awake()
        {
            var inputUnits = GetComponentsInChildren<IUnit>();

            foreach (var inputUnit in inputUnits)
            {
                inputUnit.SetPlayer(this);
                RegistInputUnit(inputUnit);
            }
        }

        void RegistInputUnit(IUnit unit)
        {
            if (inputUnitList == null)
                inputUnitList = new List<IUnit>();

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
