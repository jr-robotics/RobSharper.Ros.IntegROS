using System;
using System.Threading;
using FluentAssertions;
using RosComponentTesting;
using Xunit;

namespace RosComponentTestingTests
{
    public class ExceptionDispatcherTests
    {
        [Fact]
        public void Dispatching_exception_caches_exception()
        {
            var expectedException = new Exception();

            var target = new ExceptionDispatcher(new CancellationTokenSource());
            target.Dispatch(expectedException);

            target.HasException.Should().BeTrue();
            target.Exception.Should().Be(expectedException);
        }
        
        [Fact]
        public void Dispatching_exception_requests_cancellation()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            
            var target = new ExceptionDispatcher(cancellationTokenSource);
            target.Dispatch(new Exception());

            cancellationTokenSource.IsCancellationRequested.Should().BeTrue();
        }
        
        [Fact]
        public void Dispatching_no_exception_does_not_request_a_canellation()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            
            var target = new ExceptionDispatcher(cancellationTokenSource);

            cancellationTokenSource.IsCancellationRequested.Should().BeFalse();
            target.HasException.Should().BeFalse();
            target.Exception.Should().BeNull();
        }
    }
}