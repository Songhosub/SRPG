using System.Collections.Generic;
using UnityEngine;

public class EffectManager
{
    Dictionary<string, RuntimeAnimatorController> EffectClips = new Dictionary<string, RuntimeAnimatorController>();
    SpriteRenderer spriteRenderer;
    Animator EffectSource;
    
    public void Init()
    {
        GameObject root = GameObject.Find("@Effect_Root");

        if (root == null)
        {
            root = new GameObject { name = "@Effect_Root" };
            GameObject.DontDestroyOnLoad(root);

            GameObject go = new GameObject { name = "Effect" };
            spriteRenderer = go.GetorAddComponent<SpriteRenderer>();
            spriteRenderer.sortingLayerName = "UI";
            EffectSource = go.GetorAddComponent<Animator>();
            go.transform.parent = root.transform;
        }
    }

    public void Play(RuntimeAnimatorController clip, string dir, Vector2 pos)
    {
        if (clip == null)
        {
            return;
        }
        
        if (clip.animationClips.Length >= 4)
        {
            EffectSource.gameObject.SetActive(true);
            EffectSource.transform.position = pos;
            EffectSource.runtimeAnimatorController = clip;

            EffectSource.Play(dir, -1, 0f);
        }

        else
        {
            EffectSource.gameObject.SetActive(true);
            EffectSource.transform.position = pos;
            EffectSource.runtimeAnimatorController = clip;

            EffectSource.Play("Run", -1, 0f);
        }
    }

    public float Delay()
    {
        if(EffectSource.GetCurrentAnimatorClipInfoCount(0) <= 0)
        {
            return 0;
        }

        return EffectSource.GetCurrentAnimatorStateInfo(0).length;
    }

    public void End()
    {
        EffectSource.gameObject.SetActive(false);
    }

    public void Play(string weapon, string path, string dir, Vector2 pos)
    {
        RuntimeAnimatorController animationClip = GetOrAddAnimationClip(weapon, path);
        Play(animationClip, dir, pos);
    }

    public RuntimeAnimatorController GetOrAddAnimationClip(string weapon, string path)
    {
        if (!EffectClips.TryGetValue(weapon + path, out RuntimeAnimatorController clip))
        {
            clip = Managers.resource.Load<RuntimeAnimatorController>($"Effect/{weapon}{path}");
            if(clip != null)
            {
                EffectClips.Add(weapon + path, clip);
            }

            else if(!EffectClips.TryGetValue(path, out clip))
            {
                clip = Managers.resource.Load<RuntimeAnimatorController>($"Effect/{path}");

                if (clip != null)
                {
                    EffectClips.Add(path, clip);
                }
            }
        }

        return clip;
    }

    public void Clear()
    {
        spriteRenderer.sprite = null;
        EffectSource.runtimeAnimatorController = null;
    }
}
