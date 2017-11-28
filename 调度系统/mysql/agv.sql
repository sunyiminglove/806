/*
Navicat MySQL Data Transfer

Source Server         : 806
Source Server Version : 50553
Source Host           : localhost:3306
Source Database       : agv

Target Server Type    : MYSQL
Target Server Version : 50553
File Encoding         : 65001

Date: 2017-06-16 13:56:32
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `parameter`
-- ----------------------------
DROP TABLE IF EXISTS `parameter`;
CREATE TABLE `parameter` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of parameter
-- ----------------------------
INSERT INTO `parameter` VALUES ('1');
INSERT INTO `parameter` VALUES ('2');
INSERT INTO `parameter` VALUES ('3');

-- ----------------------------
-- Table structure for `station`
-- ----------------------------
DROP TABLE IF EXISTS `station`;
CREATE TABLE `station` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `userid` varchar(11) NOT NULL,
  `num` varchar(10) NOT NULL COMMENT '地标编号',
  `size` varchar(10) NOT NULL COMMENT '标识直径',
  `wordx` varchar(10) NOT NULL COMMENT '字符x坐标',
  `wordy` varchar(10) NOT NULL COMMENT '字符y坐标',
  `rectx` varchar(10) NOT NULL COMMENT '标识x坐标',
  `recty` varchar(10) NOT NULL COMMENT '标识y坐标',
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=40 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of station
-- ----------------------------
INSERT INTO `station` VALUES ('32', '5', '15', '20', '600', '494', '624', '536');
INSERT INTO `station` VALUES ('31', '4', '14', '20', '813', '429', '838', '480');
INSERT INTO `station` VALUES ('30', '3', '13', '20', '740', '435', '766', '481');
INSERT INTO `station` VALUES ('26', '1', '11', '20', '548', '549', '601', '565');
INSERT INTO `station` VALUES ('29', '2', '12', '20', '532', '489', '559', '535');
INSERT INTO `station` VALUES ('33', '6', '19', '20', '606', '369', '600', '387');
INSERT INTO `station` VALUES ('34', '7', '16', '20', '604', '304', '600', '322');
INSERT INTO `station` VALUES ('35', '8', '17', '20', '550', '444', '602', '464');
INSERT INTO `station` VALUES ('36', '9', '3', '20', '897', '434', '912', '482');
INSERT INTO `station` VALUES ('37', '10', '1', '20', '511', '492', '526', '537');
INSERT INTO `station` VALUES ('38', '11', '12', '20', '71', '567', '95', '627');
INSERT INTO `station` VALUES ('39', '12', '13', '20', '109', '567', '131', '626');
