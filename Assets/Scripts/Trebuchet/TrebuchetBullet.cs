using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ2020
{
    public class TrebuchetBullet : MonoBehaviour
    {
        [SerializeField] private Trebuchet trebuchet;

        private List<Vector3> paths = new List<Vector3>();
        private float pathTime;
        private float timer = 0f;
        private int count;

        private readonly float baseSpeed = 6.2f;

        public void SetBulletPaths(List<Vector3> path)
        {
            paths = path;
            pathTime = GameManager.In.attackTime / (paths.Count-1);
            count = 0;
        }

        private void OnEnable()
        {
            transform.position = paths[0];
        }

        private void Update()
        {
            if(count < paths.Count-1)
            {
                timer += Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, paths[count + 1],Time.deltaTime* baseSpeed);
                if(timer >= pathTime * Vector3.Distance(paths[count], paths[count + 1]))
                {
                    count++;
                    timer = 0f;
                    transform.position = paths[count];
                    if(count == paths.Count-1)
                    {
                        trebuchet.Attack(paths[count]);
                        gameObject.SetActive(false);
                        //transform.localScale = Vector3.zero;
                    }
                }
            }
        }
    }
}
