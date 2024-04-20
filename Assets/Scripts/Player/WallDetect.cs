using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
public class WallDetect : MonoBehaviour
{

    [SerializeField] private LayerMask wallMask;

    [SerializeField] private Camera FPS_Camera;
    [SerializeField] private Animator FPS_Animator;
    [SerializeField] private float maxDistance;
    [SerializeField] private float maxAngle;
    [SerializeField] private TwoBoneIKConstraint twoBoneIkConstraint;
    [SerializeField] private Transform leftHandTarget;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Vector3 leftHandOffset;
    //[SerializeField] private float handLerpSpeed;
    //[SerializeField] private float handStepDistance;
    //[SerializeField] private float handStepHeight;
    //[SerializeField] float stepLength;
    //Vector3 oldPostion;
    //Vector3 newPosition;
    Vector3 hitPoint;
    //Vector3 currentPosition;
    //float lerp;
    private float angle;
    bool isTouching = false;
    private RaycastHit hit;
    private PlayerStateMachine playerMovement;
    public bool canLeanOnWalll
    {
        get
        {
           
            if (Physics.Raycast(FPS_Camera.transform.position+(FPS_Camera.transform.right* leftHandOffset.x)+(FPS_Camera.transform.up * leftHandOffset.y), FPS_Camera.transform.forward, out hit, maxDistance, wallMask) )
            {
                angle = Vector3.Angle(-FPS_Camera.transform.forward, hit.normal);
               if(angle <= maxAngle)
                {
                    if (!isTouching)
                    {
                        hitPoint = hit.point;
                        StartTouching();
                      
                    }
                    return true;
                }
                else
                {
                    ExitTouching();
                    return false;
                }
                   
            }
            else
            {
                ExitTouching();
                return false;
            }

        }
    }
    private void Awake()
    {
        playerMovement = GetComponent<PlayerStateMachine>();
    }
    private void Start()
    {
        //lerp = 1;
    }
    private void Update()
    {
        OnTouching();
    }
    void StartTouching()
    {
       // currentPosition=oldPostion = newPosition = hitPoint;
        isTouching = true;
    }
    void OnTouching()
    {
        if (canLeanOnWalll && !playerMovement.IsPlayerMoving)
        {
            leftHandTarget.position = hitPoint;
            FPS_Animator.Play("WallTouch", 0);
            FPS_Animator.SetBool("isDetouch", false);
            twoBoneIkConstraint.weight = 1;

            /*if (Vector3.Distance(newPosition, FPS_Camera.transform.position) > handStepDistance)
            {
                lerp = 0;
                hitPoint = hit.point+(FPS_Camera.transform.right* stepLength);
                newPosition = hitPoint;
            }*/
        }
        /*if (lerp < 1)
        {
            Vector3 tempPosition = Vector3.Lerp(oldPostion, newPosition, lerp);
            tempPosition.z += -Mathf.Sin(lerp * Mathf.PI) * handStepHeight;
            currentPosition = tempPosition;
            lerp += Time.deltaTime * handLerpSpeed;
        }
        else
        {
            oldPostion = newPosition;
        }*/
    }
    void ExitTouching()
    {
        FPS_Animator.SetBool("isDetouch", true);
        twoBoneIkConstraint.weight = 0;
        isTouching = false;
    }

    /*bool isMoving()
    {
        return lerp < 1;
    }*/
}
