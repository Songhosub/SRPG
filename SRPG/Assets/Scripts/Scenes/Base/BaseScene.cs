using UnityEngine;

public abstract class BaseScene : MonoBehaviour
{
    public Define.Scene _sceneType { get; protected set; } = Define.Scene.Unknown;

    protected Transform root
    {
        get
        {
            GameObject root = GameObject.Find("@Scene_Root");
            if (root == null)
            {
                root = new GameObject { name = "@Scene_Root" };
                DontDestroyOnLoad(root);
            }
            return root.transform;
        }
    }

    protected virtual void Init()
    {
        Managers.scene.Init();

        gameObject.transform.parent = root;
    }

    //���⼭ ���� ������ ���� ���� ���̱⿡ abstract��
    public virtual void Clear()
    {
        Destroy(gameObject);
    }
}
