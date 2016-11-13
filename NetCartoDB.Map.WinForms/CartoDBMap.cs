using System;
using System.Collections.Generic;
using CefSharp.WinForms;
using CefSharp;
using System.Windows.Forms;
using System.Threading.Tasks;
using NetCarto.Map.Common;
using NetCarto.Map.Common.Layers;
using NetCarto.Map.WinForms.Designer.Map.Objects;
using System.ComponentModel;
using System.Diagnostics;

namespace NetCarto.Map.WinForms
{
    [ToolboxItem(true)]
    public partial class CartoMap : UserControl
    {
        internal bool IgnoreDesignMode { get; set; } = false;
        private static bool? isDesignMode;
        private static bool IsDesignMode()
        {
            if (isDesignMode == null)
                isDesignMode = (Process.GetCurrentProcess().ProcessName.ToLower().Contains("devenv"));

            return isDesignMode.Value;
        }

        private static bool IsInitalized = false;

        internal class CefSharpJsHelper : MapJsHelper
        {
            private CartoMap _map;
            internal CefSharpJsHelper(CartoMap map)
            {
                _map = map;
            }
            public override object Execute(string function, params object[] args)
            {
                return _map.Eval(String.Format(function, args));
            }
            public override object Execute(string function)
            {
                return _map.Eval(function);
            }
            public override object ExecuteAsync(string function, params object[] args)
            {
                return _map.EvalAsync(String.Format(function, args));
            }
            public override object ExecuteAsync(string function)
            {
                return _map.EvalAsync(function);
            }
        }

        private MapOptionsDesigner _options = new MapOptionsDesigner();
        private MapJsHelper _jsHelper;
        private TimeSpan _scriptTimeout = new TimeSpan(0, 0, 10);

        private event EventHandler<AddressChangedEventArgs> AddressChanged;
        private event EventHandler<ConsoleMessageEventArgs> ConsoleMessage;
        private event EventHandler<FrameLoadEndEventArgs> FrameLoadEnd;
        private event EventHandler<FrameLoadStartEventArgs> FrameLoadStart;
        private event EventHandler<IsBrowserInitializedChangedEventArgs> IsBrowserInitializedChanged;
        private event EventHandler<LoadErrorEventArgs> LoadError;
        private event EventHandler<LoadingStateChangedEventArgs> LoadingStateChanged;
        private event EventHandler<LoadingStateChangedEventArgs> PageReady;
        private event EventHandler<StatusMessageEventArgs> StatusMessage;
        private event EventHandler<TitleChangedEventArgs> TitleChanged;

        private readonly ChromiumWebBrowser webView;

        private void initialize()
        {
            if (!IsInitalized)
            {
                var settings = new CefSettings();

                settings.CefCommandLineArgs.Add("access-control-allow-origin", "*");
                settings.CachePath = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.CacheFolder);
                //settings.UserAgent = Properties.Settings.Default.UserAgent;
                //Perform dependency check to make sure all relevant resources are in our output directory.
                Cef.Initialize(settings, shutdownOnProcessExit: true, performDependencyCheck: true);

                IsInitalized = true;
            }
        }

        public CartoMap()
        {
            if (IgnoreDesignMode || !IsDesignMode())
            {
                initialize();
                var url = (IsDesignMode()) ? @"C:\Users\ygiron\Dropbox\src\CSharp\NetCarto.Core\NetCarto.Test.WinForms\bin\Debug" + @"/net.cartodb.html" : Application.StartupPath + @"/net.cartodb.html";
                webView = new ChromiumWebBrowser(url) { Dock = DockStyle.Fill };
                init();
            }
            else
            {
                this.BorderStyle = BorderStyle.FixedSingle;
            }

            InitializeComponent();
        }

        public CartoMap(MapOptionsDesigner options)
        {
            if (options == null) return;

            if (IgnoreDesignMode || !IsDesignMode())
            {
                initialize();
                _options = options;
                webView = new ChromiumWebBrowser(Application.StartupPath + @"/net.cartodb.html") { Dock = DockStyle.Fill };
                init();
            }
            else
            {
                this.BorderStyle = BorderStyle.FixedSingle;
            }

            InitializeComponent();
        }

        public void Refresh(MapOptionsDesigner options)
        {
            this.Options = options;
            this.webView.Refresh();
        }

