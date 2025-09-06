using UnityEngine;
[CreateAssetMenu(menuName = "Enemy Shot Pattern")]
public class EnemyShotPattern : ScriptableObject
{
    [Header("Enemy Shot Pattern Data")]
    public GameObject bulletPrefab;
    public Sprite bulletSprite;
    public float defaultBulletSpeed = 500f;

    public virtual void Fire(Vector2 origin)
    {

    }
}
