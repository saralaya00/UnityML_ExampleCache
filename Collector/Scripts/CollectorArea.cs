using System.Collections;
using System.Collections.Generic;
using MLAgents;
using UnityEngine;

namespace CollectorML
{
    public class CollectorArea : Area
    { 
        public GameObject goodCoin;
        public GameObject badCoin;

        public int goodCoinCount;
        public int badCoinCount;

        public bool respawn;
        public float range;
        
        public override void ResetArea()
        {
            
        }

        public void ResetCollecterArea(GameObject[] agents)
        {
            foreach (GameObject agent in agents)
            {
                if (agent.transform.parent == gameObject.transform)
                {
                    agent.transform.position = new Vector3(Random.Range(-range, range), 2f,
                                                   Random.Range(-range, range))
                                               + transform.position;
                    //agent.transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));
                }
            }
            
            InstantiateCoins(goodCoinCount, goodCoin);
            InstantiateCoins(badCoinCount, badCoin);
        }

        private void InstantiateCoins(int count, GameObject obj)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject go = Instantiate(obj,
                    new Vector3(Random.Range(-range, range), 1f, Random.Range(-range, range)) + transform.position, 
                    Quaternion.identity);

                go.GetComponent<CoinLogic>().respawn = respawn;
                go.GetComponent<CoinLogic>().myArea = this;
            }
        }
    }
}