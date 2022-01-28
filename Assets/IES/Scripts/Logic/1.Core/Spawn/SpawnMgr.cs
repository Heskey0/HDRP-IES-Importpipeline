using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 放置未Instantiate的GameObject
/// </summary>
public class SpawnMgr : Singleton<SpawnMgr>
{
    private int layer = 1 << 10;
    private static TimerModel timerTask;

    public bool isSpawning = false;
    public event Action OnSpawnedCallback = spawned;
    public event Action OnCanceledCallback = Canceled;

    public void Spawn(GameObject go, int typeid, int itemid)
    {
        if (isSpawning)
        {
            return;
        }
        UIManager.Instance.RemoveLayer(UILayer.Top);
        isSpawning = true;

        go.Instantiate()
            .AddComponent4go<SpawnItem>()
            .Layer(10);


        Ray ray;
        RaycastHit hit = new RaycastHit();

        timerTask = TimerMgr.Instance.CreateTimer(Time.deltaTime, -1, () =>
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 5000, ~layer))
            {
                SpawnItem.Go.transform.position = hit.point;
            }

            //左键放置
            if (Input.GetMouseButtonDown(0))
            {
                OnSpawnedCallback?.Invoke();
                OnSpawnedCallback = spawned;

                SpawnItem.Go.Layer(0)
                    .AddComponentGracefully<Entity>()
                    .Newid(typeid, itemid);
                GameObject.Destroy(SpawnItem.Go.GetComponent<SpawnItem>());
                SpawnItem.Go = null;
                isSpawning = false;
                timerTask.Stop();
            }

            //右键取消
            if (Input.GetMouseButtonDown(1))
            {
                OnCanceledCallback?.Invoke();
                OnCanceledCallback = Canceled;

                SpawnItem.Go
                    .Layer(0);
                GameObject.Destroy(SpawnItem.Go);
                SpawnItem.Go = null;
                isSpawning = false;
                timerTask.Stop();
            }
        });

        //被放置
        OnSpawnedCallback?.Invoke();

        timerTask.Start();
    }


    public static void spawned()
    {
        //SpawnItem.Go.AddComponent<Entity>();
    }

    public static void Canceled()
    {
    }
}