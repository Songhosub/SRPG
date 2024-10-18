using UnityEngine;

public class ResourceManager
{
    //·¦ÇÎ
    public T Load<T>(string path, string folder = "") where T : Object
    {
        return Resources.Load<T>(folder + path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");
        if (prefab == null)
        {
            return null;
        }

        return Object.Instantiate(prefab, parent);
    }

    public void Destroy(GameObject original)
    {
        if(original == null)
        {
            return;
        }
        /*
        if (original.TryGetComponent<Poolable>(out Poolable poolable))
        {
            Managers.pool.Push(poolable);
            return;
        }
        */
        Object.Destroy(original);
    }
}