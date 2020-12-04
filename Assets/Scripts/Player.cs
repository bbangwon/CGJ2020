using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ2020
{
    public class Player : MonoBehaviour
    {
        public enum States
        {
            Alive,
            Die
        }

        public States State { get; private set; } = States.Alive;
        
        List<IUnit> unitList = null;
        public Item_Controller item_Controller;

        private void Awake()
        {
            var inputUnits = GetComponentsInChildren<IUnit>();

            foreach (var inputUnit in inputUnits)
            {
                inputUnit.SetPlayer(this);
                RegistUnit(inputUnit);
            }
        }

        private void Start()
        {
            GameManager.In.RegistPlayer(this);
        }

        void RegistUnit(IUnit unit)
        {
            if (unitList == null)
                unitList = new List<IUnit>();

            unitList.Add(unit);
        }

        private void Update()
        {
            if(unitList.Count == 1)
            {
                var unit = unitList[0];

                if(State == States.Alive)
                {
                    unit.Axis(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));

                    if (Input.GetButtonDown("Fire1"))
                        unit.OnAttack();

                    if (Input.GetButtonDown("Fire2"))
                        unit.OnTrebuchetChangeMode();
                }              
            }
        }

        public void Die()
        {
            State = States.Die;
            if (State == States.Die)
                unitList.ForEach(unit => unit.OnDie());
        }
    } 
}
