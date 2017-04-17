using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TAS
{
    public class JumpCollider : MonoBehaviour
    {

        public GameObject player;
        public float JumpPadForce =  10.0f;

        private void OnTriggerEnter(Collider playerCollider)
        {
            player.GetComponent<CharacterControls>().jumpPad(JumpPadForce, transform.up);
        }

    }
}