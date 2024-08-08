using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace TheAiAlchemist
{
	[CreateAssetMenu(fileName = "InputReader", menuName = "TheAiAlchemist/Settings/InputReader")]
	public class InputReaderSO : ScriptableObject, GameInput.IGeneralInputActions
	{
		// Gameplay
		public UnityEvent jumpEvent;
		public UnityEvent<Vector3> clickEvent;
		// public UnityEvent jumpCanceledEvent;
		// public UnityEvent attackEvent;
		// public UnityEvent interactEvent; // Used to talk, pickup objects, interact with tools like the cooking cauldron
		// public UnityEvent extraActionEvent; // Used to bring up the inventory
		// public UnityEvent pauseEvent;
		// public UnityEvent<Vector2> moveEvent;
		// public UnityEvent<Vector2, bool> cameraMoveEvent;
		// public UnityEvent enableMouseControlCameraEvent;
		// public UnityEvent disableMouseControlCameraEvent;

		// Dialogue
		// public UnityEvent advanceDialogueEvent;
		// public UnityEvent onMoveSelectionEvent;

		private GameInput gameInput;

		private void OnEnable()
		{
			if (gameInput == null)
			{
				gameInput = new GameInput();
				gameInput.GeneralInput.SetCallbacks(this);
			}

			EnableGameplayInput();
		}

		private void OnDisable()
		{
			DisableAllInput();
		}
		
		public void OnJump(InputAction.CallbackContext context)
		{
			if (jumpEvent != null
			    && context.phase == InputActionPhase.Performed)
				jumpEvent.Invoke();

			// if (jumpCanceledEvent != null
			//     && context.phase == InputActionPhase.Canceled)
			// 	jumpCanceledEvent.Invoke();
		}

		public void OnClick(InputAction.CallbackContext context)
		{
			// Debug.Log("Click still works");
			if (clickEvent != null
			    && context.phase == InputActionPhase.Performed)
				clickEvent.Invoke(Mouse.current.position.value);
		}

		// public void OnAttack(InputAction.CallbackContext context)
		// {
		// 	if (attackEvent != null
		// 	    && context.phase == InputActionPhase.Performed)
		// 		attackEvent.Invoke();
		// }
		//
		// public void OnExtraAction(InputAction.CallbackContext context)
		// {
		// 	if (extraActionEvent != null
		// 	    && context.phase == InputActionPhase.Performed)
		// 		extraActionEvent.Invoke();
		// }
		//
		// public void OnInteract(InputAction.CallbackContext context)
		// {
		// 	if (interactEvent != null
		// 	    && context.phase == InputActionPhase.Performed)
		// 		interactEvent.Invoke();
		// }
		//
		// public void OnMove(InputAction.CallbackContext context)
		// {
		// 	if (moveEvent != null)
		// 	{
		// 		moveEvent.Invoke(context.ReadValue<Vector2>());
		// 	}
		// }
		//
		// public void OnPause(InputAction.CallbackContext context)
		// {
		// 	if (pauseEvent != null
		// 	    && context.phase == InputActionPhase.Performed)
		// 		pauseEvent.Invoke();
		// }
		//
		// public void OnRotateCamera(InputAction.CallbackContext context)
		// {
		// 	if (cameraMoveEvent != null)
		// 	{
		// 		cameraMoveEvent.Invoke(context.ReadValue<Vector2>(), IsDeviceMouse(context));
		// 	}
		// }
		//
		// public void OnMouseControlCamera(InputAction.CallbackContext context)
		// {
		// 	if (context.phase == InputActionPhase.Performed)
		// 		enableMouseControlCameraEvent?.Invoke();
		//
		// 	if (context.phase == InputActionPhase.Canceled)
		// 		disableMouseControlCameraEvent?.Invoke();
		// }

		// private bool IsDeviceMouse(InputAction.CallbackContext context) => context.control.device.name == "Mouse";
		//
		// public void OnMoveSelection(InputAction.CallbackContext context)
		// {
		// 	if (context.phase == InputActionPhase.Performed)
		// 		onMoveSelectionEvent.Invoke();
		// }
		//
		// public void OnAdvanceDialogue(InputAction.CallbackContext context)
		// {
		// 	if (context.phase == InputActionPhase.Performed)
		// 		advanceDialogueEvent.Invoke();
		// }

		// public void EnableDialogueInput()
		// {
		// 	gameInput.Dialogues.Enable();
		// 	gameInput.GeneralInput.Disable();
		// }

		public void EnableGameplayInput()
		{
			gameInput.GeneralInput.Enable();
		}

		public void DisableAllInput()
		{
			gameInput.GeneralInput.Disable();
		}

		public bool CheckGameInputEnable()
		{
			return gameInput.GeneralInput.enabled;
		}
	}
}
