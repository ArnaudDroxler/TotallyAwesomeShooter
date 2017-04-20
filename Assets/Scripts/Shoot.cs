using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAS
{

    public class Shoot : MonoBehaviour
    {
        public int magazineSize = 4;
        public float reloadRate = 1;
        private float magazineState;
        public float fireRate = 1;
        private float nextFire = 0;
        
        public Rocket rocket;
        public float moveSpeed;
        
        public Camera m_camera;

        // Use this for initialization
        void Start()
        {
            magazineState = magazineSize * 100;
        }

        // Update is called once per frame
        void Update()
        {
            CheckForInput();
        }

        private void FixedUpdate()
        {
            Debug.Log(magazineState);
            magazineState += reloadRate*100 * Time.fixedDeltaTime;
            if (magazineState > magazineSize * 100)
                magazineState = magazineSize * 100;
        }

        void CheckForInput()
        {
            // Button name in inputManager
            if (Input.GetButton("Fire1") && Time.time >= nextFire)
            {
                if (magazineState >= 100)
                {
                    magazineState -= 100;
                    Rocket clone;
                    clone = Instantiate(rocket, m_camera.transform.position + m_camera.transform.forward * 0.5f, m_camera.transform.rotation);
                    clone.setSpeed(moveSpeed);
                }

                nextFire = Time.time + fireRate;
            }
        }

    }
}
