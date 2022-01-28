using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Setup : MonoBehaviour
{
    private void Awake()
    {
        /*
         * 初始化引擎
         * 初始化UI,Res
         * 打开SpawnPanel
         */
        GameObject root = new GameObject("GameRoot");
        root.AddComponent4go<GameEngine>()
            .DontDestoryOnLoad();

        
        UIManager.Instance.Init();
        ResMgr.Instance.Init();
        SelectMgr.Instance.Init();

        UIManager.Instance.Add("UI/MainScene/SpawnPanel/SpawnPanel", UILayer.Normal);
    }


}