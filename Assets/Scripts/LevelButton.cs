using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    public int level;

    public void LoadLevel()
    {
        string levelStr = $"level{this.level}";
        if (IsSceneInBuildSettings(levelStr))
        {
            SceneManager.LoadScene(levelStr);
        }
        else 
        {
            Debug.Log($"chua co {levelStr}");
        }
    }

    private bool IsSceneInBuildSettings(string sceneName)
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;

        for (int i = 0; i < sceneCount; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);

            if (name == sceneName)
            {
                return true;
            }
        }

        return false;
    }
}
