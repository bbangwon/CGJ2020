using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;
using UnityEngine.SceneManagement;

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

        [Tooltip("일반공격 범위")]
        public float Attack_Radius = 1f;

        [Tooltip("화염구 범위")]
        public float Fireball_Radius = 1.2f;

        [Tooltip("근접공격 범위")]
        public float Melee_Radius = 0.75f;

        [Tooltip("아이템 프리팹")]
        [SerializeField]
        GameObject[] itemPrefabs;

        [Tooltip("플레이어 컬러 정보")]
        public PlayerColors playerColors;
        [SerializeField] bool Easter_GotoTitle;
        List<Player> playerList = null;        

        public int PlayerCount => playerList.Count;
        public int AlivePlayerCount => playerList.Count(player => player.State == Player.States.Alive);

        Coroutine itemGenerateCoroutine = null;

        public List<int> selectedPlayerNumbers;

        DictionaryObjectPool itemObjectPool;
        Dictionary<GameObject, int> itemObjectPoolLookup;

        public bool isDebugKeyboardUse;

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
            if (instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);

            selectedPlayerNumbers = new List<int>();
            playerList = new List<Player>();
            //Camera.main.orthographicSize = 3.6f;    //1280x720    

            itemObjectPool = new DictionaryObjectPool();

            foreach (var item in itemPrefabs)
                itemObjectPool.AddObjectPool(item.name, item);

            itemObjectPoolLookup = new Dictionary<GameObject, int>();

            //테스트
            //SelectedGamePlayerCount = GameObject.FindObjectsOfType<Player>().Count();
        }

        private void Update()
        {
            //게임이 끝났는지 확인(1명만 남거나 비겼을때)
            //일단 게임오버체크는 나중에
            if (GameState == GameStates.Ing)
            {
                if (selectedPlayerNumbers.Count > 1)
                {
                    if (playerList.Count(p => p.State == Player.States.Alive) < 2)
                        GameState = GameStates.Over;
                }

                if (!isDebugKeyboardUse)
                    return;

                int switchPlayer = -1;
                //테스트로 1P, 2P 바꿀수 있도록
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    switchPlayer = 0;
                }

                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    switchPlayer = 1;
                }

                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    switchPlayer = 2;
                }

                if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    switchPlayer = 3;
                }

                if (switchPlayer > -1)
                {
                    playerList.ForEach(pl => pl.IsInputable = false);

                    var player = playerList.FirstOrDefault(pl => pl.PlayerNumber == switchPlayer);
                    if (player != null)
                        player.IsInputable = true;
                }
            }
            //이스터에그 타이틀 이동 관련
            if (Easter_GotoTitle)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    Easter_GotoTitle = false;
                    GoFirstScene();
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
                Result.In.Open(alivePlayer.PlayerNumber);
            }       
        }

        public void RegistPlayer(Player player)
        {
            playerList.Add(player);

            if (playerList.Count == selectedPlayerNumbers.Count)
                GameState = GameStates.Ing;

            if(playerList.Count > 0)
                playerList.First().IsInputable = true;

            if (!isDebugKeyboardUse)
                playerList.ForEach(p => p.IsInputable = true);          
        }

        public void Easter_Die()
        {
            if (playerList != null)
            {
                playerList.ForEach(player => player.Die());
                GameState = GameStates.Over;
                StartCoroutine(EasterEgg());
            }
        }
        IEnumerator EasterEgg()
        {
            SpriteRenderer Social_Distance = GameObject.Find("Social_Distance").GetComponent<SpriteRenderer>(); //삭제하기 쉽게 지역 변수로 했습니다
            Color alpha = new Color(0, 0, 0, Time.deltaTime / 5);
            yield return new WaitForSeconds(1);
            while (Social_Distance.color.a < 1)
            {
                Social_Distance.color += alpha;
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(5);
            Easter_GotoTitle = true;
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
                SoundManager.In.Play(SoundManager.AudioTypes.Rock_Hit);

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
       
        private void Start()
        {
            
        }
        
        public void GoFirstScene()
        {
            Destroy(gameObject);
            SceneManager.LoadScene(0);
        }

        public void GoSecondScene()
        {
            Destroy(gameObject);
            SceneManager.LoadScene(1);
        }
    } 
}
