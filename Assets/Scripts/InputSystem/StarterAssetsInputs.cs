using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Значения ввода символов")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool block;
		public bool attack;
		public bool kick;
		public bool powerUp;

		[Header("Настройки движения")]
		public bool analogMovement;
		PlayerMovetment playerMovetment;

        [Header("Настройки курсора мыши")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

        void Start()
        {
			playerMovetment = GetComponent<PlayerMovetment>();
        }

#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
            if (cursorInputForLook)
            {
                LookInput(value.Get<Vector2>());
            }
        }

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}
		
		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnBlock(InputValue value)
        {
			BlockInput(value.isPressed);

			if (value.isPressed && playerMovetment.Grounded)
				playerMovetment.IsMove = false;
            else
				playerMovetment.IsMove = true;
		}

		public void OnKick(InputValue value)
        {
			KickInput(value.isPressed);

		}
		public void OnPowerUp(InputValue value)
		{
			PowerUpInput(value.isPressed);

		}

		public void OnAttack(InputValue value)
		{
			AttackInput(value.isPressed);
		}
#endif
		public void KickInput(bool newKickState)
        {
			kick = newKickState;

		}
		public void PowerUpInput(bool newPowerUpState)
		{
			powerUp = newPowerUpState;

		}
		public void BlockInput(bool newBlockState)
        {
			block = newBlockState;
        }

		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		}

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}
		public void AttackInput(bool newAttackSmash)
        {
			attack = newAttackSmash;
		}

        public void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
			Cursor.visible = !newState;
		}
    }
}