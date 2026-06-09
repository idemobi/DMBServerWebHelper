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
internal sealed class RequestCounterTests
{
    [Test]
    public void IncrementAddsOneToTotal()
    {
        long before = RequestCounter.Total;

        RequestCounter.Increment();

        Assert.That(RequestCounter.Total, Is.EqualTo(before + 1));
    }

    [Test]
    public void IncrementIsThreadSafe()
    {
        const int incrementCount = 512;
        long before = RequestCounter.Total;

        Parallel.For(0, incrementCount, _ => RequestCounter.Increment());

        Assert.That(RequestCounter.Total, Is.EqualTo(before + incrementCount));
    }
}