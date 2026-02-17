using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioClip Music;
    [SerializeField] AudioClip WinSound;
    [SerializeField] AudioClip DiceLaunchSound;
    [SerializeField] AudioClip DiceMergeSound;
    AudioSource MusicSource;
    AudioSource PlayerSource;
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
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var Player = GameObject.FindGameObjectWithTag("Player");
        if(Player)
        PlayerSource = Player.GetComponent<AudioSource>();
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
}
