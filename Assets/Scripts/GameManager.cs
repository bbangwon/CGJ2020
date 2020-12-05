using System.Collections.Generic;
using UnityEngine;


namespace CGJ2020
{
    public class GameManager : SingletonMono<GameManager>
    {
        [Tooltip("트레뷰셋 기본 이동 속도")]
        public float trebuchetMoveSpeed = 0.05f;

        [Tooltip("기수 기본 이동 속도")]
        public float flagmanMoveSpeed = 0.03f;

        [Tooltip("에임 이동 속도")]
        public float aimMoveSpeed = 0.1f;        

        [Tooltip("공격 시간")]
        public float attackTime = 2f;

        [Tooltip("공격 쿨타임")]
        public float attackCooltime = 5f;

        [Tooltip("최소 사정 거리")]
        public float minAttackRange = 150f;

        [Tooltip("최대 사정 거리")]
        public float maxAttackRange = 500f;

        [Tooltip("모드 변경 시간")]
        public float trebuchetModeChangeTime = 2f;

        [Tooltip("아이템 생성 시간")]
        public float itemGenerateTime = 30f;

        [Tooltip("버프 이동 속도 증가율")]
        public float buffedMoveSpeedAmount = 0.03f;

        [Tooltip("버프 이동 속도 효과 시간")]
        public float buffedMoveSpeedEffectTime = 10f;

        [Tooltip("버프 사정거리 효과 시간")]
        public float buffedAttackRangeEffectTime = 20f;

        [Tooltip("화염탄 돌덩이 크기 증가율")]
        public float buffedFireballSizeAmount = 2f;

        [Tooltip("화염탄 화염 효과 시간")]
        public float buffedFireballEffectTime = 5f;

        [Tooltip("위험존 프리팹")]
        public GameObject dangerZonePrefab;


        List<Player> playerList = null;

        public int PlayerCount => playerList.Count;

        private void Awake()
        {
            playerList = new List<Player>();
        }

        public void RegistPlayer(Player player)
        {
            playerList.Add(player);
        }

        public void Easter_Die()
        {
            if (playerList != null)
                playerList.ForEach(player => player.Die());
        }

        public void CreateDangerZone(Vector3 position, Player sender)
        {
            GameObject dangerZoneObj = Instantiate(dangerZonePrefab, position, Quaternion.identity);
            DangerZone dangerZone = dangerZoneObj.GetComponent<DangerZone>();            

            if (sender.item_Controller.State_Fireball)
            {
                dangerZone.Excute(DangerZone.Types.Fireball, null);
            }
            else
            {
                dangerZone.Excute(DangerZone.Types.Normal, sender);
            }
        }
        void test()
        {
            playerList[0].item_Controller.State_Fireball = false;
            CreateDangerZone(Vector2.zero, playerList[0]);
        }
        private void Start()
        {
            Invoke("test", 1f);
        }
    } 
}
