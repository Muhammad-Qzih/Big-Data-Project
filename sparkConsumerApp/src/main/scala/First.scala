
import org.apache.kafka.common.serialization.StringDeserializer
import org.apache.log4j.varia.NullAppender
import org.apache.log4j.{BasicConfigurator, Level, Logger}
import org.apache.spark.SparkConf
import org.apache.spark.sql.functions.{col, regexp_extract, regexp_replace, to_date, to_timestamp, udf}
import org.apache.spark.sql.{DataFrame, Row, SparkSession}
import org.apache.spark.streaming.{Seconds, StreamingContext}
import org.apache.spark.streaming.kafka010.{ConsumerStrategies, KafkaUtils, LocationStrategies}

object First {
  def main(args: Array[String]): Unit = {

    // extract the  id from the string
    def getIdValue(id: String): String = {
      id.replaceAll("\\{'id': '(\\d+)'", "$1").replace(" ", "")
    }

    // extract the  date from the string
    def getDateValue(date: String): String = {
      date.replaceAll("'date': '(.*?)'", "$1").trim();
    }

    // extract the  user from the string
    def getUserName(user: String): String = {
      user.replaceAll("'user': '(\\w+)'", "$1").trim();
    }

    // extract the text from the string
    def getTextValue(text: String): String = {
      text.replaceAll("'text': '|'}$", "").trim();
    }

    // connect with mongodb
    def connectToDatabase(dataFrame: DataFrame): Unit = {
      dataFrame.write
        .format("mongo")
        .mode("append")
        .option("uri", "mongodb://localhost:27017/project.tweets")
        .save()
    }

    // get tuple with contains clean values
    def getTuple(record: Array[String]): (Int,
      String,
      String,
      String,
      Long) = {
      (
        getIdValue(record(0)).toInt,
        getDateValue(record(1)),
        getUserName(record(2)),
        getTextValue(record(3)),
        System.currentTimeMillis()
      )
    }


    val nullAppender = new NullAppender
    BasicConfigurator.configure(nullAppender)

    val conf = new SparkConf().setMaster("local[2]").setAppName("KafkaConsumer")
    val ssc = new StreamingContext(conf, Seconds(1))
    val spark = SparkSession.builder().config(conf).getOrCreate()


    val kafkaParams = Map[String, Object](
      "bootstrap.servers" -> "localhost:9092",
      "key.deserializer" -> classOf[StringDeserializer],
      "value.deserializer" -> classOf[StringDeserializer],
      "group.id" -> "use_a_separate_group_id_for_each_stream",
      "auto.offset.reset" -> "latest",
      "enable.auto.commit" -> (false: java.lang.Boolean),
    )


    val stream = KafkaUtils.createDirectStream[String, String](
      ssc,
      LocationStrategies.PreferConsistent,
      ConsumerStrategies.Subscribe[String, String](Array("muhammad1"), kafkaParams)
    )

    import spark.implicits._

    stream.foreachRDD { rdd =>

      if (!rdd.isEmpty()) {
        rdd.foreach(println)
        val rddOfTuples = rdd.map(line => {
          val record = line.value.split(",");
          getTuple(record)

        })

        spark.conf.set("spark.sql.legacy.timeParserPolicy", "LEGACY")

        // convert the rdd of tuples to dataframe and convert the date from string to date
        val df = rddOfTuples.toDF("id", "date", "user", "text", "producedTweet")
          .withColumn("date", to_timestamp(col("date"), "EEE MMM dd HH:mm:ss zzz yyyy"))

        df.show(5)

        println("--------------------------------")
        connectToDatabase(df);
      }
    }
    ssc.start()
    ssc.awaitTermination()
  }
}
