using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Text.RegularExpressions;

namespace TAS
{


    public class GameManager : MonoBehaviour {
        public String levelName = "myLevel";
        GameObject player;
        GameObject ui;

        // Use this for initialization
        void Awake() {
            GameObject spawn =  GameObject.FindGameObjectWithTag("StartPlatform");
            player = (GameObject)Instantiate(Resources.Load("Player"));

            // Setup UI
            ui = (GameObject)Instantiate(Resources.Load("GameUI"));
            ui.GetComponentInChildren<SpeedDisplay>().player = player;
            ui.GetComponentInChildren<TimerDisplay>().player = player;
            

            player.GetComponent<CharacterControls>().SetupPlayer(spawn.transform);
    }

        // Update is called once per frame
        public void EndGame() {
            float score = player.GetComponent<CharacterControls>().timer;
            Destroy(player.GetComponent<Shoot>());
            Destroy(player.GetComponent<CharacterControls>());
            Destroy(ui);

            // SHOW END LEVEL UI
            // setup new ui
            ui = (GameObject)Instantiate(Resources.Load("EndOfLevel"));
            Button btnMenu = GameObject.Find("Menu").GetComponent<Button>(); 
            btnMenu.onClick.AddListener(() => menu());
            Button btnRestart = GameObject.Find("Restart").GetComponent<Button>();
            btnRestart.onClick.AddListener(() => restart());
            Cursor.visible = true;


            var ts = TimeSpan.FromMilliseconds(score);
            var parts = string.Format("{0:D2}:{1:D2}:{2:D3}", ts.Minutes, ts.Seconds, ts.Milliseconds).Split(':').SkipWhile(s => Regex.Match(s, @"00\w").Success).ToArray();
            var result = string.Join(":", parts);
            ui.GetComponentsInChildren<Text>()[1].text += result;

            float highscore = PlayerPrefs.GetFloat(levelName, 0);
            if (highscore == 0 || score < highscore)
                highscore = score;

            ts = TimeSpan.FromMilliseconds(highscore);
            ui.GetComponentsInChildren<Text>()[2].text += string.Join(":", string.Format("{0:D2}:{1:D2}:{2:D3}", ts.Minutes, ts.Seconds, ts.Milliseconds).Split(':').SkipWhile(s => Regex.Match(s, @"00\w").Success).ToArray());
            
            // Save records
            PlayerPrefs.SetFloat(levelName, highscore);
        }

        public void restart()
        {
            Destroy(player);
            Debug.Log("aésldfjasdf");
            // Destroy completely the player object
            Awake();
        }

        public void menu()
        {
            Debug.Log("aésldfjasdf");

        }
    }
}
