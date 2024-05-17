create database btp;
\c btp;
create table clients(
    idclient serial primary key,
    numero varchar(500)
);
create table btp(
    idbtp serial primary key,
    email varchar(250),
    mdp varchar(900)
);
insert into btp (email,mdp) values('a@gmail.com','androany');
create table type_finitions(
    idtypefinition serial primary key,
    nom_finition varchar(200),
    augmentation_pourcentage double precision
);
create table type_maisons(
    idtypemaison serial primary key,
    nom_maison varchar(200),
    descri varchar(500)
);
create table unites(
    idunite serial primary key,
    nom_unite varchar(200)
);
create table devis(
    iddevis serial primary key,
    idclient int references clients(idclient),
    idtypemaison int references type_maisons(idtypemaison),
    idtypefinition int references type_finitions(idtypefinition),
    taux_finition double precision,
    montant_total_travaux double precision default 0,
    date_insertion date default current_date,
    date_debut_travaux date default null,
    montant_total double precision default 0,
    duree double precision default 0
);
create table type_travaux(
    idtypetravaux serial primary key,
    nom_travaux varchar(300),
    numero_type varchar(200)
);
create table travaux(
    idtravaux serial primary key,
    idtypetravaux int references type_travaux(idtypetravaux),
    numero varchar(200),
    designation varchar(300),
    idunite int references unites(idunite),
    pu double precision default 0,
    date date default current_date
);
create table travaux_des_maisons(
    idtravauxdesmaisons serial primary key,
    idtypemaison int references type_maisons(idtypemaison),
    idtravaux int references travaux(idtravaux),
    idunite int references unites(idunite),
    quantite double precision default 0,
    pu double precision default 0,
    total double precision default 0
);
create table paiement_devis(
    idpaiementdevis serial primary key,
    iddevis int references devis(iddevis),
    date_insertion date default current_date,
    date_prevue date default null,
    date_paiement date default null,
    montant double precision
);
-- ity resaka oe ny devis efa vita tsy kitihana intsony
create table detail_devis(
    iddetail_devis serial primary key,
    iddevis int references devis(iddevis),
    idtravaux int references travaux(idtravaux),
    idunite int references unites(idunite),
    quantite double precision default 0,
    pu double precision default 0,
    total double precision default 0
);
alter table type_maisons add column duree double precision default 0;
alter table travaux_des_maisons add duree double precision default 0;
alter table detail_devis add column duree double precision default 0;
alter table travaux_des_maisons add column designation varchar(300);
alter table detail_devis add column designation varchar(300);
alter table devis add column ref_devis varchar(200);
alter table devis add column lieu varchar(200);
alter table paiement_devis add column ref_paiement varchar(200);
alter table type_maisons add column surface double precision;

