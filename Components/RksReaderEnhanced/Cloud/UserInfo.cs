using Newtonsoft.Json;
using System.Security.Cryptography;

namespace yt6983138.github.io.RksReaderEnhanced;

public struct UserInfo
{
	public ACL ACL;
	public AuthInfo authData;
	public string avatar;
	public DateTime createdAt;
	public bool emailVerified;
	public bool mobilePhoneVerified;
	public string nickname;
	public string objectId;
	public string sessionToken;
	public string shortId;
	public DateTime updatedAt;
	public string username;
}
public struct AuthInfo
{
	public TapTapAuthInfo taptap;
}
public struct TapTapAuthInfo
{
	public string access_token;
	public string avatar;
	public string kid;
	public string mac_algorithm;
	public string mac_key;
	public string name;
	public string openid;
	public string token_type;
	public string unionid;
}
public struct ACL
{
	[JsonProperty("*")] // ???? cant understand what does star means
	public ACLReadWrite star;
}
public struct ACLReadWrite
{
	public bool read;
	public bool write;
}