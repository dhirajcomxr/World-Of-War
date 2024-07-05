using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WOT.Control
{
    [CreateAssetMenu(menuName ="WOT/CombatItem")]
    public class CombatItem : Item
    {
        public Transform prefab;
        public bool isUnarmed;

        [Header("Animations Name")]

        public string attack_1;
        public string attack_2;
        public string attack_3;
        public string attack_4;
        public string attack_5;

        public string skill_1;
        public string skill_2;
        public string skill_3;
    }
}
