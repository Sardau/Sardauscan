/*
 * Modified Firmware from Sardauscan (https://github.com/Sardau/Sardauscan) 
 * to make the Hardware compatible with Horus 3d Scan software from BQ (https://github.com/bqlabs/horus)
 * 
 * The firmware understands just the basic G-Codes for turning the tables and enabling or disabling the lasers 
 * (setting the speed does not work) but that's enough for scanning.
 * Feel free to modify or extend the code
 * 
 * You need the AccelStepper library from https://github.com/adafruit/AccelStepper
 *  
 *  Andi M.
 */

#include <AccelStepper.h>
#include "configuration.h"

char SerialBuffer[255];
unsigned char Serialpos=0;

AccelStepper stepper1(HALFSTEP, motorPin1, motorPin3, motorPin2, motorPin4);

int RotationToSteps(int rotation)
{
  return rotation*STEP_BY_MINMOVE;
}

void HandleSerial(void)
{
  SerialBuffer[Serialpos]=Serial.read();
  if(SerialBuffer[Serialpos]==0x18)
  {
      Serial.print("\r\nHorus 0.2 ['$' for help]\r\n");
      Serialpos=0;
  }
  else if(SerialBuffer[Serialpos]=='\r' || SerialBuffer[Serialpos]=='\n')
  {
    SerialBuffer[Serialpos+1]='\0';
    if(!strcmp(SerialBuffer,"$\r") || !strcmp(SerialBuffer,"$\n") || !strcmp(SerialBuffer,"$\r\n")) //answer to "$", not needed
    {
      Serial.println("$$ (view settings)");
      Serial.println("$# (view # parameters)");
      Serial.println("$G (view parser state)");
      Serial.println("$I (view build info)");
      Serial.println("$N (view startup blocks)");
      Serial.println("$x=value (save setting)");
      Serial.println("$Nx=line (save startup block)");
      Serial.println("$C (check gcode mode)");
      Serial.println("$X (kill alarm lock)");
      Serial.println("~ (cycle start)");
      Serial.println("! (feed hold)");
      Serial.println("? (current status)");
      Serial.println("ctrl-x (reset)");
      Serial.println("ok");
    }
    else if(!strncmp(SerialBuffer,"G50",3))     //Zero all
    {
      stepper1.setCurrentPosition(0);
      Serial.println("ok");
    }
    else if(!strncmp(SerialBuffer,"M0",2))     //Pause
    {
      Serial.println("ok");
    }
    else if(!strncmp(SerialBuffer,"M2",2))     //Reset
    {
      Serial.println("ok");
    }
    else if(!strncmp(SerialBuffer,"M17",3))     //Enable Steppers
    {
      stepper1.enableOutputs();
      Serial.println("ok");
    }
    else if(!strncmp(SerialBuffer,"M18",3))     //Disable Steppers
    {
      stepper1.disableOutputs();
      Serial.println("ok");
    }
    else if(!strncmp(SerialBuffer,"M50",3))     //Read LDR
    {
      Serial.println("ok");
    }
    else if(!strncmp(SerialBuffer,"M70T",4))     //Laser off
    {
      if(SerialBuffer[4]=='1')
        digitalWrite( LASER_PIN_1, 0);
      if(SerialBuffer[4]=='2')
        digitalWrite( LASER_PIN_4, 0);
      Serial.println("ok");
    }
    else if(!strncmp(SerialBuffer,"M71T",4))     //Laser on
    {
      if(SerialBuffer[4]=='1')
        digitalWrite( LASER_PIN_1, 1);
      if(SerialBuffer[4]=='2')
        digitalWrite( LASER_PIN_4, 1);
      Serial.println("ok");
    }
    else if(!strncmp(SerialBuffer,"G1",2))     //movement
    {
      if(SerialBuffer[2]=='F')              //speed (not working)
      {
        char arg2[10];
        for(int i=4;i<strlen(SerialBuffer);i++)
        {
          arg2[i-4]=SerialBuffer[i];
        }
        long rotspeed=atoi(arg2);
//        stepper1.setMaxSpeed(rotspeed);
      }
      if(SerialBuffer[2]=='X')              // move to degree
      {
        char arg2[10];
        for(int i=3;i<strlen(SerialBuffer);i++)
        {
          arg2[i-3]=SerialBuffer[i];
        }
        long pos=(atoi(arg2));
        pos=(pos*512)/360;
        stepper1.moveTo(RotationToSteps(pos));
        stepper1.runToPosition();
      }
      Serial.println("ok");
    }
    else if(!strncmp(SerialBuffer,"$1",2))     //
    {
      Serial.println("ok");
    }
    Serialpos=0;
  }
  else
    Serialpos++;
}

void setup() {
  stepper1.setMaxSpeed(500.0);
  stepper1.setAcceleration(200.0);
  stepper1.setSpeed(400);
  stepper1.moveTo(0);
  stepper1.runToPosition();
  Serial.begin(SERIAL_BAUD);
 
  pinMode(LASER_PIN_1, OUTPUT);
  pinMode(LASER_PIN_2, OUTPUT);
  pinMode(LASER_PIN_3, OUTPUT);
  pinMode(LASER_PIN_4, OUTPUT);
  digitalWrite(LASER_PIN_1, LOW);
  digitalWrite(LASER_PIN_2, LOW);
  digitalWrite(LASER_PIN_3, LOW);
  digitalWrite(LASER_PIN_4, LOW);

  Serial.print("\r\nHorus 0.2 ['$' for help]\r\n"); //send identification string at startup to make Horus recognise the board

  Serial.flush();
}


void loop() {
  if(Serial.available())
    HandleSerial();
  Serial.flush();
}



