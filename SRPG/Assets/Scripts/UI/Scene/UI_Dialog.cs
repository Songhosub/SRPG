using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Dialog : UIScene
{
    Speaker[] speakers = new Speaker[2];
    List<Dialog> dialogs = new List<Dialog>();
    bool isAutoStart = true;
    bool isFirst = true;
    int currentDialogIndex = -1;
    int currentSpeakerIndex = 0;
    float typingSpeed = 0.095f;
    bool isTypingEffect;

    List<Stat> stats= new List<Stat>();

    public enum Images
    {
        LeftSpeaker,
        RightSpeaker,
        DialogBox
    }

    public enum Texts
    {
        LeftName,
        RightName,
        Dialog
    }

    public enum GameObjects
    {
        Arrow
    }

    public override void Init() 
    {
        base.Init();

        dialogs = Managers.data.LoadJson<DialogData>($"Dialog{Managers.data.StatDict.story}").dialog;

        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        for(int i = 0; i < speakers.Length; i++)
        {
            if (i == 0)
            {
                speakers[i].imageSpeaker = GetImage((int)Images.LeftSpeaker);
                speakers[i].textDialog = GetText((int)Texts.Dialog);
                speakers[i].textName = GetText((int)Texts.LeftName);
                speakers[i].objectArrow = GetGameObject((int)GameObjects.Arrow);
            }

            else
            {
                speakers[i].imageSpeaker = GetImage((int)Images.RightSpeaker);
                speakers[i].textDialog = GetText((int)Texts.Dialog);
                speakers[i].textName = GetText((int)Texts.RightName);
                speakers[i].objectArrow = GetGameObject((int)GameObjects.Arrow);
            }
        }

        Setup();
    }

    public void Setting(List<Stat> _stat)
    {
        stats = _stat;
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(()=>UpdateDialog());
    }

    private void Setup()
    {
        for(int i = 0; i < speakers.Length; i++)
        {
            SetActiveObject(speakers[i], false);
        }
    }

    public bool UpdateDialog()
    {
        if (isFirst == true)
        {
            Setup();

            if (isAutoStart)
            {
                SetNextDialog();
            }

            isFirst = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (isTypingEffect == true)
            {
                isTypingEffect = false;

                StopCoroutine("OnTypingText");
                speakers[currentSpeakerIndex].textDialog.text = dialogs[currentDialogIndex].speakerDialog;
                speakers[currentSpeakerIndex].objectArrow.SetActive(true);

                return false;
            }

            if (dialogs.Count > currentDialogIndex + 1)
            {
                SetNextDialog();
            }

            else
            {
                UI_GetCharacter getCharacter = Managers.ui.ShowPopupUI<UI_GetCharacter>();
                getCharacter.Init();
                getCharacter.Setting(stats);

                for (int i = 0; i < speakers.Length; i++)
                {
                    SetActiveObject(speakers[i], false);
                    speakers[i].imageSpeaker.gameObject.SetActive(false);
                }

                return true;
            }
        }


        return false;
    }

    private void SetNextDialog()
    {
        SetActiveObject(speakers[currentSpeakerIndex], false);
        currentDialogIndex++;
        currentSpeakerIndex = dialogs[currentDialogIndex].speakerIndex;
        speakers[currentSpeakerIndex].textName.text = dialogs[currentDialogIndex].speakerName;
        speakers[currentSpeakerIndex].imageSpeaker.sprite = Managers.resource.Load<Sprite>(dialogs[currentDialogIndex].spritePath, "Sprites/Face/");
        SetActiveObject(speakers[currentSpeakerIndex], true);
        StartCoroutine("OnTypingText");
    }

    private void SetActiveObject(Speaker speaker, bool visible)
    {
        speaker.textName.gameObject.SetActive(visible);
        speaker.textDialog.gameObject.SetActive(visible);

        speaker.objectArrow.SetActive(false);

        Color color = speaker.imageSpeaker.color;
        if (speaker.imageSpeaker.sprite != null)
        {
            color.a = visible == true ? 1 : 0.2f;
        }        
        speaker.imageSpeaker.color = color;
    }

    IEnumerator OnTypingText()
    {
        int index = 0;

        isTypingEffect = true;

        while (index < dialogs[currentDialogIndex].speakerDialog.Length)
        {
            speakers[currentSpeakerIndex].textDialog.text = dialogs[currentDialogIndex].speakerDialog.Substring(0, index);

            index++;

            yield return new WaitForSeconds(typingSpeed);
        }

        isTypingEffect = false;

        speakers[currentSpeakerIndex].objectArrow.SetActive(true);
    }

    public struct Speaker
    {
        public Image imageSpeaker;      //대화창 이미지 UI
        public Text textName;        //현재 대사중인 캐릭터 이름 출력용 텍스트 UI
        public Text textDialog;      //현재 대사 출력용 텍스트 UI
        public GameObject objectArrow; //대사 완료 후 표시되는 화살표 모양 오브젝트
    }
}   
