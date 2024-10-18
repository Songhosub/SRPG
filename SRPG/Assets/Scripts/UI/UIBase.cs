using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class UIBase : MonoBehaviour
{
    IDictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    public abstract void Init();

    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);

        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];

        _objects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
            {
                objects[i] = Util.FindChild(gameObject, names[i], true);
            }
            else
            {
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);
            }
        }
    }

    //저장된 매칭시킨 파일에서 필요한 오브젝트를 가져오는 함수
    protected T Get<T>(int index) where T : UnityEngine.Object
    {
        if (_objects.TryGetValue(typeof(T), out UnityEngine.Object[] objects))
        {
            return objects[index] as T;
        }

        return null;
    }

    protected Text GetText(int index)
    {
        return Get<Text>(index);
    }

    protected Button GetButton(int index)
    {
        return Get<Button>(index);
    }

    protected Image GetImage(int index)
    {
        return Get<Image>(index);
    }

    protected GameObject GetGameObject(int index)
    {
        return Get<GameObject>(index);
    }

    public static void BindUIEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UIEventHandler evt = Util.GetorAddComponent<UIEventHandler>(go);

        switch (type)
        {
            case (Define.UIEvent.Click):
                evt.OnClickHandler = null;
                evt.OnClickHandler += action;
                break;
            case (Define.UIEvent.Drag):
                evt.OnDragHandler = null;
                evt.OnDragHandler += action;
                break;
            case (Define.UIEvent.Over):
                evt.OnOverHandler = null;
                evt.OnOverHandler += action;
                break;
            case (Define.UIEvent.Exit):
                evt.OnExitHandler = null;
                evt.OnExitHandler += action;
                break;
        }
    }
}
