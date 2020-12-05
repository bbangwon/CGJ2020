using KZLib.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CGJ2020
{
    public class UIPlayer : MonoBehaviour, IJoyCon
    {
        int playerNumber;
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
                //캐릭터 선택
                SelectPlayer();
            }

            if(_button == JoyConLib.Button.SR)
            {
                //게임 시작
                JoyConMgr.In.ClearJoyCon();
                SceneManager.LoadScene(1);
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

        public void SetAddJoyCon()
        {
            if(!GameManager.In.isDebugKeyboardUse)
                playerNumber = JoyConMgr.In.AddJoyCon(this);
        }

        public void SelectPlayer()
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    } 
}
