using System.Collections;
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

        [Header("Enemy SFX")]
        public AudioClip spellCardSFX;
        [Range(0, 1)] public float spellCardSFXVolume = 0.05f;
        public GameObject enemyAudioSource;

        [Header("Stage Music")]
        public AudioSource bgmSource;
        public AudioClip herLastTwilight;
        public AudioClip introBGM;
        public AudioClip mainBGM;
        public float number = 2.5f;
        [SerializeField][Range(0, 1)] private float bgmVolume = 1f;
        public float loopStartTime = 63f;
        public float loopEndTime = 111.3f;

        [Header("References")]
        private PlayerManager playerManager;
        private PlayerShooter playerShooter;
        private Coroutine bgmCoroutine;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);

                introBGM.LoadAudioData();
                mainBGM.LoadAudioData();

                playerManager = PlayerInputManager.instance.playerObject.GetComponent<PlayerManager>();
                playerShooter = PlayerInputManager.instance.playerObject.GetComponent<PlayerShooter>();

                playerShootingSource.clip = playerShootingSFX;
                playerShootingSource.loop = true;

                bgmSource.volume = bgmVolume;
                playerShootingSource.volume = shootingVolume;

                PlayBGM(herLastTwilight);
                PlayPlayerShooting(playerShootingSFX);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void Update()
        {
            if (PlayerInputManager.instance.isShooting && !playerManager.isDead && !playerShooter.isPaused && !UIManager.instance.isInPauseMenu)
            {
                playerShootingSource.volume = shootingVolume;
            }
            else
            {
                playerShootingSource.volume = 0f;
            }
            if (bgmSource.time >= loopEndTime)
            {
                bgmSource.time = loopStartTime;
            }
        }
        public void PlayBGM(AudioClip shootingClip)
        {
            if (bgmSource.isPlaying) bgmSource.Stop();
            bgmSource.clip = herLastTwilight;
            bgmSource.Play();
        }
        public void PlayBGMWithIntro()
        {
            if (bgmCoroutine != null)
                StopCoroutine(bgmCoroutine);

            //bgmCoroutine = StartCoroutine(PlayIntroThenBGM());
        }
        private IEnumerator PlayIntroThenBGM()
        {
            bgmSource.loop = false;
            bgmSource.clip = introBGM;
            bgmSource.Play();


            // Wait until intro is almost done
            yield return new WaitForSeconds(introBGM.length - 0.1f);

            // Preload the main BGM clip
            bgmSource.clip = mainBGM;

            // Wait for the intro to actually finish
            while (bgmSource.time > 0 && bgmSource.isPlaying)
            {
                yield return null;
            }

            bgmSource.loop = true;
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
            PlayBGM(herLastTwilight);
        }
    }
}