using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour {
	public float clientHInput = 0;
	public float clientVInput = 0;
	public bool isTouch = false;
	public bool action = false;
	
	void  Awake (){
		//RE-enable the network messages now we've loaded the right level
		Network.isMessageQueueRunning = true;
		
		if (Network.isServer) {
			Debug.Log ("Server registered the game at the masterserver.");
			MultiplayerFunctions.SP.RegisterHost (GameSettings.serverTitle, GameSettings.description);
		}
	}	
	
	// Update is called once per frame
	void Update (){
	}
	
	// Die by collision
	void OnCollisionEnter2D(Collision2D other)
	{
		Die();
	}
	
	void Die()
	{
		Application.LoadLevel(Application.loadedLevel);
	}
	
	void FixedUpdate() {
		VCAnalogJoystickBase moveJoystick = VCAnalogJoystickBase.GetInstance("MoveJoyStick");
		VCButtonBase actionButton = VCButtonBase.GetInstance("Action");
		Vector2 directionVector = new Vector2(moveJoystick.AxisX, moveJoystick.AxisY);
		if (directionVector != Vector2.zero){
			// Get the length of the directon vector and then normalize it
			// Dividing by the length is cheaper than normalizing when we already have the length anyway
			var directionLength = directionVector.magnitude;
			directionVector = directionVector / directionLength;
			
			// Make sure the length is no bigger than 1
			directionLength = Mathf.Min(1.0f, directionLength);
			
			// Make the input vector more sensitive towards the extremes and less sensitive in the middle
			// This makes it easier to control slow speeds when using analog sticks
			directionLength = directionLength * directionLength;
			
			// Multiply the normalized direction vector by the modified length
			directionVector = directionVector * directionLength;
		}
		
		GetComponent<NetworkView>().RPC("SendInput", RPCMode.Server, directionVector.x, directionVector.y, 
		                actionButton.Pressed, Input.touchCount > 0);
	}
	
	[RPC]
	void SendInput (float HInput, float VInput, bool actionButtonPressed, bool touchInput)
	{
		clientHInput = HInput;
		clientVInput = VInput;
		action = actionButtonPressed;
		isTouch = touchInput;
	}
	
	public static void AutoResize(int screenWidth, int screenHeight){
		Vector2 resizeRatio = new Vector2((float)Screen.width / screenWidth, (float)Screen.height / screenHeight);
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(resizeRatio.x, resizeRatio.y, 1.0f));
	}	
}