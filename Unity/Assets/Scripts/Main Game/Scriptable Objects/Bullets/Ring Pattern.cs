using KH;
using UnityEngine;

[CreateAssetMenu(menuName = "Patterns/Ring Pattern")]
public class RingPattern : EnemyShotPattern
{
    [Header("Ring Pattern Data")]
    public int count = 36;
    public float spreadDegrees = 360f;
    public float startAngle = 0f;

    // Each bullet pattern will contain it's own trigonometrical calculations to fire its specific pattern
    public override void Fire(Vector2 origin)
    {
        float angle = startAngle;
        float step = spreadDegrees / count;
        for (int i = 0; i < count; i++)
        {
            float radians = angle * Mathf.Rad2Deg;
            Vector2 direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

            GameObject bullet = ObjectPool.instance.SpawnBullet(origin);

            BulletController bulletController = bullet.GetComponent<BulletController>();
            bulletController.InitializeEnemyBullet(direction, defaultBulletSpeed, bulletSprite);

            angle += step;
        }
    }
}
