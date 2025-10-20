using System.Collections.Generic;
using UnityEngine;
namespace KH
{
    public class LightZoneManager : MonoBehaviour
    {
        public static LightZoneManager instance { get; private set; }

        [Header("Setup")]
        [SerializeField] private GameObject pillarZonePrefab;
        [SerializeField] private GameObject followZonePrefab;
        public List<LightZone> activeZones;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            {
                Destroy(gameObject);
            }
        }
        public LightZone SpawnFollowLightZone(Transform bulletTransform)
        {
            var zoneObj = Instantiate(followZonePrefab, bulletTransform.position, Quaternion.identity);
            var zone = zoneObj.GetComponent<LightZone_FollowBullet>();
            zone.AttachToBullet(bulletTransform);
            activeZones.Add(zone);
            return zone;
        }
        public LightZone SpawnPillarLightZone(Vector2 position)
        {
            var zoneObj = Instantiate(pillarZonePrefab, position, Quaternion.identity);
            var zone = zoneObj.GetComponent<LightZone_PillarFormation>();
            //zone.InitializeLightZone(position);
            activeZones.Add(zone);
            return zone;
        }

    }
}

