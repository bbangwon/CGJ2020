using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ2020
{
    public class DangerZone : MonoBehaviour
    {
        private float Duration;
        Animator animation;
        Collider2D collider;
        [SerializeField] private GameObject CannonEffect;
        [SerializeField] private GameObject FireEffect;
        public enum Types
        {
            NearAttack,
            Normal,
            Fireball
        }
        [SerializeField]private Player Own;

        private void Awake()
        {
            collider = GetComponent<Collider2D>();
        }
        public void Excute(Types type, Player sender)
        {
            Own = sender;
            switch (type)
            {
                case Types.Normal:
                    Camera_Shaker.instance.Camera_Shake(0.1f, 0.05f);
                    SetRadius(1); //공격 범위

                    {
                        //FireEffect.SetActive(false);
                        //animation = CannonEffect.GetComponent<Animator>();
                        //animation.SetBool("Fireball", false);
                        //CannonEffect.SetActive(true);
                        //Invoke("verdict_hit", 0.1f);
                        //StartCoroutine(DestroyAtEffectDuration());

                        StartCoroutine(DestroyADuration(0.1f)); //테스트용

                    }//포탄 이펙트 활성화 및 이펙트 꺼지면 파괴됨

                    break;
                case Types.Fireball:
                    Camera_Shaker.instance.Camera_Shake(0.1f, 0.05f);
                    SetRadius(1.5f); //공격 범위
                    {
                        //animation = CannonEffect.GetComponent<Animator>();
                        //animation.SetBool("Fireball", true);
                        //CannonEffect.SetActive(true);
                        //FireEffect.SetActive(true);
                        
                        StartCoroutine(DestroyADuration(GameManager.In.buffedFireballEffectTime));
                    }//화염 포탄 이펙트 활성화 및 이펙트 꺼지면 파괴됨
                    break;
                case Types.NearAttack:
                    SetRadius(0.5f); //공격 범위
                    {
                        //animation = CannonEffect.GetComponent<Animator>();
                        //animation.SetBool("Fireball", true);
                        //CannonEffect.SetActive(true);
                        //FireEffect.SetActive(true);

                        float durationTime = GameManager.In.trebuchetModeChangeTime / 2f;


                        StartCoroutine(AfterDurationAndDestroy(durationTime, durationTime));
                    }//화염 포탄 이펙트 활성화 및 이펙트 꺼지면 파괴됨
                    break;
                default:
                    break;
            }
            
        }
        private void SetRadius(float radius)
        {
            transform.localScale = new Vector2(radius, radius);

            var circle_collider = collider as CircleCollider2D;
            circle_collider.radius *= radius;
        }
        private void verdict_hit()
        {
            collider.enabled = false;
        }
        private IEnumerator DestroyAtEffectDuration() //이펙트지속시간 후에 Destroy
        {
            float timer = 0.0f;
            yield return new WaitForEndOfFrame();
            var stateInfo = animation.GetCurrentAnimatorStateInfo(0);
            timer = stateInfo.length;
            yield return new WaitForSeconds(timer);
            Destroy(gameObject);
        }
        private IEnumerator DestroyADuration(float time) //지속시간 후에 Destroy
        {
            yield return new WaitForSeconds(time);
            Destroy(gameObject);
        }

        IEnumerator AfterDurationAndDestroy(float afterDuration, float destroyDuration)
        {
            collider.enabled = false;
            yield return new WaitForSeconds(afterDuration);
            collider.enabled = true;
            yield return DestroyADuration(destroyDuration);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Player p = collision.GetComponentInParent<Player>();
            if (Own == p)
                return;
            if(collision.CompareTag("Bannerman"))
            {
                p.Die();
            }
        }

    } 
}
