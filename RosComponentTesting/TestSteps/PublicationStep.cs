using System;
using Microsoft.Extensions.DependencyInjection;

namespace RosComponentTesting.TestSteps
{
    public class PublicationStep : ITestStep, ITestStepExecutor
    {
        public IPublication Publication { get; }

        public PublicationStep(IPublication publication)
        {
            if (publication == null) throw new ArgumentNullException(nameof(publication));
            
            Publication = publication;
        }

        public void Execute(IServiceProvider serviceProvider)
        {
            serviceProvider
                .GetRequiredService<IRosPublisherResolver>()
                .GetPublisherFor(Publication.Topic)
                .Publish(Publication.Message);
        }

        public void Cancel()
        {
            
        }
    }
}