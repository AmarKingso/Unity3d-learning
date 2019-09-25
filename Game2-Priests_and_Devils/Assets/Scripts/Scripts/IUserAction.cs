using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameModel;

public interface IUserAction{
	void MoveBoat();
	void MoveRole(RoleModel _role);
	int GameOver();
	void PlayAgain();
}
