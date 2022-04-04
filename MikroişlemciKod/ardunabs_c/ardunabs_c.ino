#define USE_ARDUINO_INTERRUPTS true    

#include <OneWire.h> // OneWire kütüphanesini ekliyoruz.
#include <SoftwareSerial.h>
SoftwareSerial btSerial(0, 1); // RX, TX
int DS18S20_Pin = 2; 
OneWire ds(DS18S20_Pin);

#include <PulseSensorPlayground.h>     // Nabız Sensörü Kütüphanesi   

const int PulseWire = 1;       
const int LED13 = 13;          
int Threshold = 550;           


                            
PulseSensorPlayground pulseSensor;  // Nabızı tanımlıyoruz

void setup() {   

  Serial.begin(9600);          // Haberleşmeyi başlatır
  btSerial.begin(9600); //Bluetooth u başlatır
  pulseSensor.analogInput(PulseWire);   
  pulseSensor.blinkOnPulse(LED13);       
  pulseSensor.setThreshold(Threshold);   
  pulseSensor.begin();
}



void loop() {

 int myBPM = pulseSensor.getBeatsPerMinute();  // Nabız Değerini Çağırır
 float temperature = getTemp(); // Sıcaklık Değerini Çağırır

 Serial.print("$"); 
 Serial.print("#");                             
 Serial.print(myBPM); // Nabız değerini gösterir
 Serial.print("#");
 Serial.print(temperature); // Sıcaklık değerini gösterir
 Serial.println("#");
 delay(1000);                    


}

// Sıcaklık Hesabı 
float getTemp(){
  //returns the temperature from one DS18S20 in DEG Celsius

  byte data[12];
  byte addr[8];

  if ( !ds.search(addr)) {
      //no more sensors on chain, reset search
      ds.reset_search();
      return -1000;
  }

  if ( OneWire::crc8( addr, 7) != addr[7]) {
      Serial.println("CRC is not valid!");
      return -1000;
  }

  if ( addr[0] != 0x10 && addr[0] != 0x28) {
      Serial.print("Device is not recognized");
      return -1000;
  }

  ds.reset();
  ds.select(addr);
  ds.write(0x44,1); // start conversion, with parasite power on at the end

  byte present = ds.reset();
  ds.select(addr);    
  ds.write(0xBE); // Read Scratchpad

  for (int i = 0; i < 9; i++) { // we need 9 bytes
    data[i] = ds.read();
  }

  ds.reset_search();

  byte MSB = data[1];
  byte LSB = data[0];

  float tempRead = ((MSB << 8) | LSB); //using two's compliment
  float TemperatureSum = tempRead / 16;

  return TemperatureSum;

}
  
