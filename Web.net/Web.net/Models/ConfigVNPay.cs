using Microsoft.AspNetCore.DataProtection.KeyManagement;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace Web.net.Models
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Security.Cryptography;
    using System.Text;
    using Microsoft.AspNetCore.Http;

    public class ConfigVNPay
    {
        public static string VnpPayUrl = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
        public static string VnpReturnUrl = "http://localhost:8080/Baya_Project/status-payment";
        public static string VnpTmnCode = "FETQ33PG";
        public static string SecretKey = "1JWI9I2PCF3D7V1WQJTRWVFAV3PPF07C";
        public static string VnpApiUrl = "https://sandbox.vnpayment.vn/merchant_webapi/api/transaction";

        public static string GenerateMD5(string message)
        {
            try
            {
                using var md5 = MD5.Create();
                byte[] inputBytes = Encoding.UTF8.GetBytes(message);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string Sha256(string message)
        {
            try
            {
                using (var sha256 = SHA256.Create())
                {
                    byte[] inputBytes = Encoding.UTF8.GetBytes(message);
                    byte[] hashBytes = sha256.ComputeHash(inputBytes);
                    return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string HashAllFields(Dictionary<string, string> fields)
        {
            try
            {
                var sortedFields = fields.OrderBy(kv => kv.Key).ToList();
                var sb = new StringBuilder();

                foreach (var field in sortedFields)
                {
                    if (!string.IsNullOrEmpty(field.Value))
                    {
                        sb.Append($"{field.Key}={field.Value}&");
                    }
                }

                if (sb.Length > 0)
                    sb.Length--; // Remove the trailing '&'

                return HmacSHA512(SecretKey, sb.ToString());
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string HmacSHA512(string key, string data)
        {
            try
            {
                using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key)))
                {
                    byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                    return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GetIpAddress(HttpRequest request)
        {
            try
            {
                string ipAddress = request.Headers["X-FORWARDED-FOR"].FirstOrDefault();
                return string.IsNullOrEmpty(ipAddress) ? request.HttpContext.Connection.RemoteIpAddress?.ToString() : ipAddress;
            }
            catch (Exception ex)
            {
                return $"Invalid IP: {ex.Message}";
            }
        }

        public static string GetRandomNumber(int length)
        {
            const string chars = "0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }

}
