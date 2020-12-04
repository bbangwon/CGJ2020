using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ2020
{
    public class ViewRange : MonoBehaviour
    {
        [SerializeField] private float baseSize = 10f;
        [SerializeField] Transform viewMinRange;

        private void OnEnable()
        {
            viewMinRange.gameObject.SetActive(true);
        }

        private void Start()
        {
            viewMinRange.localScale = new Vector3(baseSize *GameManager.In.minAttackRange, baseSize * GameManager.In.minAttackRange, 1f);
            ChangeViewRange(GameManager.In.maxAttackRange);
        }

        private void OnDisable()
        {
            viewMinRange.gameObject.SetActive(false);
        }

        public void ChangeViewRange(float range)
        {
            transform.localScale = new Vector3(baseSize * range, baseSize * range, 1f);
        }
    }
}
