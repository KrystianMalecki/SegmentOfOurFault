using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Firebase.Extensions;
public class LoginSceneManager : MonoBehaviour
{

    public static LoginSceneManager instance;
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
        loginSceneState = SceneLoadingManager.instance.loginSceneState;
        SetupScene();


    }

    public TMP_InputField loginText;
    public TMP_InputField passwordText;
    public TextMeshProUGUI buttonText;
    public readonly static string loginButtonText = "Login";
    public readonly static string registerButtonText = "Register";

    [SerializeReference]
    public LoginSceneState loginSceneState = LoginSceneState.None;
    public void SetupScene()
    {
        buttonText.text = loginSceneState switch
        {
            LoginSceneState.Login => loginButtonText,
            LoginSceneState.Register => registerButtonText,
            _ => throw new System.NotImplementedException(),
        };
        SceneLoadingManager.instance.startFlag = true;
        if (PlayerPrefs.HasKey("login"))
        {
            loginText.text = PlayerPrefs.GetString("login");
            passwordText.text = PlayerPrefs.GetString("password");
        }
    }
    public void OnSubmitButtonClick()
    {
        SetupData(loginText.text, passwordText.text);
        if (loginSceneState == LoginSceneState.Login)
        {
            UserManager.instance.Login(loginText.text, passwordText.text).CatchErrors()?.ContinueWithOnMainThread(task =>
            {
                Debug.Log("login" + task.Result);

                if (task.Result)
                {
                    SceneLoadingManager.instance.LoadScene(Scene.Map);
                }

            });
        }
        else if (loginSceneState == LoginSceneState.Register)
        {
            UserManager.instance.Register(loginText.text, passwordText.text).CatchErrors()?.ContinueWithOnMainThread(task =>
            {
                Debug.Log("register" + task.Result);
                if (task.Result)
                {
                    SceneLoadingManager.instance.LoadScene(Scene.Map);
                }

            });

        }
    }
    private void SetupData(string login, string password)
    {
        PlayerPrefs.SetString("login", login);
        PlayerPrefs.SetString("password", password);
    }
}
