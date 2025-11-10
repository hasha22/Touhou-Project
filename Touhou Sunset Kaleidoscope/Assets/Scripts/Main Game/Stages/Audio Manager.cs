using UnityEngine;
namespace KH
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance { get; private set; }

        [SerializeField] private AudioSource audioPrefab;

        [Header("Player SFX")]
        public AudioClip deathSFX;
        public AudioSource playerShootingSource;
        public AudioClip playerShootingSFX;
        [SerializeField][Range(0, 1)] private float shootingVolume = 1f;

        [Header("Stage Music")]
        public AudioSource bgmSource;
        public AudioClip mainBGM;
        [SerializeField][Range(0, 1)] private float bgmVolume = 1f;

        [Header("References")]
        private PlayerManager playerManager;
        private PlayerShooter playerShooter;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);

                playerManager = PlayerInputManager.instance.playerObject.GetComponent<PlayerManager>();
                playerShooter = PlayerInputManager.instance.playerObject.GetComponent<PlayerShooter>();

                bgmSource.clip = mainBGM;
                playerShootingSource.clip = playerShootingSFX;

                bgmSource.loop = true;
                playerShootingSource.loop = true;

                bgmSource.volume = bgmVolume;
                playerShootingSource.volume = shootingVolume;

                PlayBGM(mainBGM);
                PlayPlayerShooting(playerShootingSFX);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void Update()
        {

            if (PlayerInputManager.instance.isShooting && !playerManager.isDead && !playerShooter.isPaused)
            {
                playerShootingSource.volume = shootingVolume;
            }
            else
            {
                playerShootingSource.volume = 0f;
            }
        }
        public void PlayBGM(AudioClip bgmClip)
        {
            if (bgmSource.isPlaying) bgmSource.Stop();
            bgmSource.clip = bgmClip;
            bgmSource.Play();
        }
        public void PlayPlayerShooting(AudioClip shootingClip)
        {
            if (playerShootingSource.isPlaying) playerShootingSource.Stop();
            playerShootingSource.clip = shootingClip;
            playerShootingSource.Play();
        }
        public void PlaySFX(AudioClip audioClip, Transform spawnTransform, float volume)
        {
            AudioSource audioSource = Instantiate(audioPrefab, spawnTransform.position, Quaternion.identity);
            audioSource.clip = audioClip;
            audioSource.volume = volume;
            audioSource.Play();
            Destroy(audioSource.gameObject, audioSource.clip.length);
        }
        public void ResetAudioManager()
        {
            PlayBGM(mainBGM);
        }
    }
}