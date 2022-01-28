using System;
using System.Globalization;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class EntityTransform : MonoBehaviour
{
    private Entity _entity;
    private Transform _transform;

    private InputField _ifPosX;
    private InputField _ifPosY;
    private InputField _ifPosZ;
    private InputField _ifRotationX;
    private InputField _ifRotationY;
    private InputField _ifRotationZ;
    private InputField _ifScaleX;
    private InputField _ifScaleY;
    private InputField _ifScaleZ;

    private Button _btnRespawn;
    private Button _btnDestory;

    private void Start()
    {
        /******************************  Init  ******************************/

        #region Init

        _entity = SelectMgr.Instance.SelectedEntity;
        _transform = _entity.transform;

        _ifPosX = gameObject.Find<InputField>("Pos/IF_X");
        _ifPosY = gameObject.Find<InputField>("Pos/IF_Y");
        _ifPosZ = gameObject.Find<InputField>("Pos/IF_Z");
        _ifRotationX = gameObject.Find<InputField>("Rotation/IF_X");
        _ifRotationY = gameObject.Find<InputField>("Rotation/IF_Y");
        _ifRotationZ = gameObject.Find<InputField>("Rotation/IF_Z");
        _ifScaleX = gameObject.Find<InputField>("Scale/IF_X");
        _ifScaleY = gameObject.Find<InputField>("Scale/IF_Y");
        _ifScaleZ = gameObject.Find<InputField>("Scale/IF_Z");

        _btnRespawn = gameObject.Find<Button>("btnRespawn");
        _btnDestory = gameObject.Find<Button>("btnDestroy");


        var position = _transform.position;
        _ifPosX.text = position.x.ToString(CultureInfo.InvariantCulture);
        _ifPosY.text = position.y.ToString(CultureInfo.InvariantCulture);
        _ifPosZ.text = position.z.ToString(CultureInfo.InvariantCulture);
        var rotation = _transform.eulerAngles;
        _ifRotationX.text = rotation.x.ToString(CultureInfo.InvariantCulture);
        _ifRotationY.text = rotation.y.ToString(CultureInfo.InvariantCulture);
        _ifRotationZ.text = rotation.z.ToString(CultureInfo.InvariantCulture);
        var scale = _transform.localScale;
        _ifScaleX.text = scale.x.ToString(CultureInfo.InvariantCulture);
        _ifScaleY.text = scale.y.ToString(CultureInfo.InvariantCulture);
        _ifScaleZ.text = scale.z.ToString(CultureInfo.InvariantCulture);

        #endregion

        /******************************  Listener  ******************************/

        #region Listener

        /******************************  position  ******************************/
        _ifPosX.onEndEdit.AddListener(value =>
        {
            
            float result;
            if (!float.TryParse(value, out result))
            {
                _ifPosX.text = position.x.ToString(CultureInfo.InvariantCulture);
                return;
            }

            var pos = _transform.position;
            pos.x = result;
            _transform.position = pos;
        });
        _ifPosY.onEndEdit.AddListener(value =>
        {
            
            float result;
            if (!float.TryParse(value, out result))
            {
                _ifPosY.text = position.y.ToString(CultureInfo.InvariantCulture);
                return;
            }

            var pos = _transform.position;
            pos.y = result;
            _transform.position = pos;
        });
        _ifPosZ.onEndEdit.AddListener(value =>
        {
            
            float result;
            if (!float.TryParse(value, out result))
            {
                _ifPosZ.text = position.z.ToString(CultureInfo.InvariantCulture);
                return;
            }

            var pos = _transform.position;
            pos.z = result;
            _transform.position = pos;
        });

        /******************************  rotation  ******************************/
        _ifRotationX.onEndEdit.AddListener(value =>
        {
            
            float result;
            if (!float.TryParse(value, out result))
            {
                _ifRotationX = gameObject.Find<InputField>("Rotation/IF_X");
                return;
            }

            var rot = _transform.eulerAngles;
            rot.x = result;
            _transform.eulerAngles = rot;
        });
        _ifRotationY.onEndEdit.AddListener(value =>
        {
            
            float result;
            if (!float.TryParse(value, out result))
            {
                _ifRotationY = gameObject.Find<InputField>("Rotation/IF_Y");
                return;
            }

            var rot = _transform.eulerAngles;
            rot.y = result;
            _transform.eulerAngles = rot;
        });
        _ifRotationZ.onEndEdit.AddListener(value =>
        {
            
            float result;
            if (!float.TryParse(value, out result))
            {
                _ifRotationZ = gameObject.Find<InputField>("Rotation/IF_Z");
                return;
            }

            var rot = _transform.eulerAngles;
            rot.z = result;
            _transform.eulerAngles = rot;
        });

        /******************************  scale  ******************************/
        _ifScaleX.onEndEdit.AddListener(value =>
        {
            
            float result;
            if (!float.TryParse(value, out result))
            {
                _ifScaleX.text = scale.x.ToString(CultureInfo.InvariantCulture);
                return;
            }

            var sca = _transform.localScale;
            sca.x = result;
            _transform.localScale = sca;
        });
        _ifScaleY.onEndEdit.AddListener(value =>
        {
            
            float result;
            if (!float.TryParse(value, out result))
            {
                _ifScaleY.text = scale.y.ToString(CultureInfo.InvariantCulture);
                return;
            }

            var sca = _transform.localScale;
            sca.y = result;
            _transform.localScale = sca;
        });
        _ifScaleZ.onEndEdit.AddListener(value =>
        {
            
            float result;
            if (!float.TryParse(value, out result))
            {
                _ifScaleZ.text = scale.z.ToString(CultureInfo.InvariantCulture);
                return;
            }

            var sca = _transform.localScale;
            sca.z = result;
            _transform.localScale = sca;
        });
        
        /******************************  button  ******************************/
        _btnRespawn.onClick.AddListener(() =>
        {
            
            var go = _transform.gameObject;
            go.Name(SpawnTable.Instance[_entity.itemid].ItemName);
            GameObject.Destroy(go);

            SpawnMgr.Instance.Spawn(go, _entity.typeid, _entity.itemid);
        });
        _btnDestory.onClick.AddListener(() =>
        {
            
            GameObject.Destroy(_transform.gameObject);
            UIManager.Instance.RemoveLayer(UILayer.Top);
        });

        #endregion
    }
}