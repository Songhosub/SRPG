using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Unity.VisualScripting;

public class Enemy : Character
{
    public Player target;
    List<Tile> route = new List<Tile>();

    protected override void Start()
    {
        base.Start();

        MapManager map = Managers.scene.map;
        map.ExpValue += stat.Exp;
        map.CurrentTileCharacter(this, Define.SearchType.Move);

        startDir = map.EnemyDir;
        endDir = startDir;
        CurrentDir = startDir;
        moveDir = startDir;
        ani.Play(startDir, -1, 0);
    }

    public void CoroutineStart()
    {
        StartCoroutine(GameStart());
    }

    public override void Destroy()
    {
        base.Destroy();

        Managers.scene.map.EnemysRemove(this);


        StopAllCoroutines();

        gameObject.SetActive(false);
    }

    public override void GameEnd()
    {
        base.GameEnd();
    }

    // Update is called once per frame
    IEnumerator GameStart()
    {
        while(true)
        {
            yield return null;

            if (!(isAttack || isMove || Managers.scene.map.targets.Count <= 0 || Managers.scene.turn.stateTurn == TurnManager.State.None))
            {
                if (state == State.Move)
                {
                    SeachTarget();
                    StartCoroutine(CharacterMove(stat.MoveDis));
                }

                if (state == State.Attack)
                {
                    StartCoroutine(CharacterAttack());
                }
            }            
        }        
    }

    /*
    void ShowLoad()
    {
        List<Tile> path = MapManager.AStar(moveCurrentTile, target.moveCurrentTile, moveSearchType);
        for (int i = 0; i < path.Count - 1; i++)
        {
            LineRenderer lr = path[i].GetComponent<LineRenderer>();
            Vector2[] linePoints = new Vector2[2];
            linePoints[0] = path[i].Pos;
            linePoints[1] = path[i + 1].Pos;
            lr.material.color = Color.red;
            //lr.SetPositions(linePoints);
            lr.enabled = true;
        }

        int playerDis = path.Count;
    }
    */

    public void SeachTarget()
    {
        foreach (KeyValuePair<Player, int> t in Managers.scene.map.targets.ToList())
        {
            int dis;
            if (Managers.scene.map.AStar(moveCurrentTile, t.Key.moveCurrentTile, Define.SearchType.Move) == null)
            {
                dis = 1000;
            }

            else
            {
                dis = Managers.scene.map.AStar(moveCurrentTile, t.Key.moveCurrentTile, Define.SearchType.Move).Count;
            }

            Managers.scene.map.targets[t.Key] = dis;
        }
        Managers.scene.map.targets = Managers.scene.map.targets.OrderBy(item => item.Value).ToDictionary(x => x.Key, x => x.Value);
        target = Managers.scene.map.targets.First().Key;
        route = Managers.scene.map.AStar(moveCurrentTile, target.moveCurrentTile, Define.SearchType.Move);
    }

