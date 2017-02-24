# Sardauscan

Sardauscan is a 3d laser scanner aimed to cost less than 30$ ... 

Yes it's cheap.. and I think I can call it "the cheapest 3d scanner on Earth" ;)


All the harware you need is: an Arduino Nano, a 1 to 4 line laser, a micro geared stepper (28BYJ-48), and a hercule hd twist webcam

The main software is writen in c# and winforms.

It is based on a system of tasks and processes.

A task is an operation : scan, save file, filter noise, smooth, or build mesh.

A process is a list of tasks.

you will be able to, via plugins, develop and add your own tasks.

The software will not be linked to any specific hardware.
Using plugins, you will be able to add harware controllers for cameras, turn tables and lasers.
For example, if you wish to use another scanner, image capture or whatever.

Directory "Firmware" => code for the arduino
Directory "STL" the source stl and 123Design file.
Directory "Sardauscan" => the client software

The code is written in C# and winforms and is build on Visual Basic 2010. so it's obviously for windows.
No make file, but visual solution file.

the firmware needed for the arduino accellstepper library can be found here:
https://github.com/adafruit/AccelStepper

