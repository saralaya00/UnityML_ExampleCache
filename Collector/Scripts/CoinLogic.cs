using System.Collections;
using System.Collections.Generic;
using CollectorML;
using UnityEngine;

namespace CollectorML
{
    public class CoinLogic : MonoBehaviour
    {

        public bool respawn;
        [HideInInspector]
        public CollectorArea myArea;

        public void OnCollected()
        {
            if (respawn)
            {
                transform.position = new Vector3(Random.Range(-myArea.range, myArea.range), transform.position.y, Random.Range(-myArea.range, myArea.range));       
            }

            else
            {
                Destroy(gameObject);
            }
        }
    }
}
