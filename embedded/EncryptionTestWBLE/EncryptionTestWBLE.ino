#include <ArduinoBearSSL.h>
#include <AES128.h>
#include <uECC.h>
#include <ArduinoECCX08.h>
#include <ArduinoBLE.h>

/*
 * Written by Emma Pinegar for the HGTP senior capstone project.
 * Last updated February 8, 2021.
 */

 // BLE gesture service
BLEService gestureService("913CF3FD-7173-43A5-82F4-DFD6F61BAF5F");

// BLE gesture characteristic
BLEStringCharacteristic gestureChar("44B1DF4E-15C8-4F97-9F34-123D33B0C29D",  BLERead | BLEWrite | BLENotify, 16);

// BLE encrytion characteristic
//BLEStringCharacteristic encryptChar("60B244C6-27BE-4A26-B09D-384DE90BC449",  BLEWrite | BLENotify, 64);

// BLE charateristic value for no gesture
String NO_GESTURE = "4";

// BLE proximity service
BLEService proximityService("449F54C6-8F5F-42A4-9E18-BCBAA519ADD4");

// BLE proximity characteristic
BLEUnsignedCharCharacteristic proximityChar("258FE654-B36A-49EC-BB40-33134B9E2C8E",  BLERead | BLENotify);

// BLE charateristic value for no gesture
int NO_PROXIMITY = 256;

uint8_t privKey1[32];
uint8_t pubKey1[64];
uint8_t secretKey1[32];

bool SECURE = false;

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

  BLE.setDeviceName("HGTP");
  BLE.setLocalName("HGTP");
  BLE.setAdvertisedService(gestureService);
  
  gestureService.addCharacteristic(gestureChar);
//  gestureService.addCharacteristic(encryptChar);
  BLE.addService(gestureService);
  
  proximityService.addCharacteristic(proximityChar);
  BLE.addService(proximityService);

  BLE.setEventHandler(BLEConnected, connectedHandler);
  BLE.setEventHandler(BLEDisconnected, disconnectedHandler);

  gestureChar.writeValue(NO_GESTURE);
  proximityChar.writeValue(NO_PROXIMITY);

  gestureChar.setEventHandler(BLEWritten, characteristicWrittenHandler);

  BLE.advertise();

  Serial.println("Bluetooth device active, waiting to connect...");
  
  Serial.println("testing encryption..");
  //set the RNG function
  uECC_set_rng(&RNG);
  
  //generate public/private key pair #1 using secp256r1
  if (!makeKey(pubKey1, privKey1))
    return;
    
  //verify the public key is on the curve
  if (!checkKey(pubKey1))
    return;

  Serial.println("private key: ");
  printArray(privKey1, 32);
  Serial.println("public key: ");
  printArray(pubKey1, 64);
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
      // update the value once in a while
      if (count%30==0)
      {
        updateGesture();
      }
      // proximity will also be sent here when the app has that functionality
    } 
    else if (gestureChar.subscribed())
    {
      sendPublicKey();
    }
    count++;
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
 * Handles when the value of the gesture characteristic is written by another device
 */
void characteristicWrittenHandler(BLEDevice central, BLECharacteristic characteristic)
{
  Serial.println(gestureChar.value());
  if (gestureChar.written() && !SECURE)
  {
      Serial.println("received a message on the gesture charateristic");
      String appPubKeyMessage = gestureChar.value();
      
      //TODO: check that the message is the right length
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
      if (!makeSharedKey(appPubKey, privKey1, secretKey1))
      {
        //TODO: what do we do when we can't make a shared secret
        return;
      }
  
      String sharedKeyHex = arrayToHex(secretKey1, 32);
      Serial.println("shared secret key in hex:");
      Serial.println(sharedKeyHex);
      //TODO: hash shared secret?
      SECURE = true;
  }
}

/* 
 * Check the gesture sent from the sensor.
 * Write the new gesture in the gesture characteristic. 
 */
void updateGesture() 
{
  int gestureValue = 4;
  printGesture(gestureValue);
  //pad the message
  byte message[16];
  message[0] = gestureValue;
  
  if (!padMessage(message, sizeof(message), 1))
    return;

  //generate the IV
  byte encode_iv[16];
  String ivHex;
  if (!generateIV(encode_iv, &ivHex))
    return;
  String messageHex;
  if (!encryptMessage(secretKey1, message, encode_iv, &messageHex))
    return;
  String encryptedMessage = ivHex + messageHex;
  gestureChar.writeValue(ivHex);
  delay(300);
  gestureChar.writeValue(encryptedMessage);
  delay(300);
}

/*
 * Check the proximity sent from the sensor.
 * Write the new gesture in the proximity characteristic.
 */
void updateProximity()
{
  int scaledProximity = printProximity(32);
  proximityChar.writeValue(scaledProximity);
}

/*
 * Print the gesture detected
 */
void printGesture(int gesture)
{
  switch(gesture)
  {
    case 1:
      Serial.println("Detected UP gesture");
      break;
    case 2:
      Serial.println("Detected DOWN gesture");
      break;
    case 3:
      Serial.println("Detected LEFT gesture");
      break;
    case 4:
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
 * writes the public key in four 16 byte parts on the gesture characteristic
 */
void sendPublicKey ()
{
  String arduinoPubKeyMessage = arrayToHex(pubKey1, 64);
  Serial.println("sending Arduino's public key message");
  Serial.println(arduinoPubKeyMessage);
  gestureChar.writeValue("starting");
  delay(300);
  gestureChar.writeValue(arduinoPubKeyMessage.substring(0,15));
  delay(300);
  gestureChar.writeValue(arduinoPubKeyMessage.substring(16,31));
  delay(300);
  gestureChar.writeValue(arduinoPubKeyMessage.substring(32,47));
  delay(300);
  gestureChar.writeValue(arduinoPubKeyMessage.substring(48,63));
  delay(300);
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
  size_t num_bytes = size;
  if (!ECCX08.begin() || !ECCX08.locked())
  {
    return 0;
  }
  return ECCX08.random(*dest, num_bytes);
}
