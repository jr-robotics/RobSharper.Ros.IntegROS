# Verify your expectations

Once you have [filtered the ROS message stream](filter.md) you state assertions to verify your expectations.

Here is an example.
We expect, that during a scenario no log message with log level fatal should have been published: 
````c#
[RosbagScenario(BagFiles.PathToROSBAG))]
public class MyTestClass : ForScenario
{
    [ExpectThat]
    public void No_fatal_log_messages_should_occur()
    {
        // Step 1: Filter message stream
        var logMessages = Scenario.Messages
            .InTopic<Log>("/rosout")
        
        // Step 2: Verify filtered messages
        logMessages.Should()
            .NotContain(msg => msg.Value.level == Log.FATAL);
    }
}
````

In step 1 the ROS message stream is filtered to contain only Log messages from topic `/rosout` (see [filtered section](filter.md) for more details on filtering).

Step 2 is to verify the selected messages.
IntegROS.NET builds on [Fluent Assertions](https://fluentassertions.com/introduction) to express your expectations.

1) Call `Should()` on your filtered messages or a single message field. 
   This opens the world of fluent assertions and gives you access to an abundance of verification methods.
2) Call an assertion method.
   The assertion method lets your test fail, if the stated assertion does note hold.  

The two steps are discussed in the remainder of this section.

## Expectations start with Should()

The `Should()` method starts a new expectation (Fluent Assertion).
You can call `Should()` on every object you want to verify.
This can be a stream of ROS messages (like in the example above), a single object or a property of an object.

After calling `Should()` you get an abundance of new verification methods to express your expectations (like `NotCOntain()` in the example above). 
The methods offered depend on the type of the subject (the object on which you call `Should()`).
Calling `Should()` on a stream of ROS messages offers other methods than calling `Should()` on a single ROS message.
Calling `SHould()` on a `string` property is different than calling it on a `DateTime` property.

We will introduce the most prominent possibilities in the following sections.


## Verify a stream of ROS messages

### ROS Stream contains messages

`Contain(Predicate p)`, `OnlyContain(Predicate p)`, `NotContain(Predicate p)`

You can check if the ROS message stream contains elements matching the given expression.

`Contain` is satisfied, if *one element* of the stream matches the predicate. \
`OnlyContain` and `NotContain` are satisfied, if *all elements* of the stream match the predicate.

The following two assertions represent the same expectation "should not contain FATAL log messages":

```c#
Scenario.Messages
    .InTopic<Log>("/rosout")
    .Should()
    .NotContain(msg => msg.Value.level == Log.FATAL);
```

```c#
Scenario.Messages
    .InTopic<Log>("/rosout")
    .Should()
    .OnlyContain(msg => msg.Value.level < Log.FATAL);
```

### Verify number of elements

`BeEmpty()`, `NotBeEmpty()`, \
`HaveCount(int expected)`, `HaveCountGreaterThan(int expected)`, `HaveCountLessThan(int expected)`

Checks the number of ROS messages in the stream.

The following two assertions again represent the same expectation (should not contain FATAL log messages):
```c#
Scenario.Messages
    .InTopic<Log>("/rosout")
    .Where(msg => msg.Value.level == Log.FATAL)
    .Should()
    .BeEmpty();
```

```c#
Scenario.Messages
    .InTopic<Log>("/rosout")
    .Where(msg => msg.Value.level == Log.FATAL)
    .Should()
    .HaveCount(0);
```


### More verification methods

Ask your IDE's autocomplete suggestions after calling `Should()` and/or read [Fluent Assertions documentation on collection assertions](https://fluentassertions.com/collections/) for more options.



## Verify fields of a ROS message

To verify a field of a ROS message, call `Should()` on a property of the message object followed by a fluent assertion.

The following example verifies, that the first error message occured after January 1st 2020:
```c#
var errorMessage = Scenario.Messages
    .InTopic<Log>("/rosout")
    .First(m => m.Value.level >= Log.ERROR);

errorMessage.TimeStamp.Should().BeAfter(new DateTime(2020, 01, 01));
```

Verify that the message looks like expected:
```c#
var errorMessage = Scenario.Messages
    .InTopic<Log>("/rosout")
    .First(m => m.Value.level >= Log.ERROR);

errorMessage.Value.name.Should().Be("FailingNodeName");
errorMessage.Value.msg.Should().Be("An error occurred");
```
Note: `name` and `msg` are fields of ROS message type [`rosgraph_msgs/Log`](http://docs.ros.org/en/api/rosgraph_msgs/html/msg/Log.html)


### More verification methods

Ask your IDE's autocomplete suggestions after calling `Should()` and/or read [Fluent Assertions documentation](https://fluentassertions.com/introduction) for more options:
- [Basic Assertions](https://fluentassertions.com/basicassertions/)
- [Nullable types (Objects)](https://fluentassertions.com/nullabletypes/)
- [Numeric types](https://fluentassertions.com/numerictypes/)
- [Dates & Times](https://fluentassertions.com/datetimespans/)
- [Booleans](https://fluentassertions.com/booleans/)
- [Strings](https://fluentassertions.com/strings/)
- [Enums](https://fluentassertions.com/enums/)