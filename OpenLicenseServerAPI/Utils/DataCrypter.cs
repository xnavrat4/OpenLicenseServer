using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;

namespace OpenLicenseServerAPI;

public class DataCrypter
{
    private readonly string _privateKey;
    public DataCrypter(string privateKey)
    {
        _privateKey = privateKey;
    }
    public string Sign(string data)
    {
        var rsa = RSA.Create();
        var keyBytes = Convert.FromBase64String(_privateKey);
        rsa.ImportRSAPrivateKey(keyBytes, out _);
        var dataBytes = Convert.FromBase64String(data);
        var signed = rsa.SignData(dataBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        return Convert.ToBase64String(signed);
    }
    
    
    public string Decrypt(string data, byte [] keyAndIV)
    {
        byte[] buffer = Convert.FromBase64String(data);
        string decryptedString;
        using (Aes aes = Aes.Create())
        {
            aes.Key = keyAndIV[0..16];
            aes.IV = keyAndIV[16..32];
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream memoryStream = new MemoryStream(buffer))
            {
                using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                    {
                        decryptedString = streamReader.ReadToEnd();
                    }
                }
            }
        }

        var jsonObject = Encoding.UTF8.GetString(Convert.FromBase64String(decryptedString));
        return jsonObject;
    }

    public byte[] GetKeyAndIV(string encrypted)
    {
        var keyBytes = Convert.FromBase64String(_privateKey);
        var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(keyBytes, out _);
        var dataBytes = Convert.FromBase64String(encrypted);
        var decrypted = rsa.Decrypt(dataBytes, RSAEncryptionPadding.Pkcs1);
        return decrypted;
    } 
    
    public static string Encrypt(string data, string publicKey, byte[] keyAndIV)
    {
        byte[] encrytpedMessage;
        using (Aes aes = Aes.Create())
        {
            aes.Key = keyAndIV[0..16];
            aes.IV = keyAndIV[16..32];
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;
            
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            // Create the streams used for encryption.
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(data);
                    }
                    encrytpedMessage = msEncrypt.ToArray();
                }
            }
        }
        
        var encryptB64 = Convert.ToBase64String(encrytpedMessage);

        var rsa = RSA.Create();
        var keyBytes = Convert.FromBase64String(publicKey);
        rsa.ImportRSAPublicKey(keyBytes, out _);
        var encrypted = rsa.Encrypt(keyAndIV, RSAEncryptionPadding.Pkcs1);
        var KeyAndIVB64 = Convert.ToBase64String(encrypted);

        var json = JsonSerializer.Serialize(new { Key = KeyAndIVB64, Secret = encryptB64 });
        return json;
    }
}