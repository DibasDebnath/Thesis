﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class CarCarriculumAgent : Agent
{
    // Start is called before the first frame update
    Rigidbody rBody;
    Quaternion Q;
    Vector3 V;

    [Header("Roads")]
    public GameObject[] Roads;
    public int RoadSelected;

    [Header("Car Movement")]
    public float distanceToTarget;
    public float oldDistanceToTarget;
    public float oldDistanceToTargetTwo;
    public float vecDot;

    Vector3 PosOfCar;
    Vector3 oldPosOfCar;
    Vector3 movement;
    Vector3 fwd;

    public Vector3 directionToTarget;
    public Transform ground;
    [Header("Car Reward")]
    public float rewardMine;
    public float oldRewardMine;
    public int targetReach;

    [Header("Car Sensors")]
    public GameObject[] SpawnPoints;
    public GameObject[] casters;
    public float[] barDistance;
    RaycastHit hit;
    public bool[] TargetBool;
    public float[] TargetDis;
    public Vector3 dir;
    Vector2 directionVec = new Vector2(0, 0);

    public int collideNo;
    public float velocityMagnitude;
    //public float velocityZ;
    private void FixedUpdate()
    {
        velocityMagnitude = Mathf.Round(rBody.velocity.sqrMagnitude * 100f) / 100f;
        //velocityZ = Mathf.Abs(rBody.velocity.z);
        Steer();
        Accelerate();
        UpdateWheelPoses();
    }

    public void Start()
    {
        Roads[RoadSelected].SetActive(true);
        //RoadSelected = 0;
        rBody = GetComponent<Rigidbody>();
        Q = this.transform.rotation;
        V = this.transform.position;
    }
    public Transform Target;

    public override void AgentReset()
    {
        //if (this.transform.position.y < -1)
        //{
        // If the Agent fell, zero its momentum
        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;
        this.transform.position = V;// new Vector3(0, 0.5f, 0);
        this.transform.rotation = Q;
        rewardMine = 0;
        //reward = 0;
        for (int i = 0; i < 5; i++)
        {
            Roads[i].SetActive(false);
        }
        Roads[RoadSelected].SetActive(true);
        //}
        //ground.transform.position.x+
        /*
        Target.position = new Vector3(
            ground.transform.position.x + Random.Range(-1.1f, 1.1f), 
            ground.transform.position.y + 0.1f, 
            ground.transform.position.z + Random.Range(-1.1f, 1.1f));
            */
        //Target.position = SpawnPoints[Random.Range(0,8)].transform.position;

    }


    public override void CollectObservations()
    {
        // Target and Agent positions
        //AddVectorObs(Target.position);
        AddVectorObs(Target.position.x);
        AddVectorObs(Target.position.z);
        //AddVectorObs(this.transform.position);
        AddVectorObs(this.transform.position.x);
        AddVectorObs(this.transform.position.z);

        AddVectorObs(distanceToTarget);
        // directionVec.Set(directionToTarget.x, directionToTarget.z);// = new Vector2();
        //AddVectorObs(directionVec);
        //AddVectorObs(directionToTarget.x);
        //AddVectorObs(directionToTarget.z);

        //AddVectorObs(oldDistanceToTarget);

        // Agent velocity
        //AddVectorObs(velocityMagnitude);
        AddVectorObs(vecDot);

        //AddVectorObs(rBody.velocity.z);
        //CheckTargetForward();
        //AddVectorObs(TargetBool[2]);

        for (int i = 0; i < 6; i++)
        {
            CheckTargetAll(i);
            CheckBar(i);
            AddVectorObs(barDistance[i]);
            //AddVectorObs(TargetDis[i]);

        }
        AddVectorObs(TargetDis[2]);
    }

    private void CheckTargetForward()
    {
        dir = casters[2].transform.forward;
        if (Physics.Raycast(casters[2].transform.position, dir, out hit, 10))
        {

            if (hit.transform.gameObject.name == "Target")
            {
                TargetBool[2] = true;
            }
            else
            {
                TargetBool[2] = false;
            }
        }
        else
        {
            TargetBool[2] = false;
        }
    }
    private void CheckTargetAll(int i)
    {
        switch (i)
        {
            case 0:
                dir = -casters[i].transform.right;
                break;
            case 1:
                dir = casters[i].transform.right;
                break;
            case 2:
                dir = casters[i].transform.forward;
                break;
            case 3:
                dir = -casters[i].transform.forward;
                break;
            case 4:
                dir = -casters[i].transform.right;
                break;
            case 5:
                dir = casters[i].transform.right;
                break;
            default:
                dir = -casters[i].transform.forward;
                break;
        }
        if (Physics.Raycast(casters[i].transform.position, dir, out hit, 10))
        {
            if (hit.transform.gameObject.name == "Target")
            {
                TargetDis[i] = hit.distance;
            }
            else
            {
                TargetDis[i] = 100;
            }

        }
        else
        {
            TargetDis[i] = 100;
        }
    }

    private void CheckBar(int i)
    {
        switch (i)
        {
            case 0:
                dir = -casters[i].transform.right;
                break;
            case 1:
                dir = casters[i].transform.right;
                break;
            case 2:
                dir = casters[i].transform.forward;
                break;
            case 3:
                dir = -casters[i].transform.forward;
                break;
            case 4:
                dir = -casters[i].transform.right;
                break;
            case 5:
                dir = casters[i].transform.right;
                break;
            default:
                dir = -casters[i].transform.forward;
                break;
        }
        if (Physics.Raycast(casters[i].transform.position, dir, out hit, 10))
        {
            if (hit.transform.gameObject.tag == "Bar")
            {
                barDistance[i] = hit.distance;
            }
            else
            {
                barDistance[i] = 100;
            }

        }
        else
        {
            barDistance[i] = 100;
        }
    }







    public override void AgentAction(float[] vectorAction, string textAction)
    {
        // Actions, size = 2
        //Vector3 controlSignal = Vector3.zero;
        /*
        if(vectorAction[0] == 1)
        {
           // m_horizontalInput = vectorAction[0];
            m_verticalInput = 1;
            m_horizontalInput = 0;
        }
        else if(vectorAction[0] == 2)
        {
            //m_horizontalInput = vectorAction[0];
            m_verticalInput = -1;
            m_horizontalInput = 0;
        }
        else if (vectorAction[0] == 3)
        {
            m_horizontalInput = -1;
            m_verticalInput = 1;
        }
        else if (vectorAction[0] == 4)
        {
            m_horizontalInput = -1;
            m_verticalInput = -1;
        }
        else if (vectorAction[0] == 5)
        {
            m_horizontalInput = 1;
            m_verticalInput = 1;
        }
        else if (vectorAction[0] == 6)
        {
            m_horizontalInput = 1;
            m_verticalInput = -1;
        }
        else
        {
            m_horizontalInput = 0;
            m_verticalInput = 0;
        }
        */

        //   if (brain.brainParameters.vectorActionSpaceType == SpaceType.discrete)
        //   {
        if (vectorAction[0] == 1)
            m_horizontalInput = 1;
        else if (vectorAction[0] == 2)
            m_horizontalInput = -1;
        else
            m_horizontalInput = 0;

        if (vectorAction[1] == 1)
            m_verticalInput = 1;
        else if (vectorAction[1] == 2)
            m_verticalInput = -1;
        else
            m_verticalInput = 0;


        /*
        if((m_verticalInput == 1 && m_horizontalInput == 1) || (m_verticalInput == 1 && m_horizontalInput == -1))
        {
            AddReward(0.01f);
            rewardMine += 0.01f;
        }
        else if (m_verticalInput == 1)
        {
            AddReward(0.005f);
            rewardMine += 0.005f;
        }
        
        else if (m_verticalInput == -1)
        {
            AddReward(-0.05f);
            rewardMine -= 5;
        }
        
        if ((m_verticalInput == 0 && m_horizontalInput == 1) || (m_verticalInput == 0 && m_horizontalInput == -1))
        {
            AddReward(-1f);
            rewardMine -= 100;
        }
        */

        // m_verticalInput = vectorAction[1];

        //   }

        //rBody.AddForce(controlSignal * speed);

        // Rewards
        distanceToTarget = Vector3.Distance(this.transform.position,
                                                  Target.position);

        distanceToTarget = Mathf.Round(distanceToTarget * 1000f) / 1000f;

        directionToTarget = (Target.transform.position - this.transform.position).normalized;
        // Reached target

        if (counterTwo > 100)
        {

            if (distanceToTarget < oldDistanceToTargetTwo - 0.02f)
            {
                //  AddReward(0.03f);
                //  rewardMine += 0.03f;
                // Done();
            }
            else
            {
                AddReward(-0.8f);
                rewardMine -= 0.8f;
                //Done();
            }
            oldDistanceToTargetTwo = distanceToTarget;
            counterTwo = 0;
        }
        else
        {
            counterTwo++;
        }






        if (counter > 10)
        {
            /*
            if (distanceToTarget < oldDistanceToTarget-0.02f)
            {
                AddReward(0.03f);
                rewardMine += 0.03f;
                // Done();
            }
            else
            {
                AddReward(-0.03f);
                rewardMine -= 0.03f;
                //Done();
            }
            */
            PosOfCar = this.transform.position;
            movement = PosOfCar - oldPosOfCar;
            fwd = this.transform.forward;
            vecDot = Vector3.Dot(fwd, movement);
            if (vecDot > 0.15)
            {
                AddReward(0.06f);
                rewardMine += 0.06f;
                // Done();
            }
            else
            {
                AddReward(-0.08f);
                rewardMine -= 0.08f;
                //Done();
            }
            oldPosOfCar = PosOfCar;

            /*
            if (velocityMagnitude > 1f)
            {
                AddReward(0.02f);
                rewardMine += 0.02f;
            }
            
            else
            {
                AddReward(-0.01f);
                rewardMine -= 0.01f;
            }
            */
            if (TargetDis[2] != 100)
            {
                AddReward(0.1f);
                rewardMine += 0.1f;
            }
            /*  
              else if (TargetDis[0] != 100 || TargetDis[1] != 100)
              {
                  AddReward(0.01f);
                  rewardMine += 1;
              }
              else if (TargetDis[4] != 100 || TargetDis[5] != 100)
              {
                  AddReward(0.05f);
                  rewardMine += 5;
              }
              else if (TargetDis[3] != 100)
                  {
                //      AddReward(-0.05f);
               //   rewardMine -= 5;
                  }
              */
            /*
        for(int i = 0; i < 6; i++)
        {
            if (barDistance[i] > 0.2f)
            {
                AddReward(0.01f);
                rewardMine += 1;
            }
            else
            {
                AddReward(-0.01f);
                rewardMine -= 1;
            }
        }
            */

            oldDistanceToTarget = distanceToTarget;

            counter = 0;
        }
        else
        {
            counter++;
        }

        if (rewardMine < -3)
        {
            Done();
        }



        // Fell off platform
        /*
        if (this.transform.position.y < ground.transform.position.y - 1)
        {
            AddReward(-5f);
            rewardMine -= 100;
            Done();
        }
        */


        //m_horizontalInput = 0;
        //m_verticalInput = 0;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Bar")
        {
            AddReward(-1f);
            rewardMine -= 1f;
            //    if (collideNo > 4)
            //     {
            oldRewardMine = rewardMine;

            //          collideNo = 0;
            // Done();    // Change it for Apply Scene
            //     }
            //     else
            //      {
            //         collideNo += 1;
            //        AgentReset();
            //   }


            //Done();
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "Bar")
        {
            AddReward(-0.008f);
            rewardMine -= 0.008f;
            //    if (collideNo > 4)
            //     {
            // oldRewardMine = rewardMine;

            //          collideNo = 0;
            // Done();    // Change it for Apply Scene
            //     }
            //     else
            //      {
            //         collideNo += 1;
            //        AgentReset();
            //   }


            //Done();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Target")
        {
            AddReward(5f);
            rewardMine += 5f;
            //   if(targetReach > 1)
            //    {
            oldRewardMine = rewardMine;

            if(oldRewardMine > 8)
            {
                RoadSelected++;
                if (RoadSelected > 4)
                {
                    RoadSelected = 4;
                }
            }
            
            Done();
            //    }
            //    else
            //    {

            targetReach += 1;
            //       AgentReset();
            //  }
        }


    }














    public void GetInput()
    {
        m_horizontalInput = Input.GetAxis("Horizontal");
        m_verticalInput = Input.GetAxis("Vertical");
    }

    private void Steer()
    {
        m_steeringAngle = maxSteerAngle * m_horizontalInput;
        frontDriverW.steerAngle = m_steeringAngle;
        frontPassengerW.steerAngle = m_steeringAngle;

        //frontDriverW.motorTorque = m_horizontalInput * motorForce;
        //frontPassengerW.motorTorque = m_horizontalInput * motorForce;
    }

    private void Accelerate()
    {
        frontDriverW.motorTorque = m_verticalInput * motorForce;
        frontPassengerW.motorTorque = m_verticalInput * motorForce;
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(frontDriverW, frontDriverT);
        UpdateWheelPose(frontPassengerW, frontPassengerT);
        UpdateWheelPose(rearDriverW, rearDriverT);
        UpdateWheelPose(rearPassengerW, rearPassengerT);
    }

    private void UpdateWheelPose(WheelCollider _collider, Transform _transform)
    {
        Vector3 _pos = _transform.position;
        Quaternion _quat = _transform.rotation;

        _collider.GetWorldPose(out _pos, out _quat);

        _transform.position = _pos;
        _transform.rotation = _quat;
    }

    [Header("Car Inputs")]
    public int counter = 0;
    public int counterTwo = 0;
    public float m_horizontalInput;
    public float m_verticalInput;
    private float m_steeringAngle;

    public WheelCollider frontDriverW, frontPassengerW;
    public WheelCollider rearDriverW, rearPassengerW;
    public Transform frontDriverT, frontPassengerT;
    public Transform rearDriverT, rearPassengerT;
    public float maxSteerAngle = 30;
    public float motorForce = 50;
}
