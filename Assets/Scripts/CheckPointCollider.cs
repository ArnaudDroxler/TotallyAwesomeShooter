using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAS
{
    public class CheckPointCollider : MonoBehaviour
    {
        public GameObject player;

        private void OnTriggerEnter(Collider playerCollider)
        {
            if (playerCollider.GetComponent<CharacterControls>())
            {
                player.GetComponent<CharacterControls>().reSpawnPosition = GetComponent<Transform>().position;
            }
        
        }
    }

}
