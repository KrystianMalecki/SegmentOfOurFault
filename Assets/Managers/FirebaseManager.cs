using Firebase.Database;
using Firebase.Firestore;
using Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Firebase.Auth;
using Firebase.Extensions;
using System.Linq;

public class FirebaseManager : MonoBehaviour
{
    //singleton
    public static FirebaseManager instance;
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
        Init();
    }


    public static DatabaseReference databaseReference;
    public static FirebaseAuth auth;



    public void Init()
    {
        databaseReference = FirebaseDatabase.GetInstance("https://segmentofourfault-default-rtdb.europe-west1.firebasedatabase.app").RootReference;
        auth = FirebaseAuth.DefaultInstance;


    }


    public static readonly CustomYieldInstruction WaitFor1Second = new WaitForSecondsRealtime(1);
    public static readonly CustomYieldInstruction WaitFor1Minute = new WaitForSecondsRealtime(60);
    public readonly static string mail = "@game.com";

    #region user
    private static DatabaseReference databaseUsers => databaseReference.Child("users");
    private static FirebaseUser currentUser => auth.CurrentUser;
    private static DatabaseReference GetUser(string guid) => databaseUsers.GetList().ChildOrNew(guid);
    private static DatabaseReference GetCurrentUser() => GetUser(currentUser.UserId);
    public async Task<string> SetUserData(string guid, UserData userData) => await GetUser(guid).SetDataAsRawJson(userData);
    public async Task<string> SetCurrentUserData() => await GetCurrentUser().SetDataAsRawJson(UserManager.instance.currentUserData);

    public async Task<UserData> GetUserData(string guid) => await GetUser(guid).GetDataFromRawJson<UserData>();
    #endregion
    #region login
    public async Task<bool> Login(string login, string password)
    {
        FirebaseUser newUser = null;

        var value = await await auth.SignInWithEmailAndPasswordAsync(login + mail, password).ContinueWith(async task =>
          {

              if (task.IsCanceled)
              {
                  Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                  return false;
              }
              if (task.IsFaulted)
              {
                  Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                  return false;
              }
              FirebaseUser newUser = task.Result;

              Debug.LogFormat("User signed in successfully: {0} ({1})",
                  newUser.DisplayName, newUser.UserId);
              return true;
          });
        if (value)
        {
            UserManager.instance.currentUserData = await GetUserData(currentUser.UserId);
        }
        return value;

    }
    public async Task<bool> Register(string login, string password)
    {
        string guid = "";
        string userGuid = "";
        var value = await auth.CreateUserWithEmailAndPasswordAsync(login + mail, password).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                    return false;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    return false;
                }

                // Firebase user has been created.
                FirebaseUser newUser = task.Result;
                Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                    newUser.DisplayName, newUser.UserId);
                userGuid = newUser.UserId;






                return true;
            });
        if (!value)
        {
            return false;
        }
        Debug.Log("balls");
        Debug.Log("balls");

        CastleInsideData insideData = new CastleInsideData();

        guid = await SetCastleInsideData(null, insideData);
        insideData.guid = guid;
        await SetCastleInsideData(guid, insideData);

        Debug.Log("balls");

        var userData = new UserData();
        userData.name = login;
        userData.rankStatus = "the King";
        userData.level = 1;
        userData.castlesGuids.Add(guid);

        userData.guid = userGuid;
        await SetUserData(userGuid, userData);
        UserManager.instance.currentUserData = userData;
        Debug.Log("balls");

        await SetCastleInsideData(guid, insideData);
        CastleOnMapData mapData = new CastleOnMapData();
        mapData.guid = guid;
        mapData.name = "Castle of";
        mapData.ownerGuid = userData.guid;
        Debug.Log("balls");

        mapData.position = new Vector2Int(UnityEngine.Random.Range(-3, 3), UnityEngine.Random.Range(-3, 3));

        await SetCastleOnMapData(guid, mapData);
        Debug.Log("balls");

        return value;
    }
    public void SignOut()
    {
        auth.SignOut();
    }
    #endregion
    #region castles
    private static DatabaseReference databaseCastlesGroup => databaseReference.Child("castles");

    #region CastleOnMap
    private static DatabaseReference GetCastleOnMap(string guid) => databaseCastlesMap.GetList().ChildOrNew(guid);

    private static DatabaseReference databaseCastlesMap => databaseCastlesGroup.Child("map");

    public async Task<string> SetCastleOnMapData(string guid, CastleOnMapData castleMap) => await GetCastleOnMap(guid).SetDataAsRawJson(castleMap);
    public async Task<CastleOnMapData> GetCastleOnMapData(string guid) => await GetCastleOnMap(guid).GetDataFromRawJson<CastleOnMapData>();

    public async Task<List<CastleOnMapData>> GetAllCastlesOnMapData() => await databaseCastlesMap.GetList().GetAllDataFromRawJson<CastleOnMapData>();
    #endregion
    #region CastleOnMap
    private static DatabaseReference GetCastleInside(string guid) => databaseCastlesInside.GetList().ChildOrNew(guid);

    private static DatabaseReference databaseCastlesInside => databaseCastlesGroup.Child("inside");

    public async Task<string> SetCastleInsideData(string guid, CastleInsideData castleInside) => await GetCastleInside(guid).SetDataAsRawJson(castleInside);
    public async Task<CastleInsideData> GetCastleInsideData(string guid) => await GetCastleInside(guid).GetDataFromRawJson<CastleInsideData>();

    #endregion

    #endregion
}

