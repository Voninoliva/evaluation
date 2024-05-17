using System;
using System.Collections.Generic;
using btp.Models.Csv;
using Microsoft.EntityFrameworkCore;

namespace btp.Models.Data;

public partial class BtpContext : DbContext
{
    public BtpContext()
    {
    }

    public BtpContext(DbContextOptions<BtpContext> options)
        : base(options)
    {
    }
    public virtual DbSet<PaiementCsv> PaiementCsvs { get; set; }
    public virtual DbSet<DevisCsv> DevisCsvs { get; set; }
    public virtual DbSet<MaisonTravaux> MaisonTravauxes { get; set; }
    public virtual DbSet<Btp> Btps { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<DetailDevi> DetailDevis { get; set; }

    public virtual DbSet<Devi> Devis { get; set; }

    public virtual DbSet<PaiementDevi> PaiementDevis { get; set; }

    public virtual DbSet<Travaux> Travauxes { get; set; }

    public virtual DbSet<TravauxDesMaison> TravauxDesMaisons { get; set; }

    public virtual DbSet<TypeFinition> TypeFinitions { get; set; }

    public virtual DbSet<TypeMaison> TypeMaisons { get; set; }

    public virtual DbSet<TypeTravaux> TypeTravauxes { get; set; }

    public virtual DbSet<Unite> Unites { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("name=mot");
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();
        } 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Btp>(entity =>
        {
            entity.HasKey(e => e.Idbtp).HasName("btp_pkey");

            entity.ToTable("btp");

            entity.Property(e => e.Idbtp).HasColumnName("idbtp");
            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .HasColumnName("email");
            entity.Property(e => e.Mdp)
                .HasMaxLength(900)
                .HasColumnName("mdp");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Idclient).HasName("clients_pkey");

            entity.ToTable("clients");

            entity.Property(e => e.Idclient).HasColumnName("idclient");
            entity.Property(e => e.Numero)
                .HasMaxLength(500)
                .HasColumnName("numero");
        });

        modelBuilder.Entity<DetailDevi>(entity =>
        {
            entity.HasKey(e => e.IddetailDevis).HasName("detail_devis_pkey");

            entity.ToTable("detail_devis");

            entity.Property(e => e.IddetailDevis).HasColumnName("iddetail_devis");
            entity.Property(e => e.Designation)
                .HasMaxLength(300)
                .HasColumnName("designation");
            entity.Property(e => e.Duree)
                .HasDefaultValueSql("0")
                .HasColumnName("duree");
            entity.Property(e => e.Iddevis).HasColumnName("iddevis");
            entity.Property(e => e.Idtravaux).HasColumnName("idtravaux");
            entity.Property(e => e.Idunite).HasColumnName("idunite");
            entity.Property(e => e.Pu)
                .HasDefaultValueSql("0")
                .HasColumnName("pu");
            entity.Property(e => e.Quantite)
                .HasDefaultValueSql("0")
                .HasColumnName("quantite");
            entity.Property(e => e.Total)
                .HasDefaultValueSql("0")
                .HasColumnName("total");

            entity.HasOne(d => d.IddevisNavigation).WithMany(p => p.DetailDevis)
                .HasForeignKey(d => d.Iddevis)
                .HasConstraintName("detail_devis_iddevis_fkey");

            entity.HasOne(d => d.IdtravauxNavigation).WithMany(p => p.DetailDevis)
                .HasForeignKey(d => d.Idtravaux)
                .HasConstraintName("detail_devis_idtravaux_fkey");

            entity.HasOne(d => d.IduniteNavigation).WithMany(p => p.DetailDevis)
                .HasForeignKey(d => d.Idunite)
                .HasConstraintName("detail_devis_idunite_fkey");
        });

        modelBuilder.Entity<Devi>(entity =>
        {
            entity.HasKey(e => e.Iddevis).HasName("devis_pkey");

            entity.ToTable("devis");

            entity.Property(e => e.Iddevis).HasColumnName("iddevis");
            entity.Property(e => e.DateDebutTravaux).HasColumnName("date_debut_travaux");
            entity.Property(e => e.DateInsertion)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("date_insertion");
            entity.Property(e => e.Duree)
                .HasDefaultValueSql("0")
                .HasColumnName("duree");
            entity.Property(e => e.Idclient).HasColumnName("idclient");
            entity.Property(e => e.Idtypefinition).HasColumnName("idtypefinition");
            entity.Property(e => e.Idtypemaison).HasColumnName("idtypemaison");
            entity.Property(e => e.Lieu)
                .HasMaxLength(200)
                .HasColumnName("lieu");
            entity.Property(e => e.MontantTotal)
                .HasDefaultValueSql("0")
                .HasColumnName("montant_total");
            entity.Property(e => e.MontantTotalTravaux)
                .HasDefaultValueSql("0")
                .HasColumnName("montant_total_travaux");
            entity.Property(e => e.RefDevis)
                .HasMaxLength(200)
                .HasColumnName("ref_devis");
            entity.Property(e => e.TauxFinition).HasColumnName("taux_finition");

            entity.HasOne(d => d.IdclientNavigation).WithMany(p => p.Devis)
                .HasForeignKey(d => d.Idclient)
                .HasConstraintName("devis_idclient_fkey");

            entity.HasOne(d => d.IdtypefinitionNavigation).WithMany(p => p.Devis)
                .HasForeignKey(d => d.Idtypefinition)
                .HasConstraintName("devis_idtypefinition_fkey");

            entity.HasOne(d => d.IdtypemaisonNavigation).WithMany(p => p.Devis)
                .HasForeignKey(d => d.Idtypemaison)
                .HasConstraintName("devis_idtypemaison_fkey");
        });

