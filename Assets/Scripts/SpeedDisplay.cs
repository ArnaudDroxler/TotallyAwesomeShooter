using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace TAS
{
    public class SpeedDisplay : MonoBehaviour
    {

        private Text speedDisplay;
        private CharacterControls player;

        private void Start()
        {
            player = (CharacterControls)GameObject.FindObjectOfType(typeof(CharacterControls));
            speedDisplay = GetComponent<Text>();
        }

        public void Update()
        {   
            Vector3 ups = player.m_controller.velocity;
            ups.y = 0;
            speedDisplay.text = Mathf.Round(ups.magnitude * 100) / 100 + " ups";
        }
    }
}
