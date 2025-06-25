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
        // �̱��� ����
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // AudioSource �߰�������
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.clip = bgmClip;
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;

        // ��������Ʈ �����Ϸ��� AudioMixer ���� (����)
        if (masterMixer != null)
            bgmSource.outputAudioMixerGroup = masterMixer.FindMatchingGroups("Master")[0];

        // �ڵ� ���
        bgmSource.Play();
    }

    /// <summary>
    /// ȿ���� ��� ����
    /// </summary>
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume);
    }

    /// <summary>
    /// BGM ���� ����
    /// </summary>
    public void SetBGMVolume(float volume)
    {
        // mixer ���� exposed parameter ��: "BGMVolume"
        masterMixer?.SetFloat("BGMVolume", Mathf.Log10(volume) * 20);
    }
}
