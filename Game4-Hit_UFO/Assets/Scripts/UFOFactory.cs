using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskFactory : MonoBehaviour {
	public GameObject ufo = null;                 //飞碟预制体
	private List<UFOData> used = new List<UFOData>();   //正在被使用的飞碟列表
	private List<UFOData> free = new List<UFOData>();   //空闲的飞碟列表

	public GameObject getDisk(int round) {
		int choice = 0;
		int scope1 = 1, scope2 = 4, scope3 = 7;           //随机的范围
		float y = -10f;                             //刚实例化时的飞碟的竖直位置
		string tag;
		ufo = null;

		//根据回合，随机选择要飞出的飞碟
		if (round == 1) 
			choice = Random.Range(0, scope1);
		else if (round == 2)
			choice = Random.Range(0, scope2);
		else
			choice = Random.Range(0, scope3);
		if (choice <= scope1) 
			tag = "disk1";
		else if (choice <= scope2 && choice > scope1)
			tag = "disk2";
		else 
			tag = "disk3";

		for (int i = 0; i < free.Count; i++) {
			if (free[i].tag == tag) {
				ufo = free[i].gameObject;
				free.Remove(free[i]);
				break;
			}
		}

		if (ufo == null) {
			if(tag == "disk1")
				ufo = Instantiate(Resources.Load<GameObject>("Prefabs/disk1"), new Vector3(0, y, 0), Quaternion.identity);
			else if (tag == "disk2")
				ufo = Instantiate(Resources.Load<GameObject>("Prefabs/disk2"), new Vector3(0, y, 0), Quaternion.identity);
			else
				ufo = Instantiate(Resources.Load<GameObject>("Prefabs/disk3"), new Vector3(0, y, 0), Quaternion.identity);
			//给新实例化的飞碟赋予其他属性
			float ran_x = Random.Range(-1f, 1f) < 0 ? -1 : 1;
			ufo.GetComponent<Renderer>().material.color = ufo.GetComponent<UFOData>().color;
			ufo.GetComponent<UFOData>().pos = new Vector3(ran_x, y, 0);
			ufo.transform.localScale = ufo.GetComponent<UFOData>().scale;
		}
		//添加到使用列表中
		used.Add(ufo.GetComponent<UFOData>());
		return ufo;
	}

	//回收飞碟
	public void FreeDisk(GameObject disk) {
		for (int i = 0; i < used.Count; i++) {
			if (disk.GetInstanceID() == used[i].gameObject.GetInstanceID()) {
				used[i].gameObject.SetActive(false);
				free.Add(used[i]);
				used.Remove(used[i]);
				break;
			}
		}
	}
}