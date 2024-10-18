using UnityEngine;

public class Util
{
    //�ֻ��� �θ�, �̸��� ������ �ʰ� �� Ÿ�Կ��� �ش��ϸ� ����(���۳�Ʈ �̸�), ��������� ��� ���ĸ� ã������ �ڽ��� �ڽĵ� ã�� ������
    public static T FindChild<T> (GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
        {
            return null;
        }
        
        //���� �ڽĸ�
        if(recursive == false)
        {
            for(int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if(string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T compoenet = transform.GetComponent<T>();
                    if(compoenet != null)
                    {
                        return compoenet;
                    }
                }
            }
        }
        
        //�ڽ��� �ڽı���
        else
        {
            foreach(T component in go.GetComponentsInChildren<T>())
            {
                if(string.IsNullOrEmpty(name) || component.name == name)
                {
                    return component;
                }
            }
        }
        
        return null;
    }

    //������Ʈ�� �ƴ� ���� ������Ʈ�� ã�� ���
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        
        if (transform == null)
        {
            return null;
        }
        
        return transform.gameObject;
    }

    public static T GetorAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        if (!(go.TryGetComponent<T>(out T component)))
        {
            component = go.AddComponent<T>();
        }
        
        return component;
    }
}