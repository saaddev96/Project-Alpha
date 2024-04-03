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

}
