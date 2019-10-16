using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOFactory : MonoBehaviour {
	public GameObject ufo;      //飞碟实例
	private List<UFOData> used = new List<UFOData>();     //使用中的飞碟
	private Queue<UFOData> free = new Queue<UFOData>();		//空闲的飞碟

	private void Awake() {
		ufo = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/ufo"), Vector3.zero, Quaternion.identity);
		ufo.SetActive(false);
	}

	//得到一个飞碟，参数round用于计算当前关卡飞碟速度
	public GameObject getUFO(int round) {
		GameObject need = null;                     //用于返回的游戏对象
		Color color;                                //飞碟的颜色
		int color_choice = Random.Range(0, 3);      //决定飞碟的颜色
		int side = Random.Range(0, 2);              //飞碟从屏幕左右侧飞出
		int angle = Random.Range(0, 21);            //飞碟飞出角度
		int score;                                  //不同颜色对应的分值
		int scale = Random.Range(2, 5);             //飞碟大小
		float y = Random.Range(4f, 6f);

		if (free.Count > 0)                 //有空闲飞碟直接使用
			need = free.Dequeue().gameObject;
		else {                              //没有就生成新的游戏对象
			need = GameObject.Instantiate<GameObject>(ufo, Vector3.zero, Quaternion.identity);
			need.AddComponent<UFOData>();
		}

		if (color_choice == 0) {
			color = Color.red;
			score = 1;
		}
		else if (color_choice == 1) {
			color = Color.blue;
			score = 2;
		}
		else {
			color = Color.green;
			score = 3;
		}

		need.GetComponent<UFOData>().speed = 2 * round + 2f;
		need.GetComponent<UFOData>().color = color;
		need.GetComponent<UFOData>().scale = new Vector3(scale / 4f, 0.05f, scale / 4f);
		need.GetComponent<UFOData>().score = score + (round + 1) * (5 - scale);
		if (side == 0) {
			need.GetComponent<UFOData>().angle = angle;
			need.GetComponent<UFOData>().pos = new Vector3(-12, y, 0);
		}
		else {
			need.GetComponent<UFOData>().angle = 180 - angle;
			need.GetComponent<UFOData>().pos = new Vector3(12, y, 0);
		}

		used.Add(need.GetComponent<UFOData>());

		return need;
	}

	//将使用中的飞碟回收
	public void addToFree(GameObject obj) {
		foreach(var tmp in used) {
			if (tmp.GetInstanceID() == obj.GetComponent<UFOData>().GetInstanceID()) {
				obj.gameObject.SetActive(false);
				used.Remove(tmp);
				free.Enqueue(tmp);
				break;
			}
		}
	}

}