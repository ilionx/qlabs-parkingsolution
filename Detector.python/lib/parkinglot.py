import cv2
from parkinglotspot import ParkingLotSpot
from camera import Camera

class ParkingLot(object):
    template_regions=[]
    parking_spots=[]

    def __init__(self, config, camera):
        if(config['configured'] != "True"):
            self.configure(camera)
            # Set to empty to clean
            config['template_regions'] = ''
            for tr in self.template_regions:
                config['template_regions'] += '\n{0},{1},{2},{3}'.format(tr[0],tr[1],tr[2],tr[3])

            # Set to empty to clean
            config['parking_spots'] = ''
            for ps in self.parking_spots:
                config['parking_spots'] += '\n{0},{1},{2},{3},{4},{5},{6}'.format(ps.spot_id,ps.top_left[0],ps.top_left[1],ps.bottom_right[0],ps.bottom_right[1],ps.car_width, "QNH")

            # Now it's configures
            config['configured'] = "True"
        else:
            # First load template region
            template_region_strings = config['template_regions'].split('\n')
            for trs in template_region_strings:
                # Ignore empty rows
                if (trs == ''):
                    continue
                tr = trs.split(',')
                self.template_regions.append([int(tr[0]), int(tr[1]), int(tr[2]), int(tr[3])])

            # Second load parking spots
            parking_spot_strings = config['parking_spots'].split('\n')
            for pss in parking_spot_strings:
                # Ignore empty rows
                if (pss == ''):
                    continue
                ps = pss.split(',')                
                self.parking_spots.append(ParkingLotSpot(int(ps[0]),(int(ps[1]),int(ps[2])),(int(ps[3]),int(ps[4])),int(ps[5]),ps[6]))
                # self.parking_spots[-1].location = config['location']

    def configure(self, camera):
        print('Configuration starting..')
        frame = camera.getSingleFrame()

        print('Select the reference region')
        template_regions = cv2.selectROIs('Select the reference region(s), enter to confirm spot, esc to close', frame)
        for template_region in template_regions:
            self.template_regions.append(template_region)
        cv2.destroyAllWindows()

        print('Select spots')
        regions = cv2.selectROIs('Select spots, enter to confirm spot, esc to close', frame)
        spot_id = 1 # Spots start at 1
        for region in regions:
            self.parking_spots.append(ParkingLotSpot(spot_id, (region[0], region[1]), (region[0]+region[2], region[1]+region[3]), region[2]/3))
            spot_id += 1
        cv2.destroyAllWindows()
