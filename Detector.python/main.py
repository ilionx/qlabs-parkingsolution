# default imports
import os, sys
import cv2

# ConfigParser for camera and server settings
import configparser

# Homemade classes
import lib.spots as spots
from lib.camera import Camera
from lib.parkinglot import ParkingLot
from lib.backendnotifier import BackendNotifier

# Main class for running purposes (with UI)
class Main:
	
	config_filename = 'config'
	cam = None
	parking_lot = None
	backend_notifier = None
	
	def __init__(self):
	
		# read config file
		config = configparser.ConfigParser()
		config.read(self.config_filename)
		refresh_in_seconds = 1
		
		# exit on unavailable config file
		if len(config) == 1:
			sys.exit("Config file not found or empty, please create ./%s" % (self.config_filename))
		
		# exit if sections not present
		if 'default' not in config.sections():
			sys.exit("Default settings not found")

		refresh_in_seconds = int(config['default']['refresh_in_seconds'])
		# exit if sections not present
		if 'camera' not in config.sections():
			sys.exit("Camera settings not found")
		
		self.cam = Camera(config['camera'])
	
		# exit if sections not present
		if 'backend' not in config.sections():
			sys.exit("Backend settings not found")
		self.backend_notifier = BackendNotifier(config['backend'])

		# exit if sections not present
		if 'parking_lot' not in config.sections():
			sys.exit("Parking lot settings not found")

		self.parking_lot = ParkingLot(config['parking_lot'], self.cam)

		# write any changes to config
		with open(self.config_filename, 'w') as configfile:
			config.write(configfile)
		
		# Execute Main Loop
		self.execute(refresh_in_seconds)

	
	def execute(self, refresh_in_seconds):
		self.cam.start()
		frame_num = -1
		while(True):
			check, frame = self.cam.getNextFrame()
			frame_num += 1
			if(check and (frame_num % (self.cam.fps*refresh_in_seconds) == 0 or frame_num == 0)):
				spots.find(frame, self.parking_lot)
				frame_small = cv2.resize(frame, (1280,720))
				cv2.imshow('Free spots marked with green (press q to close)',frame_small)
				if (frame_num % (self.cam.fps*2) == 0):
					self.backend_notifier.notifyAll(self.parking_lot.parking_spots)
					frame_num = 0
			if(cv2.waitKey(1) & 0xFF == ord('q')):
				break
		self.cam.stop()


# Start program
Main()