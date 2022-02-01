CREATE DATABASE  IF NOT EXISTS `dotnet_ubuntu` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `dotnet_ubuntu`;
-- MySQL dump 10.13  Distrib 8.0.25, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: dotnet_ubuntu
-- ------------------------------------------------------
-- Server version	8.0.25

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `circles`
--

DROP TABLE IF EXISTS `circles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `circles` (
  `ID` bigint NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `circles`
--

LOCK TABLES `circles` WRITE;
/*!40000 ALTER TABLE `circles` DISABLE KEYS */;
/*!40000 ALTER TABLE `circles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `circles_users`
--

DROP TABLE IF EXISTS `circles_users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `circles_users` (
  `ID` bigint NOT NULL AUTO_INCREMENT,
  `circles` bigint NOT NULL,
  `users` bigint NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `circles` (`circles`),
  KEY `users` (`users`),
  CONSTRAINT `circles_users_ibfk_1` FOREIGN KEY (`circles`) REFERENCES `circles` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `circles_users_ibfk_2` FOREIGN KEY (`users`) REFERENCES `users` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `circles_users`
--

LOCK TABLES `circles_users` WRITE;
/*!40000 ALTER TABLE `circles_users` DISABLE KEYS */;
/*!40000 ALTER TABLE `circles_users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `messages`
--

