using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMgr : MonoBehaviour
{
    public static AudioMgr Instance;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    public List<SoundClip> m_audioClips = new List<SoundClip>();
    public struct SoundClip
    {
        public AudioClip clip;
        public string name;
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        LoadAudioClips();
    }
    private void LoadAudioClips()
    {
        m_audioClips.Clear();
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Sound");
        foreach(AudioClip c in clips)
        {
            SoundClip sc = new SoundClip();
            sc.clip = c;
            sc.name = c.name;
            m_audioClips.Add(sc);
        }
    }
    public void LoadClip_SFX(string name)
    {
        if (!m_audioClips.Exists(a => a.name == name)) return;
        SoundClip c = m_audioClips.Find(a => a.name == name);
        sfxSource.PlayOneShot(c.clip);
    }
    public void LoadClip_SFX(string name, float volume)
    {
        if (!m_audioClips.Exists(a => a.name == name)) return;
        SoundClip c = m_audioClips.Find(a => a.name == name);
        sfxSource.PlayOneShot(c.clip, volume);
    }
    public void LoadClip_BGM(string name)
    {
        if (!m_audioClips.Exists(a => a.name == name)) return;
        SoundClip c = m_audioClips.Find(a => a.name == name);
        bgmSource.clip = c.clip;
        bgmSource.Play();
    }
}
