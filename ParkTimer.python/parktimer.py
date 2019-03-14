from Adafruit_LED_Backpack import AlphaNum4, BicolorBargraph24, SevenSegment

import RPi.GPIO as GPIO
import widget
import datetime
import time

class Timer(object):
    """A simple timer class"""
    
    def __init__(self):
        pass

    def start(self):
        """Starts the timer"""
        self.start = datetime.datetime.now()
        return self.start
    
    def stop(self, message="Total: "):
        """Stops the timer.  Returns the time elapsed"""
        self.stop = datetime.datetime.now()
        time = str(self.stop - self.start)
        time = time.replace(':', '')
        return time
    
    def elapsed(self, message="Elapsed: "):
        """Time elapsed since start was called"""
        time = str(datetime.datetime.now() - self.start)
        #  0:00:00.000000 FORMAT
        time = time.replace(':', '')
        return time

class SevenSegmentWidget(widget.Widget):
    """Seven segment LED backpack widget.  Can display simple numeric values
    in the range of -999 to 9999.
    """

    def __init__(self, address='0x70', brightness='15', decimal_digits='2',
                 justify_right='True', invert='False'):
        """Create an instance of the seven segment display widget.  Can pass in
        the following optional parameters to control the widget (note all
        parameters are strings as they are parsed from config files):
          - address: I2C address, default is 0x70
          - brightness: Brightness of the display, can be a value from 0 to 15
                        with 15 being the brightest.  The default is 15.
          - decimal_digits: Number of digits to show after decimal point, default
                            is 0.
          - justify_right: Justify numeric display to the right side if true (the
                           default).  Set to false to justify to the left side.
          - invert: Vertically flip the display if true.  Default is false.  Note
                    that when flipped the decimal points will be at the top!
        """
        # Setup display and initial state.
        self._display = SevenSegment.SevenSegment(address=int(address, 0))
        self._display.begin()
        self._display.set_brightness(int(brightness))
        if self.parse_bool(invert):
            self._display.set_invert(True)
        self._decimal_digits = int(decimal_digits)
        self._justify_right = self.parse_bool(justify_right)
        # Clear the display
        self._display.clear()
        self._display.write_display()
    
    def set_fixed_decimal(self, value):
        self._display.set_fixed_decimal(value)
        self._display.write_display()

    def set_value(self, value):
        value = value.lower()
        self._display.clear()
        value = float(value)
        self._display.print_float(value, decimal_digits=self._decimal_digits,
                                    justify_right=self._justify_right)
        # self._display.set_fixed_decimal(value)
        self._display.write_display()
        


GPIO.setwarnings(False)
GPIO.setmode(GPIO.BOARD)

#GPIO.setup(18, GPIO.IN, pull_up_down=GPIO.PUD_UP)

IR_BREAK_START = 11
GPIO.setup(IR_BREAK_START, GPIO.IN)

IR_BREAK_FLASH = 13
GPIO.setup(IR_BREAK_FLASH, GPIO.IN)

IR_BREAK_STOP = 15
GPIO.setup(IR_BREAK_STOP, GPIO.IN)

FLASH_LIGHT = 40
GPIO.setup(FLASH_LIGHT, GPIO.OUT)

display = SevenSegmentWidget()

# parktimer = Timer()
# # parktimer.start()

display.set_fixed_decimal(True)


def start():
    #display.set_value("3")
    #time.sleep(1)
    #display.set_value("2")
    #time.sleep(1)
    #display.set_value("1")
    #time.sleep(1)

    parktimer = Timer()
    parktimer.start()
    
    LAST_LIGHT_STATUS = 0

    while True:
        #button_state = GPIO.input(18)
        IR_BREAK_FLASH_INPUT = GPIO.input(IR_BREAK_FLASH)
        IR_BREAK_STOP_INPUT = GPIO.input(IR_BREAK_STOP)
        
        if(IR_BREAK_FLASH_INPUT == 0 and LAST_LIGHT_STATUS == 1):
            GPIO.output(FLASH_LIGHT, GPIO.HIGH)
            LAST_LIGHT_STATUS = 0
        else:
            if(LAST_LIGHT_STATUS == 0):
                GPIO.output(FLASH_LIGHT, GPIO.LOW)
                LAST_LIGHT_STATUS = 1
            
            
        if(IR_BREAK_STOP_INPUT == 0):
           display.set_value(parktimer.stop() + "")
           time.sleep(0.2)
           break
        
        display.set_value(parktimer.elapsed() + "")



while True:
    #button_state = GPIO.input(18)
    
    IR_BREAK_START_INPUT = GPIO.input(IR_BREAK_START)


    if(IR_BREAK_START_INPUT == 0):
        start()
