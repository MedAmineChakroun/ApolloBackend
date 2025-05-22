using ApolloBackend.Functions;
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
        public async Task<List<DocumentVente>> SynchroniseCommandes()
        {
            List<DocumentVente> DocumentVenteListe = new List<DocumentVente>();

            try
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

                var listDoc = (await _documentVenteService.GetDocumentVentes())
                              .Where(r => r.DocFlag != 1)
                              .ToList();
                foreach (var entete in listDoc)
                {
                    if (!compta.FactoryTiers.ExistNumero(entete.DocTiersCode))
                    {

                        var clientAdd = (IBOClient3)compta.FactoryClient.Create();
                        clientAdd.CT_Num = entete.DocTiersCode;
                        clientAdd.CT_Intitule = entete.DocTiersIntitule.Length > 68
                            ? entete.DocTiersIntitule.Substring(0, 68)
                            : entete.DocTiersIntitule;
                        clientAdd.SetDefault();
                        clientAdd.WriteDefault();
                    }

                    var nouveauCommande = commercial.FactoryDocumentVente.CreateType(DocumentType.DocumentTypeVenteCommande);
                    var client = compta.FactoryClient.ReadNumero(entete.DocTiersCode);
                    nouveauCommande.Tiers = client;
                    nouveauCommande.SetDefaultClient(client);
                    nouveauCommande.DO_Date = entete.DocDate ?? DateTime.Now;
                    nouveauCommande.DO_Ref = entete.DocPiece;
                    nouveauCommande.SetDefaultDO_Piece();
                    nouveauCommande.WriteDefault();

                    var lignesAssociees = await _documentVenteLigneService.getByDocumentVenteLignePiece(entete.DocPiece);

                    foreach (var ligne in lignesAssociees)
                    {
                        var produit = await _produitService.GetProduitByCode(ligne.LigneArtCode);
                        var famille = await _familleService.GetFamilleByIntitule(produit.ArtFamille);
                        var famCode = famille.FamCode;

                        if (!commercial.FactoryArticle.ExistReference(ligne.LigneArtCode))
                        {
                            var articleAdd = (IBOArticle3)commercial.FactoryArticle.Create();
                            articleAdd.AR_Ref = ligne.LigneArtCode;
                            articleAdd.AR_Design = ligne.LigneArtDesi;
                            articleAdd.Famille = commercial.FactoryFamille.ReadCode(FamilleType.FamilleTypeDetail, famCode);
                            articleAdd.WriteDefault();
                        }

                        var article = commercial.FactoryArticle.ReadReference(ligne.LigneArtCode);
                        var ligneCommande = (IBODocumentVenteLigne3)nouveauCommande.FactoryDocumentLigne.Create();
                        ligneCommande.SetDefaultArticle(article, Convert.ToDouble(ligne.LigneQte));
                        ligneCommande.DL_PrixUnitaire = Convert.ToDouble(ligne.LignePu);
                        ligneCommande.TTC = false;
                        ligneCommande.WriteDefault();
                    }
                    DocumentVente doc = await _documentVenteService.UpdateDocumentFlag(entete.DocId, 1);
                    //push to list
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

                _produitService.UpdateProduitFlag(articleApp.ArtId, 1);

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


                _clientService.UpdateFlag(clientApp.TiersId, 1);


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
