using System;

namespace RosComponentTesting.RosNet
{
    public static class TestBuilderRosExtensions
    {
        public static TestBuilder UseRosDotNet(this TestBuilder builder,
            string rosMasterUri = null, string nodeName = null)
        {
            builder.TestExecutorFactory = new RosTestExecutorFactory(rosMasterUri, nodeName);

            return builder;
        }
    }
}