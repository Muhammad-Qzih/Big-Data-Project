import csv
import re
import time
from confluent_kafka import Producer

# generate producer instance from kafka  and pass configuration to this instance
def produce_message(tweet_list):
    producer = Producer({'bootstrap.servers': 'localhost:9092'})
    for tweet in tweet_list:
        producer.produce('muhammad1', value = str(tweet))

    producer.flush()

    

tweet_list = []
index= 0

# read data from csv file
with open('training.1600000.processed.noemoticon.csv', 'r') as csvfile:
    reader = csv.reader(csvfile)
    for row in reader:
        if index < 200:
            tweet = {   
            "id": row[1],
            "date": row[2],
            "user": row[4],
            "text": re.sub(r"'text': '|'", '', row[5]),
            "retweets": int(time.time() * 1000)
            }
            tweet_list.append(tweet)
        else:
            index = 0
            produce_message(tweet_list)
            tweets = [] 
            time.sleep(1)
       
       
        index += 1






