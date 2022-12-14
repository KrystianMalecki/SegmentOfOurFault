using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DataStorageSO", menuName = "Custom/DataStorageSO")]
public class DataStorageSO : ScriptableObject
{
    public LoginSceneState loginSceneState = LoginSceneState.None;
    public string selectedCastleGuid = null;
    public bool startFlag = false;

}
