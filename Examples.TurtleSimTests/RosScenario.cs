using System;
using System.Collections.Generic;

namespace Examples.TurtleSimTests
{
    public enum RosScenarioState
    {
        Pending = 0,
        Setup,
        Exercise,
        TearDown,
        Completed,
        Failed
    }
    
    public abstract class RosScenario
    {
        private readonly object _lock = new object();
        private bool _executed = false;
        
        public RosScenarioState State { get; private set; } = RosScenarioState.Pending;
        
        protected IRosApplication RosApp { get; }
        protected IRosMessageRecorder MessageRecorder { get; }

        public IEnumerable<RecordedMessage> Messages
        {
            get
            {
                Execute();
                return MessageRecorder.Messages;
            }
        }

        protected virtual void Setup()
        {
        }

        protected abstract void Exercise();

        protected virtual void Teardown()
        { 
        }

        protected void Wait(TimeSpan fromSeconds)
        {
            // TODO
        }

        protected void AddCheckpoint(string id)
        {
            // TODO: Publish checkpoint
        }

        public void Execute()
        {
            if (_executed)
                return;

            lock (_lock)
            {
                if (_executed)
                    return;

                try
                {
                    State = RosScenarioState.Setup;
                    SetupInternal();
                    MessageRecorder.Start();

                    State = RosScenarioState.Exercise;
                    Exercise();

                    State = RosScenarioState.TearDown;
                    MessageRecorder.Stop();
                    Teardown();

                    State = RosScenarioState.Completed;
                }
                catch
                {
                    State = RosScenarioState.Failed;
                    throw;
                }
                finally
                {
                    _executed = true;
                }
            }
        }

        private void SetupInternal()
        {
            // TODO: advertise scenario topics
            //     /integros/checkpoint
            
            Setup();
        }
    }
}