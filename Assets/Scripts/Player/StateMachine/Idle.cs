using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WOT.Player
{
    public class Idle : Base
    {
        public Vector3 velocity;
        public bool attack;
        public Idle(MainPlayer player) : base(player)
        {
            player = _player;
        }

        public override void EnterState()
        {
            base.EnterState();
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void UpdateState()
        {
            base.UpdateState();
            _input = InputAction.Instance._moveAction;
            if (!_isDead)
            {
                _player.anim.SetFloat(AnimHash.Horizontal, _input.x, .1f, Time.deltaTime);
                _player.anim.SetFloat(AnimHash.Virtical, _input.y, .1f, Time.deltaTime);
                if (_input.magnitude >= 0.1f) MovementUpdate();
            }
            else
            {
                _player.anim.SetFloat(AnimHash.Horizontal, 0, .1f, Time.deltaTime);
                _player.anim.SetFloat(AnimHash.Virtical, 0, .1f, Time.deltaTime);
            }

        }
    }
}
