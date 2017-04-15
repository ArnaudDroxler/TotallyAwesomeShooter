using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAS
{

    public class Shoot : MonoBehaviour
    {
        public float fireRate = 1;
        private float nextFire = 0;
        
        public Rocket rocket;
        public float moveSpeed;
        
        public Camera m_camera;

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            CheckForInput();
        }
        

        void CheckForInput()
        {
            // Button name in inputManager
            if (Input.GetButton("Fire1") && Time.time >= nextFire)
            {
                Rocket clone;
                clone = Instantiate(rocket, m_camera.transform.position + m_camera.transform.forward * 0.5f, m_camera.transform.rotation);
                clone.setSpeed(moveSpeed);

                nextFire = Time.time + fireRate;
            }
        }

    }
}
