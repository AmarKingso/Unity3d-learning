using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judge : MonoBehaviour{
    public int judgment(int flag,int[] srcnum,int[] dstnum,int[] boatnum) {
		//判断游戏完成的条件
		if (dstnum[0] + dstnum[1] == 6)
			return 2;

		//游戏继续
		if (flag == 1) {
			if ((srcnum[0] == 0 || srcnum[0] >= srcnum[1]) && (dstnum[0] == 0 || dstnum[0] >= dstnum[1]))
				return 0;
		}
		else if (flag == 0) {
			if (srcnum[0] + boatnum[0] == 0 || srcnum[0] + boatnum[0] >= srcnum[1] + boatnum[1])
				return 0;
		}
		else {
			if (dstnum[0] + boatnum[0] == 0 || dstnum[0] + boatnum[0] >= dstnum[1] + boatnum[1])
				return 0;
		}

		//游戏失败
		return 1;
	}
}
