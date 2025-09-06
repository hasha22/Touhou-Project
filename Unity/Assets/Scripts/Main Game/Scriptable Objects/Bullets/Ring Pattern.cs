using KH;
using UnityEngine;

[CreateAssetMenu(menuName = "Patterns/Ring Pattern")]
public class RingPattern : EnemyShotPattern
{
    [Header("Ring Pattern Data")]
    public int count = 36;
    public float spreadDegrees = 360f;
    public float startAngle = 0f;

    public override void Fire(Vector2 origin)
    {
        float step = spreadDegrees / count;

        Debug.Log($"Pool: {ObjectPool.instance}, Sprite: {bulletSprite}");
        for (int i = 0; i < count; i++)
        {
            float radians = startAngle * Mathf.Rad2Deg;
            Vector2 direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

            GameObject bullet = ObjectPool.instance.SpawnBullet(origin);

            if (bullet == null) Debug.LogError("SpawnBulletAt returned null!");

            BulletController bulletController = bullet.GetComponent<BulletController>();
            bulletController.InitializeEnemyBullet(direction, defaultBulletSpeed, bulletSprite);

            startAngle += step;
        }
    }
}
