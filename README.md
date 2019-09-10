### 解释游戏对象（GameObjects）和 资源（Assets）的区别与联系
#### 区别
**GameObject**：是由unity创建的实例，在场景中所有实际使用的对象都是游戏对象，即出现在场景栏中的对象。

**Asset**：是可以用于游戏中的具体资源，如脚本、图像、视频等，不依赖于unity而存在。 
#### 联系
GameObeject实际上是由asset实例化后的对象，可以由asset创建，同时也能由其保存。

---
### 下载几个游戏案例，分别总结资源、对象组织的结构（指资源的目录组织结构与游戏对象树的层次结构）
**资源的目录组织结构**有纹理、模型、动画、脚本、完整的工程实例、教程和编辑器等等
**游戏对象树的层次结构**有网格、脚本、声音以及灯光等多个游戏对象

---
### 编写一个代码，使用 debug 语句来验证 MonoBehaviour 基本行为或事件触发的条件
代码如下
```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour{
    void Awake() {
        Debug.Log("Awake");
    }
    void Start(){
        Debug.Log("Start");
    }

    void Update(){
        Debug.Log("Update");
    }
    
    void FixedUpdate(){
        Debug.Log("FixedUpdate");
    }
    
    void LateUpdate(){
        Debug.Log("LateUpdate");
    }
    
    void OnGUI(){
        Debug.Log("OnGUI");
    }
    
    void OnDisable(){
        Debug.Log("OnDisable");
    }
    
    void OnEnable(){
        Debug.Log("OnEnable");
    }
}
```


得到如下结果：
![结果上](https://img-blog.csdnimg.cn/20190908214737789.png)![结果下](https://img-blog.csdnimg.cn/20190908214909678.png)
根据console的输出可以知道在运行游戏时程序的生命周期大致是怎么样的

---
### 查找脚本手册，了解 GameObject，Transform，Component 对象
#### 分别翻译官方对三个对象的描述（Description）
**GameObject**：是Unity场景里面所有实体的基类，即是所有其他组件 (Component) 的容器。游戏中的所有对象都是包含不同组件 (Component) 的游戏对象 (GameObject)。
**Transform**：物体的位置、旋转和缩放
**Component**：一切附加到游戏物体的基类
#### 描述下图中 table 对象（实体）的属性、table 的 Transform 的属性、 table 的部件
table的**对象**是GameObject，第一个选择框是activeSelf 属性，第二个选择框是对象名称，第三个选择框是static属性，第四个选择框是选项卡（Tag）属性，第五个选择框是层（Layer）属性，第六个选择框是预设 （Prefabs）属性
table的**Transform**的属性有位置（Position）、旋转（Rotation）和规模（Scale）
table的**部件**有Transform、Mesh filter、Box Collider和Mesh Renderer。
#### 用 UML 图描述 三者的关系
![在这里插入图片描述](https://img-blog.csdnimg.cn/20190908224838965.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2Rpb3NtYWlfa2luZ3Nv,size_16,color_FFFFFF,t_70)

---
### 整理相关学习资料，编写简单代码验证以下技术的实现
#### 查找对象
```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class example : MonoBehaviour{
    GameObject find_obj;
    void Start() {
        find_obj = GameObject.Find("Cube");
    }
    void Update() {
        if (find_obj != null) {
            Debug.Log("find it");
        }
    }
}
```

#### 添加子对象
```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class example : MonoBehaviour {
    void Start() {
        GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube2.transform.position = new Vector3(-3, 0, 0);
    }
}
```

#### 遍历对象树
```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class example : MonoBehaviour {
    void Start() {
        GameObject[] allnode = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject i in allnode)
            Debug.Log(i.name);
    }
}
```

#### 清除所有子对象
```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class example : MonoBehaviour {
    void Start() {
        GameObject root = GameObject.Find("Sphere");
        foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
            GameObject.Destroy(child.gameObject);
    }
}
```

---
### 资源预设（Prefabs）与 对象克隆 (clone) 
#### 预设（Prefabs）有什么好处？
- 可以回收再利用对象，不用每次都从基础游戏对象构建游戏
- 有助于对对象进行修改以及保存

#### 预设与对象克隆 (clone or copy or Instantiate of Unity Object) 关系？
两者都能快速获取相同的游戏对象，区别在于预设是和场景中对应的游戏对象相链接的，当改变预设时，场景中的游戏对象属性也随之改变，而克隆不会改变

#### 制作 table 预制，写一段代码将 table 预制资源实例化成游戏对象
```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class example : MonoBehaviour {
    public GameObject prefab;
    void Start() {
        GameObject instance = (GameObject)Instantiate(prefab, transform.position, transform.rotation);
    }
}
```
保存脚本后，会看到脚本所在的对象的脚本栏中，多出名为prefab的选项，将预制的对象拖入到该选项，点击运行即可

---





