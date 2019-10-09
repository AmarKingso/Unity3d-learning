using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSActionManager : MonoBehaviour, ISSActionCallBack{
	Dictionary<int, SSAction> executing = new Dictionary<int, SSAction>();
	List<SSAction> waitToDo = new List<SSAction>();
	List<int> waitToDestroy = new List<int>();

	protected void Update() {
		foreach(SSAction act in waitToDo) {
			executing[act.GetInstanceID()] = act;
		}
		waitToDo.Clear();

		foreach(KeyValuePair<int, SSAction> kv in executing) {
			SSAction act = kv.Value;
			//当前动作做完就该被销毁，否则继续执行
			if (act.destroy)
				waitToDestroy.Add(act.GetInstanceID());
			else
				act.Update();
		}

		foreach(int key in waitToDestroy) {
			SSAction act = executing[key];
			executing.Remove(key);
			Destroy(act);
		}

		waitToDestroy.Clear();
	}

	public void runAction(GameObject gameobject,SSAction action,ISSActionCallBack manager) {
		action.gameobject = gameobject;
		action.transform = gameobject.transform;
		action.callback = manager;
		waitToDo.Add(action);
		action.Start();
	}

	public void SSActionEvent(SSAction src, int events = 1, int int_param = 0, string str_param = null, Object obj_param = null) {

	}
}
