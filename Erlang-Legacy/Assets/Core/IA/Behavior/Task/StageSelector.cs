using System.Collections.Generic;
using System.Linq;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Core.AI
{
    public class StageSelector : Composite
    {
        public SharedInt CurrentStage;
        public List<string> IncludedTasksPerStage;

        // A list of indexes of every child task. This list is used by the Fischer-Yates shuffle algorithm.
        private List<int> childIndexList = new List<int>();
        // The random child index execution order.
        private Stack<int> childrenExecutionOrder = new Stack<int>();
        // The task status of the last child ran.
        private TaskStatus executionStatus = TaskStatus.Inactive;

        public override void OnStart()
        {
            // Select considered child indicies based on the current stage
            childIndexList.Clear();
            childIndexList = IncludedTasksPerStage[Clamp(CurrentStage.Value)].Split(',').Select(int.Parse).ToList();

            // Randomize the indecies
            ShuffleChilden();
        }

        private int Clamp(int currentStage)
        {
            return Mathf.Clamp(currentStage, 0, IncludedTasksPerStage.Count - 1);
        }

        public override int CurrentChildIndex()
        {
            // Peek will return the index at the top of the stack.
            return childrenExecutionOrder.Peek();
        }

        public override bool CanExecute()
        {
            // Continue exectuion if no task has return success and indexes still exist on the stack.
            return childrenExecutionOrder.Count > 0 && executionStatus != TaskStatus.Success;
        }

        public override void OnChildExecuted(TaskStatus childStatus)
        {
            // Pop the top index from the stack and set the execution status.
            if (childrenExecutionOrder.Count > 0)
            {
                childrenExecutionOrder.Pop();
            }
            executionStatus = childStatus;
        }

        public override void OnConditionalAbort(int childIndex)
        {
            // Start from the beginning on an abort
            childrenExecutionOrder.Clear();
            executionStatus = TaskStatus.Inactive;
            ShuffleChilden();
        }

        public override void OnEnd()
        {
            // All of the children have run. Reset the variables back to their starting values.
            executionStatus = TaskStatus.Inactive;
            childrenExecutionOrder.Clear();
        }

        public override void OnReset()
        {
            // Reset the public properties back to their original values

        }

        private void ShuffleChilden()
        {
            // Use Fischer-Yates shuffle to randomize the child index order.
            for (int i = childIndexList.Count; i > 0; --i)
            {
                int j = Random.Range(0, i);
                int index = childIndexList[j];
                childrenExecutionOrder.Push(index);
                childIndexList[j] = childIndexList[i - 1];
                childIndexList[i - 1] = index;
            }
        }
    }
}