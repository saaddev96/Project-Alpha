using System.Collections;
using UnityEngine;
using System;

public class Footsteps : MonoBehaviour
{
    [Header("Layer Target")]
    [SerializeField] private LayerMask groundMask;
    [Space]
    [Header("Terrain Texture and  Footsteps Sounds")]
    [SerializeField] private TerrainTextureSound[] terrainTextureSounds;
    [Space]
    [Header("Footsteps Sound Source")]
    [SerializeField] private AudioSource FootSoundSource;
    [Space]
    [Header("Footsteps Sound Settings")]
    [Range(0, 1)]
    [SerializeField] private float footStepsVolume;
    private FootstepsSounds footStepSound;
    public static event Action<Data> OnPlayerStepEvent;
    public static Texture ActiveTerrainTexture;
    private void Start()
    {

        StartCoroutine(GroundChecking());
    }



    IEnumerator GroundChecking()
    {
        while (true)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f, groundMask))
            {
                if (hit.collider.TryGetComponent<Terrain>(out Terrain terrain))
                {
                    yield return StartCoroutine(PlayFootStepsOnTerrain(terrain, hit.point));
                }
                else if (hit.collider.TryGetComponent<MeshRenderer>(out MeshRenderer meshRendrer))
                {
                    yield return StartCoroutine(PlayFootStepsOnMeshRendrer(meshRendrer));
                }
                
            }
            yield return null;
        }
    }

    IEnumerator PlayFootStepsOnTerrain(Terrain terrain, Vector3 hitPoint)
    {
        Vector3 terrainPos = terrain.transform.position;
        int mapx = Mathf.FloorToInt(((hitPoint.x - terrainPos.x) / terrain.terrainData.size.x) * terrain.terrainData.alphamapWidth);
        int mapz = Mathf.FloorToInt(((hitPoint.z - terrainPos.z) / terrain.terrainData.size.z) * terrain.terrainData.alphamapHeight);

        float[,,] splatmapData = terrain.terrainData.GetAlphamaps(mapx, mapz, 1, 1);

        int texIndex = 0;
        for (int i = 1; i < splatmapData.Length; i++)
        {
            if (splatmapData[0, 0, i] > splatmapData[0, 0, texIndex])
            {
                texIndex = i;
            }
        }
        foreach (var terrainTextureSound in terrainTextureSounds)
        {
            if (terrainTextureSound.albedo == terrain.terrainData.terrainLayers[texIndex].diffuseTexture)
            {
                footStepSound = terrainTextureSound.footStepSounds;
                ActiveTerrainTexture = terrainTextureSound.albedo;
                yield return null;
                break;
            }
        }
        yield return null;
    }
    IEnumerator PlayFootStepsOnMeshRendrer(MeshRenderer meshRendrer)
    {
        foreach(var terrainTextureSound in terrainTextureSounds)
        {
            if (terrainTextureSound.albedo == meshRendrer.material.GetTexture("_MainTex"))
            {
                footStepSound = terrainTextureSound.footStepSounds;
                ActiveTerrainTexture = terrainTextureSound.albedo;
                yield return null;
                break;
            }
        }
    }
    int soundIteration = 0;
    void PlayFootSteps()
    {
        if (footStepSound != null)
        {
            OnPlayerStepEvent.Invoke(new AudioData { clip=footStepSound.FootSteps[soundIteration],aSource= FootSoundSource,volume= footStepsVolume });
            if(footStepSound.FootSteps.Count-1 > soundIteration)
            {
                soundIteration++;
            }
            else
            {
                soundIteration = 0;
            }
        }
    }

    [System.Serializable]
    public class TerrainTextureSound
    {
        public Texture albedo;
        public FootstepsSounds footStepSounds;
    }
}
