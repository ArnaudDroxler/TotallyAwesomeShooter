using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAS
{

    public class rotateGun : MonoBehaviour
    {

        private Transform gun;
        private float i;
        private CharacterControls player;

        void Start()
        {
            gun = GetComponent<Transform>();
            player = (CharacterControls)GameObject.FindObjectOfType(typeof(CharacterControls));
        }

        void Update()
        {
            i += 1;
            if (i == 360)
            {
                i = 0;
            }
            gun.Rotate(0, i / 180, 0);
        }

        private void OnTriggerEnter(Collider playerCollider)
        {
            player.activeGun(true);
            //Destroy(gameObject);
        }
    }
}
