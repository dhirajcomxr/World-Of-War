using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WOT.Player;

namespace WOT.Control
{
    public class DamageCollider : MonoBehaviour
    {
        Collider damageCollider;
        public int currentWeaponDamage = 25;
        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.enabled = false;
            damageCollider.isTrigger = true;
        }

        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }
        public void DisbaleDamageCollider()
        {
            damageCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(TagHash.PLAYER))
            {
                PlayerStats stats = other.GetComponent<PlayerStats>();
                if (stats != null)
                {
                    stats.TakeDamage(currentWeaponDamage);
                }
            }
            /*if (other.CompareTag(TagHash.ENEMY))
            {
                EnemyStats stats = other.GetComponent<EnemyStats>();
                if (stats != null)
                {
                    stats.TakeDamage(currentWeaponDamage);
                }
            }*/

        }
    }
}
