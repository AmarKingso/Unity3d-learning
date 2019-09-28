using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCSequenceAction : SSAction, ISSActionCallBack {
	int index = 0;      //动作列表的索引
	int times = -1;      //组合动作执行次数,-1代表无限循环
	public List<SSAction> act_list;

	private CCSequenceAction() { }

	public override void Start() {
		foreach(SSAction act in act_list) {
			act.gameobject = gameobject;
			act.transform = transform;
			act.callback = this;
			act.Start();
		}
	}

	public override void Update() {
		if (act_list.Count == 0)
			return;
		if (index < act_list.Count)
			act_list[index].Update();
	}

	public static CCSequenceAction getSSAction(int index,int times,List<SSAction> list) {
		CCSequenceAction action = ScriptableObject.CreateInstance<CCSequenceAction>();
		action.index = index;
		action.times = times;
		action.act_list = list;
		return action;
	}

	public void SSActionEvent(SSAction src, int events = 1, int int_param = 0, string str_param = null, Object obj_param = null) {
		src.destroy = false;
		index++;
		if (index >= act_list.Count) {
			index = 0;
			if (times > 0)
				times--;
			if (times == 0) {
				destroy = true;
				callback.SSActionEvent(this);
			}
		}
	}

	void OnDestroy() {

	}
}