        modelBuilder.Entity<PaiementDevi>(entity =>
        {
            entity.HasKey(e => e.Idpaiementdevis).HasName("paiement_devis_pkey");

            entity.ToTable("paiement_devis");

            entity.Property(e => e.Idpaiementdevis).HasColumnName("idpaiementdevis");
            entity.Property(e => e.DateInsertion)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("date_insertion");
            entity.Property(e => e.DatePaiement).HasColumnName("date_paiement");
            entity.Property(e => e.DatePrevue).HasColumnName("date_prevue");
            entity.Property(e => e.Iddevis).HasColumnName("iddevis");
            entity.Property(e => e.Montant).HasColumnName("montant");
            entity.Property(e => e.RefPaiement)
                .HasMaxLength(200)
                .HasColumnName("ref_paiement");

            entity.HasOne(d => d.IddevisNavigation).WithMany(p => p.PaiementDevis)
                .HasForeignKey(d => d.Iddevis)
                .HasConstraintName("paiement_devis_iddevis_fkey");
        });

        modelBuilder.Entity<Travaux>(entity =>
        {
            entity.HasKey(e => e.Idtravaux).HasName("travaux_pkey");

            entity.ToTable("travaux");

            entity.Property(e => e.Idtravaux).HasColumnName("idtravaux");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("date");
            entity.Property(e => e.Designation)
                .HasMaxLength(300)
                .HasColumnName("designation");
            entity.Property(e => e.Idtypetravaux).HasColumnName("idtypetravaux");
            entity.Property(e => e.Idunite).HasColumnName("idunite");
            entity.Property(e => e.Numero)
                .HasMaxLength(200)
                .HasColumnName("numero");
            entity.Property(e => e.Pu)
                .HasDefaultValueSql("0")
                .HasColumnName("pu");

            entity.HasOne(d => d.IdtypetravauxNavigation).WithMany(p => p.Travauxes)
                .HasForeignKey(d => d.Idtypetravaux)
                .HasConstraintName("travaux_idtypetravaux_fkey");

            entity.HasOne(d => d.IduniteNavigation).WithMany(p => p.Travauxes)
                .HasForeignKey(d => d.Idunite)
                .HasConstraintName("travaux_idunite_fkey");
        });

        modelBuilder.Entity<TravauxDesMaison>(entity =>
        {
            entity.HasKey(e => e.Idtravauxdesmaisons).HasName("travaux_des_maisons_pkey");

            entity.ToTable("travaux_des_maisons");

            entity.Property(e => e.Idtravauxdesmaisons).HasColumnName("idtravauxdesmaisons");
            entity.Property(e => e.Designation)
                .HasMaxLength(300)
                .HasColumnName("designation");
            entity.Property(e => e.Duree)
                .HasDefaultValueSql("0")
                .HasColumnName("duree");
            entity.Property(e => e.Idtravaux).HasColumnName("idtravaux");
            entity.Property(e => e.Idtypemaison).HasColumnName("idtypemaison");
            entity.Property(e => e.Idunite).HasColumnName("idunite");
            entity.Property(e => e.Pu)
                .HasDefaultValueSql("0")
                .HasColumnName("pu");
            entity.Property(e => e.Quantite)
                .HasDefaultValueSql("0")
                .HasColumnName("quantite");
            entity.Property(e => e.Total)
                .HasDefaultValueSql("0")
                .HasColumnName("total");

            entity.HasOne(d => d.IdtravauxNavigation).WithMany(p => p.TravauxDesMaisons)
                .HasForeignKey(d => d.Idtravaux)
                .HasConstraintName("travaux_des_maisons_idtravaux_fkey");

            entity.HasOne(d => d.IdtypemaisonNavigation).WithMany(p => p.TravauxDesMaisons)
                .HasForeignKey(d => d.Idtypemaison)
                .HasConstraintName("travaux_des_maisons_idtypemaison_fkey");

            entity.HasOne(d => d.IduniteNavigation).WithMany(p => p.TravauxDesMaisons)
                .HasForeignKey(d => d.Idunite)
                .HasConstraintName("travaux_des_maisons_idunite_fkey");
        });

