using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    public string Name { get { return this.ToString().Replace("Skill (", "").Replace(")", ""); } }
    public Sprite Icon { get { return Managers.resource.Load<Sprite>(Name, "Sprites/UI/"); } }
    protected Character player;
    public Character hit { get; protected set; }
    public int power { get; protected set; }
    public int mp { get; protected set; }

    public abstract string ManualName();
    public abstract string Manual(int damage);

    public abstract void Setting();
    public abstract Vector2 Pos(Character target);

    public virtual void Init(Character player)
    {
        this.player = player;
        power = player.stat.Atk;
        hit = null;
    }

    public virtual void Ready()
    {
        Managers.scene.map.CurrentTileCharacter(player, Define.SearchType.Attack);
        player.MoveAreaUnSelect();
    }

    public virtual void SelectedTarget(Character hit, Tile tile = null)
    {
        this.hit = hit;
    }

    public virtual void Run()
    {
        player.MP -= mp;
    }
}
