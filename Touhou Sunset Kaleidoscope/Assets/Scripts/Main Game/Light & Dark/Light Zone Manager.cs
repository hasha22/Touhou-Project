using System.Collections.Generic;
using UnityEngine;
namespace KH
{
    public class LightZoneManager : MonoBehaviour
    {
        public static LightZoneManager instance { get; private set; }

        [Header("Setup")]
        public List<ILuminousZone> activeZones = new List<ILuminousZone>();
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
        public LightZone_FollowBullet SpawnFollowLightZone(BulletType bullet, Transform bulletTransform)
        {
            GameObject zone = ObjectPool.instance.GetPooledFollowZone_Capsule();
            LightZone_FollowBullet lightZone = zone.GetComponent<LightZone_FollowBullet>();

            lightZone.AttachToBullet(bulletTransform);
            activeZones.Add(lightZone);
            return lightZone;
        }

        public LightZoneBase SpawnPillar(Vector2 pos)
        {
            GameObject zone = ObjectPool.instance.GetPooledPillarZone();
            LightZone_Pillar lightZone = zone.GetComponent<LightZone_Pillar>();

            activeZones.Add(lightZone);
            return lightZone;
        }
        public bool IsInLight(Vector2 pos)
        {
            foreach (var z in activeZones)
                if (z.ContainsPoint(pos))
                    return true;
            return false;
        }
        public void UnRegisterZone(ILuminousZone zone)
        {
            activeZones.Remove(zone);
        }

    }
}

