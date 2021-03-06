# IntegROS unit tests for configuration without UML.Robotics.ROS Support

IntegROS supports ROS messages from different ROS implementations for .NET.

Messages can be...
* *Written by hand* using `RobSharper.Ros.MessageEssentials.RosMessageAttribute` annotations
* *Generated* from ROS files (*.msg, *.srv, *.action) using the RobSharper [dotnet rosmsg](https://github.com/jr-robotics/RobSharper.Ros.MessageCli) tool
which can generate messages for
  * RobSharper
  * ROS.NET (Only for JOANNEUM RESEARCH Robotics)
    
Messages for RobSharper are supported by default.
Support for ROS.NET messages require the `RobSharper.Ros.Adapters.UmlRobotics` NuGet package (which is a dependency for ROS.NET messages generated by dotnet rosmsg). 
IntegROS will automatically detect, if the package is available and add message parsing support on it's own.
As of 2021/04/14 ROS.NET support relies on infrastructure of JOANNEUM RESEARCH Robotics which is not publicly available.

This is a test project, where the no ROS.NET message package is installed.