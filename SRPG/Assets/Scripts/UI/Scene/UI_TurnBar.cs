using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_TurnBar : UIScene
{
    public enum GameObjects
    {
        TurnBar,
        Top,
        Middle,
        Bottom,
        Close,
        Panel,
        OK,
        Cancle
    }

    List<UI_Turn_Slot> slots = new List<UI_Turn_Slot>();
    TurnManager turn;

    public override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(GameObjects));

        turn = Managers.scene.turn;

        GetGameObject((int)GameObjects.TurnBar).SetActive(false);
        GetGameObject((int)GameObjects.Panel).SetActive(false);

        BindUIEvent(GetGameObject((int)GameObjects.Close), (PointerEventData data) => { GetGameObject((int)GameObjects.Panel).SetActive(true); }, Define.UIEvent.Click);
        BindUIEvent(GetGameObject((int)GameObjects.OK), (PointerEventData data) => { OKClick(); }, Define.UIEvent.Click);
        BindUIEvent(GetGameObject((int)GameObjects.Cancle), (PointerEventData data) => { GetGameObject((int)GameObjects.Panel).SetActive(false); }, Define.UIEvent.Click);
    }

    public void CreateBar(List<Character> characters)
    {
        GetGameObject((int)GameObjects.TurnBar).SetActive(true);

        for(int i =  0; i < characters.Count; i++)
        {
            if(i <= 0)
            {
                slots.Add(Managers.ui.MakeSubItem<UI_Turn_Slot>(GetGameObject((int)GameObjects.Top).transform));
                slots[i].CreateSlot(100);
                GetGameObject((int)GameObjects.Middle).transform.SetSiblingIndex(i+1);
            }

            else if (i <= 5)
            {
                slots.Add(Managers.ui.MakeSubItem<UI_Turn_Slot>(GetGameObject((int)GameObjects.Middle).transform));
                slots[i].CreateSlot(65);
                GetGameObject((int)GameObjects.Bottom).transform.SetSiblingIndex(i + 1);
            }

            else
            {
                slots.Add(Managers.ui.MakeSubItem<UI_Turn_Slot>(GetGameObject((int)GameObjects.Bottom).transform));
                slots[i].CreateSlot(0);
            }
        }
    }

    public void SlotsSet()
    {
        for(int i = 0; i < slots.Count; i++)
        {
            slots[i].SetSlot(turn._characters[i].Face, turn._characters[i].turnTime,
                turn.turnLength / turn._characters[i].stat.Agi);
        }
    }

    public void DestroySlot()
    {
        UI_Turn_Slot temp = slots[slots.Count - 1];
        slots.RemoveAt(slots.Count - 1);
        Destroy(temp.gameObject);
    }

    public void OKClick()
    {
        Managers.data.GetDict.stats.Clear();
        Managers.scene.map.GameOver(Define.GameoverType.Lose);
    }
}
