using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TAS
{
    public class AmmoBarProgress : MonoBehaviour {

        private Shoot shoot;
        private Image foregroundImage;

        // Use this for initialization
        void Start() {
            shoot = (Shoot)GameObject.FindObjectOfType(typeof(Shoot));
            foregroundImage = transform.Find("Foreground").GetComponent<Image>();
        }

        // Update is called once per frame
        void Update()
        {
            foregroundImage.fillAmount = shoot.getMagazineState();
        }
    }
}
