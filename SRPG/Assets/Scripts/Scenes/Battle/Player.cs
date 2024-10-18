using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : Character
{
    protected override void Start()
    {
        base.Start();
        MapManager map = Managers.scene.map;

        startDir = map.PlayerDir;
        CurrentDir = startDir;
        endDir = startDir;
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

        MapManager map = Managers.scene.map;
        map.PlayersRemove(this);

        StopCoroutine(GameStart());

        gameObject.SetActive(false);
    }

    public override void GameEnd()
    {
        base.GameEnd();
    }

    public void GetExp(int exp)
    {
        stat.Exp += exp;
        while (stat.Exp >= NextLevelExp)
        {
            stat.Exp -= NextLevelExp;
            stat.Level++;            
            stat.AP++;
            stat.SP++;
        }
        Managers.data.RenewerStat(stat);
    }

    // Update is called once per frame
    IEnumerator GameStart()
    {
        Vector3 mousePos = Vector3.zero;

        while (true)
        {
            yield return null;

            if (!(isAttack || isMove))
            {
                //이동
                //자기턴에 타일을 클릭
                /*
                if (Input.GetButtonUp("Fire1") && Managers.scene.map._pick == this)
                {
                    mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
                */
                if (Input.GetMouseButtonUp(0) && Managers.scene.map._pick == this)
                {
                    //if (mousePos == Camera.main.ScreenToWorldPoint(Input.mousePosition))
                    {
                        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        RaycastHit2D hit = Physics2D.Raycast(mousePos, transform.forward);

                        if (hit)
                        {
                            if (EventSystem.current.IsPointerOverGameObject() == false)
                            {
                                //이동
                                if (state == State.Move)
                                {
                                    //클릭한 것이 이동가능한 타일이라면
                                    Tile tile = hit.collider.GetComponent<Tile>();
                                    if (tile != null && tile.moveSelect)
                                    {
                                        //캐릭터를 이동
                                        StartCoroutine(CharacterMove(tile));
                                    }
                                }

                                //공격
                                if (state == State.Attack)
                                {
                                    Character hitEnemy = hit.collider.GetComponent<Character>();
                                    if (hitEnemy != null)
                                    {
                                        Managers.scene.map.SelectedTarget(hit.collider.gameObject, hitEnemy);
                                    }
                                }
                            }
                        }
                    }
                }
            }                       
        }
    }


    //실제 캐릭터 조작
    //캐릭터를 이동시키는 함수
    IEnumerator CharacterMove(Tile tile)
    {
        moveCurrentTile.state = Tile.State.Empty;
        isMove = true;

        tile._playerRoute.Add(tile);

        float speed = 0;
        float time =  0.5f;
        int i = 0;

        transform.position = TilePosToCharacter(moveCurrentTile);

        ani.Play(tile._playerRoute[1].dir, -1, 0.3f);

        ani.speed = 1;
        //ani.SetTrigger(tile._playerRoute[1].dir);
        while (i < tile._playerRoute.Count)
        {
            Vector3 start = this.transform.position;
            Vector3 newPos = TilePosToCharacter(tile._playerRoute[i]);

            if (speed >= time)
            {
                speed = 0;
                i++;

                if (i + 1 < tile._playerRoute.Count)
                {
                    ani.Play(tile._playerRoute[i + 1].dir, -1, 0);
                }
                else
                {
                    ani.Play(tile.dir, -1, 0);
                }
            }

            this.transform.position = Vector3.Lerp(start, newPos, speed / time);
            speed += Time.deltaTime;

            yield return null;
        }

        tile._playerRoute.Remove(tile);
        transform.position = TilePosToCharacter(tile);

        ani.speed = 0;
        ani.Play(tile.dir, -1, 0.3f);
        moveDir = tile.dir;
        CurrentDir = moveDir;
        endDir = moveDir;

        Camera.main.GetOrAddComponent<FallowCamera>().TargetPosition(transform);
        isMove = false;
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
