COSO PARA QUE NO HAYA ACCIDENTES JEJE

CREATE TABLE user_account (
	id SERIAL PRIMARY KEY,
	name varchar(20) NOT NULL UNIQUE
-- 	is_deleted BOOLEAN NOT NULL
);

CREATE TABLE user_nickname(
	id SERIAL PRIMARY KEY,
	description varchar(20) NOT NULL UNIQUE,
	id_user INT NOT NULL REFERENCES user_account (id)
-- 	is_deleted BOOLEAN NOT NULL
);

-----------------------------------------

drop table user_nickname;
drop table user_account;

-----------------------------------------

select * from user_account;
select * from user_nickname;

-----------------------------------------

delete from user_nickname;
delete from user_account;
