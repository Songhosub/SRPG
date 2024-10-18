using System;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extension
{
    public static void BindUIEvent(this GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UIBase.BindUIEvent(go, action, type);
    }

    public static T GetorAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
        return Util.GetorAddComponent<T>(go);
    }
}