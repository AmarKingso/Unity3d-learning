using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserAction{
	void setGameState(GameState state);
	GameState getGameState();
	int getScore();
	int getRound();
	int getHealth();
	void ReStart();
}
