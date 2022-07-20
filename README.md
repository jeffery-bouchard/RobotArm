# Optically Aware Robot (OAR)

## About
This project is a GUI application, written in C#, that communicates with a robot arm via a serial port. The robot arm has four actuators that control the position of the arm: base, elbow, shoulder, and gripper. There is also a proximity sensor which detects objects for the purpose of object avoidance. This application is intended to work in conjunction with a connected robot arm which sends and receives commands over a serial port. The application includes two primary ways of communicating with the robot: manually via the operator tab, and automatically via the programmer tab.

## User Guide

### Serial Port Connection
The setup menu has options for connecting and disconnecting to/from a serial port. When connect is selected, a dialog box is displayed with a drop-down menu showing the available serial ports.

### Operator
The operator tab displays a diagram of the robot arm with sliders, increment and decrement buttons, and positional text boxes for each actuator. Additionally, a home button is provided to reset the robot arm to the center position.

![Operator Tab](/docs/img/operator_tab.jpg)

Users can also manually control the robot via the following quick keys:

| Motor | Decrement | Increment |
| -- | -- | -- |
| Gripper | X | S |
| Elbow | ↓ | ↑ |
| Shoulder | ← | → |
| Base | Z | C |

![Quick Keys](/docs/img/quick_keys.jpg)

### Programmer
The programmer tab provides a large text box that allows users to create and run scripts. The following commands are supported:

- gripper(position): sets the gripper position
- elbow(position): sets the elbow position
- shoulder(position): sets the shoulder position
- base(position): sets the base position
- proximity(limit): sets the object avoidance proximity limit
- wait(duration): waits the specified duration in seconds

The automation section provides a means for operators to load, run, and stop scripts. Scripts may be saved via the file menu.

![Programmer Tab](/docs/img/programmer_tab.jpg)

### Object Avoidance
The safety section includes options for object detection and avoidance. The object avoidance button toggles the feature on or off. The text box allows users to enter the proximity limit in inches. The LED icon changes color to show that the path is clear (green), or there is an object detected within the proximity limit (red). If object detection is disabled, the LED icon turns grey.

![Object Avoidance](/docs/img/object_avoidance.jpg)

![Object Detected](/docs/img/object_detected.jpg)

## Architecture

The architecure was determined based on two architecturally significant requirements. The first was a latency requirement for the GUI which states the system shall process the command with an average latency of 50 milliseconds. The second was a safety requirement which states the motor position is changed with a maximum velocity of 18 degrees of rotation per second. As a result, a command design pattern was used with a command queue. The queue allowed the latency requirement to be possible while ensuring the maximum velocity safety requirement was met. Additionally, the classes were organized with the physical robot arm components that are being controlled. A UML class diagram is shown below, along with a sequence diagram that describes gripper motor incrementation. The other commands operate in a similar way.

![UML_Class_Diagram](/docs/img/UML_Class.jpg)

![UML_Sequence_Diagram](/docs/img/UML_Sequence.jpg)

## Test

Tests are included for unit test, integration test, and system test. Unit test and integration test were executed using the unit test framework. System tests were executed using Ranarex Studio. Tests were designed to focus on key functionalities of the application and interfaces.
