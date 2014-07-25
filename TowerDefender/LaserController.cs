using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TowerDefender
{
    public class LaserController
    {
        private SerialPort _port;
        private float _x = 150; //make this the default x position
        private float _y = 100; //make this the default y position

        private DateTime _lastFired = DateTime.MinValue;
        private bool _isFiring;
        private float gain = 0.5f;
        private bool _updated;
    
        public void Connect(SerialPort port)
        {
            _port = port;
            port.Open();
        }

        public void Update(float deltax, float deltay, bool shouldFire)
        {
            
             if ((deltax < 15) && (deltax > -15))  { xgain = .15f; } else { xgain = 2f; }
            if ((deltay < 15) && (deltay > -15))  { ygain = .15f; } else { ygain = 2f; }
            
            if (deltax > 0)
                _x += gain;
            if (deltax < 0)
                _x -= gain;

            if (deltay < 0)
                _y += gain;
            if (deltay > 0)
                _y -= gain;

            _x = Math.Min(Math.Max(_x, 0), 350); //range of hardware
            _y = Math.Min(Math.Max(_y, 0), 200); //range of hardware

    //laser timing is handled by the arduino for reliability
    
            /*
            if (shouldFire && !_isFiring && (DateTime.Now - _lastFired).TotalMilliseconds > 2000)
            {
                _isFiring = true;
                _lastFired = DateTime.Now;
            }

            if (_isFiring && (DateTime.Now - _lastFired).TotalMilliseconds > 500)
            {
                _isFiring = false;
            }
            */
            
        if (_updated){
            
            _isFiring = true;
            var text = String.Format("{0:000},{1:000},{2}\r\n", _x, _y, _isFiring ? 1 : 0);
            //Thread.Sleep(100);
            _port.Write(text);
            _updated = false;
        }

            var resp = _port.ReadExisting();
            if (resp == "updated"){ //I'm expecting a response of "updated" from the arduino
             _updated = true;
               
            }
        }
        
        
        
        
        public void Hunt()  //Cant find any targets, so hunt for them
        {
            
       gain = 2f;
       bool panPlus = true;
       bool tiltPlus = true;
        
            if ((_x - gain)  < 0) //set value to the minimum pan search value
               panPlus = true;
               
            if ((_x + gain) > 350) //set value to the maximum pan search value
                panPlus = false;

            if ((_y - gain) < 0) //set value to the minimum tilt search value
                tiltPlus = true;
                
            if ((_y + gain) > 100) //set value to the maximum tilt search value
                tiltlus = false;
              
              
                if (panPlus) {
                 _x += gain;   
                } else {
                 _x -= gain;     
                }
                
                if (tiltPlus) {
                 _y += gain;   
                } else {
                 _y -= gain;     
                }

            _x = Math.Min(Math.Max(_x, 0), 700); //range of hardware
            _y = Math.Min(Math.Max(_y, 0), 400); //range of hardware

    

        if (_updated){
          
            _isFiring = false;
            var text = String.Format("{0:000},{1:000},{2}\r\n", _x, _y, _isFiring ? 1 : 0);
            //Thread.Sleep(100);
            _port.Write(text);
        }

            var resp = _port.ReadExisting();
            if (resp == "updated"){ //I'm expecting a response of "updated" from the arduino
             _updated = true;
               
            }
        }
    }
}
