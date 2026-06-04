#region Copyright

// ©2002-2026 idéMobi
// www.idemobi.com

#endregion

#region

using DMBServerWebHelper;
using NUnit.Framework;

#endregion

namespace DMBserverWebHelperUnitTest;

[TestFixture]
internal sealed class SecurityHashToolsTests
{
    [Test]
    public void GenerateSha256ReturnsKnownHashForText()
    {
        string hash = SecurityHashTools.GenerateSha256("abc");

        Assert.That(hash, Is.EqualTo("ba7816bf8f01cfea414140de5dae2223b00361a396177a9cb410ff61f20015ad"));
    }

    [Test]
    public void GenerateSha256TreatsNullAsEmptyString()
    {
        string nullHash = SecurityHashTools.GenerateSha256(null);
        string emptyHash = SecurityHashTools.GenerateSha256(string.Empty);

        Assert.Multiple(() =>
        {
            Assert.That(nullHash, Is.EqualTo(emptyHash));
            Assert.That(nullHash, Is.EqualTo("e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855"));
        });
    }
}