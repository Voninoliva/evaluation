select sum(pu*quantite) from travaux_des_maisons group by idtypemaison 
-- maka an le type ana travaux
 select distinct t.* from travaux tr  join type_travaux t on tr.idtypetravaux=t.idtypetravaux join detail_devis d on tr.idtravaux=d.idtravaux
 where d.iddevis=8

--  maka liste ana detaildevis
select detail_devis.* from detail_devis join travaux on detail_devis.idtravaux=travaux.idtravaux
where travaux.idtypetravaux=1 and iddevis=8


create or replace view v_monts_existed_data as(

);

select mois.month,COALESCE(montant_total,0) from mois left join ( select extract(month from devis.date_insertion) as m,sum(montant_total) as montant_total from devis
    group by extract(month from devis.date_insertion) ) as s on mois.month=s.m order by mois.month



SELECT months.month, COALESCE(SUM(devis.montant_total), 0) AS montant_total
FROM (
    SELECT 1 AS month UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4 UNION ALL SELECT 5 UNION ALL
    SELECT 6 UNION ALL SELECT 7 UNION ALL SELECT 8 UNION ALL SELECT 9 UNION ALL SELECT 10 UNION ALL SELECT 11 UNION ALL SELECT 12
) AS months
LEFT JOIN devis ON EXTRACT(MONTH FROM devis.date_insertion) = months.month
where extract(year from devis.date_insertion)=2023
GROUP BY months.month
ORDER BY months.month;


create or replace view mois as(
    SELECT 1 AS month UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4 UNION ALL SELECT 5 UNION ALL
    SELECT 6 UNION ALL SELECT 7 UNION ALL SELECT 8 UNION ALL SELECT 9 UNION ALL SELECT 10 UNION ALL SELECT 11 UNION ALL SELECT 12
);