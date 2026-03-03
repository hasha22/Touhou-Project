using System.Collections.Generic;
using UnityEngine;
namespace KH
{
    [CreateAssetMenu(menuName = "Stage/Stage Template")]
    public class StageTemplate : ScriptableObject
    {
        [Header("Stage Info")]
        public string stageName;
        public float initialDelay;

        [Header("Waves in this Stage")]
        public List<WaveTemplate> waves;

        [Header("Bosses")]
        public Boss boss;

        [Header("Background")]
        public Material stageBackgroundMaterial;
    }
}