using System.Collections;
using System.Collections.Generic;
using MLAgents;
using UnityEngine;
using UnityEngine.Networking;

namespace CollectorML
{
    public class CollectorAgent : Agent
    {
        public GameObject collecterAcademyGO;
        public GameObject area;
        public GameObject myLaser;
        private Rigidbody rb;
        
        private CollectorAcademy collectorAcademy;
        private CollectorArea myArea;
        private RayPerception rayPer;
        
        public bool useVectorObs;
        public bool contribute;
        
        private bool frozen;
        private bool poisoned;
        private bool satiated;
        private bool shoot;

        private float frozenTime;
        private float effectTime;

        private int coins;

        private float turnSpeed = 300;
        private float moveSpeed = 5;
        
        [Space]
        public Material normalMaterial;
        public Material badMaterial;
        public Material goodMaterial;
        public Material frozenMaterial;
        
        private Renderer renderer;
        
        //Agent Stats
        static readonly float rayDistance = 50f;
        static readonly float[] rayAngles = { 20f, 90f, 160f, 45f, 135f, 70f, 110f };
        static readonly string[] detectableObjects = { "good-coin", "agent", "wall", "bad-coin", "frozenAgent" };

        public override void InitializeAgent()
        {
            base.InitializeAgent();
            rb = GetComponent<Rigidbody>();
            Monitor.verticalOffset = 1f;
            myArea = area.GetComponent<CollectorArea>();
            rayPer = GetComponent<RayPerception>();
            collectorAcademy = collecterAcademyGO.GetComponent<CollectorAcademy>();

            renderer = gameObject.GetComponent<Renderer>() ;
        }

        public override void CollectObservations()
        {
            if (useVectorObs)
            {
//                float rayDistance = 50f;
//                float[] rayAngles = { 20f, 90f, 160f, 45f, 135f, 70f, 110f };
//                string[] detectableObjects = { "good-coin", "agent", "wall", "bad-coin", "frozenAgent" };

                AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));

                Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
                AddVectorObs(localVelocity.x);
                AddVectorObs(localVelocity.z);

                AddVectorObs(System.Convert.ToInt32(frozen));
                AddVectorObs(System.Convert.ToInt32(shoot));
            }
        }

        public override void AgentAction(float[] vectorAction, string textAction)
        {
            MoveAgent(vectorAction);
        }

        public override void AgentReset()
        {
            UnFreeze();
            UnPoison();
            UnSatiate();

            shoot = false;
            rb.velocity = Vector3.zero;
            coins = 0;

            myLaser.transform.localScale = new Vector3(0f,0f,0f);
            transform.position = new Vector3(Random.Range(-myArea.range, myArea.range),
                2f, Random.Range(-myArea.range, myArea.range)) + area.transform.position;
            transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));
        }

        public void MoveAgent(float[] actions)
        {
            shoot = false;

            if (Time.time > frozenTime + 4f && frozen)
            {
                UnFreeze();
            }

            if (Time.time > effectTime + 0.5f)
            {
                if (poisoned) UnPoison();
                if (satiated) UnSatiate();
            }

            Vector3 dirToGo = Vector3.zero;
            Vector3 rotateDir = Vector3.zero;

            if (!frozen)
            {
                bool shootCommand = false;
                if (brain.brainParameters.vectorActionSpaceType == SpaceType.continuous)
                {
                    dirToGo = transform.forward * Mathf.Clamp(actions[0], -1f, 1f);
                    rotateDir = transform.up * Mathf.Clamp(actions[1], -1f, 1f);
                    shootCommand = Mathf.Clamp(actions[2], -1f, 1f) > 0.5f;
                }

                if (shootCommand)
                {
                    shoot = true;
                    dirToGo *= 0.5f;
                    rb.velocity *= 0.75f;
                }

                rb.AddForce(dirToGo * moveSpeed, ForceMode.VelocityChange);
                transform.Rotate(rotateDir, Time.fixedDeltaTime * turnSpeed);
            
            }

                
            if (rb.velocity.sqrMagnitude > 25f)
            {
                rb.velocity *= 0.95f;
            }

            if (shoot)
            {
                myLaser.transform.localScale = new Vector3(1f,1f,1f);
                Vector3 position = transform.TransformDirection(RayPerception.PolarToCartesian(25f, 90f));

                Debug.DrawRay(transform.position, position, Color.red, 0f, true);
                RaycastHit hit;

                if (Physics.SphereCast(transform.position, 2f, position, out hit, 25f))
                {
                    if (hit.collider.gameObject.CompareTag("agent"))
                    {
                        hit.collider.gameObject.GetComponent<CollectorAgent>().Freeze();
                    }
                }    
            }

            else
            {
                myLaser.transform.localScale = new Vector3(0f, 0f, 0f);
            }
        }


        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("good-coin"))
            {
                Satiate();
                other.gameObject.GetComponent<CoinLogic>().OnCollected();
                AddReward(1f);
                coins += 1;

                if (contribute)
                    collectorAcademy.totalScore += 1;
            }

            if (other.gameObject.CompareTag("bad-coin"))
            {
                Poison();
                other.gameObject.GetComponent<CoinLogic>().OnCollected();
                AddReward(-1f);

                if (contribute)
                    collectorAcademy.totalScore -= 1; 
            }

            rb.velocity = new Vector3(0,0,0);
        }


        void Freeze()
        {
            gameObject.tag = "frozenAgent";
            frozen = true;
            frozenTime = Time.time;
            renderer.material = frozenMaterial;
        }

        void UnFreeze()
        {
            gameObject.tag = "agent";
            rb.velocity = new Vector3(0,0,0);
            frozen = false;
            renderer.material = normalMaterial;
        }

        
        void Poison()
        {
            poisoned = true;
            effectTime = Time.time;
            renderer.material = badMaterial;
        }

        void UnPoison()
        {
            poisoned = false;
            renderer.material = normalMaterial;
        }

        
        void Satiate()
        {
            satiated = true;
            effectTime = Time.time;
            renderer.material = goodMaterial;
        }

        void UnSatiate()
        {
            satiated = false;
            renderer.material = normalMaterial;
        }
    }
}
