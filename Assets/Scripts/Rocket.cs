using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAS
{
    public class Rocket : MonoBehaviour
    {
        public float radius = 500f;
        public float power = 10f;
        public float explosiveLift = 1f;


        // Use this for initialization
        void Start()
        {
            //Destroy(gameObject, 5);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnCollisionEnter(Collision collision)
        {
            Vector3 origin = collision.contacts[0].point;
            Collider[] colliders = Physics.OverlapSphere(origin, radius);

            foreach (Collider hit in colliders)
            {
                if (hit.GetComponent<CharacterControls>())
                {                    
                    hit.GetComponent<CharacterControls>().RocketForce(power, origin, radius, explosiveLift);
                }

            }

            Destroy(gameObject);

        }
    }
}
