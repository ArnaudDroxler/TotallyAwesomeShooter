using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace TAS
{
    public class SpeedDisplay : MonoBehaviour
    {

        private Text speedDisplay;
        public GameObject player;

        private void Start()
        {
            speedDisplay = GetComponent<Text>();
        }

        public void Update()
        {
            Vector3 ups = player.GetComponent<CharacterControls>().m_controller.velocity;
            ups.y = 0;
            speedDisplay.text = Mathf.Round(ups.magnitude * 100) / 100 + " ups";
        }
    }
}
