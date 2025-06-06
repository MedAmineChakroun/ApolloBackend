﻿using ApolloBackend.Functions;
using ApolloBackend.Interfaces;
using ApolloBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Objets100cLib;

namespace ApolloBackend.Functions
{
    public class SynchronisationSageFunctions : ISynchronisationSage
    {
        private readonly IDocumentVente _documentVenteService;
        public readonly IDocumentVenteLigne _documentVenteLigneService;
        public readonly ProduitsFunctions _produitService;
        private readonly FamilleFunctions _familleService;
        private readonly IClients _clientService;

        public SynchronisationSageFunctions(
           IDocumentVente documentVenteService,
           IDocumentVenteLigne documentVenteLigneService,
           ProduitsFunctions produitsService,
           FamilleFunctions familleService,
           IClients clientService
           )
        {
            _documentVenteService = documentVenteService;
            _documentVenteLigneService = documentVenteLigneService;
            _produitService = produitsService;
            _familleService = familleService;
            _clientService = clientService;
        }

        private (BSCPTAApplication100c compta, BSCIALApplication100c commercial) ConnectToSage()
        {
            var basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "files");

            var compta = new BSCPTAApplication100c
            {
                Name = Path.Combine(basePath, "ALLANI.mae"),
                Loggable = { UserName = "<Administrateur>", UserPwd = "P@ssw0rd" }
            };

            var commercial = new BSCIALApplication100c
            {
                Name = Path.Combine(basePath, "ALLANI.gcm"),
                Loggable = { UserName = "<Administrateur>", UserPwd = "P@ssw0rd" },
                CptaApplication = compta
            };

            compta.Open();
            commercial.Open();

            return (compta, commercial);
        }

        public async Task<List<DocumentVente>> SynchroniseCommandes()
        {
            List<DocumentVente> DocumentVenteListe = new List<DocumentVente>();

            try
            {
                var (compta, commercial) = ConnectToSage();

                var listDoc = (await _documentVenteService.GetDocumentVentes())
                              .Where(r => r.DocFlag != 1 && r.DocEtat == 1)
                              .ToList();

                foreach (var entete in listDoc)
                {
                    await SynchroniseClients(entete.DocTiersCode);

                    var nouveauCommande = commercial.FactoryDocumentVente.CreateType(DocumentType.DocumentTypeVenteCommande);
                    var client = compta.FactoryClient.ReadNumero(entete.DocTiersCode);
                    nouveauCommande.Tiers = client;
                    nouveauCommande.SetDefaultClient(client);
                    nouveauCommande.DO_Date = entete.DocDate ?? DateTime.Now;
                    nouveauCommande.DO_Ref = entete.DocPiece;
                    nouveauCommande.DO_Piece = entete.DocPiece;
                    nouveauCommande.WriteDefault();

                    var lignesAssociees = await _documentVenteLigneService.getByDocumentVenteLignePiece(entete.DocPiece);

                    foreach (var ligne in lignesAssociees)
                    {
                        var produit = await _produitService.GetProduitByCode(ligne.LigneArtCode);
                        var famille = await _familleService.GetFamilleByIntitule(produit.ArtFamille);
                        var famCode = famille.FamCode;

                        await SynchroniseArticles(ligne.LigneArtCode);

                        var article = commercial.FactoryArticle.ReadReference(ligne.LigneArtCode);

                        var ligneCommande = (IBODocumentVenteLigne3)nouveauCommande.FactoryDocumentLigne.Create();
                        ligneCommande.SetDefaultArticle(article, Convert.ToDouble(ligne.LigneQte));
                        ligneCommande.DL_PrixUnitaire = Convert.ToDouble(ligne.LignePu);
                        ligneCommande.TTC = false;
                        ligneCommande.WriteDefault();
                    }

                    DocumentVente doc = await _documentVenteService.UpdateDocumentFlag(entete.DocId, 1);
                    DocumentVenteListe.Add(doc);
                }

                return DocumentVenteListe;
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return DocumentVenteListe;
            }
        }

