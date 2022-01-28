using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class IESLoader : MonoBehaviour
{
    public static IESLoader Instance;
    private IESEngine engine = new IESEngine();
    private IESMetaData iesMetaData = new IESMetaData();


    private Texture cookieTextureCube = null;
    private Texture cookieTexture2D = null;

    private void Start()
    {
        Instance = this;
        engine.TextureGenerationType = TextureImporterType.Default;
    }

    public void Load(string path, Light light)
    {
        if (String.IsNullOrEmpty(path))
        {
            Debug.Log("路径为空");
            return;
        }
        if (!String.IsNullOrEmpty(engine.ReadFile(path)))
        {
            Debug.Log("无法读取ies");
            return;
        }

        //**********************************************************************//
        //-----------------------------  metaData  
        //**********************************************************************//

        iesMetaData.FileFormatVersion = null;
        iesMetaData.IESPhotometricType = engine.GetPhotometricType();
        iesMetaData.Manufacturer = engine.GetKeywordValue("MANUFAC");
        iesMetaData.LuminaireCatalogNumber = engine.GetKeywordValue("LUMCAT");
        iesMetaData.LuminaireDescription = engine.GetKeywordValue("LUMINAIRE");
        iesMetaData.LampCatalogNumber = engine.GetKeywordValue("LAMPCAT");
        iesMetaData.LampDescription = engine.GetKeywordValue("LAMP");

        (iesMetaData.IESMaximumIntensity, iesMetaData.IESMaximumIntensityUnit) = engine.GetMaximumIntensity();


        //**********************************************************************//
        //-----------------------------  获取IES贴图  
        //**********************************************************************//
        string warningMessage;
        //IES Cube贴图
        (warningMessage, cookieTextureCube) =
            engine.GenerateCubeCookie(iesMetaData.CookieCompression, (int) iesMetaData.iesSize);
        if (!string.IsNullOrEmpty(warningMessage))
        {
            Debug.Log("无法生成IES Cube贴图: " + warningMessage);
        }

        cookieTextureCube.IncrementUpdateCount();

        //IES 2D贴图
        (warningMessage, cookieTexture2D) = engine.Generate2DCookie(iesMetaData.CookieCompression,
            iesMetaData.SpotAngle, (int) iesMetaData.iesSize, iesMetaData.ApplyLightAttenuation);
        if (!string.IsNullOrEmpty(warningMessage))
        {
            Debug.Log("无法生成IES 2D贴图: " + warningMessage);
        }

        cookieTexture2D.IncrementUpdateCount();

        //**********************************************************************//
        //-----------------------------  Light  
        //**********************************************************************//
        //基本属性
        light.type = (iesMetaData.PrefabLightType == IESLightType.Point) ? LightType.Point : LightType.Spot;
        light.intensity = 1f;
        light.range = 10f;
        light.spotAngle = iesMetaData.SpotAngle;
        //HD数据
        var ies = (iesMetaData.PrefabLightType == IESLightType.Point) ? cookieTextureCube : cookieTexture2D;
        GameObject.Destroy(light.GetComponent<HDAdditionalLightData>());
        HDLightTypeAndShape hdLightTypeAndShape =
            (light.type == LightType.Point) ? HDLightTypeAndShape.Point : HDLightTypeAndShape.ConeSpot;
        HDAdditionalLightData hdLight = GameObjectExtension.AddHDLight(light.gameObject, hdLightTypeAndShape);
        if (iesMetaData.UseIESMaximumIntensity)
        {
            LightUnit lightUnit =
                (iesMetaData.IESMaximumIntensityUnit == "Lumens") ? LightUnit.Lumen : LightUnit.Candela;
            hdLight.SetIntensity(iesMetaData.IESMaximumIntensity, lightUnit);

            //
            Type type = hdLight.GetType();
            if (light.type == LightType.Point)
            {
                FieldInfo prop = type.GetField("m_IESPoint",BindingFlags.Instance | BindingFlags.NonPublic);
                try
                {
                    if (prop==null)
                    {
                        Debug.Log("********");
                    }
                    prop.SetValue(hdLight,ies);
                }
                catch (TargetInvocationException ex)
                {
                    Debug.Log(ex.InnerException);
                }
            }
            else
            {
                FieldInfo prop = type.GetField("m_IESSpot", BindingFlags.Instance | BindingFlags.NonPublic);
                try
                {
                    prop.SetValue(hdLight,ies);
                }
                catch (TargetInvocationException ex)
                {
                    Debug.Log(ex.InnerException);
                }
            }


            // if (light.type == LightType.Point)
            //     hdLight.IESPoint = ies;
            // else
            //     hdLight.IESSpot = ies;
        }
    }
}