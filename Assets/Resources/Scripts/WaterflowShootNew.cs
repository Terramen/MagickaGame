using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterflowShootNew : MonoBehaviour
{
    [SerializeField] private float defDistance = 5;
    public LineRenderer lineRenderer;
    public Transform firePoint;
    private Transform cTransform;
    public Transform endPoint;
    public Vector2 hitPoint;
    void Awake()
    {
        cTransform = GetComponent<Transform>();
    }

    void Start()
    {
    }

    void Update()
    {
        ShootWaterflow();
    }

    void ShootWaterflow()
    {

            RaycastHit2D hit = Physics2D.Raycast(firePoint.position, transform.up, 4f);
            hitPoint = hit.point;
            if (hit.collider != null)
            {
                Draw2DRay(firePoint.position, hit.point);
            }
            else
            {
                Draw2DRay(firePoint.position, endPoint.position);
            }

            
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(firePoint.position, endPoint.position);
    }*/

    void Draw2DRay(Vector2 startPos,Vector2 endPos)
    {
        lineRenderer.SetPosition(0,startPos);
        lineRenderer.SetPosition(1,endPos);
    }
}
