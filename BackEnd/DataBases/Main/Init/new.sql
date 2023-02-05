CREATE SCHEMA IF NOT EXISTS main;

CREATE  TABLE main.users ( 
	id                   integer  NOT NULL  ,
	login                char(30)  NOT NULL  ,
	passwordhash         text  NOT NULL  ,
	CONSTRAINT pk_users PRIMARY KEY ( id )
 );

