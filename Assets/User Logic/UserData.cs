using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase.Firestore;
using Helpers;

[Serializable]
public class UserData
{
    public string guid = "";
    public string name = "";
    public string rankStatus = "";

    public int level = 1;
    public List<string> castlesGuids = new List<string>();
    public UserData()
    {

    }


    public override string ToString()
    {
        return this.ToJsonString();
    }



}
