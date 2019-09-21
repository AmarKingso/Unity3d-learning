using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour{
	private IUserAction action;

    void Start(){
		action = SSDirector.getInstance().currentSceneController as IUserAction;
    }

    
    void OnGUI(){
		if (action.GameOver() == 0) {
			if(GUI.Button(new Rect(Screen.width / 2 - 50, 100, 100, 40),"Go")) {
				action.MoveBoat();
			}
		}
		else {
			if(GUI.Button(new Rect(Screen.width / 2 - 50, 40, 100, 40), "Try again")) {
				action.PlayAgain();
			}
		}
    }
}
