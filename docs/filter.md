# Filter the ROS message stream

Reducing the ROS message stream to messages of interest ist the first phase of every test method in IntegROS.NET.
Filtering means, that you restrict the result set to only those messages, that satisfy a specified condition.

You can access the complete stream of ROS messages of your scenario with `Scenario.Messages`.

````c#
[RosbagScenario(BagFiles.PathToROSBAG))]
public class MyTestClass : ForScenario
{
    [ExpectThat]
    public void Give_me_all_the_messages()
    {
        // Access to the ROS message stream.
        var allRosMessages = Scenario.Messages;
        
        // ...
    }
}
````

Once you have the ROS message stream in your hand, you can start to reduce it.
For that, use [Language INtegrated Query (LINQ)](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable?view=net-6.0) methods of .Net.
We`ve also added some useful filtering methods tailored to ROS.

### Navigation in this document

In this document, we describe the most common LINQ methods required to filter ROS messages:

* [InTopic - Filter by Topic](#intopic)
* [Where - Filter by message fields or other properties](#where)
* [Before & After - Filter the time beam](#before-after)
* [First & Last - Select a single element](#first-last)
  
If you don't find what you are looking for, consult the 
[standard query operators documentation (Microsoft Docs)](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/standard-query-operators-overview#related-sections).
Most likely, there is already a LINQ method which satisfies your needs.


## InTopic - Filter by Topic <a name="intopic"></a>

`InTopic(string topicNamePattern)`, `InTopic<TMessageType>(string topicNamePattern)`

`InTopic` methods return only messages in the specified topic in a new message stream.


The following example returns all messages from the ROS topic `/rosout`.

````c#
var messages = Scenario.Messages
    .InTopic("/rosout");
````

Or even better, as we know that `/rosout` contains only messages of ROS type `rosgraph_msgs/Log`,
we can tell the resulting message stream to treat all items as messages of type `Log`.
This does not help us now when we filter based on the topic name (since all messages in ROS are in a topic),
but it will help later on when we want to filter or assert based on some fields of the message. 

````c#
var messages = Scenario.Messages
    .InTopic("/rosout")
    .WithMessageType<Log>();
````

This can be done in an single method call too:

````c#
var messages = Scenario.Messages
    .InTopic<Log>("/rosout");
````

## Where - Filter by message fields or other properties <a name="where"></a>

`Where(Predicate p)`

The `Where` method ca filter the message stream based on a predicate.
You can pass lambda expression as a predicate.  

The following example returns all messages from the ROS topic `/rosout`.
It does the same as the `InTopic` filter, but a little bit more complicated.
However it is important to comprehend the concept for later examples.

````c#
var messages = Scenario.Messages
    .Where(m => m.Topic == "/rosout");
````

Note the lambda expression `m => m.Topic == "/rosout"`.
If this predicate evaluates to `true`, the message is kept.
If it evaluated to `false`, the message is thrown out of the resulting message stream.

Ok, let's  build a more meaningful filter.
We want to get all Log messages with log level Error or higher.

````c#
var messages = Scenario.Messages
    .InTopic<Log>("/rosout")
    .Where(m => m.Value.level >= Log.ERROR);
````

## Before & After - Filter the time beam <a name="before-after"></a>

`Before(DateTime)`, `Before(ITimestampMessage)`, `BeforeOrOn(DateTime)`, `BeforeOrOn(ITimestampMessage)`, \
`After(DateTime)`, `After(ITimestampMessage)`, `AfterOrOn(DateTime)`, `AfterOrOn(ITimestampMessage)`

The filter methods `Before` and `After` allow you to keep ROS messages before or after an event.
The event can be either a timestamp or another ROS message.

Here are some examples using `Before`. 
`After` works exactly the same way.

Select all Log messages after January 1st 2020:
````c#
var messages = Scenario.Messages
    .InTopic<Log>("/rosout")
    .After(new DateTime(2020, 1, 1);
````

Select all Log messages before the first error message occured:
````c#
var errorMessage = Scenario.Messages
    .InTopic<Log>("/rosout")
    .First(m => m.Value.level >= Log.ERROR);

var messages = Scenario.Messages
    .InTopic<Log>("/rosout")
    .Before(errorMessage);
````

### The special case of "In Between"

What if you want to select messages between two events?
Simply use both methods!

Select all Log messages on December 24th 2021:
````c#
var messages = Scenario.Messages
    .InTopic<Log>("/rosout")
    .After(new DateTime(2021, 12, 24);
    .Before(new DateTIme(2021, 12, 25);
````

### Before and After are exclusive

The two methods `Before` and `After` do what the name suggests. They select messages *before* and *after* an event.
But what if you want to include messages that occur simultaneously to the event?

Use `BeforeOrOn` and `AfterOrOn`.

Select all Log messages on December 24th 2021. 
`AfterOrOn` also includes messages published at exactly 2021/12/24 0:0:0.000.
````c#
var messages = Scenario.Messages
    .InTopic<Log>("/rosout")
    .AfterOrOn(new DateTime(2021, 12, 24);
    .Before(new DateTIme(2021, 12, 25);
````

## First & Last - Select a single element <a name="first-last"></a>

`First()`, `First(Predicate)`, `FirstOrDefault(Predicate)`, \
`Last()`, `Last(Predicate)`, `LastOrDefault(Predicate)`

You can select a single element from the message stream.

- `First()` returns the first element of the sequence
- `First(Predicate p)` returns the first element of the sequence that matches the predicate. Throws an exception if no element matches.
- `FirstOrDefault(Predicate p)`  returns the first element of the sequence that matches the predicate or null if no element matches.

`Last` methods work the same way.

**NOTE:** The methods return an element from the stream.
This is different to the other filter methods which return a new reduced stream.

Select the first error log message (if any):
````c#
var message = Scenario.Messages
    .InTopic<Log>("/rosout")
    .FirstOrDefault(m => m.Value.level = Log.ERROR);
````

## And many more...

There are more filter methods.
See the code completion suggestions in your IDE or documentation of [standard query operators (Microsoft Docs)](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/standard-query-operators-overview#related-sections).

# Next steps

Once you filtered the ROS messages you can start the next phase and [verify your expectations](verify.md).