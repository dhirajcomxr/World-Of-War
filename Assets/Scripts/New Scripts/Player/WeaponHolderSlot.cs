using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WOT.Control;

namespace WOT.Player
{
    public class WeaponHolderSlot : MonoBehaviour
    {
        public Transform parantOverride;

        public bool isRightHandSlot;
        public bool isLeftHandSlot;

        public GameObject currentWeaponModel;

        public void EquipWeapon()
        {

        }
        public void UnequipWeapon()
        {
            if (!currentWeaponModel)
            {
                currentWeaponModel.gameObject.SetActive(false);
            }
        }
        public void UnequipWeaponAndDestroy()
        {
            if (!currentWeaponModel)
            {
                Destroy(currentWeaponModel.gameObject);
            }
        }
        public void LoadWeaponModel(CombatItem combatItem)
        {
            UnequipWeaponAndDestroy();
            if (combatItem == null)
            {
                UnequipWeapon();
                return;
            }

            GameObject model = Instantiate(combatItem.prefab.gameObject) as GameObject;

            if (model != null)
            {
                if (parantOverride != null)
                {
                    model.transform.parent = parantOverride.transform;
                }
                else
                {
                    model.transform.parent = transform;
                }

                model.transform.localPosition = Vector3.zero;
                model.transform.localRotation = Quaternion.identity;
                model.transform.localScale = Vector3.one;               
            }
            currentWeaponModel = model;
        }
    }
}