public static class FirebaseHelper
{
    public static readonly string listChild = "_list";
    public static DatabaseReference GetList(this DatabaseReference reference)
    {
        return reference.Child(listChild);
    }
    public static DatabaseReference GetHeader(this DatabaseReference reference)
    {
        return reference.Child(Header.headerChild);
    }
    public static Task<TResult> CatchErrors<TResult>(
         this Task<TResult> task,
         Func<Task<TResult>, TResult> onError = null
         )
    {
        if (task.IsFaulted || (task.IsCompleted && !task.IsCompletedSuccessfully))
        {
            //todo 7 handle error
            Debug.LogError($"Exception {task.Exception}");

            onError?.Invoke(task);
            return null;
        }

        return task;
    }
    public static async Task<string> SetDataAsRawJson<T>(this DatabaseReference databaseReference, T data)
    {
        string json = JsonUtility.ToJson(data);
        // Debug.Log(json);

        return await databaseReference.SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error setting data");
                return null;


            }
            // Debug.Log(databaseReference.Key);
            return databaseReference.Key;

        });
    }
    public static async Task<T> GetDataFromRawJson<T>(this DatabaseReference databaseReference)
    {
        return await databaseReference.GetValueAsync().CatchErrors().ContinueWith(task =>
        {
            DataSnapshot snapshot = task.Result;
            return JsonUtility.FromJson<T>(snapshot.GetRawJsonValue());
        });
    }
    public static async Task<List<T>> GetAllDataFromRawJson<T>(this DatabaseReference databaseReference)
    {
        return await databaseReference.GetValueAsync().CatchErrors().ContinueWith(task =>
          {
              var snapshot = task.Result;
              return snapshot.Children.ToList().ConvertAll(child => JsonUtility.FromJson<T>(child.GetRawJsonValue()));
          });
    }
    public static DatabaseReference ChildOrNew(this DatabaseReference databaseReference, string guid)
    {
        if (guid == null)
        {
            return databaseReference.Push();
        }
        else
        {
            return databaseReference.Child(guid);
        }
    }

    //todo 9 use it
    [Serializable]
    public class Header
    {
        public static readonly string headerChild = "_header";
        public static readonly string headerCountChild = "count";

        public int count;
        //todo 9 use it
        public static void UpdateHeader(DatabaseReference list)
        {
            list.GetHeader().GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Error getting header");
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    Header header = JsonUtility.FromJson<Header>(snapshot.GetRawJsonValue());
                    header.count++;
                    list.GetHeader().SetRawJsonValueAsync(JsonUtility.ToJson(header));
                }
            });

        }
    }

}
