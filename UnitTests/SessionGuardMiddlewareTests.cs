#region Copyright

// ©2002-2026 idéMobi
// www.idemobi.com

#endregion

#region

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DMBServerWebHelper;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;

#endregion

namespace DMBserverWebHelperUnitTest;

[TestFixture]
internal sealed class SessionGuardMiddlewareTests
{
    [Test]
    public async Task InvokeAsyncLoadsUnavailableSessionBeforeNextMiddleware()
    {
        TrackingSession session = new TrackingSession(isAvailable: false);
        bool nextCalled = false;
        SessionGuardMiddleware middleware = new SessionGuardMiddleware(context =>
        {
            nextCalled = true;
            Assert.That(context.Session.IsAvailable, Is.True);
            return Task.CompletedTask;
        });
        DefaultHttpContext context = new DefaultHttpContext
        {
            Session = session
        };

        await middleware.InvokeAsync(context);

        Assert.Multiple(() =>
        {
            Assert.That(nextCalled, Is.True);
            Assert.That(session.LoadCount, Is.EqualTo(1));
        });
    }

    [Test]
    public async Task InvokeAsyncDoesNotLoadAlreadyAvailableSession()
    {
        TrackingSession session = new TrackingSession(isAvailable: true);
        bool nextCalled = false;
        SessionGuardMiddleware middleware = new SessionGuardMiddleware(_ =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        });
        DefaultHttpContext context = new DefaultHttpContext
        {
            Session = session
        };

        await middleware.InvokeAsync(context);

        Assert.Multiple(() =>
        {
            Assert.That(nextCalled, Is.True);
            Assert.That(session.LoadCount, Is.Zero);
        });
    }

    private sealed class TrackingSession : ISession
    {
        private readonly Dictionary<string, byte[]> _values = new Dictionary<string, byte[]>();
        private bool _isAvailable;

        public TrackingSession(bool isAvailable)
        {
            _isAvailable = isAvailable;
        }

        public int LoadCount { get; private set; }

        #region From interface ISession

        public string Id { get; } = Guid.NewGuid().ToString("N");

        public bool IsAvailable => _isAvailable;

        public IEnumerable<string> Keys => _values.Keys;

        public void Clear()
        {
            _values.Clear();
        }

        public Task CommitAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task LoadAsync(CancellationToken cancellationToken)
        {
            LoadCount++;
            _isAvailable = true;
            return Task.CompletedTask;
        }

        public void Remove(string key)
        {
            _values.Remove(key);
        }

        public void Set(string key, byte[] value)
        {
            _values[key] = value;
        }

        public bool TryGetValue(string key, out byte[] value)
        {
            return _values.TryGetValue(key, out value!);
        }

        #endregion
    }
}
