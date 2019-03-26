# QNH - Parktimer

The parktimer is a python application specially created for racing purposes.

To see this application in action click on the following Youtube link: [demonstration](https://youtu.be/7e_Cq-uBEMg).

### Game manual

The course is simpel, you have to drive through three gates.
The first gate will start the timer, second gate will flash the xenon light and the last gate will stop the timer and print the label with the driven time on it.

Below is a picture of our stand at the TU in Eindhoven, three circles are placed around the gates.

![](../docs/images/Parktimer_stand.jpeg)

### Wiring

![](../docs/images/Parktimer_wiring.png)

### How does it work?

```
GPIO.setwarnings(False)
GPIO.setmode(GPIO.BOARD)

IR_BREAK_START = 11
GPIO.setup(IR_BREAK_START, GPIO.IN)

IR_BREAK_FLASH = 13
GPIO.setup(IR_BREAK_FLASH, GPIO.IN)

IR_BREAK_STOP = 15
GPIO.setup(IR_BREAK_STOP, GPIO.IN)

FLASH_LIGHT = 40
GPIO.setup(FLASH_LIGHT, GPIO.OUT)
```

This piece of code is for setting up the GPIO pins. The IR-breaks are `GPIO.IN` because you want to read the input. The flashlight is `GPIO.OUT` because we want to trigger the flashlight. `11, 13, 15 and 40` are the pin numbers we used, these numbers correspond with `GPIO17, GPIO27, GPIO22 and GPIO21`.

```
display = SevenSegmentWidget()
display.set_fixed_decimal(True)
```

We declare the display so we are able to write the time to it.

```
font_type = ImageFont.truetype("arial.ttf", 46)
color = (0,0,0)
```

The font type we use to write the driven time on the label is Arial. And the color of that text is black.

```
conn = cups.Connection()
printer_name = "DYMO_LabelWriter_450"
```

We make a connection with cups and declare the printer name so we can use it later on to print the label.

```
while True:
    IR_BREAK_START_INPUT = GPIO.input(IR_BREAK_START)

    if(IR_BREAK_START_INPUT == 0):
        start()
```

The code starts here. It keeps reading the `IR_BREAK_START_INPUT` of the start gate untill the input is equal to `0`, if it reads `0` it will fire the method start(). The input is `1` when nothing interrupts the IR Break.

```
while True:
    IR_BREAK_FLASH_INPUT = GPIO.input(IR_BREAK_FLASH)
    IR_BREAK_STOP_INPUT = GPIO.input(IR_BREAK_STOP)

    if(IR_BREAK_FLASH_INPUT == 0):
        GPIO.output(FLASH_LIGHT, GPIO.HIGH)
    else:
        GPIO.output(FLASH_LIGHT, GPIO.LOW)
```

In the start method is a while loop where we read the input of the IR Break of the flashlight gate and the stop gate. First we check if the flashlight gate is interrupted, when it is interrupted we send a `GPIO.HIGH` to the flashlight, which will flash the flashlight. When it isn't interrupted we send `GPIO.LOW` to the flaslight to turn it off.

```
display.set_value(parktimer.elapsed() + "")
```

The elapsed time will be written to the display

```
if(IR_BREAK_STOP_INPUT == 0):
    display.set_value(parktimer.stop() + "")

    timeElapsed = parktimer.elapsed()

    m = int(timeElapsed.split(".", 1)[0][1:3])
    s = int(timeElapsed.split(".", 1)[0][3:])
    ms = timeElapsed.split(".", 1)[1][:2]

    if (m > 0):
        timeValue = "Too slow"
    else:
        timeValue = str(s) + ":" + str(ms)

    image = Image.open("/home/pi/label.png")
    draw = ImageDraw.Draw(image)

    draw.text(xy=(815,141), text=timeValue, fill=color, font=font_type)

    timeLabelPath = "/home/pi/timeLabel.png"
    image.save(timeLabelPath, "PNG")
    conn.printFile(printer_name, timeLabelPath, "Time label print", {})
    time.sleep(0.2)
    break
```

Next we check if the IR break of the stop gate is interrupted. If so, we stop the timer and declare the minutes, seconds and milliseconds from the elapsed time. When it took longer than a minute, the timeValue is `Too slow`, otherwise the driven time will be the timeValue which looks like `s:ms`. Next we open the image and declare a draw variable so we can 'draw' text on it with the method `draw.text()`. We used the following parameters:

- `xy` are the coordinates where we draw the text on the label.
- `text` is the text we will draw onto the label.
- `fill` is the color of the text
- `font` which is the font-type of the text.

Then we save the image we drew the text on and print that image. We break out the while loop and return to checking the start gate so we can start again.

### Hardware requirements

- Raspberry Pi
- Dymo Labelwriter 450
- 1.2" 7-segment LED HT16K33 Backpack ([information](https://www.adafruit.com/product/1270))
- Three IR Break Beam Sensors ([information](https://www.adafruit.com/product/2168))
- Three 10K resistors ([information](https://www.adafruit.com/product/2784))
- Xenon Remoted Strobe Flash Fligh Light ([information](https://alexnld.com/product/xenon-remoted-strobe-flash-flight-light-with-bright-led-navigation-lights-for-fpv-racing/?gclid=Cj0KCQiA5NPjBRDDARIsAM9X1GJSL8wIRPIScwYFA2MiGTdIRPDuz0uGO0BjQvmzhfD8X3ETlRSgHoAaAkU9EALw_wcB))

### Installation

    $ pip install -r requirements.txt

This command installs the Adafruit-LED-Backpack library needed for the 1.2" 7-segment LED HT16K33 Backpack.

Next we need to install CUPS (Common Unix Printing System) so we can use the Dymo Labelwriter as a printer on the Raspberry Pi.

    $ sudo apt-get update
    $ sudo apt-get install cups
    $ sudo usermod -a -G lpadmin pi

If you want remote access you'll have to make a few changes in the config file, to do so run the command:

    $ sudo nano /etc/cups/cupsd.conf

First change the first lines to the following:

```
# Only listen for connections from the local machine
# Listen localhost:631
Port 631
```

Scroll further down in the config file until you see the “location” sections. Add the lines with <b>ADD THIS LINE</b> behind them.

```
< Location / >

# Restrict access to the server...

Order allow,deny
Allow @local                                        ADD THIS LINE
< /Location >

< Location /admin >

# Restrict access to the admin pages...

Order allow,deny
Allow @local                                        ADD THIS LINE
< /Location >

< Location /admin/conf >
AuthType Default
Require user @SYSTEM

# Restrict access to the configuration files...

Order allow,deny
Allow @local                                        ADD THIS LINE
< /Location >
```

Restart CUPS by issuing the following:

    $ sudo /etc/init.d/cups restart

You should now be able to access the print server on any local computer by typing http://{raspberry pi's IP address}:631

Next we will need to install the cups libraries and other necessary libraries to build

    $ sudo apt-get install libcups2-dev libcupsimage2-dev g++ cups

Now download the Dymo SDK and install the drivers

    $ wget http://download.dymo.com/Download%20Drivers/Linux/Download/dymo-cups-drivers-1.4.0.tar.gz
    $ tar xvf dymo-cups-drivers-1.4.0.tar.gz
    $ git clone https://aur.archlinux.org/dymo-cups-drivers.git

Copy the files inside the dymo-cups-drivers folder into the dymo-cups-drivers-1.4.0 folder

    $ cd dymo-cups-drivers-1.4.0.5/
    $ patch -Np1 -i cups-ppd-header.patch
    $ sudo ./configure
    $ sudo make
    $ sudo make install

Now you can plug the Dymo Labelwriter into a USB port on the Raspberry Pi and go to localhost:631 or remote to http://{raspberry pi's IP address}:631.

Then click on add printer on the admin page and follow the steps on the page and after it is added you're done with the installation.

### Configuration

You have to configure to things:

- Label image
- Printer name

Change the path in the line of code in the Python script: `image = Image.open("/home/pi/label.png")`, so it will match your label.
Next, change the x and y coordinates so the time is written at the place of your desire. You can test it with: `image.show()`.

At last you have to change the variable `printer_name` with the name you configured the printer, in our case it is "DYMO_LabelWriter_450".

Our label looks like this:
![](../docs/images/label.png)

### Running

    $ python ./parktimer.py

### Resources

| Plugin   | Website             |
| -------- | ------------------- |
| Pip      | [pypi.org][pip]     |
| CUPS     | [cups.org][cups]    |
| Dymo SDK | [dymo.com][dymosdk] |

[pip]: https://pypi.org/project/pip/
[cups]: https://www.cups.org/
[dymosdk]: http://www.dymo.com/nl-NL/dymo-label-sdk-and-cups-drivers-for-linux-dymo-label-sdk-cups-linux-p--1
