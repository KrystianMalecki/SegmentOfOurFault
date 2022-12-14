using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void StartLogin()
    {
        SceneLoadingManager.instance.loginSceneState = LoginSceneState.Login;
        SceneLoadingManager.instance.LoadScene(Scene.Login);
    }
    public void StartRegister()
    {
        SceneLoadingManager.instance.loginSceneState = LoginSceneState.Register;
        SceneLoadingManager.instance.LoadScene(Scene.Login);

    }

}
