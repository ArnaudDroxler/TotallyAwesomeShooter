using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAS
{

    public class Shoot : MonoBehaviour
    {
        public float fireRate = 1;
        private float nextFire = 0;

        public Rigidbody rocket;
        public float power;
        private GameObject rocketSpawnPoint;

        // Use this for initialization
        void Start()
        {
            SetInitialReferneces();
        }

        // Update is called once per frame
        void Update()
        {
            CheckForInput();
        }

        void SetInitialReferneces()
        {
            rocketSpawnPoint = GameObject.Find("RocketSpawnPoint");
        }

        void CheckForInput()
        {
            // Button name in inputManager
            if (Input.GetButton("Fire1") && Time.time >= nextFire)
            {
                Rigidbody clone;
                clone = Instantiate(rocket, rocketSpawnPoint.transform.position, rocketSpawnPoint.transform.rotation);
                clone.velocity = transform.TransformDirection(Vector3.forward * power);

                nextFire = Time.time + fireRate;
            }
        }

    }
}