DROP TABLE IF EXISTS `messages`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `messages` (
  `ID` bigint NOT NULL AUTO_INCREMENT,
  `SendUserID` bigint NOT NULL,
  `ReceiveID` bigint NOT NULL,
  `IsGroup` tinyint(1) NOT NULL,
  `Time` datetime NOT NULL,
  `ContentType` varchar(255) NOT NULL,
  `Content` mediumblob NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `SendUserID` (`SendUserID`),
  KEY `ReceiveID` (`ReceiveID`),
  CONSTRAINT `messages_ibfk_1` FOREIGN KEY (`SendUserID`) REFERENCES `users` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `messages_ibfk_2` FOREIGN KEY (`ReceiveID`) REFERENCES `users` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=70 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `messages`
--

LOCK TABLES `messages` WRITE;
/*!40000 ALTER TABLE `messages` DISABLE KEYS */;
INSERT INTO `messages` VALUES (4,2,8,0,'2021-10-19 16:38:48','text/plain',_binary 'Hello, World'),(5,2,8,0,'2021-10-19 16:38:51','text/plain',_binary 'Hello, World'),(6,2,8,0,'2021-10-19 16:38:54','text/plain',_binary 'Hello, World'),(7,2,8,0,'2021-10-19 19:04:11','text/plain',_binary 'Hello, World'),(8,2,8,0,'2021-10-19 19:04:14','text/plain',_binary 'Hello, World'),(9,2,8,0,'2021-10-19 19:04:17','text/plain',_binary 'Hello, World'),(10,2,8,0,'2021-10-19 19:51:19','text/plain',_binary 'Hello, World'),(11,2,8,0,'2021-10-19 19:51:22','text/plain',_binary 'Hello, World'),(12,2,8,0,'2021-10-19 19:51:25','text/plain',_binary 'Hello, World'),(13,2,8,0,'2021-10-19 20:01:43','text/plain',_binary 'Hello, World'),(14,2,8,0,'2021-10-19 20:01:46','text/plain',_binary 'Hello, World'),(15,2,8,0,'2021-10-19 20:01:49','text/plain',_binary 'Hello, World'),(16,2,8,0,'2021-10-19 20:08:58','text/plain',_binary 'Hello, World'),(17,2,8,0,'2021-10-19 20:09:01','text/plain',_binary 'Hello, World'),(18,2,8,0,'2021-10-19 20:09:04','text/plain',_binary 'Hello, World'),(19,2,8,0,'2021-10-20 15:47:12','text/plain',_binary 'Hello, World'),(20,2,8,0,'2021-10-20 15:47:15','text/plain',_binary 'Hello, World'),(21,2,8,0,'2021-10-20 15:47:18','text/plain',_binary 'Hello, World'),(22,2,8,0,'2021-10-21 10:38:30','text/plain',_binary 'Hello, World'),(23,2,8,0,'2021-10-21 10:38:33','text/plain',_binary 'Hello, World'),(24,2,8,0,'2021-10-21 10:38:36','text/plain',_binary 'Hello, World'),(25,2,8,0,'2021-10-21 10:40:11','text/plain',_binary 'Hello, World'),(26,2,8,0,'2021-10-21 10:40:14','text/plain',_binary 'Hello, World'),(27,2,8,0,'2021-10-21 10:40:17','text/plain',_binary 'Hello, World'),(28,2,8,0,'2021-11-14 11:54:57','text/plain',_binary 'Hello, World'),(29,2,8,0,'2021-11-14 11:55:00','text/plain',_binary 'Hello, World'),(30,2,8,0,'2021-11-14 11:55:03','text/plain',_binary 'Hello, World'),(31,2,8,0,'2021-11-14 18:19:28','text/plain',_binary 'Hello, World'),(32,2,8,0,'2021-11-14 18:19:31','text/plain',_binary 'Hello, World'),(33,2,8,0,'2021-11-14 18:19:34','text/plain',_binary 'Hello, World'),(34,2,8,0,'2021-11-16 17:30:13','text/plain',_binary 'Hello, World'),(35,2,8,0,'2021-11-16 17:30:16','text/plain',_binary 'Hello, World'),(36,2,8,0,'2021-11-16 17:30:19','text/plain',_binary 'Hello, World'),(37,2,8,0,'2021-11-16 18:21:45','text/plain',_binary 'Hello, World'),(38,2,8,0,'2021-11-16 18:21:48','text/plain',_binary 'Hello, World'),(39,2,8,0,'2021-11-16 18:21:51','text/plain',_binary 'Hello, World'),(40,2,8,0,'2021-11-16 19:14:12','text/plain',_binary 'Hello, World'),(41,2,8,0,'2021-11-16 19:14:15','text/plain',_binary 'Hello, World'),(42,2,8,0,'2021-11-16 19:14:18','text/plain',_binary 'Hello, World'),(43,2,8,0,'2021-11-16 19:36:26','text/plain',_binary 'Hello, World'),(44,2,8,0,'2021-11-16 19:36:29','text/plain',_binary 'Hello, World'),(45,2,8,0,'2021-11-16 19:36:32','text/plain',_binary 'Hello, World'),(46,2,8,0,'2021-11-16 20:04:53','text/plain',_binary 'Hello, World'),(47,2,8,0,'2021-11-16 20:04:56','text/plain',_binary 'Hello, World'),(48,2,8,0,'2021-11-16 20:04:59','text/plain',_binary 'Hello, World'),(49,2,8,0,'2021-11-16 20:12:48','text/plain',_binary 'Hello, World'),(50,2,8,0,'2021-11-16 20:12:51','text/plain',_binary 'Hello, World'),(51,2,8,0,'2021-11-16 20:12:54','text/plain',_binary 'Hello, World'),(52,2,8,0,'2021-11-16 20:13:44','text/plain',_binary 'Hello, World'),(53,2,8,0,'2021-11-16 20:13:47','text/plain',_binary 'Hello, World'),(54,2,8,0,'2021-11-16 20:13:50','text/plain',_binary 'Hello, World'),(55,2,8,0,'2021-11-16 20:14:21','text/plain',_binary 'Hello, World'),(56,2,8,0,'2021-11-19 15:34:33','text/plain',_binary 'Hello, World'),(57,2,8,0,'2021-11-19 15:34:36','text/plain',_binary 'Hello, World'),(58,2,8,0,'2021-11-19 15:34:39','text/plain',_binary 'Hello, World'),(59,2,8,0,'2021-11-19 16:06:58','text/plain',_binary 'Hello, World'),(60,2,8,0,'2021-11-19 16:07:01','text/plain',_binary 'Hello, World'),(61,2,8,0,'2021-11-19 16:07:04','text/plain',_binary 'Hello, World'),(62,2,8,0,'2021-11-19 16:12:20','text/plain',_binary 'Hello, World'),(63,2,8,0,'2021-11-19 16:28:35','text/plain',_binary 'Hello, World'),(64,2,8,0,'2021-11-19 16:28:38','text/plain',_binary 'Hello, World'),(65,2,8,0,'2021-11-19 16:32:43','text/plain',_binary 'Hello, World'),(66,2,8,0,'2021-11-19 16:37:42','text/plain',_binary 'Hello, World'),(67,2,8,0,'2021-11-19 16:47:31','text/plain',_binary 'Hello, World'),(68,2,8,0,'2021-11-19 16:50:35','text/plain',_binary 'Hello, World'),(69,2,8,0,'2021-11-19 16:52:35','text/plain',_binary 'Hello, World');
/*!40000 ALTER TABLE `messages` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `posts`
--

DROP TABLE IF EXISTS `posts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `posts` (
  `ID` bigint NOT NULL AUTO_INCREMENT,
  `Title` varchar(255) NOT NULL,
  `Summary` varchar(1023) NOT NULL,
  `Focus` varchar(255) NOT NULL,
  `PostDateTime` varchar(255) NOT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `postID` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=32 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `posts`
--

LOCK TABLES `posts` WRITE;
/*!40000 ALTER TABLE `posts` DISABLE KEYS */;
INSERT INTO `posts` VALUES (6,'第一个帖子','委托的精髓 --- 以变量作为方法(函数)使用,类似于C中函数指针',' C# ','2019/1/1 0:00:00'),(16,'grsgsvbfbrdbgr','tehbergerththngbfdbgnerhbrgrgrgrdbgnnuyi','sgbrdges','2021/7/29 21:38:40'),(17,'我的GitHub','我的GitHub\n\n我的GitHub\n我的GitHub\n我的GitHub\n\n我的','GitHub','2021/7/29 21:49:47'),(18,'123123','123123123123123','123123123','2021/7/29 21:51:51'),(19,'45335278','3485345343483456486486483','737352786786','2021/7/29 21:53:47'),(20,'啦啦啦','啊啦啦啦啦啦啊啦啦啦啦啦\n\n哦不错','啦啦','2021/7/29 21:59:04'),(23,'GitHub使用','在编程届有个共识，想要成为一个合格的程序员必须要掌握 GitHub 的用法！\n\n','GitHub','2021/7/30 12:28:17'),(24,'HTML 5 全局 contenteditable 属性','定义和用法\n\ncontenteditable 属性规定是否可编辑元素的内容。\n\n','HTML','2021/7/30 13:28:21'),(25,'\"坚持就是胜利\"','\"无须担心，明天早上牛奶还会放在门前的台阶上，《新政治家报》也会照常出版。\n——G\"','\"加油,奥利给!!!\"','2021/8/19 17:50:55'),(26,'\"坚持就是胜利\"','\"无须担心，明天早上牛奶还会放在门前的台阶上，《新政治家报》也会照常出版。\n——G\"','\"加油,奥利给!!!\"','2021/8/19 18:01:34'),(27,'\"坚持就是胜利\"','\"无须担心，明天早上牛奶还会放在门前的台阶上，《新政治家报》也会照常出版。\n——G\"','\"加油,奥利给!!!\"','2021/8/19 18:02:07'),(28,'\"坚持就是胜利\"','\"无须担心，明天早上牛奶还会放在门前的台阶上，《新政治家报》也会照常出版。\n——G\"','\"加油,奥利给!!!\"','2021/8/19 18:02:23'),(29,'\"坚持就是胜利\"','\"无须担心，明天早上牛奶还会放在门前的台阶上，《新政治家报》也会照常出版。\n——G\"','\"加油,奥利给!!!\"','2021/8/19 18:09:58'),(30,'\"好久不见\"','\"（仅代表个人观点，不喜勿喷，并无嘲笑和讽刺的意味，如果有哪里语言不当，或有言论冒\"','\"甚是想念\"','2021/8/19 18:22:38'),(31,'!!!!!','!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!','!!!!','2021/8/19 18:46:46');
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
INSERT INTO `posts_tags` VALUES (6,2,''),(6,47,''),(16,2,'{}'),(17,2,'{}'),(18,3,'{}'),(19,1,'{}'),(20,1,'{}'),(20,3,'{}'),(24,7,'{}'),(29,4,'{}'),(30,1,'{}'),(31,6,'{\"Type\":\"Owned\"}');
/*!40000 ALTER TABLE `posts_tags` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `postsinfo`
--

DROP TABLE IF EXISTS `postsinfo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `postsinfo` (
  `ID` bigint NOT NULL,
  `Content` mediumtext NOT NULL,
  KEY `ID` (`ID`),
  CONSTRAINT `postsinfo_ibfk_1` FOREIGN KEY (`ID`) REFERENCES `posts` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `postsinfo`
--

LOCK TABLES `postsinfo` WRITE;
/*!40000 ALTER TABLE `postsinfo` DISABLE KEYS */;
INSERT INTO `postsinfo` VALUES (6,'<div id=\"context\">\n    \n      <div class=\"mume markdown-preview  margin-center col-md-8\">\n      <h4 class=\"mume-header\" id=\"%E5%A7%94%E6%89%98\">委托</h4>\n\n<p><strong>委托的精髓 --- 以变量作为方法(函数)使用,类似于C中函数指针</strong></p>\n<ul>\n<li>此处函数与方法意义相同,统称为<strong>方法</strong></li>\n<li>此处\"C\"指 <strong>C/C++语言</strong></li>\n</ul>\n\n<p>这里先定义一个命名空间,里面有两个类</p>\n<pre data-role=\"codeBlock\" data-info=\"cs\" class=\"language-csharp\"><span class=\"token keyword\">namespace</span> <span class=\"token namespace\">ConsoleApp2</span>\n<span class=\"token punctuation\">{</span>\n    <span class=\"token keyword\">class</span> <span class=\"token class-name\">Program</span>\n    <span class=\"token punctuation\">{</span>\n        <span class=\"token keyword\">public</span> <span class=\"token keyword\">static</span> <span class=\"token return-type class-name\"><span class=\"token keyword\">void</span></span> <span class=\"token function\">Main</span><span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">string</span><span class=\"token punctuation\">[</span><span class=\"token punctuation\">]</span></span> args<span class=\"token punctuation\">)</span>\n        <span class=\"token punctuation\">{</span>\n           \n        <span class=\"token punctuation\">}</span>\n        \n    <span class=\"token punctuation\">}</span>\n    <span class=\"token keyword\">class</span> <span class=\"token class-name\">B</span>\n    <span class=\"token punctuation\">{</span>\n        <span class=\"token keyword\">public</span> <span class=\"token keyword\">static</span> <span class=\"token return-type class-name\"><span class=\"token keyword\">int</span></span> <span class=\"token function\">Comparor</span><span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">int</span></span> x<span class=\"token punctuation\">,</span> <span class=\"token class-name\"><span class=\"token keyword\">int</span></span> y<span class=\"token punctuation\">)</span>\n        <span class=\"token punctuation\">{</span>\n            <span class=\"token keyword\">return</span> x <span class=\"token operator\">-</span> y<span class=\"token punctuation\">;</span>\n        <span class=\"token punctuation\">}</span>\n    <span class=\"token punctuation\">}</span>\n<span class=\"token punctuation\">}</span>\n</pre><p>现在要在<code>Program</code>类里使用<code>B</code>类里的<code>Comparor</code>方法,该怎么办呢</p>\n<ol>\n<li>可以直接引用</li>\n</ol>\n<pre data-role=\"codeBlock\" data-info=\"cs\" class=\"language-csharp\"><span class=\"token keyword\">namespace</span> <span class=\"token namespace\">ConsoleApp2</span>\n<span class=\"token punctuation\">{</span>\n    <span class=\"token keyword\">class</span> <span class=\"token class-name\">Program</span>\n    <span class=\"token punctuation\">{</span>\n        <span class=\"token keyword\">public</span> <span class=\"token keyword\">static</span> <span class=\"token return-type class-name\"><span class=\"token keyword\">void</span></span> <span class=\"token function\">Main</span><span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">string</span><span class=\"token punctuation\">[</span><span class=\"token punctuation\">]</span></span> args<span class=\"token punctuation\">)</span>\n        <span class=\"token punctuation\">{</span>\n           Console<span class=\"token punctuation\">.</span><span class=\"token function\">WriteLine</span><span class=\"token punctuation\">(</span>B<span class=\"token punctuation\">.</span><span class=\"token function\">Comparor</span><span class=\"token punctuation\">(</span><span class=\"token number\">1</span><span class=\"token punctuation\">,</span><span class=\"token number\">2</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n        <span class=\"token punctuation\">}</span>\n        \n    <span class=\"token punctuation\">}</span>\n    <span class=\"token keyword\">class</span> <span class=\"token class-name\">B</span>\n    <span class=\"token punctuation\">{</span>\n        <span class=\"token keyword\">public</span> <span class=\"token keyword\">static</span> <span class=\"token return-type class-name\"><span class=\"token keyword\">int</span></span> <span class=\"token function\">Comparor</span><span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">int</span></span> x<span class=\"token punctuation\">,</span> <span class=\"token class-name\"><span class=\"token keyword\">int</span></span> y<span class=\"token punctuation\">)</span>\n        <span class=\"token punctuation\">{</span>\n            <span class=\"token keyword\">return</span> x <span class=\"token operator\">-</span> y<span class=\"token punctuation\">;</span>\n        <span class=\"token punctuation\">}</span>\n    <span class=\"token punctuation\">}</span>\n<span class=\"token punctuation\">}</span>\n</pre><ol start=\"2\">\n<li>利用委托实例化一个变量(这个变量的类型后面再说)指向这个方法,再使用这个变量</li>\n</ol>\n<pre data-role=\"codeBlock\" data-info=\"cs\" class=\"language-csharp\"><span class=\"token keyword\">namespace</span> <span class=\"token namespace\">ConsoleApp2</span>\n<span class=\"token punctuation\">{</span>\n    <span class=\"token keyword\">public</span> <span class=\"token keyword\">delegate</span> <span class=\"token return-type class-name\"><span class=\"token keyword\">int</span></span> <span class=\"token function\">DelegateComparor</span><span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">int</span></span> x<span class=\"token punctuation\">,</span><span class=\"token class-name\"><span class=\"token keyword\">int</span></span> y<span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span><span class=\"token comment\">//在类外定义委托</span>\n    <span class=\"token keyword\">class</span> <span class=\"token class-name\">Program</span>\n    <span class=\"token punctuation\">{</span>\n        <span class=\"token keyword\">public</span> <span class=\"token keyword\">static</span> <span class=\"token return-type class-name\"><span class=\"token keyword\">void</span></span> <span class=\"token function\">Main</span><span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">string</span><span class=\"token punctuation\">[</span><span class=\"token punctuation\">]</span></span> args<span class=\"token punctuation\">)</span>\n        <span class=\"token punctuation\">{</span>\n            <span class=\"token class-name\">DelegateComparor</span> comparor <span class=\"token operator\">=</span> <span class=\"token keyword\">new</span> <span class=\"token constructor-invocation class-name\">DelegateComparor</span><span class=\"token punctuation\">(</span>B<span class=\"token punctuation\">.</span>Comparor<span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n            Console<span class=\"token punctuation\">.</span><span class=\"token function\">WriteLine</span><span class=\"token punctuation\">(</span><span class=\"token function\">comparor</span><span class=\"token punctuation\">(</span><span class=\"token number\">1</span><span class=\"token punctuation\">,</span><span class=\"token number\">2</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n        <span class=\"token punctuation\">}</span>\n        \n    <span class=\"token punctuation\">}</span>\n    <span class=\"token keyword\">class</span> <span class=\"token class-name\">B</span>\n    <span class=\"token punctuation\">{</span>\n        <span class=\"token keyword\">public</span> <span class=\"token keyword\">static</span> <span class=\"token return-type class-name\"><span class=\"token keyword\">int</span></span> <span class=\"token function\">Comparor</span><span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">int</span></span> x<span class=\"token punctuation\">,</span> <span class=\"token class-name\"><span class=\"token keyword\">int</span></span> y<span class=\"token punctuation\">)</span>\n        <span class=\"token punctuation\">{</span>\n            <span class=\"token keyword\">return</span> x <span class=\"token operator\">-</span> y<span class=\"token punctuation\">;</span>\n        <span class=\"token punctuation\">}</span>\n    <span class=\"token punctuation\">}</span>\n<span class=\"token punctuation\">}</span>\n</pre><p>这样做的好处是可以更加灵活的使用不同类里的同一类型方法.并且有利于统一规范.避免引用不当造成的异常.(具体见后面)</p>\n<h4 class=\"mume-header\" id=\"func%E4%B8%8Eaction\">Func&lt;&gt;与Action&lt;&gt;</h4>\n\n<p>还是刚才那个类</p>\n<pre data-role=\"codeBlock\" data-info=\"cs\" class=\"language-csharp\"><span class=\"token keyword\">namespace</span> <span class=\"token namespace\">ConsoleApp2</span>\n<span class=\"token punctuation\">{</span>\n    <span class=\"token keyword\">class</span> <span class=\"token class-name\">Program</span>\n    <span class=\"token punctuation\">{</span>\n        <span class=\"token keyword\">public</span> <span class=\"token keyword\">static</span> <span class=\"token return-type class-name\"><span class=\"token keyword\">void</span></span> <span class=\"token function\">Main</span><span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">string</span><span class=\"token punctuation\">[</span><span class=\"token punctuation\">]</span></span> args<span class=\"token punctuation\">)</span>\n        <span class=\"token punctuation\">{</span>\n           \n        <span class=\"token punctuation\">}</span>\n        \n    <span class=\"token punctuation\">}</span>\n    <span class=\"token keyword\">class</span> <span class=\"token class-name\">B</span>\n    <span class=\"token punctuation\">{</span>\n        <span class=\"token keyword\">public</span> <span class=\"token keyword\">static</span> <span class=\"token return-type class-name\"><span class=\"token keyword\">int</span></span> <span class=\"token function\">Comparor</span><span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">int</span></span> x<span class=\"token punctuation\">,</span> <span class=\"token class-name\"><span class=\"token keyword\">int</span></span> y<span class=\"token punctuation\">)</span>\n        <span class=\"token punctuation\">{</span>\n            <span class=\"token keyword\">return</span> x <span class=\"token operator\">-</span> y<span class=\"token punctuation\">;</span>\n        <span class=\"token punctuation\">}</span>\n    <span class=\"token punctuation\">}</span>\n<span class=\"token punctuation\">}</span>\n</pre><p>现在回到刚才那个问题,怎样在<code>Program</code>类里使用<code>B</code>类里的<code>Comparor</code>方法,除了委托之外?<br>\n在委托部分,我们发现委托出来指向方法的变量<code>comparor</code>的类型并未直接说明,只在委托定义中</p>\n<pre data-role=\"codeBlock\" data-info=\"cs\" class=\"language-csharp\"><span class=\"token keyword\">public</span> <span class=\"token keyword\">delegate</span> <span class=\"token return-type class-name\"><span class=\"token keyword\">int</span></span> <span class=\"token function\">DelegateComparor</span><span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">int</span></span> x<span class=\"token punctuation\">,</span><span class=\"token class-name\"><span class=\"token keyword\">int</span></span> y<span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n</pre><p>才可知这是一个有两个<code>int</code>参数,返回值为<code>int</code>的方法.</p>\n<p>那么,如果能够找到像C中一样指向该类型函数的指针(如<code>int (*p)(int,int)</code>)的类型,就可以直接定义这种类型的变量来赋值了.</p>\n<p>因此,C#中提供了<code>Func&lt;&gt;</code>和<code>Action&lt;&gt;</code>来表达这一\"函数指针\"类型</p>\n<p>实例:</p>\n<blockquote>\n<p>C# == C<br>\nAction == void (*)() <em>//无参数,无返回值</em><br>\nAction&lt;T1&gt; == void (*)(T1) <em>//有一个参数,无返回值</em><br>\nAction&lt;T1,T2&gt; == void (*)(T1,T2) <em>//有两个参数,无返回值</em><br>\nFunc&lt;T&gt; == T (*)() <em>//无参数,有唯一返回值</em><br>\nFunc&lt;T1,T&gt; == T (*)(T1) <em>//有一个参数,有一个返回值</em><br>\nFunc&lt;T1,T2,T&gt; == T (*)(T1,T2) <em>//有两个参数,有一个返回值</em></p>\n</blockquote>\n<p>因此,可以这样定义:</p>\n<pre data-role=\"codeBlock\" data-info=\"cs\" class=\"language-csharp\"><span class=\"token keyword\">namespace</span> <span class=\"token namespace\">ConsoleApp2</span>\n<span class=\"token punctuation\">{</span>\n    <span class=\"token keyword\">class</span> <span class=\"token class-name\">Program</span>\n    <span class=\"token punctuation\">{</span>\n        <span class=\"token keyword\">public</span> <span class=\"token keyword\">static</span> <span class=\"token return-type class-name\"><span class=\"token keyword\">void</span></span> <span class=\"token function\">Main</span><span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">string</span><span class=\"token punctuation\">[</span><span class=\"token punctuation\">]</span></span> args<span class=\"token punctuation\">)</span>\n        <span class=\"token punctuation\">{</span>\n           <span class=\"token class-name\">Func<span class=\"token punctuation\">&lt;</span><span class=\"token keyword\">int</span><span class=\"token punctuation\">,</span> <span class=\"token keyword\">int</span><span class=\"token punctuation\">,</span> <span class=\"token keyword\">int</span><span class=\"token punctuation\">&gt;</span></span> comparor <span class=\"token operator\">=</span> B<span class=\"token punctuation\">.</span>Comparor<span class=\"token punctuation\">;</span>\n           Console<span class=\"token punctuation\">.</span><span class=\"token function\">WriteLine</span><span class=\"token punctuation\">(</span><span class=\"token function\">comparor</span><span class=\"token punctuation\">(</span><span class=\"token number\">1</span><span class=\"token punctuation\">,</span><span class=\"token number\">2</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n        <span class=\"token punctuation\">}</span>\n        \n    <span class=\"token punctuation\">}</span>\n    <span class=\"token keyword\">class</span> <span class=\"token class-name\">B</span>\n    <span class=\"token punctuation\">{</span>\n        <span class=\"token keyword\">public</span> <span class=\"token keyword\">static</span> <span class=\"token return-type class-name\"><span class=\"token keyword\">int</span></span> <span class=\"token function\">Comparor</span><span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">int</span></span> x<span class=\"token punctuation\">,</span> <span class=\"token class-name\"><span class=\"token keyword\">int</span></span> y<span class=\"token punctuation\">)</span>\n        <span class=\"token punctuation\">{</span>\n            <span class=\"token keyword\">return</span> x <span class=\"token operator\">-</span> y<span class=\"token punctuation\">;</span>\n        <span class=\"token punctuation\">}</span>\n    <span class=\"token punctuation\">}</span>\n<span class=\"token punctuation\">}</span>\n</pre><p>毕.</p>\n<h4 class=\"mume-header\" id=\"%E4%BB%8E%E5%87%BD%E6%95%B0%E5%88%B0%E5%8C%BF%E5%90%8D%E5%87%BD%E6%95%B0%E5%86%8D%E5%88%B0lambda%E8%A1%A8%E8%BE%BE%E5%BC%8F\">从函数到匿名函数再到lambda表达式</h4>\n\n<p>ps. 看标题就知道这是个进化的过程</p>\n<h5 class=\"mume-header\" id=\"%E5%87%BD%E6%95%B0-%E5%88%9D%E7%BA%A7%E5%BD%A2%E6%80%81\">函数---初级形态</h5>\n\n<p>仍然是刚才那个类,定义一个List序列</p>\n<pre data-role=\"codeBlock\" data-info=\"cs\" class=\"language-csharp\"><span class=\"token keyword\">namespace</span> <span class=\"token namespace\">ConsoleApp2</span>\n<span class=\"token punctuation\">{</span>\n    <span class=\"token keyword\">class</span> <span class=\"token class-name\">Program</span>\n    <span class=\"token punctuation\">{</span>\n        <span class=\"token keyword\">public</span> <span class=\"token keyword\">static</span> <span class=\"token return-type class-name\"><span class=\"token keyword\">void</span></span> <span class=\"token function\">Main</span><span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">string</span><span class=\"token punctuation\">[</span><span class=\"token punctuation\">]</span></span> args<span class=\"token punctuation\">)</span>\n        <span class=\"token punctuation\">{</span>\n            <span class=\"token class-name\">List<span class=\"token punctuation\">&lt;</span><span class=\"token keyword\">int</span><span class=\"token punctuation\">&gt;</span></span> list <span class=\"token operator\">=</span> <span class=\"token keyword\">new</span> <span class=\"token constructor-invocation class-name\">List<span class=\"token punctuation\">&lt;</span><span class=\"token keyword\">int</span><span class=\"token punctuation\">&gt;</span></span><span class=\"token punctuation\">(</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n            \n            list<span class=\"token punctuation\">.</span><span class=\"token function\">Add</span><span class=\"token punctuation\">(</span><span class=\"token number\">2</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n            list<span class=\"token punctuation\">.</span><span class=\"token function\">Add</span><span class=\"token punctuation\">(</span><span class=\"token number\">1</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n            list<span class=\"token punctuation\">.</span><span class=\"token function\">Add</span><span class=\"token punctuation\">(</span><span class=\"token number\">3</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n        <span class=\"token punctuation\">}</span>\n        \n    <span class=\"token punctuation\">}</span>\n    <span class=\"token keyword\">class</span> <span class=\"token class-name\">B</span>\n    <span class=\"token punctuation\">{</span>\n        <span class=\"token keyword\">public</span> <span class=\"token keyword\">static</span> <span class=\"token return-type class-name\"><span class=\"token keyword\">int</span></span> <span class=\"token function\">Comparor</span><span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">int</span></span> x<span class=\"token punctuation\">,</span> <span class=\"token class-name\"><span class=\"token keyword\">int</span></span> y<span class=\"token punctuation\">)</span>\n        <span class=\"token punctuation\">{</span>\n            <span class=\"token keyword\">return</span> x <span class=\"token operator\">-</span> y<span class=\"token punctuation\">;</span>\n        <span class=\"token punctuation\">}</span>\n    <span class=\"token punctuation\">}</span>\n<span class=\"token punctuation\">}</span>\n</pre><p>假设现在要给这个list进行排序,该怎么规定是从大到小还是从小到大呢?</p>\n<p>这就可以通过那个<code>Comparor</code>方法来决定.</p>\n<p>输入序列中的任意要比较的两个元素<code>x</code>,<code>y</code>,如返回为负则按<code>x -&gt; y</code>排,否则按<code>y -&gt; x</code>排.(具体怎么比较由函数内部自行定义)</p>\n<p><code>List</code>类中的<code>Sort</code>方法接受一个<code>Comparison&lt;T&gt;</code>类型参数.而这个<code>Comparison&lt;T&gt;</code>,就是一个可且仅可接受<code>Func&lt;int, int, int&gt;</code>类型的委托.</p>\n<p>因此最初的写法,利用<code>Comparison&lt;T&gt;</code>委托:</p>\n<pre data-role=\"codeBlock\" data-info=\"cs\" class=\"language-csharp\"><span class=\"token keyword\">namespace</span> <span class=\"token namespace\">ConsoleApp2</span>\n<span class=\"token punctuation\">{</span>\n    <span class=\"token keyword\">class</span> <span class=\"token class-name\">Program</span>\n    <span class=\"token punctuation\">{</span>\n        <span class=\"token keyword\">public</span> <span class=\"token keyword\">static</span> <span class=\"token return-type class-name\"><span class=\"token keyword\">void</span></span> <span class=\"token function\">Main</span><span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">string</span><span class=\"token punctuation\">[</span><span class=\"token punctuation\">]</span></span> args<span class=\"token punctuation\">)</span>\n        <span class=\"token punctuation\">{</span>\n            <span class=\"token class-name\">List<span class=\"token punctuation\">&lt;</span><span class=\"token keyword\">int</span><span class=\"token punctuation\">&gt;</span></span> list <span class=\"token operator\">=</span> <span class=\"token keyword\">new</span> <span class=\"token constructor-invocation class-name\">List<span class=\"token punctuation\">&lt;</span><span class=\"token keyword\">int</span><span class=\"token punctuation\">&gt;</span></span><span class=\"token punctuation\">(</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n            \n            list<span class=\"token punctuation\">.</span><span class=\"token function\">Add</span><span class=\"token punctuation\">(</span><span class=\"token number\">2</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n            list<span class=\"token punctuation\">.</span><span class=\"token function\">Add</span><span class=\"token punctuation\">(</span><span class=\"token number\">1</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n            list<span class=\"token punctuation\">.</span><span class=\"token function\">Add</span><span class=\"token punctuation\">(</span><span class=\"token number\">3</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n            <span class=\"token class-name\">Comparison<span class=\"token punctuation\">&lt;</span><span class=\"token keyword\">int</span><span class=\"token punctuation\">&gt;</span></span> comparison <span class=\"token operator\">=</span> <span class=\"token keyword\">new</span> <span class=\"token constructor-invocation class-name\">Comparison<span class=\"token punctuation\">&lt;</span><span class=\"token keyword\">int</span><span class=\"token punctuation\">&gt;</span></span><span class=\"token punctuation\">(</span>B<span class=\"token punctuation\">.</span>Comparor<span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n            list<span class=\"token punctuation\">.</span><span class=\"token function\">Sort</span><span class=\"token punctuation\">(</span>comparison<span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n        <span class=\"token punctuation\">}</span>\n        \n    <span class=\"token punctuation\">}</span>\n    <span class=\"token keyword\">class</span> <span class=\"token class-name\">B</span>\n    <span class=\"token punctuation\">{</span>\n        <span class=\"token keyword\">public</span> <span class=\"token keyword\">static</span> <span class=\"token return-type class-name\"><span class=\"token keyword\">int</span></span> <span class=\"token function\">Comparor</span><span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">int</span></span> x<span class=\"token punctuation\">,</span> <span class=\"token class-name\"><span class=\"token keyword\">int</span></span> y<span class=\"token punctuation\">)</span>\n        <span class=\"token punctuation\">{</span>\n            <span class=\"token keyword\">return</span> x <span class=\"token operator\">-</span> y<span class=\"token punctuation\">;</span>\n        <span class=\"token punctuation\">}</span>\n    <span class=\"token punctuation\">}</span>\n<span class=\"token punctuation\">}</span>\n</pre><p>当然也可以采用直接引用法:</p>\n<pre data-role=\"codeBlock\" data-info=\"cs\" class=\"language-csharp\">list<span class=\"token punctuation\">.</span><span class=\"token function\">Sort</span><span class=\"token punctuation\">(</span>B<span class=\"token punctuation\">.</span>Comparor<span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n</pre><p>不过这样做的弊端也很明显: 如果这个<code>B.Comparor</code>的类型不为<code>Func&lt;int,int,int&gt;</code> 就会引发异常,也不利于规范.委托的好处就在于<strong>给指向函数的变量做出规范</strong>.</p>\n<p>最后,这里不妨简化,去掉定义的<code>Comparison&lt;T&gt;</code>委托类型变量.</p>\n<pre data-role=\"codeBlock\" data-info=\"cs\" class=\"language-csharp\"><span class=\"token keyword\">namespace</span> <span class=\"token namespace\">ConsoleApp2</span>\n<span class=\"token punctuation\">{</span>\n    <span class=\"token keyword\">class</span> <span class=\"token class-name\">Program</span>\n    <span class=\"token punctuation\">{</span>\n        <span class=\"token keyword\">public</span> <span class=\"token keyword\">static</span> <span class=\"token return-type class-name\"><span class=\"token keyword\">void</span></span> <span class=\"token function\">Main</span><span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">string</span><span class=\"token punctuation\">[</span><span class=\"token punctuation\">]</span></span> args<span class=\"token punctuation\">)</span>\n        <span class=\"token punctuation\">{</span>\n            <span class=\"token class-name\">List<span class=\"token punctuation\">&lt;</span><span class=\"token keyword\">int</span><span class=\"token punctuation\">&gt;</span></span> list <span class=\"token operator\">=</span> <span class=\"token keyword\">new</span> <span class=\"token constructor-invocation class-name\">List<span class=\"token punctuation\">&lt;</span><span class=\"token keyword\">int</span><span class=\"token punctuation\">&gt;</span></span><span class=\"token punctuation\">(</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n            \n            list<span class=\"token punctuation\">.</span><span class=\"token function\">Add</span><span class=\"token punctuation\">(</span><span class=\"token number\">2</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n            list<span class=\"token punctuation\">.</span><span class=\"token function\">Add</span><span class=\"token punctuation\">(</span><span class=\"token number\">1</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n            list<span class=\"token punctuation\">.</span><span class=\"token function\">Add</span><span class=\"token punctuation\">(</span><span class=\"token number\">3</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n\n            list<span class=\"token punctuation\">.</span><span class=\"token function\">Sort</span><span class=\"token punctuation\">(</span><span class=\"token keyword\">new</span> <span class=\"token constructor-invocation class-name\">Comparison<span class=\"token punctuation\">&lt;</span><span class=\"token keyword\">int</span><span class=\"token punctuation\">&gt;</span></span><span class=\"token punctuation\">(</span>B<span class=\"token punctuation\">.</span>Comparor<span class=\"token punctuation\">)</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n        <span class=\"token punctuation\">}</span>\n        \n    <span class=\"token punctuation\">}</span>\n    <span class=\"token keyword\">class</span> <span class=\"token class-name\">B</span>\n    <span class=\"token punctuation\">{</span>\n        <span class=\"token keyword\">public</span> <span class=\"token keyword\">static</span> <span class=\"token return-type class-name\"><span class=\"token keyword\">int</span></span> <span class=\"token function\">Comparor</span><span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">int</span></span> x<span class=\"token punctuation\">,</span> <span class=\"token class-name\"><span class=\"token keyword\">int</span></span> y<span class=\"token punctuation\">)</span>\n        <span class=\"token punctuation\">{</span>\n            <span class=\"token keyword\">return</span> x <span class=\"token operator\">-</span> y<span class=\"token punctuation\">;</span>\n        <span class=\"token punctuation\">}</span>\n    <span class=\"token punctuation\">}</span>\n<span class=\"token punctuation\">}</span>\n</pre><h5 class=\"mume-header\" id=\"%E5%8C%BF%E5%90%8D%E5%87%BD%E6%95%B0-%E4%B8%AD%E7%BA%A7%E5%BD%A2%E6%80%81\">匿名函数---中级形态</h5>\n\n<p>发现到这里<code>B</code>中的<code>Comparor</code>方法只用了一次,却需要专门定义一个新的类和方法,</p>\n<p>两个字,<strong style=\"color:red;\">繁琐!</strong></p>\n<p>于是后来可以用<code>delegate</code>关键字,<code>delegate(){}</code>来定义匿名方法,所谓匿名方法就是没名字的方法,一气呵成的定义和使用,用完即丢弃.</p>\n<p>这里用匿名函数来重写:</p>\n<pre data-role=\"codeBlock\" data-info=\"cs\" class=\"language-csharp\"><span class=\"token keyword\">namespace</span> <span class=\"token namespace\">ConsoleApp2</span>\n<span class=\"token punctuation\">{</span>\n    <span class=\"token keyword\">class</span> <span class=\"token class-name\">Program</span>\n    <span class=\"token punctuation\">{</span>\n        <span class=\"token keyword\">public</span> <span class=\"token keyword\">static</span> <span class=\"token return-type class-name\"><span class=\"token keyword\">void</span></span> <span class=\"token function\">Main</span><span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">string</span><span class=\"token punctuation\">[</span><span class=\"token punctuation\">]</span></span> args<span class=\"token punctuation\">)</span>\n        <span class=\"token punctuation\">{</span>\n            <span class=\"token class-name\">List<span class=\"token punctuation\">&lt;</span><span class=\"token keyword\">int</span><span class=\"token punctuation\">&gt;</span></span> list <span class=\"token operator\">=</span> <span class=\"token keyword\">new</span> <span class=\"token constructor-invocation class-name\">List<span class=\"token punctuation\">&lt;</span><span class=\"token keyword\">int</span><span class=\"token punctuation\">&gt;</span></span><span class=\"token punctuation\">(</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n            \n            list<span class=\"token punctuation\">.</span><span class=\"token function\">Add</span><span class=\"token punctuation\">(</span><span class=\"token number\">2</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n            list<span class=\"token punctuation\">.</span><span class=\"token function\">Add</span><span class=\"token punctuation\">(</span><span class=\"token number\">1</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n            list<span class=\"token punctuation\">.</span><span class=\"token function\">Add</span><span class=\"token punctuation\">(</span><span class=\"token number\">3</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n\n            list<span class=\"token punctuation\">.</span><span class=\"token function\">Sort</span><span class=\"token punctuation\">(</span><span class=\"token keyword\">new</span> <span class=\"token constructor-invocation class-name\">Comparison<span class=\"token punctuation\">&lt;</span><span class=\"token keyword\">int</span><span class=\"token punctuation\">&gt;</span></span><span class=\"token punctuation\">(</span><span class=\"token keyword\">delegate</span> <span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">int</span></span> x<span class=\"token punctuation\">,</span> <span class=\"token class-name\"><span class=\"token keyword\">int</span></span> y<span class=\"token punctuation\">)</span> <span class=\"token punctuation\">{</span> <span class=\"token keyword\">return</span> x <span class=\"token operator\">-</span> y<span class=\"token punctuation\">;</span> <span class=\"token punctuation\">}</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n        <span class=\"token punctuation\">}</span>\n        \n    <span class=\"token punctuation\">}</span>\n<span class=\"token punctuation\">}</span>\n</pre><h5 class=\"mume-header\" id=\"lambda%E8%A1%A8%E8%BE%BE%E5%BC%8F-%E6%9C%80%E7%BB%88%E5%BD%A2%E6%80%81\">lambda表达式---最终形态</h5>\n\n<p>上面的匿名函数已经做出了一定程度的简化,但是木大木大木大木大还不够简单!!!</p>\n<p>有更简单的!!!</p>\n<p>有更简单的!!!</p>\n<p>有更简单的!!!</p>\n<p>重要的事情说三遍怕你听不清</p>\n<p>当委托里的变量类型明确时(比如这里<code>Conparison&lt;int&gt;</code>就规定了一定是<code>int</code>),匿名函数可以采用一种全新的书写方式:<code>()=&gt;</code>,即lambda表达式.</p>\n<p>这里用lambda表达式来重写:</p>\n<pre data-role=\"codeBlock\" data-info=\"cs\" class=\"language-csharp\"><span class=\"token keyword\">namespace</span> <span class=\"token namespace\">ConsoleApp2</span>\n<span class=\"token punctuation\">{</span>\n    <span class=\"token keyword\">class</span> <span class=\"token class-name\">Program</span>\n    <span class=\"token punctuation\">{</span>\n        <span class=\"token keyword\">public</span> <span class=\"token keyword\">static</span> <span class=\"token return-type class-name\"><span class=\"token keyword\">void</span></span> <span class=\"token function\">Main</span><span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">string</span><span class=\"token punctuation\">[</span><span class=\"token punctuation\">]</span></span> args<span class=\"token punctuation\">)</span>\n        <span class=\"token punctuation\">{</span>\n            <span class=\"token class-name\">List<span class=\"token punctuation\">&lt;</span><span class=\"token keyword\">int</span><span class=\"token punctuation\">&gt;</span></span> list <span class=\"token operator\">=</span> <span class=\"token keyword\">new</span> <span class=\"token constructor-invocation class-name\">List<span class=\"token punctuation\">&lt;</span><span class=\"token keyword\">int</span><span class=\"token punctuation\">&gt;</span></span><span class=\"token punctuation\">(</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n            \n            list<span class=\"token punctuation\">.</span><span class=\"token function\">Add</span><span class=\"token punctuation\">(</span><span class=\"token number\">2</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n            list<span class=\"token punctuation\">.</span><span class=\"token function\">Add</span><span class=\"token punctuation\">(</span><span class=\"token number\">1</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n            list<span class=\"token punctuation\">.</span><span class=\"token function\">Add</span><span class=\"token punctuation\">(</span><span class=\"token number\">3</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n\n            list<span class=\"token punctuation\">.</span><span class=\"token function\">Sort</span><span class=\"token punctuation\">(</span><span class=\"token keyword\">new</span> <span class=\"token constructor-invocation class-name\">Comparison<span class=\"token punctuation\">&lt;</span><span class=\"token keyword\">int</span><span class=\"token punctuation\">&gt;</span></span><span class=\"token punctuation\">(</span><span class=\"token punctuation\">(</span>x<span class=\"token punctuation\">,</span>y<span class=\"token punctuation\">)</span> <span class=\"token operator\">=&gt;</span> x <span class=\"token operator\">-</span> y<span class=\"token punctuation\">)</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n        <span class=\"token punctuation\">}</span>\n        \n    <span class=\"token punctuation\">}</span>\n<span class=\"token punctuation\">}</span>\n</pre><ul>\n<li>当只有一个参数时,<code>()</code>可省略</li>\n<li>当后面的表达式超过一句时,需用大括号括起</li>\n</ul>\n<p>最简单形式形成!</p>\n<p>最后排序结果:</p>\n<blockquote>\n<p>1<br>\n2<br>\n3</p>\n</blockquote>'),(16,'<p>tehbergerththngbfdbgnerhbrgrgrgrdbgnnuyiktyjmyfjtdjtdhd</p>'),(17,'<p><a data-cke-saved-href=\"https://github.com/ryankamanri\" href=\"https://github.com/ryankamanri\">我的GitHub</a></p><hr><h2 style=\"font-style:italic\"><strong>我的GitHub</strong></h2><h3 style=\"color:#aaaaaa; font-style:italic\"><em>我的GitHub</em></h3><div style=\"background:#eeeeee; border:1px solid #cccccc; padding:5px 10px\"><s>我的GitHub</s></div><blockquote><p>我的GitHub</p></blockquote><p><br></p>'),(18,'<p>123123123123123</p>'),(19,'<p>3485345343483456486486483</p>'),(20,'<p>啊啦啦啦啦啦啊啦啦啦啦啦</p><p>哦不错</p>'),(23,'<p><strong>在编程届有个共识，想要成为一个合格的程序员必须要掌握 GitHub 的用法！</strong></p><p><img data-cke-saved-src=\"https://pic2.zhimg.com/80/v2-ddef4098eac5654451aa4d9c68c656e9_720w.jpg\" src=\"https://pic2.zhimg.com/80/v2-ddef4098eac5654451aa4d9c68c656e9_720w.jpg\" width=\"1920\"></p><p>接下来，我们用两万字加一百张图片从头到尾的给你介绍 GitHub 的具体使用，通过这个 GitHub 教程，让你掌握 GitHub 的使用方法。</p><p><a data-cke-saved-href=\"https://zhuanlan.zhihu.com/p/369486197\" data-cke-pa-onclick=\"window.open(this.href, \'\', \'resizable=yes,status=yes,location=yes,toolbar=yes,menubar=yes,fullscreen=yes,scrollbars=yes,dependent=yes\'); return false;\" href=\"https://zhuanlan.zhihu.com/p/369486197\">原文链接</a><br></p>'),(24,'<h2>定义和用法</h2><p>contenteditable 属性规定是否可编辑元素的内容。</p><h2>HTML 4.01 与 HTML 5 之间的差异</h2><p>contenteditable 属性是 HTML5 中的新属性。</p><h2>语法</h2><div style=\"background:#eeeeee;border:1px solid #cccccc;padding:5px 10px;\">&lt;<em>element</em> contenteditable=\"<em>value</em>\"&gt;</div><h3>属性值</h3><table class=\" cke_show_border\"><tbody><tr><th>值</th><th>描述</th></tr><tr><td>true</td><td>规定可以编辑元素内容。</td></tr><tr><td>false</td><td>规定无法编辑元素内容。</td></tr><tr><td><em>classname</em></td><td>继承父元素的 contenteditable 属性。</td></tr></tbody></table>'),(25,'\"<blockquote><p>无须担心，明天早上牛奶还会放在门前的台阶上，《新政治家报》也会照常出版。<br>——George&nbsp;Orwell<br></p></blockquote><p>向外看的是梦中人，向内看的是清醒者。<br>向内看者，唯自我意识觉醒之人。<br>再绚烂的世界，也只不过是内心的一道装饰。<br>支起内心这堵墙的，是随着时间的流逝，一点一点积累起来的坚持。&nbsp;</p><div style=\"background:#eeeeee;border:1px solid #cccccc;padding:5px 10px;\">&nbsp;（仅代表个人观点，不喜勿喷，并无嘲笑和讽刺的意味，如果有哪里语言不当，或有言论冒犯抱歉，只是单纯的觉得，请杠精远离，另外我父母还健在，不是孤儿，受过教育，非杠非黑，并无敌意，仅表达字面含义无讽刺、暗示意味，并无意引发论战，语言不当之处请见谅，本人尊重一切，若本回复冒犯到您我诚挚表示歉意，若您不赞同我的观点不必特地回复我)<br></div>\"'),(26,'\"<blockquote><p>无须担心，明天早上牛奶还会放在门前的台阶上，《新政治家报》也会照常出版。<br>——George&nbsp;Orwell<br></p></blockquote><p>向外看的是梦中人，向内看的是清醒者。<br>向内看者，唯自我意识觉醒之人。<br>再绚烂的世界，也只不过是内心的一道装饰。<br>支起内心这堵墙的，是随着时间的流逝，一点一点积累起来的坚持。&nbsp;</p><div style=\"background:#eeeeee;border:1px solid #cccccc;padding:5px 10px;\">&nbsp;（仅代表个人观点，不喜勿喷，并无嘲笑和讽刺的意味，如果有哪里语言不当，或有言论冒犯抱歉，只是单纯的觉得，请杠精远离，另外我父母还健在，不是孤儿，受过教育，非杠非黑，并无敌意，仅表达字面含义无讽刺、暗示意味，并无意引发论战，语言不当之处请见谅，本人尊重一切，若本回复冒犯到您我诚挚表示歉意，若您不赞同我的观点不必特地回复我)<br></div>\"'),(27,'\"<blockquote><p>无须担心，明天早上牛奶还会放在门前的台阶上，《新政治家报》也会照常出版。<br>——George&nbsp;Orwell<br></p></blockquote><p>向外看的是梦中人，向内看的是清醒者。<br>向内看者，唯自我意识觉醒之人。<br>再绚烂的世界，也只不过是内心的一道装饰。<br>支起内心这堵墙的，是随着时间的流逝，一点一点积累起来的坚持。&nbsp;</p><div style=\"background:#eeeeee;border:1px solid #cccccc;padding:5px 10px;\">&nbsp;（仅代表个人观点，不喜勿喷，并无嘲笑和讽刺的意味，如果有哪里语言不当，或有言论冒犯抱歉，只是单纯的觉得，请杠精远离，另外我父母还健在，不是孤儿，受过教育，非杠非黑，并无敌意，仅表达字面含义无讽刺、暗示意味，并无意引发论战，语言不当之处请见谅，本人尊重一切，若本回复冒犯到您我诚挚表示歉意，若您不赞同我的观点不必特地回复我)<br></div>\"'),(28,'\"<blockquote><p>无须担心，明天早上牛奶还会放在门前的台阶上，《新政治家报》也会照常出版。<br>——George&nbsp;Orwell<br></p></blockquote><p>向外看的是梦中人，向内看的是清醒者。<br>向内看者，唯自我意识觉醒之人。<br>再绚烂的世界，也只不过是内心的一道装饰。<br>支起内心这堵墙的，是随着时间的流逝，一点一点积累起来的坚持。&nbsp;</p><div style=\"background:#eeeeee;border:1px solid #cccccc;padding:5px 10px;\">&nbsp;（仅代表个人观点，不喜勿喷，并无嘲笑和讽刺的意味，如果有哪里语言不当，或有言论冒犯抱歉，只是单纯的觉得，请杠精远离，另外我父母还健在，不是孤儿，受过教育，非杠非黑，并无敌意，仅表达字面含义无讽刺、暗示意味，并无意引发论战，语言不当之处请见谅，本人尊重一切，若本回复冒犯到您我诚挚表示歉意，若您不赞同我的观点不必特地回复我)<br></div>\"'),(29,'\"<blockquote><p>无须担心，明天早上牛奶还会放在门前的台阶上，《新政治家报》也会照常出版。<br>——George&nbsp;Orwell<br></p></blockquote><p>向外看的是梦中人，向内看的是清醒者。<br>向内看者，唯自我意识觉醒之人。<br>再绚烂的世界，也只不过是内心的一道装饰。<br>支起内心这堵墙的，是随着时间的流逝，一点一点积累起来的坚持。&nbsp;</p><div style=\"background:#eeeeee;border:1px solid #cccccc;padding:5px 10px;\">&nbsp;（仅代表个人观点，不喜勿喷，并无嘲笑和讽刺的意味，如果有哪里语言不当，或有言论冒犯抱歉，只是单纯的觉得，请杠精远离，另外我父母还健在，不是孤儿，受过教育，非杠非黑，并无敌意，仅表达字面含义无讽刺、暗示意味，并无意引发论战，语言不当之处请见谅，本人尊重一切，若本回复冒犯到您我诚挚表示歉意，若您不赞同我的观点不必特地回复我)<br></div>\"'),(30,'\"<blockquote><p>（仅代表个人观点，不喜勿喷，并无嘲笑和讽刺的意味，如果有哪里语言不当，或有言论冒犯抱歉，只是单纯的觉得，请杠精远离，另外我父母还健在，不是孤儿，受过教育，非杠非黑，并无敌意，仅表达字面含义无讽刺、暗示意味，并无意引发论战，语言不当之处请见谅，本人尊重一切，若本回复冒犯到您我诚挚表示歉意，若您不赞同我的观点不必特地回复我)<br><br></p></blockquote>\"'),(31,'<p>!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!</p>');
/*!40000 ALTER TABLE `postsinfo` ENABLE KEYS */;
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
) ENGINE=InnoDB AUTO_INCREMENT=49 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tags`
--

LOCK TABLES `tags` WRITE;
/*!40000 ALTER TABLE `tags` DISABLE KEYS */;
INSERT INTO `tags` VALUES (-1,'默认标签'),(1,'第一个标签'),(2,'计算机'),(3,'考研'),(4,'哲学'),(5,'计算机科学技术'),(6,'网络工程'),(7,'软件工程'),(9,'电路原理'),(10,'模拟电子技术'),(11,'人工智能'),(12,'网络管理与维护'),(13,'汇编语言'),(14,'路由与交换技术'),(15,'需求分析与系统设计'),(16,'数据库'),(17,'程序设计语言'),(18,'电路原理图'),(19,'动态电路'),(20,'三极管'),(21,'半导体'),(22,'场效应'),(23,'大数据'),(24,'深度学习'),(25,'VLAN间路由'),(26,'生成树协议'),(27,'端口聚合'),(28,'存储器系统'),(29,'中断系统'),(30,'需求文档'),(31,'数据流图'),(32,'用例图'),(33,'SQL server'),(34,'Oracle'),(35,'Java'),(36,'C++'),(37,'Python'),(38,'Linux命令'),(39,'线性代数'),(40,'概率论'),(41,'用例'),(42,'参与者'),(43,'E-R图'),(44,'SQL语句'),(45,'数据库设计'),(47,'C#'),(48,'.NET Core');
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
INSERT INTO `tags_tags` VALUES (1,-1,''),(2,-1,''),(3,-1,''),(4,-1,''),(5,2,''),(6,2,''),(7,2,''),(9,5,''),(10,5,''),(11,5,''),(12,6,''),(13,6,''),(14,6,''),(15,7,''),(16,7,''),(17,7,''),(18,9,''),(19,9,''),(20,10,''),(21,10,''),(22,10,''),(23,11,''),(24,11,''),(25,12,''),(26,12,''),(27,12,''),(28,13,''),(29,13,''),(30,15,''),(31,15,''),(32,15,''),(33,16,''),(34,16,''),(35,17,''),(36,17,''),(37,17,'{\"relation\":1}'),(38,23,''),(39,24,''),(40,24,''),(41,32,''),(42,32,''),(43,33,''),(44,33,''),(45,34,''),(47,17,''),(48,47,'');
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
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (2,'654321','qq.974481066.qq@gmail.com'),(3,'456789','2168359585@qq.com'),(4,'262425','2624250238@qq.com'),(5,'123456789','853909407@qq.com'),(8,'123456','974481066@qq.com'),(9,'123456','1014958042@qq.com');
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
INSERT INTO `users_posts` VALUES (3,23,'{}'),(3,24,'{}'),(3,29,'{}'),(3,30,'{}'),(3,31,'{\"Type\":\"Owned\"}'),(8,6,''),(8,16,'{}'),(8,17,'{}'),(8,18,'{}'),(8,19,'{}'),(8,20,'{}');
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
INSERT INTO `users_tags` VALUES (2,1,''),(2,4,''),(3,2,'{\"Type\":[\"Self\",\"Interested\"]}'),(3,6,'{\"Type\":[\"Self\"]}'),(3,7,'{\"Type\":[\"Interested\"]}'),(3,12,'{\"Type\":[\"Self\"]}'),(3,16,'{\"Type\":[\"Interested\"]}'),(3,28,'{\"Type\":[\"Interested\"]}'),(3,29,'{\"Type\":[\"Self\"]}'),(5,1,'{\"Type\":[\"Self\"]}'),(5,3,'{\"Type\":[\"Interested\"]}'),(5,5,'{\"Type\":[\"Interested\"]}'),(5,7,'{\"Type\":[\"Self\"]}'),(8,2,'{\"Type\":[\"System.Dynamic.ExpandoObject\",\"Self\",\"Interested\"]}'),(8,5,'{\"Type\":[\"Self\",\"Interested\"]}'),(8,34,'{\"Type\":[\"Interested\"]}'),(8,47,'{\"Type\":[\"System.Dynamic.ExpandoObject\",\"Self\"]}');
/*!40000 ALTER TABLE `users_tags` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users_users`
--

