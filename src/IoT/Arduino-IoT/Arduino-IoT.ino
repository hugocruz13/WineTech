#include "DHT.h"

#define DHTPIN 2      
#define DHTTYPE DHT11  
#define LDRpin A0     

DHT dht(DHTPIN, DHTTYPE);

void setup() {
  Serial.begin(9600);
  dht.begin();
}

void loop() {
  float temp = dht.readTemperature();
  float hum = dht.readHumidity();
  int ldrValue = analogRead(LDRpin);

  if (isnan(temp) || isnan(hum)) {
    Serial.println("Erro na leitura do DHT11");
  } else {
    Serial.println(String(temp, 1) + "," + String(hum, 1) + "," + String(ldrValue));
  }

  delay(1000); 
}