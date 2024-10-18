using UnityEngine;

public class Attack : Skill
{
    public override string ManualName()
    {
        return "몸통박치기";
    }

    public override string Manual(int damage)
    {
        return $"적에게 대미지를 준다.\n대상 : 적군\t\t거리 : {player.stat.AttackDis}\n종류 : 단일\t\t위력 : {damage}\n소비 MP : {mp}";
    }

    public override void Setting()
    {
        mp = 0;
    }

    public override Vector2 Pos(Character target)
    {
        return (player.transform.position + target.transform.position) / 2;
        /*
        switch (player.weapon)
        {
            case Define.WeaponType.Sword:
                return (player.transform.position + target.transform.position) / 2;
            case Define.WeaponType.Claw:
                
            default:
                return Vector2.zero;
        }
        */
    }

    override public void Init(Character player)
    {
        base.Init(player);
    }

    public override void Ready()
    {
        base.Ready();

        player.AttackAreaSet(player.stat.AttackDis);
        player.AttackAreaSelect();
        player.state = Character.State.Attack;

        player.ani.speed = 0;
    }

    override public void SelectedTarget(Character hit, Tile tile = null)
    {
        if (hit.GetComponent<Enemy>() == null || !player.attackArea.Contains(hit.moveCurrentTile))
        {
            return;
        }
        base.SelectedTarget(hit);

        player.ani.Play(hit.moveCurrentTile.dir, -1, 0.3f);
        player.CurrentDir = hit.moveCurrentTile.dir;
        player.endDir = hit.moveCurrentTile.dir;
    }

    override public void Run()
    {
        base.Run();
        hit.Damaged(power);
    }
}
