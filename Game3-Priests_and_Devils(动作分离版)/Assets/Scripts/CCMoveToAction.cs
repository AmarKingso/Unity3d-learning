using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCMoveToAction : SSAction{
	public Vector3 target;
	public float speed;

	private CCMoveToAction() { }

    public static CCMoveToAction getSSAction(Vector3 target,float speed) {
		CCMoveToAction action = ScriptableObject.CreateInstance<CCMoveToAction>();
		action.target = target;
		action.speed = speed;
		return action;
	}

	public override void Start() {
		//不需要做任何事
	}

	public override void Update() {
		transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

		if (transform.position == target) {	//到达目的地
			destroy = true;
			callback.SSActionEvent(this);		//回调告知动作完成
		}
	}
}
