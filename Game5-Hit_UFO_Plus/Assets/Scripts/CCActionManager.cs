using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCActionManager : SSActionManager, ISSActionCallBack{
	CCFlyAction fly;
	FirstController sc;

	protected void Start() {
		sc = (FirstController)SSDirector.getInstance().currentSceneController;
		sc.am = this;
	}

	public void SSActionEvent(SSAction src, int events = 1, int int_param = 0, string str_param = null, Object obj_param = null) {
		if(src is CCFlyAction) {
			Singleton<UFOFactory>.Instance.addToFree(src.gameobject);
		}
	}

	public void UFOFly(GameObject ufo, float gravity) {
		fly = CCFlyAction.getSSAction(ufo.GetComponent<UFOData>().speed, gravity, ufo.GetComponent<UFOData>().angle);
		//Debug.Log(ufo.name + ": " + fly.xSpeed);
		runAction(ufo, fly, this);
	}
}
