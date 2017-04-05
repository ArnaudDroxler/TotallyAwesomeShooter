using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAS
{
    public class CheckPointCollider : MonoBehaviour
    {
        public GameObject player;

        private void OnTriggerEnter()
        {
            player.GetComponent<CharacterControls>().reSpawnPosition = GetComponent<Transform>().position;
        
        }
    }

}
