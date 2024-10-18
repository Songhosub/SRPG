using UnityEngine;

public class SceneManagerEx
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }
    public MapManager map;
    public TurnManager turn;

    public void Init()
    {
        //���� ���̶�� MapManager�� TurnManager�� Staticó�� ����ϰ� ���ش�
        if(CurrentScene._sceneType == Define.Scene.Battle)
        {
            map = GameObject.FindObjectOfType<MapManager>();
            turn = GameObject.FindObjectOfType<TurnManager>();
            if(turn ==  null)
            {
                //���� TurnManager�� ���� ��� ������ش�
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