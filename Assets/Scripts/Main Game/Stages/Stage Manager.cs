using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KH
{
    public class StageManager : MonoBehaviour
    {
        public static StageManager instance { get; private set; }

        [Header("Stage Data")]
        public List<StageTemplate> stages;
        public int currentStageIndex = 0;
        [SerializeField] private string currentStageName;
        public float elapsedStageTime;
        [SerializeField] private float delayInBetweenStages = 3f;
        private StageTemplate currentStage;

        [Header("Current Wave Information")]
        private int currentWaveIndex;
        [SerializeField] private float timerBetweenWaves = 0f;

        [Header("Flags")]
        public bool isPaused = false;
        private bool waitingForNextWave = false;
        [SerializeField] private bool hasSpawnedBoss = false;
        public bool isStageBossDefeated = false;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

        }
        private void Start()
        {
            // Initializes first stage
            if (stages.Count > 0)
            {
                StartStage(0);
            }
        }
        public void StartStage(int stageIndex)
        {
            AudioManager.instance.PlayBGM(AudioManager.instance.herLastTwilight);
            UIManager.instance.HideBossUI();
            currentStageIndex = stageIndex;
            currentStage = stages[currentStageIndex];

            BackgroundManager.instance.SwitchBackground(currentStage.stageBackgroundMaterial);
            UIManager.instance.ShowStagePresentation(currentStage.presentationText, currentStage.stageName, currentStage.stageNameKanji, currentStage.initialDelay);

            if (stages[stageIndex].initialDelay > 0f) StartCoroutine(InitialWaveDelayCoroutine(stages[stageIndex].initialDelay, stageIndex));
            else InitializeStage(stageIndex);
        }
        private IEnumerator InitialWaveDelayCoroutine(float delay, int index)
        {
            FaithManager.instance.isStageBeingDelayed = true;
            yield return new WaitForSeconds(delay);
            FaithManager.instance.isStageBeingDelayed = false;
            InitializeStage(index);
        }
        private void Update()
        {
            if (isPaused) return;
            if (currentStage == null) return;
            if (WaveManager.instance.currentWave == null) return;

            elapsedStageTime += Time.deltaTime;

            // If the wave just finished, start the waiting timer
            if (WaveManager.instance.IsWaveFinished() && !waitingForNextWave)
            {
                waitingForNextWave = true;
                timerBetweenWaves = 0;
            }

            if (waitingForNextWave)
            {
                timerBetweenWaves += Time.deltaTime;

                if (timerBetweenWaves >= currentStage.waves[currentWaveIndex].delayBeforeNextWave)
                {
                    waitingForNextWave = false;
                    timerBetweenWaves = 0;

                    // Boss spawning - changed all stages to have one boss due to project new project scope
                    if (currentStage.boss.spawnAfterWaveIndex == currentWaveIndex + 1 && !hasSpawnedBoss)
                    {
                        StartCoroutine(SpawnBossWithDelay(currentStage.boss, currentStage.boss.delayBeforeSpawn));
                        hasSpawnedBoss = true;
                        return;
                    }

                    // Move to next wave or finish stage
                    if (isStageBossDefeated)
                    {
                        OnStageCompleted();
                        isStageBossDefeated = false;
                    }
                    else if (currentWaveIndex < currentStage.waves.Count - 1 && !isStageBossDefeated)
                    {
                        currentWaveIndex++;
                        WaveManager.instance.InitializeWave(currentStage.waves[currentWaveIndex]);
                    }
                }
            }
        }
        private IEnumerator SpawnBossWithDelay(Boss bossData, float delay)
        {
            yield return new WaitForSeconds(delay);
            BackgroundManager.instance.SwitchBackground(currentStage.boss.bossBackgroundMaterial);
            TriggerBossEvent(bossData);
        }
        public void InitializeStage(int index)
        {
            currentWaveIndex = 0;
            elapsedStageTime = 0;
            currentStageName = currentStage.stageName;

            hasSpawnedBoss = false;

            // Start first wave
            if (currentStage.waves.Count > 0)
                WaveManager.instance.InitializeWave(currentStage.waves[currentWaveIndex]);
        }
        private void OnStageCompleted()
        {
            if (currentStageIndex < stages.Count - 1)
            {
                //Fade out and advance to next stage
                UIManager.instance.ShowStageBonus(currentStage.stageBonus);

                StartCoroutine(DelayStageCoroutine());
            }
            else
            {
                UIManager.instance.StartVictoryScreenCoroutine();
                currentStage = null;
                elapsedStageTime = 0;
            }
        }
        private IEnumerator DelayStageCoroutine()
        {
            yield return new WaitForSeconds(delayInBetweenStages);
            StartStage(currentStageIndex + 1);
        }
        private void TriggerBossEvent(Boss bossData)
        {
            EnemyDatabase.instance.SpawnBoss(bossData);
        }
        public void ResetStage()
        {
            timerBetweenWaves = 0;
            waitingForNextWave = true;
            WaveManager.instance.ResetWave();
            StartStage(currentStageIndex);
        }

    }
}
