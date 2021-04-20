#include <ArduinoBearSSL.h>
#include <AES128.h>
#include <uECC.h>
#include <ArduinoBLE.h>
#include <Arduino_APDS9960.h>

/*
 * Written by Emma Pinegar for the HGTP senior capstone project.
 * Last updated February 9, 2021.
 */

 // BLE gesture service
BLEService gestureService("913CF3FD-7173-43A5-82F4-DFD6F61BAF5F");

// BLE gesture characteristic
BLEStringCharacteristic gestureChar("44B1DF4E-15C8-4F97-9F34-123D33B0C29D",  BLERead | BLEWrite | BLENotify, 16);

// BLE charateristic value for no gesture
String NO_GESTURE = "0000000000000000";

// BLE proximity service
BLEService proximityService("449F54C6-8F5F-42A4-9E18-BCBAA519ADD4");

// BLE proximity characteristic
BLEStringCharacteristic proximityChar("258FE654-B36A-49EC-BB40-33134B9E2C8E",  BLERead | BLENotify, 16);

// BLE charateristic value for no gesture
String NO_PROXIMITY = "0000000000000000";

//BLE battery service
BLEService batteryService("180F");

//BLE battery characteristic
BLEUnsignedCharCharacteristic batteryChar("2A19", BLERead | BLENotify);

int INIT_BATTERY = 100;

uint8_t privKey[32];
uint8_t pubKey[64];
uint8_t secretKey[32];

String appPubKeyMessage;

bool SECURE = false;

const int GESTURE = 1;
const int PROXIMITY = 2;
const int BATTERY = 3;

void setup() 
{
  Serial.begin(9600);
  while(!Serial);

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

  batteryService.addCharacteristic(batteryChar);
  BLE.addService(batteryService);

  BLE.setEventHandler(BLEConnected, connectedHandler);
  BLE.setEventHandler(BLEDisconnected, disconnectedHandler);

  gestureChar.writeValue(NO_GESTURE);
  proximityChar.writeValue(NO_PROXIMITY);
  batteryChar.writeValue(INIT_BATTERY);

  gestureChar.setEventHandler(BLEWritten, characteristicWrittenHandler);

  BLE.advertise();

  Serial.println("Bluetooth device active, waiting to connect...");
  
  Serial.println("testing encryption..");
  //seed the random number generator and set the RNG function
  randomSeed(analogRead(0));
  uECC_set_rng(&RNG);
  
  //generate public/private key pair using secp256r1
  if (!makeKey(pubKey, privKey))
    return;
    
  //verify the public key is on the curve
  if (!checkKey(pubKey))
    return;

  Serial.println("private key: ");
  printArray(privKey, 32);
  Serial.println("public key: ");
  printArray(pubKey, 64);
}

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
  int count = 0;
  while (central.connected())
  {
    if (SECURE)
    {
      // update the value
      if (APDS.gestureAvailable())
      {
        updateGesture();
      }
//      if (count%10000 == 0 && APDS.proximityAvailable())
//      {
//        updateProximity();
//      }
//      if (count%30000 == 0)
//      {
//        updateBattery();
//      }
    } 
    else if (gestureChar.subscribed())
    {
      sendPublicKey();
    }
    count++;
  }
}

/*
 * Handles disconnection with central device
 */
void disconnectedHandler(BLEDevice central)
{
  SECURE = false;
  appPubKeyMessage = "";
  digitalWrite(LED_BUILTIN, LOW);
  Serial.print("Disconnected from central: ");
  Serial.println(central.address());
}

/*
 * Handles when the value of the gesture characteristic is written by another device
 */
void characteristicWrittenHandler(BLEDevice central, BLECharacteristic characteristic)
{
  if (gestureChar.written() && !SECURE)
  {
    String nextMessage = gestureChar.value();
    Serial.println("received a message on the gesture charateristic");
    Serial.println(nextMessage);
    appPubKeyMessage.concat(nextMessage);
    int nextMessageLength = nextMessage.length();
    int messageLength = appPubKeyMessage.length();
    
    if (nextMessageLength != 16 || messageLength > 128)
    {
      Serial.println("public key was not received properly");
      central.disconnect();
      return;
    }
    else if (messageLength == 128)
    {     
      Serial.println("received app's public key message:");
      Serial.println(appPubKeyMessage);
      uint8_t appPubKey[64];
      hexToArray(appPubKeyMessage, appPubKey, 64);
      Serial.println("checking key to verify it is on secp256r1");
      
      if (!checkKey(appPubKey))
      {
        central.disconnect();
        return;
      }
      
      if (!makeSharedKey(appPubKey, privKey, secretKey))
      {
        //TODO: what do we do when we can't make a shared secret
        central.disconnect();
        return;
      }
  
      String sharedKeyHex = arrayToHex(secretKey, 32);
      Serial.println("shared secret key in hex:");
      Serial.println(sharedKeyHex);
      //TODO: hash shared secret?
      SECURE = true;
    }
  }
}

