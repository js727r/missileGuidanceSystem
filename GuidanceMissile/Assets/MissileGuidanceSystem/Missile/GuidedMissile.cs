using UnityEngine;

namespace MissileGuidanceSystem.Scripts.Missile
{
    [RequireComponent(typeof(Rigidbody))]
    public class GuidedMissile : MonoBehaviour
    {
        public TrackingType trackingType;
        public TrackingLevel trackingLevel;
        public TargetType targetType;
        public GameObject targetObject;
        public Vector3 coord;

        public float trackingPower = 2f;
        public float missileSpeed = 10f;
        public float turningForce = 30f;


        private Rigidbody _rigidbody;

        // Start is called before the first frame update
        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            IgnorePhysics();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            GuideMissileDirection();
            MoveToForward();
        }

        private void IgnorePhysics()
        {
            // transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            // _rigidbody.velocity = Vector3.zero;
            _rigidbody.freezeRotation = true;
        }

        private void MoveToForward()
        {
            _rigidbody.velocity = transform.forward * missileSpeed;
        }

        private void GuideMissileDirection()
        {
            // valid check
            if (trackingType is TrackingType.None
                || targetType is TargetType.Object && targetObject is null) return;

            // calculate LOS(Line of sight)
            Vector3 LOS = Vector3.zero;
            if (targetType is TargetType.Object)
                LOS = targetObject.transform.position - transform.position;
            else
                LOS = coord - transform.position;

            Debug.DrawRay(transform.position, LOS, Color.red);
            // calculate how much rotate
            Quaternion guidedQuaternion = transform.rotation;

            if (trackingType is TrackingType.Pursuit)
            {
                guidedQuaternion = Quaternion.LookRotation(LOS, transform.up);
            }

            // rotate missile
            if (trackingLevel is TrackingLevel.Level1)
                transform.rotation = guidedQuaternion;
            else
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, guidedQuaternion, trackingPower);
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