using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WOT.Core;

namespace WOT.Player
{
    public class PlayerManager : CharacterManager
    {
        InputHandler inputHandler;
        PlayerLoco playerLoco;
        CameraHandler cameraHandler;

        Animator anim;


        public bool isInteracting;

        [Header("Player Flags")]
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;
        public bool canDoCombo;


        private void Awake()
        {
            cameraHandler = CameraHandler.Instance;
            
        }
        private void Start()
        {
            inputHandler = GetComponent<InputHandler>();
            playerLoco = GetComponent<PlayerLoco>();
            anim = GetComponent<Animator>();
        }
        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;
            if (cameraHandler)
            {
                cameraHandler.followTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
            }
        }

        private void Update()
        {
            float delta = Time.deltaTime;

            isInteracting = anim.GetBool(AnimHash.Interacting);
            canDoCombo = anim.GetBool(AnimHash.CanDoCombo);

            inputHandler.TickInput(delta);
            playerLoco.HandleMovementInput(delta);
            playerLoco.HandleRollingAndSprinting(delta);
            playerLoco.HandleFalling(delta, playerLoco.moveDirection);

        }
        private void LateUpdate()
        {
            inputHandler.rollFlag = false;
            inputHandler.sprintFlag = false;

            inputHandler.rb_Input = false;
            inputHandler.s1_Input = false;
            inputHandler.s2_Input = false;
            inputHandler.s3_Input = false;

            if (isInAir)
            {
                playerLoco.inAirTime = playerLoco.inAirTime + Time.deltaTime;
            }
        }
    }
}