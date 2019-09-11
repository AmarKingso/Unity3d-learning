using UnityEngine;
using System.Collections;

public class TicTacToe : MonoBehaviour {
	private int[,] cell = new int[3, 3];        //存储3x3棋盘各个格子对应状态：0为未标识；1为叉号；2为圆圈
	private int state;      //记录当前游戏状态，0为游戏开始前；1为进行中；2为结束
	private string UpBar;		//上方提示栏
	private string DownBar;		//下方选择游戏走向按钮
	private int turn;       //谁的回合：0为叉号；1为圆圈
	private int step;		//已经下了的步数

	public Texture2D circle;
	public Texture2D cross;

	/*初始化每一局游戏*/
	void InitGame() {
		for(int i = 0; i < 3; i++) {
			for (int j = 0; j < 3; j++)
				cell[i, j] = 0;
		}
		state = 0;
		turn = 0;
		step = 0;
	}

	/*判断当前这一步是否结束游戏*/
	bool IsWin(int x, int y) {
		for(int i = 1; i < 3; i++) {
			if (cell[x, (y + i) % 3] != cell[x, y]) {
				break;
			}
			if (i == 2)
				return true;
		}
		for (int i = 1; i < 3; i++) {
			if (cell[(x + i) % 3, y] != cell[x, y]) {
				break;
			}
			if (i == 2)
				return true;
		}
		if (x == y) {
			for (int i = 1; i < 3; i++) {
				if (cell[(x + i) % 3, (y + i) % 3] != cell[x, y]) {
					break;
				}
				if (i == 2)
					return true;
			}
		}
		if (x + y == 2) {
			for (int i = 1; i < 3; i++) {
				if (cell[(x + i) % 3, (y - i + 3) % 3] != cell[x, y]) {
					break;
				}
				if (i == 2)
					return true;
			}
		}
		return false;
	}

	void Start() {
		InitGame();
	}

	void OnGUI() {
		/*设置文字风格*/
		GUIStyle textstyle = new GUIStyle();
		textstyle.fontSize = 30;
		textstyle.normal.textColor = new Color(1, 1, 1);
		textstyle.fontStyle = FontStyle.Bold;
		textstyle.alignment = TextAnchor.MiddleCenter;

		GUI.BeginGroup(new Rect(Screen.width / 2 - 125, Screen.height / 2 - 225, 250, 450));
		/*判断当前游戏状态*/
		if (state == 0) {
			UpBar = "等待开始";
			DownBar = "Play";
		}
		else if(state == 1) {
			if (turn == 0)
				UpBar = "X的回合";
			else
				UpBar = "O的回合";
				DownBar = "Reset";
		}
		else {
			if (step == 10)
				UpBar = "平局";
			else if (turn == 1)
				UpBar = "先手获胜";
			else
				UpBar = "后手获胜";
		}

		/*棋盘上方提示文本*/
		GUI.Label(new Rect(25, 30, 200, 50), UpBar, textstyle);

		/*构建棋盘*/
		for(int i = 0; i < 3; i++) {
			for(int j = 0; j < 3; j++) {
				if (cell[i, j] == 1)
					GUI.Button(new Rect(5 + j * 80, 100 + i * 80, 80, 80), cross);
				else if (cell[i, j] == 2)
					GUI.Button(new Rect(5 + j * 80, 100 + i * 80, 80, 80), circle);
				else {
					/*点击按钮且正在游戏中*/
					if (GUI.Button(new Rect(5 + j * 80, 100 + i * 80, 80, 80), "") && state == 1) {
						if (turn == 0) {
							cell[i, j] = 1;
							turn = 1;
						}
						else {
							cell[i, j] = 2;
							turn = 0;
						}
						step++;
						if (IsWin(i, j))
							state = 2;
						else if (step == 9) {
							step++;
							state = 2;
						}
						
					}
				}
			}
		}

		/*构建下方按钮*/
		if(GUI.Button(new Rect(50, 360, 150, 30), DownBar)) {
			/*开始游戏*/
			if (state == 0)
				state = 1;
			else {
				state = 0;
				InitGame();
			}
		}
		
		GUI.EndGroup();
	}

}