// Demo Code for SerialCommand Library
// Steven Cogswell
// May 2011
/*

Modified the FirmWare from the code by kroimon, which is a heavily simplified versin of Steven's original.
https://github.com/kroimon/Arduino-SerialCommand


laser :
L => ask laser configuration => response number of lasers
L x => ask status of laser index X
      => 'x' laser number
L x y  => turn laser on or off 
    'x' laser number
    'y' state: 0=off 1=on

Table :
T => table position
T S => return the number of steps for a revolution 
T C => Set Current position as 0 (for absolute positionning
T R xxx => rotate relatively xxx steps 
T A xxx => turn position absolute xxx steps 

unknow command 
=> response "What?"

*/

//#include <SerialCommand.h>
#include "SerialCommand.h"
#include "configuration.h"
#include <AccelStepper.h>



AccelStepper stepper1(HALFSTEP, motorPin1, motorPin3, motorPin2, motorPin4);
SerialCommand SCmd;     // The demo SerialCommand object

void setup() {


  Serial.begin(9600);


//SETUP LASER
  pinMode(LASER_PIN_1, OUTPUT);
  pinMode(LASER_PIN_2, OUTPUT);
  pinMode(LASER_PIN_3, OUTPUT);
  pinMode(LASER_PIN_4, OUTPUT);
  digitalWrite(LASER_PIN_1, LOW);
  digitalWrite(LASER_PIN_2, LOW);
  digitalWrite(LASER_PIN_3, LOW);
  digitalWrite(LASER_PIN_4, LOW);
  SCmd.addCommand("L",LaserCommand);
  SCmd.addCommand("l",LaserCommand);
  
 //SETUP TURN TABLE
  stepper1.setMaxSpeed(500.0);
  stepper1.setAcceleration(200.0);
  stepper1.setSpeed(400);
  stepper1.moveTo(0);
  stepper1.runToPosition(); 
  SCmd.addCommand("T",TableCommand);
  SCmd.addCommand("t",TableCommand);
  
  // Setup callbacks for SerialCommand commands
  //SCmd.addCommand("ON",    LED_on);          // Turns LED on
  //SCmd.addCommand("OFF",   LED_off);         // Turns LED off
  SCmd.addCommand("HELLO", sayHello);        // Echos the string argument back
  SCmd.addCommand("P",     processCommand);  // Converts two arguments to integers and echos them back
  SCmd.addDefaultHandler(unrecognized);      // Handler for command that isn't matched  (says "What?")
  Serial.println(FIRMWARE_VERSION);
  Serial.println("Ready");
  Serial.flush();//NESSESARY?
}

void loop() {
  SCmd.readSerial();     // We don't do much, just process serial commands
}

/*
void LED_on() {
  Serial.println("LED on");
  digitalWrite(arduinoLED, HIGH);
}

void LED_off() {
  Serial.println("LED off");
  digitalWrite(arduinoLED, LOW);
}
*/

void sayHello() {
  char *arg;
  arg = SCmd.next();    // Get the next argument from the SerialCommand object buffer
  if (arg != NULL) {    // As long as it existed, take it
    Serial.print("Hello ");
    Serial.println(arg);
  }
  else {
    Serial.println("Hello, whoever you are");
  }
}


void processCommand() {
  int aNumber;
  char *arg;

  Serial.println("We're in processCommand");
  arg = SCmd.next();
  if (arg != NULL) {
    aNumber = atoi(arg);    // Converts a char string to an integer
    Serial.print("First argument was: ");
    Serial.println(aNumber);
  }
  else {
    Serial.println("No arguments");
  }

  arg = SCmd.next();
  if (arg != NULL) {
    aNumber = atol(arg);
    Serial.print("Second argument was: ");
    Serial.println(aNumber);
  }
  else {
    Serial.println("No second argument");
  }
}

// This gets set as the default handler, and gets called when no other command matches.
void unrecognized(const char *command) {
  Serial.println("What?");
}


///////////////////////////////ADD LASER FUNC
int getLaserPin(int laserIndex)
{
       if(laserIndex==0)
         return LASER_PIN_1; 
       else if(laserIndex==1)
         return LASER_PIN_2; 
       else if(laserIndex==2)
         return LASER_PIN_3; 
       else if(laserIndex==3)
         return LASER_PIN_4; 
       else
         return (-1); 
}

void LaserCommand()
{
  char *arg; 
  arg = SCmd.next(); 
  if (arg != NULL) 
  {
    int laserIndex=atoi(arg);
    char *arg2 = SCmd.next(); 
    if (arg2 == NULL) 
    {
       Serial.print("LASER_STATE: "); 
       Serial.print(laserIndex); 
       int pin =getLaserPin(laserIndex);
       Serial.print("("); 
       Serial.print(pin); 
       Serial.print(") = "); 
       if(pin>=0)
         Serial.println(digitalRead(pin)); 
       else
         Serial.println(-1); 
    }
    else 
    {
       int laserState = atoi(arg2);
       int pin =getLaserPin(laserIndex);
       if(pin>=0)
        digitalWrite( pin, laserState==1?HIGH:LOW);
       Serial.print("SET_LASER: "); 
       Serial.print(laserIndex); 
       Serial.print("("); 
       Serial.print(pin); 
       Serial.print(") = "); 
      Serial.println(digitalRead(pin)); 
     }
   }
   else 
   {
    Serial.print("LASER_COUNT: "); 
    Serial.println(LASER_COUNT); 
   }
}

/////////////////TURN TABLE
int RotationToSteps(int rotation)
{
  return rotation*STEP_BY_MINMOVE;
}
int StepsToRotation(int rot)
{
  return rot/STEP_BY_MINMOVE;
}

void TableCommand()
{
	 char *arg; 
	 arg = SCmd.next(); 
	 if (arg != NULL) 
	 {
		 if(arg[0]=='R'||arg[0]=='r') // T R
		 {
                    char *arg2 = SCmd.next(); 
                    int pos=atoi(arg2);
                    stepper1.move(RotationToSteps(pos));
                    stepper1.runToPosition();
		    Serial.print("RELATIVE ROTATION :"); 
		    Serial.print(pos); 
		    Serial.print(" => "); 
		    Serial.println(StepsToRotation(stepper1.currentPosition())); 
		 }
		 else if(arg[0]=='A'||arg[0]=='a') // T A
		 {
                    char *arg2 = SCmd.next(); 
                    int pos=atoi(arg2);
                    stepper1.moveTo(RotationToSteps(pos));
                    stepper1.runToPosition();
		    Serial.print("ABSOLUTE ROTATION "); 
		    Serial.print(pos); 
		    Serial.print(" => "); 
		    Serial.println(StepsToRotation(stepper1.currentPosition())); 
		 }
		 else if(arg[0]=='S'||arg[0]=='s') //T S
                  {
		    Serial.print("REVOLUTION STEPS "); 
		    Serial.print(" => "); 
		    Serial.println(StepsToRotation(REVOLUTION_STEP)); 
                  }
		 else if(arg[0]=='C' ||arg[0]=='s') //T C
                  {
                    stepper1.setCurrentPosition(0);
		    Serial.print("RESET CUTTENT POSITION "); 
		    Serial.print(" => "); 
		    Serial.println(StepsToRotation(stepper1.currentPosition())); 
                  }
                 else
                 {
		    Serial.print("Unknown Table command :"); 
		    Serial.println(arg); 
                 }
	 } 
  else {
     Serial.print("Position "); 
    Serial.println(StepsToRotation(stepper1.currentPosition())); 
  }
}

