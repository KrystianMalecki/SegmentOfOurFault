using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase.Firestore;

[Serializable]
public class UserData
{
    public string id = null;
    public string name = null;
    public int level = 0;

    public override string ToString()
    {
        return JsonUtility.ToJson(this, true);
    }


}
