using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ2020
{
    public class DrawLineRenderer : MonoBehaviour
    {
        [SerializeField] private Transform point1;
        [SerializeField] private Transform point2;
        [SerializeField] private Transform point3;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private float vertexCount = 12f;
        [SerializeField] private float point2Yposition = 2f;

        private List<Vector3> pointList = new List<Vector3>();

        public List<Vector3> AttackLine { get { return pointList; } }

        //public List<Vector3> AttackLine()
        //{
        //    List<Vector3> attackLine = new List<Vector3>();
        //    for (float ratio = 0; ratio <= 1; ratio += 1 / vertexCount)
        //    {
        //        var tangent1 = Vector3.Lerp(point1.position, point2.position, ratio);
        //        var tangent2 = Vector3.Lerp(point2.position, point3.position, ratio);
        //        var curve = Vector3.Lerp(tangent1, tangent2, ratio);
        //
        //        attackLine.Add(curve);
        //    }
        //    return attackLine;
        //}

        private void Update()
        {
            transform.localPosition = -point1.position;
            point2.transform.position = new Vector3((point1.position.x + point3.position.x) / 2, point2Yposition * 2, (point1.position.z + point3.position.z) / 2);

            pointList.Clear();
            for (float ratio = 0; ratio <= 1; ratio += 1 / vertexCount)
            {
                var tangent1 = Vector3.Lerp(point1.position, point2.position, ratio);
                var tangent2 = Vector3.Lerp(point2.position, point3.position, ratio);
                var curve = Vector3.Lerp(tangent1, tangent2, ratio);

                pointList.Add(curve);
            }

            lineRenderer.positionCount = pointList.Count;
            lineRenderer.SetPositions(pointList.ToArray());
        }
    }
}
