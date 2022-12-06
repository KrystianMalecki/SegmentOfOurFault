using Firebase.Database;
using Firebase.Firestore;
using Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

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
        Init();
    }
    /* public static FirebaseFirestore database;
     public void Init()
     {

         database = FirebaseFirestore.DefaultInstance;
         GetUsers(() => { Debug.Log("after"); });
         AddUser(new UserData() { level = -1, name = "baller" });
     }
     public void GetUsers(Action afterSuccess = null)
     {

         List<UserData> users = new List<UserData>();
         database.Collection("users").GetSnapshotAsync().ContinueWith(task =>
         {
             Debug.Log("start task");
             if (task.IsFaulted)
             {
                 Debug.LogError("Error getting users");
             }
             else
             {
                 QuerySnapshot snapshot = task.Result;
                 Debug.Log("output" + snapshot.Documents);

                 foreach (DocumentSnapshot document in snapshot.Documents)
                 {
                     var dict = document.ToDictionary();


                     UserData user = UserDatafromDocument(document);
                     users.Add(user);
                     Debug.Log(user.ToString());
                 }


                 afterSuccess?.Invoke();

             }
         });
     }

     public static UserData UserDatafromDocument(DocumentSnapshot userDocument)
     {
         UserData userData = new UserData();
         userData.id = userDocument.Id;
         userData.name = userDocument.GetDataOrDefault<string>("name");
         userData.level = userDocument.GetDataOrDefault<int>("level");

         return userData;
     }
     public void AddUser(UserData user)
     {
         if (user.id != null)
         {
             database.Collection("users").Document(user.id).SetAsync(user);
         }
         else
         {

             database.Collection("users").AddAsync(user).ContinueWith(task =>
             {
                 if (task.IsFaulted)
                 {
                     Debug.LogError("Error adding user");
                 }
                 else
                 {
                     DocumentReference document = task.Result;
                     Debug.Log("Added user with ID: " + document.Id);
                     user.id = document.Id;
                     if (user.id == null)
                     {
                         Debug.Log("Error!");
                     }
                     AddUser(user);
                 }
             });

         }



     }*/
    public static DatabaseReference databaseReference;
    public static DatabaseReference databaseUsers => databaseReference.Child("users");
    //todo -1 remove
    public static DatabaseReference databaseLogins => databaseReference.Child("logins");

    public static readonly bool displayMoreInfo = false;
    public string guid = "9602a144-0888-4903-ab6e-ee023015d510";
    public void Init()
    {
        databaseReference = FirebaseDatabase.GetInstance("https://segmentofourfault-default-rtdb.europe-west1.firebasedatabase.app").RootReference;
        // var guid =  SetUserAsync(new UserData() { level = -1, name = "baller" });
        //  UpdateUser(guid, x => { Debug.Log(x); });
        // databaseUsers.GetList().Child(guid).ChildChanged += (sender, eventData) => { Debug.Log(eventData.Snapshot); };
    }
    /*public void AddOneToUser(string guid)
    {
        UpdateUser(guid, x => { x.level++; SetUserAsync(x); });
    }
    public void p()
    {
        AddOneToUser(guid);
    }*/
    /* public void AddUserAsync(UserData user,Action<UserData> onSuccess=null)
     {
         //todo 5 handler error
         if (user.id == null)
         {
             user.id = Guid.NewGuid().ToString();
         }
         databaseReference.GetList().Child(user.id).GetValueAsync().ContinueWith(task => {
             if (task.IsFaulted)
             {
                 Debug.LogError("Error adding user");
             }
             else
             {
                 if (task.Result.Exists)
                 {
                     Debug.Log("User already exists, trying new guid");
                     user.id = null;
                     AddUserAsync(user);
                 }
                 else
                 {
                     SetUserAsync(user);
                     onSuccess.Invoke(user);
                 }
             }
         }); 
     }*/
    /*
        public async Task<string> SetUserData(UserData user)
        {
            if (user.id == null)
            {
                user.id = Guid.NewGuid().ToString();
            }
            string json = JsonUtility.ToJson(user);
            if (displayMoreInfo) { Debug.Log("user to json: " + json); }
           await databaseUsers.GetList().Child(user.id).SetRawJsonValueAsync(json);
            return user.id;
        }*/
    /*public string SetUserAsync(UserData user)
    {
        if (user.id == null)
        {
            user.id = Guid.NewGuid().ToString();
        }
        string json = JsonUtility.ToJson(user);
        if (displayMoreInfo) { Debug.Log("user to json: " + json); }
        databaseUsers.GetList().Child(user.id).SetRawJsonValueAsync(json);
        return user.id;
    }
    */
    public async Task<bool> SetUserByGuid(string guid,UserData userData)
    {
        string json = JsonUtility.ToJson(userData);
        return await databaseUsers.GetList().Child(guid).SetRawJsonValueAsync(json).ContinueWithErrorHandling(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error getting user");
            }
            else if (task.IsCompleted)
            {
                return true;
            }
            return false;
        });
    }
    public async Task<UserData> GetUserByGuid(string guid)
    {
        return await databaseUsers.GetList().Child(guid).GetValueAsync().ContinueWithErrorHandling(task =>
          {
              if (task.IsFaulted)
              {
                  Debug.LogError("Error getting user");
              }
              else if (task.IsCompleted)
              {
                  DataSnapshot snapshot = task.Result;
                  if (displayMoreInfo) { Debug.Log("got user in json: " + snapshot.GetRawJsonValue()); }
                  return JsonUtility.FromJson<UserData>(snapshot.GetRawJsonValue());
              }
              return null;
          });
    }
    public async Task<string> GetGuid(string login, string password)
    {
        return await databaseLogins.GetList().Child(login).GetValueAsync().ContinueWithErrorHandling(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error getting guid");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string databasePassword = snapshot.Child("password").Value as string;
                if (databasePassword != password)
                {
                    return null;
                }
                return snapshot.Child("guid").Value as string;
            }
            return null;
        });
    }
    /*  public void UpdateUser(string guid, Action<UserData> onUpdate)
      {
          GetUserAsync(guid).ContinueWith(task =>
          {
              if (task.IsFaulted)
              {
                  Debug.LogError("Error updating user");
              }
              else if (task.IsCompleted)
              {
                  onUpdate.Invoke(task.Result);
              }
          });
      }*/
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
    public static Task<TNewResult> ContinueWithErrorHandling<TResult, TNewResult>(
        this Task<TResult> task,
        Func<Task<TResult>, TNewResult> onSuccess = null,
        Func<Task<TResult>, TNewResult> onError = null
        )
    {
        if (task.IsFaulted)
        {
            //todo 7 handle error
            onError?.Invoke(task);
            Debug.LogError("Error getting user");

        }
        else if (task.IsCompleted)
        {
            onSuccess?.Invoke(task);
        }
        return null;
    }
    public static Task<TNewResult> ContinueWithErrorHandling< TNewResult>(
        this Task task,
        Func<Task, TNewResult> onSuccess = null,
        Func<Task, TNewResult> onError = null
        )
    {
        if (task.IsFaulted)
        {
            //todo 7 handle error
            onError?.Invoke(task);
            Debug.LogError("Error getting user");

        }
        else if (task.IsCompleted)
        {
            onSuccess?.Invoke(task);
        }
        return null;
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
