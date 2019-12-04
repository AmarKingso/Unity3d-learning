using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameModel {
	public class BoatModel {
		int pos;                //船只所在位置，0表示开始岸边，1表示途中，2表示目的岸边
		GameObject boat;        //储存游戏对象
		Vector3 start;          //船开始所在坐标
		Vector3 end;            //船结束所在坐标
		Vector3[] src_pos;      //在开始岸边船上能载人的位置
		Vector3[] dst_pos;      //在结束岸边船上能载人的位置
		RoleModel[] passenger;

		public BoatModel() {
			pos = 0;
			start = new Vector3(-2, 0.55f, 0);
			end = new Vector3(2, 0.55f, 0);
			boat = Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Boat"), start, Quaternion.identity);
			boat.name = "boat";
			src_pos = new Vector3[] { new Vector3(-1.5f, 0.8f, 0), new Vector3(-2.5f, 0.8f, 0) };
			dst_pos = new Vector3[] { new Vector3(2.5f, 0.8f, 0), new Vector3(1.5f, 0.8f, 0) };
			passenger = new RoleModel[2];

			boat.AddComponent(typeof(Click));
		}

		//移动船
		public Vector3 Move(int state) {
			if (state == 0) {
				pos = 2;
				return end;
			}
			else if (state == 2) {
				pos = 0;
				return start;
			}
			else
				return Vector3.zero;
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
		}

		public RoleModel getRole(int flag) {
			for (int i = 0; i < passenger.Length; i++) {
				if (passenger[i] != null && passenger[i].getFlag() == flag) 
						return passenger[i];
			}
			return null;
		}
	}

	public class RoleModel {
		int pos;        //角色所在位置，0为开始岸边，1为船上，2为目的岸边
		int flag;       //对象对应的角色，1为priest，2为devil
		GameObject role;
		CoastModel coast;
		Click click;

		public RoleModel(int n) {
			pos = 0;
			flag = n;

			if (flag == 1) 
				role = Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Priest"), Vector3.zero, Quaternion.identity);
			else
				role = Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Devil"), Vector3.zero, Quaternion.identity);

			click = role.AddComponent(typeof(Click)) as Click;
			click.setRole(this);
		}

		public Vector3 getMidPosition(Vector3 dst) {
			Vector3 mid = dst;
			if (pos == 1)
				mid.x = role.transform.position.x;
			else 
				mid.y = role.transform.position.y;
			
			return mid;
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

		public GameObject getGameObject() {
			return role;
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

		public RoleModel getRole(int flag) {
			for (int i = 0; i < empty.Length; i++) {
				if (empty[i] != null && empty[i].getFlag() == flag)
					return empty[i];
			}
			return null;
		}

		public void reset() {
			empty = new RoleModel[6];
		}
	}
}
