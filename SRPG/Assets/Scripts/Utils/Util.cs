using UnityEngine;

public class Util
{
    //최상의 부모, 이름은 비교하지 않고 그 타입에만 해당하면 리턴(컴퍼넌트 이름), 재귀적으로 사용 지식만 찾으건지 자식의 자식도 찾을 것인지
    public static T FindChild<T> (GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
        {
            return null;
        }
        
        //직속 자식만
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
        
        //자식의 자식까지
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

    //컴포넌트가 아닌 게임 오브젝트를 찾을 경우
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