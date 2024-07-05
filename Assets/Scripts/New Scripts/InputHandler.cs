using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WOT.Player;

namespace WOT
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;

        public float moveAmount;

        public float mouseX;
        public float mouseY;

        public bool b_Input;
        public bool rb_Input;



        public bool s1_Input;
        public bool s2_Input;
        public bool s3_Input;
        public bool lockOnInput;

        public bool rollFlag;
        public bool sprintFlag;
        public bool comboFlag;
        public bool lockOnFlag;




        public float rollInputTimer;

        Vector2 cameraInput;
        Vector2 movementInput;

        PlayerAttacker attacker;
        PlayerInventory inventory;
        PlayerManager playerManager;
        CameraHandler cameraHandler;
        private void Start()
        {
            attacker = GetComponent<PlayerAttacker>();
            inventory = GetComponent<PlayerInventory>();
            playerManager = GetComponent<PlayerManager>();
            cameraHandler = CameraHandler.Instance;
        }

        private void OnEnable()
        {
            /*if (inputActions == null)
            {
                inputActions = new PlayerInputs();
                inputActions.Player.move.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.Player.cam.performed += camActions => cameraInput = camActions.ReadValue<Vector2>();

                inputActions.PlayerAction.LockOn.performed += i => lockOnInput = true;
            }
            inputActions.Enable();*/
        }

        private void OnDisable()
        {
           // inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            HandleMoveInout(delta);
            HandleRollingInput(delta);
            HandleAttackInput(delta);
            HandleLockOnInput();
        }

        public void HandleRollingInput(float delta)
        {
            b_Input = InputAction.Instance.isShiftClicked;

            if (b_Input)
            {
                rollInputTimer += delta;
                sprintFlag = true;
            }
            else
            {
                if (rollInputTimer > 0 && rollInputTimer < 0.5f)
                {
                    sprintFlag = false;
                    rollFlag = true;
                }

                rollInputTimer = 0;
            }
        }
        private void HandleMoveInout(float delta)
        {
            horizontal = InputAction.Instance._moveAction.x;
            vertical = InputAction.Instance._moveAction.y;

            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

            mouseX = InputAction.Instance._mouseAction.x;
            mouseY = InputAction.Instance._mouseAction.y;
        }
        private void HandleAttackInput(float delta)
        {
            rb_Input = InputAction.Instance.meleAttack;
            s1_Input = InputAction.Instance.skill_1;
            s2_Input = InputAction.Instance.skill_2;
            s3_Input = InputAction.Instance.skill_3;

            if (rb_Input)
            {
                if (playerManager.canDoCombo)
                {
                    if (comboFlag) return;
                    comboFlag = true;
                    attacker.HandleWeaponCombo(inventory.rightCombatItem);
                    InputAction.Instance.meleAttack = false;
                    comboFlag = false;
                }
                else
                {
                    if (playerManager.isInteracting)
                        return;

                    if (playerManager.canDoCombo)
                        return;
                    attacker.HandleLightAttack(inventory.rightCombatItem);
                }
                InputAction.Instance.meleAttack = false;
            }
            else if (s1_Input)
            {
                attacker.HandleHeavyAttack(inventory.rightCombatItem,1);
                InputAction.Instance.skill_1 = false;
            }
            else if (s2_Input)
            {
                attacker.HandleHeavyAttack(inventory.rightCombatItem,2);
                InputAction.Instance.skill_2 = false;
            }
            else if (s3_Input)
            {
                attacker.HandleHeavyAttack(inventory.rightCombatItem,3);
                InputAction.Instance.skill_3 = false;
            }
        }


        private void HandleLockOnInput()
        {
            lockOnInput = InputAction.Instance.lockOnTarget;
            if (lockOnInput && !lockOnFlag)
            {
                cameraHandler.ClearLockOnTargets();
                lockOnInput = false;
                cameraHandler.HandleLockOn();
                if (cameraHandler.nearestLockOnTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
                    lockOnFlag = true;
                }

            }
            else if (lockOnInput && lockOnFlag)
            {
                lockOnFlag = false;
                lockOnInput = false;
                cameraHandler.ClearLockOnTargets();
            }
            cameraHandler.SetCameraHeight();
        }
    }
}