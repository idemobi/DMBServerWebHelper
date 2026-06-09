#region Copyright

// ©2002-2026 idéMobi
// www.idemobi.com

#endregion

#region

using DMBServerWebHelper;
using DMBserverWebHelperUnitTest.Helpers;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;

#endregion

namespace DMBserverWebHelperUnitTest;

[TestFixture]
internal sealed class CaptchaFactoryTests
{
    private static DefaultHttpContext CreateSessionContext()
    {
        return new DefaultHttpContext
        {
            Session = new InMemorySession()
        };
    }

    [Test]
    public void RandomCaptchaNoMistakeReturnsRequestedLengthAndAllowedCharacters()
    {
        string captcha = CaptchaFactory.RandomCaptchaNoMistake(32);
        const string allowedCharacters = "cdefhkmnpqrtwxyCEFHKMNPRTWXY379";

        Assert.Multiple(() =>
        {
            Assert.That(captcha, Has.Length.EqualTo(32));
            Assert.That(captcha.All(allowedCharacters.Contains), Is.True);
        });
    }

    [Test]
    public void RandomCaptchaNoMistakeSupportsConcurrentGeneration()
    {
        const int captchaCount = 128;
        const string allowedCharacters = "cdefhkmnpqrtwxyCEFHKMNPRTWXY379";

        string[] captchas = Enumerable.Range(0, captchaCount)
            .AsParallel()
            .Select(_ => CaptchaFactory.RandomCaptchaNoMistake(16))
            .ToArray();

        Assert.Multiple(() =>
        {
            Assert.That(captchas, Has.Length.EqualTo(captchaCount));
            Assert.That(captchas.All(captcha => captcha.Length == 16), Is.True);
            Assert.That(captchas.All(captcha => captcha.All(allowedCharacters.Contains)), Is.True);
        });
    }

    [Test]
    public void RandomCaptchaToImageStoresUppercaseCaptchaAndReturnsPngBase64()
    {
        DefaultHttpContext context = CreateSessionContext();

        string imageBase64 = CaptchaFactory.RandomCaptchaToImage(context);
        string storedCaptcha = CaptchaFactory.GetStoredCaptcha(context);
        byte[] image = Convert.FromBase64String(imageBase64);

        Assert.Multiple(() =>
        {
            Assert.That(storedCaptcha, Has.Length.EqualTo(8));
            Assert.That(storedCaptcha, Is.EqualTo(storedCaptcha.ToUpperInvariant()));
            Assert.That(CaptchaFactory.TestCaptcha(context, storedCaptcha.ToLowerInvariant()), Is.True);
            Assert.That(image.Take(8).ToArray(), Is.EqualTo(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 }));
        });
    }

    [Test]
    public void RandomCaptchaToImageSupportsConcurrentGenerationForDifferentSessions()
    {
        const int captchaCount = 12;

        var results = Enumerable.Range(0, captchaCount)
            .AsParallel()
            .Select(_ =>
            {
                DefaultHttpContext context = CreateSessionContext();
                string imageBase64 = CaptchaFactory.RandomCaptchaToImage(context);
                string storedCaptcha = CaptchaFactory.GetStoredCaptcha(context);
                byte[] image = Convert.FromBase64String(imageBase64);
                bool accepted = CaptchaFactory.TestCaptcha(context, storedCaptcha);

                return new
                {
                    StoredCaptcha = storedCaptcha,
                    Accepted = accepted,
                    PngHeader = image.Take(8).ToArray()
                };
            })
            .ToArray();

        Assert.Multiple(() =>
        {
            Assert.That(results, Has.Length.EqualTo(captchaCount));
            Assert.That(results.All(result => result.StoredCaptcha.Length == 8), Is.True);
            Assert.That(results.All(result => result.Accepted), Is.True);
            Assert.That(results.All(result => result.PngHeader.SequenceEqual(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 })), Is.True);
        });
    }

    [Test]
    public void TestCaptchaCannotReplayAValidValue()
    {
        DefaultHttpContext context = CreateSessionContext();
        context.Session.SetString("Captcha", "REPLAY");

        bool firstAttempt = CaptchaFactory.TestCaptcha(context, "replay");
        bool secondAttempt = CaptchaFactory.TestCaptcha(context, "replay");

        Assert.Multiple(() =>
        {
            Assert.That(firstAttempt, Is.True);
            Assert.That(secondAttempt, Is.False);
            Assert.That(context.Session.Keys, Does.Not.Contain("Captcha"));
        });
    }

    [Test]
    public void TestCaptchaComparesStoredSessionValueCaseInsensitively()
    {
        DefaultHttpContext context = CreateSessionContext();
        context.Session.SetString("Captcha", "ABCD");

        Assert.Multiple(() =>
        {
            Assert.That(CaptchaFactory.GetStoredCaptcha(context), Is.EqualTo("ABCD"));
            Assert.That(CaptchaFactory.TestCaptcha(context, "abcd"), Is.True);
            Assert.That(context.Session.Keys, Does.Not.Contain("Captcha"));
        });
    }

    [Test]
    public void TestCaptchaConsumesStoredValueAfterFailedAttempt()
    {
        DefaultHttpContext context = CreateSessionContext();
        context.Session.SetString("Captcha", "SECRET");

        bool result = CaptchaFactory.TestCaptcha(context, "wrong");

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False);
            Assert.That(context.Session.Keys, Does.Not.Contain("Captcha"));
            Assert.That(CaptchaFactory.TestCaptcha(context, "SECRET"), Is.False);
        });
    }

    [Test]
    public void TestCaptchaRejectsHistoricalDefaultWhenNoSessionValueExists()
    {
        DefaultHttpContext context = CreateSessionContext();

        Assert.Multiple(() =>
        {
            Assert.That(CaptchaFactory.GetStoredCaptcha(context), Is.Empty);
            Assert.That(CaptchaFactory.TestCaptcha(context, "not defined"), Is.False);
            Assert.That(CaptchaFactory.TestCaptcha(context, string.Empty), Is.False);
        });
    }
}