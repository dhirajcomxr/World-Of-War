using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WOT.Player
{
    public class PlayerControllers : MonoBehaviour
    {
        [Header("Collider")]
        public Collider[] bodyParts;
        public LayerMask playerLayer;


        private void Start()
        {
            SetLayerToParts();
        }
        public void SetLayerToParts()
        {
            foreach (Collider item in bodyParts)
            {
                item.gameObject.layer = 6;
                item.isTrigger = true;
            }
        }
    }
}