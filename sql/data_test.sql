insert into type_finitions(nom_finition,augmentation_pourcentage) values
('standard',14);
('gold',2),
('premium',14),
('VIP',25);
insert into type_maisons (nom_maison,descri) values
('maison familiale','ideale pour 5 personnes'),
('maison de vacances','pour toute la famille'),
('maison communautaire','pour les étudiants');

insert into unites(nom_unite) values
('m2'),
('m3'),
('fft');
insert into type_travaux(numero_type,nom_travaux) values
('000','travaux préparatoire'),
('100','travaux de terrassement'),
('200','travaux en infrastructure');

insert into travaux(idtypetravaux,numero,designation,idunite,pu)values
(1,'001','mur de soutenement',2,190000),
(2,'101','décapage des terrains',1,3072.87),
(2,'102','dressage de plateforme',1,3736);

insert into travaux_des_maisons(idtypemaison,idtravaux,idunite,quantite,pu,total) values
(1,1,2,2.5,190000,475000),
(2,1,2,3.7,190000,703000),
(2,2,1,1,3072.87,3072.87),
(3,3,2,6,3736,22416),
(3,2,1,1,3072.87,3072.87);
insert into type_maisons (nom_maison,descri) values
('maison A','ideale ';)


