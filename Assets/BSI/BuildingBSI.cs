using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBSI : MonoBehaviour
{
    public int level;
    public BuildingTypeBSI type;

    public void OnMouseDown()
    {
        Debug.Log($"Clicked on {name}");

        InsideManager.instance.selected?.Deselect();


        if (InsideManager.instance.selected == this)
        {
            InsideManager.instance.selected = null;
        }
        else
        {
            InsideManager.instance.selected = this;
            Select();
        }
    }
    public void Select()
    {
        InsideManager.instance.buildingMenu.SetData(this);
    }
    public void Deselect()
    {

    }


    public static ResourcesDataBSI GetCostForLevel(BuildingTypeBSI type, int level)
    {
        ResourcesDataBSI resources = type switch
        {

            BuildingTypeBSI.MainBase => new ResourcesDataBSI
            {
                coins = 1000,
                wood = 1000,
                stone = 1000,
                food = 1000
            },
            BuildingTypeBSI.Food => new ResourcesDataBSI
            {
                coins = 0,
                wood = 100,
                stone = 100,
                food = 50
            },
            BuildingTypeBSI.Stone => new ResourcesDataBSI
            {
                coins = 0,
                wood = 100,
                stone = 100,
                food = 50
            },
            BuildingTypeBSI.Wood => new ResourcesDataBSI
            {
                coins = 0,
                wood = 100,
                stone = 100,
                food = 50
            },
            BuildingTypeBSI.Coins => new ResourcesDataBSI
            {
                coins = 0,
                wood = 20,
                stone = 20,
                food = 0
            },
            BuildingTypeBSI.Storage => new ResourcesDataBSI
            {
                coins = 100,
                wood = 500,
                stone = 500,
                food = 0
            },
            BuildingTypeBSI.Walls => new ResourcesDataBSI
            {
                coins = 20,
                wood = 50,
                stone = 200,
                food = 0
            },
            BuildingTypeBSI.None => new ResourcesDataBSI
            {
                coins = 0,
                wood = 0,
                stone = 0,
                food = 0
            },
            _ => throw new System.NotImplementedException(),
        };
        resources.food *= level;
        resources.stone *= level;
        resources.wood *= level;
        resources.food *= level;

        return resources;
    }


    public string GetName() => type switch
    {
        BuildingTypeBSI.None => throw new System.NotImplementedException(),
        BuildingTypeBSI.Wood => "Lumberjack",
        BuildingTypeBSI.Stone => "Stone quarry",
        BuildingTypeBSI.Coins => "Tax Collector",
        BuildingTypeBSI.Food => "Bakery",
        BuildingTypeBSI.Storage => "Storage",
        BuildingTypeBSI.Walls => "Walls",
        BuildingTypeBSI.MainBase => "Castle",
    };
}
public enum BuildingTypeBSI
{
    None,
    Wood,
    Stone,
    Coins,
    Food,
    Storage,
    Walls,
    MainBase
}
