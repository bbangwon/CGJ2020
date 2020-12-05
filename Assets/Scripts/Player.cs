using System.Collections.Generic;
using UnityEngine;
using KZLib.Tools;

namespace CGJ2020
{
    public class Player : MonoBehaviour, IJoyCon
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

        //test
        public bool IsInputable { get; set; } = false;

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

            if (!GameManager.In.isDebugKeyboardUse)
                playerNumber = JoyConMgr.In.AddJoyCon(this);
            else
                playerNumber = GameManager.In.PlayerCount - 1;
        }

        void RegistUnit(IUnit unit)
        {
            if (unitList == null)
                unitList = new List<IUnit>();

            unitList.Add(unit);
        }

        private void Update()
        {
            if (!IsInputable || State != States.Alive || !GameManager.In.isDebugKeyboardUse)
                return;

            var unit = unitList[currentUnitIndex];
            unit.Axis(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));

            if (Input.GetButtonDown("Fire1"))
                unit.OnAttack();

            if (Input.GetButtonDown("Fire2"))
                unit.OnTrebuchetChangeMode();

            if (Input.GetButtonDown("Fire3"))
            {
                ChangeUnit();
            }
        }

        void ChangeUnit()
        {
            unitList[currentUnitIndex].OnDeselect();
            currentUnitIndex = ++currentUnitIndex % unitList.Count;
            unitList[currentUnitIndex].OnSelect();
        }

        public void Die()
        {
            State = States.Die;
            if (State == States.Die)
                unitList.ForEach(unit => unit.OnDie());

            SoundManager.In.Play(SoundManager.AudioTypes.Bannerman_Die);

            if(!GameManager.In.isDebugKeyboardUse)
                JoyConMgr.In.SetVibration(PlayerNumber);
        }

        public void SetStick(Vector2 _stick)
        {
            if (State != States.Alive || !IsInputable || GameManager.In.isDebugKeyboardUse)
                return;

            var unit = unitList[currentUnitIndex];
            unit.Axis(new Vector2(-_stick.x, _stick.y));
        }

        public void SetGyro(Vector3 _gyro)
        {
        }

        public void SetAccel(Vector3 _accel)
        {
        }

        public void SetOrientation(Quaternion _orientation)
        {
        }

        public void SetButtonDown(JoyConLib.Button _button)
        {
            if (!IsInputable || State != States.Alive || GameManager.In.isDebugKeyboardUse)
                return;

            var unit = unitList[currentUnitIndex];

            switch (_button)
            {
                case JoyConLib.Button.DPAD_DOWN:
                    unit.OnAttack();
                    break;
                case JoyConLib.Button.DPAD_RIGHT:
                    break;
                case JoyConLib.Button.DPAD_LEFT:
                    unit.OnTrebuchetChangeMode();
                    break;
                case JoyConLib.Button.DPAD_UP:
                    break;
                case JoyConLib.Button.SL:
                case JoyConLib.Button.SR:
                    ChangeUnit();
                    break;
                case JoyConLib.Button.MINUS:
                    break;
                case JoyConLib.Button.HOME:
                    break;
                case JoyConLib.Button.PLUS:
                    break;
                case JoyConLib.Button.CAPTURE:
                    break;
                case JoyConLib.Button.STICK:
                    break;
                case JoyConLib.Button.SHOULDER_1:
                    break;
                case JoyConLib.Button.SHOULDER_2:
                    break;
                default:
                    break;
            }
        }

        public void SetButton(JoyConLib.Button _button)
        {
        }

        public void SetButtonUp(JoyConLib.Button _button)
        {
        }
    } 
}
