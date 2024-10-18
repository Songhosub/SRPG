using System.Collections.Generic;
using System.Linq;

public class DialogScene : BaseScene
{
    List<Stat> stats = new List<Stat>();

    protected override void Init()
    {
        _sceneType = Define.Scene.Dialog;

        base.Init();
        Managers.data.GetDict.stats = Managers.data.StatDict.stats.ToList();
        switch (Managers.data.StatDict.story)
        {
            case 0:
                stats.Add(Managers.resource.Load<Character>("amami_haruka", "Prefabs/Character/").stat);
                stats.Add(Managers.resource.Load<Character>("hoshii_miki", "Prefabs/Character/").stat);
                stats.Add(Managers.resource.Load<Character>("kisaragi_chihaya", "Prefabs/Character/").stat);
                break;
            case 1:
                stats.Add(Managers.resource.Load<Character>("miura_azusa", "Prefabs/Character/").stat);
                stats.Add(Managers.resource.Load<Character>("shijo_takane", "Prefabs/Character/").stat);
                break;
            case 2:
                stats.Add(Managers.resource.Load<Character>("futami_ami", "Prefabs/Character/").stat);
                stats.Add(Managers.resource.Load<Character>("futami_mami", "Prefabs/Character/").stat);
                break;
            case 3:
                stats.Add(Managers.resource.Load<Character>("minase_iori", "Prefabs/Character/").stat);
                stats.Add(Managers.resource.Load<Character>("takatsuki_yayoi", "Prefabs/Character/").stat);
                break;
            case 4:
                stats.Add(Managers.resource.Load<Character>("kikuchi_makoto", "Prefabs/Character/").stat);
                stats.Add(Managers.resource.Load<Character>("hagiwara_yukiho", "Prefabs/Character/").stat);
                break;
            case 5:
                stats.Add(Managers.resource.Load<Character>("akizuki_ritsuko", "Prefabs/Character/").stat);
                stats.Add(Managers.resource.Load<Character>("ganaha_hibiki", "Prefabs/Character/").stat);
                break;
            case 6:
                break;
            default:
                break;
        }

        Managers.data.GetDict.stats.AddRange(stats);
        UI_Dialog dialog = Managers.ui.ShowSceneUI<UI_Dialog>();
        dialog.Init();
        dialog.Setting(stats);
    }

    private void Awake()
    {
        Init();
    }

    public override void Clear()
    {
        base.Clear();
    }
}