/* 
 * Check the gesture sent from the sensor.
 * Write the new gesture in the gesture characteristic. 
 */
void updateGesture() 
{
  int gestureValue = APDS.readGesture();
  printGesture(gestureValue);
  sendMessage(GESTURE, gestureValue);
  Serial.println("encrypted gesture sent");
}

/*
 * encrypts and sends the value on the specified charateristic
 */
void sendMessage(int sendChar, int messageVal)
{
  String messageHex;
  String ivHex;
  prepareMessage(messageVal, &messageHex, &ivHex);
  writeEncryptedValue(sendChar, ivHex, 32);
  writeEncryptedValue(sendChar, messageHex, 32);
}

/*
 * prepare encrypted message and iv to be sent 
 */
void prepareMessage(int messageVal, String *messageHex, String *ivHex)
{
  byte message[16];
  message[0] = messageVal;
  if (!padMessage(message, sizeof(message), 1))
    return;

  //generate the IV
  byte encode_iv[16];
  if (!generateIV(encode_iv, ivHex))
    return;
  if (!encryptMessage(secretKey, message, encode_iv, messageHex))
    return;
}

/*
 * Writes the encrypted value to the specified characterisitc in 16 byte chunks
 */
void writeEncryptedValue(int sendChar, String message, int messageLength)
{
  switch(sendChar)
  {
    case GESTURE:
      for (int i = 0; i < messageLength; i+=16)
      {
        gestureChar.writeValue(message.substring(i,i+16));
        delay(200);
      }
      Serial.println("encrypted message sent on gesture charateristic");
      break;
    case PROXIMITY:
      for (int i = 0; i < messageLength; i+=16)
      {
        proximityChar.writeValue(message.substring(i,i+16));
        delay(200);
      }
      Serial.println("encrypted message sent on proximity charateristic");
      break;
    case BATTERY:
      for (int i = 0; i < messageLength; i+=16)
      {
        //batteryChar.writeValue(message.substring(i,i+16));
        delay(200);
      }
      Serial.println("encrypted message sent on battery characteristic");
      break;
    default:
      Serial.println("the specified charateristic does not exist");
      break;
  }
}

/*
 * Check the proximity sent from the sensor.
 * Write the new gesture in the proximity characteristic.
 */
void updateProximity()
{
  int scaledProximity = printProximity(APDS.readProximity());
  sendMessage(PROXIMITY, scaledProximity);
  Serial.println("encrypted proximity sent");
  //proximityChar.writeValue(scaledProximity);
}

/*
 * Write the new battery level in the battery characteristic.
 */
