using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAS
{
    public class EndCollider : MonoBehaviour
    {
        
        private void OnTriggerEnter(Collider playerCollider)
        {
            if (playerCollider.GetComponent<CharacterControls>())
            {
                playerCollider.GetComponent<CharacterControls>().endGame();
            }
        }

    }
}