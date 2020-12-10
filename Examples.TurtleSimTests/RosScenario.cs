using System;
using System.Collections.Generic;
using IntegROS;

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
        private RosScenarioConfiguration _config;

        public RosScenarioState State { get; private set; } = RosScenarioState.Pending;

        private RosScenarioConfiguration Configuration
        {
            get
            {
                if (_config == null)
                {
                    lock (_lock)
                    {
                        if (_config == null)
                        {
                            _config = RosScenarioConfigurationFactory.Instance.Create();
                        }
                    }
                }

                return _config;
            }
        }
        
        protected IRosApplication RosApp => Configuration.RosApp;

        protected IRosMessageRecorder MessageRecorder => Configuration.MessageRecorder;

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

    public class RosScenarioConfigurationFactory
    {
        public static RosScenarioConfigurationFactory Instance
        {
            get;
        } = new RosScenarioConfigurationFactory();
        
        public RosScenarioConfiguration Create()
        {
            return new RosScenarioConfiguration(null, null);
        }
    }

    public sealed class RosScenarioConfiguration
    {
        public RosScenarioConfiguration(IRosApplication rosApp, IRosMessageRecorder messageRecorder)
        {
            RosApp = rosApp ?? throw new ArgumentNullException(nameof(rosApp));
            MessageRecorder = messageRecorder ?? throw new ArgumentNullException(nameof(messageRecorder));
        }

        public IRosApplication RosApp { get; }
        public IRosMessageRecorder MessageRecorder { get; }
    }
}