    IEnumerator CharacterMove(int mov)
    {
        isMove = true;

        yield return new WaitForSeconds(0.5f);

        if (route == null)
        {
            isMove = false;

            if (Managers.scene.map.AStar(moveCurrentTile, target.moveCurrentTile, Define.SearchType.Attack).Count <= stat.AttackDis)
            {
                state = State.Attack;
            }

            else
            {
                Managers.scene.map.End();
            }
        }

        else
        {
            float speed = 0;
            float time = 0.5f;
            int i = 0;

            if (route.Count <= stat.MoveDis + stat.AttackDis)
            {
                if (Managers.scene.map.AStar(moveCurrentTile, target.moveCurrentTile, Define.SearchType.Attack).Count <= stat.AttackDis)
                {
                    isMove = false;
                    state = State.Attack;
                }

                else
                {
                    MoveAreaSelect();
                    List<Tile> temp = new List<Tile>();

                    while (route[route.Count - stat.AttackDis].state == Tile.State.Full && moveCurrentTile != route[route.Count - stat.AttackDis])
                    {
                        route[route.Count - stat.AttackDis].state = Tile.State.Hurdle;
                        temp.Add(route[route.Count - stat.AttackDis]);
                        SeachTarget();
                        yield return null;
                    }

                    ani.Play(route[1].dir, -1, 0);
                    ani.speed = 1;

                    while (i <= route.Count - stat.AttackDis)
                    {
                        if (route.Count - stat.AttackDis < 0)
                        {
                            break;
                        }

                        Vector3 start = this.transform.position;
                        Vector3 newPos = TilePosToCharacter(route[i]);

                        if (speed >= time)
                        {
                            speed = 0;
                            i++;

                            if (i + 1 < route.Count)
                            {
                                ani.Play(route[i].dir, -1, 0);
                            }
                            else
                            {
                                ani.Play(route[route.Count - stat.AttackDis].dir, -1, 0);
                            }
                        }

                        this.transform.position = Vector3.Lerp(start, newPos, speed / time);
                        speed += Time.deltaTime;

                        yield return null;
                    }

                    transform.position = TilePosToCharacter(route[route.Count - stat.AttackDis]);

                    moveCurrentTile.state = Tile.State.Empty;

                    ani.speed = 0;
                    //ani.Play(route[route.Count - AttackDis].dir, -1, 0);
                    moveDir = route[route.Count - stat.AttackDis].dir;
                    CurrentDir = moveDir;
                    endDir = moveDir;

                    yield return new WaitForSeconds(0.2f);

                    foreach (Tile t in temp)
                    {
                        t.state = Tile.State.Full;
                    }

                    Camera.main.GetOrAddComponent<FallowCamera>().TargetPosition(transform);
                    isMove = false;
                    state = State.Attack;
                }
            }

            else
            {
                MoveAreaSelect();

                List<Tile> temp = new List<Tile>();
                
                while (route[mov].state == Tile.State.Full)
                {
                    route[mov].state = Tile.State.Hurdle;
                    temp.Add(route[mov]);
                    SeachTarget();
                    yield return null;
                    if(temp.Count >= mov * 4)
                    {
                        mov--;

                        foreach (Tile t in temp)
                        {
                            t.state = Tile.State.Full;
                        }

                        temp.Clear();
                    }
                }
                
                ani.Play(route[1].dir, -1, 0);
                ani.speed = 1;

                while (i <= mov)
                {
                    Vector3 start = this.transform.position;
                    Vector3 newPos = TilePosToCharacter(route[i]);

                    if (speed >= time)
                    {
                        speed = 0;
                        i++;

                        if (i + 1 < route.Count)
                        {
                            ani.Play(route[i].dir, -1, 0);
                        }
                        else
                        {
                            ani.Play(route[mov].dir, -1, 0);
                        }
                    }

                    this.transform.position = Vector3.Lerp(start, newPos, speed / time);
                    speed += Time.deltaTime;

                    yield return null;
                }

                transform.position = TilePosToCharacter(route[mov]);

                moveCurrentTile.state = Tile.State.Empty;

                ani.speed = 0;
                ani.Play(route[mov].dir, -1, 0);
                moveDir = route[mov].dir;
                CurrentDir = moveDir;
                endDir = moveDir;

                yield return new WaitForSeconds(0.2f);

                foreach (Tile t in temp)
                {
                    t.state = Tile.State.Full;
                }

                Camera.main.GetOrAddComponent<FallowCamera>().TargetPosition(transform);
                isMove = false;
                Managers.scene.map.End();
            }
        }
    }

    IEnumerator CharacterAttack()
    {
        isAttack = true;
        Managers.scene.map.Attack();

        Managers.scene.map.SelectedTarget(target.gameObject, target);
        yield return new WaitForSeconds(0.5f);
        Managers.effect.Play(Enum.GetName(typeof(Define.WeaponType), stat.weaponType), skills[0].Name, CurrentDir, skills[0].Pos(target));
        Managers.sound.Play(skills[0].Name, Define.Sound.Effect, Enum.GetName(typeof(Define.WeaponType), stat.weaponType));
        yield return new WaitForSeconds(Managers.effect.Delay());
        Managers.scene.map.OK();
        yield return new WaitForSeconds(0.5f);
        isAttack = false;
        Managers.effect.End();
        Managers.scene.map.End();
    }

    public override void Damaged(float damage)
    {
        base.Damaged(damage);

        if (HP <= 0)
        {
            Destroy();
        }
    }
}