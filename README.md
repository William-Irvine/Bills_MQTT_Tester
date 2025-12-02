# Bills_MQTT_Tester
Simple .NET MAUI app for Android to connect to an MQTT Broker and write a JSON message to a topic using MQTTnet. 

I built this app using .NET MAUI and MQTTnet so that I could test functionality of saving data submitted via the app to a Postgres Database through an MQTT Broker running an a Raspberry Pi 4. 

My use case:

Raspberry Pi running mosquitto broker.

Python script running to ingest data coming over on topic data/car as a JSON message and save to appropriate columns in a PostgreSQL Database also deployed on the Raspberry Pi.

