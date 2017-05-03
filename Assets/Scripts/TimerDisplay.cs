using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace TAS
{
    public class TimerDisplay : MonoBehaviour
    {

        private Text timerDisplay;
        private CharacterControls player;

        private void Start()
        {
            player = (CharacterControls)GameObject.FindObjectOfType(typeof(CharacterControls));
            timerDisplay = GetComponent<Text>();
        }

        public void Update()
        {
            double timer = player.timer;

            var ts = TimeSpan.FromMilliseconds(timer);
            var parts = string.Format("{0:D2}:{1:D2}:{2:D3}",  ts.Minutes, ts.Seconds, ts.Milliseconds).Split(':').SkipWhile(s => Regex.Match(s, @"00\w").Success).ToArray();
            var result = string.Join(":", parts);

            timerDisplay.text = result;
        }
    }
}
