using DG.Tweening;
using s0m6ng.Utility;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ResourceMgr : MonoBehaviour
{
    public Volume m_BlurVolume;
    DepthOfField m_Blur;
    Tweener m_BlurTween;

    public static ResourceMgr Instance => Singleton<ResourceMgr>.Instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        m_BlurVolume.profile.TryGet(out m_Blur);
    }
    public void Blur(float value, float time)
    {
        m_BlurTween.Kill();
        m_BlurTween = DOTween.To(() => m_Blur.focalLength.value, x => m_Blur.focalLength.value = x, value, time);
    }
}
