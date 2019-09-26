using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameModel {
	public class Move: MonoBehaviour {
		int move_mod = 0;       //移动方式，0为静止，1为向中间点移动，2为向终点移动
		float speed = 10f;
		Vector3 mid_pos;
		Vector3 dst_pos;

		private void Update() {
			if (move_mod == 1) {
				transform.position = Vector3.MoveTowards(transform.position, mid_pos, speed * Time.deltaTime);
				if (transform.position == mid_pos)
					move_mod = 2;
			}
			else if (move_mod == 2) {
				transform.position = Vector3.MoveTowards(transform.position, dst_pos, speed * Time.deltaTime);
				if (transform.position == dst_pos)
					move_mod = 0;
			}
		}

		public void MoveToPosition(Vector3 pos) {
			dst_pos = pos;
			mid_pos = pos;
			if (pos.y == transform.position.y) {        //同一水平面上
				move_mod = 2;
			}
			else if (pos.y < transform.position.y) {      //目的地低于当前位置
				mid_pos.y = transform.position.y;
				move_mod = 1;
			}
			else {                                       //目的地高于当前位置
				mid_pos.x = transform.position.x;
				move_mod = 1;
			}
		}

		public void reset() {
			move_mod = 0;
		}
	}

	public class BoatModel {
		int pos;				//船只所在位置，0表示开始岸边，1表示途中，2表示目的岸边
		GameObject boat;        //储存游戏对象
		Vector3 start;			//船开始所在坐标
		Vector3 end;			//船结束所在坐标
		Vector3[] src_pos;      //在开始岸边船上能载人的位置
		Vector3[] dst_pos;      //在结束岸边船上能载人的位置
		RoleModel[] passenger;
		Move move;

		public BoatModel() {
			pos = 0;
			start = new Vector3(-2, 0.55f, 0);
			end = new Vector3(2, 0.55f, 0);
			boat = Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Boat"), start, Quaternion.identity);
			boat.name = "boat";
			src_pos = new Vector3[] { new Vector3(-1.5f, 0.8f, 0), new Vector3(-2.5f, 0.8f, 0) };
			dst_pos = new Vector3[] { new Vector3(2.5f, 0.8f, 0), new Vector3(1.5f, 0.8f, 0) };
			passenger = new RoleModel[2];

			move = boat.AddComponent(typeof(Move)) as Move;
			boat.AddComponent(typeof(Click));
		}

		//移动船
		public void Move(int state) {
			if (state == 0) {
				move.MoveToPosition(end);
				pos = 2;
			}
			else if (state == 2){
				move.MoveToPosition(start);
				pos = 0;
			}
		}

		public void setPos(int n) {
			pos = n;
		}

		public int getPos() {
			return pos;
		}

		//检查船上有多少空位
		public int getEmptyNum() {
			int res = 0;
			for (int i = 0; i < passenger.Length; i++) {
				if (passenger[i] == null)
					res++;
			}
			return res;
		}

		public int getEmptyIndex() {
			for (int i = 0; i < passenger.Length; i++) {
				if (passenger[i] == null)
					return i;
			}
			return -1;
		}

		public Vector3 getEmptyPosition() {
			if (pos == 0)
				return src_pos[getEmptyIndex()];
			else if (pos == 2)
				return dst_pos[getEmptyIndex()];
			else
				return Vector3.zero;
		}

		public int[] getRolesNum() {
			int[] num = { 0, 0 };
			for (int i = 0; i < passenger.Length; i++) {
				if (passenger[i] != null) {
					if (passenger[i].getFlag() == 1)
						num[0]++;
					else if (passenger[i].getFlag() == 2)
						num[1]++;
				}
			}
			return num;
		}

		public GameObject getGameObject() {
			return boat;
		}

		public void getOnBoat(RoleModel p) {
			passenger[getEmptyIndex()] = p;
		}

		public RoleModel getOffBoat(string pname) {
			for (int i = 0; i < passenger.Length; i++) {
				if (passenger[i] != null && passenger[i].getName() == pname) {
					RoleModel tmp = passenger[i];
					passenger[i] = null;
					return tmp;
				}
			}
			return null;
		}

		public void reset() {
			pos = 0;
			boat.transform.position = start;
			passenger = new RoleModel[2];
			move.reset();
		}
	}

	public class RoleModel {
		int pos;        //角色所在位置，0为开始岸边，1为船上，2为目的岸边
		int flag;       //对象对应的角色，1为priest，2为devil
		GameObject role;
		CoastModel coast;
		Move move;
		Click click;

		public RoleModel(int n) {
			pos = 0;
			flag = n;

			if (flag == 1) 
				role = Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Priest"), Vector3.zero, Quaternion.identity);
			else
				role = Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Devil"), Vector3.zero, Quaternion.identity);

			move = role.AddComponent(typeof(Move)) as Move;
			click = role.AddComponent(typeof(Click)) as Click;
			click.setRole(this);
		}
		public void Move(Vector3 dst) {
			move.MoveToPosition(dst);
		}

		public void setPosition(Vector3 p) {
			role.transform.position = p;
		}

		public void setName(string name) {
			role.name = name;
		}

		public string getName() {
			return role.name;
		}

		public int getPos() {
			return pos;
		}

		public int getFlag() {
			return flag;
		}

		public void getOnboat(BoatModel b) {
			coast = null;
			role.transform.parent = b.getGameObject().transform;
			pos = 1;
		}

		public void getOnCoast(CoastModel c) {
			coast = c;
			role.transform.parent = null;
			if (c.getType() == 0)
				pos = 0;
			else
				pos = 2;
		}

		//上船
		public void reset(int n, CoastModel c) {
			pos = 0;
			role.transform.parent = null;
			role.transform.position = new Vector3(-3.25f - 0.5f * n, 1.25f, 0);
			coast = c;
			coast.setToEmpty(this, n);
			move.reset();		
		}
	}

	public class CoastModel {
		int type;			//0为开始岸边，1为目的岸边
		GameObject coast;
		Vector3[] pos;      //岸上放置角色的位置
		RoleModel[] empty;

		public CoastModel(int n) {
			type = n;
			empty = new RoleModel[6];
			if (type == 0) {
				coast = Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Coast"), new Vector3(-4.5f, 0.5f, 0), Quaternion.identity);
				coast.name = "srcCoast";
				pos = new Vector3[] { new Vector3(-3.25f, 1.25f, 0), new Vector3(-3.75f, 1.25f, 0), new Vector3(-4.25f, 1.25f, 0), new Vector3(-4.75f, 1.25f, 0), new Vector3(-5.25f, 1.25f, 0), new Vector3(-5.75f, 1.25f, 0) };
			}
			else {
				coast = Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Coast"), new Vector3(4.5f, 0.5f, 0), Quaternion.identity);
				coast.name = "dstCoast";
				pos = new Vector3[] { new Vector3(3.25f, 1.25f, 0), new Vector3(3.75f, 1.25f, 0), new Vector3(4.25f, 1.25f, 0), new Vector3(4.75f, 1.25f, 0), new Vector3(5.25f, 1.25f, 0), new Vector3(5.75f, 1.25f, 0) };
			}		
		}

		public void setToEmpty(RoleModel r,int index) {
			empty[index] = r;
		}

		public int[] getRolesNum() {
			int[] num = { 0, 0 };
			for(int i = 0; i < empty.Length; i++) {
				if (empty[i] != null) {
					if (empty[i].getFlag() == 1)
						num[0]++;
					else
						num[1]++;
				}
			}
			return num;
		}

		public Vector3 getEmptyPosition() {
			return pos[getEmptyIndex()];
		}

		public int getEmptyIndex() {
			for (int i = 0; i < empty.Length; i++) {
				if (empty[i] == null)
					return i;
			}
			return -1;
		}

		public int getType() {
			return type;
		}

		public void getOnCoast(RoleModel r) {
			empty[getEmptyIndex()] = r;
		}

		public RoleModel getOffCoast(string rname) {
			for (int i = 0; i < empty.Length; i++) {
				if (empty[i] != null && empty[i].getName() == rname) {
					RoleModel tmp = empty[i];
					empty[i] = null;
					return tmp;
				}
			}
			return null;
		}

		public void reset() {
			empty = new RoleModel[6];
		}
	}
}
