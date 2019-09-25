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
			role[i].setPosition(new Vector3(-3.25f - 0.5f * i, 1.25f, 0));
			role[i].setName("priest" + i);
			role[i].getOnCoast(src);
			src.getOnCoast(role[i]);
		}
		for (int i = 3; i < 6; i++) {
			role[i] = new RoleModel(2);
			role[i].setPosition(new Vector3(-3.25f - 0.5f * i, 1.25f, 0));
			role[i].setName("devil" + i);
			role[i].getOnCoast(src);
			src.getOnCoast(role[i]);
		}
	}

	public void MoveBoat() {
		if (boat.getEmptyNum() < 2) {
			//离开岸边就开始检测
			int tmp = boat.getPos();
			boat.setPos(1);
			if ((userGUI.state = GameOver()) == 1)
				return;
			boat.Move(tmp);
			userGUI.state = GameOver();
		}
		else
			return;	
	}

	public void MoveRole(RoleModel _role) {
		CoastModel tmp;
		if (boat.getPos() == 0)             //船在开始岸边
			tmp = src;
		else if (boat.getPos() == 2)        //船在目的岸边
			tmp = dst;
		else
			tmp = null;

		if (_role.getPos() == 1 && boat.getPos() != 1) {             //人在船上
			boat.getOffBoat(_role.getName());
			_role.Move(tmp.getEmptyPosition());
			_role.getOnCoast(tmp);
			tmp.getOnCoast(_role);
		}
		else {                                          //人在岸上
			if (_role.getPos() == boat.getPos()) {      //人和船在同一边
				if (boat.getEmptyNum() == 0)
					return;
				tmp.getOffCoast(_role.getName());
				_role.Move(boat.getEmptyPosition());
				_role.getOnboat(boat);
				boat.getOnBoat(_role);
			}
		}
		userGUI.state = GameOver();
	}

	public int GameOver() {
		int[] srcnum = src.getRolesNum();
		int[] dstnum = dst.getRolesNum();
		int[] boatnum = boat.getRolesNum();

		if (boat.getPos() == 1) {		
			if ((srcnum[0] != 0 && srcnum[0] < srcnum[1]) || (dstnum[0] != 0 && dstnum[0] < dstnum[1]))
				return 1;
		}
		else if(boat.getPos() == 0) {
			if (srcnum[0] != 0 && srcnum[0] + boatnum[0] < srcnum[1] + boatnum[1])
				return 1;
		}
		else {
			if (dstnum[0] != 0 && dstnum[0] + boatnum[0] < dstnum[1] + boatnum[1])
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
			role[i].reset(i, src);
	}
}
