using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    private static Scene s_targetScene = Scene.MushroomsScene;

    public static void Load(Scene targetScene)
    {
        s_targetScene = targetScene;
        SceneManager.LoadSceneAsync(s_targetScene.ToString());
    }

    public static void LoaderCallback()
    {
        Load(s_targetScene);
    }
}
