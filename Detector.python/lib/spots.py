import cv2
import numpy as np
from matplotlib import pyplot as plt
from parkinglot import ParkingLot
from parkinglotspot import ParkingLotSpot

############################ BEGIN: TWEAKING PARAMETERS ###########################################
threshold = 0.96   #used to determine if a spot is empty
font = cv2.FONT_HERSHEY_TRIPLEX
############################### END: TWEAKING PARAMETERS ###########################################


def find(image, parking_lot):
    # creates a template, its just a car sized patch of pavement
    # sample: template = image[width1:width2", height1:height2]
    # template = image[200:280,640:690]
    res = []
    for tr in parking_lot.template_regions:
        template = image[tr[1]:tr[1]+tr[3], tr[0]:tr[0]+tr[2]]
        m, n, chan = image.shape

        #blurs the template a bit
        template = cv2.GaussianBlur(template,(3,3),2)
        h, w, chan = template.shape

        # Apply template Matching 
        r = cv2.matchTemplate(image,template,cv2.TM_CCORR_NORMED)
        res.append(r)
        min_val, max_val, min_loc, max_loc = cv2.minMaxLoc(r)
        top_left = max_loc
        bottom_right = (top_left[0] + w, top_left[1] + h)
        # Draw reference rectangle to screen
        # cv2.rectangle(image, top_left, bottom_right, (0,0,255), 5)

    for ps in parking_lot.parking_spots:
        # Reset to default of not-empty
        prev_state = ps.empty
        ps.empty = False
        # Draw spot rectangle + number to screen
        cv2.rectangle(image,  ps.top_left,  ps.bottom_right, (200,200,200), 5)
        cv2.putText(image, '{0}'.format(ps.spot_id), (ps.top_left[0], ps.top_left[1]+27), font, 1, (100,100,100))

        # Creates the region of interest
        tl =  ps.top_left
        br =  ps.bottom_right
        
        for r in res:
            if(ps.empty):
                break
            my_roi = r[tl[1]:br[1],tl[0]:br[0]]

            # Extracts statistics by column
            ps.col_mean = np.mean(my_roi, 0)
            ps.inverted_variance = 1 - np.var(my_roi,0)
            ps.empty_col_probability =  ps.col_mean *  ps.inverted_variance

            # Check if space is empty
            num_consec_pixels_over_threshold = 0
            curr_col = 0

            for prob_val in ps.empty_col_probability:
                curr_col += 1

                if(prob_val > threshold):
                    num_consec_pixels_over_threshold += 1
                else:
                    num_consec_pixels_over_threshold = 0

                if (num_consec_pixels_over_threshold >=  ps.car_width):
                    ps.empty = True
                    center_x =  ps.top_left[0] + ((ps.bottom_right[0] - ps.top_left[0])/2)
                    center_y =  ps.top_left[1] + ((ps.bottom_right[1] - ps.top_left[1])/2)
                    cv2.circle(image, (center_x, center_y), 5, (0,255,0), 20)
                    # only one car per place, can break now
                    break