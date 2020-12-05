using CGJ2020;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CGJ2020
{
    public class Player_Bannerman : MonoBehaviour, IUnit
    {
        public void Axis(Vector2 axis)
        {
            if (m_player.State == Player.States.Die || Stop || GameManager.In.GameState == GameManager.GameStates.Over) //이동 불가 상태이거나 죽었으면 Return
                return;
            if (axis.x > 0)
                transform.localScale = Vector3.one;
            else if (axis.x < 0)
                transform.localScale = new Vector3(-1, 1, 1);
            if (m_player.item_Controller.State_SpeedUp)
            {
                animation.speed = 1.5f;
                rigidbody.velocity = new Vector2(axis.x, axis.y) * (GameManager.In.flagmanMoveSpeed * GameManager.In.buffedMoveSpeedAmount);
            }
            else
            {
                animation.speed = 1f;
                rigidbody.velocity = new Vector2(axis.x, axis.y) * GameManager.In.flagmanMoveSpeed;
            }

            Is_Move = axis != Vector2.zero;
        }
        public void OnAttack() { }
        public void OnTrebuchetChangeMode() { }
        public void SetPlayer(Player player) 
        {
            m_player = player;
            Change_Color();
        }
        private void Change_Color()
        {
            Debug.Log(m_player.PlayerNumber);
            Debug.Log(GameManager.In.playerColors.playerColors);
            Head_Color.color = GameManager.In.playerColors.playerColors[m_player.PlayerNumber];
        }
        public bool Stop;    //플레이어(기수) 이동 불가 상태
        private Vector2 Origin_Pos;
        private Collider2D collider;
        private Rigidbody2D rigidbody;
        private Animator animation;
        private Player m_player;
        [SerializeField] private SpriteRenderer Head_Color;
        public void Initialize_Bannerman()
        {
            //m_player.State = State.Alive;
            transform.position = Origin_Pos;
        } //다시 플레이 할 수 있게 위치 초기화

        private bool Is_Move = false;
         //애니메이션용 
        private void Refresh_Anim()
        {
            if (GameManager.In.GameState == GameManager.GameStates.Over)
            {
                rigidbody.velocity = Vector2.zero;
                animation.SetBool("Move", false);
                return;
            }
            if (m_player.State == Player.States.Die)
                animation.SetBool("Move", false);
            else
            {
                if(!Stop)
                    animation.SetBool("Move", Is_Move);
                else
                    animation.SetBool("Move", false);
            }
        } //애니메이션 작동
        private void Start()
        {

        }
        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            animation = GetComponent<Animator>();
            collider = GetComponent<Collider2D>();
            Origin_Pos = transform.position;
        }

        private void Update()
        {
            Refresh_Anim();
            Easter_Die();
            float x = Mathf.Clamp(transform.position.x, GameManager.In.screenViewRect.xMin + 0.5f, GameManager.In.screenViewRect.xMax - 0.5f);
            float y = Mathf.Clamp(transform.position.y, GameManager.In.screenViewRect.yMin + 0.5f, GameManager.In.screenViewRect.yMax - 0.5f);
            transform.position = new Vector2(x, y);

        }
        private List<Collider2D> easter = new List<Collider2D>();
        private Collider2D[] Easter()
        {
            easter.Clear();

            Vector2 originPos = transform.position;
            Collider2D[] hitedTargets = Physics2D.OverlapCircleAll(originPos, .1f);

            foreach (Collider2D hitedTarget in hitedTargets)
            {
                Vector2 targetPos = hitedTarget.transform.position;
                Vector2 dir = (targetPos - originPos).normalized;
                RaycastHit2D rayHitedTarget = Physics2D.Raycast(originPos, dir, .1f);
                if (rayHitedTarget)
                {
                    easter.Add(hitedTarget);
                }
            }
            if (easter.Count > 0)
            {
                return easter.ToArray();
            }
            else
                return null;
        }
        private void Easter_Die()
        {
            if (Easter() == null)
                return;
            int count = 0;
            for (int i = 0; i < Easter().Length; i++)
            {
                if (Easter()[i].CompareTag("Bannerman") )
                    count++;
            }
            if (count >= GameManager.In.AlivePlayerCount && GameManager.In.AlivePlayerCount > 1 && GameManager.In.GameState != GameManager.GameStates.Over)
            {

                GameManager.In.Easter_Die();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Item"))
            {
                Item_Info info = collision.GetComponent<Item_Info>();
                switch (info.item)
                {
                    case Item_Info.Item.Speed_Up:
                        if (m_player.item_Controller.State_SpeedUp)
                            break;
                        else
                        {
                            m_player.item_Controller.SpeedUp();
                        }
                        break;
                    case Item_Info.Item.Range_Up:
                        if (m_player.item_Controller.State_RangeUp)
                            break;
                        else
                            m_player.item_Controller.RangeUp();
                        break;
                    case Item_Info.Item.Fireball:
                        if (m_player.item_Controller.State_Fireball)
                            break;
                        else
                            m_player.item_Controller.GetFireball();
                        break;
                }
                //아이템 먹는 이펙트 넣어주는 구간

                //Destroy(collision.gameObject);
                GameManager.In.ItemDespawn(collision.gameObject);
            } //아이템 먹었을 때 효능
        }

        public void OnDie()
        {
            rigidbody.velocity = Vector2.zero;
            collider.enabled = false;
            animation.SetBool("Died", true);
            //죽었을때 애니메이션 연출 구현해주세요..
        }

        public void OnSelect()
        {
            if(Stop)
            Stop = false;
            //선택되었을때
        }

        public void OnDeselect()
        {
            if (!Stop)
            {
                rigidbody.velocity = Vector2.zero;
                  Stop = true;
            }
            //선택해제 되었을때
        }
    }
}