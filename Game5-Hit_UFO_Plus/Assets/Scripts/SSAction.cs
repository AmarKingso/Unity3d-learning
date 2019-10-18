using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSAction : ScriptableObject{
	public bool destroy = false;	//该动作对象是否需要销毁

	public GameObject gameobject { get; set; }
	public Transform transform { get; set; }
	public ISSActionCallBack callback { get; set; }

	protected SSAction() { }

	public virtual void Start() {
		throw new System.NotImplementedException();
	}

	public virtual void Update() {
		throw new System.NotImplementedException();
	}
}
