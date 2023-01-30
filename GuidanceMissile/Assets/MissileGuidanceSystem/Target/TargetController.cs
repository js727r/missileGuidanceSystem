using UnityEngine;

namespace MissileGuidanceSystem.Target
{
    public class TargetController : MonoBehaviour
    {
        public float mouseSensitive = 2f;
        // Start is called before the first frame update
        
        // Update is called once per frame
        void Update()
        {
            MouseControl();
        }

        private void MouseControl()
        {
            var mouseVector = new Vector3(Input.GetAxisRaw("Mouse X"), 0,Input.GetAxisRaw("Mouse Y"));

            mouseVector.y += ((Input.GetButton("Up") ? 1 : 0) + (Input.GetButton("Down") ? -1 : 0)) * 0.2f;
            
            transform.position += mouseVector*mouseSensitive;
        }
    }
}
