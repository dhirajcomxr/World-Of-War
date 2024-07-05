using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WOT.Player
{
    public class MainPlayer : MonoBehaviour
    {
        #region States
        Base _currentState;
        public Idle _idle;
        #endregion

        public CharacterController controller;
        public Animator anim;
        public Transform cameraTransform;
        public Rigidbody rb;

        [Space(10)]
        public float currentStamina = 10;
        public float staminaSpeed = 5;
        public float playerSpeed = 15;
        public float dashSpeed = 30;
        public float dashTime = .2f;
        public float sprintSpeedMultiplier = 4;
        public float turnSmoothDamp = .1f;
        public float jumpForce = 15;
        public float gravityMultiplier = 3.0f;

        [Space(10)]
        public bool isGrounded = false;
        public bool isCooldown;
        public bool isAttackDashCompleted = false;
        public bool isStaminaCoolDown = false;
        public bool isUsableStaminaRestored = false;
        public bool isShieldActivated = false;
        public bool isSpecialAttackCooldown = false;
        public bool isInCutScene = false;
        public bool isEnemyLocked = false;
        public bool isDead;


        private void Start()
        {
            StateInitialize();
            _currentState = _idle;
            _currentState.EnterState();
        }
        public void StateInitialize()
        {
            _idle = new Idle(this);
        }
        private void Update()
        {
            _currentState.UpdateState();
        }
        public void ChangeCurrentState(Base newState)
        {
            _currentState.ExitState();
            _currentState = newState;
            _currentState.EnterState();
        }
    }
}
