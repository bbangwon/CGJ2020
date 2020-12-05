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
            if (m_player.State == Player.States.Die || Stop) //이동 불가 상태이거나 죽었으면 Return
                return;
            Turn();
            float Speed = 0;
            if (m_player.item_Controller.State_SpeedUp)
                Speed = GameManager.In.flagmanMoveSpeed + GameManager.In.buffedMoveSpeedAmount;
            else
                Speed = GameManager.In.flagmanMoveSpeed;
            transform.Translate(axis * Speed);
        }
        public void OnAttack() { }
        public void OnTrebuchetChangeMode() { }
        public void SetPlayer(Player player) 
        {
            m_player = player;
        }

        public bool Stop;    //플레이어(기수) 이동 불가 상태
        private Vector2 Origin_Pos;

        private Animator animation;
        private Player m_player;
        public void Initialize_Bannerman()
        {
            //m_player.State = State.Alive;
            transform.position = Origin_Pos;
        } //다시 플레이 할 수 있게 위치 초기화

        private bool Is_Move
        {
            get
            {
                if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0 || Mathf.Abs(Input.GetAxis("Vertical")) > 0)
                    return
                        true;
                else
                    return
                        false;
            }
        } //애니메이션용 
        private void Refresh_Anim()
        {
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

        private void Turn()
        {
            if (Input.GetAxis("Horizontal") > 0)
                transform.localScale = Vector3.one;
            else if (Input.GetAxis("Horizontal") < 0)
                transform.localScale = new Vector3(-1, 1, 1);

        } //캐릭터 방향 바꾸기

        private void Awake()
        {
            animation = GetComponent<Animator>();
            Origin_Pos = transform.position;
        }

        private void Update()
        {
            Refresh_Anim();

            float x = Mathf.Clamp(transform.position.x, GameManager.In.screenViewRect.xMin + 0.5f, GameManager.In.screenViewRect.xMax - 0.5f);
            float y = Mathf.Clamp(transform.position.y, GameManager.In.screenViewRect.yMin + 0.5f, GameManager.In.screenViewRect.yMax - 0.5f);
            transform.position = new Vector2(x, y);

        }

        private List<Collider2D> easter = new List<Collider2D>();
        private Collider2D[] Easter()
        {
            easter.Clear();

            Vector2 originPos = transform.position;
            Collider2D[] hitedTargets = Physics2D.OverlapCircleAll(originPos, .5f);

            foreach (Collider2D hitedTarget in hitedTargets)
            {
                Vector2 targetPos = hitedTarget.transform.position;
                Vector2 dir = (targetPos - originPos).normalized;
                RaycastHit2D rayHitedTarget = Physics2D.Raycast(originPos, dir, 2);
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
        private bool Easter_Die()
        {
            if (Easter() == null)
                return false;
            int count = 0;
            for (int i = 0; i < Easter().Length; i++)
            {
                if (Easter()[i].CompareTag("Bannerman"))
                    count++;
            }
            if (count >= GameManager.In.PlayerCount)
                return true;
            else return false;
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
                collision.gameObject.SetActive(false);
            } //아이템 먹었을 때 효능
        }

        public void OnDie()
        {
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
                Stop = true;
            //선택해제 되었을때
        }
    }
}