void updateBattery()
{
  int percentVoltage = 90;
  Serial.print("Battery percentage: ");
  Serial.println(percentVoltage);
  batteryChar.writeValue(percentVoltage);
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

/*
 * writes the public key in 16 byte parts on the gesture characteristic
 */
void sendPublicKey ()
{
  String arduinoPubKeyMessage = arrayToHex(pubKey, 64);
  Serial.println("sending Arduino's public key message");
  Serial.println(arduinoPubKeyMessage);
  gestureChar.writeValue("starting");
  delay(200);
  writeEncryptedValue(GESTURE, arduinoPubKeyMessage, 128);
}

/*
 * Makes a public/private key pair on curve secp256r1. 
 */
int makeKey (uint8_t *pubKey, uint8_t *privKey)
{
  if (!uECC_make_key(pubKey, privKey))
  {
    Serial.println("generating the public/private key pair failed");
    Serial.println("exiting...");
    return 0;
  }
  Serial.println("generated the public private key pair");
  return 1;
}

/*
 * Checks if the given public key is on curve secp256r1. 
 */
int checkKey (uint8_t *pubKey)
{
  if (!uECC_valid_public_key(pubKey))
  {
    Serial.println("public key is not valid");
    Serial.println("exiting...");
    return 0;
  }
  Serial.println("public key has been validated");
  return 1;
}

/*
 * Makes a shared key using the given public and private keys.
 */
int makeSharedKey (uint8_t *pubKey, uint8_t *privKey, uint8_t *secretKey)
{
  if (!uECC_shared_secret(pubKey, privKey, secretKey))
  {
    Serial.println("generating shared secret failed");
    Serial.println("exiting...");
    return 0;
  }
  Serial.println("shared secret generated successfully");
  return 1;
}

/*
 * Pads a message to fill the 16 bytes. 
 */
int padMessage (uint8_t *message, size_t num_bytes, size_t offset)
{
  if (!RNG(&message[offset], num_bytes-offset))
  {
    Serial.println("padding message failed");
    Serial.println("exiting...");
    return 0;
  }
  Serial.println("successfully padded message");
  printArray(message, num_bytes);
  return 1;
}

/*
 * Generates an IV to be used for encryption. 
 */
int generateIV (uint8_t *encode_iv, String *ivHex)
{
  if (!RNG(&encode_iv[0], 16))
  {
    Serial.println("generating iv failed");
    Serial.println("exiting...");
    return 0;
  }
  Serial.println("generated iv");
  printArray(encode_iv, 16);
  *ivHex = arrayToHex(encode_iv, 16);
  Serial.println(*ivHex);
  return 1;
}

/*
 * Encrypts the message using the given secret key and IV. 
 */
int encryptMessage (uint8_t *secretKey, uint8_t *message, uint8_t *encode_iv, String *messageHex)
{
  if (!AES128.runEnc(secretKey, 32, message, 16, encode_iv))
  {
    Serial.println("encrypting failed");
    Serial.println("exiting...");
    return 0;
  }
  Serial.println("message encrypted");
  printArray(message, 16);
  *messageHex = arrayToHex(message, 16);
  Serial.println(*messageHex);
  return 1;
}

/*
 * Decrypts the message given the proper key and IV.
 */
int decryptMessage (uint8_t *secretKey, uint8_t *message, uint8_t *decode_iv)
{
  if (!AES128.runDec(secretKey, 32, message, 16, decode_iv))
  {
    Serial.println("decryption failed");
    Serial.println("exiting...");
    return 0;
  }
  Serial.println("message decrypted");
  printArray(message, 16);
  return 1;
}

/*
 * Prints the values from an array.
 */
void printArray (uint8_t *data, size_t num_bytes)
{
  for (int i = 0; i < num_bytes; i++)
  {
    Serial.print(data[i]);
    Serial.print(" ");
  }
  Serial.println("");
}

/*
 * Turns a decimal byte array into a string of the equivalent hex values. 
 */
String arrayToHex (uint8_t *data, size_t num_bytes)
{
  String hex = "";
  for (int i = 0; i < num_bytes; i++)
  {
    String nextByte = String(data[i], HEX);
    if (data[i] < 16)
    {
      hex.concat(0);
    }
    hex.concat(nextByte);
  }
  return hex;
}

/*
 * Turns a string of hex bytes into the equivalent decimal byte array.
 */
void hexToArray (String hex, uint8_t *data, size_t num_bytes)
{
  for (int i = 0; i < num_bytes*2; i+=2)
  {
    byte nextByte = hexToDec(hex.charAt(i), hex.charAt(i+1));
    data[i/2] = nextByte;
  }
}

/*
 * Turns a hex byte into a decimal byte.
 */
byte hexToDec (char firstNibble, char secondNibble)
{
  return (asciiToByte(firstNibble) << 4) + asciiToByte(secondNibble);
}

/*
 * Turns an ascii(hex) character into a decimal value.
 */
byte asciiToByte (char nibble)
{
  byte nibble_val = (byte)nibble;
  if (nibble_val > 47 && nibble_val < 58)
  {
    nibble_val-=48;
  }
  else if (nibble_val > 96 && nibble_val < 103)
  {
    nibble_val-=87;
  }
  return nibble_val;
}

/*
 * Random number generator for the encryption.
 * Size is the number of bytes to randomly generate
 * which will be stored starting at dest.
 */
int RNG(uint8_t *dest, unsigned size)
{
  for (int i = 0; i < size; i++)
  {
    dest[i] = random(256);
  }
  return 1;
}
