/*
Navicat MySQL Data Transfer

Source Server         : locall
Source Server Version : 50528
Source Host           : localhost:3306
Source Database       : jszx_db

Target Server Type    : MYSQL
Target Server Version : 50528
File Encoding         : 65001

Date: 2013-03-12 22:09:01
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `admins_tb`
-- ----------------------------
DROP TABLE IF EXISTS `admins_tb`;
CREATE TABLE `admins_tb` (
  `AdminID` int(11) NOT NULL AUTO_INCREMENT COMMENT '主键，管理员自动编号，自增长',
  `AdminName` varchar(50) NOT NULL COMMENT '管理员姓名',
  `AdminAccount` varchar(50) NOT NULL COMMENT '管理员账号',
  `AdminPWD` varchar(50) NOT NULL COMMENT '管理员登录密码',
  `AdminAuthority` tinyint(4) NOT NULL DEFAULT '2' COMMENT '1超级管理员，默认值2，普通管理员',
  `AdminStatus` tinyint(4) NOT NULL DEFAULT '1' COMMENT '默认1正常状态，2挂起状态①',
  `AdminBackup` varchar(200) DEFAULT NULL COMMENT '备注',
  PRIMARY KEY (`AdminID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of admins_tb
-- ----------------------------

-- ----------------------------
-- Table structure for `classtimes_tb`
-- ----------------------------
DROP TABLE IF EXISTS `classtimes_tb`;
CREATE TABLE `classtimes_tb` (
  `ClsTmIndex` int(11) NOT NULL COMMENT '主键，课节号，值必须手动指定',
  `ClsTmName` varchar(50) NOT NULL COMMENT '课节名称：第一节',
  `ClsTmStart` time NOT NULL COMMENT '课节开始时间',
  `ClsTmEnd` time NOT NULL COMMENT '课节结束时间',
  PRIMARY KEY (`ClsTmIndex`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of classtimes_tb
-- ----------------------------

-- ----------------------------
-- Table structure for `courses_tb`
-- ----------------------------
DROP TABLE IF EXISTS `courses_tb`;
CREATE TABLE `courses_tb` (
  `CrsID` int(11) NOT NULL AUTO_INCREMENT COMMENT '主键，自增，课程编号',
  `CrsName` varchar(100) DEFAULT NULL COMMENT '课程名',
  `CrsTeacher` varchar(50) DEFAULT NULL COMMENT '教师姓名',
  `CrsClasses` varchar(50) DEFAULT NULL COMMENT '上课班级',
  `CrsHour` float DEFAULT NULL COMMENT '上课学时',
  `CrsTimes` int(11) DEFAULT NULL COMMENT '上课次数',
  `CrsNum` int(11) DEFAULT NULL COMMENT '上课人数',
  `CrsConf` varchar(200) DEFAULT NULL COMMENT '配置要求',
  `CrsRemark` varchar(200) DEFAULT NULL COMMENT '备注',
  PRIMARY KEY (`CrsID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of courses_tb
-- ----------------------------

-- ----------------------------
-- Table structure for `exprecords_tb`
-- ----------------------------
DROP TABLE IF EXISTS `exprecords_tb`;
CREATE TABLE `exprecords_tb` (
  `RecordID` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '主键，自增，实验记录编号',
  `CourseName` varchar(50) DEFAULT NULL COMMENT '课程名称',
  `ExpName` varchar(50) DEFAULT NULL COMMENT '实验名称',
  `ExpClasses` varchar(50) DEFAULT NULL COMMENT '实验班级',
  `Shoulder` int(11) DEFAULT NULL COMMENT '应到人数',
  `Realizer` int(11) DEFAULT NULL COMMENT '实到人数',
  `Groups` int(11) DEFAULT NULL COMMENT '实验组数',
  `PerGroup` int(11) DEFAULT NULL COMMENT '每组人数',
  `StudentStatus` varchar(200) DEFAULT NULL COMMENT '学生出勤情况',
  `InstrumentStatus` varchar(200) DEFAULT NULL COMMENT '仪器使用情况',
  `Problems` varchar(200) DEFAULT NULL COMMENT '是否出现问题',
  `TeacherName` varchar(50) DEFAULT NULL COMMENT '教师姓名',
  `TeacherNumber` varchar(50) DEFAULT NULL COMMENT '教工号',
  `StudentName` varchar(50) DEFAULT NULL COMMENT '学生姓名',
  `ExpCls` tinyint(4) DEFAULT NULL COMMENT '上课节次',
  `PostTime` datetime DEFAULT NULL COMMENT '提交时间',
  `ExpDate` datetime DEFAULT NULL COMMENT '实验日期',
  `ExpLab` varchar(50) DEFAULT NULL COMMENT '实验机房',
  `ExpTerm` int(11) DEFAULT NULL COMMENT '实验学期，外键Terms_tb-TermID',
  `ExpWeek` tinyint(4) DEFAULT NULL COMMENT '实验周次',
  `ExpWeekDay` tinyint(4) DEFAULT NULL COMMENT '实验工作日',
  PRIMARY KEY (`RecordID`),
  KEY `ExpTerm` (`ExpTerm`),
  CONSTRAINT `exprecords_tb_ibfk_1` FOREIGN KEY (`ExpTerm`) REFERENCES `terms_tb` (`TermID`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of exprecords_tb
-- ----------------------------

-- ----------------------------
-- Table structure for `labs_tb`
-- ----------------------------
DROP TABLE IF EXISTS `labs_tb`;
CREATE TABLE `labs_tb` (
  `LabID` int(10) NOT NULL AUTO_INCREMENT COMMENT '主键，自增，实验室编号',
  `LabName` varchar(50) NOT NULL COMMENT '实验室名',
  `LabAdmin` int(11) DEFAULT NULL COMMENT '实验室管理员，外键admins_tb-adminID',
  `LabAddr` varchar(50) DEFAULT NULL COMMENT '实验室地点',
  `LabIP` varchar(50) DEFAULT NULL COMMENT '实验室教师机IP',
  `LabKeyWord` varchar(50) DEFAULT NULL COMMENT '实验室关键字，用于课表导入',
  PRIMARY KEY (`LabID`),
  KEY `LabAdmin` (`LabAdmin`),
  CONSTRAINT `labs_tb_ibfk_1` FOREIGN KEY (`LabAdmin`) REFERENCES `admins_tb` (`AdminID`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of labs_tb
-- ----------------------------

-- ----------------------------
-- Table structure for `schedule_tb`
-- ----------------------------
DROP TABLE IF EXISTS `schedule_tb`;
CREATE TABLE `schedule_tb` (
  `ScdID` int(11) NOT NULL AUTO_INCREMENT COMMENT '主键，自增，课程安排编号',
  `ScdCrs` int(11) NOT NULL COMMENT '上课课程，外键courses_tb-CrsID',
  `ScdWeek` tinyint(4) NOT NULL COMMENT '上课周次',
  `ScdWeekDay` tinyint(4) NOT NULL COMMENT '上课工作日，周日值为0',
  `ScdClass` int(4) NOT NULL COMMENT '上课课节，外键classTimes_tb-ClsTmIndex',
  `ScdLab` int(11) NOT NULL COMMENT '上课实验室，外键labs_tb-LabID',
  `ScdTerm` int(10) NOT NULL COMMENT '上机学期，外键terms_tb-TermID',
  PRIMARY KEY (`ScdID`),
  KEY `ScdCrs` (`ScdCrs`),
  KEY `ScdClass` (`ScdClass`),
  KEY `ScdLab` (`ScdLab`),
  KEY `ScdTerm` (`ScdTerm`),
  CONSTRAINT `schedule_tb_ibfk_4` FOREIGN KEY (`ScdTerm`) REFERENCES `terms_tb` (`TermID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `schedule_tb_ibfk_1` FOREIGN KEY (`ScdCrs`) REFERENCES `courses_tb` (`CrsID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `schedule_tb_ibfk_2` FOREIGN KEY (`ScdClass`) REFERENCES `classtimes_tb` (`ClsTmIndex`) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT `schedule_tb_ibfk_3` FOREIGN KEY (`ScdLab`) REFERENCES `labs_tb` (`LabID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of schedule_tb
-- ----------------------------

-- ----------------------------
-- Table structure for `terms_tb`
-- ----------------------------
DROP TABLE IF EXISTS `terms_tb`;
CREATE TABLE `terms_tb` (
  `TermID` int(11) NOT NULL AUTO_INCREMENT COMMENT '主键，自增，学期编号',
  `TermYear` varchar(50) NOT NULL COMMENT '学期年',
  `TermIndex` bit(1) NOT NULL DEFAULT b'0' COMMENT '默认值0上学期，1下学期',
  `TermStartDay` datetime NOT NULL COMMENT '本学期开学日期，必须为周一',
  `TermWeeks` tinyint(4) NOT NULL COMMENT '本学期周数',
  `TermIsUse` bit(1) NOT NULL DEFAULT b'0' COMMENT '默认值0不可以，1当前可用学期，只有一条记录为1',
  PRIMARY KEY (`TermID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of terms_tb
-- ----------------------------
