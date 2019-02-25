class ParkingLotSpot(object):
    spot_id=None
    top_left=None
    bottom_right=None
    car_width=None
    roi=None
    col_mean=None
    inverted_variance=None
    empty_col_probability=None
    empty=False
    location=None

    def __init__(self, spot_id, top_left, bottom_right, car_width, location):
        self.spot_id = spot_id
        self.top_left = top_left
        self.bottom_right = bottom_right
        self.car_width = car_width
        self.location = location