        private void init()
        {
            this._jsHelper = new CefSharpJsHelper(this);
            this.InitCefSharp();
            this.Controls.Add(this.webView);
            this.PageReady += Map_PageReady;
        }

        private void Map_PageReady(object sender, LoadingStateChangedEventArgs e)
        {
            CreateMap(); 
        }

        private void CreateMap()
        {
            _jsHelper.Create(this._options);

            if (this._options.Layers == null) return;

            foreach (ILayer layer in this._options.Layers)
            {
                if (layer != null)
                    this.webView.ExecuteScriptAsync(layer.Create());
            }
        }

        private void InitCefSharp()
        {
            this.webView.AddressChanged += WebView_AddressChanged;
            this.webView.ConsoleMessage += WebView_ConsoleMessage;
            this.webView.FrameLoadEnd += WebView_FrameLoadEnd;
            this.webView.FrameLoadStart += WebView_FrameLoadStart;
            this.webView.IsBrowserInitializedChanged += WebView_IsBrowserInitializedChanged;
            this.webView.LoadError += WebView_LoadError;
            this.webView.LoadingStateChanged += WebView_LoadingStateChanged;
            this.webView.StatusMessage += WebView_StatusMessage;
            this.webView.TitleChanged += WebView_TitleChanged;

            //this.webView.ResizeBegin += (s, e) => SuspendLayout();
            //this.webView.ResizeEnd += (s, e) => ResumeLayout(true);
        }

        #region Chromium Functions
        public void ShowDevTools()
        {
            this.webView.ShowDevTools();
        }

        public void CloseDevTools()
        {
            this.webView.CloseDevTools();
        }

        private void WebView_TitleChanged(object sender, TitleChangedEventArgs e)
        {
            if (this.TitleChanged != null) this.TitleChanged(this, e);
        }

        private void WebView_StatusMessage(object sender, StatusMessageEventArgs e)
        {
            if (this.StatusMessage != null) this.StatusMessage(this, e);
        }

        private void WebView_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (this.LoadingStateChanged != null) this.LoadingStateChanged(sender, e);
            if (e.CanReload && PageReady != null) this.PageReady(sender, e);
        }

        private void WebView_LoadError(object sender, LoadErrorEventArgs e)
        {
            if (this.LoadError != null) this.LoadError(this, e);
        }

        private void WebView_IsBrowserInitializedChanged(object sender, IsBrowserInitializedChangedEventArgs e)
        {
            if (this.IsBrowserInitializedChanged != null) this.IsBrowserInitializedChanged(this, e);
        }

        private void WebView_FrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {
            if (this.FrameLoadStart != null) this.FrameLoadStart(this, e);
        }

        private void WebView_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (this.FrameLoadEnd != null) this.FrameLoadEnd(this, e);
        }

        private void WebView_ConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            Console.WriteLine("Chromium: {0} {1} ({2})", e.Line, e.Message, e.Source);
            if (this.ConsoleMessage != null) this.ConsoleMessage(this, e);
        }

        private void WebView_AddressChanged(object sender, AddressChangedEventArgs e)
        {
            if (this.AddressChanged != null) this.AddressChanged(this, e);
        }

        public void Execute(string script)
        {
            this.webView.ExecuteScriptAsync(script);
        }

        public object Eval(string script)
        {
            if (!this.webView.IsBrowserInitialized) return null;
            var task = this.EvalAsync(script);
            task.Wait();
            if (task.Result == null || !task.Result.Success)
            {
                Console.WriteLine(task.Result.Message);
                return null;
            }

            return task.Result.Result;
        }

        public Task<JavascriptResponse> EvalAsync(string script)
        {
            return this.webView.EvaluateScriptAsync(script, this._scriptTimeout);
        }

        new public void Dispose()
        {
            Cef.Shutdown();
            base.Dispose();
        }
        #endregion

        #region Map functions
        [Category("Map"), Description("Set options value for map control."), Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public MapOptionsDesigner Options { get { return _options; }  set { _options = value; } }
        //public MapOptionsDesigner Options { get { return MapOptionsDesigner.From(_options); } set { _options = value; } }

        public int Map_SetZoom(int zoom)
        {
            _jsHelper.SetZoom(zoom);
            return 0;
        }

        public int Map_GetZoom()
        {
            _jsHelper.GetZoom();
            return 0;
        }

        public int Map_SumZoom(int zoom)
        {
            _jsHelper.SumZoom(zoom);
            return 0;
        }
        #endregion
    }
}
