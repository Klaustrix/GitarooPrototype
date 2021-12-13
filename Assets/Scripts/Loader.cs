using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public static string currentScene;

    public enum Scene 
    {
        TitleScreen,
        LoadingScene,
        SongMenu,
        Gameplay,
        Results,
        Editor,
        _PrototypeScreen,
    }

    private static Action onLoaderCallback;

    public static void Load(Scene scene)
    {
        //Set the loader callback action to load the target scene
        onLoaderCallback = () => 
        {
            SceneManager.LoadScene(scene.ToString());
        };

        //Load the loading scene
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
        currentScene = Scene.LoadingScene.ToString();
    }

    public static void LoaderCallback()
    {
        //Triggered after the first Update which lets the screen refresh
        //Execute the loader callback action which will load the target scene
        if (onLoaderCallback != null)
        {
            onLoaderCallback();
            onLoaderCallback = null;
        }
    }
}
