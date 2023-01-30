using UnityEngine;
using UnityEngine.Serialization;

namespace MissileGuidanceSystem.Scripts.Missile
{
    [RequireComponent(typeof(Rigidbody))]
    public class GuidedMissile : MonoBehaviour
    {
        public TrackingType trackingType;
        public TrackingLevel trackingLevel;
        public TargetType targetType;
        public GameObject targetObject;
        public Vector3 targetCoord;

        public float rotationPerTick = 2f;
        public float missileSpeed = 10f;
        public float turningForce = 30f;
        
        private Rigidbody _rigidbody;

        // Start is called before the first frame update
        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            IgnorePhysics();
        }

        private void IgnorePhysics()
        {
            _rigidbody.freezeRotation = true;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            MoveForward();
            GuideMissileDirection();
        }
        
        private void MoveForward()
        {
            _rigidbody.velocity = transform.forward * missileSpeed;
        }

        private void GuideMissileDirection()
        {
            if (trackingType is TrackingType.None
                || targetType is TargetType.Object && targetObject is null) return;

            Quaternion guidedQuaternion = CalculateGuidedQuaternion();

            TryToLookAt(guidedQuaternion);
        }
        
        private Quaternion CalculateGuidedQuaternion()
        {
            Quaternion guidedQuaternion = transform.rotation;

            Vector3 LOS = CalculateLineOfSight();

            if (trackingType is TrackingType.Pursuit)
            {
                guidedQuaternion = Quaternion.LookRotation(LOS, transform.up);
            }

            return guidedQuaternion;
        }
        
        private Vector3 CalculateLineOfSight()
        {
            // line of sight
            Vector3 LOS;

            if (targetType is TargetType.Object)
                LOS = targetObject.transform.position - transform.position;
            else
                LOS = targetCoord - transform.position;

            return LOS;
        }
        
        private void TryToLookAt(Quaternion guidedQuaternion)
        {
            if (trackingLevel is TrackingLevel.Level1)
            {
                transform.rotation = guidedQuaternion;
            }
            else
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, guidedQuaternion, rotationPerTick);
            }
        }
    }

    public enum TrackingType
    {
        None,
        Pursuit,
        PN
    }

    public enum TargetType
    {
        Object,
        Coord
    }

    public enum TrackingLevel
    {
        Level1,
        Level2,
        Level3,
        Level4
    }
}