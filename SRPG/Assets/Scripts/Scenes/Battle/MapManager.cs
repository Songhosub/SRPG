using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using Unity.VisualScripting;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using System;

public class MapManager : MonoBehaviour
{
    //캐릭터와 애너미 관리
    public GameObject _pickObject;
    public Character _pick;

    public GameObject _targetObject;
    public Character _target;

    public int index = 0;
    public Dictionary<Player, int> targets = new Dictionary<Player, int>();

    //전투 관리
    FallowCamera fallowCamera;

    //조종대상 Pick
    public string PlayerDir;

    //선택대상 Target
    public string EnemyDir;

    //타일 관리
    public GameObject _tilePrefab;
    public Dictionary<Vector2Int, Tile> _tiles = new Dictionary<Vector2Int, Tile>();
    public List<Player> _players = new List<Player>();
    public List<Enemy> _enemys = new List<Enemy>();

    //전투 종료
    public List<Player> competePlayers = new List<Player>();
    public int ExpValue;

    UI_Battle uI;

    Vector2Int[] _checkDir = new Vector2Int[4]
    {
        //Forward
        new Vector2Int(8, 4),
        //Back
        new Vector2Int(-8, -4),
        //Left
        new Vector2Int(-8, 4),
        //Right
        new Vector2Int(8, -4),
    };
    string[] _stringDir = new string[4]
    {
        "Forward",
        "Back",
        "Left",
        "Right"
    };

    //시작 설정
    private void Awake()
    {
        Managers.ui.ShowPopupUI<UI_Select>();

        fallowCamera = Camera.main.GetOrAddComponent<FallowCamera>();
    }

    public void GameStart()
    {
        _players.AddRange(GameObject.FindObjectsOfType<Player>());
        competePlayers.AddRange(GameObject.FindObjectsOfType<Player>());
        
        _enemys.AddRange(GameObject.FindObjectsOfType<Enemy>());

        for (int i = 0; i < _players.Count; i++)
        {
            targets.Add(_players[i], 0);
        }

        foreach (Player player in _players)
        {
            player.CoroutineStart();
            CurrentTileCharacter(player, Define.SearchType.Move);
        }

        foreach (Enemy enemy in _enemys)
        {
            enemy.CoroutineStart();
            CurrentTileCharacter(enemy, Define.SearchType.Move);
        }
    }

    public void PlayersRemove(Player player)
    {
        if (targets.ContainsKey(player))
        {
            targets.Remove(player);
        }

        if (_players.Contains(player))
        {
            _players.Remove(player);
            if(_players.Count <= 0)
            {
                GameOver(Define.GameoverType.Lose);
            }
        }
    }

    public void EnemysRemove(Enemy enemy)
    {
        if (_enemys.Contains(enemy))
        {
            _enemys.Remove(enemy);
            if (_enemys.Count <= 0)
            {
                GameOver(Define.GameoverType.Win);
            }
        }
    }

    public void GameOver(Define.GameoverType type)
    {
        StopAllCoroutines();
        Managers.scene.turn.GameEnd();

        switch (type)
        {
            case Define.GameoverType.Win:
                //StopAllCoroutines();

                if (SceneManager.GetActiveScene().name.Contains(Managers.data.StatDict.story.ToString()))
                {
                    Managers.data.StatDict.story++;
                    Managers.data.StatDict.stats = Managers.data.GetDict.stats.ToList();
                    Managers.data.GetDict.stats.Clear();
                }

                // (Player player in _players)
                {
                   // player.GameEnd();
                }
                End();
                //Managers.scene.turn.GameEnd();

                UI_GetExp GetExp = Managers.ui.ShowPopupUI<UI_GetExp>();
                GetExp.Init();
                GetExp.Setting(competePlayers, ExpValue);

                break;

            case Define.GameoverType.Lose:
                //StopAllCoroutines();

                Managers.data.GetDict.stats.Clear();

                //foreach (Enemy enemy in _enemys)
                {
                    //enemy.GameEnd();
                }

                End();
                //Managers.scene.turn.GameEnd();
                Managers.scene.LoadScene("Lobby");
                break;
        }
    }

