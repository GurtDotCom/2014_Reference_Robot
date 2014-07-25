

#define X_STEP_PIN 54
#define X_DIR_PIN 55
#define X_ENABLE_PIN 38
#define X_MIN_PIN 2

#define Y_STEP_PIN 60
#define Y_DIR_PIN 61
#define Y_ENABLE_PIN 56
#define Y_MIN_PIN 3


int fricken_laser = 53;
int curPan, curTilt, panDiff, tiltDiff = 0;
unsigned long lastFire;
int seekPos;

// for printf
int my_putc(char c, FILE *t)
{
    Serial.write(c);
}

void setup()
{
    fdevopen(&my_putc, 0);  
    Serial.begin(115200);
    Serial.setTimeout(1000);
    pinMode(fricken_laser, OUTPUT);
    digitalWrite(fricken_laser, HIGH);
    
pinMode(X_STEP_PIN , OUTPUT);
pinMode(X_DIR_PIN , OUTPUT);
pinMode(X_ENABLE_PIN , OUTPUT);
pinMode(X_MIN_PIN , INPUT);

pinMode(Y_STEP_PIN , OUTPUT);
pinMode(Y_DIR_PIN , OUTPUT);
pinMode(Y_ENABLE_PIN , OUTPUT);
pinMode(Y_MIN_PIN , INPUT);

digitalWrite(X_ENABLE_PIN , LOW);
digitalWrite(Y_ENABLE_PIN , LOW);

Zeroize();

}

void loop()
{
  
       if((millis() - lastFire) >= 500){
        digitalWrite(fricken_laser, HIGH);
        } 
    char buf[10];
    int num_read = 0;
    memset(buf,0,sizeof(buf));
    num_read = Serial.readBytesUntil('\n',buf,10);
    if (num_read == 10)
    {
        int pan = 0;
        int tilt = 0;
        int laser_on = 0;
        sscanf(buf,"%d,%d,%d\n",&pan,&tilt,&laser_on);
        
        
 if (pan < curPan){
digitalWrite(X_DIR_PIN , HIGH);  
panDiff = curPan - pan;
} 
if (pan > curPan){
digitalWrite(X_DIR_PIN , LOW); 
panDiff = pan - curPan;
}


if (tilt < curTilt){
digitalWrite(Y_DIR_PIN , HIGH); 
tiltDiff = curTilt - tilt;
} 
if (tilt > curTilt){
digitalWrite(Y_DIR_PIN , LOW);  
tiltDiff = tilt - curTilt;
}     


while ((panDiff != 0) && (tiltDiff !=0)){
digitalWrite(X_STEP_PIN , LOW);
digitalWrite(X_STEP_PIN , HIGH);
panDiff = panDiff - 1;
digitalWrite(Y_STEP_PIN , LOW);
digitalWrite(Y_STEP_PIN , HIGH);
tiltDiff = tiltDiff - 1;
//delay(1);  
delayMicroseconds(999);
//__asm__("nop\n\t"); 
}

if (pan != curPan){
for (int run = 0; run < panDiff; run++) {
digitalWrite(X_STEP_PIN , LOW);
digitalWrite(X_STEP_PIN , HIGH);
//delay(1);  
delayMicroseconds(999);
//__asm__("nop\n\t"); 
      }
}
if (tilt != curTilt){
for (int run = 0; run < tiltDiff; run++) {
digitalWrite(Y_STEP_PIN , LOW);
digitalWrite(Y_STEP_PIN , HIGH);
//delay(1);  
delayMicroseconds(999);
//__asm__("nop\n\t"); 
      }
}
        
        
        if ((laser_on) && ((millis()-lastFire) >= 2000)){
        digitalWrite(fricken_laser, LOW);
        lastFire = millis();    
        }
    
    
        printf("updated");
        curPan = pan;
        curTilt = tilt;
    }
    
       if((millis() - lastFire) >= 500){
        digitalWrite(fricken_laser, HIGH);
        } 

    
}

// samples datas
// 100,100,1

// 150,100,0


void Zeroize(){
  
  seekPos = 1;
digitalWrite(X_DIR_PIN , LOW);  
digitalWrite(Y_DIR_PIN , LOW);
  
for (int run = 0; run < 50; run++) {
digitalWrite(X_STEP_PIN , LOW);
digitalWrite(X_STEP_PIN , HIGH);
digitalWrite(Y_STEP_PIN , LOW);
digitalWrite(Y_STEP_PIN , HIGH);
delay(3);  

      }
  
      
digitalWrite(X_DIR_PIN , HIGH);  
digitalWrite(Y_DIR_PIN , HIGH);

  while (digitalRead(X_MIN_PIN) == LOW ) {
digitalWrite(X_STEP_PIN , LOW);
digitalWrite(X_STEP_PIN , HIGH);
delay(8);  

}  

while (digitalRead(Y_MIN_PIN) == LOW ) {
digitalWrite(Y_STEP_PIN , LOW);
digitalWrite(Y_STEP_PIN , HIGH);
delay(8);
} 

digitalWrite(X_DIR_PIN , LOW);  
digitalWrite(Y_DIR_PIN , LOW);


for (int run = 0; run < 200; run++) {
digitalWrite(X_STEP_PIN , LOW);
digitalWrite(X_STEP_PIN , HIGH);
digitalWrite(Y_STEP_PIN , LOW);
digitalWrite(Y_STEP_PIN , HIGH);
//delay(3);
delayMicroseconds(700);
//__asm__("nop\n\t"); 
      }
      
      
for (int run = 0; run < 150; run++) {
digitalWrite(X_STEP_PIN , LOW);
digitalWrite(X_STEP_PIN , HIGH);
//delay(3);
delayMicroseconds(700);
//__asm__("nop\n\t"); 

}
curPan = 350;
curTilt = 200;
      
      
 
  
}





