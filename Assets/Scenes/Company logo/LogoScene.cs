using UnityEngine;

public class LogoScene : MonoBehaviour
{
    public void LoadStartMenu ()
    {
        LevelManager.levelManager.LoadLevel("Start");
    }
}
