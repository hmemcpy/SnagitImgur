using System.Runtime.Serialization;

namespace SnagitImgur.OAuth
{
    [DataContract]
    public class ImgurToken
    {
        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }
        [DataMember(Name = "refresh_token")]
        public string RefreshToken { get; set; }
        [DataMember(Name = "account_id")]
        public int AccountID { get; set; }
        [DataMember(Name = "account_username")]
        public string AccountUsername { get; set; }
    }
}