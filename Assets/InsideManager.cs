using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Firebase.Extensions;
using NaughtyAttributes;
using UnityEngine.UI;

public class InsideManager : MonoBehaviour
{
    //singleton
    public static InsideManager instance;
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
        selectedCastleGuid = SceneLoadingManager.instance.selectedCastleGuid;
        Init().ContinueWithOnMainThread(task => { InitCastle(); });

    }
    public TextMeshProUGUI castleNameText;
    public TextMeshProUGUI ownerNameText;

    public UserData ownerUserData;
    public bool isOwner;
    public CastleInsideData castleInsideData;
    public CastleOnMapData castleOnMapData;

    private string selectedCastleGuid;

    [Foldout("Resources Building")]
    public BuildingBSI woodBBSI;
    [Foldout("Resources Building")]
    public BuildingBSI foodBBSI;
    [Foldout("Resources Building")]
    public BuildingBSI coinsBBSI;
    [Foldout("Resources Building")]
    public BuildingBSI stoneBBSI;

    [Foldout("Resources Text")]
    public TextMeshProUGUI woodAmountText;
    [Foldout("Resources Text")]
    public TextMeshProUGUI woodProductionText;
    [Foldout("Resources Text")]
    public TextMeshProUGUI foodAmountText;
    [Foldout("Resources Text")]
    public TextMeshProUGUI foodProductionText;
    [Foldout("Resources Text")]
    public TextMeshProUGUI coinsAmountText;
    [Foldout("Resources Text")]
    public TextMeshProUGUI coinsProductionText;
    [Foldout("Resources Text")]
    public TextMeshProUGUI stoneAmountText;
    [Foldout("Resources Text")]
    public TextMeshProUGUI stoneProductionText;

    private BuildingBSI _selected;
    public BuildingMenuBSI buildingMenu;
    public Button upgradeButton;

    public BuildingBSI selected
    {
        get => _selected;
        set
        {

            _selected = value;

            ToggleButtons((_selected != null)/*, _selected?.?.ownerGuid != UserManager.instance.currentUserData.guid*/);
        }
    }
    public void ToggleButtons(bool toggle/*, bool enableAttack*/)
    {
        if (toggle)
        {
            //  buildingMenu.transform.position = _selected.transform.position;
            // attackButton.interactable = enableAttack;
            // defendButton.interactable = !enableAttack;
        }
        buildingMenu.Toggle(toggle);


    }

    public async Task Init()
    {
        castleInsideData = await FirebaseManager.instance.GetCastleInsideData(selectedCastleGuid);
        castleOnMapData = await FirebaseManager.instance.GetCastleOnMapData(selectedCastleGuid);
        ownerUserData = await FirebaseManager.instance.GetUserData(castleOnMapData.ownerGuid);
        isOwner = ownerUserData.guid == UserManager.instance.currentUserData.guid;
        Debug.Log($"{ownerUserData.guid == UserManager.instance.currentUserData.guid} {ownerUserData.guid} {UserManager.instance.currentUserData.guid}");
    }
    public void InitCastle()
    {

        ChangeCoins();
        ownerNameText.text = $"{ownerUserData.name}, {ownerUserData.rankStatus}";
        castleNameText.text = $"{castleOnMapData.name}";

        woodBBSI.level = castleInsideData.insideData.woodBuildingLevel;
        foodBBSI.level = castleInsideData.insideData.foodBuildingLevel;
        coinsBBSI.level = castleInsideData.insideData.coinsBuildingLevel;
        stoneBBSI.level = castleInsideData.insideData.stoneBuildingLevel;
        if (isOwner)
        {
            StartCoroutine(AddPerMinuteProduction());
        }
        else
        {
        }
        upgradeButton.gameObject.SetActive(isOwner);
        ChangeCoins();
        ChangeWood();
        ChangeStone();
        ChangeFood();
        ToggleButtons(false);
    }
    public void ChangeCoins(int? add = null, int? set = null)
    {
        castleInsideData.insideData.ChangeCoins(add, set);
        coinsAmountText.text = $"{castleInsideData.insideData.coins.ToString("0")}";
        coinsProductionText.text = $"+{castleInsideData.insideData.perMinute.coins.ToString("0")}/min";

    }
    public void ChangeWood(int? add = null, int? set = null)
    {
        castleInsideData.insideData.ChangeWood(add, set);
        woodAmountText.text = $"{castleInsideData.insideData.wood.ToString("0")}/{castleInsideData.insideData.maxAmountResources.wood.ToString("0")}";
        woodProductionText.text = $"+{castleInsideData.insideData.perMinute.wood.ToString("0")}/min";

    }
    public void ChangeFood(int? add = null, int? set = null)
    {
        castleInsideData.insideData.ChangeFood(add, set);
        foodAmountText.text = $"{castleInsideData.insideData.food.ToString("0")}/{castleInsideData.insideData.maxAmountResources.food.ToString("0")}";
        foodProductionText.text = $"+{castleInsideData.insideData.perMinute.food.ToString("0")}/min";

    }
    public void ChangeStone(int? add = null, int? set = null)
    {
        castleInsideData.insideData.ChangeStone(add, set);
        stoneAmountText.text = $"{castleInsideData.insideData.stone.ToString("0")}/{castleInsideData.insideData.maxAmountResources.stone.ToString("0")}";
        stoneProductionText.text = $"+{castleInsideData.insideData.perMinute.stone.ToString("0")}/min";

    }



    IEnumerator AddPerMinuteProduction()
    {
        yield return new WaitForSeconds(5);

        while (true)
        {

            ChangeCoins(add: castleInsideData.insideData.perMinute.coins);
            ChangeWood(add: castleInsideData.insideData.perMinute.wood);
            ChangeStone(add: castleInsideData.insideData.perMinute.stone);
            ChangeFood(add: castleInsideData.insideData.perMinute.food);
            //  Debug.Log("added");
            SaveDataAsync();
            yield return FirebaseManager.WaitFor1Second;
        }
    }
    public void SaveDataAsync()
    {
        FirebaseManager.instance.SetCastleInsideData(selectedCastleGuid, castleInsideData);

    }
    public void GoToMap()
    {
        SceneLoadingManager.instance.LoadScene(Scene.Map);
    }


}
