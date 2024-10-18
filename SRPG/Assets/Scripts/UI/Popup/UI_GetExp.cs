using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GetExp : UIPopup
{
    List<Player> players = new List<Player>();
    LevelUpTemp temp;

    int expValue;
    int currentExp;
    bool isGet;
    bool isLevelUp = false;

    bool isFirst = true;
    int currentPlayerIndex = -1;

    public enum Images
    {
        Face,
        CurrentExpBar
    }

    public enum Texts
    {
        GetExpText,
        remainingExpText,
        CurrentExpText
    }

    public enum GameObjects
    {
        LevelUpText
    }


    public override void Init()
    {
        base.Init();
        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        GetGameObject((int)GameObjects.LevelUpText).SetActive(false);
    }

    public void Setting(List<Player> _players, int _expValue)
    {
        players.Clear();
        players = _players;
        expValue = _expValue;
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => UpdateGetExp());
        Managers.scene.LoadScene("Lobby");
    }

    public bool UpdateGetExp()
    {
        if (isFirst == true)
        {
            SetNextGetExp();

            isFirst = false;
        }

        else if (Input.GetMouseButtonDown(0))
        {
            if (isGet == true)
            {
                isGet = false;

                StopCoroutine("ExpUp");

                if (players[currentPlayerIndex].stat.Exp + expValue >= players[currentPlayerIndex].NextLevelExp)
                {
                    isLevelUp = true;
                }

                players[currentPlayerIndex].GetExp(currentExp);
                GetImage((int)Images.CurrentExpBar).fillAmount = (float)(players[currentPlayerIndex].stat.Exp) / (float)players[currentPlayerIndex].NextLevelExp;
                GetText((int)Texts.CurrentExpText).text = $"현재 경험치 : {players[currentPlayerIndex].stat.Exp}";
                GetText((int)Texts.GetExpText).text = $"획득 경험치 : {expValue}";
                GetText((int)Texts.remainingExpText).text = $"남은 경험치 : {players[currentPlayerIndex].NextLevelExp - (players[currentPlayerIndex].stat.Exp)}";

                if (isLevelUp)
                {
                    GetGameObject((int)GameObjects.LevelUpText).SetActive(true);
                }

                return false;
            }

            if (isLevelUp)
            {
                GetGameObject((int)GameObjects.LevelUpText).SetActive(false);
                UI_LevelUP LevelUP = Managers.ui.ShowPopupUI<UI_LevelUP>();
                LevelUP.Init();
                LevelUP.Setting(temp, players[currentPlayerIndex]);
                isLevelUp = false;
                return false;
            }

            if (players.Count > currentPlayerIndex + 1)
            {
                SetNextGetExp();
            }

            else
            {
                return true;
            }
        }

        return false;
    }

    private void SetNextGetExp()
    {
        currentPlayerIndex++;
        GetImage((int)Images.Face).sprite = players[currentPlayerIndex].Face;
        StartCoroutine("ExpUp");
    }

    IEnumerator ExpUp()
    {
        isGet = true;
        isLevelUp = false;

        int i = 0;
        int count = 0;
        currentExp = expValue;

        temp.level = players[currentPlayerIndex].stat.Level;
        temp.ap = players[currentPlayerIndex].stat.AP;
        temp.sp = players[currentPlayerIndex].stat.SP;

        while (i < currentExp)
        {
            i += 1;
            count += 1;

            yield return new WaitForSeconds(0.1f);

            GetImage((int)Images.CurrentExpBar).fillAmount = (float)(players[currentPlayerIndex].stat.Exp + i) / (float)players[currentPlayerIndex].NextLevelExp;
            GetText((int)Texts.CurrentExpText).text = $"현재 경험치 : {players[currentPlayerIndex].stat.Exp + i}";
            GetText((int)Texts.GetExpText).text = $"획득 경험치 : {count}";
            GetText((int)Texts.remainingExpText).text = $"남은 경험치 : {players[currentPlayerIndex].NextLevelExp - (players[currentPlayerIndex].stat.Exp + i)}";

            if((players[currentPlayerIndex].stat.Exp + i) >= players[currentPlayerIndex].NextLevelExp)
            {
                players[currentPlayerIndex].stat.Exp += i;
                players[currentPlayerIndex].stat.Exp -= players[currentPlayerIndex].NextLevelExp;
                players[currentPlayerIndex].stat.Level++;
                players[currentPlayerIndex].stat.AP++;
                players[currentPlayerIndex].stat.SP++;

                currentExp -= i;
                i = 0;

                isLevelUp = true;
            }
        }

        players[currentPlayerIndex].stat.Exp += i;

        if (isLevelUp)
        {
            GetGameObject((int)GameObjects.LevelUpText).SetActive(true);
        }

        Managers.data.RenewerStat(players[currentPlayerIndex].stat);

        isGet = false;
    }
}
