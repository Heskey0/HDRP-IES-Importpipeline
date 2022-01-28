using System;
using UnityEngine;
using UnityEngine.UI;

public class SpawnPanel : MonoBehaviour
{
    private GameObject _typeContent;
    private GameObject _itemContent;

    private int _selectedType = 1;
    private int _selectedItem = 1;

    /******************************  RefreshType  ******************************/

    #region RefreshType

    public void RefreshType()
    {
        _typeContent.gameObject.DestroyAllChildren();

        int i = 1;
        foreach (var info in TypeTable.Instance.GetAll())
        {
            /*
             * 加载TypeItem并添加到content
             * 设置TypeItem的text，toggle
             */
            var typeItem = ResMgr.Instance.GetInstance("TypeItem").Name(info.Value.TypeName);
            typeItem.transform.SetParent(_typeContent.transform, false);
            var txtName = typeItem.transform.Find("Label").GetComponent<Text>();
            var toggle = typeItem.GetComponent<Toggle>();
            toggle.group = _typeContent.GetComponent<ToggleGroup>();
            txtName.text = info.Value.TypeName;

            //闭包绑定 索引 和 toggle
            var index = i;
            i++;
            toggle.isOn = false;
            toggle.onValueChanged.AddListener((isOn) => { onTypeToggleValueChanged(isOn, index); });
            if (i == 2) RefreshItem(1); //默认选中第一个
        }
    }

    private void onTypeToggleValueChanged(bool isOn, int typeIndex)
    {
        if (typeIndex == _selectedType || !isOn)
        {
            return;
        }

        //上一个按钮颜色还原
        var toggle = _typeContent.transform.Find(TypeTable.Instance[_selectedType].TypeName).GetComponent<Toggle>();
        var newColors = toggle.colors;
        newColors.normalColor = Color.yellow;
        toggle.colors = newColors;
        //当前按钮颜色改变
        toggle = _typeContent.transform.Find(TypeTable.Instance[typeIndex].TypeName).GetComponent<Toggle>();
        newColors = toggle.colors;
        newColors.normalColor = Color.white;
        toggle.colors = newColors;

        _selectedType = typeIndex;


        Debug.Log("refresh:" + typeIndex + ":" + TypeTable.Instance[typeIndex].TypeName);
        RefreshItem(typeIndex);
    }

    #endregion

    /******************************  RefreshItem  ******************************/

    #region RefreshItem

    public void RefreshItem(int typeIndex)
    {
        _itemContent.gameObject.DestroyAllChildren();
        int i = 0;
        foreach (var info in SpawnTable.Instance.GetAll())
        {
            /*
             * 加载Item并添加到content
             * 设置Item的sprite，toggle
             */

            if (info.Value.ItemType != TypeTable.Instance[typeIndex].TypeName)
            {
                i++;
                continue;
            }

            var itemName = info.Value.ItemName;
            var spawnItem = ResMgr.Instance.GetInstance("Item").Name(itemName);
            spawnItem.transform.SetParent(_itemContent.transform, false);

            var itemImg = spawnItem.transform.Find("ImgBg").GetComponent<Image>();
            var itemImgPath = info.Value.ImagePath;
            itemImg.sprite = ResMgr.Instance.GetResources<Sprite>(itemImgPath);
            var toggle = spawnItem.GetComponent<Toggle>();
            toggle.group = _itemContent.GetComponent<ToggleGroup>();

            //闭包绑定 索引 和 toggle
            i++;
            var index = i;
            toggle.isOn = false;
            toggle.onValueChanged.AddListener((isOn) => { onItemToggleValueChanged(isOn, typeIndex, index); });
        }
    }

    private void onItemToggleValueChanged(bool isOn, int typeid, int itemid)
    {
        if (!isOn)
        {
            return;
        }

        _selectedItem = itemid;

        Debug.Log("放置物体:" + SpawnTable.Instance[itemid].ItemName);

        var go = ResMgr.Instance.GetResources<GameObject>(SpawnTable.Instance[itemid].ModelPath);

        SpawnMgr.Instance.OnSpawnedCallback += () =>
        {
            try
            {
                _itemContent.transform.Find(SpawnTable.Instance[itemid].ItemName).GetComponent<Toggle>().isOn = false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        };
        SpawnMgr.Instance.Spawn(go, typeid, itemid);
    }

    #endregion


    /******************************  LifeTime  ******************************/

    #region LifeTime

    private void Awake()
    {
        _typeContent = transform.Find("SV_Type/Viewport/Content").gameObject;
        _itemContent = transform.Find("SV_Item/Viewport/Content").gameObject;

        RefreshType();
    }

    private void OnDestroy()
    {
    }

    #endregion
}