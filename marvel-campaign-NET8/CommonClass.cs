﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace marvel_campaign_NET8
{
    public static class AppInp
    {
        public const string InputAuth_Agent_Id = "Agent_Id";
        public const string InputAuth_Token = "Token";

        public const string Input_Customer_Id = "Customer_Id";
        public const string Input_Scheduled_Time = "Scheduled_Time";
        public const string Input_Campaign_Code = "Campaign_Code";
        public const string Input_Batch_Code = "Batch_Code";
        public const string Input_Call_Id = "Call_Id";
        public const string Input_Batch_Id = "Batch_Id";
        public const string Input_Assign_From = "Assign_From";
        public const string Input_Gender = "Gender";
        public const string Input_Is_Valid = "Is_Valid";

        public const string Input_SearchArr_field_name = "field_name";
        public const string Input_SearchArr_logic_operator = "logic_operator";
        public const string Input_SearchArr_value = "value";
        public const string Input_Search_Operator_is_not = "is not";
        public const string Input_Search_Operator_contains = "contains";
        public const string Input_Search_Operator_not_contains = "not contains";


        public const string Input_datatable_order = "order";

    }

    public static class AppOutp
    {
        public const string OutputResult_Field = "result";
        public const string OutputDetails_Field = "details";

        public const string OutputResult_SUCC = "success";
        public const string OutputDetails_Inv_Para = "Invalid Parameters.";

        public const string OutputResult_FAIL = "fail";

        public const string Not_Auth_Desc = "Not Auth.";

        public const string STATUS_SUCC = "success";

        public const string STATUS_Active = "Active";


    }


    public static class ValidateClass
    {
        // JWT
        private static readonly string Secret = Environment.GetEnvironmentVariable("JWT_Secret") ?? "";


        public static string GenerateToken(string P_Username)
        {
            byte[] _non_base64_secret = Convert.FromBase64String(Secret);
            SymmetricSecurityKey _symmetric_security_key = new SymmetricSecurityKey(_non_base64_secret);

            ClaimsIdentity _claims_identity = new ClaimsIdentity();
            _claims_identity.AddClaim(new Claim(ClaimTypes.Name, P_Username));

            SecurityTokenDescriptor _security_token_descriptor = new SecurityTokenDescriptor
            {
                Subject = _claims_identity,
                Expires = DateTime.UtcNow.AddMinutes(60 * 24),
                SigningCredentials = new SigningCredentials(_symmetric_security_key, SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler _jwt_security_token_handler = new JwtSecurityTokenHandler();
            JwtSecurityToken _jwt_security_token = _jwt_security_token_handler.CreateJwtSecurityToken(_security_token_descriptor);
            return _jwt_security_token_handler.WriteToken(_jwt_security_token);
        }

        public static string? ValidateToken(string P_Token)
        {
            ClaimsIdentity? _claims_identity;

            ClaimsPrincipal? _claims_principal = GetClaimsPrincipal(P_Token);
            if (_claims_principal == null) return null;


            _claims_identity = (ClaimsIdentity?)_claims_principal.Identity;


            Claim? _claim_name = _claims_identity?.FindFirst(ClaimTypes.Name);
            return _claim_name?.Value; // username
        }

        public static ClaimsPrincipal? GetClaimsPrincipal(string P_Token)
        {
            try
            {
                JwtSecurityTokenHandler _jwt_security_token_handler = new JwtSecurityTokenHandler();
                JwtSecurityToken _jwt_security_token = (JwtSecurityToken)_jwt_security_token_handler.ReadToken(P_Token);

                if (_jwt_security_token == null) return null;

                byte[] _non_base64_secret = Convert.FromBase64String(Secret);

                TokenValidationParameters _token_validation_parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(_non_base64_secret)
                };

                SecurityToken _security_token;
                ClaimsPrincipal _claims_principal = _jwt_security_token_handler.ValidateToken(P_Token, _token_validation_parameters, out _security_token);

                return _claims_principal;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool Authenticated(string token, string P_Username)
        {
            if (string.IsNullOrEmpty(token) ||
                string.IsNullOrEmpty(P_Username))
            {
                return false;
            }

            return ValidateToken(token) == P_Username;

        }



    }
}