    IEnumerator Start()
    {
        Vector3 mousePos = Vector3.zero;

        while (true)
        {
            yield return null;

            //캐릭터 행동중 실행 불가
            if (!(_pick != null && (_pick.isAttack || _pick.isMove) || Managers.scene.turn.stateTurn != TurnManager.State.PlayerTurn))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }

                if(Input.GetMouseButtonUp(0))
                {
                    if (mousePos == Camera.main.ScreenToWorldPoint(Input.mousePosition))
                    {
                        RaycastHit2D hit = Physics2D.Raycast(mousePos, transform.forward);

                        if (hit)
                        {
                            if (!EventSystem.current.IsPointerOverGameObject())
                            {
                                Character hitChatacter = hit.collider.GetComponent<Character>();
                                Tile hitTile = hit.collider.GetComponent<Tile>();

                                if (!(_pick != null && _pick.state == Character.State.Attack))
                                {
                                    //빈 타일 클릭으로 초기화
                                    if (hitChatacter == null && hitTile != null && !hitTile.moveSelect && !hitTile.attackSelect)
                                    {
                                        ObjectSelect(null, null);
                                    }

                                    if (hitChatacter != null)
                                    {
                                        ObjectSelect(hit.collider.gameObject, hitChatacter);
                                    }
                                }
                            }
                        }
                    }
                }
            }                       
        }
    }

    public void ObjectSelect(GameObject hitObject, Character hitCharacter)
    {
        if (_pickObject == hitObject)
        {
            return;
        }

        TileClear();

        if (_pick != null)
        {
            ClearUI();
            _pick.transform.position = _pick.TilePosToCharacter(_pick.moveCurrentTile);
            _pick.ani.Play(_pick.startDir, -1, 0.3f);
            _pick.CurrentDir = _pick.startDir;
            _pick.endDir = _pick.startDir;
            _pick.moveDir = _pick.startDir;
            CurrentTileCharacter(_pick, Define.SearchType.Move);
            if (_pick.state == Character.State.Attack)
            {
                _pick.state = Character.State.Move;
            }
        }

        if (hitCharacter != null)
        {
            _pickObject = hitObject;
            _pick = hitCharacter;

            uI = Managers.ui.ShowPopupUI<UI_Battle>();
            uI.Init();

            uI.PickSet(true);
            uI.TargetSet(false);

            fallowCamera.TargetPosition(_pickObject.transform);

            CurrentTileCharacter(_pick, Define.SearchType.Move);
            _pick.MoveAreaSet(_pick.stat.MoveDis);
            _pick.MoveAreaSelect();
        }

        else
        {
            _pickObject = hitObject;
            _pick = null;

            _targetObject = null;
            _target = null;

            //fallowCamera.target = null;            
        }
    }

    //Attack버튼과 연결할 함수
    //이동범위를 비활성화하고 공격범위를 활성화
    public void Attack(int index = 0)
    {
        if (_pick == null || _pick.isMove || _pick.state == Character.State.Idle)
        {
            return;
        }

        if (Managers.scene.turn.stateTurn == TurnManager.State.PlayerTurn)
        {
            if(_pick.skills.Count > index)
            {
                this.index = index;
                _pick.skills[index].Ready();
                uI.PickSet(true, _pick.skills[index].mp);
            }
        }

        else if (Managers.scene.turn.stateTurn == TurnManager.State.EnemyTurn)
        {
            _pick.skills[index].Init(_pick);
        }
    }

    //Attack대상을 표시
    public void SelectedTarget(GameObject hitObject, Character hitEnemy)
    {
        if (_pick == null || _pick.isMove || _pick.state == Character.State.Idle)
        {
            return;
        }

        if (Managers.scene.turn.stateTurn == TurnManager.State.PlayerTurn)
        {
            _targetObject = hitObject;
            _target = hitEnemy;

            _pick.skills[index].SelectedTarget(_target);

            if (_pick.skills[index].hit != _target)
            {
                return;
            }

            uI.ErrorTextUnactive();
            uI.TargetSet(true, _pick.skills[index].power);
        }

        else if (Managers.scene.turn.stateTurn == TurnManager.State.EnemyTurn)
        {
            _targetObject = hitObject;
            _target = hitEnemy;
            _pick.ani.Play(_target.moveCurrentTile.dir, -1, 0.3f);
            _pick.CurrentDir = _target.moveCurrentTile.dir;
            _pick.endDir = _target.moveCurrentTile.dir;

            uI.TargetSet(true, _pick.stat.Atk);
        }

        //fallowCamera.target.position = (hitObject.transform.position + characterObject.transform.position) / 2.0f;
    }

    public void OK()
    {
        if (_target == null || _pick.isMove || _pick.state == Character.State.Idle)
        {
            return;
        }

        if (Managers.scene.turn.stateTurn == TurnManager.State.PlayerTurn)
        {
            if (_pick.skills[index].hit == null)
            {
                return;
            }
            _pick.skills[index].Run();

            uI.TargetSet(false);
        }

        else if (Managers.scene.turn.stateTurn == TurnManager.State.EnemyTurn)
        {
            _target.Damaged(_pick.stat.Atk);

            uI.TargetSet(false);
        }
    }

    public void Cancle()
    {
        if (_pick == null || _pick.isMove || _pick.state == Character.State.Idle)
        {
            return;
        }
        ObjectSelect(null, null);
    }

    public void End()
    {
        if (_pick == null || _pick.isMove || _pick.isAttack || _pick.state == Character.State.Idle)
        {
            return;
        }
      
        CurrentTileCharacter(_pick, Define.SearchType.Move);
        Managers.scene.turn.TurnEnd();
        _pick.state = Character.State.Idle;
        _pick.startDir = _pick.endDir;
        _pick.CurrentDir = _pick.startDir;
        _pick.endDir = _pick.startDir;
        _pick.moveDir = _pick.startDir;
        ObjectSelect(null, null);
    }

    //이동 범위 표시 관련
    //캐릭터가 있는 타일들의 State을 Full로 설정
    //캐릭터가 현재 이는 타일을 저장 (저장된 타일을 기준으로 이동가능한 범위를 탐색
    public void CurrentTileCharacter(Character character, Define.SearchType type)
    {
        if (_tiles.TryGetValue(character.Pos(), out Tile t))
        {
            if (type == Define.SearchType.Move)
            {
                character.moveCurrentTile = t;
                t.currentCharacter = character;
                t.state = Tile.State.Full;
            }

            if (type == Define.SearchType.Attack)
            {
                character.attackCurrentTile = t;
                t.currentCharacter = character;
            }
        }
    }

    //타일을 기준으로 주변 타일을 검색
    //해당 함수를 반복하여 이동가능한 타일을 탐색
    public Tile GetAroundTile(Tile tile, Vector2Int dir)
    {
        if (_tiles.TryGetValue(tile.intPos + dir, out Tile t))
        {
            return t;
        }

        else
        {
            return null;
        }
    }

    //위의 함수들을 사용하여
    //캐릭터가 이동가능한 타일을 반환해주는 함수
                                        //주변을 찾을 타일들,           탐색목적  
    public List<Tile> SearchTile(List<Tile> tileManagers, Define.SearchType type)
    {
        List<Tile> area = tileManagers;
        int a = area.Count;

        for (int t = 0; t < a; t++)
        {
            for (int j = 0; j < _checkDir.Length; j++)
            {
                //주변 타일 확인
                Tile tileManager = GetAroundTile(area[t], _checkDir[j]);

                //이동이라면
                if(type == Define.SearchType.Move)
                {
                    if (tileManager != null && !tileManager.moveSelect && tileManager.state != Tile.State.Hurdle)
                    {
                        for (int i = 0; i < area[t]._playerRoute.Count; i++)
                        {
                            tileManager._playerRoute.Add(area[t]._playerRoute[i]);
                        }
                        tileManager._playerRoute.Add(area[t]);
                        tileManager.dir = _stringDir[j];
                        tileManager.moveSelect = true;
                        area.Add(tileManager);
                    }
                }

                //공격이라면
                if (type == Define.SearchType.Attack)
                {
                    if (tileManager != null && !tileManager.attackSelect)
                    {
                        tileManager.attackSelect = true;
                        tileManager.dir = _stringDir[j];
                        area.Add(tileManager);
                    }
                }
            }
        }

        return area;
    }


  //타일 초기화 관련
    //모든 타일을 초기화 시켜주는 함수
    public void TileClear()
    {
        foreach(Tile tile in _tiles.Values.ToList())
        {
            tile.Clear();
        }
    }

    public List<Tile> AStar(Tile current, Tile target, Define.SearchType type)
    {
        TileClear();

        List<Tile> routeTile = new List<Tile>();
        SetHValue(target, current);
        routeTile.Add(current);

        while (!routeTile.Contains(target))
        {
            for (int j = 0; j < _checkDir.Length; j++)
            {
                //주변 타일 확인
                Tile tileManager = GetAroundTile(routeTile[0], _checkDir[j]);

                if (type == Define.SearchType.Move && tileManager == target && routeTile[0].state == Tile.State.Full && current != routeTile[0])
                {
                    continue;
                }

                if (tileManager != null && tileManager.state != Tile.State.Hurdle && !tileManager._enemyRoute.Contains(current) && tileManager != current)
                {
                    SetHValue(target, tileManager);
                    SetGValue(routeTile[0], tileManager);
                    for (int i = 0; i < routeTile[0]._enemyRoute.Count; i++)
                    {
                        tileManager._enemyRoute.Add(routeTile[0]._enemyRoute[i]);
                    }
                    tileManager._enemyRoute.Add(routeTile[0]);
                    tileManager.dir = _stringDir[j];
                    routeTile.Add(tileManager);
                }
            }

            routeTile.Remove(routeTile[0]);
            routeTile = routeTile.OrderBy(obj => obj.F).ToList();

            if (routeTile.Count == 0)
            {
                return null;
            }
        }

        //target._enemyRoute.Remove(target);
        return target._enemyRoute;
    }

    public void SetHValue(Tile target, Tile current)
    {
        Vector2Int dis = target.intPos - current.intPos;
        int disX = Mathf.Abs(dis.x / 8);
        int disY = Mathf.Abs(dis.y / 4);
        if (disX < disY)
        {
            current.H = disY;
        }
        else
        {
            current.H = disX;
        }
    }

    public void SetGValue(Tile prev, Tile current)
    {
        current.G = prev.G + 1;
    }


    public void SetUI()
    {
        uI = Managers.ui.ShowPopupUI<UI_Battle>();
    }

    public void ClearUI()
    {
        if(Managers.ui.TryClosePopupUI(uI))
        {
            uI = null;
            Managers.ui.ClosePopupUI();
        }            
    }

    public void CameraClear()
    {
        //fallowCamera.target = null;
    }
}
