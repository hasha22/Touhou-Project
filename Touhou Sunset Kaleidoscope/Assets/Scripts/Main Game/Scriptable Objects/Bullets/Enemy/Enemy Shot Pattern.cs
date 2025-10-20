using UnityEngine;
namespace KH
{
    public class EnemyShotPattern : ScriptableObject
    {
        [Header("Enemy Shot Pattern Data")]
        public BulletType bulletType;
        public float defaultBulletSpeed = 3f;

        // Virtual enables the method to be overridden by child classes
        public virtual void Fire(Vector2 origin, GameObject enemy)
        {

        }
    }
}

