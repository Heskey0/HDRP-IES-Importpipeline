using UnityEngine;

public class Entity : MonoBehaviour
{
    public int typeid = 1;
    public int itemid = 1;
    public string lightPath;

    public void Newid(int newTypeid,int newItemid)
    {
        typeid = newTypeid;
        itemid = newItemid;
    }
 
    void Start()
    {
        lightPath = SpawnTable.Instance[itemid].LightPath;

    }
    
}