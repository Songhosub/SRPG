using UnityEngine;

public class Boom : Skill
{
    public override string ManualName()
    {
        return "�ͽ��÷���";
    }

    public override string Manual(int damage)
    {
        return $"������ ���� ���� ��� ������ ������� �ش�.\n��� : ����\t\t�Ÿ� : {player.stat.AttackDis}\n���� : ����\t\t���� : {damage}\n�Һ� MP : {mp}";
    }

    public override void Setting()
    {
        mp = 30;
    }

    public override Vector2 Pos(Character target)
    {
        return target.transform.position;
    }

    public override void Init(Character player)
    {
        base.Init(player);
    }

    public override void Ready()
    {
        base.Ready();

        player.AttackAreaSet(player.stat.AttackDis);
        player.TempAreaSelect();

        player.state = Character.State.Attack;

        player.ani.speed = 0;
    }

    public override void SelectedTarget(Character hit, Tile tile = null)
    {
        if (hit.GetComponent<Enemy>() == null || !player.attackArea.Contains(hit.moveCurrentTile))
        {
            return;
        }
        base.SelectedTarget(hit);

        Managers.scene.map.TileClear();
        hit.attackCurrentTile = hit.moveCurrentTile;
        hit.AttackAreaSet(player.stat.AttackDis);
        player.TempAreaSelect();
        hit.AttackAreaSelect();

        foreach(Character character in  Managers.scene.map._enemys)
        {
            Managers.scene.map.CurrentTileCharacter(character, Define.SearchType.Attack);
        }

        player.ani.Play(hit.moveCurrentTile.dir, -1, 0.3f);
        player.endDir = hit.moveCurrentTile.dir;
    }

    public override void Run()
    {
        base.Run();
        hit.Damaged(power);
        foreach (Tile t in hit.attackArea)
        {
            if (t.currentCharacter != null)
            {
                if(t.currentCharacter.GetComponent<Enemy>() != null)
                {
                    t.currentCharacter.Damaged(power);
                    t.Clear();
                }
            }
        }
    }
}
