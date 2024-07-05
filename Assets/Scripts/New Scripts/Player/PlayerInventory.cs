using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WOT.Control;

namespace WOT.Player
{
    public class PlayerInventory : MonoBehaviour
    {
        WeaponSlotManager weaponSlotManager;

        public CombatItem leftCombatItem;
        public CombatItem rightCombatItem;


        private void Awake()
        {
            weaponSlotManager = GetComponent<WeaponSlotManager>();
        }

        private void Start()
        {
            weaponSlotManager.LoadWeaponOnSlot(rightCombatItem, true);
            weaponSlotManager.LoadWeaponOnSlot(leftCombatItem, false);
        }
    }
}