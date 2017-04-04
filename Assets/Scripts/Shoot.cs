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

        private GameObject rocketSpawnPoint;

        // Use this for initialization
        void Start()
        {
            rocketSpawnPoint = GameObject.Find("RocketSpawnPoint");
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
                clone = Instantiate(rocket, rocketSpawnPoint.transform.position, rocketSpawnPoint.transform.rotation);
                clone.setSpeed(moveSpeed);

                nextFire = Time.time + fireRate;
            }
        }

    }
}
