using KZLib.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CGJ2020
{
    public class UIPlayer : MonoBehaviour, IJoyCon
    {
        public int playerNumber { get; set; }
        public void SetAccel(Vector3 _accel)
        {
            
        }

        public void SetButton(JoyConLib.Button _button)
        {
            
        }

        public void SetButtonDown(JoyConLib.Button _button)
        {
            if(_button == JoyConLib.Button.DPAD_DOWN)
            {
                if (TitleManager.In.TitleState == TitleManager.TitleStates.Title)
                    TitleManager.In.TitleState = TitleManager.TitleStates.CharacterSelect;
                else
                    //캐릭터 선택
                    SelectPlayer();
            }

            if(_button == JoyConLib.Button.SHOULDER_2)
            {
                if(GameManager.In.selectedPlayerNumbers.Count >= 2)
                {
                    //게임 시작
                    JoyConMgr.In.ClearJoyCon();
                    SceneManager.LoadScene(1);
                }
            }
        }

        public void SetButtonUp(JoyConLib.Button _button)
        {
            
        }

        public void SetGyro(Vector3 _gyro)
        {
            
        }

        public void SetOrientation(Quaternion _orientation)
        {
            
        }

        public void SetStick(Vector2 _stick)
        {
         
        }

        public void SetAddJoyCon(int index)
        {
            if (!GameManager.In.isDebugKeyboardUse)
                playerNumber = JoyConMgr.In.AddJoyCon(this);
            else
                playerNumber = index;
        }

        public void SelectPlayer()
        {
            transform.GetChild(0).gameObject.SetActive(true);
            GameManager.In.selectedPlayerNumbers.Add(playerNumber);
        }
    } 
}
