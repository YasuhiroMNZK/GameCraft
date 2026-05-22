const int switchPin = 2;

int lastState = HIGH;

void setup() {
  pinMode(switchPin, INPUT_PULLUP);
  Serial.begin(9600);
}

void loop() {
  int state = digitalRead(switchPin);

  if (state != lastState) {
    delay(20); // チャタリング対策
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
}