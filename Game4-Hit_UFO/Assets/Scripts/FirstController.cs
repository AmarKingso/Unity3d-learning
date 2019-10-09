using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour, ISceneController, IUserAction{
    public CCActionManager am;
    public DiskFactory disk_factory;
    public UserGUI user_gui;

	private int score = 0;
	private float gravity = 9.8f;
    private Queue<GameObject> disk_queue = new Queue<GameObject>();          //游戏场景中的飞碟队列
    private List<GameObject> disk_notshot = new List<GameObject>();          //没有被打中的飞碟队列
    private int round = 1;                                                   //回合
    private float speed = 2f;                                                //发射一个飞碟的时间间隔
    private bool playing_game = false;                                       //游戏中
    private bool game_over = false;                                          //游戏结束
    private bool game_start = false;                                         //游戏开始
    private int score_round2 = 10;                                           //去到第二回合所需分数
    private int score_round3 = 25;                                           //去到第三回合所需分数

    void Start (){
        SSDirector director = SSDirector.getInstance();     
        director.currentSceneController = this;             
        disk_factory = Singleton<DiskFactory>.Instance;
        am = gameObject.AddComponent<CCActionManager>() as CCActionManager;
        user_gui = gameObject.AddComponent<UserGUI>() as UserGUI;
    }
	
	void Update (){
        if(game_start)
        {
            //游戏结束，取消定时发送飞碟
            if (game_over)
            {
                CancelInvoke("LoadResources");
            }
            //设定一个定时器，发送飞碟，游戏开始
            if (!playing_game)
            {
                InvokeRepeating("LoadResources", 1f, speed);
                playing_game = true;
            }
			//发送飞碟
			launchUFO();
        }
    }

    public void LoadResources(){
        disk_queue.Enqueue(disk_factory.getDisk(round)); 
    }

    private void launchUFO(){
        float position_x = 16;                       
        if (disk_queue.Count != 0){
            GameObject disk = disk_queue.Dequeue();
            disk_notshot.Add(disk);
            disk.SetActive(true);
            //设置被隐藏了或是新建的飞碟的位置
            float ran_y = Random.Range(1f, 4f);
            float ran_x = Random.Range(-1f, 1f) < 0 ? -1 : 1;
            disk.GetComponent<UFOData>().pos = new Vector3(ran_x, ran_y, 0);
            Vector3 position = new Vector3(-disk.GetComponent<UFOData>().pos.x * position_x, ran_y, 0);
            disk.transform.position = position;
			//设置飞碟初始所受的力和角度
			float speed = Random.Range(10, 30);
            float angle = Random.Range(15f, 28f);
			am.UFOFly(disk, speed, gravity, angle);
        }

        for (int i = 0; i < disk_notshot.Count; i++){
            GameObject temp = disk_notshot[i];
            //飞碟飞出摄像机视野也没被打中
            if (temp.transform.position.y < -10 && temp.gameObject.activeSelf == true){
                disk_factory.FreeDisk(disk_notshot[i]);
                disk_notshot.Remove(disk_notshot[i]);
                user_gui.ReduceBlood();
            }
        }
    }

    public void HitUFO(Vector3 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray);
        bool not_hit = false;
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            //射线打中物体
            if (hit.collider.gameObject.GetComponent<UFOData>() != null)
            {
                //射中的物体要在没有打中的飞碟列表中
                for (int j = 0; j < disk_notshot.Count; j++)
                {
                    if (hit.collider.gameObject.GetInstanceID() == disk_notshot[j].gameObject.GetInstanceID())
                    {
                        not_hit = true;
                    }
                }
                if(!not_hit)
                {
                    return;
                }
                disk_notshot.Remove(hit.collider.gameObject);
				score += hit.collider.gameObject.GetComponent<UFOData>().score;
				//等0.1秒后执行回收飞碟
				StartCoroutine(WaitingParticle(0.08f, hit, disk_factory, hit.collider.gameObject));
                break;
            }
        }
    }

    public int getScore(){
        return score;
    }

    public void ReStart(){
        game_over = false;
        playing_game = false;
		score = 0;
        round = 1;
        speed = 2f;
    }
 
    public void GameOver(){
        game_over = true;
    }
 
    IEnumerator WaitingParticle(float wait_time, RaycastHit hit, DiskFactory disk_factory, GameObject obj){
        yield return new WaitForSeconds(wait_time); 
        hit.collider.gameObject.transform.position = new Vector3(0, -9, 0);
        disk_factory.FreeDisk(obj);
    }
    public void BeginGame(){
        game_start = true;
    }
}
