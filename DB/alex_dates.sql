CREATE DATABASE alex_dates;

USE alex_dates;


-- Create the stuff table.
CREATE TABLE stuff
(
	id INT UNIQUE NOT NULL AUTO_INCREMENT,
    firstname CHAR(20) NOT NULL,
    lastname CHAR(20) NOT NULL,
    email CHAR(255) DEFAULT "",
    phone CHAR(10) DEFAULT "",
    PRIMARY KEY (firstname, lastname)
);



-- Create the clients table.
CREATE TABLE clients
(
	id INT AUTO_INCREMENT NOT NULL,
    firstname CHAR(20) NOT NULL,
    lastname CHAR(20) NOT NULL,
    email CHAR(255) DEFAULT "",
    phone CHAR(10) DEFAULT "",
    descip TEXT,
    bonus FLOAT DEFAULT 0,
    PRIMARY KEY (id, firstname, lastname)
);



-- Create the gifts table.
CREATE TABLE gifts
(
	giftname CHAR(20) PRIMARY KEY,
    price FLOAT NOT NULL
);


-- Create the services table.
CREATE TABLE services
(
	servicename CHAR(20) PRIMARY KEY,
    price FLOAT NOT NULL
);


-- Create the appointment_incomes table.
CREATE TABLE appointment_incomes
(
	date_in DATE NOT NULL,
    total FLOAT NOT NULL
);



-- Create the last_time_used table.
CREATE TABLE last_time_used
(
	date_in DATE NOT NULL
);



-- Create the appointments table.
CREATE TABLE appointments
(
	row_index INT NOT NULL,
    date_in DATE NOT NULL,
    client_phone CHAR(10) DEFAULT "",
    stuff_id INT NOT NULL,
    service_name CHAR(20) NOT NULL,
    client_id INT DEFAULT -1,
    client_name CHAR(20) DEFAULT "",
    client_lastname CHAR(20) DEFAULT "",
    payment_done BOOL DEFAULT FALSE,
    FOREIGN KEY (stuff_id) REFERENCES stuff (id),
    FOREIGN KEY (client_id) REFERENCES clients (id)
);








-- Create Procedure add_stuff
DELIMITER $$
CREATE PROCEDURE add_stuff(IN stuff_name CHAR(20), 
                           IN stuff_lname CHAR(20), 
						   IN stuff_email CHAR(255), 
						   IN stuff_phone CHAR(10))
	BEGIN
		INSERT INTO stuff (firstname, lastname, email, phone) 
        values (stuff_name, stuff_lname, stuff_email, stuff_phone);
	END$$
DELIMITER ;



-- Create Procedure edit_stuff
DELIMITER $$
CREATE PROCEDURE edit_stuff(
						   IN key_name CHAR(20),
                           IN key_lname CHAR(20),
						   IN stuff_name CHAR(20), 
                           IN stuff_lname CHAR(20), 
						   IN stuff_email CHAR(255), 
						   IN stuff_phone CHAR(10))
	BEGIN
		UPDATE stuff
        SET firstname = stuff_name, lastname = stuff_lname, email = stuff_email, phone = stuff_phone
        WHERE firstname = key_name AND lastname = key_lname;
	END$$
DELIMITER ;



-- Create Procedure delete_stuff
DELIMITER $$
CREATE PROCEDURE delete_stuff(IN key_name CHAR(20), IN key_lname CHAR(20))
	BEGIN
		DELETE FROM stuff
		WHERE firstname = key_name AND lastname = key_lname;
	END$$
DELIMITER ;



-- Create Procedure get_stuff_keys
DELIMITER $$
CREATE PROCEDURE get_stuff_keys()
	BEGIN
		SELECT firstname, lastname
        FROM stuff;
	END$$
DELIMITER ;



-- Create Procedure get_stuff
DELIMITER $$
CREATE PROCEDURE get_stuff(IN key_name CHAR(20), IN key_lname CHAR(20))
	BEGIN
		SELECT *
        FROM stuff
        WHERE firstname = key_name AND lastname = key_lname; 
	END$$
DELIMITER ;


