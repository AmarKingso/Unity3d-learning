using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { GameReady,RoundReady,Running,Finish};

public interface ISceneController{
	void LoadResources();
}
