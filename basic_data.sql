-- -------------------------------------------------------------
-- USE THIS WHEN YOU HAVE JUST INSERTED THE EMPTY DATABASE MODEL
-- -------------------------------------------------------------

USE `reuzze`;

-- -----------------------------------------------------
-- Table `reuzze`.`regions`
-- -----------------------------------------------------

INSERT INTO regions (region_name)
VALUES 
('West-Vlaanderen'),
('Oost-Vlaanderen'),
('Antwerpen'),
('Vlaams-Brabant'),
('Limburg');

-- -----------------------------------------------------
-- Table `reuzze`.`roles`
-- -----------------------------------------------------

INSERT INTO roles (role_name, role_description)
VALUES 
('Administrator', 'Admin of the webapplication.'),
('User', 'User of the webapplication.'),
('Guest', 'Guest of the webapplication.');

-- -----------------------------------------------------
-- Table `reuzze`.`categories`
-- -----------------------------------------------------

INSERT INTO categories (category_id, category_name, category_description, category_parentid)
VALUES 
('1', "Auto's & Motoren", "Auto's & Motoren", NULL),
('2', "Auto - Accessoires", "Auto - Accessoires", '1'),
('3', "Auto - Onderdelen & Wisselstuk", "Auto - Onderdelen & Wisselstuk", '1'),
('4', "Auto - Hifi & Navigatie", "Auto - Hifi & Navigatie", '1'),
('5', "Auto - Tuning & Styling", "Auto - Tuning & Styling", '1'),
('6', "Beauty", "Beauty", NULL),
('7', "Make-up", "Make-up", '6'),
('8', "Parfum", "Parfum", '6'),
('9', "Gezondheid & Welzijn", "Gezondheid & Welzijn", '6'),
('10', "Sieraden & Horloges", "Sieraden & Horloges", '6'),
('11', "Boeken, Films & Muziek", "Boeken, Films & Muziek", NULL),
('12', "Boeken & Strips", "Boeken & Strips", '11'),
('13', "Dvd's, Video's & Films", "Dvd's, Video's & Films", '11'),
('14', "Muziek & Instrumenten", "Muziek & Instrumenten", '11'),
('15', "Elektronica", "Elektronica", NULL),
('16', "Computers & Tablets", "Computers & Tablets", '15'),
('17', "Smartphones & Mobiele telefonie", "Smartphones & Mobiele telefonie", '15'),
('18', "Tv, Audio & Video", "Tv, Audio & Video", '15'),
('19', "Fotografie & Camera's", "Fotografie & Camera's", '15'),
('20', "Huis & Tuin", "Huis & Tuin", NULL),
('21', "Meubels", "Meubels", '20'),
('22', "Tuin & Terras", "Tuin & Terras", '20'),
('23', "Woondecoratie & Design", "Woondecoratie & Design", '20'),
('24', "Mode", "Mode", NULL),
('25', "Dameskleding", "Dameskleding", '24'),
('26', "Damesschoenen", "Damesschoenen", '24'),
('27', "Herenkleding", "Herenkleding", '24'),
('28', "Kinderkleding meisjes", "Kinderkleding meisjes", '24'),
('29', "Overige", "Overige", NULL),
('30', "Business & Industrie", "Business & Industrie", '29'),
('31', "Immobiliën", "Immobiliën", '29'),
('32', "Verzamelen", "Verzamelen", NULL),
('33', "Kunst & Antiek", "Kunst & Antiek", '32'),
('34', "Munten & Bankbiljetten", "Munten & Bankbiljetten", '32'),
('35', "Postzegels", "Postzegels", '32'),
('36', "Verzamelen", "Verzamelen", '32'),
('37', "Vrije Tijd", "Vrije Tijd", NULL),
('38', "Doe-Het-Zelf & Hobby", "Doe-Het-Zelf & Hobby", '37'),
('39', "Games & Consoles", "Games & Consoles", '37'),
('40', "Sport & Fietsen", "Sport & Fietsen", '37'),
('41', "Tickets & Reizen", "Tickets & Reizen", '37');