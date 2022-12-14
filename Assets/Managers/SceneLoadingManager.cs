using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingManager : MonoBehaviour
{
    public static SceneLoadingManager instance;
    public static Scene loadedScene;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        if (!startFlag)
        {
            LoadScene(Scene.MainMenu);
        }
    }


    private void LoadSceneInternal(int sceneNumber)
    {

        SceneManager.LoadScene(sceneNumber, LoadSceneMode.Single);

    }
    public void LoadScene(Scene scene)
    {
        LoadSceneInternal((int)scene);
        loadedScene = scene;
    }
    public LoginSceneState loginSceneState
    {
        get => GameManager.instance.dataStorage.loginSceneState;
        set => GameManager.instance.dataStorage.loginSceneState = value;
    }
    public string selectedCastleGuid
    {
        get => GameManager.instance.dataStorage.selectedCastleGuid;
        set => GameManager.instance.dataStorage.selectedCastleGuid = value;
    }
    public bool startFlag
    {
        get => GameManager.instance.dataStorage.startFlag;
        set => GameManager.instance.dataStorage.startFlag = value;
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (loadedScene == Scene.MainMenu)
            {
                Application.Quit();
            }
            else
            {
                LoadScene(Scene.MainMenu);

            }
        }
    }
}
public enum Scene
{
    MainMenu = 0,
    LoadingScreen = 1,
    Login = 2,
    Game = 3,
    Map = 4,
}
public enum LoginSceneState
{
    None,
    Login,
    Register,
    Animatic
}
