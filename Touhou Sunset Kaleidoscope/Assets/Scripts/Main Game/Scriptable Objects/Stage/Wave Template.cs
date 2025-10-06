using System.Collections.Generic;
using UnityEngine;
namespace KH
{
    [CreateAssetMenu(menuName = "Stage/Wave Template")]
    public class WaveTemplate : ScriptableObject
    {
        public List<SpawnEvent> spawnEvents;
    }
}


