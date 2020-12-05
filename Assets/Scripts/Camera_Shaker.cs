using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Shaker : MonoBehaviour
{
    static public Camera_Shaker instance;
     Vector3 Cam_Position;
     float Shaker_shake = 0f;
     float Shaker_shakeAmount = 0.7f;

    public void Camera_Shake(float time, float Str)
    {
        Shaker_shakeAmount = Str;
        Shaker_shake = time;
    }
     public void Shaker()
    {
        if (Shaker_shake > 0)
        {
            Debug.Log("SakeL");
            Camera.main.transform.position = Camera.main.transform.position + Random.insideUnitSphere * Shaker_shakeAmount;
            Shaker_shake -= Time.deltaTime;
        }
        else
        {
            Camera.main.transform.position = Vector3.Lerp(transform.position,Cam_Position,5);
            Shaker_shake = 0f;
        }
    }
    private void Update()
    {
        Shaker();
    }
    void Awake()
    {
        instance = this;
        Cam_Position = transform.position;
    }
}
