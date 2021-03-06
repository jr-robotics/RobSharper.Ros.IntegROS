# IntegROS unit tests for configuration with UML.Robotics.ROS Support

IntegROS supports ROS messages from different ROS implementations for .NET.

Messages can be...
* written by hand using `RobSharper.Ros.MessageEssentials.RosMessageAttribute` annotations.
* generated from ROS files (*.msg, *.srv, *.action) using the RobSharper [dotnet rosmsg](https://github.com/jr-robotics/RobSharper.Ros.MessageCli) tool
  which can generate messages for
    * RobSharper
    * ROS.NET (Only for JOANNEUM RESEARCH Robotics)

Messages for RobSharper are supported by default.

Support for ROS.NET messages require the `RobSharper.Ros.Adapters.UmlRobotics` NuGet package (which is a dependency for ROS.NET messages generated by dotnet rosmsg).
IntegROS will automatically detect, if the package is available and add message parsing support on it's own.
As of 2021/04/14 ROS.NET support relies on infrastructure of JOANNEUM RESEARCH Robotics which is not publicly available.

This is a test project, where a ROS.NET message package is installed (and should be supported).

Unit tests in this project are only working inside of JOANNEUM RESEARCH's infrastructure.
By default, test in this project will be ignored. Compile with property JRINTERNAL=True if they should not be ignored (and access to JOANNEUM RESEARCH infrastructure is available).

```
dotnet build -p:JRINTERNAL=True
dotnet test --no-build --verbosity normal --logger junit -p:CollectCoverage=true -p:CoverletOutputFormat=cobertura -p:CoverletOutput=TestResults/
```