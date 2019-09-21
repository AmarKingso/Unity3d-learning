using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameModel;

public class FirstController : MonoBehaviour, ISceneController, IUserAction {
	CoastModel src;
	CoastModel dst;
	GameObject river;
	BoatModel boat;
	RoleModel[] role;

	UserGUI userGUI;

	void Awake() {
		SSDirector director = SSDirector.getInstance();
		director.currentSceneController = this;
		userGUI = gameObject.AddComponent<UserGUI>() as UserGUI;
		role = new RoleModel[6];
		director.currentSceneController.LoadResources();
		
	}

	public void LoadResources() {
		//载入游戏对象
		src = new CoastModel(0);
		dst = new CoastModel(1);
		river = Instantiate<GameObject>(Resources.Load<GameObject>("prefabs/River"), new Vector3(0, 0.25f, 0), Quaternion.identity);
		river.name = "river";
		boat = new BoatModel();
		for (int i = 0; i < 3; i++) {
			role[i] = new RoleModel(1);
			role[i].SetPosition(new Vector3(-3.25f - 0.5f * i, 1.25f, 0));
			role[i].SetName("priest" + i);
			role[i].SetInCoast(i);
		}
		for (int i = 3; i < 6; i++) {
			role[i] = new RoleModel(2);
			role[i].SetPosition(new Vector3(-3.25f - 0.5f * i, 1.25f, 0));
			role[i].SetName("devil" + i);
			role[i].SetInCoast(i);
		}
	}

	public void MoveBoat() {
		for (int i = 0; i < 6; i++) {
			if (role[i].getInboat() == 0) {
				if (boat.getPos() == 0)
					role[i].Move(new Vector3(2.5f, 0.8f, 0));
				else
					role[i].Move(new Vector3(-1.5f, 0.8f, 0));
			}
			if (role[i].getInboat() == 1) {
				if (boat.getPos() == 0)
					role[i].Move(new Vector3(1.5f, 0.8f, 0));
				else
					role[i].Move(new Vector3(-2.5f, 0.8f, 0));
			}
		}
		if (boat.EmptyNum() == 2 || boat.getPos() == 1)
			return;
		boat.Move();
		
	}

	public void MoveRole(RoleModel _role) {
		if (boat.EmptyNum() == 0 || boat.getPos() == 1)		
			return;

		//船上有空位且停靠在岸边
		if (_role.getPos() == 1) {				//人在船上
			if (boat.getPos() == 0) {               //船在开始岸边
				int index_c = src.getEmptyIndex();
				int index_b = _role.getInboat();

				_role.Move(src.getEmptyPosition());

				_role.change(0, -1, index_c);
				boat.setPassenger(index_b, 0);
				src.setEmpty(index_c, _role.getFlag());
			}
			else if (boat.getPos() == 2) {        //船在目的岸边
				int index_c = dst.getEmptyIndex();
				int index_b = _role.getInboat();

				_role.Move(dst.getEmptyPosition());

				_role.change(2, -1, index_c);
				boat.setPassenger(index_b, 0);
				dst.setEmpty(index_c, _role.getFlag());
			}
		}
		else {              //人在岸上
			if (_role.getPos() == boat.getPos()) {      //人和船在同一边
				int index_b = boat.EmptyIndex();
				int index_c = _role.getIncoast();

				_role.Move(boat.getEmptyPosition());

				_role.change(1, index_b, -1);
				boat.setPassenger(index_b, _role.getFlag());
				src.setEmpty(index_c, 0);
			}
			else
				return;
		}
	}

	public int GameOver() {
		int[] srcnum = src.getRolesNum();
		int[] dstnum = dst.getRolesNum();
		int[] boatnum = boat.getRolesNum();

		if (boat.getPos() == 1) {		
			if (srcnum[0] < srcnum[1] || dstnum[0] < dstnum[1])
				return 1;
		}
		else if(boat.getPos() == 0) {
			if (srcnum[0] + boatnum[0] < srcnum[1] + boatnum[1])
				return 1;
		}
		else {
			if (dstnum[0] + boatnum[0] < dstnum[1] + boatnum[1])
				return 1;
		}
		if (dst.getEmptyIndex() == -1)
			return 2;

		return 0;
	}

	public void PlayAgain() {
		src.reset();
		dst.reset();
		boat.reset();
		for (int i = 0; i < 6; i++)
			role[i].reset(i);
	}
}
