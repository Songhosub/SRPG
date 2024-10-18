using UnityEngine;

public class Heal : Skill
{
    public override string ManualName()
    {
        return "힐";
    }

    public override string Manual(int damage)
    {
        return $"아군의 체력을 회복한다.\n대상 : 아군\t\t거리 : 없음\n종류 : 단일\t\t위력 : {damage}\n소비 MP : {mp}";
    }

    public override void Setting()
    {
        mp = 10;
    }

    public override Vector2 Pos(Character target)
    {
        return target.transform.position;
    }

    override public void Init(Character player)
    {
        base.Init(player);

        power = -power;
    }

    public override void Ready()
    {
        base.Ready();

        foreach (Player _player in Managers.scene.map._players)
        {
            if (_player != player)
            {
                _player.moveCurrentTile.attackSelect = true;
                _player.moveCurrentTile.HealSelectMat();
                player.attackArea.Add(_player.moveCurrentTile);
            }
        }

        player.attackCurrentTile.attackSelect = true;
        player.attackCurrentTile.HealSelectMat();
        player.attackArea.Add(player.attackCurrentTile);

        player.state = Character.State.Attack;

        player.ani.speed = 0;
    }

    override public void SelectedTarget(Character hit, Tile tile = null)
    {
        if (hit.GetComponent<Player>() == null || hit.stat.MaxHP <= hit.HP)
        {
            return;
        }


        base.SelectedTarget(hit);
        if(hit != player)
        {
            player.ani.Play(hit.moveCurrentTile.dir, -1, 0.3f);
            player.CurrentDir = hit.moveCurrentTile.dir;
            player.endDir = hit.moveCurrentTile.dir;
        }
    }

    override public void Run()
    {
        base.Run();
        hit.Damaged(power);
    }
}
