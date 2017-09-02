# Sardauscan

Sardauscan is a 3D laser scanner aimed to cost less than 30$

It is quite possibly the lowest-cost 3D scanner currently available.

Hardware List: 
- Arduino Nano
- 1 to 4 line lase
-a a micro geared stepper (28BYJ-48)
- Hercule HD Twist webcam

This software is written in C# and Winforms, and is based on a system of tasks and processes.

A task is an operation, such as: scan, save file, filter noise, smooth, or build mesh.

A process is simply a list of tasks.

It is possible to develop and add your own tasks via plugins.

The software will not be linked to any specific hardware.
Using plugins, one could add harware controllers for additional cameras, turn tables and lasers.
For example, if you wish to use another scanneror webcam.

Directory "Firmware" => code for the arduino
Directory "STL" the source stl and 123Design file.
Directory "Sardauscan" => the client software

The code is written in C# and Winforms and is build on Visual Basic 2010, for Windows.

the firmware needed for the Adafruit Motor Shield library can be found here:
https://github.com/adafruit/AccelStepper

