using UnityEngine;
using System.Collections;
using System;

public class TestBehaviourScript : MonoBehaviour {

    WorldCondition worldCond = new WorldCondition();
    ConditionTable condTable = new ConditionTable();
    
	// Use this for initialization
	void Start () {
        System.Random rand = new System.Random(0);
        worldCond.generate(rand);
        condTable.generate( rand , worldCond , 5 , 3 );

        for (int i = 0; i < condTable.numSelection; ++i)
        {
            Condition cond = condTable.getCondition(i);
            for (int j = 0; j < Condition.TotalExprNum; ++j)
            {
                String str = cond.getContent(j);
                Debug.Log(str);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
