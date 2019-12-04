using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour{
	public int state = 0;
	private IUserAction action;
	GUIStyle style;

    void Start(){
		action = SSDirector.getInstance().currentSceneController as IUserAction;
		style = new GUIStyle();
		style.alignment = TextAnchor.MiddleCenter;
    }
   
    void OnGUI(){
		if(state == 0) {
			if (GUI.Button(new Rect(Screen.width / 2 - 50, 100, 100, 40), "Go")) {
				action.MoveBoat();
			}
			if(GUI.Button(new Rect(Screen.width / 2 - 50, 160, 100, 40), "Tip")){
				action.nextStep();
			}
		}
		else if (state == 1) {
			GUI.Label(new Rect(Screen.width / 2 - 50, 100, 100, 40), "GameOver", style);
			if(GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 20, 100, 40),"Try again")) {
				state = 0;
				action.PlayAgain();
			}
		}
		else{
			GUI.Label(new Rect(Screen.width / 2 - 50, 100, 100, 40), "You win!", style);
			if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 20, 100, 40), "Play again")) {
				state = 0;
				action.PlayAgain();
			}
		}
    }
}
