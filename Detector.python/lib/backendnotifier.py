# Json format for backend:
# {
# 	"spotId": 1,
# 	"location": "p1",
# 	"available": true,
# 	"metadata": {}
# }

import json
from requests_futures.sessions import FuturesSession

class BackendNotifier:

    url = None
    enabled = False

    def __init__(self, config):
        self.url = '{0}?code={1}'.format(config['url'], config['api_key'])
        self.enabled = config['enabled'] == 'True'

    # def bg_resp(self, sess, resp):
    #     # Handle the callback from finished request


    def notify(self, session, requests, jsonSpots):
        headers = {'Content-Type': 'application/json'}
        r = session.post(self.url, data=jsonSpots, timeout=1, headers=headers)
        # requests.append(r)
        # print('Post response code: {0}, {1}, {2}'.format(r.status_code, spot.spot_id, r))
    
    def notifyAll(self, spots):
        if (not self.enabled):
            return
        session = FuturesSession(max_workers=10)
        jsonSpots=[]
        requests = []
        for spot in spots:
            jsonSpots.append({
                "spotId": spot.spot_id,
                "location": spot.location,
                "available": spot.empty,
                "metadata": {}
            })
        self.notify(session, requests, json.dumps(jsonSpots))