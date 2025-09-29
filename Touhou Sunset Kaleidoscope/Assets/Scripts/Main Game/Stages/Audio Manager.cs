using UnityEngine;
namespace KH
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance { get; private set; }
        [SerializeField] private AudioSource audioPrefab;

        [Header("Player SFX")]
        public AudioClip deathSFX;

        [Header("Stage Music")]
        public AudioSource bgmSource;
        public AudioClip mainBGM;
        [SerializeField][Range(0, 1)] private float bgmVolume = 1f;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);

                bgmSource = GetComponent<AudioSource>();
                bgmSource.loop = true;
                bgmSource.volume = bgmVolume;

                PlayBGM(mainBGM);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        public void PlayBGM(AudioClip bgmClip)
        {
            if (bgmSource.isPlaying) bgmSource.Stop();
            bgmSource.clip = bgmClip;
            bgmSource.Play();
        }
        public void PlaySFX(AudioClip audioClip, Transform spawnTransform, float volume)
        {
            AudioSource audioSource = Instantiate(audioPrefab, spawnTransform.position, Quaternion.identity);
            audioSource.clip = audioClip;
            audioSource.volume = volume;
            audioSource.Play();
            Destroy(audioSource.gameObject, audioSource.clip.length);
        }
    }
}