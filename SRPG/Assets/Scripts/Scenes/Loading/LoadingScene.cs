using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScene : BaseScene
{
    public static string nextScene;
    Slider LoadingBar;
    Text LoadingTxt;

    protected override void Init()
    {
        base.Init();

        _sceneType = Define.Scene.Loading;

        UI_Loading load = Managers.ui.ShowSceneUI<UI_Loading>();
        load.Init();
        LoadingBar = load.ReturnSlider();
        LoadingTxt = load.ReturnText();
    }

    void Start()
    {
        Init();

        StartCoroutine(TansitionNextScene());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("Loading");
    }

    IEnumerator TansitionNextScene()
    {
        //����� ���� �񵿱� �������� �ε�
        AsyncOperation ao = SceneManager.LoadSceneAsync(nextScene);

        //�ε�Ǵ� ���� ����� ȭ�鿡 ������ �ʰ� ��
        ao.allowSceneActivation = false;

        float timer = 0.0f;
        while (!ao.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            LoadingTxt.text = $"{(int)(LoadingBar.value * 100)}%";

            if (ao.progress < 0.9f)
            {
                LoadingBar.value = Mathf.Lerp(LoadingBar.value, ao.progress, timer);
                if (LoadingBar.value >= ao.progress)
                {
                    timer = 0f;
                }
            }

            else
            {
                LoadingBar.value = Mathf.Lerp(LoadingBar.value, 1f, timer);
                if (LoadingBar.value == 1.0f)
                {
                    ao.allowSceneActivation = true;
                    Managers.Clear();
                    yield break;
                }
            }
        }
    }

    public override void Clear()
    {
        base.Clear();
    }
}
