//using UnityEngine;
//using System.Collections;
//
//using System.Collections.Generic;
//using System;
//
//    public class QuizletCredentials {
//    const string key = "Authorization";
//        private string userName, apiToken;
//        private DateTime validTo;
//        private Dictionary<string, string> authHeader = new Dictionary<string, string>();
//
//        // Get Credentials with Authentication
//        public QuizletCredentials(string userName, DateTime validTo, string apiToken) {
//        	string value;
//            this.userName = userName;
//            this.validTo = validTo;
//            this.apiToken = apiToken;
//			value = "Bearer " + apiToken;
//            authHeader.Add(key, value);
//        }
//
//
//        // Get Credentials as anonymous user using public sets
//        public QuizletCredentials(string apiToken) {
//			string value = "Bearer " + apiToken;
//			authHeader.Add(key, value);
//        }
//
//        public string UserName {
//            get { return userName; }
//            set { 
//                if (userName != null)
//                    throw new InvalidOperationException("API Credentials are immutable once created");
//                userName = value;
//            }
//        }
//
//        public DateTime ValidTo {
//            get { return validTo; } 
//            set {
//                if (validTo != default(DateTime))
//                    throw new InvalidOperationException("API credentials are immutable once created");
//                validTo = value;
//            }
//        }
//        
//        public string ApiToken {
//            get { return apiToken; }
//            set {
//                if (apiToken != null)
//                    throw new InvalidOperationException("API credentials are immutable once created");
//                apiToken = value;
//            }
//        }
//
//		public Dictionary<string, string> AuthHeader {
//            get { return authHeader; }
//        }
//    }