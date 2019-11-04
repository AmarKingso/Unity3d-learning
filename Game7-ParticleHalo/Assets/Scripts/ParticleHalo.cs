using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHalo : MonoBehaviour
{
	private ParticleSystem par_sys;
	private ParticleSystem.MainModule main;
	private ParticleSystem.Particle[] halo;
	private ParticleData[] par_data;

	public float size = 0.05f;              //粒子大小
	public float time = 9999f;				//粒子生命周期
	public int par_count = 10000;			//粒子数量

	void Start()
    {
		halo = new ParticleSystem.Particle[par_count];
		par_data = new ParticleData[par_count];

		//初始化粒子系统
		par_sys = this.GetComponent<ParticleSystem>();
		main = par_sys.main;
		main.loop = false;
		main.startLifetime = time;
		main.startSpeed = 0f;
		main.startSize = size;
		main.maxParticles = par_count;
		par_sys.Emit(par_count);
		par_sys.GetParticles(halo);

		randomPlace();
    }

    void Update()
    {
		for (int i = 0; i < par_count; i++)
		{
			if (par_data[i].clockwise)			//顺时针
				par_data[i].angle -= Random.Range(0.01f, 0.5f);
			else                                //逆时针
				par_data[i].angle += Random.Range(0.01f, 0.5f);
			par_data[i].angle %= 360f;
			float theta = par_data[i].angle / 180 * Mathf.PI;

			//粒子进行游离
			float r = par_data[i].radius;
			r += Mathf.PingPong(Time.time * par_data[i].speed, 2f);

			halo[i].position = new Vector3(r * Mathf.Cos(theta), r * Mathf.Sin(theta), 0);
		}

		par_sys.SetParticles(halo, halo.Length);
	}

	//随机放置粒子
	private void randomPlace()
	{
		for(int i = 0; i < par_count; i++)
		{
			float r = Random.Range(5f, 12f);
			if (r % 3 == 1)
				r = Random.Range(7f, 10f);
			else if(r%3==2)
				r = Random.Range(8f, 9f);
			float angle = Random.Range(0f, 360f);
			float theta = angle / 180 * Mathf.PI;
			float speed = Random.Range(0.5f, 1.5f);
			int flag = Random.Range(0, 2);
			bool clockwise = (flag == 0) ? false : true;


			halo[i].position = new Vector3(r * Mathf.Cos(theta), r * Mathf.Sin(theta), 0f);
			par_data[i] = new ParticleData(angle, r, speed, clockwise);
		}
		par_sys.SetParticles(halo, halo.Length);
	}
}
