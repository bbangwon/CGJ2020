using UnityEngine;
using UnityEngine.SceneManagement;

namespace CGJ2020
{
    public class PlayerSelect : MonoBehaviour
    {
        [SerializeField]
        UIPlayer[] UIPlayers;
        // Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < UIPlayers.Length; i++)
            {
                UIPlayers[i].SetAddJoyCon(i);
            }
        }

        private void Update()
        {
            if(GameManager.In.isDebugKeyboardUse)
            {
                if(Input.GetKeyDown(KeyCode.Alpha1))
                {
                    UIPlayers[0].SelectPlayer();
                }

                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    UIPlayers[1].SelectPlayer();
                }

                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    UIPlayers[2].SelectPlayer();
                }

                if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    UIPlayers[3].SelectPlayer();
                }

                if(Input.GetKeyDown(KeyCode.Return))
                {
                    if (TitleManager.In.TitleState == TitleManager.TitleStates.Title)
                        TitleManager.In.TitleState = TitleManager.TitleStates.CharacterSelect;

                    else if (GameManager.In.selectedPlayerNumbers.Count >= 2)
                        SceneManager.LoadScene(1);
                }
            }
        }
    } 
}
