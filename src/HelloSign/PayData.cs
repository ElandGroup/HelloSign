using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using RestSharp.Serializers;

namespace HelloSign
{

    [SerializeAs]
    public class PayData
    {
        public PayData()
        {

        }

        private SortedDictionary<string, object> m_values = new SortedDictionary<string, object>();

        public void SetValue(string key, object value)
        {
            m_values[key] = value;
        }

        public void RemoveValue(string key)
        {
            m_values.Remove(key);
        }

        public object GetValue(string key)
        {
            object o = null;
            m_values.TryGetValue(key, out o);
            return o;
        }
        public bool IsSet(string key)
        {
            object o = null;
            m_values.TryGetValue(key, out o);
            if (null != o)
                return true;
            else
                return false;
        }

        public string ToUrl()
        {
            string buff = "";
            foreach (KeyValuePair<string, object> pair in m_values)
            {

                if (pair.Key == "key" || pair.Key == "sign" || pair.Value == null || string.IsNullOrWhiteSpace(pair.Value.ToString()))
                {
                    continue;
                }
                buff += pair.Key + "=" + pair.Value + "&";
            }
            buff = buff.Trim('&');
            return buff;
        }


        public string ToJson()
        {
            string jsonStr = JsonConvert.SerializeObject(m_values);
            return jsonStr;
        }

        public void FromJson(string jsonStr)
        {
            m_values = JsonConvert.DeserializeObject<SortedDictionary<string, object>>(jsonStr);
        }

        public string MakeSign(string key)
        {
            string str = ToUrl();
            str += "&key=" + key;
            //MD5加密
            var md5 = MD5.Create();
            var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            var sb = new StringBuilder();
            foreach (byte b in bs)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString().ToUpper();
        }



        public bool CheckSign(string key)
        {
            if (!IsSet("sign"))
            {
                return false;
            }
            else if (GetValue("sign") == null || GetValue("sign").ToString() == "")
            {
                return false;
            }

            string return_sign = GetValue("sign").ToString();

            string cal_sign = MakeSign(key);

            if (cal_sign == return_sign)
            {
                return true;
            }
            return false;

        }

        /// <summary>
        /// 获取Dictionary
        /// </summary>
        /// <returns>Dictionary</returns>
        public SortedDictionary<string, object> GetValues()
        {
            return m_values;
        }
    }
}
