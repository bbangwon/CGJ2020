using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;

namespace CGJ2020
{
    public class GameManager : SingletonMono<GameManager>
    {
        [Tooltip("트레뷰셋 기본 이동 속도")]
        public float trebuchetMoveSpeed = 5f;

        [Tooltip("기수 기본 이동 속도")]
        public float flagmanMoveSpeed = 8f;

        [Tooltip("에임 이동 속도")]
        public float aimMoveSpeed = 0.1f;        

        [Tooltip("공격 시간")]
        public float attackTime = 2f;

        [Tooltip("공격 쿨타임")]
        public float attackCooltime = 5f;

        [Tooltip("최소 사정 거리")]
        public float minAttackRange = 1.5f;

        [Tooltip("최대 사정 거리")]
        public float maxAttackRange = 3f;

        [Tooltip("모드 변경 시간")]
        public float trebuchetModeChangeTime = 2f;

        [Tooltip("버프 이동 속도 증가율")]
        public float buffedMoveSpeedAmount = 2f;

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
        public Rect screenViewRect = new Rect(-6.4f, -3.6f, 12.8f, 7.2f);

        [Tooltip("아이템 생성 시간")]
        public float itemGenerateTime = 30f;

        [Tooltip("아이템 프리팹")]
        [SerializeField]
        GameObject[] itemPrefabs;

        List<Player> playerList = null;        

        public int PlayerCount => playerList.Count;

        Coroutine itemGenerateCoroutine = null;

        public static int SelectedGamePlayerCount = 2;  //선택된 게임플레이어 인원

        DictionaryObjectPool itemObjectPool;
        Dictionary<GameObject, int> itemObjectPoolLookup;

        public enum GameStates
        {
            Ready,
            Ing,
            Over
        }

        GameStates gameState = GameStates.Ready;
        public GameStates GameState { 
            get => gameState; 
            private set
            {
                if (gameState == value)
                    return;

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

            itemObjectPool = new DictionaryObjectPool();

            foreach (var item in itemPrefabs)
                itemObjectPool.AddObjectPool(item.name, item);

            itemObjectPoolLookup = new Dictionary<GameObject, int>();

            //테스트
            SelectedGamePlayerCount = GameObject.FindObjectsOfType<Player>().Count();
        }

        private void Update()
        {
            //게임이 끝났는지 확인(1명만 남거나 비겼을때)
            //일단 게임오버체크는 나중에

            if(SelectedGamePlayerCount > 1)
            {
                if (playerList.Count(player => player.State == Player.States.Alive) < 2)
                    GameState = GameStates.Over;
            }

            if(GameState == GameStates.Ing)
            {
                //테스트로 1P, 2P 바꿀수 있도록
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    playerList.ForEach(pl => pl.IsInputable = false);
                    if (playerList.Count > 0)
                        playerList[0].IsInputable = true;
                }

                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    playerList.ForEach(pl => pl.IsInputable = false);
                    if (playerList.Count > 1)
                        playerList[1].IsInputable = true;
                }
            }
        }

        IEnumerator ItemGenerate()
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(itemGenerateTime);            
            while(GameState == GameStates.Ing)
            {
                Vector2 randomPos = new Vector2(
                    Random.Range(screenViewRect.xMin + 0.5f, screenViewRect.xMax - 0.5f),
                    Random.Range(screenViewRect.yMin + 0.5f, screenViewRect.yMax - 0.5f)
                );

                int randomIndex = Random.Range(0, itemPrefabs.Length);
                GameObject item = itemObjectPool[randomIndex].Spawn(randomPos);                
                itemObjectPoolLookup[item] = randomIndex;

                yield return waitForSeconds;
            }
        }

        public void ItemDespawn(GameObject item)
        {
            if (itemObjectPoolLookup.ContainsKey(item))
                itemObjectPool[itemObjectPoolLookup[item]].Despawn(item);
        }

        void OnGameReady()
        {

        }

        void OnGameIng()
        {
            itemGenerateCoroutine = StartCoroutine(ItemGenerate());
            playerList.ForEach(player => player.BeginPlay());
        }

        void OnGameOver()
        {
            playerList.ForEach(player => player.IsInputable = false);

            if(itemGenerateCoroutine != null)
            {
                StopCoroutine(itemGenerateCoroutine);
                itemGenerateCoroutine = null;
            }
            
            //게임 Result 판단(이긴 플레이어가 있는지..)
            Player alivePlayer = playerList.FirstOrDefault(player => player.State == Player.States.Alive);
            if(alivePlayer == null)
            {
                Debug.Log("게임 오버))) 비김!-- 이스트에그(사회적 거리두기를 하세요!)");
            }
            else
            {
                Debug.Log("게임 오버)))" + alivePlayer.PlayerNumber + "가 이김");
            }       
        }

        public void RegistPlayer(Player player)
        {
            playerList.Add(player);

            //PlayerNumber Test
            player.SetPlayerNumber(playerList.Count - 1);
            if (playerList.Count == SelectedGamePlayerCount)
                GameState = GameStates.Ing;

             playerList[0].IsInputable = true;
        }

        public void Easter_Die()
        {
            if (playerList != null)
            {
                playerList.ForEach(player => player.Die());
                GameState = GameStates.Over;
            }
        }

        public void CreateDangerZone(Vector3 position, Player sender, bool nearAttack = false)
        {
            GameObject dangerZoneObj = Instantiate(dangerZonePrefab, position, Quaternion.identity);
            DangerZone dangerZone = dangerZoneObj.GetComponent<DangerZone>();            

            if(nearAttack)
            {
                dangerZone.Excute(DangerZone.Types.NearAttack, sender);
            }
            else
            {
                if (sender.item_Controller.State_Fireball)
                {
                    dangerZone.Excute(DangerZone.Types.Fireball, null);
                }
                else
                {
                    dangerZone.Excute(DangerZone.Types.Normal, sender);
                }
            }
        }
        void test()
        {
            playerList[0].item_Controller.State_Fireball = false;
            CreateDangerZone(Vector2.zero, playerList[0]);
        }
        private void Start()
        {
            //Invoke("test", 1f);
            
        }
    } 
}
