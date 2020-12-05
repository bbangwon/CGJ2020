using UnityEngine;

namespace CGJ2020
{
    public class TrebuchetTest : MonoBehaviour, IUnit
    {

        Player player;
        public void SetPlayer(Player player)
        {
            this.player = player;
        }

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



        void MyMoveSpeed()
        {
            float trebuchetMoveSpeed = GameManager.In.trebuchetMoveSpeed;
        }

        public void OnDie()
        {
            
        }

        public void OnSelect()
        {
            throw new System.NotImplementedException();
        }

        public void OnDeselect()
        {
            throw new System.NotImplementedException();
        }
    } 
}
