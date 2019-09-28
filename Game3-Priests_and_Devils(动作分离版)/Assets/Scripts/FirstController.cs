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
	public CCActionManager am;
	Judge judge;
	public float speed = 10f;

	void Awake() {
		SSDirector director = SSDirector.getInstance();
		director.currentSceneController = this;
		userGUI = gameObject.AddComponent<UserGUI>() as UserGUI;
		am = gameObject.AddComponent<CCActionManager>() as CCActionManager;
		judge = gameObject.AddComponent<Judge>() as Judge;
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
			am.moveBoat(boat.getGameObject(), boat.Move(tmp), speed);
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
			am.moveRole(_role.getGameObject(), _role.getMidPosition(tmp.getEmptyPosition()), tmp.getEmptyPosition(), speed);
			_role.getOnCoast(tmp);
			tmp.getOnCoast(_role);
		}
		else {                                          //人在岸上
			if (_role.getPos() == boat.getPos()) {      //人和船在同一边
				if (boat.getEmptyNum() == 0)
					return;
				tmp.getOffCoast(_role.getName());
				am.moveRole(_role.getGameObject(), _role.getMidPosition(boat.getEmptyPosition()), boat.getEmptyPosition(), speed);
				_role.getOnboat(boat);
				boat.getOnBoat(_role);
			}
		}
		userGUI.state = GameOver();
	}

	public int GameOver() {
		return judge.judgment(boat.getPos(), src.getRolesNum(), dst.getRolesNum(), boat.getRolesNum());
	}

	public void PlayAgain() {
		src.reset();
		dst.reset();
		boat.reset();
		for (int i = 0; i < 6; i++)
			role[i].reset(i, src);
	}
}
