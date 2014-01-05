SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

DROP SCHEMA IF EXISTS `reuzze` ;
CREATE SCHEMA IF NOT EXISTS `reuzze` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci ;
USE `reuzze` ;

-- -----------------------------------------------------
-- Table `reuzze`.`regions`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `reuzze`.`regions` ;

CREATE  TABLE IF NOT EXISTS `reuzze`.`regions` (
  `region_id` INT NOT NULL AUTO_INCREMENT ,
  `region_name` VARCHAR(45) NOT NULL ,
  PRIMARY KEY (`region_id`) )
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `reuzze`.`addresses`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `reuzze`.`addresses` ;

CREATE  TABLE IF NOT EXISTS `reuzze`.`addresses` (
  `address_id` INT NOT NULL AUTO_INCREMENT ,
  `address_street` VARCHAR(45) NOT NULL ,
  `address_city` VARCHAR(45) NOT NULL ,
  `address_lat` DECIMAL(18,12) NULL ,
  `address_lon` DECIMAL(18,12) NULL ,
  `address_streetnr` INT NOT NULL ,
  `region_id` INT NOT NULL ,
  PRIMARY KEY (`address_id`) ,
  INDEX `fk_addresses_regions1_idx` (`region_id` ASC) ,
  CONSTRAINT `fk_addresses_regions1`
    FOREIGN KEY (`region_id` )
    REFERENCES `reuzze`.`regions` (`region_id` )
    ON DELETE NO ACTION
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `reuzze`.`persons`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `reuzze`.`persons` ;

CREATE  TABLE IF NOT EXISTS `reuzze`.`persons` (
  `person_id` INT NOT NULL AUTO_INCREMENT ,
  `person_firstname` VARCHAR(45) NOT NULL ,
  `person_surname` VARCHAR(255) NOT NULL ,
  `person_profile` LONGTEXT NULL ,
  `person_created` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ,
  `person_modified` TIMESTAMP NULL ,
  `person_deleted` TIMESTAMP NULL ,
  `address_id` INT NOT NULL ,
  PRIMARY KEY (`person_id`) ,
  INDEX `fk_persons_addresses1_idx` (`address_id` ASC) ,
  CONSTRAINT `fk_persons_addresses1`
    FOREIGN KEY (`address_id` )
    REFERENCES `reuzze`.`addresses` (`address_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `reuzze`.`roles`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `reuzze`.`roles` ;

CREATE  TABLE IF NOT EXISTS `reuzze`.`roles` (
  `role_id` TINYINT NOT NULL AUTO_INCREMENT ,
  `role_name` VARCHAR(45) NOT NULL ,
  `role_description` VARCHAR(255) NULL ,
  `role_created` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ,
  `role_modified` TIMESTAMP NULL ,
  `role_deleted` TIMESTAMP NULL ,
  PRIMARY KEY (`role_id`) )
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `reuzze`.`users`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `reuzze`.`users` ;

CREATE  TABLE IF NOT EXISTS `reuzze`.`users` (
  `user_id` INT NOT NULL AUTO_INCREMENT ,
  `username` VARCHAR(45) NOT NULL ,
  `password` VARCHAR(255) NOT NULL ,
  `salt` VARCHAR(255) NOT NULL ,
  `user_email` VARCHAR(255) NOT NULL ,
  `user_rating` INT NOT NULL ,
  `user_created` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ,
  `user_modified` TIMESTAMP NULL ,
  `user_deleted` TIMESTAMP NULL ,
  `user_lastlogin` TIMESTAMP NULL ,
  `user_locked` TIMESTAMP NULL ,
  `person_id` INT NOT NULL ,
  `role_id` TINYINT NOT NULL ,
  PRIMARY KEY (`user_id`, `person_id`) ,
  UNIQUE INDEX `user_username_UNIQUE` (`username` ASC) ,
  UNIQUE INDEX `user_email_UNIQUE` (`user_email` ASC) ,
  INDEX `fk_users_persons1` (`person_id` ASC) ,
  INDEX `fk_users_roles1_idx` (`role_id` ASC) ,
  CONSTRAINT `fk_users_persons1`
    FOREIGN KEY (`person_id` )
    REFERENCES `reuzze`.`persons` (`person_id` )
    ON DELETE CASCADE
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_users_roles1`
    FOREIGN KEY (`role_id` )
    REFERENCES `reuzze`.`roles` (`role_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `reuzze`.`categories`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `reuzze`.`categories` ;

CREATE  TABLE IF NOT EXISTS `reuzze`.`categories` (
  `category_id` TINYINT NOT NULL AUTO_INCREMENT ,
  `category_name` VARCHAR(45) NOT NULL ,
  `category_description` VARCHAR(255) NOT NULL ,
  `category_created` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ,
  `category_modified` TIMESTAMP NULL ,
  `category_deleted` TIMESTAMP NULL ,
  `category_parentid` TINYINT NULL ,
  PRIMARY KEY (`category_id`) ,
  INDEX `fk_categories_categories1_idx` (`category_parentid` ASC) ,
  CONSTRAINT `fk_categories_categories1`
    FOREIGN KEY (`category_parentid` )
    REFERENCES `reuzze`.`categories` (`category_id` )
    ON DELETE SET NULL
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `reuzze`.`entities`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `reuzze`.`entities` ;

CREATE  TABLE IF NOT EXISTS `reuzze`.`entities` (
  `entity_id` BIGINT NOT NULL AUTO_INCREMENT ,
  `entity_title` VARCHAR(255) NOT NULL ,
  `entity_description` TEXT NOT NULL ,
  `entity_starttime` DATETIME NOT NULL ,
  `entity_endtime` DATETIME NOT NULL ,
  `entity_instantsellingprice` DECIMAL(12,2) NOT NULL ,
  `entity_shippingprice` DECIMAL(6,2) NULL ,
  `entity_condition` ENUM('New', 'Used') NOT NULL DEFAULT 'Used' ,
  `entity_views` BIGINT NULL ,
  `entity_created` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ,
  `entity_modified` TIMESTAMP NULL ,
  `entity_deleted` TIMESTAMP NULL ,
  `user_id` INT NOT NULL ,
  `region_id` INT NOT NULL ,
  `category_id` TINYINT NOT NULL ,
  PRIMARY KEY (`entity_id`) ,
  INDEX `fk_entities_users1` (`user_id` ASC) ,
  INDEX `fk_entities_countries1_idx` (`region_id` ASC) ,
  INDEX `fk_entities_categories1_idx` (`category_id` ASC) ,
  CONSTRAINT `fk_entities_users1`
    FOREIGN KEY (`user_id` )
    REFERENCES `reuzze`.`users` (`user_id` )
    ON DELETE CASCADE
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_entities_regions1`
    FOREIGN KEY (`region_id` )
    REFERENCES `reuzze`.`regions` (`region_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_entities_categories1`
    FOREIGN KEY (`category_id` )
    REFERENCES `reuzze`.`categories` (`category_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `reuzze`.`media`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `reuzze`.`media` ;

CREATE  TABLE IF NOT EXISTS `reuzze`.`media` (
  `medium_id` INT NOT NULL AUTO_INCREMENT ,
  `entity_id` BIGINT NOT NULL ,
  `medium_url` VARCHAR(255) NOT NULL ,
  `medium_type` CHAR(3) NOT NULL DEFAULT 'GNR' ,
  `medium_mimetype` VARCHAR(255) NOT NULL ,
  `medium_isexternal` TINYINT(1) NULL DEFAULT false ,
  PRIMARY KEY (`medium_id`) ,
  CONSTRAINT `fk_media_entities1`
    FOREIGN KEY (`entity_id` )
    REFERENCES `reuzze`.`entities` (`entity_id` )
    ON DELETE CASCADE
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `reuzze`.`bids`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `reuzze`.`bids` ;

CREATE  TABLE IF NOT EXISTS `reuzze`.`bids` (
  `bid_id` INT NOT NULL AUTO_INCREMENT ,
  `user_id` INT NOT NULL ,
  `entity_id` BIGINT NOT NULL ,
  `bid_amount` DECIMAL(12,2) NOT NULL ,
  `bid_date` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ,
  PRIMARY KEY (`bid_id`) ,
  INDEX `fk_bids_users1_idx` (`user_id` ASC) ,
  INDEX `fk_bids_entities1_idx` (`entity_id` ASC) ,
  CONSTRAINT `fk_bids_users1`
    FOREIGN KEY (`user_id` )
    REFERENCES `reuzze`.`users` (`user_id` )
    ON DELETE CASCADE
    ON UPDATE RESTRICT,
  CONSTRAINT `fk_bids_entities1`
    FOREIGN KEY (`entity_id` )
    REFERENCES `reuzze`.`entities` (`entity_id` )
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `reuzze`.`favorites`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `reuzze`.`favorites` ;

CREATE  TABLE IF NOT EXISTS `reuzze`.`favorites` (
  `user_id` INT NOT NULL ,
  `entity_id` BIGINT NOT NULL ,
  PRIMARY KEY (`entity_id`, `user_id`) ,
  INDEX `fk_favorites_entities1_idx` (`entity_id` ASC) ,
  CONSTRAINT `fk_favorites_users1`
    FOREIGN KEY (`user_id` )
    REFERENCES `reuzze`.`users` (`user_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_favorites_entities1`
    FOREIGN KEY (`entity_id` )
    REFERENCES `reuzze`.`entities` (`entity_id` )
    ON DELETE CASCADE
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

USE `reuzze` ;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
