using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TAS
{
    public class JumpCollider : MonoBehaviour
    {
        
        public float JumpPadForce =  10.0f;

        private void OnTriggerEnter(Collider playerCollider)
        {
            Debug.Log(transform.up);
            if (playerCollider.GetComponent<CharacterControls>())
            {
                playerCollider.GetComponent<CharacterControls>().jumpPad(JumpPadForce, transform.up);
            }
        }

    }
}