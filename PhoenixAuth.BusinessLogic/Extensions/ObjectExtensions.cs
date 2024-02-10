using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PhoenixAuth.BusinessLogic.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Converts data to json
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ToJson<T>(this T data)
        {
            return JsonConvert.SerializeObject(data);
        }

        /// <summary>
        /// Base64 encode
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToBase64Encode(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            byte[] textBytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(textBytes);
        }

        /// <summary>
        /// Get the value of the description attribute if the enum has one, otherwise use the value.  
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription<TEnum>(this TEnum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            if (fi == null) return value.ToString();

            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }

            return value.ToString();
        }

        /// <summary>
        /// Checks for generic nullable objects
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="@object">Object</param>
        /// <returns>True or False</returns>
        public static bool IsNull<T>(this T @object)
        {
            return Equals(@object, null);
        }

        public static HttpContent ToHttpJsonContent<T>(this T model)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            return content;

        }

        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
        {
            var json = await content.ReadAsStringAsync();
            T value = JsonConvert.DeserializeObject<T>(json);
            return value;
        }

        public static double RoundTo(this double val, int decimals)
        {
            return Math.Round(val, decimals);
        }


    }
}