        public async Task<bool> SynchroniseArticles(string CodeArt)
        {
            try
            {
                var (compta, commercial) = ConnectToSage();

                var produit = await _produitService.GetProduitByCode(CodeArt);
                var famille = await _familleService.GetFamilleByIntitule(produit.ArtFamille);
                var famCode = famille.FamCode;
                var articleApp = _produitService.GetProduitByCode(CodeArt).Result;

                if (!commercial.FactoryArticle.ExistReference(CodeArt))
                {
                    var articleAdd = (IBOArticle3)commercial.FactoryArticle.Create();
                    articleAdd.AR_Ref = articleApp.ArtCode;
                    articleAdd.AR_Design = articleApp.ArtIntitule;
                    articleAdd.Famille = commercial.FactoryFamille.ReadCode(FamilleType.FamilleTypeDetail, famCode);

                    articleAdd.WriteDefault();
                }
                else
                {
                    var articleAdd = (IBOArticle3)commercial.FactoryArticle.ReadReference(CodeArt);
                    articleAdd.AR_Ref = articleApp.ArtCode;
                    articleAdd.AR_Design = articleApp.ArtIntitule;
                    articleAdd.Famille = commercial.FactoryFamille.ReadCode(FamilleType.FamilleTypeDetail, famCode);

                    articleAdd.Write();
                }

                await _produitService.UpdateProduitFlag(articleApp.ArtId, 1);

                return true;
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return false;
            }
        }

        public async Task<bool> SynchroniseClients(string CodeClient)
        {
            try
            {
                var (compta, commercial) = ConnectToSage();

                var clientApp = _clientService.GetClientByCodeAsync(CodeClient).Result;
                IBOClient3 clientAdd;

                if (!compta.FactoryTiers.ExistNumero(CodeClient))
                {
                    clientAdd = (IBOClient3)compta.FactoryClient.Create();
                }
                else
                {
                    clientAdd = (IBOClient3)compta.FactoryClient.ReadNumero(CodeClient);
                }

                clientAdd.CT_Num = CodeClient;
                clientAdd.CT_Intitule = clientApp.TiersIntitule.Length > 68
                    ? clientApp.TiersIntitule.Substring(0, 68)
                    : clientApp.TiersIntitule;

                clientAdd.Adresse.Adresse = clientApp.TiersAdresse1.Length > 35
                    ? clientApp.TiersAdresse1.Substring(0, 34)
                    : clientApp.TiersAdresse1;

                clientAdd.Adresse.CodePostal = clientApp.TiersCodePostal;
                clientAdd.Adresse.Ville = clientApp.TiersVille;
                clientAdd.Adresse.Pays = clientApp.TiersPays;
                clientAdd.Telecom.Telephone = clientApp.TiersTel1;
                clientAdd.Telecom.EMail = clientApp.TiersEmail;

                clientAdd.SetDefault();
                clientAdd.WriteDefault();

                await _clientService.UpdateFlag(clientApp.TiersId, 1);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteArticle(string CodeArt)
        {
            try
            {
                var (compta, commercial) = ConnectToSage();

                var Article = (IBOArticle3)commercial.FactoryArticle.ReadReference(CodeArt);
                Article.Remove();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteClient(string CodeClient)
        {
            try
            {
                var (compta, commercial) = ConnectToSage();

                var Client = (IBOClient3)compta.FactoryClient.ReadNumero(CodeClient);
                Client.Remove();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return false;
            }
        }

        public async Task<bool> DeleteCommande(string NumDocument)
        {
            try
            {
                var (compta, commercial) = ConnectToSage();

                var Commande = commercial.FactoryDocumentVente.ReadPiece(DocumentType.DocumentTypeVenteCommande, NumDocument);
                Commande.Remove();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return false;
            }
        }
    }
}