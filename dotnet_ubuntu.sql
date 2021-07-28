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
  `Title` varchar(255) NOT NULL,
  `Summary` varchar(1023) NOT NULL,
  `Focus` varchar(255) NOT NULL,
  `PostDateTime` varchar(255) NOT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `postID` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `posts`
--

LOCK TABLES `posts` WRITE;
/*!40000 ALTER TABLE `posts` DISABLE KEYS */;
INSERT INTO `posts` VALUES (6,'��һ������','ί�еľ��� --- �Ա�����Ϊ����(����)ʹ��,������C�к���ָ��',' C# ','2019/1/1 0:00:00');
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
INSERT INTO `posts_tags` VALUES (6,2,''),(6,47,'');
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
INSERT INTO `postsinfo` VALUES (6,'<div id=\"context\">\n    \n      <div class=\"mume markdown-preview  margin-center col-md-8\">\n      <h4 class=\"mume-header\" id=\"%E5%A7%94%E6%89%98\">ί��</h4>\n\n<p><strong>ί�еľ��� --- �Ա�����Ϊ����(����)ʹ��,������C�к���ָ��</strong></p>\n<ul>\n<li>�˴������뷽��������ͬ,ͳ��Ϊ<strong>����</strong></li>\n<li>�˴�\"C\"ָ <strong>C/C++����</strong></li>\n</ul>\n\n<p>�����ȶ���һ�������ռ�,������������</p>\n<pre data-role=\"codeBlock\" data-info=\"cs\" class=\"language-csharp\"><span class=\"token keyword\">namespace</span> <span class=\"token namespace\">ConsoleApp2</span>\n<span class=\"token punctuation\">{</span>\n    <span class=\"token keyword\">class</span> <span class=\"token class-name\">Program</span>\n    <span class=\"token punctuation\">{</span>\n        <span class=\"token keyword\">public</span> <span class=\"token keyword\">static</span> <span class=\"token return-type class-name\"><span class=\"token keyword\">void</span></span> <span class=\"token function\">Main</span><span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">string</span><span class=\"token punctuation\">[</span><span class=\"token punctuation\">]</span></span> args<span class=\"token punctuation\">)</span>\n        <span class=\"token punctuation\">{</span>\n           \n        <span class=\"token punctuation\">}</span>\n        \n    <span class=\"token punctuation\">}</span>\n    <span class=\"token keyword\">class</span> <span class=\"token class-name\">B</span>\n    <span class=\"token punctuation\">{</span>\n        <span class=\"token keyword\">public</span> <span class=\"token keyword\">static</span> <span class=\"token return-type class-name\"><span class=\"token keyword\">int</span></span> <span class=\"token function\">Comparor</span><span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">int</span></span> x<span class=\"token punctuation\">,</span> <span class=\"token class-name\"><span class=\"token keyword\">int</span></span> y<span class=\"token punctuation\">)</span>\n        <span class=\"token punctuation\">{</span>\n            <span class=\"token keyword\">return</span> x <span class=\"token operator\">-</span> y<span class=\"token punctuation\">;</span>\n        <span class=\"token punctuation\">}</span>\n    <span class=\"token punctuation\">}</span>\n<span class=\"token punctuation\">}</span>\n</pre><p>����Ҫ��<code>Program</code>����ʹ��<code>B</code>�����<code>Comparor</code>����,����ô����</p>\n<ol>\n<li>����ֱ������</li>\n</ol>\n<pre data-role=\"codeBlock\" data-info=\"cs\" class=\"language-csharp\"><span class=\"token keyword\">namespace</span> <span class=\"token namespace\">ConsoleApp2</span>\n<span class=\"token punctuation\">{</span>\n    <span class=\"token keyword\">class</span> <span class=\"token class-name\">Program</span>\n    <span class=\"token punctuation\">{</span>\n        <span class=\"token keyword\">public</span> <span class=\"token keyword\">static</span> <span class=\"token return-type class-name\"><span class=\"token keyword\">void</span></span> <span class=\"token function\">Main</span><span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">string</span><span class=\"token punctuation\">[</span><span class=\"token punctuation\">]</span></span> args<span class=\"token punctuation\">)</span>\n        <span class=\"token punctuation\">{</span>\n           Console<span class=\"token punctuation\">.</span><span class=\"token function\">WriteLine</span><span class=\"token punctuation\">(</span>B<span class=\"token punctuation\">.</span><span class=\"token function\">Comparor</span><span class=\"token punctuation\">(</span><span class=\"token number\">1</span><span class=\"token punctuation\">,</span><span class=\"token number\">2</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n        <span class=\"token punctuation\">}</span>\n        \n    <span class=\"token punctuation\">}</span>\n    <span class=\"token keyword\">class</span> <span class=\"token class-name\">B</span>\n    <span class=\"token punctuation\">{</span>\n        <span class=\"token keyword\">public</span> <span class=\"token keyword\">static</span> <span class=\"token return-type class-name\"><span class=\"token keyword\">int</span></span> <span class=\"token function\">Comparor</span><span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">int</span></span> x<span class=\"token punctuation\">,</span> <span class=\"token class-name\"><span class=\"token keyword\">int</span></span> y<span class=\"token punctuation\">)</span>\n        <span class=\"token punctuation\">{</span>\n            <span class=\"token keyword\">return</span> x <span class=\"token operator\">-</span> y<span class=\"token punctuation\">;</span>\n        <span class=\"token punctuation\">}</span>\n    <span class=\"token punctuation\">}</span>\n<span class=\"token punctuation\">}</span>\n</pre><ol start=\"2\">\n<li>����ί��ʵ����һ������(������������ͺ�����˵)ָ���������,��ʹ���������</li>\n</ol>\n<pre data-role=\"codeBlock\" data-info=\"cs\" class=\"language-csharp\"><span class=\"token keyword\">namespace</span> <span class=\"token namespace\">ConsoleApp2</span>\n<span class=\"token punctuation\">{</span>\n    <span class=\"token keyword\">public</span> <span class=\"token keyword\">delegate</span> <span class=\"token return-type class-name\"><span class=\"token keyword\">int</span></span> <span class=\"token function\">DelegateComparor</span><span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">int</span></span> x<span class=\"token punctuation\">,</span><span class=\"token class-name\"><span class=\"token keyword\">int</span></span> y<span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span><span class=\"token comment\">//�����ⶨ��ί��</span>\n    <span class=\"token keyword\">class</span> <span class=\"token class-name\">Program</span>\n    <span class=\"token punctuation\">{</span>\n        <span class=\"token keyword\">public</span> <span class=\"token keyword\">static</span> <span class=\"token return-type class-name\"><span class=\"token keyword\">void</span></span> <span class=\"token function\">Main</span><span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">string</span><span class=\"token punctuation\">[</span><span class=\"token punctuation\">]</span></span> args<span class=\"token punctuation\">)</span>\n        <span class=\"token punctuation\">{</span>\n            <span class=\"token class-name\">DelegateComparor</span> comparor <span class=\"token operator\">=</span> <span class=\"token keyword\">new</span> <span class=\"token constructor-invocation class-name\">DelegateComparor</span><span class=\"token punctuation\">(</span>B<span class=\"token punctuation\">.</span>Comparor<span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n            Console<span class=\"token punctuation\">.</span><span class=\"token function\">WriteLine</span><span class=\"token punctuation\">(</span><span class=\"token function\">comparor</span><span class=\"token punctuation\">(</span><span class=\"token number\">1</span><span class=\"token punctuation\">,</span><span class=\"token number\">2</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n        <span class=\"token punctuation\">}</span>\n        \n    <span class=\"token punctuation\">}</span>\n    <span class=\"token keyword\">class</span> <span class=\"token class-name\">B</span>\n    <span class=\"token punctuation\">{</span>\n        <span class=\"token keyword\">public</span> <span class=\"token keyword\">static</span> <span class=\"token return-type class-name\"><span class=\"token keyword\">int</span></span> <span class=\"token function\">Comparor</span><span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">int</span></span> x<span class=\"token punctuation\">,</span> <span class=\"token class-name\"><span class=\"token keyword\">int</span></span> y<span class=\"token punctuation\">)</span>\n        <span class=\"token punctuation\">{</span>\n            <span class=\"token keyword\">return</span> x <span class=\"token operator\">-</span> y<span class=\"token punctuation\">;</span>\n        <span class=\"token punctuation\">}</span>\n    <span class=\"token punctuation\">}</span>\n<span class=\"token punctuation\">}</span>\n</pre><p>�������ĺô��ǿ��Ը�������ʹ�ò�ͬ�����ͬһ���ͷ���.����������ͳһ�淶.�������ò�����ɵ��쳣.(���������)</p>\n<h4 class=\"mume-header\" id=\"func%E4%B8%8Eaction\">Func&lt;&gt;��Action&lt;&gt;</h4>\n\n<p>���Ǹղ��Ǹ���</p>\n<pre data-role=\"codeBlock\" data-info=\"cs\" class=\"language-csharp\"><span class=\"token keyword\">namespace</span> <span class=\"token namespace\">ConsoleApp2</span>\n<span class=\"token punctuation\">{</span>\n    <span class=\"token keyword\">class</span> <span class=\"token class-name\">Program</span>\n    <span class=\"token punctuation\">{</span>\n        <span class=\"token keyword\">public</span> <span class=\"token keyword\">static</span> <span class=\"token return-type class-name\"><span class=\"token keyword\">void</span></span> <span class=\"token function\">Main</span><span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">string</span><span class=\"token punctuation\">[</span><span class=\"token punctuation\">]</span></span> args<span class=\"token punctuation\">)</span>\n        <span class=\"token punctuation\">{</span>\n           \n        <span class=\"token punctuation\">}</span>\n        \n    <span class=\"token punctuation\">}</span>\n    <span class=\"token keyword\">class</span> <span class=\"token class-name\">B</span>\n    <span class=\"token punctuation\">{</span>\n        <span class=\"token keyword\">public</span> <span class=\"token keyword\">static</span> <span class=\"token return-type class-name\"><span class=\"token keyword\">int</span></span> <span class=\"token function\">Comparor</span><span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">int</span></span> x<span class=\"token punctuation\">,</span> <span class=\"token class-name\"><span class=\"token keyword\">int</span></span> y<span class=\"token punctuation\">)</span>\n        <span class=\"token punctuation\">{</span>\n            <span class=\"token keyword\">return</span> x <span class=\"token operator\">-</span> y<span class=\"token punctuation\">;</span>\n        <span class=\"token punctuation\">}</span>\n    <span class=\"token punctuation\">}</span>\n<span class=\"token punctuation\">}</span>\n</pre><p>���ڻص��ղ��Ǹ�����,������<code>Program</code>����ʹ��<code>B</code>�����<code>Comparor</code>����,����ί��֮��?<br>\n��ί�в���,���Ƿ���ί�г���ָ�򷽷��ı���<code>comparor</code>�����Ͳ�δֱ��˵��,ֻ��ί�ж�����</p>\n<pre data-role=\"codeBlock\" data-info=\"cs\" class=\"language-csharp\"><span class=\"token keyword\">public</span> <span class=\"token keyword\">delegate</span> <span class=\"token return-type class-name\"><span class=\"token keyword\">int</span></span> <span class=\"token function\">DelegateComparor</span><span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">int</span></span> x<span class=\"token punctuation\">,</span><span class=\"token class-name\"><span class=\"token keyword\">int</span></span> y<span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n</pre><p>�ſ�֪����һ��������<code>int</code>����,����ֵΪ<code>int</code>�ķ���.</p>\n<p>��ô,����ܹ��ҵ���C��һ��ָ������ͺ�����ָ��(��<code>int (*p)(int,int)</code>)������,�Ϳ���ֱ�Ӷ����������͵ı�������ֵ��.</p>\n<p>���,C#���ṩ��<code>Func&lt;&gt;</code>��<code>Action&lt;&gt;</code>��������һ\"����ָ��\"����</p>\n<p>ʵ��:</p>\n<blockquote>\n<p>C# == C<br>\nAction == void (*)() <em>//�޲���,�޷���ֵ</em><br>\nAction&lt;T1&gt; == void (*)(T1) <em>//��һ������,�޷���ֵ</em><br>\nAction&lt;T1,T2&gt; == void (*)(T1,T2) <em>//����������,�޷���ֵ</em><br>\nFunc&lt;T&gt; == T (*)() <em>//�޲���,��Ψһ����ֵ</em><br>\nFunc&lt;T1,T&gt; == T (*)(T1) <em>//��һ������,��һ������ֵ</em><br>\nFunc&lt;T1,T2,T&gt; == T (*)(T1,T2) <em>//����������,��һ������ֵ</em></p>\n</blockquote>\n<p>���,������������:</p>\n<pre data-role=\"codeBlock\" data-info=\"cs\" class=\"language-csharp\"><span class=\"token keyword\">namespace</span> <span class=\"token namespace\">ConsoleApp2</span>\n<span class=\"token punctuation\">{</span>\n    <span class=\"token keyword\">class</span> <span class=\"token class-name\">Program</span>\n    <span class=\"token punctuation\">{</span>\n        <span class=\"token keyword\">public</span> <span class=\"token keyword\">static</span> <span class=\"token return-type class-name\"><span class=\"token keyword\">void</span></span> <span class=\"token function\">Main</span><span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">string</span><span class=\"token punctuation\">[</span><span class=\"token punctuation\">]</span></span> args<span class=\"token punctuation\">)</span>\n        <span class=\"token punctuation\">{</span>\n           <span class=\"token class-name\">Func<span class=\"token punctuation\">&lt;</span><span class=\"token keyword\">int</span><span class=\"token punctuation\">,</span> <span class=\"token keyword\">int</span><span class=\"token punctuation\">,</span> <span class=\"token keyword\">int</span><span class=\"token punctuation\">&gt;</span></span> comparor <span class=\"token operator\">=</span> B<span class=\"token punctuation\">.</span>Comparor<span class=\"token punctuation\">;</span>\n           Console<span class=\"token punctuation\">.</span><span class=\"token function\">WriteLine</span><span class=\"token punctuation\">(</span><span class=\"token function\">comparor</span><span class=\"token punctuation\">(</span><span class=\"token number\">1</span><span class=\"token punctuation\">,</span><span class=\"token number\">2</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n        <span class=\"token punctuation\">}</span>\n        \n    <span class=\"token punctuation\">}</span>\n    <span class=\"token keyword\">class</span> <span class=\"token class-name\">B</span>\n    <span class=\"token punctuation\">{</span>\n        <span class=\"token keyword\">public</span> <span class=\"token keyword\">static</span> <span class=\"token return-type class-name\"><span class=\"token keyword\">int</span></span> <span class=\"token function\">Comparor</span><span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">int</span></span> x<span class=\"token punctuation\">,</span> <span class=\"token class-name\"><span class=\"token keyword\">int</span></span> y<span class=\"token punctuation\">)</span>\n        <span class=\"token punctuation\">{</span>\n            <span class=\"token keyword\">return</span> x <span class=\"token operator\">-</span> y<span class=\"token punctuation\">;</span>\n        <span class=\"token punctuation\">}</span>\n    <span class=\"token punctuation\">}</span>\n<span class=\"token punctuation\">}</span>\n</pre><p>��.</p>\n<h4 class=\"mume-header\" id=\"%E4%BB%8E%E5%87%BD%E6%95%B0%E5%88%B0%E5%8C%BF%E5%90%8D%E5%87%BD%E6%95%B0%E5%86%8D%E5%88%B0lambda%E8%A1%A8%E8%BE%BE%E5%BC%8F\">�Ӻ��������������ٵ�lambda����ʽ</h4>\n\n<p>ps. �������֪�����Ǹ������Ĺ���</p>\n<h5 class=\"mume-header\" id=\"%E5%87%BD%E6%95%B0-%E5%88%9D%E7%BA%A7%E5%BD%A2%E6%80%81\">����---������̬</h5>\n\n<p>��Ȼ�Ǹղ��Ǹ���,����һ��List����</p>\n<pre data-role=\"codeBlock\" data-info=\"cs\" class=\"language-csharp\"><span class=\"token keyword\">namespace</span> <span class=\"token namespace\">ConsoleApp2</span>\n<span class=\"token punctuation\">{</span>\n    <span class=\"token keyword\">class</span> <span class=\"token class-name\">Program</span>\n    <span class=\"token punctuation\">{</span>\n        <span class=\"token keyword\">public</span> <span class=\"token keyword\">static</span> <span class=\"token return-type class-name\"><span class=\"token keyword\">void</span></span> <span class=\"token function\">Main</span><span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">string</span><span class=\"token punctuation\">[</span><span class=\"token punctuation\">]</span></span> args<span class=\"token punctuation\">)</span>\n        <span class=\"token punctuation\">{</span>\n            <span class=\"token class-name\">List<span class=\"token punctuation\">&lt;</span><span class=\"token keyword\">int</span><span class=\"token punctuation\">&gt;</span></span> list <span class=\"token operator\">=</span> <span class=\"token keyword\">new</span> <span class=\"token constructor-invocation class-name\">List<span class=\"token punctuation\">&lt;</span><span class=\"token keyword\">int</span><span class=\"token punctuation\">&gt;</span></span><span class=\"token punctuation\">(</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n            \n            list<span class=\"token punctuation\">.</span><span class=\"token function\">Add</span><span class=\"token punctuation\">(</span><span class=\"token number\">2</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n            list<span class=\"token punctuation\">.</span><span class=\"token function\">Add</span><span class=\"token punctuation\">(</span><span class=\"token number\">1</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n            list<span class=\"token punctuation\">.</span><span class=\"token function\">Add</span><span class=\"token punctuation\">(</span><span class=\"token number\">3</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n        <span class=\"token punctuation\">}</span>\n        \n    <span class=\"token punctuation\">}</span>\n    <span class=\"token keyword\">class</span> <span class=\"token class-name\">B</span>\n    <span class=\"token punctuation\">{</span>\n        <span class=\"token keyword\">public</span> <span class=\"token keyword\">static</span> <span class=\"token return-type class-name\"><span class=\"token keyword\">int</span></span> <span class=\"token function\">Comparor</span><span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">int</span></span> x<span class=\"token punctuation\">,</span> <span class=\"token class-name\"><span class=\"token keyword\">int</span></span> y<span class=\"token punctuation\">)</span>\n        <span class=\"token punctuation\">{</span>\n            <span class=\"token keyword\">return</span> x <span class=\"token operator\">-</span> y<span class=\"token punctuation\">;</span>\n        <span class=\"token punctuation\">}</span>\n    <span class=\"token punctuation\">}</span>\n<span class=\"token punctuation\">}</span>\n</pre><p>��������Ҫ�����list��������,����ô�涨�ǴӴ�С���Ǵ�С������?</p>\n<p>��Ϳ���ͨ���Ǹ�<code>Comparor</code>����������.</p>\n<p>���������е�����Ҫ�Ƚϵ�����Ԫ��<code>x</code>,<code>y</code>,�緵��Ϊ����<code>x -&gt; y</code>��,����<code>y -&gt; x</code>��.(������ô�Ƚ��ɺ����ڲ����ж���)</p>\n<p><code>List</code>���е�<code>Sort</code>��������һ��<code>Comparison&lt;T&gt;</code>���Ͳ���.�����<code>Comparison&lt;T&gt;</code>,����һ�����ҽ��ɽ���<code>Func&lt;int, int, int&gt;</code>���͵�ί��.</p>\n<p>��������д��,����<code>Comparison&lt;T&gt;</code>ί��:</p>\n<pre data-role=\"codeBlock\" data-info=\"cs\" class=\"language-csharp\"><span class=\"token keyword\">namespace</span> <span class=\"token namespace\">ConsoleApp2</span>\n<span class=\"token punctuation\">{</span>\n    <span class=\"token keyword\">class</span> <span class=\"token class-name\">Program</span>\n    <span class=\"token punctuation\">{</span>\n        <span class=\"token keyword\">public</span> <span class=\"token keyword\">static</span> <span class=\"token return-type class-name\"><span class=\"token keyword\">void</span></span> <span class=\"token function\">Main</span><span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">string</span><span class=\"token punctuation\">[</span><span class=\"token punctuation\">]</span></span> args<span class=\"token punctuation\">)</span>\n        <span class=\"token punctuation\">{</span>\n            <span class=\"token class-name\">List<span class=\"token punctuation\">&lt;</span><span class=\"token keyword\">int</span><span class=\"token punctuation\">&gt;</span></span> list <span class=\"token operator\">=</span> <span class=\"token keyword\">new</span> <span class=\"token constructor-invocation class-name\">List<span class=\"token punctuation\">&lt;</span><span class=\"token keyword\">int</span><span class=\"token punctuation\">&gt;</span></span><span class=\"token punctuation\">(</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n            \n            list<span class=\"token punctuation\">.</span><span class=\"token function\">Add</span><span class=\"token punctuation\">(</span><span class=\"token number\">2</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n            list<span class=\"token punctuation\">.</span><span class=\"token function\">Add</span><span class=\"token punctuation\">(</span><span class=\"token number\">1</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n            list<span class=\"token punctuation\">.</span><span class=\"token function\">Add</span><span class=\"token punctuation\">(</span><span class=\"token number\">3</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n            <span class=\"token class-name\">Comparison<span class=\"token punctuation\">&lt;</span><span class=\"token keyword\">int</span><span class=\"token punctuation\">&gt;</span></span> comparison <span class=\"token operator\">=</span> <span class=\"token keyword\">new</span> <span class=\"token constructor-invocation class-name\">Comparison<span class=\"token punctuation\">&lt;</span><span class=\"token keyword\">int</span><span class=\"token punctuation\">&gt;</span></span><span class=\"token punctuation\">(</span>B<span class=\"token punctuation\">.</span>Comparor<span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n            list<span class=\"token punctuation\">.</span><span class=\"token function\">Sort</span><span class=\"token punctuation\">(</span>comparison<span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n        <span class=\"token punctuation\">}</span>\n        \n    <span class=\"token punctuation\">}</span>\n    <span class=\"token keyword\">class</span> <span class=\"token class-name\">B</span>\n    <span class=\"token punctuation\">{</span>\n        <span class=\"token keyword\">public</span> <span class=\"token keyword\">static</span> <span class=\"token return-type class-name\"><span class=\"token keyword\">int</span></span> <span class=\"token function\">Comparor</span><span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">int</span></span> x<span class=\"token punctuation\">,</span> <span class=\"token class-name\"><span class=\"token keyword\">int</span></span> y<span class=\"token punctuation\">)</span>\n        <span class=\"token punctuation\">{</span>\n            <span class=\"token keyword\">return</span> x <span class=\"token operator\">-</span> y<span class=\"token punctuation\">;</span>\n        <span class=\"token punctuation\">}</span>\n    <span class=\"token punctuation\">}</span>\n<span class=\"token punctuation\">}</span>\n</pre><p>��ȻҲ���Բ���ֱ�����÷�:</p>\n<pre data-role=\"codeBlock\" data-info=\"cs\" class=\"language-csharp\">list<span class=\"token punctuation\">.</span><span class=\"token function\">Sort</span><span class=\"token punctuation\">(</span>B<span class=\"token punctuation\">.</span>Comparor<span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n</pre><p>�����������ı׶�Ҳ������: ������<code>B.Comparor</code>�����Ͳ�Ϊ<code>Func&lt;int,int,int&gt;</code> �ͻ������쳣,Ҳ�����ڹ淶.ί�еĺô�������<strong>��ָ�����ı��������淶</strong>.</p>\n<p>���,���ﲻ����,ȥ�������<code>Comparison&lt;T&gt;</code>ί�����ͱ���.</p>\n<pre data-role=\"codeBlock\" data-info=\"cs\" class=\"language-csharp\"><span class=\"token keyword\">namespace</span> <span class=\"token namespace\">ConsoleApp2</span>\n<span class=\"token punctuation\">{</span>\n    <span class=\"token keyword\">class</span> <span class=\"token class-name\">Program</span>\n    <span class=\"token punctuation\">{</span>\n        <span class=\"token keyword\">public</span> <span class=\"token keyword\">static</span> <span class=\"token return-type class-name\"><span class=\"token keyword\">void</span></span> <span class=\"token function\">Main</span><span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">string</span><span class=\"token punctuation\">[</span><span class=\"token punctuation\">]</span></span> args<span class=\"token punctuation\">)</span>\n        <span class=\"token punctuation\">{</span>\n            <span class=\"token class-name\">List<span class=\"token punctuation\">&lt;</span><span class=\"token keyword\">int</span><span class=\"token punctuation\">&gt;</span></span> list <span class=\"token operator\">=</span> <span class=\"token keyword\">new</span> <span class=\"token constructor-invocation class-name\">List<span class=\"token punctuation\">&lt;</span><span class=\"token keyword\">int</span><span class=\"token punctuation\">&gt;</span></span><span class=\"token punctuation\">(</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n            \n            list<span class=\"token punctuation\">.</span><span class=\"token function\">Add</span><span class=\"token punctuation\">(</span><span class=\"token number\">2</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n            list<span class=\"token punctuation\">.</span><span class=\"token function\">Add</span><span class=\"token punctuation\">(</span><span class=\"token number\">1</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n            list<span class=\"token punctuation\">.</span><span class=\"token function\">Add</span><span class=\"token punctuation\">(</span><span class=\"token number\">3</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n\n            list<span class=\"token punctuation\">.</span><span class=\"token function\">Sort</span><span class=\"token punctuation\">(</span><span class=\"token keyword\">new</span> <span class=\"token constructor-invocation class-name\">Comparison<span class=\"token punctuation\">&lt;</span><span class=\"token keyword\">int</span><span class=\"token punctuation\">&gt;</span></span><span class=\"token punctuation\">(</span>B<span class=\"token punctuation\">.</span>Comparor<span class=\"token punctuation\">)</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n        <span class=\"token punctuation\">}</span>\n        \n    <span class=\"token punctuation\">}</span>\n    <span class=\"token keyword\">class</span> <span class=\"token class-name\">B</span>\n    <span class=\"token punctuation\">{</span>\n        <span class=\"token keyword\">public</span> <span class=\"token keyword\">static</span> <span class=\"token return-type class-name\"><span class=\"token keyword\">int</span></span> <span class=\"token function\">Comparor</span><span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">int</span></span> x<span class=\"token punctuation\">,</span> <span class=\"token class-name\"><span class=\"token keyword\">int</span></span> y<span class=\"token punctuation\">)</span>\n        <span class=\"token punctuation\">{</span>\n            <span class=\"token keyword\">return</span> x <span class=\"token operator\">-</span> y<span class=\"token punctuation\">;</span>\n        <span class=\"token punctuation\">}</span>\n    <span class=\"token punctuation\">}</span>\n<span class=\"token punctuation\">}</span>\n</pre><h5 class=\"mume-header\" id=\"%E5%8C%BF%E5%90%8D%E5%87%BD%E6%95%B0-%E4%B8%AD%E7%BA%A7%E5%BD%A2%E6%80%81\">��������---�м���̬</h5>\n\n<p>���ֵ�����<code>B</code>�е�<code>Comparor</code>����ֻ����һ��,ȴ��Ҫר�Ŷ���һ���µ���ͷ���,</p>\n<p>������,<strong style=\"color:red;\">����!</strong></p>\n<p>���Ǻ���������<code>delegate</code>�ؼ���,<code>delegate(){}</code>��������������,��ν������������û���ֵķ���,һ���ǳɵĶ����ʹ��,���꼴����.</p>\n<p>������������������д:</p>\n<pre data-role=\"codeBlock\" data-info=\"cs\" class=\"language-csharp\"><span class=\"token keyword\">namespace</span> <span class=\"token namespace\">ConsoleApp2</span>\n<span class=\"token punctuation\">{</span>\n    <span class=\"token keyword\">class</span> <span class=\"token class-name\">Program</span>\n    <span class=\"token punctuation\">{</span>\n        <span class=\"token keyword\">public</span> <span class=\"token keyword\">static</span> <span class=\"token return-type class-name\"><span class=\"token keyword\">void</span></span> <span class=\"token function\">Main</span><span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">string</span><span class=\"token punctuation\">[</span><span class=\"token punctuation\">]</span></span> args<span class=\"token punctuation\">)</span>\n        <span class=\"token punctuation\">{</span>\n            <span class=\"token class-name\">List<span class=\"token punctuation\">&lt;</span><span class=\"token keyword\">int</span><span class=\"token punctuation\">&gt;</span></span> list <span class=\"token operator\">=</span> <span class=\"token keyword\">new</span> <span class=\"token constructor-invocation class-name\">List<span class=\"token punctuation\">&lt;</span><span class=\"token keyword\">int</span><span class=\"token punctuation\">&gt;</span></span><span class=\"token punctuation\">(</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n            \n            list<span class=\"token punctuation\">.</span><span class=\"token function\">Add</span><span class=\"token punctuation\">(</span><span class=\"token number\">2</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n            list<span class=\"token punctuation\">.</span><span class=\"token function\">Add</span><span class=\"token punctuation\">(</span><span class=\"token number\">1</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n            list<span class=\"token punctuation\">.</span><span class=\"token function\">Add</span><span class=\"token punctuation\">(</span><span class=\"token number\">3</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n\n            list<span class=\"token punctuation\">.</span><span class=\"token function\">Sort</span><span class=\"token punctuation\">(</span><span class=\"token keyword\">new</span> <span class=\"token constructor-invocation class-name\">Comparison<span class=\"token punctuation\">&lt;</span><span class=\"token keyword\">int</span><span class=\"token punctuation\">&gt;</span></span><span class=\"token punctuation\">(</span><span class=\"token keyword\">delegate</span> <span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">int</span></span> x<span class=\"token punctuation\">,</span> <span class=\"token class-name\"><span class=\"token keyword\">int</span></span> y<span class=\"token punctuation\">)</span> <span class=\"token punctuation\">{</span> <span class=\"token keyword\">return</span> x <span class=\"token operator\">-</span> y<span class=\"token punctuation\">;</span> <span class=\"token punctuation\">}</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n        <span class=\"token punctuation\">}</span>\n        \n    <span class=\"token punctuation\">}</span>\n<span class=\"token punctuation\">}</span>\n</pre><h5 class=\"mume-header\" id=\"lambda%E8%A1%A8%E8%BE%BE%E5%BC%8F-%E6%9C%80%E7%BB%88%E5%BD%A2%E6%80%81\">lambda����ʽ---������̬</h5>\n\n<p>��������������Ѿ�������һ���̶ȵļ�,����ľ��ľ��ľ��ľ�󻹲�����!!!</p>\n<p>�и��򵥵�!!!</p>\n<p>�и��򵥵�!!!</p>\n<p>�и��򵥵�!!!</p>\n<p>��Ҫ������˵��������������</p>\n<p>��ί����ı���������ȷʱ(��������<code>Conparison&lt;int&gt;</code>�͹涨��һ����<code>int</code>),�����������Բ���һ��ȫ�µ���д��ʽ:<code>()=&gt;</code>,��lambda����ʽ.</p>\n<p>������lambda����ʽ����д:</p>\n<pre data-role=\"codeBlock\" data-info=\"cs\" class=\"language-csharp\"><span class=\"token keyword\">namespace</span> <span class=\"token namespace\">ConsoleApp2</span>\n<span class=\"token punctuation\">{</span>\n    <span class=\"token keyword\">class</span> <span class=\"token class-name\">Program</span>\n    <span class=\"token punctuation\">{</span>\n        <span class=\"token keyword\">public</span> <span class=\"token keyword\">static</span> <span class=\"token return-type class-name\"><span class=\"token keyword\">void</span></span> <span class=\"token function\">Main</span><span class=\"token punctuation\">(</span><span class=\"token class-name\"><span class=\"token keyword\">string</span><span class=\"token punctuation\">[</span><span class=\"token punctuation\">]</span></span> args<span class=\"token punctuation\">)</span>\n        <span class=\"token punctuation\">{</span>\n            <span class=\"token class-name\">List<span class=\"token punctuation\">&lt;</span><span class=\"token keyword\">int</span><span class=\"token punctuation\">&gt;</span></span> list <span class=\"token operator\">=</span> <span class=\"token keyword\">new</span> <span class=\"token constructor-invocation class-name\">List<span class=\"token punctuation\">&lt;</span><span class=\"token keyword\">int</span><span class=\"token punctuation\">&gt;</span></span><span class=\"token punctuation\">(</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n            \n            list<span class=\"token punctuation\">.</span><span class=\"token function\">Add</span><span class=\"token punctuation\">(</span><span class=\"token number\">2</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n            list<span class=\"token punctuation\">.</span><span class=\"token function\">Add</span><span class=\"token punctuation\">(</span><span class=\"token number\">1</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n            list<span class=\"token punctuation\">.</span><span class=\"token function\">Add</span><span class=\"token punctuation\">(</span><span class=\"token number\">3</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n\n            list<span class=\"token punctuation\">.</span><span class=\"token function\">Sort</span><span class=\"token punctuation\">(</span><span class=\"token keyword\">new</span> <span class=\"token constructor-invocation class-name\">Comparison<span class=\"token punctuation\">&lt;</span><span class=\"token keyword\">int</span><span class=\"token punctuation\">&gt;</span></span><span class=\"token punctuation\">(</span><span class=\"token punctuation\">(</span>x<span class=\"token punctuation\">,</span>y<span class=\"token punctuation\">)</span> <span class=\"token operator\">=&gt;</span> x <span class=\"token operator\">-</span> y<span class=\"token punctuation\">)</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>\n        <span class=\"token punctuation\">}</span>\n        \n    <span class=\"token punctuation\">}</span>\n<span class=\"token punctuation\">}</span>\n</pre><ul>\n<li>��ֻ��һ������ʱ,<code>()</code>��ʡ��</li>\n<li>������ı���ʽ����һ��ʱ,���ô���������</li>\n</ul>\n<p>�����ʽ�γ�!</p>\n<p>���������:</p>\n<blockquote>\n<p>1<br>\n2<br>\n3</p>\n</blockquote>');
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
INSERT INTO `tags` VALUES (-1,'Ĭ�ϱ�ǩ'),(1,'��һ����ǩ'),(2,'�����'),(3,'����'),(4,'��ѧ'),(5,'�������ѧ����'),(6,'���繤��'),(7,'��������'),(9,'��·ԭ��'),(10,'ģ����Ӽ���'),(11,'�˹�����'),(12,'���������ά��'),(13,'�������'),(14,'·���뽻������'),(15,'���������ϵͳ���'),(16,'���ݿ�'),(17,'�����������'),(18,'��·ԭ��ͼ'),(19,'��̬��·'),(20,'������'),(21,'�뵼��'),(22,'��ЧӦ'),(23,'������'),(24,'���ѧϰ'),(25,'VLAN��·��'),(26,'������Э��'),(27,'�˿ھۺ�'),(28,'�洢��ϵͳ'),(29,'�ж�ϵͳ'),(30,'�����ĵ�'),(31,'������ͼ'),(32,'����ͼ'),(33,'SQL server'),(34,'Oracle'),(35,'Java'),(36,'C++'),(37,'Python'),(38,'Linux����'),(39,'���Դ���'),(40,'������'),(41,'����'),(42,'������'),(43,'E-Rͼ'),(44,'SQL���'),(45,'���ݿ����'),(47,'C#'),(48,'.NET Core');
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
INSERT INTO `users_posts` VALUES (8,6,'');
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
INSERT INTO `users_tags` VALUES (2,1,''),(2,4,''),(3,2,'{\"Type\":[\"Self\"]}'),(3,3,'{\"Type\":[\"Interested\"]}'),(3,13,'{\"Type\":[\"Self\"]}'),(3,15,'{\"Type\":[\"Interested\"]}'),(3,47,'{\"Type\":[\"Interested\"]}'),(5,1,'{\"Type\":[\"Self\"]}'),(5,3,'{\"Type\":[\"Interested\"]}'),(5,5,'{\"Type\":[\"Interested\"]}'),(5,7,'{\"Type\":[\"Self\"]}'),(8,2,'{\"Type\":[\"System.Dynamic.ExpandoObject\",\"Interested\"]}'),(8,5,'{\"Type\":[\"Interested\"]}'),(8,16,'{\"Type\":[\"Self\",\"Interested\"]}'),(8,38,'{\"Type\":[\"Interested\"]}'),(8,47,'{\"Type\":[\"System.Dynamic.ExpandoObject\",\"Interested\"]}'),(8,48,'{\"Type\":[\"Self\",\"Interested\"]}');
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
INSERT INTO `users_users` VALUES (2,8,'');
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
INSERT INTO `usersinfo` VALUES (8,'kamanri','hwl','�Ĵ�ʦ����ѧ','�������ѧѧԺ','�������ѧ�뼼��','2019/1/1 0:00:00','��ʱû�����','/Images/HeadImage/20210714185547.jpg');
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

-- Dump completed on 2021-07-18 20:39:39