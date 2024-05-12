using UnityEngine.Rendering.Universal;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class Impact : MonoBehaviour
{
    [SerializeField] private float lifeTime =3.0f;
    [SerializeField] private DecalProjector decal;
    float counter=0;
    private void Update()
    {
        if (lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
        }
        else
        {
            StartCoroutine(EffectStartDecay());
        }
    }

    IEnumerator EffectStartDecay()
    {
        counter += Time.deltaTime;
        decal.fadeFactor = Mathf.Lerp(1, 0, counter);
         yield return new WaitUntil(() => decal.fadeFactor==0);
        this.gameObject.SetActive(false);
    }

    //TODO : Impact Should Spawn From Object Pool
}
