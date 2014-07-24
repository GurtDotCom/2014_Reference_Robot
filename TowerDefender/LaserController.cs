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
        private float _x, _currentX = 150; //make this the default x position
        private float _y, _currentY = 100; //make this the default y position
        private int stepsPerPixelX = 2; //adjust this to correspond to calibration of X
        private int stepsPerPixelY = 2; //adjust this to correspond to calibration of Y
        private bool inverseX = true;
        private bool inverseY = false;
        private DateTime _lastFired = DateTime.MinValue;
        private bool _isFiring;
        //private float gain = 0.5f;
        private bool _updated;
    
        public void Connect(SerialPort port)
        {
            _port = port;
            port.Open();
        }

        public void Update(float deltax, float deltay, bool shouldFire)
        {
       /*     if (deltax > 0)
                _x += gain;
            if (deltax < 0)
                _x -= gain;

            if (deltay < 0)
                _y += gain;
            if (deltay > 0)
                _y -= gain;

            _x = Math.Min(Math.Max(_x, 0), 700); //range of hardware
            _y = Math.Min(Math.Max(_y, 0), 400); //range of hardware

    */
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
            
           _currentX = _x;
           _currentY = _y;
           
           if (inverseX)
           {
            _x = 640 - (deltax * stepsPerPixelX);
           } else {
            _x = deltax * stepsPerPixelX;   
           }
           
            if (inverseY)
           {
            _y = 480 - (deltay * stepsPerPixelY);
           } else {
            _y = deltay * stepsPerPixelY;   
           }
            
            
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
    }
}
