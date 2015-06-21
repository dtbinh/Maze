using UnityEngine;

public class FirstPersonController : MonoBehaviour {
	
	public float mouseSensitivity = 2f;
	public float baseSpeed = 6.0f;
	public float currentSpeed;
	public float diveSlow = 2.0f;

	Vector3 moveDir = Vector3.zero;
	float verticalRotation;
	const float UP_DOWN_RANGE = 80f;

	public static CharacterController controller;

	// Use this for initialization
	void Start () {
		mouseSensitivity = GameMaster.mouseSensitivity;
		currentSpeed = baseSpeed;
		controller = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		// Camera rotate up and down
		float rotLeftRight = Input.GetAxis("Mouse X") * mouseSensitivity;
		transform.Rotate(0,rotLeftRight,0);
		// Camera rotate left and right
		verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
		verticalRotation = Mathf.Clamp(verticalRotation, -UP_DOWN_RANGE, UP_DOWN_RANGE);
		Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation,0,0);
		// Movement control
		float diveSpeed = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))?1f:diveSlow;
		moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		moveDir = transform.TransformDirection(moveDir);
		moveDir *= currentSpeed;
		moveDir.y = (Input.GetButton("Jump") || Input.GetKey (KeyCode.RightControl))? currentSpeed:-currentSpeed/diveSpeed;
		controller.Move(moveDir * Time.deltaTime);
	}
}