        modelBuilder.Entity<TypeFinition>(entity =>
        {
            entity.HasKey(e => e.Idtypefinition).HasName("type_finitions_pkey");

            entity.ToTable("type_finitions");

            entity.Property(e => e.Idtypefinition).HasColumnName("idtypefinition");
            entity.Property(e => e.AugmentationPourcentage).HasColumnName("augmentation_pourcentage");
            entity.Property(e => e.NomFinition)
                .HasMaxLength(200)
                .HasColumnName("nom_finition");
        });

        modelBuilder.Entity<TypeMaison>(entity =>
        {
            entity.HasKey(e => e.Idtypemaison).HasName("type_maisons_pkey");

            entity.ToTable("type_maisons");

            entity.Property(e => e.Idtypemaison).HasColumnName("idtypemaison");
            entity.Property(e => e.Descri)
                .HasMaxLength(500)
                .HasColumnName("descri");
            entity.Property(e => e.Duree)
                .HasDefaultValueSql("0")
                .HasColumnName("duree");
            entity.Property(e => e.NomMaison)
                .HasMaxLength(200)
                .HasColumnName("nom_maison");
            entity.Property(e => e.Surface).HasColumnName("surface");
        });

        modelBuilder.Entity<TypeTravaux>(entity =>
        {
            entity.HasKey(e => e.Idtypetravaux).HasName("type_travaux_pkey");

            entity.ToTable("type_travaux");

            entity.Property(e => e.Idtypetravaux).HasColumnName("idtypetravaux");
            entity.Property(e => e.NomTravaux)
                .HasMaxLength(300)
                .HasColumnName("nom_travaux");
            entity.Property(e => e.NumeroType)
                .HasMaxLength(200)
                .HasColumnName("numero_type");
        });

        modelBuilder.Entity<Unite>(entity =>
        {
            entity.HasKey(e => e.Idunite).HasName("unites_pkey");

            entity.ToTable("unites");

            entity.Property(e => e.Idunite).HasColumnName("idunite");
            entity.Property(e => e.NomUnite)
                .HasMaxLength(200)
                .HasColumnName("nom_unite");
        });

         modelBuilder.Entity<MaisonTravaux>(entity =>
        {
            entity.HasKey(e => e.IdmaisonTravaux).HasName("maison_travaux_pkey");

            entity.ToTable("maison_travaux");

            entity.Property(e => e.IdmaisonTravaux).HasColumnName("idmaison_travaux");
            entity.Property(e => e.CodeTravaux)
                .HasMaxLength(100)
                .HasColumnName("code_travaux");
            entity.Property(e => e.Description)
                .HasMaxLength(300)
                .HasColumnName("description");
            entity.Property(e => e.DureeTravaux).HasColumnName("duree_travaux");
            entity.Property(e => e.Pu).HasColumnName("pu");
            entity.Property(e => e.Quantite).HasColumnName("quantite");
            entity.Property(e => e.Surface).HasColumnName("surface");
            entity.Property(e => e.TypeMaison)
                .HasMaxLength(200)
                .HasColumnName("type_maison");
            entity.Property(e => e.TypeTravaux)
                .HasMaxLength(300)
                .HasColumnName("type_travaux");
            entity.Property(e => e.Unite)
                .HasMaxLength(100)
                .HasColumnName("unite");
        });
        modelBuilder.Entity<DevisCsv>(entity =>
        {
            entity.HasKey(e => e.IdDevisCsv).HasName("devis_csv_pkey");

            entity.ToTable("devis_csv");

            entity.Property(e => e.IdDevisCsv).HasColumnName("id_devis_csv");
            entity.Property(e => e.Client)
                .HasMaxLength(200)
                .HasColumnName("client");
            entity.Property(e => e.DateDebut).HasColumnName("date_debut");
            entity.Property(e => e.DateDevis).HasColumnName("date_devis");
            entity.Property(e => e.Finition)
                .HasMaxLength(200)
                .HasColumnName("finition");
            entity.Property(e => e.Lieu)
                .HasMaxLength(200)
                .HasColumnName("lieu");
            entity.Property(e => e.RefDevis)
                .HasMaxLength(200)
                .HasColumnName("ref_devis");
            entity.Property(e => e.TauxFinition).HasColumnName("taux_finition");
            entity.Property(e => e.TypeMaison)
                .HasMaxLength(200)
                .HasColumnName("type_maison");
        });

         modelBuilder.Entity<PaiementCsv>(entity =>
        {
            entity.HasKey(e => e.IdpaiementCsv).HasName("paiement_csv_pkey");

            entity.ToTable("paiement_csv");

            entity.Property(e => e.IdpaiementCsv).HasColumnName("idpaiement_csv");
            entity.Property(e => e.DatePaiement).HasColumnName("date_paiement");
            entity.Property(e => e.Montant).HasColumnName("montant");
            entity.Property(e => e.RefDevis)
                .HasMaxLength(200)
                .HasColumnName("ref_devis");
            entity.Property(e => e.RefPaiement)
                .HasMaxLength(200)
                .HasColumnName("ref_paiement");
        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
