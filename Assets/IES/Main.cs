using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Main : MonoBehaviour
{
    private IESLoader loader;
    public Light _light;

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        loader = gameObject.AddComponent<IESLoader>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            loader.Load(Application.dataPath + "/../IESProfile/1.IES", _light);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            loader.Load(Application.dataPath + "/../IESProfile/2.IES", _light);
        }
    }
}