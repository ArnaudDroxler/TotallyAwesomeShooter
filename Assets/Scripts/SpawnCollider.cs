﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAS
{
    public class SpawnCollider : MonoBehaviour
    {
        public GameObject player;

        private void OnTriggerExit()
        {
            player.GetComponent<CharacterControls>().reSpawnPosition = GetComponent<Transform>().position;
            player.GetComponent<CharacterControls>().spawnPosition = GetComponent<Transform>().position;
            player.GetComponent<CharacterControls>().timerOk = true;
        }
    }
}