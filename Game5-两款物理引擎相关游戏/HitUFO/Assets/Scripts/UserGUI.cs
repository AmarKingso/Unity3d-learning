using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour{
    private IUserAction action;

	GUIStyle label_style = new GUIStyle();
	GUIStyle detail_style = new GUIStyle();

	void Start (){
        action = SSDirector.getInstance().currentSceneController as IUserAction;
		
		label_style.normal.textColor = Color.black;
		label_style.alignment = TextAnchor.MiddleCenter;
		label_style.fontSize = 24;
		detail_style.normal.textColor = Color.black;
		detail_style.fontSize = 18;
	}
	
	void OnGUI (){
		if (action.getGameState() == GameState.GameReady) {
			if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 20, 100, 40), "游戏开始")) {
				action.setGameState(GameState.RoundReady);
			}
		}
		else if(action.getGameState() == GameState.Running) {
			GUI.Label(new Rect(Screen.width / 2 - 30, 20, 60, 40), "Round " + action.getRound().ToString(), label_style);
			GUI.Label(new Rect(10, 10, 200, 40), "分数: " + action.getScore().ToString(), detail_style);
            GUI.Label(new Rect(10, 50, 100, 40), "生命: " + action.getHealth().ToString(), detail_style);
		}
		else if (action.getGameState() == GameState.Finish) {
			GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 60, 100, 40), "游戏结束", label_style);
			GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 20, 100, 40), "最终得分：" + action.getScore().ToString(), label_style);
			if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 + 20, 100, 40), "重新开始")) {
				action.ReStart();
				action.setGameState(GameState.RoundReady);
			}
		}
    }
}
