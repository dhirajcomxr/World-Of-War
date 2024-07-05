using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WOT.Control;
namespace WOT.Player
{
    public class WeaponSlotManager : MonoBehaviour
    {
        WeaponHolderSlot leftHandSlot;
        WeaponHolderSlot rightHandSlot;

        DamageCollider leftHandDamageCollider;
        DamageCollider rightHandDamageCollider;

        public bool doNotLoadWeapon;

        private void Awake()
        {
            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();

            foreach (WeaponHolderSlot weaponHolderSlot in weaponHolderSlots)
            {
                if (weaponHolderSlot.isLeftHandSlot)
                {
                    leftHandSlot = weaponHolderSlot;
                }
                else if (weaponHolderSlot.isRightHandSlot)
                {
                    rightHandSlot = weaponHolderSlot;
                }
            }
        }

        public void LoadWeaponOnSlot(CombatItem combatItem, bool isLeft)
        {
            if (isLeft && !doNotLoadWeapon)
            {
                leftHandSlot.LoadWeaponModel(combatItem);
                LoadLeftWeaponDamageCollider();
            }
            else if (!isLeft)
            {
                rightHandSlot.LoadWeaponModel(combatItem);
                LoadRightWeaponDamageCollider();
            }
        }

        #region Handle Weapon's Damage Collider
        private void LoadLeftWeaponDamageCollider()
        {
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }
        private void LoadRightWeaponDamageCollider()
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }

        public void OpenLeftHandDamageCollider()
        {
            leftHandDamageCollider.EnableDamageCollider();
        }
        public void CloseLeftHandDamageCollider()
        {
            leftHandDamageCollider.DisbaleDamageCollider();
        }

        public void OpenRightHandDamageCollider()
        {
            rightHandDamageCollider.EnableDamageCollider();
        }
        public void CloseRightHandDamageCollider()
        {
            rightHandDamageCollider.DisbaleDamageCollider();
        }
        #endregion


    }
}