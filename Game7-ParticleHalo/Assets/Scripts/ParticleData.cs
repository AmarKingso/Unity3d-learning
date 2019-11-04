using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleData : MonoBehaviour
{
	public float angle = 0f;
	public float radius = 0f;
	public float speed = 0f;
	public bool clockwise = false;

	public ParticleData(float _angle,float _radius, float _speed, bool _clockwise)
	{
		angle = _angle;
		radius = _radius;
		speed = _speed;
		clockwise = _clockwise;
	}
}
