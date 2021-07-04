namespace Morkilian.Helper
{
    //
    //Filename: maxCamera.cs
    //
    // original: http://www.unifycommunity.com/wiki/index.php?title=MouseOrbitZoom
    //
    // --01-18-2010 - create temporary target, if none supplied at start

    using UnityEngine;
    using System.Collections;

    /*TODO: 
     * Panning should not move the target
     * Pan around a point that's by default centerec in the target, but the pan moves this point instead so we can try différent angles
     * 
     */
    //[AddComponentMenu("Camera-Control/3dsMax Camera Style")]
    public class MayaCamera : MonoBehaviour
    {
        public bool m_CanMove = false;
        public Transform m_Target;
        public Transform m_Light;
        public float m_LightRotationSpeed = 130f;
        public Vector3 m_TargetOffset;
        public float m_DefaultDistance = 5.0f;
        [Min(0.1f)]public float m_MaxDistance = 20;
        
        [Min(0.1f)]public float m_MinDistance = .6f;
        public float xSpeed = 200.0f;
        public float ySpeed = 200.0f;
        public int m_YMinLimit = -80;
        public int m_YMaxLimit = 80;
        public int m_ZoomRate = 40;
        public float m_PanSpeed = 0.3f;
        public float m_ZoomDampening = 5.0f;

        private float xDeg = 0.0f;
        private float yDeg = 0.0f;
        private float currentDistance;
        private float desiredDistance;
        private Quaternion currentRotation;
        private Quaternion desiredRotation;
        private Quaternion rotation;
        private Vector3 position;
        public static bool CanMoveOnItsOwn = false;
        void Start() { Init(); }
        void OnEnable() { Init(); }

        public void Init()
        {
            //If there is no target, create a temporary target at 'distance' from the cameras current viewpoint
            if (!m_Target)
            {
                GameObject go = new GameObject("Cam Target");
                go.transform.position = transform.position + (transform.forward * m_DefaultDistance);
                m_Target = go.transform;
            }

            m_DefaultDistance = Vector3.Distance(transform.position, m_Target.position);
            currentDistance = m_DefaultDistance;
            desiredDistance = m_DefaultDistance;

            //be sure to grab the current rotations as starting points.
            position = transform.position;
            rotation = transform.rotation;
            currentRotation = transform.rotation;
            desiredRotation = transform.rotation;

            xDeg = Vector3.Angle(Vector3.right, transform.right);
            yDeg = Vector3.Angle(Vector3.up, transform.up);

            if(m_CanMove) CanMoveOnItsOwn = true;
        }

        /*
         * Camera logic on LateUpdate to only update after all character movement logic has been handled. 
         */
        void LateUpdate()
        {
            if (CanMoveOnItsOwn)
            {
                // If Control and Alt and Middle button? ZOOM!
                if (Input.GetMouseButton(1) && Input.GetKey(KeyCode.LeftAlt))
                {
                    desiredDistance -= Input.GetAxis("Mouse Y") * Time.deltaTime * m_ZoomRate * 0.125f * Mathf.Abs(desiredDistance);
                }
                if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftShift) && m_Light!=null)
                {
                    m_Light.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Time.deltaTime * m_LightRotationSpeed, Space.World);
                }
                // If middle mouse and left alt are selected? ORBIT
                else if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftAlt))
                {
                    xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                    yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

                    ////////OrbitAngle

                    //Clamp the vertical axis for the orbit
                    yDeg = ClampAngle(yDeg, m_YMinLimit, m_YMaxLimit);
                    // set camera rotation 
                    desiredRotation = Quaternion.Euler(yDeg, xDeg, 0);
                    currentRotation = transform.rotation;

                    rotation = Quaternion.Lerp(currentRotation, desiredRotation, Time.deltaTime * m_ZoomDampening);
                    transform.rotation = rotation;
                }
                // otherwise if middle mouse is selected, we pan by way of transforming the target in screenspace
                else if (Input.GetMouseButton(2) && Input.GetKey(KeyCode.LeftAlt))
                {
                    //grab the rotation of the camera so we can move in a psuedo local XY space
                    m_Target.rotation = transform.rotation;
                    m_Target.Translate(Vector3.right * -Input.GetAxis("Mouse X") * m_PanSpeed);
                    m_Target.Translate(transform.up * -Input.GetAxis("Mouse Y") * m_PanSpeed, Space.World);
                }

                ////////Orbit Position

                // affect the desired Zoom distance if we roll the scrollwheel
                desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * m_ZoomRate * Mathf.Abs(desiredDistance);
                //clamp the zoom min/max
                desiredDistance = Mathf.Clamp(desiredDistance, m_MinDistance, m_MaxDistance);
                // For smoothing of the zoom, lerp distance
                currentDistance = Mathf.Lerp(currentDistance, desiredDistance, Time.deltaTime * m_ZoomDampening);

                // calculate position based on the new currentDistance 
                position = m_Target.position - (rotation * Vector3.forward * currentDistance + m_TargetOffset);
                transform.position = position;
            }
        }

        private static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360)
                angle += 360;
            if (angle > 360)
                angle -= 360;
            return Mathf.Clamp(angle, min, max);
        }
    } 
}