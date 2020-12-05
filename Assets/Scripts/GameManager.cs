using System.Collections.Generic;
using UnityEngine;
using System.Linq;


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

        [Tooltip("게임 화면 크기")]
        public Rect screenViewRect = new Rect(-6.4f, -3.2f, 12.8f, 7.2f);


        List<Player> playerList = null;        

        public int PlayerCount => playerList.Count;

        public enum GameStates
        {
            Ready,
            Ing,
            Over
        }

        GameStates gameState = GameStates.Ing;
        public GameStates GameState { 
            get => gameState; 
            private set
            {
                gameState = value;
                switch (gameState)
                {
                    case GameStates.Ready:
                        OnGameReady();
                        break;
                    case GameStates.Ing:
                        OnGameIng();
                        break;
                    case GameStates.Over:
                        OnGameOver();
                        break;
                    default:
                        break;
                }
            }
        }

        private void Awake()
        {
            playerList = new List<Player>();
            Camera.main.orthographicSize = 3.6f;    //1280x720          

        }

        private void Update()
        {
            //게임이 끝났는지 확인(1명만 남거나 비겼을때)
            if(playerList.Count(player => player.State == Player.States.Alive) < 2)
                GameState = GameStates.Over;

        }

        void OnGameReady()
        {

        }

        void OnGameIng()
        {

        }

        void OnGameOver()
        {
            //게임 Result 판단(이긴 플레이어가 있는지..)
            Player alivePlayer = playerList.FirstOrDefault(player => player.State == Player.States.Alive);

            if(alivePlayer == null)
            {
                Debug.Log("비김!-- 이스트에그(사회적 거리두기를 하세요!)");
            }
            else
            {
                Debug.Log(alivePlayer.PlayerNumber + "가 이김");
            }          
                
        }

        public void RegistPlayer(Player player)
        {
            playerList.Add(player);
        }

        public void Easter_Die()
        {
            if (playerList != null)
            {
                playerList.ForEach(player => player.Die());
                GameState = GameStates.Over;
            }
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
