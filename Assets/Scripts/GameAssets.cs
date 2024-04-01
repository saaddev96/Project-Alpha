using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _instance;

    public static GameAssets Instance
    {
        get
        {
            if (_instance == null) _instance = Instantiate(Resources.Load("GamesAssets") as GameObject).GetComponent<GameAssets>();
            return _instance;
        }
    }
    [Header("Player Related Sounds")]
    public GameSound PlayerSounds;
    [Space]
    [Header("FootSteps Related Sounds")]
    public FootstepsSounds ConcreteSound;
    public FootstepsSounds GrassSound;
    public FootstepsSounds MetalSound;
    public FootstepsSounds MudSound;
    public FootstepsSounds SnowSound;
    public FootstepsSounds WaterSound;
    public FootstepsSounds WoodSound;
}
