using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;


public class SelectMgr : Singleton<SelectMgr>
{
    public Entity SelectedEntity;

    private TimerModel _timer;
    RaycastHit hit = new RaycastHit();

    public void Init()
    {
        _timer = TimerMgr.Instance.CreateTimer(0, -1, () =>
        {
            if (SpawnMgr.Instance.isSpawning)
            {
                return;
            }

            catchEntity();
        });

        _timer.Start();
    }

    public void Release()
    {
        _timer.Stop();
        _timer = null;
    }

    private void catchEntity()
    {
        /*
         * 捕获Ray击中的Entity
         */
        if (Input.GetMouseButtonDown(0))
        {
            if (CanvasCheck.hitUI)
            {
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                var tmpEntity = hit.transform.gameObject.GetComponent<Entity>();
                if (tmpEntity == null)
                {
                    return;
                }

                SelectedEntity = tmpEntity;
                Debug.Log("选中Entity:" + SelectedEntity.typeid + ":" + SelectedEntity.itemid);
                UIManager.Instance.Replace("UI/MainScene/PropertyPanel/PropertyPanel", UILayer.Top);
            }
        }
    }
}