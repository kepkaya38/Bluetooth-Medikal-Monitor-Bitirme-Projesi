#define USE_ARDUINO_INTERRUPTS true    
#include <SoftwareSerial.h>
SoftwareSerial btSerial(0, 1); // RX, TX

void setup() {   

  Serial.begin(9600);          // Haberleşmeyi başlatır
  btSerial.begin(9600); //Bluetooth u başlatır
  
}



void loop() {
 int val = analogRead(A0);  //Ekg verisini çağırır

 Serial.print("$"); 
 Serial.print("#");         
 Serial.print(val); //EKG Verisi gösterir
 Serial.println("#");
 delay(1);                    


}


  
