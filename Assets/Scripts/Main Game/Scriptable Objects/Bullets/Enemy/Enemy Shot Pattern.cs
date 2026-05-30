using System.Collections.Generic;
using UnityEngine;
namespace KH
{
    public class EnemyShotPattern : ScriptableObject
    {
        [Header("Enemy Shot Pattern Data")]
        public List<BulletType> bulletTypes;
        public float defaultBulletSpeed = 3f;
        public List<AudioClip> attackSounds;
        [Range(0, 1)] public float attackSoundVolume = 1f;

        // Virtual enables the method to be overridden by child classes
        public virtual void Fire(Vector2 origin, GameObject enemy)
        {

        }
    }
}

