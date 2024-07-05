using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WOT.Control;

namespace WOT.Player
{
    public class PlayerAttacker : MonoBehaviour
    {
        [SerializeField]PlayerManager playerManager;
        PlayerAnimHandler animHandler;
        InputHandler inputHandler;


        public string lastAttack;

        private void Awake()
        {
            animHandler = GetComponent<PlayerAnimHandler>();
            inputHandler = GetComponent<InputHandler>();
        }

        public void HandleLightAttack(CombatItem combatItem)
        {
            if (playerManager.isInteracting)
                return;
            animHandler.PlayTargetAnimation(combatItem.attack_1, true);
            lastAttack = combatItem.attack_1;
        }
        public void HandleHeavyAttack(CombatItem combatItem, int skillNo)
        {
            if (playerManager.isInteracting)
                return;
            switch (skillNo){
                case 1:
                    animHandler.PlayTargetAnimation(combatItem.skill_1, true);
                    lastAttack = combatItem.skill_1;
                    break;
                case 2:
                    animHandler.PlayTargetAnimation(combatItem.skill_2, true);
                    lastAttack = combatItem.skill_2;
                    break;
                case 3:
                    animHandler.PlayTargetAnimation(combatItem.skill_3, true);
                    lastAttack = combatItem.skill_3;
                    break;


            }

        }

        public void HandleWeaponCombo(CombatItem combatItem)
        {
            if (inputHandler.comboFlag)
            {
                animHandler.anim.SetBool(AnimHash.CanDoCombo, true);
                if (lastAttack == combatItem.attack_1)
                {
                    animHandler.PlayTargetAnimation(combatItem.attack_2, true);
                    lastAttack = combatItem.attack_2;
                }
                else if (lastAttack == combatItem.attack_2)
                {
                    animHandler.PlayTargetAnimation(combatItem.attack_3, true);
                    lastAttack = combatItem.attack_3;
                }
                else if (lastAttack == combatItem.attack_3)
                {
                    animHandler.PlayTargetAnimation(combatItem.attack_4, true);
                    lastAttack = combatItem.attack_4;
                }
                else if (lastAttack == combatItem.attack_4)
                {
                    animHandler.PlayTargetAnimation(combatItem.attack_5, true);
                }

            }


        }
    }
}
