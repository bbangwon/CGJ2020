using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ2020
{
    public class Trebuchet : MonoBehaviour, IInputUnit
    {
        public void SetPlayer(Player player)
        {

        }

        public void Axis(Vector2 axis)
        {
            if (!isInstall && !isPlayerMode)
            {
                rigidbody2d.velocity = new Vector2(axis.x, axis.y) * currentSpeed;
            }
            else if (aim.gameObject.activeSelf)
            {
                aim.ActionMove(axis);
            }
        }

        public void OnAttack()
        {

        }

        public void OnTrebuchetChangeMode()
        {
            if (isInstall && !isModeChanging)
            {
                MoveOn();
            }
            else if (!isInstall)
            {
                Install();
            }
        }

        public void OnPlayerChangeMode()
        {
            if (isPlayerMode)
            {
                TrebuchetMode();
            }
            else
            {
                PlayerMode();
            }
        }

        /// <summary>
        /// 스피디아이템 사용
        /// </summary>
        public void UseItemSpeedUp(float upSize, float time)
        {
            ActionCoroutine(SpeedUp(upSize, time));
        }

        /// <summary>
        /// 거리업아이템 사용
        /// </summary>
        public void UseItemRangeUp(float upSize, float time)
        {
            ActionCoroutine(RangeUp(upSize, time));
        }

        #region Private
        [SerializeField] private TrebuchetAim aim;
        [SerializeField] private ViewRange viewAttackRange;

        private bool isPlayerMode;
        private bool isInstall;
        private bool isModeChanging;

        private float horizontal;
        private float vertical;

        private float currentSpeed;
        private float currentMaxRange;

        private Rigidbody2D rigidbody2d;
        private IEnumerator currentCoroutine;

        private void Awake()
        {
            rigidbody2d = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            isModeChanging = false;
            currentSpeed = GameManager.In.trebuchetMoveSpeed;
            currentMaxRange = GameManager.In.maxAttackRange;
            PlayerMode();
        }

        private void Update()
        {
            TestActionMove();
            TestTemporary();
        }

        /// <summary>
        /// 설치모드로 전환
        /// </summary>
        private void Install()
        {
            if (!isPlayerMode)
            {
                Debug.Log("설치모드 전환");
                isInstall = true;
                rigidbody2d.velocity = Vector2.zero;
                StartCoroutine(ChangingAttackMode());
            }
        }

        /// <summary>
        /// 공격모드로 전환중
        /// </summary>
        /// <returns></returns>
        private IEnumerator ChangingAttackMode()
        {
            Debug.Log("공격모드로 전환중");
            isModeChanging = true;
            float timer = 0f;
            while(timer <= GameManager.In.trebuchetModeChangeTime)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            AttackOn();
            isModeChanging = false;
            Debug.Log("공격모드로 전환완료");
        }

        /// <summary>
        /// 공격모드로 전환
        /// </summary>
        private void AttackOn()
        {
            Debug.Log("공격모드 전환");
            aim.gameObject.SetActive(true);
            viewAttackRange.gameObject.SetActive(true);
        }

        /// <summary>
        /// 이동모드로 전환
        /// </summary>
        private void MoveOn()
        {
            Debug.Log("이동모드 전환");
            isInstall = false;
            aim.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            aim.gameObject.SetActive(false);
            viewAttackRange.gameObject.SetActive(false);
        }

        /// <summary>
        /// 코루틴 매니저
        /// </summary>
        /// <param name="coroutine"></param>
        private void ActionCoroutine(IEnumerator coroutine)
        {
            if (currentCoroutine != null)
            {
                return; //아이템 중복 안되게함
                //StopCoroutine(currentCoroutine); //아이템 중복 되게함 //아이템 적용후 다른 아이템을 먹으면 버프가 안사라짐
            }
            currentCoroutine = coroutine;
            StartCoroutine(currentCoroutine);
        }

        /// <summary>
        /// 사거리가 증가합니다.
        /// </summary>
        /// <param name="upSize"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        private IEnumerator RangeUp(float upSize, float time)
        {
            Debug.Log("사정거리업 아이템 사용");
            float timer = 0f;
            ChangeMaxRange(GameManager.In.maxAttackRange * upSize);
            while(timer <= time)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            ChangeMaxRange(GameManager.In.maxAttackRange);
            currentCoroutine = null;
            Debug.Log("사정거리업 아이템 사용 종료");
        }

        /// <summary>
        /// 최대사거리가 늘어납니다.
        /// </summary>
        /// <param name="range"></param>
        private void ChangeMaxRange(float range)
        {
            currentMaxRange = range;
            viewAttackRange.ChangeViewRange(currentMaxRange);
            aim.CurrentMaxRange = currentMaxRange;
        }

        /// <summary>
        /// 스피드가 증가합니다.
        /// </summary>
        /// <param name="upSize"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        private IEnumerator SpeedUp(float upSize, float time)
        {
            Debug.Log("스피드업 아이템 사용");
            float timer = 0f;
            currentSpeed = GameManager.In.trebuchetMoveSpeed * upSize;
            while (timer <= time)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            currentSpeed = GameManager.In.trebuchetMoveSpeed;
            currentCoroutine = null;
            Debug.Log("스피드업 아이템 사용 종료");
        }

        /// <summary>
        /// 트레뷰셋이 비활성화됩니다.
        /// </summary>
        private void PlayerMode()
        {
            if (!isModeChanging)
            {
                Debug.Log("플레이어 모드로 전환합니다.");
                rigidbody2d.velocity = Vector2.zero;
                isPlayerMode = true;
                MoveOn();
            }
        }

        /// <summary>
        /// 트레뷰셋이 활성화됩니다.
        /// </summary>
        private void TrebuchetMode()
        {
            Debug.Log("트레뷰셋 모드로 전환합니다.");
            isPlayerMode = false;
        }

        private void TestActionMove()
        {
            if (!isInstall && !isPlayerMode)
            {
                horizontal = Input.GetAxis("Horizontal");
                vertical = Input.GetAxis("Vertical");
                rigidbody2d.velocity = new Vector2(horizontal, vertical) * currentSpeed;
            }
        }

        private void TestTemporary()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                UseItemSpeedUp(GameManager.In.buffedMoveSpeedAmount, GameManager.In.buffedMoveSpeedEffectTime);
            }

            if(Input.GetKeyDown(KeyCode.K))
            {
                UseItemRangeUp(1.5f, GameManager.In.buffedAttackRangeEffectTime);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isInstall && !isModeChanging)
                {
                    MoveOn();
                }
                else if(!isInstall)
                {
                    Install();
                }
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                if (isPlayerMode)
                {
                    TrebuchetMode();
                }
                else
                {
                    PlayerMode();
                }
            }
        }
        #endregion
    }
}
