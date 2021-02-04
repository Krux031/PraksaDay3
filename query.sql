Create table grad(
pbr int Primary Key,
naziv varchar(30) not null
);

Create table stanovnici(
jmbg int,
ime varchar(30),
prezime varchar(30),
pbr int,
spol char(1),
constraint primarni Primary key(jmbg),
constraint strani Foreign key(pbr) references grad(pbr),
constraint ch_spol Check(spol in ('M', 'Z'))
);