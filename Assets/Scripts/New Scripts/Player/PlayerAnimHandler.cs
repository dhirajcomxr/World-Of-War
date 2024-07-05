using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WOT.Core;

namespace WOT.Player
{
    public class PlayerAnimHandler : CharacterAnimManager
    {
        PlayerManager playerManager;
        InputHandler inputHandler;
        PlayerLoco playerLoco;

        int horizontal;
        int vertical;
        public bool canRotate;

        public void Initialize()
        {
            anim = GetComponent<Animator>();
            inputHandler = GetComponent<InputHandler>();
            playerLoco = GetComponent<PlayerLoco>();
            playerManager = GetComponent<PlayerManager>();
        }
        public void UpdateAnimatorValues(float vericalMovement, float horizontalMovement, bool isSprinting)
        {
            #region Vertical
            float v = 0;
            if (vericalMovement > 0 && vericalMovement < 0.55f)
            {
                v = .5f;
            }
            else if (vericalMovement > 0.55f)
            {
                v = 1;
            }
            else if (vericalMovement < 0 && vericalMovement > -0.55f)
            {
                v = -0.5f;
            }
            else if (vericalMovement < -0.55f)
            {
                v = -1;
            }
            else
            {
                v = 0;
            }
            #endregion

            #region Horizontal
            float h = 0;
            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            {
                h = .5f;
            }
            else if (horizontalMovement > 0.55f)
            {
                h = 1;
            }
            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            {
                h = -0.5f;
            }
            else if (horizontalMovement < -0.55f)
            {
                h = -1;
            }
            else
            {
                h = 0;
            }
            #endregion

            if (isSprinting)
            {
                v = 2;
                h = horizontalMovement;
            }

            anim.SetFloat(AnimHash.Virtical, v);
            anim.SetFloat(AnimHash.Horizontal, h);
        }

        public void CanRotate()
        {
            canRotate = true;
        }
        public void StopRotate()
        {
            canRotate = false;
        }
        public void EnableCombo()
        {
            anim.SetBool(AnimHash.CanDoCombo, true);

        }
        public void DisableCombo()
        {
            anim.SetBool(AnimHash.CanDoCombo, false);
        }

        private void OnAnimatorMove()
        {
            if (!playerManager.isInteracting)
                return;
            float delta = Time.deltaTime;
            playerLoco.rigidbody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            playerLoco.rigidbody.velocity = velocity;
        }
    }
}