DROP TABLE IF EXISTS `users_users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users_users` (
  `users_1` bigint NOT NULL,
  `users_2` bigint NOT NULL,
  `relations` varchar(255) NOT NULL,
  PRIMARY KEY (`users_1`,`users_2`),
  KEY `users_2` (`users_2`),
  CONSTRAINT `users_users_ibfk_1` FOREIGN KEY (`users_1`) REFERENCES `users` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `users_users_ibfk_2` FOREIGN KEY (`users_2`) REFERENCES `users` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users_users`
--

LOCK TABLES `users_users` WRITE;
/*!40000 ALTER TABLE `users_users` DISABLE KEYS */;
INSERT INTO `users_users` VALUES (3,3,'{\"Type\":[\"Focus\"]}'),(3,8,'{\"Type\":[\"Focus\"]}'),(8,8,'{\"Type\":[\"Focus\"]}');
/*!40000 ALTER TABLE `users_users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `usersinfo`
--

DROP TABLE IF EXISTS `usersinfo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `usersinfo` (
  `ID` bigint NOT NULL,
  `NickName` varchar(255) NOT NULL,
  `RealName` varchar(255) NOT NULL,
  `University` varchar(255) NOT NULL,
  `School` varchar(255) NOT NULL,
  `Speciality` varchar(255) NOT NULL,
  `SchoolYear` varchar(255) NOT NULL,
  `Introduction` varchar(4095) NOT NULL,
  `HeadImage` varchar(1023) NOT NULL,
  KEY `ID` (`ID`),
  CONSTRAINT `usersinfo_ibfk_1` FOREIGN KEY (`ID`) REFERENCES `users` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usersinfo`
--

LOCK TABLES `usersinfo` WRITE;
/*!40000 ALTER TABLE `usersinfo` DISABLE KEYS */;
INSERT INTO `usersinfo` VALUES (8,'kamanri','hwl','四川师范大学','计算机科学学院','计算机科学与技术','2019/1/1 0:00:00','暂时没有想好','/StaticFiles/Images/HeadImage/20210714185547.jpg'),(9,'带感','cxl','四川师范大学','计算机科学学院','计算机科学与技术','2019/1/1 0:00:00','喝奶茶','/Images/duckduckgo.jfif');
/*!40000 ALTER TABLE `usersinfo` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-11-23 19:21:07
