using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Battle : UIPopup
{
    public enum Images
    {
        //Pick
        PickFace,

        PickHealedHPBar,
        PickCurrentHPBar,
        PickDamagedHPBar,

        PickHealedMPBar,
        PickCurrentMPBar,
        PickDamagedMPBar,
                
        //Target
        TargetFace,

        TargetHealedHPBar,
        TargetCurrentHPBar,
        TargetDamagedHPBar,

        TargetHealedMPBar,
        TargetCurrentMPBar,
        TargetDamagedMPBar,              
    }

    public enum GameObjects
    {
        Skill1,
        Skill2,
        Skill3,
        Enter,
        Pick,
        Target,
        ErrorText,
        PickDetails,
        TargetDetails
    }

    float clickTime = 0;
    bool clickBool = false;

    List<GameObject> gameObjects = new List<GameObject> ();

    Character character;

    UI_Manual manual;
    string manual_Name;
    string manual_Effect;

    MapManager map;

    override public void Init()
    {
        base.Init();

        map = Managers.scene.map;

        character = map._pick;

        Bind<Image>(typeof(Images));
        Bind<GameObject>(typeof(GameObjects));

        //스킬들을 UI에 표시
        for (int i = 0; i < character.skills.Count; i++)
        {
            if(i < character.stat.UseSkillNum)
            {
                if (character.GetComponent<Player>() != null)
                {
                    BindEventSkill(GetGameObject(i), i);
                }
                character.skills[i].Init(character);
                GetGameObject(i).GetorAddComponent<Image>().sprite = character.skills[i].Icon;
                gameObjects.Add(GetGameObject(i));
            }
        }

        //Enter를 UI에 표시
        if (character.GetComponent<Player>() != null)
        {
            GetGameObject((int)GameObjects.Enter).GetorAddComponent<Image>().sprite = Managers.resource.Load<Sprite>("TurnEnd", "Sprites/UI/");
            BindUIEvent(GetGameObject((int)GameObjects.Enter), (PointerEventData data) => { map.End(); }, Define.UIEvent.Click);
        }
        //GetGameObject((int)GameObjects.Enter).GetorAddComponent<Image>().sprite = Managers.resource.Load<Sprite>("OK", "Sprites/UI/");

        //존재하지 않는 스킬칸이 표시되지 않도록 설정
        for (int i = 0; i < 3; i++)
        {
            if(!gameObjects.Contains(GetGameObject(i)))
            {
                GetGameObject(i).SetActive(false);
            }
        }

        if(character.state == Character.State.Idle)
        {
            GetGameObject((int)GameObjects.Enter).SetActive(false) ;
        }

        //에러문구 비활성화
        ErrorTextUnactive();
    }

    public void SkillGet(int index = 0)
    {
        if (character.state == Character.State.Idle || character.isMove || character.isAttack)
        {
            return;
        }

        if(character.MP < character.skills[index].mp)
        {
            ErrorTextActive("마나가 부족합니다.");
            return;
        }

        ClickUp();
        map.Attack(index);

        for (int i = 0; i < gameObjects.Count; i++)
        {
            UnbindEventSkill(gameObjects[i]);
            gameObjects[i].SetActive(false);
        }

        BindUIEvent(GetGameObject((int)GameObjects.Enter), (PointerEventData data) => { SkillRun(index); }, Define.UIEvent.Click);
        GetGameObject((int)GameObjects.Enter).GetorAddComponent<Image>().sprite = Managers.resource.Load<Sprite>("Action", "Sprites/UI/");

        gameObjects[index].SetActive(true);
        BindUIEvent(gameObjects[index], (PointerEventData data) => { CancleGet(); }, Define.UIEvent.Click);
        gameObjects[index].GetorAddComponent<Image>().sprite = Managers.resource.Load<Sprite>("Cancle", "Sprites/UI/");

        ErrorTextActive("대상을 선택해 주세요");

        map.CameraClear();
    }

    public void CancleGet()
    {
        map._pick = character;
        map.Cancle();

        for (int i = 0; i < gameObjects.Count; i++)
        {
            gameObjects[i].SetActive(true);
            BindEventSkill(gameObjects[i], i);
        }
    }

    public void BindEventSkill(GameObject go, int index = 0)
    {
        BindUIEvent(go, (PointerEventData data) => { SkillGet(index); }, Define.UIEvent.Click);
        BindUIEvent(go, (PointerEventData data) => { ClickDown(index); }, Define.UIEvent.Over);
        BindUIEvent(go, (PointerEventData data) => { ClickUp(); }, Define.UIEvent.Exit);
    }

    public void UnbindEventSkill(GameObject go)
    {
        BindUIEvent(go, null, Define.UIEvent.Click);
        BindUIEvent(go, null, Define.UIEvent.Over);
        BindUIEvent(go, null, Define.UIEvent.Exit);
    }

    public void SkillRun(int index = 0)
    {
        if (character.skills[index].hit == null || map._target ==  null || character.isMove || character.isAttack || character.state == Character.State.Idle)
        {
            return;
        }

        StartCoroutine(Run(index));
    }

    IEnumerator Run(int index)
    {
        character.isAttack = true;

        Managers.effect.Play(Enum.GetName(typeof(Define.WeaponType), character.stat.weaponType),
            character.skills[index].Name, character.CurrentDir, character.skills[index].Pos(map._target));
        Managers.sound.Play(character.skills[index].Name, Define.Sound.Effect, Enum.GetName(typeof(Define.WeaponType), character.stat.weaponType));
        yield return new WaitForSeconds(Managers.effect.Delay());

        Managers.scene.map.OK();
        yield return new WaitForSeconds(0.5f);

        character.isAttack = false;
        Managers.effect.End();
        map.End();
    }

    public void EndGet()
    {
        //BindUIEvent(GetButton((int)Buttons.End).gameObject, (PointerEventData data) => { map.End(); }, Define.UIEvent.Click);
    }

    public void ClickDown(int index)
    {
        manual_Name = character.skills[index].ManualName();
        manual_Effect = character.skills[index].Manual(character.stat.Atk);
        clickTime = 0;
        clickBool = true;
    }

    public void ClickUp()
    {
        clickTime = 0;
        clickBool= false;

        if(manual != null)
        {
            Managers.ui.ClosePopupUI();
            manual = null;
        }
    }

    private void Update()
    {
        if (clickBool)
        {
            clickTime += Time.deltaTime;
            if (clickTime > 1)
            {
                manual = Managers.ui.ShowPopupUI<UI_Manual>();
                manual.Init(manual_Name, manual_Effect);
                clickBool = false;
            }
        }
    }

    public void PopupDetails(Character _character)
    {
        Managers.ui.ShowPopupUI<UI_Details>().Setting(_character);
    }

    public void PickSet(bool _active, int mp = 0)
    {
        if (character.GetComponent<Player>() != null)
        {
            GetGameObject((int)GameObjects.Pick).SetActive(_active);
            if (_active)
            {
                GetImage((int)Images.PickFace).sprite = character.Face;

                GetImage((int)Images.PickHealedHPBar).gameObject.SetActive(false);
                GetImage((int)Images.PickCurrentHPBar).fillAmount = (float)character.HP / (float)character.stat.MaxHP;
                GetImage((int)Images.PickCurrentHPBar).color = Color.green;
                GetImage((int)Images.PickDamagedHPBar).gameObject.SetActive(false);

                BindUIEvent(GetGameObject((int)GameObjects.PickDetails), (PointerEventData data) => { PopupDetails(character); }, Define.UIEvent.Click);

                if (mp <= 0)
                {
                    GetImage((int)Images.PickHealedMPBar).gameObject.SetActive(true);
                    GetImage((int)Images.PickHealedMPBar).fillAmount = (float)(character.MP - mp) / (float)character.stat.MaxMP;
                    GetImage((int)Images.PickHealedMPBar).color = Color.yellow;
                    GetImage((int)Images.PickCurrentMPBar).fillAmount = (float)character.MP / (float)character.stat.MaxMP;
                    GetImage((int)Images.PickCurrentMPBar).color = Color.blue;
                    GetImage((int)Images.PickDamagedMPBar).gameObject.SetActive(false);
                }

                else
                {
                    GetImage((int)Images.PickHealedMPBar).gameObject.SetActive(false);
                    GetImage((int)Images.PickCurrentMPBar).fillAmount = (float)character.MP / (float)character.stat.MaxMP;
                    GetImage((int)Images.PickCurrentMPBar).color = Color.red;
                    GetImage((int)Images.PickDamagedMPBar).gameObject.SetActive(true);
                    GetImage((int)Images.PickDamagedMPBar).fillAmount = (float)(character.MP - mp) / (float)character.stat.MaxMP;
                    GetImage((int)Images.PickDamagedMPBar).color = Color.blue;
                }
            }
        }

        else
        {
            GetGameObject((int)GameObjects.Target).SetActive(_active);
            if (_active)
            {
                GetImage((int)Images.TargetFace).sprite = character.Face;

                GetImage((int)Images.TargetHealedHPBar).gameObject.SetActive(false);
                GetImage((int)Images.TargetCurrentHPBar).fillAmount = (float)character.HP / (float)character.stat.MaxHP;
                GetImage((int)Images.TargetCurrentHPBar).color = Color.green;
                GetImage((int)Images.TargetDamagedHPBar).gameObject.SetActive(false);

                BindUIEvent(GetGameObject((int)GameObjects.TargetDetails), (PointerEventData data) => { PopupDetails(character); }, Define.UIEvent.Click);

                if (mp <= 0)
                {
                    GetImage((int)Images.TargetHealedMPBar).gameObject.SetActive(true);
                    GetImage((int)Images.TargetHealedMPBar).fillAmount = (float)(character.MP - mp) / (float) character.stat.MaxMP;
                    GetImage((int)Images.TargetHealedMPBar).color = Color.yellow;
                    GetImage((int)Images.TargetCurrentMPBar).fillAmount = (float)character.MP / (float)character.stat.MaxMP;
                    GetImage((int)Images.TargetCurrentMPBar).color = Color.blue;
                    GetImage((int)Images.TargetDamagedMPBar).gameObject.SetActive(false);
                }

                else
                {
                    GetImage((int)Images.TargetHealedMPBar).gameObject.SetActive(false);
                    GetImage((int)Images.TargetCurrentMPBar).fillAmount = (float)character.MP / (float)character.stat.MaxMP;
                    GetImage((int)Images.TargetCurrentMPBar).color = Color.red;
                    GetImage((int)Images.TargetDamagedMPBar).gameObject.SetActive(true);
                    GetImage((int)Images.TargetDamagedMPBar).fillAmount = (float)(character.MP - mp) / (float)character.stat.MaxMP;
                    GetImage((int)Images.TargetDamagedMPBar).color = Color.blue;
                }
            }
        }
    }

    public void TargetSet(bool _active, int damage = 0)
    {
        if (character.GetComponent<Player>() != null)
        {
            GetGameObject((int)GameObjects.Target).SetActive(_active);
            if (_active)
            {
                GetImage((int)Images.TargetFace).sprite = map._target.Face;

                GetImage((int)Images.TargetHealedMPBar).gameObject.SetActive(false);
                GetImage((int)Images.TargetCurrentMPBar).fillAmount = (float)map._target.MP / (float)map._target.stat.MaxMP;
                GetImage((int)Images.TargetCurrentMPBar).color = Color.blue;
                GetImage((int)Images.TargetDamagedMPBar).gameObject.SetActive(false);

                BindUIEvent(GetGameObject((int)GameObjects.TargetDetails), (PointerEventData data) => { PopupDetails(map._target); }, Define.UIEvent.Click);

                if (damage <= 0)
                {
                    GetImage((int)Images.TargetHealedHPBar).gameObject.SetActive(true);
                    GetImage((int)Images.TargetHealedHPBar).fillAmount = (float)(map._target.HP - damage) / (float)map._target.stat.MaxHP;
                    GetImage((int)Images.TargetHealedHPBar).color = Color.yellow;
                    GetImage((int)Images.TargetCurrentHPBar).fillAmount = (float)map._target.HP / (float)map._target.stat.MaxHP;
                    GetImage((int)Images.TargetCurrentHPBar).color = Color.green;
                    GetImage((int)Images.TargetDamagedHPBar).gameObject.SetActive(false);
                }

                else
                {
                    GetImage((int)Images.TargetHealedHPBar).gameObject.SetActive(false);
                    GetImage((int)Images.TargetCurrentHPBar).fillAmount = (float)map._target.HP / (float)map._target.stat.MaxHP;
                    GetImage((int)Images.TargetCurrentHPBar).color = Color.red;
                    GetImage((int)Images.TargetDamagedHPBar).gameObject.SetActive(true);
                    GetImage((int)Images.TargetDamagedHPBar).fillAmount = (float)(map._target.HP - damage) / (float)map._target.stat.MaxHP;
                    GetImage((int)Images.TargetDamagedHPBar).color = Color.green;
                }
            }
        }

        else
        {
            GetGameObject((int)GameObjects.Pick).SetActive(_active);
            if (_active)
            {
                GetImage((int)Images.PickFace).sprite = map._target.Face;

                GetImage((int)Images.PickHealedMPBar).gameObject.SetActive(false);
                GetImage((int)Images.PickCurrentMPBar).fillAmount = (float)map._target.MP / (float)map._target.stat.MaxMP;
                GetImage((int)Images.PickCurrentMPBar).color = Color.blue;
                GetImage((int)Images.PickDamagedMPBar).gameObject.SetActive(false);

                BindUIEvent(GetGameObject((int)GameObjects.PickDetails), (PointerEventData data) => { PopupDetails(map._target); }, Define.UIEvent.Click);

                if (damage <= 0)
                {
                    GetImage((int)Images.PickHealedHPBar).gameObject.SetActive(true);
                    GetImage((int)Images.PickHealedHPBar).fillAmount = (float)(map._target.HP - damage) / (float)map._target.stat.MaxHP;
                    GetImage((int)Images.PickHealedHPBar).color = Color.yellow;
                    GetImage((int)Images.PickCurrentHPBar).fillAmount = (float)map._target.HP / (float)map._target.stat.MaxHP;
                    GetImage((int)Images.PickCurrentHPBar).color = Color.green;
                    GetImage((int)Images.PickDamagedHPBar).gameObject.SetActive(false);
                }

                else
                {
                    GetImage((int)Images.PickHealedHPBar).gameObject.SetActive(false);
                    GetImage((int)Images.PickCurrentHPBar).fillAmount = (float)map._target.HP / (float)map._target.stat.MaxHP;
                    GetImage((int)Images.PickCurrentHPBar).color = Color.red;
                    GetImage((int)Images.PickDamagedHPBar).gameObject.SetActive(true);
                    GetImage((int)Images.PickDamagedHPBar).fillAmount = (float)(map._target.HP - damage) / (float)map._target.stat.MaxHP;
                    GetImage((int)Images.PickDamagedHPBar).color = Color.green;
                }
            }
        }
    }

    public void ErrorTextActive(string text = "")
    {
        GameObject go = GetGameObject((int)GameObjects.ErrorText);
        go.SetActive(true);
        go.GetComponent<Text>().text = text;
    }

    public void ErrorTextUnactive()
    {
        GetGameObject((int)GameObjects.ErrorText).SetActive(false);
    }
}
