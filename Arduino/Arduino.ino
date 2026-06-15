#include <Wire.h>

const int switchPin = 2;
int lastState = HIGH;

// MPU6050
const int MPU_ADDR = 0x68;

int16_t ax, ay, az;
int16_t lastAx, lastAy, lastAz;


const long shakeThreshold = 12000;   
const int shakeCountRequired = 2;   
const unsigned long shakeCooldown = 500; 
// ====================

int shakeCount = 0;
unsigned long lastShakeTime = 0;

void setup() {
  pinMode(switchPin, INPUT_PULLUP);
  Serial.begin(9600);

  Wire.begin();

  Wire.beginTransmission(MPU_ADDR);
  Wire.write(0x6B);
  Wire.write(0);
  Wire.endTransmission(true);

  readAccel();

  lastAx = ax;
  lastAy = ay;
  lastAz = az;
}

void loop() {

  // -------------------
  // OpeneClose
  // -------------------

  int state = digitalRead(switchPin);

  if (state != lastState) {
    delay(20);

    state = digitalRead(switchPin);

    if (state != lastState) {
      lastState = state;

      if (state == LOW) {
        Serial.println("CLOSED");
      } else {
        Serial.println("OPEN");
      }
    }
  }

  // -------------------
  // Shake
  // -------------------

  readAccel();

  long diff =
    abs(ax - lastAx) +
    abs(ay - lastAy) +
    abs(az - lastAz);

  if (diff > shakeThreshold) {
    shakeCount++;
  } else {
    shakeCount = 0;
  }

  if (shakeCount >= shakeCountRequired) {

    if (millis() - lastShakeTime > shakeCooldown) {

      Serial.println("SHAKE");

      lastShakeTime = millis();
    }

    shakeCount = 0;
  }

  lastAx = ax;
  lastAy = ay;
  lastAz = az;

  delay(20);
}

void readAccel() {

  Wire.beginTransmission(MPU_ADDR);
  Wire.write(0x3B);
  Wire.endTransmission(false);

  Wire.requestFrom(MPU_ADDR, 6, true);

  ax = Wire.read() << 8 | Wire.read();
  ay = Wire.read() << 8 | Wire.read();
  az = Wire.read() << 8 | Wire.read();
}