using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCActionManager : SSActionManager{
	CCMoveToAction boatmove;
	CCSequenceAction rolemove;
	FirstController sc;

	protected void Start() {
		sc = (FirstController)SSDirector.getInstance().currentSceneController;
		sc.am = this;
	}

	public void moveBoat(GameObject boat,Vector3 pos,float speed) {
		boatmove = CCMoveToAction.getSSAction(pos, speed);
		runAction(boat, boatmove, this);
	}

	public void moveRole(GameObject role, Vector3 mid_pos, Vector3 end_pos, float speed) {
		List<SSAction> list = new List<SSAction>();
		list.Add(CCMoveToAction.getSSAction(mid_pos, speed));
		list.Add(CCMoveToAction.getSSAction(end_pos, speed));
		rolemove = CCSequenceAction.getSSAction(0, 1, list);
		runAction(role, rolemove, this);
	}
}
