using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Unity.Burst.Intrinsics;

public class TurnManager : MonoBehaviour
{
    public enum State
    {
        None,
        PlayerTurn,
        EnemyTurn,
    }

    public State stateTurn;

    float turnTime = 0;
    public float turnLength { get { return 10000.0f; } protected set {; } }
    bool turnOn = false;

    public List<Character> _characters = new List<Character>();

    public UI_TurnBar turnBar;

    public void GameStart()
    { 
        _characters.AddRange(GameObject.FindObjectsOfType<Player>());
        _characters.AddRange(GameObject.FindObjectsOfType<Enemy>());

        if (_characters.Count <= 0)
        {
            return;
        }

        for (int i = 0; i < _characters.Count; i++)
        {
            _characters[i].turnTime = turnLength / _characters[i].stat.Agi;
        }

        turnBar.CreateBar(_characters);
        turnBar.SlotsSet();// _characters, turnLength);

        StartCoroutine(TurnSet());
    }

    public void GameEnd()
    {
        StopAllCoroutines();

        foreach (Character character in _characters)
        {
            character.GameEnd();
        }

        stateTurn = State.None;
        _characters.Clear();
        Destroy(turnBar.gameObject);
    }

    public void TurnStart(Character character)
    {
        turnOn = true;

        if(character.GetComponent<Player>() != null)
        {
            stateTurn = State.PlayerTurn;
        }

        else
        {
            stateTurn = State.EnemyTurn;
        }

        character.state = Character.State.Move;
        Managers.scene.map.ObjectSelect(character.gameObject, character);
    }

    public void TurnEnd()
    {
        turnOn = false;
        stateTurn = State.None;
    }

    IEnumerator TurnSet()
    {
        while(true)
        {
            yield return null;
            if(!turnOn)
            {
                //다음 턴까지의 딜레이가 적은 순으로 배열
                _characters = _characters.OrderBy(obj => obj.turnTime).ToList();
                float turnTime = _characters[0].turnTime;
                yield return new WaitForSeconds(1);

                //가장 적은 캐릭터의 턴이 오도록 딜레이 감소
                for(int i = 0; i< _characters.Count; i++)
                {
                    _characters[i].turnTime -= turnTime;
                }
                turnBar.SlotsSet();//_characters, turnLength);
                TurnStart(_characters[0]);

                #region 현재 턴인 캐릭터의 딜레이 초기화
                _characters[0].turnTime = turnLength / _characters[0].stat.Agi;
                Character temp = _characters[0];
                _characters.RemoveAt(0);
                _characters.Add(temp);
                #endregion
            }
        }
    }

    public void CharactersRemove(Character character)
    {
        if (_characters.Contains(character))
        {
            _characters.Remove(character);
            turnBar.DestroySlot();
        }
    }

    public void CreateTurnbar()
    {
        turnBar = Managers.ui.ShowSceneUI<UI_TurnBar>();
        turnBar.Init();
    }
}
