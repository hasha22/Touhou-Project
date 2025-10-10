using System.Collections.Generic;
using UnityEngine;
namespace KH
{
    public class StageManager : MonoBehaviour
    {
        public static StageManager instance { get; private set; }

        [Header("Stage Data")]
        public List<WaveTemplate> waves;
        private int currentWaveIndex;
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
            if (waves.Count > 0)
            {
                currentWaveIndex = 0;
                WaveManager.instance.InitializeWave(waves[currentWaveIndex]);
            }
        }
        private void Update()
        {
            if (WaveManager.instance.IsWaveFinished() && currentWaveIndex < waves.Count - 1)
            {
                currentWaveIndex++;
                WaveManager.instance.InitializeWave(waves[currentWaveIndex]);
            }
        }

    }
}
