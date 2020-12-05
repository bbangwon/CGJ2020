using CGJ2020;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Controller : MonoBehaviour
{
    public bool State_SpeedUp;
    public bool State_RangeUp;
    public bool State_Fireball;
    public GameObject SpeedUp_Effect;
    public GameObject RangeUp_Effect;

    private void Awake()
    {
        SpeedUp_Effect.SetActive(false);
        //RangeUp_Effect.SetActive(false);
    }

    public void SpeedUp()
    {
        State_SpeedUp = true;
        SpeedUp_Effect.SetActive(true);
        StartCoroutine(Speedup());
    }
    public void RangeUp()
    {
        State_RangeUp = true;
        //RangeUp_Effect.SetActive(true);
        StartCoroutine(Rangeup());
    }
    public void GetFireball()
    {
        State_Fireball = true;
    }
    public void UseFireball()
    { 
        //파이어볼로 전환
        State_Fireball = false;
    }
    private IEnumerator Speedup()
    {
        yield return new WaitForSeconds(GameManager.In.buffedMoveSpeedEffectTime);
        State_SpeedUp = false;
        SpeedUp_Effect.SetActive(false);
    }
    private IEnumerator Rangeup()
    {
        yield return new WaitForSeconds(GameManager.In.buffedAttackRangeEffectTime);
        State_RangeUp = false;
        //RangeUp_Effect.SetActive(false);
    }
}
