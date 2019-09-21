using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour{
	public float RevolutionSpeed = 1f;		//公转速度
	public float RotationSpeed = 30f;       //自转速度
	public Vector3 Axis = Vector3.up;

    void Update(){
		transform.RotateAround(transform.parent.position, Axis, RevolutionSpeed * Time.deltaTime);
		transform.Rotate(Vector3.up * RotationSpeed * Time.deltaTime);
	}
}
