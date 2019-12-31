using UnityEngine;

namespace FZeroGXEditor.Editor
{
	public class EditorCamera : MonoBehaviour
	{
		// Camera variables
		public float scrollSpeed = 100f;
		public float lookSpeed = 128f;
		public float panSpeed = 1f;
		public float moveSpeed = 200f;
		public float fastMoveSpeed = 1000f;
		public float slowMoveSpeed = 40f;
		public float sensitivity = 2.5f;
		//public float horizontalFov = 90f;
		public float minimumY = -90f;
		public float maximumY = 90f;
		float rotationY;
		bool mouseLookEnabled;
		bool panEnabled;
		Vector3 panMouseStartPos;

		void Start()
		{
			rotationY = -transform.localEulerAngles.x;
			panMouseStartPos = Vector3.zero;
			mouseLookEnabled = false;
			panEnabled = false;

			// Set fov
			//foreach (Camera cam in Camera.allCameras)
			//	cam.fov = (horizontalFov * Screen.height) / Screen.width;
		}

		void Update()
		{
			if (Input.GetMouseButtonDown(1))
				mouseLookEnabled = true;
			if (Input.GetMouseButtonDown(2))
			{
				panEnabled = true;
				panMouseStartPos = Input.mousePosition;
			}
			if (Input.GetMouseButtonUp(1))
				mouseLookEnabled = false;
			if (Input.GetMouseButtonUp(2))
				panEnabled = false;

			// Camera speed
			float camSpeed;
			if (Input.GetKey(KeyCode.LeftShift))
				camSpeed = fastMoveSpeed;
			else if (Input.GetKey(KeyCode.LeftAlt))
				camSpeed = slowMoveSpeed;
			else
				camSpeed = moveSpeed;

			// Camera movement
			if (!Input.GetKey(KeyCode.LeftControl))
			{
				if (Input.GetKey("w"))
					transform.Translate(camSpeed * Vector3.forward * Time.deltaTime, Space.Self);
				if (Input.GetKey("s"))
					transform.Translate(camSpeed * Vector3.back * Time.deltaTime, Space.Self);
				if (Input.GetKey("a"))
					transform.Translate(camSpeed * Vector3.left * Time.deltaTime, Space.Self);
				if (Input.GetKey("d"))
					transform.Translate(camSpeed * Vector3.right * Time.deltaTime, Space.Self);
			}

			transform.Translate(scrollSpeed * Input.GetAxis("Mouse ScrollWheel") * Vector3.forward, Space.Self);

			// Mouse look
			if (mouseLookEnabled)
			{
				var rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity;

				rotationY += Input.GetAxis("Mouse Y") * sensitivity;
				rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

				transform.eulerAngles = new Vector3(-rotationY, rotationX, 0);
			}

			// Arrow look
			if (Input.GetKey(KeyCode.UpArrow))
			{
				rotationY += lookSpeed * Time.deltaTime;
				rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
				transform.eulerAngles = new Vector3(-rotationY, transform.eulerAngles.y, 0);
			}
			if (Input.GetKey(KeyCode.DownArrow))
			{
				rotationY -= lookSpeed * Time.deltaTime;
				rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
				transform.eulerAngles = new Vector3(-rotationY, transform.eulerAngles.y, 0);
			}
			if (Input.GetKey(KeyCode.LeftArrow))
			{
				var rotationX = transform.localEulerAngles.y - lookSpeed * Time.deltaTime;
				transform.eulerAngles = new Vector3(-rotationY, rotationX, 0);
			}
			if (Input.GetKey(KeyCode.RightArrow))
			{
				var rotationX = transform.localEulerAngles.y + lookSpeed * Time.deltaTime;
				transform.eulerAngles = new Vector3(-rotationY, rotationX, 0);
			}

			// Pan camera
			if (panEnabled)
			{
				Vector2 mouseDelta = (Input.mousePosition - panMouseStartPos) * panSpeed;
				transform.position -= (transform.right * mouseDelta.x) + (transform.up * mouseDelta.y);
				panMouseStartPos = Input.mousePosition;
			}
		}
	}
}
