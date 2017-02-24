#ifndef CONFIGURATION_h
#define CONFIGURATION_h

#define FIRMWARE_VERSION "Sardauscan V0.1a"

#define SERIAL_BAUD 115200
//57600

// Motor definitions
#define motorPin1  2     // IN1 on the ULN2003 driver 1
#define motorPin2  3     // IN2 on the ULN2003 driver 1
#define motorPin3  4     // IN3 on the ULN2003 driver 1
#define motorPin4  5     // IN4 on the ULN2003 driver 1

//tips (from Mark Benson)
//If anyone else is having problems with a BYJ48 stepper not doing anything, 
//change the HALFSTEP value to 4 & REVOLUTION_STEP to 2048
#define HALFSTEP 4
#define REVOLUTION_STEP 4096
#define STEP_BY_MINMOVE 4  // move by STEP_BY_MINMOVE (to avoid power loss when position is between step) 

#define LASER_COUNT 4
#define LASER_PIN_1 A1 //yellow
#define LASER_PIN_2 A2 //orange
#define LASER_PIN_3 A3 //green
#define LASER_PIN_4 13 //blue

#endif


