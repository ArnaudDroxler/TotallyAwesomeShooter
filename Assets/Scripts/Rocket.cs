using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAS
{
    public class Rocket : MonoBehaviour
    {
        public float radius = 500f;
        public float power = 10f;

        private float speed = 5.0f;

        // Use this for initialization
        void Start()
        {
            // Destroy object after 2sec
            Destroy(this.gameObject, 2);
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 nextPosition = transform.position + transform.forward * Time.deltaTime * speed;

            RaycastHit hit;
            if (Physics.Linecast(transform.position, nextPosition, out hit))
            {
                // Colision happened at hit.point

                if (hit.collider.tag == "Player")
                { 
                    transform.position = nextPosition;
                    return;
                }

                Debug.Log(hit.collider.name);
                Vector3 origin = hit.point;
                Collider[] colliders = Physics.OverlapSphere(origin, radius);

                foreach (Collider collider in colliders)
                {
                    if (collider.GetComponent<CharacterControls>())
                    {
                        collider.GetComponent<CharacterControls>().RocketForce(power, origin, radius);
                    }


                    Destroy(this.gameObject);
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
