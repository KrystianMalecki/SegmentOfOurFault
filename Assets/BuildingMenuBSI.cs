using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildingMenuBSI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI productionText;
    public TextMeshProUGUI upgradeCostText;

    public ResourcesDataBSI upgradeCost;
    [SerializeReference]
    public BuildingBSI building;
    public void Open()
    {
        gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);

    }
    public void SetData(BuildingBSI building)
    {
        this.building = building;
        nameText.text = building.GetName() + " Lvl: " + building.level.ToString();

        productionText.text = $"+{(building.level * 5)} {building.type.ToString()}/min";
        upgradeCost = BuildingBSI.GetCostForLevel(building.type, building.level + 1);
        upgradeCostText.text = $"Wood:{upgradeCost.wood}\nStone:{upgradeCost.stone}\nCoins:{upgradeCost.coins}\nFood:{upgradeCost.food}";
    }
    public void Upgrade()
    {
        if (InsideManager.instance.castleInsideData.insideData.resources.HasMoreOrEqualThan(upgradeCost))
        {
            InsideManager.instance.castleInsideData.insideData.resources.Subtract(upgradeCost);
            switch (building.type)
            {
                case BuildingTypeBSI.Wood:
                    {
                        InsideManager.instance.castleInsideData.insideData.woodBuildingLevel++;
                        break;
                    }

                case BuildingTypeBSI.Stone:
                    {
                        InsideManager.instance.castleInsideData.insideData.stoneBuildingLevel++;
                        break;
                    }
                case BuildingTypeBSI.Coins:
                    {
                        InsideManager.instance.castleInsideData.insideData.coinsBuildingLevel++;
                        break;
                    }
                case BuildingTypeBSI.Food:
                    {
                        InsideManager.instance.castleInsideData.insideData.foodBuildingLevel++;
                        break;
                    }
            }
            InsideManager.instance.castleInsideData.insideData._perMinute = null;
            building.level++;
            SetData(building);
            InsideManager.instance.SaveDataAsync();
        }


    }
    public void Toggle(bool state)
    {
        if (state)
        {
            Open();
        }
        else
        {
            Close();
        }
    }
}

