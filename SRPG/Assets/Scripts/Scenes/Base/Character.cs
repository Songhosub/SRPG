using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    //�ɷ�ġ
    public Stat stat;
    public string Path { get { return $"Character/{stat.character}"; } }
    public Sprite Face { get { return Managers.resource.Load<Sprite>(stat.character, "Sprites/Face/"); } }
    public Define.WeaponType weapon;
    public int HP = 0;
    public int MP = 0;
    public int NextLevelExp { get { return ((stat.Level) * (stat.Level)) * ((stat.Level * stat.Level) - (13 * stat.Level) + 82); } }
    public List<Skill> skills = new List<Skill>();

    //�� ����
    public float turnTime = 10.0f;

    public enum State
    {
        Idle,
        Move,
        Attack
    }
    public State state;

    //�ִϸ��̼�
    public Animator ani;
    public string startDir;
    public string moveDir;
    public string CurrentDir;
    public string endDir;

    //�̵��� ���� �ʿ��� ����
    public Tile moveCurrentTile; //���� �ڱ� �Ʒ��� ���� Ȯ��
    public List<Tile> moveArea = new List<Tile>(); //�̵� ������ �Ÿ� Ȯ��
    public bool isMove = false; //���� �̵� ������ Ȯ��

    //������ ���� �ʿ��� ����
    public Tile attackCurrentTile;
    public List<Tile> attackArea = new List<Tile>(); //���� ������ ���� Ȯ��
    public bool isAttack = false; //���� ���� ������ Ȯ��

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

    //ĳ������ ü���� ���ҽ�Ű�� �Լ�
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

    //�̵� ������ ���� ǥ�ð���
    //ĳ������ �̵��Ÿ� ��ŭ�� �̵������� ������ ��� �Լ�
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

    //�̵������� ������ Ÿ�ϵ��� ���׸����� �ٲپ� �ִ� �Լ�
    public void MoveAreaSelect()
    {
        foreach (Tile tile in moveArea)
        {
            tile.MoveSelectMat();
        }
    }


  //���� ����
    //ĳ������ ���ݰŸ� ��ŭ�� ���ݰ����� ������ ��� �Լ�
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

    //���ݰ����� ������ Ÿ�ϵ��� ���׸����� �ٲپ� �ִ� �Լ�
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

    //�ʱ�ȭ ����
    //�̵������� ������ Ÿ�ϵ��� ���׸����� �ʱ�ȭ �����ִ� �Լ�
    public void MoveAreaUnSelect()
    {
        foreach (Tile tile in moveArea)
        {
            tile.UnSelected();
        }
    }
    //���ݰ����� ������ Ÿ�ϵ��� ���׸����� �ʱ�ȭ �����ִ� �Լ�
    public void AtackAreaUnSelect()
    {
        foreach (Tile tile in attackArea)
        {
            tile.UnSelected();
        }
    }


    //��Ÿ ���
    //Ÿ���� ��ġ�� ĳ������ ���̿� ���� ������ִ� �Լ�
    public Vector3 TilePosToCharacter(Tile tile)
    {
        Vector3 pos = new Vector3(tile.Pos.x, tile.Pos.y + 0.4f, transform.position.z);
        return pos;
    }
}