using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WOT.Player
{
    public abstract class Base
    {
        public MainPlayer _player;

        protected Vector2 _input;

        protected bool _isIdle = false;        
        protected bool _isDash = false;

        public bool _isDead = false;
        public bool _isRevived = false;
        
        protected float _playerSpeed = 5;

        private float _turnSmoothVelocity;
        private float _gravity = -9.8f;
        private float _gravityMulitplier = 3.0f;

        private float _strafeSpeedMultiplier = 0.75f;
        private float _backSpeedMultiplier = 0.4f;
        protected Vector3 _velocity;
        protected Rigidbody rb;


        public Base(MainPlayer mainPlayer)
        {
            _player = mainPlayer;
        }
        public virtual void EnterState() {
            _playerSpeed = _player.playerSpeed;
            _gravityMulitplier = _player.gravityMultiplier;
            rb = _player.rb;
        }
        public virtual void UpdateState() {

            addGeavity();
            _isDead = _player.isDead;

        }
        public virtual void ExitState() { }


        Vector3 moveDir;
        public void MovementUpdate(float speed = 1)
        {
            if (!_isDead)
            {
                float targetAngle = Mathf.Atan2(_input.x, _input.y) * Mathf.Rad2Deg + _player.cameraTransform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(_player.transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _player.turnSmoothDamp);
                _player.transform.rotation = Quaternion.Euler(0, angle, 0);
                moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
                _player.controller.Move(moveDir.normalized * _playerSpeed * speed * Time.deltaTime);
            }

        }

        public void addGeavity()
        {
            if (_player.controller.isGrounded && _velocity.y < 0f)
            {
                _velocity.y = -1f;
            }
            else
            {
                _velocity.y += _gravity * _gravityMulitplier * Time.deltaTime;
            }

            _player.controller.Move(_velocity * Time.deltaTime);
        }
        public void Jump()
        {
            if (_player.isGrounded)
            {
                _velocity.y += _player.jumpForce;
                _player.anim.Play(AnimHash.jump);
                //_player.jumpParticle.Play();
            }
        }

        public void ShieldActivate()
        {
            if (!_player.isShieldActivated)
            {
                //_player.anim.Play(AnimHash.SHIELD);
               // _player.shieldParticle.Play();
                _player.isShieldActivated = true;
            }
           // _player.SheildCountDown();
        }

    }
}
