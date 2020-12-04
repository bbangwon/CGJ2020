using CGJ2020;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrebuchetTest : MonoBehaviour, IInputUnit
{
    public void Axis(Vector2 axis)
    {
        transform.Translate(axis);
    }

    public void OnAttack()
    {
        Debug.Log("공격!!");
    }

    public void OnTrebuchetChangeMode()
    {
        Debug.Log("공격/이동모드 변환");
    }
}
