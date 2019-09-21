using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameModel;

public class Click : MonoBehaviour{
	private IUserAction action;
	private RoleModel role;

	private void Start() {
		action = SSDirector.getInstance().currentSceneController as IUserAction;
	}

	private void OnMouseDown() {
		if (role != null)
			action.MoveRole(role);
	}

	public void setRole(RoleModel tmp) {
		role = tmp;
	}
}
