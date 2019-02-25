import cv2

# This class does all communication to the camera
class Camera:
    
    url = None
    cap = None
    fps = 1
    
    # fed with a config section
    def __init__(self, config):
        
        # build a nice URL
        self.url = "rtsp://%s:%s@%s:%s/%s" % (config['username'], config['password'], config['ip'], 
            config['port'], config['url_suffix'])
        self.fps = int(config['fps'])

    def getSingleFrame(self):
        self.cap = cv2.VideoCapture(self.url)
        check, frame = self.cap.read()
        self.cap.release()
        return frame

    def start(self):
        self.cap = cv2.VideoCapture(self.url)
    
    def stop(self):
        self.cap.release()

    def getNextFrame(self):
        return self.cap.read()