﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAS
{
    public class Rocket : MonoBehaviour
    {
        public float radius = 500f;
        public float power = 10f;
        public GameObject explosion;
        public LayerMask m_layerMask;

        private float speed = 5.0f;

        // Use this for initialization
        void Start()
        {
            // Destroy object after 2sec
            Destroy(this.gameObject, 2);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Vector3 nextPosition = transform.position + transform.forward * Time.deltaTime * speed;

            RaycastHit hit;
            if (Physics.Linecast(transform.position, nextPosition, out hit, m_layerMask))
            {
                if (hit.collider.GetComponent<Door>())
                {
                    hit.collider.GetComponent<Door>().Open();
                }

                // Colision happened at hit.point
                Vector3 origin = hit.point;
                Collider[] colliders = Physics.OverlapSphere(origin, radius);

                GameObject exp = Instantiate(explosion, transform.position, transform.rotation);
                Destroy(exp, exp.GetComponent<ParticleSystem>().main.duration);
                Destroy(this.gameObject);

                foreach (Collider collider in colliders)
                {
                    if (collider.GetComponent<CharacterControls>())
                    {
                        collider.GetComponent<CharacterControls>().RocketForce(power, origin, radius);
                    }

                    if (collider.GetComponent<Rigidbody>())
                    {
                        collider.GetComponent<Rigidbody>().AddExplosionForce(power, origin, radius);
                    }
                }

            }
            else
            {
                transform.position = nextPosition;
            }


        }

        public void setSpeed(float speed)
        {
            this.speed = speed;
        }
    }
}