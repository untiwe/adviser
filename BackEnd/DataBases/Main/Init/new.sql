CREATE SCHEMA IF NOT EXISTS main;

CREATE  TABLE main.roles ( 
	id                   smallserial  NOT NULL  ,
	rolename             char(20)  NOT NULL  ,
	CONSTRAINT unq_roles_id UNIQUE ( id ) 
 );

CREATE  TABLE main.users ( 
	id                   serial  NOT NULL  ,
	login                char(30)  NOT NULL  ,
	passwordhash         text  NOT NULL  ,
	rolesid              smallint    ,
	CONSTRAINT pk_users PRIMARY KEY ( id )
 );

ALTER TABLE main.users ADD CONSTRAINT fk_users_roles FOREIGN KEY ( rolesid ) REFERENCES main.roles( id );

