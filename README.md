# Sardauscan

Sardauscan is a 3d laser scanner aimed to cost less than 30$ ... 

yes its cheap.. and i thinks i can call it "the cheapest 3d scanner on Earth" ;)


All the harware you need is a arduino nano,  1 to 4 line laser and a micro geared stepper (28BYJ-48) and a hercule hd twist webcam

the code of the main software is writen in c# and winforms.

It is based on a system of task and process.

a task is a operation : scan, save file, filter noise, smooth, build mesh.

a Process is a list of task.

you will be allowed via plugins to add to develop and add your own task. 

The software will no be linked to a specific hardware.
by plugins, you will be allowed to add harware controller for camera, turn table and lasers.
For exemple if you wish to use another scanner, image capture or whatever.

directory "Firmware" => code for the arduino
directory "STL" the source stl and 123Design file.
directory "Sardauscan" => the client software

the code is C# and winforms and is build on a Visual 2010 version. so it's obviously for windows.
No make file, but visual solution file.
