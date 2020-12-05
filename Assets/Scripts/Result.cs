using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace CGJ2020
{
    public class Result : SingletonMono<Result>
    {
        [SerializeField] private GameObject result;
        [SerializeField] private Image characterHead;
        [SerializeField] private Text winMessage;
        [SerializeField] private Button goTitle;

        private void Start()
        {
            result.SetActive(false);
        }

        public void Open(int winPlayerNum)
        {
            characterHead.color = GameManager.In.playerColors.playerColors[winPlayerNum];
            winMessage.text = "Player " + (winPlayerNum+1) + " Win";
            result.SetActive(true);
            goTitle.Select();
        }

        public void ClickedGoTitle()
        {
            GameManager.In.GoFirstScene();
        }

        public void ClikedReGame()
        {
            GameManager.In.GoSecondScene();
        }
    }
}
