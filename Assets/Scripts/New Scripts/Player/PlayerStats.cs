using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WOT.Control.UI;
using WOT.Core;

namespace WOT.Player
{
    public class PlayerStats : CharacterStatsManager
    {
        public HealthBar healthBar;
        private PlayerAnimHandler animHandler;

        private void Awake()
        {
            animHandler = GetComponent<PlayerAnimHandler>();
        }

        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
        }

        int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamage(int damage)
        {
            currentHealth = currentHealth - damage;
            healthBar.SetCurrentHealth(currentHealth);
            if (currentHealth > 0) animHandler.PlayTargetAnimation(AnimHash.takeDamage, true);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                animHandler.PlayTargetAnimation(AnimHash.death, true);
            }
        }
    }

}