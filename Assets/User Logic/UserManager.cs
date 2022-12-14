using Helpers;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    //singleton
    public static UserManager instance;
    public UserData currentUserData = new UserData();
    [ReadOnly]
    public bool loggedIn = false;

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
        StartCoroutine(SaveData());
    }
    public async Task<bool> Login(string login, string password)
    {
        loggedIn = (await FirebaseManager.instance.Login(login, password));
        return loggedIn;
    }
    public async Task<bool> Register(string login, string password)
    {

        loggedIn = (await FirebaseManager.instance.Register(login, password));
        return loggedIn;
    }

    IEnumerator SaveData()
    {
        while (true)
        {
            if (currentUserData != null && loggedIn)
            {
                FirebaseManager.instance.SetCurrentUserData();
                Debug.Log(currentUserData.ToJsonString());
            }
            yield return FirebaseManager.WaitFor1Minute;
        }
    }

    public void OnApplicationQuit()
    {
        FirebaseManager.instance.SignOut();
    }
}
