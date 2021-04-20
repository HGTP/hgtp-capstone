**Purpose:**

Code developed for the Arduino Nano 33 BLE Sense. EncryptionWBLE is the sketch used for the most recent iteration of the mobile app.

**Dependencies:**

Import the following libraries in your library manager in the Arduino IDE:

1. ArduinoBLE
2. Arduino_APDS9960

Download the following libraries from their repositories and add them as libraries in the Arduino IDE:

3. [ArduinoBearSSL](https://github.com/arduino-libraries/ArduinoBearSSL)
4. [micro-ecc-static](https://github.com/kmackay/micro-ecc/tree/static)


**Requirements:**

1. An Arduino Nano BLE Sense is required to run anything developed for the Arduino. 


**Set Up:**

1. Download the files and upload them to the Arduino IDE. 
2. Arduino must be plugged in and the port of it must be used when uploading. LightBlue can be used to test BLE connectivity and message contents on iOS. Similar apps are available for Android. Note this is only for the sketches that are not encrypted.
3. Verify and run the sketch of your choice. Sketch names will incidate their purpose (more detail on each of them is included below), some sketches include a README with additional instructions. 


**Sketches:**

1. **ArduinoBluetooth:** The baseline functionality of the Arduino used in our prototype demo. Check the wikis for more information on the BLE details.
2. **ArduinoBluetoothTest:** A sketch that tests the BLE function without the gesture input. This sketch is compatible with additional Arduinos because is does not use the APDS sensor.
3. **EncryptionTestWBLE:** Encryption and BLE combined, but still faking the gesture values. The static micro-ecc library should be used. 
4. **EncryptionWBLE:** Encryption, BLE, and gesture sensor combined. See the wikis for more on the details of the encryption handshake. The static micro-ecc library should be used. Has encrypted gesture service with available but commented out proximity service as well as unencrypted standard battery service.


