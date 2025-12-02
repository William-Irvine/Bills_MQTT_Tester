import paho.mqtt.client as mqtt
import datetime
import psycopg2
import json

#database details
# Create your database and replace the following with appropriate values
DB_HOST = "localhost"
DB_NAME = "test_database_1"
DB_USER = "postgres"
DB_PASSWORD = "password1!"



#MQTT Broker connection details 
# Create your broker and replace the following with appropriate values
broker_user = "username"
broker_password = "password2!"
broker_address = "localhost" #Or the ip address of broker
port = 1883
topic = "data/car"

def on_connect(client, userdata, flags, rc):
    print(f"Connected to MQTT Broker with result code {rc}")
    client.subscribe(topic)

#Callback function when a message is received
def on_message(client, userdata, msg):
    try:   
        if msg.topic == "data/car":
            payload = json.loads(msg.payload.decode('utf-8'))
            data_to_insert = (payload['brand'], payload['model'], payload['year'])
        
            conn = psycopg2.connect(host=DB_HOST, database=DB_NAME, user=DB_USER, password=DB_PASSWORD)
            cur = conn.cursor()
        
            insert_query = "INSERT INTO cars (brand, model, year) VALUES (%s, %s, %s);"
            cur.execute(insert_query, data_to_insert)
            conn.commit()
        
            cur.close()
            conn.close()
            print(f"Data saved to PostgreSQL: {payload}")
        
    except Exception as e:
        print(f"Error processing message or saving to DB: {e}")
        
        
        
#create mqtt client instance
client = mqtt.Client("RaspberryPiSubscriber")

client.on_connect = on_connect

#assign callback function
client.on_message = on_message

#set username and password
client.username_pw_set(broker_user, broker_password)

#conect to broker
client.connect(broker_address, port, 60)
    
#subscribe to topic
#client.subscribe(topic)

#start the loop to process messages
print(f"Subscribed to topic '{topic}'. Waiting for messages...")
client.loop_forever()

