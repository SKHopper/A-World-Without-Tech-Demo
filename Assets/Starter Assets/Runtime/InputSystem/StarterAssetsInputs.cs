using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		//component of type PlayerMovement
		public IPlayerMovement specialMovement;
		//component of type MissileLauncher
		public IMissileLauncher missileComponent;
        //component of type BombLauncher
        public IBombLauncher bombComponent;
		//component of type PlayerCoreBehaviour
		public IPlayerCoreInput playerCoreBehaviour;

#if ENABLE_INPUT_SYSTEM

        public void OnFire(InputValue value)
		{
			if (!Cursor.visible) missileComponent.TryFire();
        }

        public void OnAltFire(InputValue value)
        {
            if (!Cursor.visible) bombComponent.TryFire();
        }

		public void OnInteract(InputValue value)
		{
            if (!Cursor.visible) playerCoreBehaviour.OnInteract();
        }

        public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
        }

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
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
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
			if (specialMovement != null && newMoveDirection.magnitude > 0) specialMovement.Dash(newMoveDirection);
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
		
		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}