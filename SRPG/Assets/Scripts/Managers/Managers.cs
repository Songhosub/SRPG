using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers instance;

    public static Managers Instance
    {
        get { Init(); return instance; } //유일한 매니저를 갖고온다.
    }

    ResourceManager _resource = new ResourceManager();
    UIManager _ui = new UIManager();
    DataManager _data = new DataManager();
    SceneManagerEx _scene = new SceneManagerEx();
    SoundManager _sound = new SoundManager();
    EffectManager _effect = new EffectManager();

    public static ResourceManager resource { get { return Instance._resource; } }
    public static UIManager ui { get { return Instance._ui; } }
    public static DataManager data { get { return Instance._data; } }
    public static SceneManagerEx scene { get { return Instance._scene; } }
    public static SoundManager sound { get { return Instance._sound; } }
    public static EffectManager effect { get { return Instance._effect; } }

    static void Init()
    {
        if(instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if(go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }
            
            DontDestroyOnLoad(go);
            instance = go.GetComponent<Managers>();
            
            instance._ui.Init();
            instance._sound.Init();
            instance._data.Init();
            instance._effect.Init();
        }
    }

    public static void Clear()
    {
        sound.Clear();
        ui.Clear();
        scene.Clear();
        effect.Clear();
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }
}