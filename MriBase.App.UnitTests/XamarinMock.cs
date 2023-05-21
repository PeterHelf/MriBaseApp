using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace MriBase.App.UnitTests
{
    internal class MockPlatformServices : IPlatformServices
    {
        public static void Init()
        {
            Device.Info = new MockDeviceInfo();
            Device.PlatformServices = new MockPlatformServices();
            DependencyService.Register<MockResourcesProvider>();
            DependencyService.Register<MockDeserializer>();
        }

        readonly Action<Action> invokeOnMainThread;
        readonly Action<Uri> openUriAction;
        readonly Func<Uri, CancellationToken, Task<Stream>> getStreamAsync;
        public MockPlatformServices(Action<Action> invokeOnMainThread = null, Action<Uri> openUriAction = null, Func<Uri, CancellationToken, Task<Stream>> getStreamAsync = null)
        {
            this.invokeOnMainThread = invokeOnMainThread;
            this.openUriAction = openUriAction;
            this.getStreamAsync = getStreamAsync;
        }

        public string GetMD5Hash(string input)
        {
            throw new NotImplementedException();
        }
        static int hex(int v)
        {
            if (v < 10)
                return '0' + v;
            return 'a' + v - 10;
        }

        public double GetNamedSize(NamedSize size, Type targetElement, bool useOldSizes)
        {
            return size switch
            {
                NamedSize.Default => 10,
                NamedSize.Micro => 4,
                NamedSize.Small => 8,
                NamedSize.Medium => 12,
                NamedSize.Large => 16,
                _ => throw new ArgumentOutOfRangeException(nameof(size)),
            };
        }

        public void OpenUriAction(Uri uri)
        {
            if (openUriAction != null)
                openUriAction(uri);
            else
                throw new NotImplementedException();
        }

        public bool IsInvokeRequired
        {
            get { return false; }
        }

        public string RuntimePlatform { get; set; }

        public OSAppTheme RequestedTheme => throw new NotImplementedException();

        public void BeginInvokeOnMainThread(Action action)
        {
            if (invokeOnMainThread == null)
                action();
            else
                invokeOnMainThread(action);
        }

        public Ticker CreateTicker()
        {
            return new MockTicker();
        }

        public void StartTimer(TimeSpan interval, Func<bool> callback)
        {
            Timer timer = null;
            void onTimeout(object o) => BeginInvokeOnMainThread(() =>
            {
                if (callback())
                    return;

                timer.Dispose();
            });
            timer = new Timer(onTimeout, null, interval, interval);
        }

        public Task<Stream> GetStreamAsync(Uri uri, CancellationToken cancellationToken)
        {
            if (getStreamAsync == null)
                throw new NotImplementedException();
            return getStreamAsync(uri, cancellationToken);
        }

        public Assembly[] GetAssemblies()
        {
            return Array.Empty<Assembly>();
        }

        public IIsolatedStorageFile GetUserStoreForApplication()
        {
            throw new NotImplementedException();
        }

        public string GetHash(string input)
        {
            throw new NotImplementedException();
        }

        public Color GetNamedColor(string name)
        {
            throw new NotImplementedException();
        }

        public void QuitApplication()
        {
            throw new NotImplementedException();
        }

        public SizeRequest GetNativeSize(VisualElement view, double widthConstraint, double heightConstraint)
        {
            throw new NotImplementedException();
        }
    }

    internal class MockDeserializer : IDeserializer
    {
        public Task<IDictionary<string, object>> DeserializePropertiesAsync()
        {
            return Task.FromResult<IDictionary<string, object>>(new Dictionary<string, object>());
        }

        public Task SerializePropertiesAsync(IDictionary<string, object> properties)
        {
            return Task.FromResult(false);
        }
    }

    internal class MockResourcesProvider : ISystemResourcesProvider
    {
        public IResourceDictionary GetSystemResources()
        {
            var dictionary = new ResourceDictionary();
            Style style;
            style = new Style(typeof(Label));
            dictionary[Device.Styles.BodyStyleKey] = style;

            style = new Style(typeof(Label));
            style.Setters.Add(Label.FontSizeProperty, 50);
            dictionary[Device.Styles.TitleStyleKey] = style;

            style = new Style(typeof(Label));
            style.Setters.Add(Label.FontSizeProperty, 40);
            dictionary[Device.Styles.SubtitleStyleKey] = style;

            style = new Style(typeof(Label));
            style.Setters.Add(Label.FontSizeProperty, 30);
            dictionary[Device.Styles.CaptionStyleKey] = style;

            style = new Style(typeof(Label));
            style.Setters.Add(Label.FontSizeProperty, 20);
            dictionary[Device.Styles.ListItemTextStyleKey] = style;

            style = new Style(typeof(Label));
            style.Setters.Add(Label.FontSizeProperty, 10);
            dictionary[Device.Styles.ListItemDetailTextStyleKey] = style;

            return dictionary;
        }
    }

    internal class MockTicker : Ticker
    {
        bool _enabled;

        protected override void EnableTimer()
        {
            _enabled = true;

            while (_enabled)
            {
                SendSignals(16);
            }
        }

        protected override void DisableTimer()
        {
            _enabled = false;
        }
    }

    internal class MockDeviceInfo : DeviceInfo
    {
        public override Size PixelScreenSize => throw new NotImplementedException();

        public override Size ScaledScreenSize => throw new NotImplementedException();

        public override double ScalingFactor => throw new NotImplementedException();
    }
}
