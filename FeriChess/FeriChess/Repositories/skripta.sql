SET FOREIGN_KEY_CHECKS=0;
-- -----------------------------------------------------
-- Schema Sah
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema Sah
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `Sah` DEFAULT CHARACTER SET utf8 ;
USE `Sah` ;

-- -----------------------------------------------------
-- Table `Sah`.`Igralec`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Sah`.`Igralec` (
  `IDigralec` INT NOT NULL AUTO_INCREMENT,
  `Vzdevek` VARCHAR(15) NOT NULL,
  `Geslo` VARCHAR(255) NOT NULL,
  `DanRegistracije` DATE NOT NULL,
  `SteviloKoncanihIger` INT NULL DEFAULT 0,
  `RojstniDan` DATE NULL,
  PRIMARY KEY (`IDigralec`),
  UNIQUE INDEX `Vzdevek_UNIQUE` (`Vzdevek` ASC))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Sah`.`Igra`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Sah`.`Igra` (
  `IDigre` INT NOT NULL AUTO_INCREMENT,
  `Igralec0` INT NOT NULL,
  `Igralec1` INT NOT NULL,
  `Inc` INT NULL DEFAULT 0,
  `Cas` TIME NULL,
  `KoncnoStanje` VARCHAR(255) NULL DEFAULT NULL,
  `Zmagovalec` TINYINT(1) NULL DEFAULT NULL,
  PRIMARY KEY (`IDigre`),
  INDEX `fk_Igra_Igralec1_idx` (`Igralec0` ASC),
  INDEX `fk_Igra_Igralec2_idx` (`Igralec1` ASC),
  CONSTRAINT `fk_Igra_Igralec1`
    FOREIGN KEY (`Igralec0`)
    REFERENCES `Sah`.`Igralec` (`IDigralec`)
    ,
  CONSTRAINT `fk_Igra_Igralec2`
    FOREIGN KEY (`Igralec1`)
    REFERENCES `Sah`.`Igralec` (`IDigralec`)
    )
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Sah`.`Poteze`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Sah`.`Poteze` (
  `ZaporenaStevilka` INT NOT NULL AUTO_INCREMENT,
  `Igra_IDigre` INT NOT NULL,
  `Poteza` VARCHAR(10) NOT NULL,
  `CasPoteze` TIME NOT NULL,
  PRIMARY KEY (`ZaporenaStevilka`),
  CONSTRAINT `fk_Poteza_Igra1`
    FOREIGN KEY (`Igra_IDigre`)
    REFERENCES `Sah`.`Igra` (`IDigre`)
    )
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Sah`.`Sporocilo`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Sah`.`Sporocilo` (
  `IDSporocilo` INT NOT NULL AUTO_INCREMENT,
  `Igralec_IDigralec` INT NOT NULL,
  `Igra_IDigre` INT NOT NULL,
  `Vsebina` VARCHAR(255) NOT NULL,
  `Cas` DATETIME NOT NULL,
  PRIMARY KEY (`IDSporocilo`),
  INDEX `fk_Sporocilo_Igralec1_idx` (`Igralec_IDigralec` ASC),
  INDEX `fk_Sporocilo_Igra1_idx` (`Igra_IDigre` ASC),
  CONSTRAINT `fk_Sporocilo_Igralec1`
    FOREIGN KEY (`Igralec_IDigralec`)
    REFERENCES `Sah`.`Igralec` (`IDigralec`)
    ,
  CONSTRAINT `fk_Sporocilo_Igra1`
    FOREIGN KEY (`Igra_IDigre`)
    REFERENCES `Sah`.`Igra` (`IDigre`)
    
    )
ENGINE = InnoDB;