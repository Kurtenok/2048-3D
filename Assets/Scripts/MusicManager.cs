using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioClip Music;
    [SerializeField] AudioClip WinSound;
    [SerializeField] AudioClip DiceLaunchSound;
    [SerializeField] AudioClip DiceMergeSound;
    AudioSource MusicSource;
    [SerializeField] AudioSource PlayerSource;
    public static MusicManager singleton;

    void Awake()
    {
        if (singleton != null && singleton != this)
        {
            Destroy(gameObject);
            return;
        }

        singleton = this;
        DontDestroyOnLoad(gameObject);
    }


    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

        MusicSource = GetComponent<AudioSource>();
        if (MusicSource && Music)
        {
            MusicSource.loop = true;
            MusicSource.clip = Music;
            MusicSource.Play();
        }
        singleton = this;

        SetPlayerAudioSrc();
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
       SetPlayerAudioSrc();
    }

    public void PlayWinSound()
    {
        if (!WinSound || !PlayerSource)
            return;

        PlayerSource.clip = WinSound;
        PlayerSource.Play();
    }
    public void PlayDiceLaunchSound()
    {
        if (!DiceLaunchSound || !PlayerSource)
            return;

       
        PlayerSource.clip = DiceLaunchSound;
        PlayerSource.Play();
    }
    public void PlayDiceMergeSound()
    {
        if (!DiceMergeSound || !PlayerSource)
            return;

        PlayerSource.clip = DiceMergeSound;
        PlayerSource.Play();
    }

    private void SetPlayerAudioSrc()
    {
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        if(Player)
        {
            PlayerSource = Player.GetComponent<AudioSource>();
        }
    }
}
