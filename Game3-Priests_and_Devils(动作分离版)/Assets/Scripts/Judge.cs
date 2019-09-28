using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judge : MonoBehaviour{
    public int judgment(int flag,int[] srcnum,int[] dstnum,int[] boatnum) {

		if (srcnum[0] + srcnum[1] == -1)
			return 2;

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

		return 1;
	}
}
