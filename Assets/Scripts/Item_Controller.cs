using CGJ2020;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Controller : MonoBehaviour
{
    public bool State_SpeedUp;
    public bool State_RangeUp;
    public bool State_Fireball;
    public GameObject[] SpeedUp_Effect = new GameObject[2];
    public GameObject RangeUp_Effect;

    private void Awake()
    {
        SpeedUp_Effect[0].SetActive(false);
        SpeedUp_Effect[1].SetActive(false);
        RangeUp_Effect.SetActive(false);
    }

    public void SpeedUp()
    {
        State_SpeedUp = true;
        SpeedUp_Effect[0].SetActive(true);
        SpeedUp_Effect[1].SetActive(true);
        StartCoroutine(Speedup());
    }
    public void RangeUp()
    {
        State_RangeUp = true;
        RangeUp_Effect.SetActive(true);
        StartCoroutine(Rangeup());
    }
    public void GetFireball()
    {
        State_Fireball = true;
    }
    public void UseFireball()
    { 
        State_Fireball = false;
    }
    private IEnumerator Speedup()
    {
        yield return new WaitForSeconds(GameManager.In.buffedMoveSpeedEffectTime);
        State_SpeedUp = false;
        SpeedUp_Effect[0].SetActive(false);
        SpeedUp_Effect[1].SetActive(false);
    }
    private IEnumerator Rangeup()
    {
        yield return new WaitForSeconds(GameManager.In.buffedAttackRangeEffectTime);
        State_RangeUp = false;
        RangeUp_Effect.SetActive(false);
    }
    private void Update()
    {
            if (transform.GetChild(0).localScale.x > 0)
            {
                SpeedUp_Effect[0].transform.localScale = Vector3.one;
            }
            else if (transform.GetChild(0).localScale.x < 0)
            {
                SpeedUp_Effect[0].transform.localScale = new Vector3(-1, 1, 1);
            }
        if (transform.GetChild(1).rotation.eulerAngles.y == 0)
        {
            SpeedUp_Effect[1].transform.localScale = Vector3.one;
            RangeUp_Effect.transform.localScale = Vector3.one;
            if (State_RangeUp && State_SpeedUp)
            {
                SpeedUp_Effect[1].transform.localPosition = new Vector2(0.25f, 0.45f);
                return;
            }
            else
                SpeedUp_Effect[1].transform.localPosition = new Vector2(0, 0.45f);
        }
        else if (transform.GetChild(1).rotation.eulerAngles.y > 0)
        {
            SpeedUp_Effect[1].transform.localScale = new Vector3(-1, 1, 1);
            RangeUp_Effect.transform.localScale = new Vector3(-1, 1, 1);
            if (State_RangeUp && State_SpeedUp)
            {
                SpeedUp_Effect[1].transform.localPosition = new Vector2(-0.25f, 0.45f);
                return;
            }
            else
                SpeedUp_Effect[1].transform.localPosition = new Vector2(0, 0.45f);
        }
    }
}
