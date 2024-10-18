using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Select : UIPopup
{
    public List<Stat> characters = new List<Stat>();
    public UI_Select_Beacon selectPos;

    public List<UI_Select_Slot> slots = new List<UI_Select_Slot>();
    public List<UI_Select_Beacon> beacons = new List<UI_Select_Beacon>();

    public Dictionary<UI_Select_Beacon, UI_Select_Slot> SelectDict = new Dictionary<UI_Select_Beacon, UI_Select_Slot>();

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        beacons = Managers.scene.map.transform.GetComponentsInChildren<UI_Select_Beacon>().ToList();
        
        if(Managers.data.GetDict.stats.Count > 0 )
        {
            characters = Managers.data.GetDict.stats.ToList();
        }

        else
        {
            characters = Managers.data.StatDict.stats.ToList();
        }
        
        for (int i = 0; i < characters.Count; i++)
        {
            UI_Select_Slot Selectslot = Managers.ui.MakeSubItem<UI_Select_Slot>(Util.FindChild<HorizontalLayoutGroup>(gameObject, "Content", true).transform);
            Selectslot.Init();

            Character character = Managers.resource.Load<Character>(characters[i].character, "Prefabs/Character/");
            character.Init(characters[i]);

            slots.Add(Selectslot);
            slots[i].playerPrefab = character;
            slots[i].image.sprite = character.Face;
            slots[i].text.text = character.stat.Level + "";
        }

        foreach (UI_Select_Beacon beacon in beacons)
        {
            beacon.Init();
            BindUIEvent(beacon.gameObject, (PointerEventData data) => { UISelectBeacon(beacon); }, Define.UIEvent.Click);
        }

        foreach (UI_Select_Slot slot in slots)
        {
            BindUIEvent(slot.gameObject, (PointerEventData data) => { UISelectSlot(slot); }, Define.UIEvent.Click);
        }
    }

    public void UISelectBeacon(UI_Select_Beacon beacon)
    {
        if (beacon == null)
        {
            return;
        }

        if(selectPos != null)
        {
            selectPos.UnSelected();
            selectPos = null;
        }

        selectPos = beacon;
        beacon.Selected();
    }

    public void UISelectSlot(UI_Select_Slot slot)
    {
        if (selectPos == null)
        {
            return;
        }
        
        if (!SelectDict.ContainsValue(slot)) //해당 캐릭터를 아직 생성하지 않았다면
        {   
            if (SelectDict.TryGetValue(selectPos, out UI_Select_Slot _slot))
            {   //해당 위치에 이미 캐릭터가 있다면
                //위치에 있는 캐릭터를 삭제
                _slot.UnSelected();
                SelectDict.Remove(selectPos);
            }

            //캐릭터 생성
            slot.Selected(selectPos.tile.Pos);
            SelectDict.Add(selectPos, slot);
        }

        else
        {
            slot.UnSelected();
            SelectDict.Remove(SelectDict.FirstOrDefault(x => x.Value == slot).Key);
        } //이미 생성했다면 삭제
    }

    public void StartButton()
    {
        if(SelectDict.Count == 0)
        {
            return;
        }

        foreach (UI_Select_Beacon beacon in beacons)
        {
            Destroy(beacon.gameObject);
        }

        Managers.scene.turn.GameStart();
        Managers.scene.map.GameStart();
        Managers.scene.turn.stateTurn = TurnManager.State.PlayerTurn;
        Managers.ui.ClosePopupUI();
    }
}
