using Microsoft.Extensions.Options;

namespace WebPage.Connections {
    public class ServicesBuilder {
        private readonly ServicesConfig _config;

        public ServicesBuilder (IOptions<ServicesConfig> options) {
            _config = options.Value;
        }

        // -------- USER -------------
        // ------------- Auth ---------
        public string UserPostLoginUrl {
            get {
                return Combine (_config.REST.User.Auth.BaseUrl, _config.REST.User.Auth.Login.Login);
            }
        }

        public string UserPostLogoutUrl {
            get {
                return Combine (_config.REST.User.Auth.BaseUrl, _config.REST.User.Auth.Logout);
            }
        }
        public string UserGetValidateEmailUrl {
            get {
                return Combine (_config.REST.User.Auth.BaseUrl, _config.REST.User.Auth.Login.ValidateEmailUser);
            }
        }

        public string UserPost2FAUrl {
            get {
                return Combine (_config.REST.User.Auth.BaseUrl, _config.REST.User.Auth.Login.PostTwoFactorCode);
            }
        }

        public string UserGetValidate2FAUrl {
            get {
                return Combine (_config.REST.User.Auth.BaseUrl, _config.REST.User.Auth.Login.ValidateTwoFactorCode);
            }
        }

        public string UserDelete2FAUrl {
            get {
                return Combine (_config.REST.User.Auth.BaseUrl, _config.REST.User.Auth.Login.DeleteTwoFactorCode);
            }
        }

        // ------------- Profile ---------
        public string UserGetPersonalDataUrl {
            get {
                return Combine (_config.REST.User.Profile.BaseUrl, _config.REST.User.Profile.GetPersonalData);
            }
        }

        public string UserGetAddressesUrl {
            get {
                return Combine (_config.REST.User.Profile.BaseUrl, _config.REST.User.Profile.GetAddresses);
            }
        }

        // ------------- Account ---------
        public string UserPostAccountUrl {
            get {
                return Combine (_config.REST.User.Account.BaseUrl, _config.REST.User.Account.PostAccount);
            }
        }

        public string UserDeleteAccountUrl {
            get {
                return Combine (_config.REST.User.Account.BaseUrl, _config.REST.User.Account.DeleteAccount);
            }
        }

        // ----------------- ValidateUserData ---------
        public string UserGetValidateUsernameExistsUrl {
            get {
                return Combine (_config.REST.User.Account.BaseUrl, _config.REST.User.Account.ValidateUserData.ExistenceUsername);
            }
        }

        public string UserGetValidateEmailExistsUrl {
            get {
                return Combine (_config.REST.User.Account.BaseUrl, _config.REST.User.Account.ValidateUserData.ExistenceEmail);
            }
        }

        public string UserGetValidatePhoneNumberExistsUrl {
            get {
                return Combine (_config.REST.User.Account.BaseUrl, _config.REST.User.Account.ValidateUserData.ExistencePhoneNumber);
            }
        }

        // -------- PRODUCT --------
        // ------------- Seller --------
        public string SellerGetProductsUrl {
            get {
                return Combine (_config.REST.Product.Seller.BaseUrl, _config.REST.Product.Seller.GetProducts);
            }
        }

        public string SellerGetProductDetailsUrl {
            get {
                return Combine (_config.REST.Product.Seller.BaseUrl, _config.REST.Product.Seller.GetDetailsProduct);
            }
        }

        public string SellerPostProductUrl {
            get {
                return Combine (_config.REST.Product.Seller.BaseUrl, _config.REST.Product.Seller.PostNewProduct);
            }
        }

        public string SellerPutProductDetailsUrl {
            get {
                return Combine (_config.REST.Product.Seller.BaseUrl, _config.REST.Product.Seller.PutDetailsProduct);
            }
        }

        public string SellerDeleteProductUrl {
            get {
                return Combine (_config.REST.Product.Seller.BaseUrl, _config.REST.Product.Seller.DeleteProduct);
            }
        }

        // ------------- Buyer --------
        public string BuyerGetProductsUrl {
            get {
                return Combine (_config.REST.Product.Buyer.BaseUrl, _config.REST.Product.Buyer.GetProducts);
            }
        }

        public string BuyerGetProductDetailsUrl {
            get {
                return Combine (_config.REST.Product.Buyer.BaseUrl, _config.REST.Product.Buyer.GetDetailsProduct);
            }
        }

        // ------------- Product --------
        public string ProductGetCategoriesUrl {
            get {
                return Combine (_config.REST.Product.Product.BaseUrl, _config.REST.Product.Product.GetCategories);
            }
        }

        public string ProductGetTypesUrl {
            get {
                return Combine (_config.REST.Product.Product.BaseUrl, _config.REST.Product.Product.GetTypes);
            }
        }

        public string ProductGetStatussesUrl {
            get {
                return Combine (_config.REST.Product.Product.BaseUrl, _config.REST.Product.Product.GetStatusses);
            }
        }

        // -------- AUCTION --------
        // ------------- Auctioneer --------
        public string AuctioneerGetAuctionsUrl {
            get {
                return Combine (_config.REST.Auction.Auctioneer.BaseUrl, _config.REST.Auction.Auctioneer.GetAuctions);
            }
        }

        public string AuctioneerGetAuctionDetailsUrl {
            get {
                return Combine (_config.REST.Auction.Auctioneer.BaseUrl, _config.REST.Auction.Auctioneer.GetDetailsAuction);
            }
        }

        public string AuctioneerPostAuctionUrl {
            get {
                return Combine (_config.REST.Auction.Auctioneer.BaseUrl, _config.REST.Auction.Auctioneer.PostAuction);
            }
        }

        public string AuctioneerPatchAuctionUrl {
            get {
                return Combine (_config.REST.Auction.Auctioneer.BaseUrl, _config.REST.Auction.Auctioneer.PatchAuction);
            }
        }

        // ------------- Participant --------
        public string ParticipantGetAuctionsUrl {
            get {
                return Combine (_config.REST.Auction.Participant.BaseUrl, _config.REST.Auction.Participant.GetAuctions);
            }
        }

        public string ParticipantGetAuctionDetailsUrl {
            get {
                return Combine (_config.REST.Auction.Participant.BaseUrl, _config.REST.Auction.Participant.GetDetailsAuction);
            }
        }

        public string ParticipantPostBidUrl {
            get {
                return Combine (_config.REST.Auction.Participant.BaseUrl, _config.REST.Auction.Participant.PostBid);
            }
        }

        // -------- gRPC --------
        public string GetGrpcBaseUrl {
            get {
                return _config.gRPC.BaseUrl;
            }
        }

        // -------- Helper --------
        private static string Combine (string baseUrl, string endpoint) {
            return $"{baseUrl.TrimEnd ('/')}/{endpoint.TrimStart ('/')}";
        }
    }
}
