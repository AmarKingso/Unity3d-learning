using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameModel;

enum MoveStrategy { P1D0, P2D0, P1D1, P0D1, P0D2, END, INVALID}

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

	MoveStrategy getNextMoveStratrgy() {
		int[] boat_roles = boat.getRolesNum();
		int[] total = new int[2];       //船和其停靠岸边一共有的角色

		//在初始岸边
		if (boat.getPos() == 0) {
			int[] src_roles = src.getRolesNum();
			for (int i = 0; i < 2; i++)
				total[i] = boat_roles[i] + src_roles[i];

			if (total[0] == 0 && total[1] == 1)
				return MoveStrategy.P0D1;
			else if (total[0] == 0 && total[1] == 2)
				return MoveStrategy.P0D2;
			else if (total[0] == 0 && total[1] == 3)
				return MoveStrategy.P0D2;
			else if (total[0] == 1 && total[1] == 1)
				return MoveStrategy.P1D1;
			else if (total[0] == 2 && total[1] == 2)
				return MoveStrategy.P2D0;
			else if (total[0] == 3 && total[1] == 1)
				return MoveStrategy.P2D0;
			else if (total[0] == 3 && total[1] == 2)
				return MoveStrategy.P0D2;
			else if (total[0] == 3 && total[1] == 3)
				return MoveStrategy.P0D2;
			else
				return MoveStrategy.INVALID;
		}
		else if (boat.getPos() == 2) {
			int[] dst_roles = dst.getRolesNum();
			for (int i = 0; i < 2; i++)
				total[i] = boat_roles[i] + dst_roles[i];

			if (total[0] == 0 && total[1] == 1)
				return MoveStrategy.P0D1;
			else if (total[0] == 0 && total[1] == 2)
				return MoveStrategy.P0D1;
			else if (total[0] == 0 && total[1] == 3)
				return MoveStrategy.P0D1;
			else if (total[0] == 1 && total[1] == 1)
				return MoveStrategy.P1D0;
			else if (total[0] == 2 && total[1] == 2)
				return MoveStrategy.P1D1;
			else if (total[0] == 3 && total[1] == 1)
				return MoveStrategy.P0D1;
			else if (total[0] == 3 && total[1] == 2)
				return MoveStrategy.P0D1;
			else if (total[0] == 3 && total[1] == 3)
				return MoveStrategy.END;
			else
				return MoveStrategy.INVALID;
		}
		else
			return MoveStrategy.INVALID;
	}

	public void nextStep() {
		MoveStrategy strategy = getNextMoveStratrgy();
		int[] boat_roles = boat.getRolesNum();
		int[] right_boat = new int[2];
		if (strategy == MoveStrategy.P0D1) {
			right_boat[0] = 0;
			right_boat[1] = 1;
		}
		else if (strategy == MoveStrategy.P0D2) {
			right_boat[0] = 0;
			right_boat[1] = 2;
		}
		else if (strategy == MoveStrategy.P1D1) {
			right_boat[0] = 1;
			right_boat[1] = 1;
		}
		else if (strategy == MoveStrategy.P1D0) {
			right_boat[0] = 1;
			right_boat[1] = 0;
		}
		else if (strategy == MoveStrategy.P2D0) {
			right_boat[0] = 2;
			right_boat[1] = 0;
		}
		else if(strategy == MoveStrategy.END) {
			right_boat[0] = 0;
			right_boat[1] = 0;
		}
		else
			return;

		int need_p = right_boat[0] - boat_roles[0];		//船上还需要的牧师数量
		int need_d = right_boat[1] - boat_roles[1];     //船上还需要的魔鬼数量
		
		while(need_p < 0) {
			MoveRole(boat.getRole(1));
			need_p++;
		}
		while (need_d < 0) {
			MoveRole(boat.getRole(2));
			need_d++;
		}
		if (boat.getPos() == 0) {
			while (need_p > 0) {
				MoveRole(src.getRole(1));
				need_p--;
			}
			while (need_d > 0) {
				MoveRole(src.getRole(2));
				need_d--;
			}
		}
		else if (boat.getPos() == 2) {
			while (need_p > 0) {
				MoveRole(dst.getRole(1));
				need_p--;
			}
			while (need_d > 0) {
				MoveRole(dst.getRole(2));
				need_d--;
			}
		}
	}
}
