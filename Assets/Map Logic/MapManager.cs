using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using Firebase.Extensions;
public class MapManager : MonoBehaviour
{
    public static MapManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public GameObject castleOnMapSymbolPrefab;
    public Transform castleOnMapParent;
    public ObjectPool<CastleOnMapSymbol> pool;

    public GameObject buttons;
    public GameObject info;

    public Button attackButton;
    public Button defendButton;

    public TextMeshProUGUI castleNameText;
    public TextMeshProUGUI ownerNameText;

    private CastleOnMapSymbol _selected;
    public CastleOnMapSymbol selected
    {
        get => _selected;
        set
        {

            _selected = value;
            if (_selected != null)
            {
                if (castleNameText != null)
                {
                    castleNameText.text = _selected.castleMapOnData.name;
                    FirebaseManager.instance.GetUserData(_selected.castleMapOnData.ownerGuid).ContinueWithOnMainThread<UserData>(task =>
                    {
                        ownerNameText.text = task.Result.name;
                    });
                }
            }
            else
            {
                if (castleNameText != null)
                {
                    castleNameText.text = "";
                    ownerNameText.text = "";
                }
            }
            ToggleButtons((_selected != null), _selected?.castleMapOnData?.ownerGuid != UserManager.instance.currentUserData.guid);

        }
    }
    public void Deselect()
    {
        selected = null;
    }
    public void ToggleButtons(bool toggle, bool enableAttack)
    {
        if (toggle)
        {
            buttons.transform.position = _selected.transform.position;
            //  attackButton.interactable = enableAttack;
            //  defendButton.interactable = !enableAttack;
        }
        buttons.SetActive(toggle);


    }

    public void Start()
    {
        selected = null;
        pool = new ObjectPool<CastleOnMapSymbol>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 10, 100);
        GenerateCastles();
        OnCloseInfo();
        ToggleButtons(false, false);
    }
    public Vector3 MapToWorldPositions(Vector2Int mapPosition)
    {
        //todo upgrade
        return new Vector3(mapPosition.x * 3, mapPosition.y * 3, 0);
    }
    public void MakeCastleOnMap(CastleOnMapData data)
    {
        var item = pool.Get();
        item.transform.position = MapToWorldPositions(data.position);
        item.name = data.name;
        item.Init(data);

    }
    CastleOnMapSymbol CreatePooledItem()
    {
        GameObject copy = Instantiate(castleOnMapSymbolPrefab, transform);

        var castleOnMapSymbol = copy.GetComponent<CastleOnMapSymbol>();
        return castleOnMapSymbol;
    }
    void OnTakeFromPool(CastleOnMapSymbol item)
    {
        item.gameObject.SetActive(true);
    }
    void OnReturnedToPool(CastleOnMapSymbol item)
    {
        item.gameObject.SetActive(false);
    }
    void OnDestroyPoolObject(CastleOnMapSymbol item)
    {
        Destroy(item.gameObject);
    }
    public async Task GenerateCastles()
    {
        List<CastleOnMapData> data = await FirebaseManager.instance.GetAllCastlesOnMapData(); ;
        for (int i = 0; i < data.Count; i++)
        {
            MakeCastleOnMap(data[i]);
        }
    }
    public void OpenCastle(CastleOnMapData symbol)
    {
        SceneLoadingManager.instance.selectedCastleGuid = symbol.guid;
        SceneLoadingManager.instance.LoadScene(Scene.Game);
    }
    public void OnOpenSelectedCastle()
    {
        OpenCastle(selected.castleMapOnData);
    }
    public void OnOpenInfo()
    {
        info.SetActive(true);
    }
    public void OnCloseInfo()
    {
        info.SetActive(false);
    }
    public void OnAttack()
    {
        OpenCastle(selected.castleMapOnData);
    }
    public void OnDefend()
    {
        OpenCastle(selected.castleMapOnData);
    }
}
