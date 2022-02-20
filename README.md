# IntegROS.NET

Verify your expectations on [ROS](https://ros.org) applications in a more natural way.

```c#
[ExpectThat]
public void My_Assumptions_are_met()
{
    Scenario
        .Messages
        .InTypic("/*")
        .Should()
        .MeetMyExpectation();
}
```

# Usage

Have a look into the [docs](docs/index.md).

# License

IntegROS.NET is licensed under [BSD 3-Clause](LICENSE)