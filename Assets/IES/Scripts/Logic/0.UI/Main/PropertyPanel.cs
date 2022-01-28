using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PropertyPanel : MonoBehaviour
{
    private Entity _entity;
    private GameObject _content;

    private EntityTransform _transform;
    private EntityLight _light;

    private void Awake()
    {
        _entity = SelectMgr.Instance.SelectedEntity;
        _content = transform.Find("SV_Prop/Viewport/Content").gameObject;

        // if (!_entity.GetComponent<Transform>().IsNull())
        // {
        ResMgr.Instance
            .GetInstance("UI/MainScene/PropertyPanel/Transform")
            .transform.SetParent(_content.transform);
        // }

        if (_entity.gameObject.Find<Light>(_entity.lightPath) != null)
        {
            ResMgr.Instance
                .GetInstance("UI/MainScene/PropertyPanel/Light")
                .transform.SetParent(_content.transform);
        }
    }

    private void Update()
    {
    }


}