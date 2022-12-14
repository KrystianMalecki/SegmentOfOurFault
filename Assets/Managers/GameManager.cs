using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //singleton
    public static GameManager instance;
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
        dataStorage.loginSceneState = LoginSceneState.None;
        dataStorage.selectedCastleGuid = null;
        dataStorage.startFlag = false;
    }
    [Required]
    public DataStorageSO dataStorage;
    
}
