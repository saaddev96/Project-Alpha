using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private bool debug = true;
    [SerializeField] private float bulletLifeTime =10f;
    [SerializeField] private GameObject impact;
    private float elapsedtime;
    private Vector3 hitPoint;
    private Vector3 nextPos;
    private Vector3 startPos;
    private Color debugColor;
    private Vector3 perviousPos;
    private bool hitSomething = false;
    private void Start()
    {
        if (debug)
        {
            startPos = transform.position;
            Vector3 nextPos = transform.position;
            debugColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        }
        perviousPos = transform.position;
    }

    private void Update()
    {
        CheckCollision(perviousPos);
        perviousPos = transform.position;
        Destroy(this.gameObject, bulletLifeTime);
    }

    void CheckCollision(Vector3 prevPos)
    {
        RaycastHit hit;
        Vector3 direction = transform.position - prevPos;
        Ray ray = new Ray(prevPos, direction);
        float dist = Vector3.Distance(transform.position, prevPos);
        if (Physics.Raycast(ray, out hit, dist) && !hitSomething)
        {
            hitPoint = hit.point;
            transform.position = hitPoint;
            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, hit.normal);
            Vector3 pos = hit.point;
            Instantiate(impact, pos, rot);
            hitSomething = true;
        }
    }

    private void OnDrawGizmos()
    {
        if (debug)
        {
            if (Application.isPlaying)
            {
                Gizmos.color = debugColor;
                if (hitSomething)
                {
                    Gizmos.DrawWireCube(startPos,new Vector3(0.1f, 0.1f, 0.1f));
                    Gizmos.DrawLine(startPos, hitPoint);
                    Gizmos.DrawWireSphere(hitPoint,0.04f);
                }
                else
                {
                    nextPos = transform.position;
                    Gizmos.DrawLine(startPos, nextPos);
                    Gizmos.DrawSphere(nextPos, 0.08f);
                    Gizmos.DrawWireCube(startPos, new Vector3(0.1f, 0.1f, 0.1f));

                }

            }
        }

    }
   
    // TODO Projectile Should Spawn From Object Pool
    
}
