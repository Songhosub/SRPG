using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    //능력치
    public Stat stat;
    public string Path { get { return $"Character/{stat.character}"; } }
    public Sprite Face { get { return Managers.resource.Load<Sprite>(stat.character, "Sprites/Face/"); } }
    public Define.WeaponType weapon;
    public int HP = 0;
    public int MP = 0;
    public int NextLevelExp { get { return ((stat.Level) * (stat.Level)) * ((stat.Level * stat.Level) - (13 * stat.Level) + 82); } }
    public List<Skill> skills = new List<Skill>();

    //턴 시작
    public float turnTime = 10.0f;

    public enum State
    {
        Idle,
        Move,
        Attack
    }
    public State state;

    //애니메이션
    public Animator ani;
    public string startDir;
    public string moveDir;
    public string CurrentDir;
    public string endDir;

    //이동을 위해 필요한 변수
    public Tile moveCurrentTile; //현재 자기 아래의 발판 확인
    public List<Tile> moveArea = new List<Tile>(); //이동 가능한 거리 확인
    public bool isMove = false; //현재 이동 중인지 확인

    //공격을 위해 필요한 변수
    public Tile attackCurrentTile;
    public List<Tile> attackArea = new List<Tile>(); //공격 가능한 범위 확인
    public bool isAttack = false; //현재 공격 중인지 확인

    public void Init( Stat _stat)
    {
        Managers.scene.map.CurrentTileCharacter(this, Define.SearchType.Move);
        stat = _stat;
        skills.Clear();

        HP = stat.MaxHP;
        MP = stat.MaxMP;

        foreach (string name in stat.Skills)
        {
            GetSkill(name);
        }
    }

    public void GetSkill(string name)
    {
        if (Managers.data.skills.TryGetValue(name, out Skill skill))
        {
            skills.Add(skill);
        }
    }

    protected virtual void Start()
    {
        ani = GetComponent<Animator>();
        transform.Translate(Vector3.back); 

        HP = stat.MaxHP;
        MP = stat.MaxMP;

        weapon = (Define.WeaponType)(stat.weaponType);

        if (stat.Skills.Count <= 0)
        {
            stat.Skills.Add("Attack");
            GetSkill(stat.Skills[0]);
        }

        ani.speed = 0;
        //Managers.scene.turn._characters.Add(this);
    }

    public virtual void Destroy()
    {
        TurnManager turn = Managers.scene.turn;
        turn.CharactersRemove(this);
    }

    public virtual void GameEnd()
    {
        StopAllCoroutines();
    }

    //캐릭터의 체력을 감소시키는 함수
    public virtual void Damaged(float damage)
    {
        HP -= (int)damage;
        if (HP <= 0)
        {
            moveCurrentTile.state = Tile.State.Empty;
        }
    }

    public Vector2Int Pos()
    {
        return new Vector2Int(Mathf.RoundToInt(transform.position.x * 10.0f), Mathf.RoundToInt((transform.position.y - 0.4f) * 10.0f));
    }

    //이동 가능한 범위 표시관련
    //캐릭터의 이동거리 만큼의 이동가능한 범위를 얻는 함수
    public void MoveAreaSet(int Dis)
    {
        //Managers.scene.map.TileClear();

        List<Tile> tiles = new List<Tile>();
        moveCurrentTile.moveSelect = true;
        tiles.Add(moveCurrentTile);

        for (int i = 0; i < Dis; i++)
        {
            tiles = Managers.scene.map.SearchTile(tiles, Define.SearchType.Move);
        }

        moveArea = tiles;

        foreach (Tile t in moveArea.ToList())
        {
            if (t.state != Tile.State.Empty)
            {
                t.moveSelect = false;
                moveArea.Remove(t);
            }
        }    
    }

    //이동가능한 범위의 타일들의 마테리얼을 바꾸어 주는 함수
    public void MoveAreaSelect()
    {
        foreach (Tile tile in moveArea)
        {
            tile.MoveSelectMat();
        }
    }


  //공격 관련
    //캐릭터의 공격거리 만큼의 공격가능한 범위를 얻는 함수
    public void AttackAreaSet(int Dis)
    {
        //Managers.scene.map.TileClear();

        List<Tile> tiles = new List<Tile>();
        attackCurrentTile.attackSelect = true;
        tiles.Add(attackCurrentTile);
        for (int i = 0; i < Dis; i++)
        {
            tiles = Managers.scene.map.SearchTile(tiles, Define.SearchType.Attack);
        }

        tiles.Remove(attackCurrentTile);
        attackCurrentTile.attackSelect = false;

        attackArea = tiles;

        foreach (Tile t in attackArea.ToList())
        {
            if (t.state == Tile.State.Hurdle)
            {
                t.attackSelect = false;
                attackArea.Remove(t);
            }
        }
    }

    //공격가능한 범위의 타일들의 마테리얼을 바꾸어 주는 함수
    public void AttackAreaSelect()
    {
        foreach (Tile tile in attackArea)
        {
            tile.AttackSelectMat();
        }
    }
    public void HealAreaSelect()
    {
        foreach (Tile tile in attackArea)
        {
            tile.HealSelectMat();
        }
    }
    public void TempAreaSelect()
    {
        foreach (Tile tile in attackArea)
        {
            tile.TempSelectMat();
        }
    }

    //초기화 관련
    //이동가능한 범위의 타일들의 마테리얼을 초기화 시켜주는 함수
    public void MoveAreaUnSelect()
    {
        foreach (Tile tile in moveArea)
        {
            tile.UnSelected();
        }
    }
    //공격가능한 범위의 타일들의 마테리얼을 초기화 시켜주는 함수
    public void AtackAreaUnSelect()
    {
        foreach (Tile tile in attackArea)
        {
            tile.UnSelected();
        }
    }


    //기타 계산
    //타일의 위치를 캐릭터의 높이에 맞춰 계산해주는 함수
    public Vector3 TilePosToCharacter(Tile tile)
    {
        Vector3 pos = new Vector3(tile.Pos.x, tile.Pos.y + 0.4f, transform.position.z);
        return pos;
    }
}