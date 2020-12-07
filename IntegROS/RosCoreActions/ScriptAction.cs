using System;
using System.IO;

namespace IntegROS.RosCoreActions
{
    public class ScriptAction : IRosCoreConnectionAction
    {
        private readonly string _scriptPath;

        public ScriptAction(string scriptPath)
        {
            _scriptPath = scriptPath ?? throw new ArgumentNullException(nameof(scriptPath));
        }

        public void Execute(RosConfiguration rosConfiguration)
        {
            throw new NotImplementedException();
        }
        
        public static IRosCoreConnectionAction Create(string scriptPath)
        {
            if (!File.Exists(scriptPath))
                throw new ArgumentException($"Script file '{scriptPath}' does not exist.", nameof(scriptPath));

            return new ScriptAction(scriptPath);
        }
    }
}