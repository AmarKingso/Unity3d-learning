using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCFlyAction : SSAction{

	float initSpeed;	//初速度
	float gravity;		//重力系数
	float angle;        //初速度的角度
	float xSpeed, ySpeed;

	private CCFlyAction() { }

	public static CCFlyAction getSSAction(float speed, float gravity, float angle) {
		CCFlyAction action = CreateInstance<CCFlyAction>();
		action.initSpeed = speed;
		action.gravity = gravity;
		action.angle = angle;
		action.xSpeed = speed * Mathf.Cos(angle);
		action.ySpeed = speed * Mathf.Sin(angle);
		return action;
	}

	public override void Start() {
		//不需要做任何事
	}

	public override void Update() {
		transform.position += new Vector3(Time.deltaTime * xSpeed, (ySpeed - gravity * Time.deltaTime / 2) * Time.deltaTime, 0);
		ySpeed -= gravity * Time.deltaTime;

		if (transform.position.y < -10f) { //飞碟位置小于-10，结束动作
			destroy = true;
			callback.SSActionEvent(this);       //回调告知动作完成
		}
	}
}
