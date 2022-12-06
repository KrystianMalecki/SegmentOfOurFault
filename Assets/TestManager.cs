using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using System;

public class TestManager : MonoBehaviour
{
    public static TestManager instance;
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
    //tmpro text 
    public TextMeshProUGUI documentsList;
    //tmpro input
    public TMP_InputField input;
    public void Init()
    {

        /* db = FirebaseFirestore.DefaultInstance;
         CollectionReference usersRef = db.Collection("users");
         usersRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
         {
             QuerySnapshot snapshot = task.Result;
             foreach (DocumentSnapshot document in snapshot.Documents)
             {
                 Debug.Log(String.Format("User: {0}", document.Id));
                 Dictionary<string, object> documentDictionary = document.ToDictionary();
                 Debug.Log(String.Format("name: {0}", documentDictionary["name"]));
             }

             Debug.Log("Read all data from the users collection.");
         });*/
    }

}
