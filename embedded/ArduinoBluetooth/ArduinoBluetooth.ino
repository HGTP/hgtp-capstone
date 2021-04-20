/*
 * This code is based off of examples from the public domain for ArduinoBLE, BLEPeripheral, and Arduino_APDS9960
 * Emma Pinegar, HGTP, U of U Senior Capstone
 * Last updated November 23, 2020
 */

#include <ArduinoBLE.h>
#include <Arduino_APDS9960.h>

 // BLE gesture service
BLEService gestureService("913CF3FD-7173-43A5-82F4-DFD6F61BAF5F");

// BLE gesture characteristic
BLEUnsignedCharCharacteristic gestureChar("44B1DF4E-15C8-4F97-9F34-123D33B0C29D",  BLERead | BLENotify);

// BLE charateristic value for no gesture
int NO_GESTURE = 4;

 // BLE proximity service
BLEService proximityService("449F54C6-8F5F-42A4-9E18-BCBAA519ADD4");

// BLE proximity characteristic
BLEUnsignedCharCharacteristic proximityChar("258FE654-B36A-49EC-BB40-33134B9E2C8E",  BLERead | BLENotify);

// BLE charateristic value for no gesture
int NO_PROXIMITY = INT_MAX;

/*
 * Set up BLE and gesture sensor. Start advertising BLE service.
 */
void setup() 
{
  Serial.begin(9600);
  while (!Serial);

  pinMode(LED_BUILTIN, OUTPUT); 

  if (!BLE.begin())
  {
    Serial.println("Bluetooth failed");
    return;
  }

  if (!APDS.begin())
  {
    Serial.println("Gesture sensor failed");
    return;
  }

  BLE.setDeviceName("HGTP");
  BLE.setLocalName("HGTP");
  BLE.setAdvertisedService(gestureService);
  
  gestureService.addCharacteristic(gestureChar);
  BLE.addService(gestureService);
  
  proximityService.addCharacteristic(proximityChar);
  BLE.addService(proximityService);

  BLE.setEventHandler(BLEConnected, connectedHandler);
  BLE.setEventHandler(BLEDisconnected, disconnectedHandler);

  gestureChar.writeValue(NO_GESTURE);
  proximityChar.writeValue(NO_PROXIMITY);
  
  BLE.advertise();
  
  Serial.println("Bluetooth device active, waiting to connect...");
}

/*
 * Check for central device.
 */
void loop() 
{
  BLEDevice central = BLE.central();
  digitalWrite(LED_BUILTIN, HIGH);
  delay(200);
  digitalWrite(LED_BUILTIN, LOW);
  delay(200);
}

/*
 * Handles connection with central device
 */
void connectedHandler(BLEDevice central)
{
  Serial.print("Connected to central: ");
  Serial.println(central.address());
  digitalWrite(LED_BUILTIN, HIGH);
  while (central.connected())
  {
    if (APDS.gestureAvailable())
    {
      updateGesture();
    }
    
    if (APDS.proximityAvailable())
    {
      //updateProximity();
    }
    delay(200);
  }
}

/*
 * Handles disconnection with central device
 */
void disconnectedHandler(BLEDevice central)
{
  digitalWrite(LED_BUILTIN, LOW);
  Serial.print("Disconnected from central: ");
  Serial.println(central.address());
}


/* 
 * Check the gesture sent from the sensor.
 * Write the new gesture in the gesture characteristic. 
 */
void updateGesture() 
{
  int gesture = APDS.readGesture();
  printGesture(gesture);
  gestureChar.writeValue(gesture);
}

/*
 * Check the proximity sent from the sensor.
 * Write the new gesture in the proximity characteristic.
 */
void updateProximity()
{
  int proximity = APDS.readProximity();
  int scaledProximity = printProximity(proximity);
  if (scaledProximity != -1)
  {
    proximityChar.writeValue(scaledProximity);
  }
}

/*
 * Print the gesture detected
 */
void printGesture(int gesture)
{
  switch(gesture)
  {
    case GESTURE_UP:
      Serial.println("Detected UP gesture");
      break;
    case GESTURE_DOWN:
      Serial.println("Detected DOWN gesture");
      break;
    case GESTURE_LEFT:
      Serial.println("Detected LEFT gesture");
      break;
    case GESTURE_RIGHT:
      Serial.println("Detected RIGHT gesture");
      break;
    default:
      Serial.println("Detected NO gesture");
      break;
  }
}

/*
 * Print the detected proximity
 */
int printProximity(int proximity)
{
  if (proximity == -1)
  {
    Serial.println("ERROR");
    return proximity;
  }
  else
  {
    int scaledProximity = proximity/64;
    switch(scaledProximity)
    {
      case 0:
        Serial.println("VERY CLOSE");
        break;
      case 1:
        Serial.println("kinda close");
        break;
      case 2:
        Serial.println("nearby");
        break;
      case 3:
        Serial.println("kinda far");
        break;
      default:
        Serial.println("ERROR");
        scaledProximity = -1;
        break;
    }
    return scaledProximity;
  }
}