using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CastleInsideBadStaticImplementation
{
    public ResourcesDataBSI resources;
    [System.NonSerialized]
    public ResourcesDataBSI _maxAmountResources = null;
    public ResourcesDataBSI maxAmountResources
    {
        get
        {
            if (_maxAmountResources == null)
            {
                _maxAmountResources = new ResourcesDataBSI();
                _maxAmountResources.stone = storageBuildingLevel * 1000;
                _maxAmountResources.wood = storageBuildingLevel * 1000;
                _maxAmountResources.food = storageBuildingLevel * 1000;
                _maxAmountResources.coins = -1;

            }
            return _maxAmountResources;
        }


    }
    [System.NonSerialized]
    public ResourcesDataBSI _perMinute = null;
    public ResourcesDataBSI perMinute
    {
        get
        {
            if (_perMinute == null)
            {
                _perMinute = new ResourcesDataBSI();
                _perMinute.stone = stoneBuildingLevel * 5;
                _perMinute.wood = woodBuildingLevel * 5;
                _perMinute.food = foodBuildingLevel * 5;
                _perMinute.coins = coinsBuildingLevel * 5;

            }
            return _perMinute;
        }


    }
    public int stoneBuildingLevel = 1;
    public int woodBuildingLevel = 1;
    public int foodBuildingLevel = 1;
    public int coinsBuildingLevel = 1;

    public int storageBuildingLevel = 1;

    public bool ChangeCoins(int? add = null, int? set = null) => ChangeValue(ref resources.coins, -1, add, set);
    public bool ChangeWood(int? add = null, int? set = null) => ChangeValue(ref resources.wood, maxAmountResources.wood, add, set);
    public bool ChangeStone(int? add = null, int? set = null) => ChangeValue(ref resources.stone, maxAmountResources.stone, add, set);
    public bool ChangeFood(int? add = null, int? set = null) => ChangeValue(ref resources.food, maxAmountResources.food, add, set);

    public decimal food => resources.food;
    public decimal wood => resources.wood;
    public decimal stone => resources.stone;
    public decimal coins => resources.coins;

    public bool ChangeValue(ref int variable, int maxAmount, int? add = null, int? set = null)
    {
        if (add != null)
        {
            variable += add.Value;
        }
        else if (set != null)
        {
            variable = set.Value;
        }
        if (variable > maxAmount && maxAmount != -1)
        {
            variable = maxAmount;
            return false;
        }
        return true;
    }
}

[System.Serializable]
public class ResourcesDataBSI
{
    public int coins;
    public int wood;
    public int stone;
    public int food;
    public bool HasMoreOrEqualThan(ResourcesDataBSI other)
    {
        if (coins < other.coins)
        {
            return false;
        }
        if (wood < other.wood)
        {
            return false;
        }
        if (stone < other.stone)
        {
            return false;
        }
        if (food < other.food)
        {
            return false;
        }
        return true;
    }
    public void Add(ResourcesDataBSI other)
    {
        coins += other.coins;
        wood += other.wood;
        stone += other.stone;
        food += other.food;
    }
    public void Subtract(ResourcesDataBSI other)
    {
        coins -= other.coins;
        wood -= other.wood;
        stone -= other.stone;
        food -= other.food;
    }
}

