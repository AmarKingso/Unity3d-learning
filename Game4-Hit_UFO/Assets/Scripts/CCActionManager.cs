using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCActionManager : SSActionManager{
	CCFlyAction fly;
	FirstController sc;

	protected void Start() {
		sc = (FirstController)SSDirector.getInstance().currentSceneController;
		sc.am = this;
	}

	public void UFOFly(GameObject ufo, float speed, float gravity, float angle) {
		fly = CCFlyAction.getSSAction(speed, gravity, angle);
		runAction(ufo, fly, this);
	}
}
