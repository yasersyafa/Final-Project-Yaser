using System.Collections.Generic;
using UnityEngine;

namespace Helpers.BehaviorTree
{
    public class Sequence : Node
    {
        private List<Node> nodes = new();

        public Sequence(List<Node> nodes) { this.nodes = nodes; }

        public override NodeState Evaluate()
        {
            bool anyChildRunning = false;

            foreach (var node in nodes)
            {
                switch (node.Evaluate())
                {
                    case NodeState.Failure:
                        state = NodeState.Failure;
                        return state;
                    case NodeState.Success:
                        continue;
                    case NodeState.Running:
                        anyChildRunning = true;
                        continue;
                }
            }
            state = anyChildRunning ? NodeState.Running : NodeState.Success;
            return state;
        }
    }
    
}
