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
            if (!Input.GetMouseButton(0)) return;
            
            var mouseVector = new Vector3(Input.GetAxisRaw("Mouse X"), 0,Input.GetAxisRaw("Mouse Y"));
            transform.position += mouseVector*mouseSensitive;
        }
    }
}
