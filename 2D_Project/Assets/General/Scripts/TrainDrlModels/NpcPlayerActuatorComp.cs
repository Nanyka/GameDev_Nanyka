using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Actuators;
using UnityEngine;

namespace TheAiAlchemist
{
    public class NpcPlayerActuatorComp : ActuatorComponent
    {
        [SerializeField] private NpcPlayer controller;
        private ActionSpec _actionSpec = ActionSpec.MakeDiscrete(9);
        
        public override IActuator[] CreateActuators()
        {
            return new IActuator[] { new NpcPlayerActuator(controller) };
        }
    
        public override ActionSpec ActionSpec
        {
            get { return _actionSpec; }
        }
    }
    
    public class NpcPlayerActuator : IActuator
    {
        private NpcPlayer _controller;
        ActionSpec _actionSpec;
    
        public NpcPlayerActuator(NpcPlayer controller)
        {
            _controller = controller;
            _actionSpec = ActionSpec.MakeDiscrete(9);
        }
    
        public void OnActionReceived(ActionBuffers actionBuffers)
        {
            // Debug.Log($"Receive an action");
            _controller.TakeAction(actionBuffers.DiscreteActions[0]);
        }
        
        public void Heuristic(in ActionBuffers actionBuffersOut)
        {
            var discreteActionOut = actionBuffersOut.DiscreteActions;
            discreteActionOut[0] = _controller.currentAction;
        }
    
        public ActionSpec ActionSpec {
            get { return _actionSpec; }
        }
        public string Name
        {
            get { return "NpcPlayer"; }
        }
        
        public void WriteDiscreteActionMask(IDiscreteActionMask actionMask) { }
        public void ResetData() { }
    
    }
}