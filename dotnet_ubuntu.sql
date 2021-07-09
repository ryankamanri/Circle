-- MySQL dump 10.13  Distrib 8.0.25, for Win64 (x86_64)
--
-- Host: localhost    Database: dotnet_ubuntu
-- ------------------------------------------------------
-- Server version	8.0.25

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES gbk */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `posts`
--

DROP TABLE IF EXISTS `posts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `posts` (
  `ID` bigint NOT NULL AUTO_INCREMENT,
  `post` mediumtext,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `postID` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `posts`
--

LOCK TABLES `posts` WRITE;
/*!40000 ALTER TABLE `posts` DISABLE KEYS */;
INSERT INTO `posts` VALUES (1,'第一个帖子'),(2,'第二个帖子'),(3,'第三个帖子'),(4,'4'),(5,'无了无了');
/*!40000 ALTER TABLE `posts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `posts_tags`
--

DROP TABLE IF EXISTS `posts_tags`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `posts_tags` (
  `posts` bigint NOT NULL,
  `tags` bigint NOT NULL,
  `relations` varchar(255) NOT NULL,
  PRIMARY KEY (`posts`,`tags`),
  KEY `PT_FK_P` (`posts`),
  KEY `PT_FK_T` (`tags`),
  CONSTRAINT `posts_tags_ibfk_1` FOREIGN KEY (`posts`) REFERENCES `posts` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `posts_tags_ibfk_2` FOREIGN KEY (`tags`) REFERENCES `tags` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `posts_tags`
--

LOCK TABLES `posts_tags` WRITE;
/*!40000 ALTER TABLE `posts_tags` DISABLE KEYS */;
INSERT INTO `posts_tags` VALUES (1,2,''),(2,3,''),(3,1,''),(3,4,'');
/*!40000 ALTER TABLE `posts_tags` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tags`
--

DROP TABLE IF EXISTS `tags`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tags` (
  `ID` bigint NOT NULL AUTO_INCREMENT,
  `tag` char(10) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `tagID` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=47 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tags`
--

LOCK TABLES `tags` WRITE;
/*!40000 ALTER TABLE `tags` DISABLE KEYS */;
INSERT INTO `tags` VALUES (-1,'任何根节点的父节点'),(1,'第一个标签'),(2,'计算机'),(3,'考研'),(4,'哲学'),(5,'计算机科学技术'),(6,'网络工程'),(7,'软件工程'),(9,'电路原理'),(10,'模拟电子技术'),(11,'人工智能'),(12,'网络管理与维护'),(13,'汇编语言'),(14,'路由与交换技术'),(15,'需求分析与系统设计'),(16,'数据库'),(17,'程序设计语言'),(18,'电路原理图'),(19,'动态电路'),(20,'三极管'),(21,'半导体'),(22,'场效应'),(23,'大数据'),(24,'深度学习'),(25,'VLAN间路由'),(26,'生成树协议'),(27,'端口聚合'),(28,'存储器系统'),(29,'中断系统'),(30,'需求文档'),(31,'数据流图'),(32,'用例图'),(33,'SQL server'),(34,'Oracle'),(35,'Java'),(36,'C++'),(37,'Python'),(38,'Linux命令'),(39,'线性代数'),(40,'概率论'),(41,'用例'),(42,'参与者'),(43,'E-R图'),(44,'SQL语句'),(45,'数据库设计');
/*!40000 ALTER TABLE `tags` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tags_tags`
--

DROP TABLE IF EXISTS `tags_tags`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tags_tags` (
  `tags_1` bigint NOT NULL,
  `tags_2` bigint NOT NULL,
  `relations` varchar(255) NOT NULL,
  PRIMARY KEY (`tags_1`,`tags_2`),
  KEY `tags_2` (`tags_2`),
  CONSTRAINT `tags_tags_ibfk_1` FOREIGN KEY (`tags_1`) REFERENCES `tags` (`ID`),
  CONSTRAINT `tags_tags_ibfk_2` FOREIGN KEY (`tags_2`) REFERENCES `tags` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tags_tags`
--

LOCK TABLES `tags_tags` WRITE;
/*!40000 ALTER TABLE `tags_tags` DISABLE KEYS */;
INSERT INTO `tags_tags` VALUES (2,-1,''),(5,2,''),(6,2,''),(7,2,''),(9,5,''),(10,5,''),(11,5,''),(12,6,''),(13,6,''),(14,6,''),(15,7,''),(16,7,''),(17,7,''),(18,9,''),(19,9,''),(20,10,''),(21,10,''),(22,10,''),(23,11,''),(24,11,''),(25,12,''),(26,12,''),(27,12,''),(28,13,''),(29,13,''),(30,15,''),(31,15,''),(32,15,''),(33,16,''),(34,16,''),(35,17,''),(36,17,''),(37,17,''),(38,23,''),(39,24,''),(40,24,''),(41,32,''),(42,32,''),(43,33,''),(44,33,''),(45,34,'');
/*!40000 ALTER TABLE `tags_tags` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `ID` bigint NOT NULL AUTO_INCREMENT,
  `Password` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `Account` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `userID` (`ID`),
  UNIQUE KEY `Account_Unique` (`Account`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'123456','974481066@qq.com'),(2,'654321','qq.974481066.qq@gmail.com'),(3,'456789','2168359585@qq.com'),(4,'262425','2624250238@qq.com'),(5,'123456789','853909407@qq.com');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users_posts`
--

DROP TABLE IF EXISTS `users_posts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users_posts` (
  `users` bigint NOT NULL,
  `posts` bigint NOT NULL,
  `relations` varchar(255) NOT NULL,
  PRIMARY KEY (`users`,`posts`),
  KEY `UP_FK_P` (`posts`),
  KEY `UP_FK_U` (`users`),
  CONSTRAINT `users_posts_ibfk_1` FOREIGN KEY (`posts`) REFERENCES `posts` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `users_posts_ibfk_2` FOREIGN KEY (`users`) REFERENCES `users` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users_posts`
--

LOCK TABLES `users_posts` WRITE;
/*!40000 ALTER TABLE `users_posts` DISABLE KEYS */;
INSERT INTO `users_posts` VALUES (1,5,'{\"t1\":1}'),(2,3,'');
/*!40000 ALTER TABLE `users_posts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users_tags`
--

DROP TABLE IF EXISTS `users_tags`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users_tags` (
  `users` bigint NOT NULL,
  `tags` bigint NOT NULL,
  `relations` varchar(255) NOT NULL,
  PRIMARY KEY (`users`,`tags`),
  KEY `UT_FK_T` (`tags`),
  KEY `UT_FK_U` (`users`),
  CONSTRAINT `users_tags_ibfk_1` FOREIGN KEY (`tags`) REFERENCES `tags` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `users_tags_ibfk_2` FOREIGN KEY (`users`) REFERENCES `users` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users_tags`
--

LOCK TABLES `users_tags` WRITE;
/*!40000 ALTER TABLE `users_tags` DISABLE KEYS */;
INSERT INTO `users_tags` VALUES (1,1,''),(1,2,''),(1,3,''),(2,1,''),(2,4,'');
/*!40000 ALTER TABLE `users_tags` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-07-09 17:43:07
