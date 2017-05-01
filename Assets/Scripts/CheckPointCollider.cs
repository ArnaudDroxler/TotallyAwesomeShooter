using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAS
{
    public class CheckPointCollider : MonoBehaviour
    {
        private void OnTriggerEnter(Collider playerCollider)
        {
            if (playerCollider.GetComponent<CharacterControls>())
            {
                playerCollider.GetComponent<CharacterControls>().reSpawnPosition = GetComponent<Transform>().position;
                playerCollider.GetComponent<CharacterControls>().reSpawnRotY = GetComponent<Transform>().localEulerAngles.y;


            }

        }
    }

}
