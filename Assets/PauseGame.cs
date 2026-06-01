
using UnityEngine;
using UnityEditor;
public class PauseGame : MonoBehaviour
{
    void Update()
    {
#if UNITY_EDITOR
       
        if (Input.GetKeyDown(KeyCode.P)&& Application.isPlaying)
        { 
            Debug.Break();
        }
#endif
    }
}
