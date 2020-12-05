using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ2020
{
    public class TrebuchetAim : MonoBehaviour
    {
        [SerializeField,Range(0.005f,0.1f)] private float errorRange = 0.01f;

        private float speed;
        private float x2Y2;

        private float horizontal;
        private float vertical;

        private Rigidbody2D rigidbody2d;

        public float CurrentMaxRange { get; set; }

        private void Awake()
        {
            rigidbody2d = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            speed = GameManager.In.aimMoveSpeed;
            CurrentMaxRange = GameManager.In.maxAttackRange;
        }

        private void Update()
        {
            TestActionMove();
        }

        public void ActionMove(Vector2 axis)
        {
            if (CheckInRange())
            {
                rigidbody2d.velocity = new Vector2(axis.x, axis.y) * speed;
            }
            else
            {
                rigidbody2d.velocity = Vector2.zero;
            }
        }

        private void TestActionMove()
        {
            if (CheckInRange())
            {
                horizontal = Input.GetAxis("Horizontal");
                vertical = Input.GetAxis("Vertical");
                rigidbody2d.velocity = new Vector2(horizontal, vertical) * speed;
            }
            else
            {
                rigidbody2d.velocity = Vector2.zero;
            }
        }

        private bool CheckInRange()
        {
            float x = transform.localPosition.x;
            float y = transform.localPosition.y;
            float angle = Trigonometric.EulerAngle(transform.parent.transform.position, transform.position);

            x2Y2 = x * x + y * y;

            if (x2Y2 >= GameManager.In.minAttackRange * GameManager.In.minAttackRange &&
                x2Y2 <= CurrentMaxRange * CurrentMaxRange)
            {
                return true;
            }
            else if(x * x + y * y < GameManager.In.minAttackRange * GameManager.In.minAttackRange)
            {
                transform.localPosition = Trigonometric.CirclePoint2D(angle, GameManager.In.minAttackRange + errorRange);
                return false;
            }
            else
            {
                transform.localPosition = Trigonometric.CirclePoint2D(angle, CurrentMaxRange - errorRange);
                return false;
            }
            
        }
    }
}
