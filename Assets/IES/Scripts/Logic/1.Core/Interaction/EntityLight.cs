using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class EntityLight : MonoBehaviour
{
    private Light _light;

    private Toggle _toggleIsOn;
    private InputField _ifIntensity;

    private Button _btnImport;
    private InputField _ifPath;

    private void Start()
    {
        var entity = SelectMgr.Instance.SelectedEntity;
        _light = entity.gameObject.Find<Light>(entity.lightPath);
        //_light.type = LightType.Point;

        _toggleIsOn = gameObject.Find<Toggle>("ToggleIsOn");
        _ifIntensity = gameObject.Find<InputField>("IF_Intensity");
        _btnImport = gameObject.Find<Button>("btnImport");
        _ifPath = gameObject.Find<InputField>("IF_path");

        _toggleIsOn.isOn = _light.enabled;
        _ifIntensity.text = _light.intensity.ToString(CultureInfo.InvariantCulture);


        _toggleIsOn.onValueChanged.AddListener(isOn => { _light.enabled = isOn; });
        _ifIntensity.onEndEdit.AddListener(value =>
        {
            float result;
            if (!float.TryParse(value, out result))
            {
                _ifIntensity.text = _light.intensity.ToString(CultureInfo.InvariantCulture);
                return;
            }

            _light.intensity = result;
        });
        _btnImport.onClick.AddListener(() =>
        {
            IESLoader.Instance.Load(Application.dataPath + "/../IESProfile/" + _ifPath.text, _light);
        });
    }
}