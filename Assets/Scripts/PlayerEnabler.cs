using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ2020
{
    public class PlayerEnabler : MonoBehaviour
    {
        [SerializeField]
        Player[] players;

        [SerializeField]
        int[] testPlayers;

        private void Start()
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i].AddJoyCon(i);
                players[i].NonUse();
            }

            if(GameManager.In.selectedPlayerNumbers.Count == 0)
            {
                GameManager.In.selectedPlayerNumbers.AddRange(testPlayers);
            }

            foreach (var playerNumber in GameManager.In.selectedPlayerNumbers)
            {
                players[playerNumber].ReadyPlay();
            }
       }
    } 
}
