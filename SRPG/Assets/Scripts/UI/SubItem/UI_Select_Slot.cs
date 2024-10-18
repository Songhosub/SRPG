using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Select_Slot : UIBase
{
    public Character playerPrefab;
    public GameObject player;

    public Image image;
    public Text text;

    float clickTime;
    bool clickBool;

    UI_Details details;
    RectTransform rect;

    override public void Init()
    {
        image = Util.FindChild<Image>(gameObject, "Player");
        text = Util.FindChild<Text>(gameObject, "Level");

        BindUIEvent(gameObject, (PointerEventData data) => { ClickDown(); }, Define.UIEvent.Over);
        BindUIEvent(gameObject, (PointerEventData data) => { ClickUp(data); }, Define.UIEvent.Exit);

        rect = GetComponent<RectTransform>();
        rect.localScale = Vector3.one;
    }

    public void ClickDown()
    {
        clickTime = 0;
        clickBool = true;
    }
    
    public void ClickUp(PointerEventData data)
    {
        clickTime = 0;
        clickBool = false;

        if (details != null)
        {
            Managers.ui.ClosePopupUI();
            details = null;
        }
    }

    private void Update()
    {
        if (clickBool)
        {
            clickTime += Time.deltaTime;
            if (clickTime > 1)
            {
                details = Managers.ui.ShowPopupUI<UI_Details>();
                details.Setting(playerPrefab);
                details.SlotSetting(playerPrefab);
                clickBool = false;
            }
        }
    }

    public void Selected(Vector3 Pos)
    {
        player = Managers.resource.Instantiate(playerPrefab.Path);
        player.transform.position = Pos + (Vector3.up * 0.4f);
        GetComponent<Image>().color = Color.gray;
    }


    public void UnSelected()
    {
        Destroy(player);
        player = null;
        GetComponent<Image>().color = Color.white;
    }
}
