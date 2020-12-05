using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ2020
{
    public class DangerZone : MonoBehaviour
    {
        private float Duration;
        [SerializeField] private GameObject CannonEffect;
        [SerializeField] private GameObject FireballEffect;
        [SerializeField] private GameObject FireEffect;
        public enum Types
        {
            Normal,
            Fireball
        }
        Player Own;
        public void Excute(Types type, Player sender)
        {
            Own = sender;
            switch (type)
            {
                case Types.Normal:
                    SetRadius(1);
                    Duration = 0.1f;
                    //이펙트
                    CannonEffect.SetActive(true);
                    FireEffect.SetActive(false);
                    FireballEffect.SetActive(false);
                    CannonEffect.transform.parent = null;
                    Destroy(CannonEffect, 1f); //이펙트 시간에 맞게
                    break;
                case Types.Fireball:
                    SetRadius(1.5f);
                    Duration = GameManager.In.buffedFireballEffectTime;
                    //이펙트
                    CannonEffect.SetActive(false);
                    FireEffect.SetActive(true);
                    FireballEffect.SetActive(true);
                    CannonEffect.transform.parent = null;
                    Destroy(FireballEffect, 1f); //이펙트 시간에 맞게
                    CannonEffect.transform.parent = null;
                    Destroy(FireEffect, GameManager.In.buffedFireballEffectTime); //불 이펙트 지속시간
                    break;
                default:
                    break;
            }
            Destroy(gameObject, Duration);
        }
        private void SetRadius(float radius)
        {
            transform.localScale = new Vector2(radius, radius);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (Own == collision.GetComponent<Player>() && Own != null)
                return;
            if(collision.CompareTag("Bannerman"))
            {
                collision.GetComponentInParent<Player>().Die();
            }
        }

    } 
}
