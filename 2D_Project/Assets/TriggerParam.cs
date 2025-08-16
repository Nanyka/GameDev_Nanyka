using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class TriggerParam : StateMachineBehaviour
{
    [SerializeField] private AnimTriggerUnit[] animTriggerUnits;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // if (Random.Range(0,10) <= triggerRatio)
        //     animator.SetTrigger(parameterName);
        var randomValue = Random.Range(0, 100f);
        var currentValue = 0f;
        foreach (var animTriggerUnit in animTriggerUnits)
        {
            currentValue += animTriggerUnit.triggerRatio;
            // Debug.Log($"Random: {randomValue}, Current: {currentValue},");
            if (randomValue < currentValue)
            {
                animator.SetTrigger(animTriggerUnit.parameterName);
                break;
            }
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    // override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    // {
    //     Debug.Log("OnStateExit");
    //     animator.SetTrigger("Roar");
    // }
    
    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}

[Serializable]
public class AnimTriggerUnit
{
    public string parameterName;
    public float triggerRatio;
}
