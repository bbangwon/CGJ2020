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
            if (Is_Died || Stop) //이동 불가 상태이거나 죽었으면 Return
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
        public bool Is_Died; //죽었을 때
        public bool Stop;    //플레이어(기수) 이동 불가 상태
        private Vector2 Origin_Pos;

        private Animator animation;
        private Player m_player;
        public void Initialize_Bannerman()
        {
            Is_Died = false;
            transform.position = Origin_Pos;
        } //다시 플레이 할 수 있게 초기화

        private Vector2 dir = Vector2.zero;
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
            animation.SetBool("Died", Is_Died);
            if (Is_Died)
                animation.SetBool("Move", false);
            else
                animation.SetBool("Move", Is_Move);
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
            if (Easter_Die())
            {
                GameManager.In.Easter_Die();
                //특별 배너
                //모든 플레이어 죽이기
                //Is_Died = true;
            }

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
            else if (collision.CompareTag("Cannon"))
            {
                //죽는 모션 다르게 할 경우 넣어주는 구간
                Is_Died = true;
            } //탄환 맞았을 때 효능

              //else if (collision.CompareTag("Flame"))
              //{
              //    //죽는 모션 다르게 할 경우 넣어주는 구간
              //    Is_Died = true;
              //}
        }

        public void OnDie()
        {
            //죽었을때 애니메이션 연출 구현해주세요..
        }
    }
}