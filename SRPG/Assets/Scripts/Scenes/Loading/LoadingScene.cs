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
        //저장된 씬을 비동기 혀익으로 로드
        AsyncOperation ao = SceneManager.LoadSceneAsync(nextScene);

        //로드되는 씬의 모습이 화면에 보이지 않게 됨
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
