using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class CarAgent : Agent
{
    // Start is called before the first frame update
    Rigidbody rBody;
    Quaternion Q;
    Vector3 V;
    public float distanceToTarget;
    public float oldDistanceToTarget;
    public Vector3 directionToTarget;
    public Transform ground;
    public int rewardMine;

    public GameObject[] SpawnPoints;

    public GameObject[] casters;
    public float[] barDistance;
    RaycastHit hit;
    public bool[] TargetBool;
    public float[] TargetDis;
    public Vector3 dir;



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
        //reward = 0;

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
        //AddVectorObs(this.transform.position);
        AddVectorObs(distanceToTarget);
        AddVectorObs(directionToTarget.x);
        AddVectorObs(directionToTarget.z);

        //AddVectorObs(oldDistanceToTarget);

        // Agent velocity
        AddVectorObs(velocityMagnitude);
        //AddVectorObs(rBody.velocity.z);
        //CheckTargetForward();
        //AddVectorObs(TargetBool[2]);
        for (int i = 0; i < 6; i++)
        {
            CheckTargetAll(i);
            CheckTarget(i);
            AddVectorObs(barDistance[i]);
            AddVectorObs(TargetDis[i]);

        }
        
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
                TargetDis[i] = -1;
            }

        }
        else
        {
            TargetDis[i] = -1;
        }
    }

    private void CheckTarget(int i)
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
                barDistance[i] = -1;
            }

        }
        else
        {
            barDistance[i] = -1;
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
            if(vectorAction[0] == 1)
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
            SetReward(0.02f);
            reward += 2;
        }
        else if (m_verticalInput == 1)
        {
            SetReward(0.01f);
            reward += 1;
        }
        else if (m_verticalInput == -1)
        {
            SetReward(-0.05f);
            reward -= 5;
        }
        else if ((m_verticalInput == 0 && m_horizontalInput == 1) || (m_verticalInput == 0 && m_horizontalInput == -1))
        {
            SetReward(-1f);
            reward -= 1;
        }
        */

        // m_verticalInput = vectorAction[1];

        //   }

        //rBody.AddForce(controlSignal * speed);

        // Rewards
        distanceToTarget = Vector3.Distance(this.transform.position,
                                                  Target.position);

        distanceToTarget = Mathf.Round(distanceToTarget * 100f) / 100f;

        directionToTarget = (Target.transform.position - this.transform.position).normalized;
        // Reached target

        if (distanceToTarget < 0.3f)
        {
            AddReward(5f);
            rewardMine += 500;
            Done();
        }
        
        
        if (counter > 10)
        {
            
            if (distanceToTarget < oldDistanceToTarget-0.01)
            {
                AddReward(0.05f);
                rewardMine += 5;
                // Done();
            }
            else
            {
                AddReward(-0.06f);
                rewardMine -= 6;
                //Done();
            }

            
            if (velocityMagnitude > 1)
            {
                AddReward(0.2f);
                rewardMine += 1;
            }
            
            else
            {
                AddReward(-0.01f);
                rewardMine -= 1;
            }
         
            if (TargetDis[2] != -1)
                {
                    AddReward(0.03f);
                rewardMine += 3;
                }
                
                else if (TargetDis[0] != -1 || TargetDis[1] != -1)
                {
                    AddReward(0.01f);
                rewardMine += 1;
                }
                else if (TargetDis[4] != -1 || TargetDis[5] != -1)
                {
                    AddReward(0.02f);
                    rewardMine += 2;
                }
                else if (TargetDis[3] != -1)
                    {
                        AddReward(-0.05f);
                    rewardMine -= 5;
                    }
                
                /*
            for(int i = 0; i < 4; i++)
            {
                if (castershit[i] == false)
                {
                    SetReward(-0.1f);
                    reward -= 10;
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
            AddReward(-0.5f);
            rewardMine -= 50;
            Done();
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


    public int counter = 0;
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
