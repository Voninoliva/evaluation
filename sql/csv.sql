create table maison_travaux(
    idmaison_travaux serial primary key,
    type_maison varchar(200),
    description varchar(300),
    surface double precision,
    code_travaux varchar(100),
    type_travaux varchar(300),
    unite varchar(100),
    pu double precision,
    quantite double precision,
    duree_travaux double precision
);
create table devis_csv(
    id_devis_csv serial primary key,
    client varchar(200),--join
    ref_devis varchar(200),--
    type_maison varchar(200),--join
    finition varchar(200),--join
    taux_finition double precision,--
    date_devis date,--
    date_debut date,--
    lieu varchar(200)--
);
create table paiement_csv(
    idpaiement_csv serial primary key,
    ref_devis varchar(200),
    ref_paiement varchar(200),
    date_paiement date,
    montant double precision
);
-- ho an le mi insert  anaty tables
create or replace view v_maisons_unites as(
select * 
from maison_travaux m
join unites u on m.unite=u.nom_unite);


create or replace view v_maisons_unites_travaux as(
    select m.* ,u.nom_unite,t.numero,t.designation,t.idunite,t.date,t.idtravaux,tm.nom_maison,tm.descri,tm.idtypemaison,m.pu*quantite as total,t.numero
    from maison_travaux m 
    join unites u on m.unite=u.nom_unite
    join travaux t on m.code_travaux=t.numero 
    join type_maisons tm on m.type_maison=tm.nom_maison
);

create or replace view v_pour_devis as(
select *
from devis_csv dc
join clients c on dc.client=c.numero 
join type_maisons tm on dc.type_maison=tm.nom_maison
join type_finitions tf on dc.finition=tf.nom_finition
where tf.nom_finition=dc.finition and dc.taux_finition=tf.augmentation_pourcentage);

create or replace view v_paiement_devis as(
select p.*,d.iddevis 
from paiement_csv p
join devis  d on p.ref_devis= d.ref_devis);


