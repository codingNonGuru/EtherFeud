import json
from websocket_server import WebsocketServer

host = '127.0.0.1'
port = 8787

server = WebsocketServer(port, host)

clients = []

matchedClients = []

newClients = []

def handleConnect(client, server):
    clients.append(client)

    hasFound = False
    for matchedClient in matchedClients:
        if matchedClient is client:
            hasFound = True
            break

    if hasFound is True:
        matchedClients.append(client)

    newClients.append(client)

    broadcastNewcomers()

server.set_fn_new_client(handleConnect)

def broadcastNewcomers():
    #for newClient in newClients:
    #    connectionEvent = {}
    #    connectionEvent['eventName'] = 'playerJoined'
    #    connectionEvent['eventData'] = {'id' : 21, 'positionX' : 10, 'positionZ' : 5}

        #for client in clients:
    #        message = json.dumps(connectionEvent)
            #print message
            #print client
            #server.send_message(client, message)

        #newClients.remove(newClient)

    eventObject = {'key' : "0x578", 'positionX' : 10, 'positionZ' : 5}

    eventData = '[playerJoined, ' + json.dumps(eventObject) + ']'

    server.send_message_to_all(eventData)

server.run_forever()