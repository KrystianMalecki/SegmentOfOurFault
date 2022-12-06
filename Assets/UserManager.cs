using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    //singleton
    public static UserManager instance;
    public static UserData currentUser;
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
        if (PlayerPrefs.HasKey("login"))
        {

        }
    }
    public async Task<bool> Login(string name, string login, string password)
    {
        PlayerPrefs.SetString("login", login);
        PlayerPrefs.SetString("password", password);
        string guid = await FirebaseManager.instance.GetGuid(login, password).ContinueWithErrorHandling(guid => { return guid; }).Result;
        if (guid == null)
        {
            currentUser = new UserData();
            currentUser.name = name;
            //set id to hash of login
            currentUser.id = login + login.GetHashCode().ToString();

            bool successSet = await FirebaseManager.instance.SetUserByGuid(guid, currentUser);


        }
        else
        {
            currentUser = await FirebaseManager.instance.GetUserByGuid(guid).ContinueWithErrorHandling(user =>
            {
                return user;
            }).Result;
        }
        if (currentUser == null)
        {
        }

        return true;
    }
}
