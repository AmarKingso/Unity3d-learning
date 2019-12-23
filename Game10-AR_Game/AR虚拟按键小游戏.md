## 图片识别与建模
### 环境配置
- **SDK下载**  
登录Vuforia官网下载图中标注出的[SDK](https://developer.vuforia.com/downloads/sdk)，下载后运行安装程序将其安装到与Unity的Editor同级的目录下
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191223132310812.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2Rpb3NtYWlfa2luZ3Nv,size_16,color_FFFFFF,t_70)

- **Unity设置**
1. 打开Unity创建项目后，右键hierarchy栏，可以看到多出一项Vuforia Engine
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191223132701586.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2Rpb3NtYWlfa2luZ3Nv,size_16,color_FFFFFF,t_70)

2. 删除原有Camera，点击创建一个ARCamera；查看其Inspector界面，可以看到其Vuforia Behavior一栏是不可操作的
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191223133123806.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2Rpb3NtYWlfa2luZ3Nv,size_16,color_FFFFFF,t_70)

3. 根据其所给提示，打开```build settings  -> PC,Mac & Linux Standalone -> Player Settings```，在其显示的Inspector界面的XR Settings进行如下配置
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191223133812204.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2Rpb3NtYWlfa2luZ3Nv,size_16,color_FFFFFF,t_70)
设置完成后可以对该元件进行修改，点击打开Configuration  
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191223133959572.png)

4. 进入界面后对两处进行配置  
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191223140552446.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2Rpb3NtYWlfa2luZ3Nv,size_16,color_FFFFFF,t_70)  
首先点击add license进入其官网，点击get development key，下图已经创建好一个license manager，点击进入即可查看到自己的license key，将其拷贝到unity的license key框中即可  
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191223140737545.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2Rpb3NtYWlfa2luZ3Nv,size_16,color_FFFFFF,t_70)  
其次创建DataBase，同样下图已经创建好了一个database  
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191223141247169.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2Rpb3NtYWlfa2luZ3Nv,size_16,color_FFFFFF,t_70)  
点击进入创建好的database，点击add target创建新的target  
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191223153425721.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2Rpb3NtYWlfa2luZ3Nv,size_16,color_FFFFFF,t_70)  
target设置画面如下，添加后会对提供的照片进行处理，星级越高的照片代表效果越好，一般来说使用边缘分明的更容易识别，所以我在上传时选择了类似几何图形和像素图片的照片  
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191223153605816.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2Rpb3NtYWlfa2luZ3Nv,size_16,color_FFFFFF,t_70)  
下载database为unity的package，将其导入项目  
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191223142916224.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2Rpb3NtYWlfa2luZ3Nv,size_16,color_FFFFFF,t_70)  
之前的configuration中的DataBases会自动检测到导入的database  
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191223143248718.png)  
至此环境配置基本完成

### 图片识别
- 创建image Target  
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191223143654389.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2Rpb3NtYWlfa2luZ3Nv,size_16,color_FFFFFF,t_70)
- 在创建的Image Target的inspectors界面中配置如下，即将之前导入的target作为参数  
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191223143804713.png)
- 将要识别的对象挂载到Image Target下，这里方便起见直接挂了个sphere  
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191223154137345.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2Rpb3NtYWlfa2luZ3Nv,size_16,color_FFFFFF,t_70)
- 点击运行即可（PC要有摄像头才能够有画面），效果如下：  
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191223154313425.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2Rpb3NtYWlfa2luZ3Nv,size_16,color_FFFFFF,t_70)
![在这里插入图片描述](https://img-blog.csdnimg.cn/201912231543463.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2Rpb3NtYWlfa2luZ3Nv,size_16,color_FFFFFF,t_70)

## 虚拟按键小游戏
- 添加虚拟按键，点击下图中的```Add Virtual Button```即在Image Target下生成一个虚拟按键子对象

![在这里插入图片描述](https://img-blog.csdnimg.cn/20191223170232530.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2Rpb3NtYWlfa2luZ3Nv,size_16,color_FFFFFF,t_70)
- 对虚拟按键简单设置位置等参数，简单起见使其覆盖识别图  
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191223170430812.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2Rpb3NtYWlfa2luZ3Nv,size_16,color_FFFFFF,t_70)
- 编写脚本，简单实现改点击虚拟按钮随机改变物体颜色的功能，其代码如下：
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class move : MonoBehaviour, IVirtualButtonEventHandler
{
    private GameObject sphere;
    public float step = 1f;
    private Color[] color = { Color.red, Color.blue, Color.yellow, Color.black };
    private System.Random rd = new System.Random();

    void IVirtualButtonEventHandler.OnButtonPressed(VirtualButtonBehaviour vbb) {
        int index = (int)rd.Next(4);
        sphere.GetComponent<MeshRenderer>().material.color = color[index];
        Debug.Log(color[index]);
    }

    void IVirtualButtonEventHandler.OnButtonReleased(VirtualButtonBehaviour vbb) {
        Debug.Log("released");
    }

    // Start is called before the first frame update
    void Start()
    {
        VirtualButtonBehaviour[] vbbs = GetComponentsInChildren<VirtualButtonBehaviour>();
        foreach(var vbb in vbbs) {
            vbb.RegisterEventHandler(this);
        }

        sphere = transform.Find("Sphere").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

```

- 运行视频[传送门](https://v.youku.com/v_show/id_XNDQ4MDAxNTI0OA==.html?spm=a2hzp.8244740.0.0)




