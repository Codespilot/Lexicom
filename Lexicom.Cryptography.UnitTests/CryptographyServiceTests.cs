using Lexicom.Cryptography.Extensions;
using Lexicom.Cryptography.Options;
using Lexicom.DependencyInjection.Amenities.Extensions;
using Lexicom.UnitTesting.DependencyInjection;

namespace Lexicom.Cryptography.UnitTests;

public class CryptographyServiceTests
{
    [Fact]
    public async Task Encryption_And_Decryption()
    {
        var ita = new IntegrationTestAssistant();

        ita.Configuration.AddInMemoryCollection(new CryptographyStringSecretOptions
        {
            Base64StringSecretKey = "MTIzNDU2Nzg5MTIzNDU2Nzg5MTIzNDU2Nzg5MTIzNDU=",
        });

        ita.AddLexicomCryptography(c =>
        {
            c.AddStringSecretOptions();
        });

        string originalPlainText = "my plain text";

        var cryptographyService = ita.Make<ICryptographyService>();

        string encryptedbase64 = await cryptographyService.EncryptAsync(originalPlainText);

        string plainText = await cryptographyService.DecryptAsync(encryptedbase64);

        Assert.False(string.IsNullOrWhiteSpace(encryptedbase64));
        Assert.NotEqual(originalPlainText, encryptedbase64);
        Assert.NotEqual(plainText, encryptedbase64);

        Assert.False(string.IsNullOrWhiteSpace(plainText));
        Assert.Equal(originalPlainText, plainText);
    }
}
