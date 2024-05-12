using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{

    [SerializeField] private bool canHeadBob = true;
    [SerializeField] private Transform cameraCtrler = null;
    [SerializeField] private Transform HandsCtrlerLeft = null;
    [SerializeField] private Transform HandsCtrlerRight = null;
    [Header("Head Bob General Settings")]
    [SerializeField, Range(0, 100)] private float amplitudeX = 1f;
    [SerializeField, Range(0, 40)] private float freqX = 10f;
    [SerializeField, Range(0, 100)] private float amplitudeY = 1f;
    [SerializeField, Range(0, 40)] private float freqY = 10f;
    [SerializeField, Range(-1, 1)] private float InitRotation;
    private Vector3 startPos;
    private Quaternion LeftHandStartRot;
    private Quaternion RightHandStartRot;
    Footsteps footsteps;
    bool isSoundPlayed = false;
    bool isAds => PlayerStateMachine.instance.IsAdsing;
    private void Awake()
    {
        footsteps = GetComponent<Footsteps>();
        startPos = cameraCtrler.localPosition;
        LeftHandStartRot = HandsCtrlerLeft.localRotation;
        RightHandStartRot = HandsCtrlerRight.localRotation;
    }
    private Vector3 StepMotion(float speed)
    {
       
        Vector3 pos = Vector3.zero;
        float sin = Mathf.Sin(Time.time * freqY * speed);
        float cos = Mathf.Cos(Time.time * freqX * speed);
        PlayerFootSteps(cos);
        pos.y += sin * amplitudeY / 10000* speed;
        pos.x += cos * amplitudeX / 10000* speed;
        if (PlayerStateMachine.instance.IsAdsing)
        {
            pos /= 20;
        }
        return pos;
    }
    private Quaternion StepRotation(float speed)
    {
        Quaternion rot = Quaternion.identity;
        float sin = Mathf.Sin(Time.time * freqY * speed);
        float Rot = Mathf.Abs(speed) > 1 ? InitRotation : 0;
        rot.x += sin* amplitudeY / 500 * speed+ Rot;
        return rot;
    }

    void PlayerFootSteps(float cos)
    {
        if (Mathf.Abs(Mathf.RoundToInt(cos)) == 1 && !isSoundPlayed)
        {
            isSoundPlayed = true;
            footsteps.PlayFootSteps();
        }
        else if (Mathf.Abs(Mathf.RoundToInt(cos)) == 0)
        {
            isSoundPlayed = false;
        }
    }
    private Vector3 Focusing()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + cameraCtrler.position.y, transform.position.z);
         pos += cameraCtrler.forward * 20.0f;
        return pos;
    }
    private void Update()
    {
        ResetPos();
        ResetRot();
        cameraCtrler.LookAt(Focusing());
    }
    public void PlayMotion(float speed)
    {
        if (!canHeadBob) return;
        cameraCtrler.localPosition += StepMotion(speed);
        if (isAds) return;
        HandsCtrlerLeft.localRotation = StepRotation(speed);
        HandsCtrlerRight.localRotation = StepRotation(-speed);
    }
    public void ResetPos()
    {
        if (!canHeadBob) return;
        if (cameraCtrler.localPosition == startPos) return;
        cameraCtrler.localPosition = Vector3.Lerp(cameraCtrler.localPosition, startPos, 4* Time.deltaTime);
    }
    private void ResetRot()
    {
        if (HandsCtrlerLeft.localRotation == LeftHandStartRot && HandsCtrlerRight.localRotation == RightHandStartRot) return;
        HandsCtrlerLeft.localRotation = Quaternion.Lerp(HandsCtrlerLeft.localRotation, LeftHandStartRot, 4 * Time.deltaTime);
        HandsCtrlerRight.localRotation = Quaternion.Lerp(HandsCtrlerRight.localRotation, RightHandStartRot, 4 * Time.deltaTime);
    }
}
