using KZLib.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ2020
{
    public class TitleManager : MonoBehaviour
    {
        public GameObject title;
        public GameObject character;

        static TitleManager instance;
        public static TitleManager In => instance;

        public enum TitleStates
        {
            Title,
            CharacterSelect
        }

        TitleStates titleState = TitleStates.Title;
        public TitleStates TitleState {
            get => titleState;
            set
            {
                titleState = value;
                title.SetActive(titleState == TitleStates.Title);
                character.SetActive(titleState == TitleStates.CharacterSelect);
            }
        }

        private void Awake()
        {
            instance = this;
            JoyConMgr.In.ClearJoyCon();
        }
    } 
}
