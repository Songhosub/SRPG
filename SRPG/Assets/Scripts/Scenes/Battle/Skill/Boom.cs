using UnityEngine;

public class Boom : Skill
{
    public override string ManualName()
    {
        return "익스플로젼";
    }

    public override string Manual(int damage)
    {
        return $"지정한 범위 내의 모든 적에게 대미지를 준다.\n대상 : 적군\t\t거리 : {player.stat.AttackDis}\n종류 : 범위\t\t위력 : {damage}\n소비 MP : {mp}";
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
