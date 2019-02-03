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
    descrip TEXT,
    bonus FLOAT DEFAULT 0,
    PRIMARY KEY (id)
);




-- Create the payments table.
CREATE TABLE payments
(
	stuff_id INT NOT NULL,
    amount FLOAT DEFAULT 0,
    date_from DATE NOT NULL,
    date_to DATE NOT NULL,
    FOREIGN KEY (stuff_id) REFERENCES stuff (id)
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



-- Create the has_gifts table.
CREATE TABLE has_gifts
(
	client_id INT NOT NULL,
    gift_id CHAR(20) NOT NULL,
    FOREIGN KEY (client_id) REFERENCES clients (id),
    FOREIGN KEY (gift_id) REFERENCES gifts (giftname)
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
    FOREIGN KEY (service_name) REFERENCES services (servicename)
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







-- Create Procedure add_client
DELIMITER $$
CREATE PROCEDURE add_client(IN client_name CHAR(20), 
                            IN client_lname CHAR(20), 
						    IN client_email CHAR(255), 
						    IN client_phone CHAR(10),
                            IN client_disc TEXT,
                            IN client_bonus FLOAT)
	BEGIN
		INSERT INTO clients (firstname, lastname, email, phone, descrip, bonus) 
        values (client_name, client_lname, client_email, client_phone, client_disc, client_bonus);
	END$$
DELIMITER ;



-- Create Procedure edit_client
DELIMITER $$
CREATE PROCEDURE edit_client(
                             IN key_int INT,
							 IN client_name CHAR(20), 
                             IN client_lname CHAR(20), 
						     IN client_email CHAR(255), 
						     IN client_phone CHAR(10),
                             IN client_disc TEXT,
                             IN client_bonus FLOAT)
	BEGIN
		UPDATE clients
        SET firstname = client_name, lastname = client_lname, email = client_email, phone = client_phone, descrip = client_disc, bonus = client_bonus
        WHERE id = key_int;
	END$$
DELIMITER ;



-- Create Procedure delete_client
DELIMITER $$
CREATE PROCEDURE delete_client(IN key_int INT)
	BEGIN
		DELETE FROM clients
		WHERE id = key_int;
	END$$
DELIMITER ;



-- Create Procedure get_client
DELIMITER $$
CREATE PROCEDURE get_client(IN key_int INT)
	BEGIN
		SELECT *
        FROM clients
        WHERE id = key_int;
	END$$
DELIMITER ;



-- Create Procedure get_client_by_lastname
DELIMITER $$
CREATE PROCEDURE get_client_by_lastname(IN key_lname CHAR(20))
	BEGIN
		SELECT *
        FROM clients
        WHERE lastname = key_lname;
	END$$
DELIMITER ;


-- Create Procedure get_client_by_phone
DELIMITER $$
CREATE PROCEDURE get_client_by_phone(IN key_phone CHAR(10))
	BEGIN
		SELECT *
        FROM clients
        WHERE phone = key_phone;
	END$$
DELIMITER ;






-- Create Procedure add_service
DELIMITER $$
CREATE PROCEDURE add_service(IN service_name CHAR(20), 
                             IN service_price FLOAT)
	BEGIN
		INSERT INTO services
        values (service_name, service_price);
	END$$
DELIMITER ;



-- Create Procedure edit_service
DELIMITER $$
CREATE PROCEDURE edit_service(IN service_key CHAR(20),
							  IN service_name CHAR(20), 
						      IN service_price FLOAT)
	BEGIN
		UPDATE services
        SET servicename   = service_name, price = service_price
        WHERE servicename = service_key;
	END$$
DELIMITER ;



-- Create Procedure delete_service
DELIMITER $$
CREATE PROCEDURE delete_service(IN service_key CHAR(20))
	BEGIN
		DELETE FROM services
		WHERE servicename = service_key;
	END$$
DELIMITER ;



-- Create Procedure get_service_keys
DELIMITER $$
CREATE PROCEDURE get_service_keys()
	BEGIN
		SELECT servicename
        FROM services;
	END$$
DELIMITER ;



-- Create Procedure get_service
DELIMITER $$
CREATE PROCEDURE get_service(IN service_key CHAR(20))
	BEGIN
		SELECT *
        FROM services
        WHERE servicename = service_key;
	END$$
DELIMITER ;






-- Create Procedure add_gift
DELIMITER $$
CREATE PROCEDURE add_gift(IN gift_name CHAR(20), 
						  IN gift_price FLOAT)
	BEGIN
		INSERT INTO gifts
        values (gift_name, gift_price);
	END$$
DELIMITER ;



-- Create Procedure edit_gift
DELIMITER $$
CREATE PROCEDURE edit_gift(IN gift_key CHAR(20),
						   IN gift_name CHAR(20), 
						   IN gift_price FLOAT)
	BEGIN
		UPDATE gifts
        SET giftname   = gift_name, price = gift_price
        WHERE giftname = gift_key;
	END$$
DELIMITER ;



-- Create Procedure delete_gift
DELIMITER $$
CREATE PROCEDURE delete_gift(IN gift_key CHAR(20))
	BEGIN
		DELETE FROM gifts
		WHERE giftname = gift_key;
	END$$
DELIMITER ;



-- Create Procedure get_gift_keys
DELIMITER $$
CREATE PROCEDURE get_gift_keys()
	BEGIN
		SELECT giftname
        FROM gifts;
	END$$
DELIMITER ;



-- Create Procedure get_gift
DELIMITER $$
CREATE PROCEDURE get_gift(IN gift_key CHAR(20))
	BEGIN
		SELECT *
        FROM gifts
        WHERE giftname = gift_key;
	END$$
DELIMITER ;









-- Create Procedure set_payment
DELIMITER $$
CREATE PROCEDURE set_payment(IN in_stuff_id INT, 
							 IN in_amount FLOAT, 
							 IN in_date_from DATE, 
							 IN in_date_to DATE)
	BEGIN
    
		IF EXISTS(SELECT * FROM payments WHERE stuff_id = in_stuff_id AND date_from = in_date_from AND date_to = in_date_to) THEN
			UPDATE payments
            SET amount     = amount + in_amount
            WHERE stuff_id = in_stuff_id AND date_from = in_date_from AND date_to = in_date_to;
            
		ELSE
			INSERT INTO payments VALUES (in_stuff_id, in_amount, in_date_from, in_date_to);
		END IF;
    
	END$$
DELIMITER ;





-- Create Procedure add_hasgift
DELIMITER $$
CREATE PROCEDURE add_hasgift(IN in_client_id INT, IN in_gift_id CHAR(20))
	BEGIN
		INSERT INTO has_gifts
        VALUES (in_client_id, in_gift_id);
        
        UPDATE clients
        SET bonus = bonus - (SELECT price FROM gifts WHERE giftname = in_gift_id)
        WHERE id = in_client_id;
	END$$
DELIMITER ;


/*
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
    FOREIGN KEY (service_name) REFERENCES services (servicename)
);
*/

-- Create Procedure add_appointment
DELIMITER $$
CREATE PROCEDURE add_appointment(IN in_stuff_id INT,
								 IN in_client_id INT,
								 IN in_date DATE,
								 IN in_service CHAR(20),
                                 IN in_payment BOOL,
								 IN in_row INT,
								 IN in_name CHAR(20), 
								 IN in_lname CHAR(20), 
								 IN in_phone CHAR(10) )
	BEGIN
           
           
           
           
		-- The appointment Exists.
		IF EXISTS (SELECT * FROM appointments WHERE stuff_id = in_stuff_id AND date_in = in_date AND row_index = in_row) THEN
        
        
			-- Increase Client Bonus.
			IF (SELECT payment_done FROM appointments WHERE stuff_id = in_stuff_id AND date_in = in_date AND row_index = in_row) = FALSE THEN
				IF in_payment = TRUE AND in_client_id > 0 THEN
					UPDATE clients
                    SET bonus = bonus + (SELECT price FROM services WHERE servicename = in_service)
                    WHERE id  = in_client_id;
				END IF;
                
                
			-- Decrease Client Bonus.
			ELSE
				IF in_payment = FALSE AND in_client_id > 0 THEN
					UPDATE clients
                    SET bonus = bonus - (SELECT price FROM services WHERE servicename = in_service)
                    WHERE id  = in_client_id;
				END IF;
			END IF;
					
				
				
			-- Update the appointments data.
			UPDATE appointments
            SET client_id  = in_client_id, client_phone = in_phone, service_name = in_service, client_name = in_name, client_lastname = in_lname, payment_done = in_payment
            WHERE stuff_id = in_stuff_id AND date_in = in_date AND row_index = in_row;
         
         
         
         
         
         
		-- The appointment does not exists.
		ELSE
			INSERT INTO appointments 
            VALUES (in_row, in_date, in_phone, in_stuff_id, in_service, in_client_id, in_name, in_lname, in_payment);
		END IF;
	END$$
DELIMITER ;