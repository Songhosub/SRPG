using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTree : UIPopup
{
    UI_Lobby Lobby;
    Dictionary<string, Image> skills = new Dictionary<string, Image>();

    List<Stat> stats = new List<Stat>();
    List<Sprite> sprites = new List<Sprite>();

    List<string> currentSkill = new List<string>();
    List<string> setSkill = new List<string>();

    Image[] skillImages;
    Dictionary<string, Image> skillName = new Dictionary<string, Image>();
    int currentIndex = 0;
    int currentSP = 0;
    int setskillnum = 0;


    public enum Images
    {
        Face,
        Skill1,
        Skill2,
        Skill3,
        Front_Image,
        Back_Image
    }

    public enum Texts
    {
        Name_Text,
        SP_Text,
        Index_Text
    }

    public enum GameObjects
    {
        SkillTree,
        Scrollbar,
        OK,
        Cancle,
        Panel
    }

    public override void Init()
    {
        base.Init();
        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        skillImages = GetGameObject((int)GameObjects.SkillTree).GetComponentsInChildren<Image>();

        for (int i = 0; i < skillImages.Length; i++)
        {
            if (skillImages[i] != null)
            {
                skillImages[i].color = Color.black;
                if (Managers.data.skills.ContainsKey(skillImages[i].sprite.name))
                {
                    skills.Add(skillImages[i].sprite.name, skillImages[i]);
                    string name = skillImages[i].name.Substring(0, 3);

                    skillName.Add(name, skillImages[i]);
                }
            }
        }
        
        foreach (KeyValuePair<string, Image> keyValue in skills)
        {
            BindUIEvent(keyValue.Value.gameObject, (PointerEventData data) => { SkillSlotClick(keyValue.Value); }, Define.UIEvent.Click);
        }


        for (int i = (int)Images.Skill1; i < (int)Images.Skill3 + 1; i++)
        {
            GetImage(i).gameObject.SetActive(false);
        }

        BindUIEvent(GetImage((int)Images.Skill2).gameObject, (PointerEventData data) => { SkillSetClick(); }, Define.UIEvent.Click);
        BindUIEvent(GetImage((int)Images.Skill3).gameObject, (PointerEventData data) => { SkillSetClick(); }, Define.UIEvent.Click);
        GetGameObject((int)GameObjects.Panel).SetActive(false);

        BindUIEvent(GetGameObject((int)GameObjects.OK), (PointerEventData data) => { OKClick(); }, Define.UIEvent.Click);
        BindUIEvent(GetGameObject((int)GameObjects.Cancle), (PointerEventData data) => { CancleClick(); }, Define.UIEvent.Click);
    }

    public void Setting(UI_Lobby _Lobby)
    {
        Lobby = _Lobby;
        stats = Lobby.stats;
        sprites = Lobby.sprites;
        currentIndex = 0;
        SetIndex();
    }

    //스킬 트리의 스킬을 클릭 시 실행
    public void SkillSlotClick(Image img)
    {
        if (!currentSkill.Contains(img.sprite.name)) //해당 스킬과 같은 스킬을 가지고있지 않다면
        {
            if (currentSP > 0 && Search(img.name))//SP가 있을 경우 & 이전 스킬을 배웠는지 확인
            {
                currentSP--;
                currentSkill.Add(img.sprite.name);
                GetText((int)Texts.SP_Text).text = $"SP : {currentSP}";
                img.color = Color.white;
            }
        }

        else //해당 스킬과 같은 스킬을 이미 가지고 있다면
        {
            if (!setSkill.Contains(img.sprite.name)) //해당 스킬이 세팅중인 스킬에 포함되지 않는다면
            {
                if (setskillnum < 3) //스킬 칸에 여유가 있다면
                {
                    currentSkill.Remove(img.sprite.name);
                    currentSkill.Insert(setskillnum, img.sprite.name);
                    if (Managers.data.skills.TryGetValue(currentSkill[setskillnum], out Skill skill))
                    {
                        SkillSetting(skill); //해당 스킬을 세팅중인 스킬에 추가
                    }
                }
            }
        }
    }

    public bool Search(string _name)
    {
        if(!_name.Contains("To"))
        {
            return true;
        } 

        string name = _name.Substring(_name.IndexOf("To")).Replace("To", "");
        int num = name.Count(f => (f == '_'));

        for(int i = 0; i <= num; i++)
        {
            string target = "S" + name.Substring(0, 2);
            if(skillName.TryGetValue(target, out Image image))
            {
                if (currentSkill.Contains(image.sprite.name))
                {
                    return true;
                }

                else
                {
                    name = name.Substring(2).Replace("_","");
                }
            }
        }

        return false;
    }

    //세팅된 스킬을 클릭 시 활성화
    public void SkillSetClick()
    {
        if (1 < setskillnum) //세팅된 스킬이 있다면
        {
            setskillnum--; //가장 최근 스킬부터 제거
            GetImage((int)Images.Skill1 + setskillnum).sprite = null;
            GetImage((int)Images.Skill1 + setskillnum).gameObject.SetActive(false);
            setSkill.RemoveAt(setskillnum);
        }
    }

    public void OKClick()
    {
        stats[currentIndex].SP = currentSP;
        stats[currentIndex].UseSkillNum = setskillnum;
        stats[currentIndex].Skills = currentSkill.ToList();
        StartCoroutine(Noti());
        Managers.data.RenewerStat(stats[currentIndex]);
    }

    public void CancleClick()
    {
        Lobby.SkillTreeUnclick();
    }

    IEnumerator Noti()
    {
        GetGameObject((int)GameObjects.Panel).SetActive(true);
        yield return new WaitForSeconds(1);
        GetGameObject((int)GameObjects.Panel).SetActive(false);
    }

    public void PageBack()
    {
        currentIndex--;
        SetIndex();
    }

    public void PageFront()
    {
        currentIndex++;
        SetIndex();
    }

    public void SetIndex()
    {
        if (currentIndex == 0)
        {
            BindUIEvent(GetImage((int)Images.Back_Image).gameObject, null, Define.UIEvent.Click);
            GetImage((int)Images.Back_Image).color = Color.gray;
        }
        else
        {
            BindUIEvent(GetImage((int)Images.Back_Image).gameObject, (PointerEventData data) => { PageBack(); }, Define.UIEvent.Click);
            GetImage((int)Images.Back_Image).color = Color.white;
        }

        if (currentIndex + 1 == stats.Count)
        {
            BindUIEvent(GetImage((int)Images.Front_Image).gameObject, null, Define.UIEvent.Click);
            GetImage((int)Images.Front_Image).color = Color.gray;
        }
        else
        {
            BindUIEvent(GetImage((int)Images.Front_Image).gameObject, (PointerEventData data) => { PageFront(); }, Define.UIEvent.Click);
            GetImage((int)Images.Front_Image).color = Color.white;
        }

        GetUI(currentIndex);
    }

    public void GetUI(int index)
    {
        setSkill.Clear();
        currentSkill.Clear();
        GetImage((int)Images.Face).sprite = sprites[index];
        GetText((int)Texts.Name_Text).text = stats[index].name;
        GetText((int)Texts.SP_Text).text = $"SP : {stats[index].SP}";
        GetText((int)Texts.Index_Text).text = $"{index + 1} / {stats.Count}";
        currentSP = stats[index].SP;
        currentSkill = stats[index].Skills.ToList();
        setskillnum = 0;

        for (int i = 0; i < skillImages.Length; i++)
        {
            if (skillImages[i] != null)
            {
                skillImages[i].color = Color.black;
            }
        }

        for (int i = (int)Images.Skill1; i < (int)Images.Skill3 + 1; i++)
        {
            GetImage((int)Images.Skill1 + setskillnum).sprite = null;
            GetImage(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < stats[index].Skills.Count; i++)
        {
            if (Managers.data.skills.TryGetValue(stats[index].Skills[i], out Skill skill))
            {
                if (setskillnum < stats[index].UseSkillNum)
                {
                    SkillSetting(skill);
                }

                if (skills.TryGetValue(stats[index].Skills[i], out Image img))
                {
                    img.color = Color.white;
                }
            }
        }
    }

    public void SkillSetting(Skill skill)
    {
        setSkill.Add(skill.Name);
        GetImage((int)Images.Skill1 + setskillnum).gameObject.SetActive(true);
        GetImage((int)Images.Skill1 + setskillnum).sprite = skill.Icon;
        setskillnum++;
    }
}
