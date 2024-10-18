using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    int _order = 10;

    Stack<UIPopup> _popupStack = new Stack<UIPopup>();
    UIScene _sceneUI = null;

    Transform root 
    { 
        get 
        { 
            GameObject root = GameObject.Find("@UI_Root"); 
            if (root == null)
            {
                root = new GameObject { name = "@UI_Root" };
                DontDestroyOnLoad(root);
            }
            return root.transform;
        } 
    }

    public void Init()
    {
        if (GameObject.FindObjectOfType(typeof(EventSystem)) == null)
        {
            GameObject go = Managers.resource.Instantiate("UI/EventSystem", root);
            go.name = "@EventSystem";
        }
    }

    public T ShowPopupUI<T>(string name= null) where T : UIPopup
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }
        
        GameObject go = Managers.resource.Instantiate("UI/Popup/" + name, root);
        
        if (go != null)
        {
            T popup = Util.GetorAddComponent<T>(go);
            _popupStack.Push(popup);
            return popup;
        }
        
        return null;
    }

    public T ShowSceneUI<T>(string name = null) where T : UIScene
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }
        
        GameObject go = Managers.resource.Instantiate("UI/Scene/" + name, root);
        
        if (go != null)
        {
            T scene = Util.GetorAddComponent<T>(go);
            _sceneUI = scene;
            return scene;
        }
        
        return null;
    }


    public void ClosePopupUI()
    {
        if(_popupStack.TryPop(out UIPopup uiPopup))
        {
            Managers.resource.Destroy(uiPopup.gameObject);
            _order--;
        }
    }

    public void CloseAllPopupUI()
    {
        while(_popupStack.Count > 0)
        {
            ClosePopupUI();
        }
    }

    //안전장치 정도 쓰지 않을 확률이 높음
    public bool TryClosePopupUI(UIPopup popup)
    {
        if (!(_popupStack.TryPeek(out UIPopup uiPopup)))
        {
            return false;
        }
        
        if(uiPopup != popup)
        {
            return false;
        }

        return true;
    }

    public void SetCanvas(GameObject go, bool sort = true)
    {
        if (sort)
        {
            Canvas canvas = Util.GetorAddComponent<Canvas>(go);
            //이 경우에만 Sort Order가 가능
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.overrideSorting = true;

            CanvasScaler canvasScaler = Util.GetorAddComponent<CanvasScaler>(go);
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1920, 1080);
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;

            canvas.sortingOrder = _order;
            _order++;
        }
        
        else
        {
            Util.GetorAddComponent<Canvas>(go).sortingOrder = 0;
        }
    }

    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UIBase
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }
        
        GameObject go = Managers.resource.Instantiate($"UI/SubItem/{name}");
        
        if (go != null)
        {
            if(parent != null)
            {
                go.transform.SetParent(parent);
            }
            
            T subItem = Util.GetorAddComponent<T>(go);

            go.GetComponent<RectTransform>().localScale = Vector3.one;
            return subItem;
        }
        
        return null;
    }

    public void Clear()
    {
        CloseAllPopupUI();
        if(_sceneUI != null)
        {
            Destroy(_sceneUI.gameObject);
            _sceneUI = null;
        }
    }
}