using UnityEngine;
using System.Collections;

public class Generate : MonoBehaviour {
	public GameObject gamepadPrefab;
	
	void  Awake (){
		//RE-enable the network messages now we've loaded the right level
		Network.isMessageQueueRunning = true;
		
		if(Network.isServer){
			Debug.Log("Server registered the game at the masterserver.");
			MultiplayerFunctions.SP.RegisterHost(GameSettings.serverTitle, GameSettings.description);
		}
	}
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
					
	[RPC]
	void Spawn(NetworkViewID viewID)
	{
		GameObject clone = Instantiate(gamepadPrefab) as GameObject;
		NetworkView nView;
		nView = clone.GetComponent<NetworkView>();
		nView.viewID = viewID;	
		float scaleX = (float) Screen.width / 800;
		float scaleY = (float) Screen.height / 480;
		
		GameObject actionButton = clone.transform.Find("ActionButton").gameObject;
		GameObject pressed = actionButton.transform.Find("a_pressed").gameObject;
		GameObject up = actionButton.transform.Find("a_up").gameObject;
		Rect newSize = new Rect(pressed.GetComponent<GUITexture>().pixelInset.x * scaleX, 
		                        pressed.GetComponent<GUITexture>().pixelInset.y * scaleY,
		                        pressed.GetComponent<GUITexture>().pixelInset.width * scaleX,
		                        pressed.GetComponent<GUITexture>().pixelInset.height * scaleY);
		pressed.GetComponent<GUITexture>().pixelInset = newSize;                      
		up.GetComponent<GUITexture>().pixelInset = newSize;
		
		GameObject moveJoyStick = clone.transform.Find("MoveJoyStick").gameObject;
		GameObject back = moveJoyStick.transform.Find("analog_back").gameObject;
		GameObject front = moveJoyStick.transform.Find("analog_front").gameObject;
		newSize = new Rect(back.GetComponent<GUITexture>().pixelInset.x * scaleX, 
		                   back.GetComponent<GUITexture>().pixelInset.y * scaleY,
		                   back.GetComponent<GUITexture>().pixelInset.width * scaleX,
		                   back.GetComponent<GUITexture>().pixelInset.height * scaleY);
		back.GetComponent<GUITexture>().pixelInset = newSize;                      
		front.GetComponent<GUITexture>().pixelInset = newSize;					                                         			
	}
	
	[RPC]
	void Reload ()
	{
		Network.isMessageQueueRunning = false;
		Application.LoadLevel(Application.loadedLevel);
	}	
}