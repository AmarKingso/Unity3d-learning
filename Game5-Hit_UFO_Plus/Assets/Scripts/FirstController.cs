using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour, ISceneController, IUserAction{
    public CCActionManager am;
    public UFOFactory ufo_factory;
    public UserGUI user_gui;

	int score = 0;                                                  //分数
	int health = 5;                                          //血量
	int round = 0;                                                  //回合
	float gravity = 1f;												//所受重力
	float time = 0;													//发射时间间隔
	GameState gs = GameState.GameReady;								//游戏状态
    Queue<GameObject> ufo_queue = new Queue<GameObject>();          //游戏场景中的飞碟队列
	List<GameObject> no_shot_list = new List<GameObject>();			//未被击中的飞碟列表
    
    void Awake (){
        SSDirector director = SSDirector.getInstance();     
        director.currentSceneController = this;
		this.gameObject.AddComponent<UFOFactory>();
		ufo_factory = Singleton<UFOFactory>.Instance;
        am = gameObject.AddComponent<CCActionManager>() as CCActionManager;
        user_gui = gameObject.AddComponent<UserGUI>() as UserGUI;
		this.LoadResources();
    }
	
	void Update (){
		if(gs == GameState.Running) {
			if (health == 0)
				gs = GameState.Finish;
			else {
				if (Input.GetButton("Fire1")) {
					Vector3 pos = Input.mousePosition;
					hitUFO(pos);
				}

				//判断飞碟是否飞出视角
				foreach(var tmp in no_shot_list) {
					if (tmp.transform.position.x < -13|| tmp.transform.position.x > 13|| tmp.transform.position.y < 0) {
						health--;
						no_shot_list.Remove(tmp);
						break;
					}
				}

				//隔一段时间发射一个飞碟
				if (time > 1.5 - round * 0.1f) {
					launchUFO();
					time = 0;

					//当前轮结束，进入下一轮
					if (no_shot_list.Count == 0) {
						gs = GameState.RoundReady;
					}
				}
				else
					time += Time.deltaTime;
			}
		}

		if (gs == GameState.RoundReady && health > 0) {
			gs = GameState.Running;
			round++;
			time = 0;
			nextRound();
		}
	
    }

	void nextRound() {
		for (int i = 0; i < 10; i++) {
			GameObject tmp = ufo_factory.getUFO(round);
			tmp.name = "ufo" + (i + 1);
			ufo_queue.Enqueue(tmp);
		}
	}

    public void LoadResources(){
		 
    }

    private void launchUFO(){
		if (ufo_queue.Count != 0) {
			GameObject ufo = ufo_queue.Dequeue();

			ufo.transform.position = ufo.GetComponent<UFOData>().pos;
			ufo.transform.localScale = ufo.GetComponent<UFOData>().scale;
			ufo.GetComponent<Renderer>().material.color = ufo.GetComponent<UFOData>().color;
			ufo.SetActive(true);

			no_shot_list.Add(ufo);
			am.UFOFly(ufo, gravity);
		}
    }

    public void hitUFO(Vector3 pos){
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray);

        foreach (var hit in hits){
            if (hit.collider.gameObject.GetComponent<UFOData>() != null) {
				//Debug.Log(hit.collider.gameObject.name);
				score += hit.collider.gameObject.GetComponent<UFOData>().score;
				no_shot_list.Remove(hit.collider.gameObject);
				hit.collider.gameObject.transform.position = new Vector3(0, -9f, 0);
			}
        }
    }

	public void setGameState(GameState state) {
		gs = state;
	}

	public GameState getGameState() {
		return gs;
	}

    public int getScore(){
        return score;
    }

	public int getRound() {
		return round;
	}

	public int getHealth() {
		return health;
	}

	public void ReStart(){
		score = 0;
		health = 5;
		round = 0;
		time = 0;
		ufo_queue.Clear();
    }
}
