using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Mixer")]
    public AudioMixer masterMixer;

    [Header("Background Music")]
    public AudioClip bgmClip;
    private AudioSource bgmSource;

    void Awake()
    {
        // 싱글턴 세팅
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // AudioSource 추가·설정
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.clip = bgmClip;
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;

        // 볼륨·뮤트 조절하려면 AudioMixer 연결 (선택)
        if (masterMixer != null)
            bgmSource.outputAudioMixerGroup = masterMixer.FindMatchingGroups("Master")[0];

        // 자동 재생
        bgmSource.Play();
    }

    /// <summary>
    /// 효과음 재생 예시
    /// </summary>
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume);
    }

    /// <summary>
    /// BGM 볼륨 조절
    /// </summary>
    public void SetBGMVolume(float volume)
    {
        // mixer 에서 exposed parameter 예: "BGMVolume"
        masterMixer?.SetFloat("BGMVolume", Mathf.Log10(volume) * 20);
    }
}
