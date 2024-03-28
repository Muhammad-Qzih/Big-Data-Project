ThisBuild / version := "0.1.0-SNAPSHOT"

ThisBuild / scalaVersion := "2.12.18"


lazy val root = (project in file("."))
  .settings(
    name := "untitled2",
    libraryDependencies ++= Seq(
      "org.apache.spark" %% "spark-core" % "3.1.2",
      "org.apache.spark" %% "spark-sql" % "3.1.2",
      "org.scalanlp" %% "breeze" % "1.1",
      "org.apache.spark" %% "spark-mllib" % "3.1.2" % "provided",
      "org.apache.spark" %% "spark-streaming" % "3.1.2",
      "org.apache.kafka" % "kafka-clients" % "3.1.2",
      "org.apache.spark" %% "spark-streaming-kafka-0-10" % "3.1.2",
      "org.mongodb.spark" %% "mongo-spark-connector" % "3.0.1",
      "org.mongodb.scala" %% "mongo-scala-driver" % "2.9.0",
      "org.mongodb" % "mongo-java-driver" % "3.12.10"
    )
  )

