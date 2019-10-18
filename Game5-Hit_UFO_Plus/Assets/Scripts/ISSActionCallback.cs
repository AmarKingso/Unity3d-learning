using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISSActionCallBack{
	//event为0代表动作正在执行，1为完成
	void SSActionEvent(SSAction src, int events = 1, int int_param = 0, string str_param = null, Object obj_param = null);
}
