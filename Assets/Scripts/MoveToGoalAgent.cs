using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class MoveToGoalAgent : Agent
{   
    [SerializeField] private Transform targetTransform;

    public override void OnEpisodeBegin(){
        transform.position = Vector3.zero;
    }

    public override void CollectObservations(VectorSensor sensor){
        sensor.AddObservation(transform.position); //passing 3 (float) values
        sensor.AddObservation(targetTransform.position); //passing 3 (float) values
    }

    public override void OnActionReceived(ActionBuffers actions){
        Debug.Log(actions.ContinuousActions[0]);
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        float moveSpeed = 1f;
        transform.position += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut){
        ActionSegment<float> continuousActions =  actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");  
    }

    private void OnTriggerEnter(Collider other){
        if(other.TryGetComponent<Goal>(out Goal goal)){
            SetReward(+1f);
            EndEpisode();
        }
        if(other.TryGetComponent<Wall>(out Wall wall)){
            SetReward(-1f);
            EndEpisode();
        }
    }
}
