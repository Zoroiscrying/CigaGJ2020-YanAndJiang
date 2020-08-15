using UnityEngine;

namespace NaughtyCharacter
{
	[CreateAssetMenu(fileName = "PlayerController", menuName = "NaughtyCharacter/PlayerController")]
	public class PlayerController : Controller
	{
		public float ControlRotationSensitivity = 3.0f;

		private PlayerInput _playerInput;
		private PlayerCamera _playerCamera;

		public override void Init()
		{
			_playerInput = FindObjectOfType<PlayerInput>();
			_playerCamera = FindObjectOfType<PlayerCamera>();
		}

		public override void OnCharacterUpdate()
		{
			_playerInput.UpdateInput();

			UpdateControlRotation();
			Character.SetMovementInput(GetMovementInput());
			Character.SetJumpInput(_playerInput.JumpInput);
		}

		public override void OnCharacterFixedUpdate()
		{
			// do camera calculation to turn around moving vectors
			
			// _playerCamera.SetPosition(Character.transform.position);
			// _playerCamera.SetControlRotation(Character.GetControlRotation());
		}

		private void UpdateControlRotation()
		{
			// Vector2 camInput = _playerInput.CameraInput;
			Vector2 controlRotation = Character.GetControlRotation();
			// Vector3 XYZDirection = (Character.transform.position - _playerCamera.transform.position).normalized;
			// Vector2 XZDirection = new Vector2(XYZDirection.x, XYZDirection.z).normalized;
			

			// Adjust the pitch angle (X Rotation)
			float pitchAngle = controlRotation.x;
			// pitchAngle -= camInput.y * ControlRotationSensitivity;

			// Adjust the yaw angle (Y Rotation)
			float yawAngle = controlRotation.y;
			// yawAngle += camInput.x * ControlRotationSensitivity;

			controlRotation = new Vector2(pitchAngle, yawAngle);
			Character.SetControlRotation(controlRotation);
		}

		private Vector3 GetMovementInput()
		{
			// Calculate the move direction relative to the character's yaw rotation
			var transform = Character.transform;
			// Debug.Log(_playerCamera.transform.position);
			Vector3 XYZDirection = (transform.position - _playerCamera.transform.position).normalized;
			XYZDirection = _playerCamera.transform.forward;
			Vector2 XZDirection = new Vector2(XYZDirection.x, XYZDirection.z).normalized;
			Debug.DrawRay(Character.transform.position, XYZDirection*3, Color.blue);
			Quaternion rawRotation = Quaternion.LookRotation(new Vector3(XZDirection.x, 0, XZDirection.y));
			
			// Quaternion yawRotation = Quaternion.Euler(0.0f, Character.GetControlRotation().y, 0.0f);
			// Quaternion yawRotation = Quaternion.Euler(0.0f, rawRotation.y, 0.0f);
			
			Vector3 forward = rawRotation * Vector3.forward;
			Debug.DrawRay(Character.transform.position, forward*3, Color.red);
			Vector3 right = rawRotation * Vector3.right;
			Vector3 movementInput = (forward * _playerInput.MoveInput.y + right * _playerInput.MoveInput.x);

			if (movementInput.sqrMagnitude > 1f)
			{
				movementInput.Normalize();
			}

			return movementInput;
		}
	}
}