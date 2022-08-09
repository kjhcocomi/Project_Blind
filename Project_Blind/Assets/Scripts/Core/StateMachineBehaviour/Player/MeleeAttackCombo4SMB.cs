﻿using UnityEngine;
using UnityEngine.Animations;

namespace Blind
{
    public class MeleeAttackCombo4SMB: SceneLinkedSMB<PlayerCharacter>
    {
        public override void OnSLStateEnter(Animator animator,AnimatorStateInfo stateInfo,int layerIndex) {
            _monoBehaviour.enableAttack();
            _monoBehaviour.AttackableMove(_monoBehaviour._attackMove * _monoBehaviour.GetFacing());
        }

        public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_monoBehaviour.CheckForPowerAttack())
            {
                animator.speed = 0.1f;
            }
            else
            {
                if (_monoBehaviour.isJump)
                {
                    _monoBehaviour.AttackableMove(_monoBehaviour._attackMove * _monoBehaviour.GetFacing());
                }
                _monoBehaviour.enableAttack();
            }
        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            if (!_monoBehaviour.isJump)
            {
                _monoBehaviour.AirborneVerticalMovement(1f);
                _monoBehaviour.UpdateJump();
                _monoBehaviour.CheckForGrounded();
                _monoBehaviour.GroundedHorizontalMovement(true);
                _monoBehaviour.UpdateFacing();
            }
            else _monoBehaviour.GroundedHorizontalMovement(false);
            
            if (_monoBehaviour.CheckForUpKey())
            {
                animator.speed = 1.0f;
                _monoBehaviour._attack.DamageReset(_monoBehaviour._powerAttackdamage);
                _monoBehaviour.enableAttack();
                _monoBehaviour.AttackableMove(_monoBehaviour._attackMove * _monoBehaviour.GetFacing());
            }
        }
        public override void OnSLStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _monoBehaviour._attack.DefultDamage();
            _monoBehaviour.DisableAttack();
            _monoBehaviour.MeleeAttackComoEnd();
        }
    }
}