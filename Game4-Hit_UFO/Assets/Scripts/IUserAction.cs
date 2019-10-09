using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserAction{
	void HitUFO(Vector3 pos);
	int getScore();
	void GameOver();
	void BeginGame();
	void ReStart();
}
