using UnityEngine;

public class SceneManagerEx
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }
    public MapManager map;
    public TurnManager turn;

    public void Init()
    {
        //전투 씬이라면 MapManager와 TurnManager를 Static처럼 사용하게 해준다
        if(CurrentScene._sceneType == Define.Scene.Battle)
        {
            map = GameObject.FindObjectOfType<MapManager>();
            turn = GameObject.FindObjectOfType<TurnManager>();
            if(turn ==  null)
            {
                //만약 TurnManager가 없을 경우 만들어준다
                turn = new GameObject { name = "TurnManager" }.GetorAddComponent<TurnManager>();
            }
        }

        else
        {
            map = null;
            turn = null;
            return;
        }
    }

    public void LoadScene(string name)
    {
        Managers.Clear();
        //SceneManager.LoadScene(name);
        LoadingScene.LoadScene(name);
    }

    string GetSceneName(Define.Scene type)
    {
        string name = System.Enum.GetName(typeof(Define.Scene), type);
        return name;
    }

    public void Clear()
    {
        map = null;
        turn = null;
        CurrentScene.Clear();
    }
}