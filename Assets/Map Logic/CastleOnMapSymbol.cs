using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CastleOnMapSymbol : MonoBehaviour
{
    public CastleOnMapData castleMapOnData;
    public SpriteRenderer flagSR;
    public void OnMouseDown()
    {

        MapManager.instance.selected?.Deselect();


        if (MapManager.instance.selected == this)
        {
            MapManager.instance.selected = null;
        }
        else
        {
            MapManager.instance.selected = this;
            Select();
        }
    }
    public void Init(CastleOnMapData data)
    {
        castleMapOnData = data;
        flagSR.color = data.banerColor;
    }

    public void Select()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
    }
    public void Deselect()
    {

    }
}
