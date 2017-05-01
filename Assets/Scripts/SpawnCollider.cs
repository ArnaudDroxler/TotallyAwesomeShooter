using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAS
{
    public class SpawnCollider : MonoBehaviour
    {
        public bool startWithRocketLauncher = false;

        private void OnTriggerExit(Collider playerCollider)
        { 
            if (playerCollider.GetComponent<CharacterControls>())
            {
                CharacterControls player = playerCollider.GetComponent<CharacterControls>();
                player.reSpawnPosition = GetComponent<Transform>().position;
                player.spawnPosition = GetComponent<Transform>().position;
                player.timerOk = true;
            }
        }
    }
}
