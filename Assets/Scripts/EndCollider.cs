using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAS
{
    public class EndCollider : MonoBehaviour
    {

        public GameObject player;

        private void OnTriggerEnter()
        {
            player.GetComponent<CharacterControls>().timerOk = false;
        }

    }
}