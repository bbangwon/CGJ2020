using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace CGJ2020
{
    public class Player : MonoBehaviour
    {
        public enum States
        {
            Ready,
            Alive,
            Die
        }
        public States State { get; private set; } = States.Ready;
        
        List<IUnit> unitList = null;
        public Item_Controller item_Controller;

        int currentUnitIndex = 0;

        int playerNumber;
        public int PlayerNumber => playerNumber;        

        public void SetPlayerNumber(int playerNumber)
        {
            this.playerNumber = playerNumber;
        }

        public void BeginPlay()
        {
            State = States.Alive;
        }

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
            if (State == States.Alive)
            {
                var unit = unitList[currentUnitIndex];
                unit.Axis(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));

                if (Input.GetButtonDown("Fire1"))
                    unit.OnAttack();

                if (Input.GetButtonDown("Fire2"))
                    unit.OnTrebuchetChangeMode();

                if (Input.GetButtonDown("Fire3"))
                {
                    unitList[currentUnitIndex].OnDeselect();
                    currentUnitIndex = ++currentUnitIndex % unitList.Count;
                    unitList[currentUnitIndex].OnSelect();
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
