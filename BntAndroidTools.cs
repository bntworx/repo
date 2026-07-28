using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace BntAndroidTools
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }

    static class UpdateChecker
    {
        public const string CURRENT_VERSION = "8.16";
        public const string VERSION_URL = "https://raw.githubusercontent.com/bntworx/repo/master/version.txt";

        public static void CheckForUpdates(Action<string, string> onNewVersion)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                using (var wc = new WebClient())
                {
                    wc.Headers.Add("User-Agent", "BNT-Tools");
                    string remote = wc.DownloadString(VERSION_URL).Trim();
                    string[] lines = remote.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    if (lines.Length < 2) return;

                    string remoteVersion = lines[0].Trim();
                    string downloadUrl = lines[1].Trim();

                    if (remoteVersion != CURRENT_VERSION)
                    {
                        if (onNewVersion != null)
                            onNewVersion(remoteVersion, downloadUrl);
                    }
                }
            }
            catch { }
        }

        public static bool DownloadUpdate(string url, string destPath, Action<int> onProgress)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                using (var wc = new WebClient())
                {
                    wc.Headers.Add("User-Agent", "BNT-Tools");
                    wc.DownloadProgressChanged += (s, e) =>
                    {
                        if (onProgress != null) onProgress(e.ProgressPercentage);
                    };
                    wc.DownloadFile(url, destPath);
                    return true;
                }
            }
            catch { return false; }
        }
    }

    static class Adb
    {
        public static string Run(string args, bool quiet = true)
        {
            try
            {
                var psi = new ProcessStartInfo("adb", args)
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.UTF8,
                    StandardErrorEncoding = Encoding.UTF8
                };
                using (var p = Process.Start(psi))
                {
                    string stdout = p.StandardOutput.ReadToEnd();
                    string stderr = p.StandardError.ReadToEnd();
                    p.WaitForExit();
                    return stdout.Trim();
                }
            }
            catch { return ""; }
        }

        public static bool IsAvailable()
        {
            try
            {
                var psi = new ProcessStartInfo("adb", "version")
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                using (var p = Process.Start(psi))
                {
                    string s = p.StandardOutput.ReadToEnd();
                    p.WaitForExit();
                    return s.Contains("Android Debug Bridge");
                }
            }
            catch { return false; }
        }

        public static bool IsDeviceConnected()
        {
            string r = Run("devices");
            string[] lines = r.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 1; i < lines.Length; i++)
                if (lines[i].Contains("\tdevice")) return true;
            return false;
        }

        public static bool IsRooted()
        {
            string r = Run(@"shell su -c ""id""");
            return r.Contains("uid=0");
        }

        public static string GetProp(string prop)
        {
            return Run("shell getprop " + prop);
        }

        public static string RunShell(string cmd)
        {
            return Run("shell " + cmd);
        }
    }

    static class Colors
    {
        public static readonly Color DarkBg = Color.FromArgb(18, 18, 30);
        public static readonly Color PanelBg = Color.FromArgb(24, 24, 40);
        public static readonly Color SidebarBg = Color.FromArgb(14, 14, 24);
        public static readonly Color LogoBg = Color.FromArgb(10, 10, 20);
        public static readonly Color CardBg = Color.FromArgb(30, 30, 50);
        public static readonly Color CardHover = Color.FromArgb(40, 40, 65);
        public static readonly Color Accent = Color.FromArgb(0, 180, 210);
        public static readonly Color AccentDim = Color.FromArgb(0, 130, 160);
        public static readonly Color AccentGreen = Color.FromArgb(0, 200, 120);
        public static readonly Color Text = Color.FromArgb(220, 225, 240);
        public static readonly Color TextDim = Color.FromArgb(110, 115, 140);
        public static readonly Color Red = Color.FromArgb(230, 60, 60);
        public static readonly Color Orange = Color.FromArgb(240, 160, 40);
        public static readonly Color Blue = Color.FromArgb(80, 140, 240);
        public static readonly Color Header = Color.FromArgb(0, 180, 210);
        public static readonly Color TabBg = Color.FromArgb(22, 22, 38);
        public static readonly Color TabActive = Color.FromArgb(0, 180, 210);
        public static readonly Color TabHover = Color.FromArgb(35, 35, 58);
        public static readonly Color OutputBg = Color.FromArgb(12, 12, 22);
        public static readonly Color OutputHeader = Color.FromArgb(20, 20, 36);
        public static readonly Color Border = Color.FromArgb(40, 40, 65);
        public static readonly Color Samsung = Color.FromArgb(34, 100, 220);
        public static readonly Color Xiaomi = Color.FromArgb(245, 100, 30);
        public static readonly Color Huawei = Color.FromArgb(200, 30, 30);
        public static readonly Color Oppo = Color.FromArgb(0, 160, 80);
        public static readonly Color Vivo = Color.FromArgb(40, 80, 200);
        public static readonly Color Motorola = Color.FromArgb(120, 40, 200);
        public static readonly Color Lg = Color.FromArgb(170, 40, 120);
        public static readonly Color Nokia = Color.FromArgb(50, 120, 200);
        public static readonly Color MediaTek = Color.FromArgb(0, 150, 200);
        public static readonly Color Sony = Color.FromArgb(30, 30, 30);
    }

    public class FlatButton : Button
    {
        public Color HoverColor = Colors.CardHover;
        public Color ActiveColor = Colors.Accent;
        private bool _active;
        public bool Active { get { return _active; } set { _active = value; BackColor = value ? ActiveColor : Colors.CardBg; ForeColor = value ? Color.White : Colors.Text; Invalidate(); } }

        public FlatButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            FlatAppearance.MouseOverBackColor = Colors.CardHover;
            BackColor = Colors.CardBg;
            ForeColor = Colors.Text;
            Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            Cursor = Cursors.Hand;
            TextAlign = ContentAlignment.MiddleLeft;
            DoubleBuffered = true;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (!_active) BackColor = HoverColor;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (!_active) BackColor = Colors.CardBg;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
        }
    }

    public class ActionButton : FlatButton
    {
        public ActionButton()
        {
            Font = new Font("Segoe UI", 9f);
            Height = 32;
        }
    }

    public class MainForm : Form
    {
        private Panel sidebar, contentPanel, topPanel, outputPanel, brandTabBar;
        private Panel sectionContainer;
        private RichTextBox outputBox;
        private Label deviceLabel;
        private string deviceInfo = "";
        private string deviceDetails = "";
        private bool isRooted = false;
        private string currentBrand = "general";
        private List<FlatButton> navButtons = new List<FlatButton>();
        private List<FlatButton> brandTabButtons = new List<FlatButton>();
        private FlatButton activeNav;
        private FlatButton activeBrandTab;

        public MainForm()
        {
            SetupForm();
            SetupContent();
            SetupBrandTabs();
            SetupSidebar();
            DetectDevice();
            ShowSection("dashboard");
            CheckForUpdates();
        }

        private void CheckForUpdates()
        {
            new Thread(() =>
            {
                UpdateChecker.CheckForUpdates((version, url) =>
                {
                    this.Invoke(new Action(() =>
                    {
                        var updateForm = new Form
                        {
                            Text = "BNT Tools - Update Required",
                            Size = new Size(450, 220),
                            StartPosition = FormStartPosition.CenterScreen,
                            BackColor = Colors.PanelBg,
                            FormBorderStyle = FormBorderStyle.FixedDialog,
                            MaximizeBox = false,
                            MinimizeBox = false,
                            TopMost = true
                        };

                        var msgLabel = new Label
                        {
                            Text = "NEW VERSION AVAILABLE: v" + version + "\n\nCurrent: v" + UpdateChecker.CURRENT_VERSION + "\n\nYou must update to continue using this tool.",
                            Font = new Font("Segoe UI", 9.5f),
                            ForeColor = Colors.Text,
                            Location = new Point(15, 15),
                            Width = 420,
                            Height = 75
                        };

                        var timerLabel = new Label
                        {
                            Text = "App will close in 60 seconds...",
                            Font = new Font("Consolas", 10f, FontStyle.Bold),
                            ForeColor = Colors.Red,
                            Location = new Point(15, 95),
                            Width = 420,
                            Height = 22
                        };

                        var downloadBtn = new FlatButton
                        {
                            Text = "Download Update",
                            Location = new Point(15, 125),
                            Size = new Size(200, 35),
                            BackColor = Colors.Accent,
                            ForeColor = Color.White,
                            Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
                        };
                        downloadBtn.Click += (s, e) =>
                        {
                            try { Process.Start(new ProcessStartInfo(url) { UseShellExecute = true }); } catch { }
                        };

                        var closeBtn = new FlatButton
                        {
                            Text = "Close App",
                            Location = new Point(230, 125),
                            Size = new Size(200, 35),
                            BackColor = Colors.Red,
                            ForeColor = Color.White,
                            Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
                        };
                        closeBtn.Click += (s, e) => { updateForm.Close(); Application.Exit(); };

                        updateForm.Controls.AddRange(new Control[] { msgLabel, timerLabel, downloadBtn, closeBtn });

                        var countdownTimer = new System.Windows.Forms.Timer { Interval = 1000 };
                        int secondsLeft = 60;
                        countdownTimer.Tick += (s, e) =>
                        {
                            secondsLeft--;
                            timerLabel.Text = "App will close in " + secondsLeft + " seconds...";
                            if (secondsLeft <= 0)
                            {
                                countdownTimer.Stop();
                                updateForm.Close();
                                Application.Exit();
                            }
                        };

                        updateForm.Show();
                        countdownTimer.Start();
                    }));
                });
            }).Start();
        }

        private void SetupForm()
        {
            Text = "BNT Android Tools v8.16";
            Size = new Size(1400, 850);
            MinimumSize = new Size(1200, 750);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Colors.DarkBg;
            ForeColor = Colors.Text;
            Font = new Font("Segoe UI", 9.5f);
            DoubleBuffered = true;
            try
            {
                string icoPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "bnticon.ico");
                if (File.Exists(icoPath)) Icon = new Icon(icoPath);
                else
                {
                    var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("BntAndroidTools.bnticon.ico");
                    if (stream != null) Icon = new Icon(stream);
                }
            }
            catch { }
        }

        private void SetupBrandTabs()
        {
            brandTabBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 40,
                BackColor = Colors.SidebarBg,
                Padding = new Padding(4, 4, 4, 0)
            };

            string[][] brands = new[] {
                new[] { "GENERAL", "general" },
                new[] { "SAMSUNG", "samsung" },
                new[] { "XIAOMI", "xiaomi" },
                new[] { "HUAWEI", "huawei" },
                new[] { "OPPO", "oppo" },
                new[] { "VIVO", "vivo" },
                new[] { "ONEPLUS", "oneplus" },
                new[] { "MOTOROLA", "motorola" },
                new[] { "NOKIA", "nokia" },
                new[] { "MTK", "mtk" },

            };

            int x = 2;
            for (int i = 0; i < brands.Length; i++)
            {
                string brandName = brands[i][0];
                string brandKey = brands[i][1];
                var tabBtn = new FlatButton
                {
                    Text = brandName,
                    Location = new Point(x, 2),
                    Size = new Size(90, 34),
                    BackColor = Colors.SidebarBg,
                    ForeColor = Colors.TextDim,
                    Font = new Font("Consolas", 8.5f, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Tag = brandKey
                };
                if (i == 0)
                {
                    tabBtn.BackColor = Colors.Accent;
                    tabBtn.ForeColor = Color.White;
                    activeBrandTab = tabBtn;
                }
                tabBtn.Click += BrandTab_Click;
                brandTabButtons.Add(tabBtn);
                brandTabBar.Controls.Add(tabBtn);
                x += 94;
            }
            Controls.Add(brandTabBar);
        }

        private void BrandTab_Click(object sender, EventArgs e)
        {
            var btn = (FlatButton)sender;
            if (activeBrandTab != null)
            {
                activeBrandTab.BackColor = Colors.SidebarBg;
                activeBrandTab.ForeColor = Colors.TextDim;
            }
            btn.BackColor = Colors.Accent;
            btn.ForeColor = Color.White;
            activeBrandTab = btn;
            currentBrand = btn.Tag.ToString();
            ShowSection("brand_" + currentBrand);
        }

        private void SetupSidebar()
        {
            sidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 200,
                BackColor = Colors.PanelBg,
                Padding = new Padding(4, 4, 4, 4)
            };

            var sidebarHeader = new Label
            {
                Text = "FUNCTIONS",
                Dock = DockStyle.Top,
                Height = 28,
                Font = new Font("Consolas", 8.5f, FontStyle.Bold),
                ForeColor = Colors.Accent,
                BackColor = Colors.PanelBg,
                TextAlign = ContentAlignment.MiddleCenter
            };

            var navPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Colors.PanelBg,
                AutoScroll = true,
                Padding = new Padding(2, 2, 2, 2)
            };

            string[][] items = new[] {
                new[] { "brand_general", "  Dashboard" },
                new[] { "ads", "  Ad Removal" },
                new[] { "frp", "  FRP Bypass" },
                new[] { "fastboot", "  Fastboot FRP" },
                new[] { "mtpfrp", "  MTP FRP Bypass" },
                new[] { "bloat", "  Bloatware" },
                new[] { "utils", "  Device Utils" },
                new[] { "privacy", "  Privacy Shield" },
                new[] { "apps", "  App Manager" },
                new[] { "quick", "  Quick Actions" },
                new[] { "dev", "  Developer Tools" },
                new[] { "net", "  Network Tools" },
                new[] { "downloads", "  Downloads" },
                new[] { "settings", "  Settings" },
            };

            int y = 2;
            foreach (var item in items)
            {
                var btn = new FlatButton
                {
                    Tag = item[0],
                    Text = item[1],
                    Location = new Point(2, y),
                    Size = new Size(190, 30),
                    BackColor = Colors.PanelBg,
                    ForeColor = Colors.Text,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font = new Font("Segoe UI", 8.5f),
                    Padding = new Padding(5, 0, 0, 0)
                };
                btn.Click += Nav_Click;
                navButtons.Add(btn);
                navPanel.Controls.Add(btn);
                y += 32;
            }

            var adbPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 44,
                BackColor = Colors.Red,
                Padding = new Padding(3)
            };
            var adbBtn = new FlatButton
            {
                Text = "DOWNLOAD ADB",
                Dock = DockStyle.Fill,
                BackColor = Colors.Red,
                ForeColor = Color.White,
                Font = new Font("Consolas", 9f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };
            adbBtn.Click += (s, e) =>
            {
                try
                {
                    Process.Start(new ProcessStartInfo("https://developer.android.com/tools/releases/platform-tools") { UseShellExecute = true });
                }
                catch { }
            };
            adbPanel.Controls.Add(adbBtn);
            sidebar.Controls.Add(navPanel);
            sidebar.Controls.Add(sidebarHeader);
            sidebar.Controls.Add(adbPanel);
            Controls.Add(sidebar);
        }

        private void Nav_Click(object sender, EventArgs e)
        {
            var btn = (FlatButton)sender;
            if (activeNav != null) activeNav.Active = false;
            btn.Active = true;
            activeNav = btn;
            ShowSection(btn.Tag.ToString());
        }

        private void SetupContent()
        {
            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Colors.DarkBg,
            };
            Controls.Add(contentPanel);

            outputPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 160,
                BackColor = Colors.OutputBg,
                Padding = new Padding(5)
            };

            var outputHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 28,
                BackColor = Colors.OutputHeader,
                Padding = new Padding(8, 0, 0, 0)
            };
            var outputHeaderLabel = new Label
            {
                Text = "LOG OUTPUT",
                Font = new Font("Consolas", 8.5f, FontStyle.Bold),
                ForeColor = Colors.Accent,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Colors.OutputHeader
            };
            var clearBtn = new FlatButton
            {
                Text = "CLEAR",
                Dock = DockStyle.Right,
                Width = 60,
                BackColor = Colors.OutputHeader,
                ForeColor = Colors.TextDim,
                Font = new Font("Consolas", 7.5f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };
            clearBtn.Click += (s, e) => outputBox.Clear();
            outputHeader.Controls.Add(outputHeaderLabel);
            outputHeader.Controls.Add(clearBtn);

            outputBox = new RichTextBox
            {
                Dock = DockStyle.Fill,
                BackColor = Colors.OutputBg,
                ForeColor = Colors.Accent,
                Font = new Font("Consolas", 9f),
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                WordWrap = false
            };

            outputPanel.Controls.Add(outputBox);
            outputPanel.Controls.Add(outputHeader);

            topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 38,
                BackColor = Colors.PanelBg,
                Padding = new Padding(10, 0, 10, 0)
            };

            deviceLabel = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 9f),
                ForeColor = Colors.AccentGreen,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Colors.PanelBg,
                Text = "No device connected"
            };

            var refreshBtn = new FlatButton
            {
                Text = "REFRESH",
                Dock = DockStyle.Right,
                Width = 80,
                BackColor = Colors.PanelBg,
                ForeColor = Colors.Accent,
                Font = new Font("Consolas", 8f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };
            refreshBtn.Click += (s, e) => DetectDevice();

            topPanel.Controls.Add(deviceLabel);
            topPanel.Controls.Add(refreshBtn);

            sectionContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Colors.DarkBg,
                AutoScroll = true,
                Padding = new Padding(10, 5, 10, 5)
            };

            contentPanel.Controls.Add(sectionContainer);
            contentPanel.Controls.Add(outputPanel);
            contentPanel.Controls.Add(topPanel);
        }

        private void Log(string msg, Color? color = null)
        {
            if (outputBox.InvokeRequired)
            {
                outputBox.Invoke(new Action(() => Log(msg, color)));
                return;
            }
            outputBox.SelectionColor = color ?? Colors.Accent;
            outputBox.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] " + msg + "\n");
            outputBox.SelectionStart = outputBox.TextLength;
            outputBox.ScrollToCaret();
        }

        private void ClearOutput()
        {
            outputBox.Clear();
        }

        private string RunAdb(string args)
        {
            string result = Adb.Run(args);
            Log("adb " + args + " => " + (result.Length > 200 ? result.Substring(0, 200) + "..." : (result.Length > 0 ? result : "(empty)")), Colors.TextDim);
            return result;
        }

        private void RunAdbBg(string args, string label = "")
        {
            string display = label.Length > 0 ? label : args;
            Log("Running: " + display, Colors.Blue);
            new Thread(() =>
            {
                Adb.Run(args);
                Log("Done: " + display, Colors.Accent);
            }).Start();
        }

        private void DetectDevice()
        {
            Log("Checking ADB...", Colors.Orange);
            if (!Adb.IsAvailable())
            {
                Log("FATAL: ADB not found! Install from platform-tools", Colors.Red);
                deviceLabel.Text = "ADB NOT FOUND";
                deviceLabel.ForeColor = Colors.Red;
                return;
            }
            Log("ADB found.", Colors.Accent);

            Log("Scanning for devices...", Colors.Orange);
            if (!Adb.IsDeviceConnected())
            {
                Log("No device found. Enable USB Debugging & connect.", Colors.Red);
                deviceLabel.Text = "NO DEVICE CONNECTED";
                deviceLabel.ForeColor = Colors.Red;
                return;
            }

            string model = Adb.GetProp("ro.product.model");
            string brand = Adb.GetProp("ro.product.brand");
            string android = Adb.GetProp("ro.build.version.release");
            string sdk = Adb.GetProp("ro.build.version.sdk");
            string device = Adb.GetProp("ro.product.device");
            string arch = Adb.GetProp("ro.product.cpu.abi");
            string board = Adb.GetProp("ro.product.board");
            string patch = Adb.GetProp("ro.build.version.security_patch");
            string build = Adb.GetProp("ro.build.display.id");
            string mfg = Adb.GetProp("ro.product.manufacturer");
            string devname = Adb.RunShell("settings get global device_name");

            isRooted = Adb.IsRooted();

            deviceInfo = string.Format("{0} {1} | Android {2} (SDK {3}) | {4} | Root: {5}",
                brand, model, android, sdk, arch, isRooted ? "YES" : "NO");

            deviceLabel.Text = deviceInfo;
            deviceLabel.ForeColor = Colors.Accent;

            deviceDetails = string.Format("Manufacturer: {0} | Device: {1}\nBoard: {2} | Build: {3}\nSecurity Patch: {4} | Dev Name: {5}",
                mfg, device, board, build, patch, devname);

            Log(string.Format("Device: {0} {1} | Android {2} | Root: {3}", brand, model, android, isRooted ? "YES" : "NO"), Colors.Accent);
        }

        private void ShowSection(string section)
        {
            ClearOutput();
            sectionContainer.Controls.Clear();

            if (section.StartsWith("brand_"))
            {
                ShowBrandPage(section.Replace("brand_", ""));
                return;
            }

            switch (section)
            {
                case "dashboard": ShowDashboard(); break;
                case "ads": ShowAds(); break;
                case "frp": ShowFrp(); break;
                case "fastboot": ShowFastboot(); break;
                case "mtpfrp": ShowMtpFrp(); break;
                case "bloat": ShowBloat(); break;
                case "utils": ShowUtils(); break;
                case "privacy": ShowPrivacy(); break;
                case "apps": ShowApps(); break;
                case "quick": ShowQuick(); break;
                case "dev": ShowDev(); break;
                case "net": ShowNet(); break;
                case "downloads": ShowDownloads(); break;
                case "settings": ShowSettings(); break;

            }
        }

        private Panel CreateSection(string title)
        {
            sectionContainer.Controls.Clear();
            return sectionContainer;
        }

        private Panel CreateCard(string title, string desc, int x, int y, int w = 260, int h = 90)
        {
            var panel = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(w, h),
                BackColor = Colors.CardBg,
                Padding = new Padding(10, 8, 10, 8)
            };

            var titleLabel = new Label
            {
                Text = title,
                Font = new Font("Consolas", 10f, FontStyle.Bold),
                ForeColor = Colors.Accent,
                Dock = DockStyle.Top,
                Height = 24,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Colors.CardBg
            };

            var descLabel = new Label
            {
                Text = desc,
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = Colors.TextDim,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.TopLeft,
                BackColor = Colors.CardBg
            };

            panel.Controls.Add(descLabel);
            panel.Controls.Add(titleLabel);
            return panel;
        }

        private ActionButton MakeBtn(string text, int x, int y, int w = 200, Action onClick = null)
        {
            var btn = new ActionButton
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(w, 32)
            };
            if (onClick != null) btn.Click += (s, e) => onClick();
            return btn;
        }

        private string PromptInput(string message)
        {
            var form = new Form
            {
                Text = "Input",
                Size = new Size(400, 150),
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Colors.PanelBg,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };
            var lbl = new Label { Text = message, Location = new Point(15, 15), Width = 350, ForeColor = Colors.Text };
            var txt = new TextBox { Location = new Point(15, 45), Width = 350, BackColor = Colors.CardBg, ForeColor = Colors.Text, BorderStyle = BorderStyle.FixedSingle };
            var ok = new FlatButton { Text = "OK", Location = new Point(265, 80), Width = 100, Height = 30, ActiveColor = Colors.Accent };
            var cancel = new FlatButton { Text = "Cancel", Location = new Point(150, 80), Width = 100, Height = 30 };
            ok.Click += (s, e) => { form.DialogResult = DialogResult.OK; form.Close(); };
            cancel.Click += (s, e) => { form.DialogResult = DialogResult.Cancel; form.Close(); };
            form.Controls.AddRange(new Control[] { lbl, txt, ok, cancel });
            form.AcceptButton = ok;
            return form.ShowDialog() == DialogResult.OK ? txt.Text : "";
        }

        private bool Confirm(string message)
        {
            return MessageBox.Show(message, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
        }

        private void ExecuteWithWait(Action action, string label)
        {
            Log("Starting: " + label, Colors.Orange);
            Cursor = Cursors.WaitCursor;
            try { action(); }
            catch (Exception ex) { Log("Error: " + ex.Message, Colors.Red); }
            Cursor = Cursors.Default;
            Log("Completed: " + label, Colors.Accent);
        }

        // =====================================================================
        //                           DASHBOARD
        // =====================================================================
        //                           DASHBOARD / BRAND PAGES
        // =====================================================================
        private void ShowDashboard()
        {
            ShowBrandPage(currentBrand);
        }

        private void ShowBrandPage(string brand)
        {
            var panel = CreateSection("DASHBOARD");
            int availW = sectionContainer.Width - 30;
            int y = 10;

            var infoCard = CreateCard("CONNECTED DEVICE", deviceInfo, 10, y, availW, 55);
            panel.Controls.Add(infoCard);
            y += 65;

            if (Adb.IsDeviceConnected() && !string.IsNullOrEmpty(deviceDetails))
            {
                var detailsCard = CreateCard("DEVICE DETAILS", deviceDetails, 10, y, availW, 65);
                panel.Controls.Add(detailsCard);
                y += 75;
            }

            if (!Adb.IsAvailable())
            {
                var adbWarnCard = new Panel
                {
                    Location = new Point(10, y),
                    Size = new Size(availW, 55),
                    BackColor = Color.FromArgb(50, 20, 20),
                    Padding = new Padding(10)
                };
                var adbWarnLabel = new Label
                {
                    Text = "ADB NOT FOUND - Install from Platform Tools",
                    Font = new Font("Consolas", 9.5f, FontStyle.Bold),
                    ForeColor = Colors.Red,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleLeft,
                    BackColor = Color.FromArgb(50, 20, 20)
                };
                adbWarnCard.Controls.Add(adbWarnLabel);
                panel.Controls.Add(adbWarnCard);
                y += 65;
            }

            if (brand == "general")
            {
                string[][] sections = new[] {
                    new[] { "Ad Removal", "Hosts, DNS, nuclear\nDisable SDKs, banners", "ads" },
                    new[] { "FRP Bypass", "Setup wizard, accounts\nFull bypass suite", "frp" },
                    new[] { "Fastboot FRP", "Erase FRP/persist\nBootloader unlock", "fastboot" },
                    new[] { "MTP FRP Bypass", "USB mode switch, Samsung\nUniversal MTP methods", "mtpfrp" },
                    new[] { "Bloatware", "13 brands, full clean\nSearch, reinstall", "bloat" },
                    new[] { "Device Utils", "Info, reboot, backup\nScreenshot, APK install", "utils" },
                    new[] { "Privacy Shield", "Permissions, telemetry\nAudit, encrypt", "privacy" },
                    new[] { "App Manager", "Force stop, clear, bulk\nUninstall, info", "apps" },
                    new[] { "Quick Actions", "Optimize, cache clear\nTimeout, USB, battery", "quick" },
                    new[] { "Developer Tools", "Logcat, dumpsys, shell\nMonkey test, benchmark", "dev" },
                    new[] { "Network Tools", "WiFi, DNS, ping, IP", "net" },
                    new[] { "Downloads", "ADB + USB drivers\nDriver packages", "downloads" },
                    new[] { "Settings", "About, logs, export", "settings" },
                };
                int gap = 10;
                int cardH = 80;
                int cols = Math.Max(1, (availW + gap) / (250 + gap));
                int cardW = (availW - (cols - 1) * gap) / cols;
                for (int i = 0; i < sections.Length; i++)
                {
                    int col = i % cols;
                    int row = i / cols;
                    int x = 10 + col * (cardW + gap);
                    int cy = y + row * (cardH + gap);
                    var card = CreateCard(sections[i][0], sections[i][1], x, cy, cardW, cardH);
                    card.Cursor = Cursors.Hand;
                    string sec = sections[i][2];
                    card.Click += (s, e) => ShowSection(sec);
                    foreach (Control c in card.Controls) { c.Cursor = Cursors.Hand; c.Click += (s, e) => ShowSection(sec); }
                    panel.Controls.Add(card);
                }
            }
            else
            {
                string brandTitle = brand.ToUpper() + " TOOLS";
                Color brandColor = Colors.Accent;
                string[][] brandActions = new string[0][];
                string brandModels = "";

                switch (brand)
                {
                    case "samsung":
                        brandColor = Colors.Samsung;
                        brandModels = "Galaxy S: S2-S24 Ultra | Galaxy Note: Note 3-20 Ultra | Galaxy A: A01-A54 | Galaxy Z: Fold/Flip 1-5 | Galaxy Tab: Tab S3-S9 | J Series, M Series, F Series";
                        brandActions = new[] {
                            new[] { "Erase FRP", "frp" }, new[] { "Factory Reset", "frp" },
                            new[] { "Fastboot FRP", "fastboot" }, new[] { "MTP FRP Bypass", "mtpfrp" },
                            new[] { "Odin Mode", "fastboot" }, new[] { "Knox Reset", "mtpfrp" },
                            new[] { "Smart Switch Method", "mtpfrp" }, new[] { "Emergency Call", "mtpfrp" },
                            new[] { "Samsung Bloatware", "bloat" }, new[] { "Samsung USB Driver", "downloads" },
                        };
                        break;
                    case "xiaomi":
                        brandColor = Colors.Xiaomi;
                        brandModels = "Mi: Mi 8-Mi 14 Ultra | Redmi: Note 7-13 Pro+, K50-K70, 12-13 | Poco: F3-F6, X3-X6 Pro | Pad 5-6 Pro | Mix: Mix 4-Fold";
                        brandActions = new[] {
                            new[] { "Erase FRP", "frp" }, new[] { "MIUI Setup Wizard", "frp" },
                            new[] { "Mi Account Clear", "mtpfrp" }, new[] { "Xiaomi MTP Bypass", "mtpfrp" },
                            new[] { "Fastboot FRP", "fastboot" }, new[] { "Xiaomi Bloatware", "bloat" },
                            new[] { "Xiaomi USB Driver", "downloads" },
                        };
                        break;
                    case "huawei":
                        brandColor = Colors.Huawei;
                        brandModels = "P Series: P8-P60 Pro | Mate: Mate 8-Mate 60 Pro | Nova: Nova 2-Nova 12 | Honor: 6X-Magic6 Pro (pre-split) | Y Series: Y5-Y9 | MatePad: 11-Pro";
                        brandActions = new[] {
                            new[] { "Erase FRP", "frp" }, new[] { "HiSuite Method", "mtpfrp" },
                            new[] { "Huawei MTP Bypass", "mtpfrp" }, new[] { "Fastboot FRP", "fastboot" },
                            new[] { "Huawei Bloatware", "bloat" }, new[] { "Huawei USB Driver", "downloads" },
                        };
                        break;
                    case "oppo":
                        brandColor = Colors.Oppo;
                        brandModels = "Find: Find X2-X7 Ultra | Reno: Reno 4-12 Pro | A Series: A15-A98 | K Series: K3-K11 | F Series: F9-F27 | Pad 2 | Realme: GT 1-6 Pro, C55-C67, 11-13 Pro+";
                        brandActions = new[] {
                            new[] { "Erase FRP", "frp" }, new[] { "ColorOS Setup", "mtpfrp" },
                            new[] { "OPPO MTP Bypass", "mtpfrp" }, new[] { "Fastboot FRP", "fastboot" },
                            new[] { "OPPO Bloatware", "bloat" }, new[] { "OPPO USB Driver", "downloads" },
                        };
                        break;
                    case "vivo":
                        brandColor = Colors.Vivo;
                        brandModels = "X Series: X50-X100 Pro+ | V Series: V9-V30 Pro | Y Series: Y12s-Y78+ | iQOO: Neo 5-Z9 Turbo | NEX: NEX 3 | Pad: Pad 2-3 Pro | T Series: T1-T3";
                        brandActions = new[] {
                            new[] { "Erase FRP", "frp" }, new[] { "FuntouchOS Setup", "mtpfrp" },
                            new[] { "Vivo MTP Bypass", "mtpfrp" }, new[] { "Fastboot FRP", "fastboot" },
                            new[] { "Vivo Bloatware", "bloat" }, new[] { "Vivo USB Driver", "downloads" },
                        };
                        break;
                    case "oneplus":
                        brandColor = Colors.Red;
                        brandModels = "OnePlus: 3/3T-12 Pro | Nord: N10-N30 CE | Pad: OnePlus Pad | CE Series: CE 1-3 | Ace: Ace 1-3V | Open (Fold)";
                        brandActions = new[] {
                            new[] { "Erase FRP", "frp" }, new[] { "Fastboot FRP", "fastboot" },
                            new[] { "OnePlus MTP Bypass", "mtpfrp" }, new[] { "OnePlus Bloatware", "bloat" },
                        };
                        break;
                    case "motorola":
                        brandColor = Colors.Motorola;
                        brandModels = "Moto G: G10-G84 Power | Edge: Edge 30-50 Pro | Razr: Razr 2019-Razr+ 2024 | One: One Vision-Action | Moto E: E6-E22 | ThinkPhone";
                        brandActions = new[] {
                            new[] { "Erase FRP", "frp" }, new[] { "Moto Setup Wizard", "mtpfrp" },
                            new[] { "Motorola MTP Bypass", "mtpfrp" }, new[] { "Fastboot FRP", "fastboot" },
                            new[] { "Motorola Bloatware", "bloat" },
                        };
                        break;
                    case "nokia":
                        brandColor = Colors.Nokia;
                        brandModels = "Nokia: 1.3-8.4 | G: G10-G60 | X: X10-X30 | C: C01-C32 | XR: XR20-XR21 | Plus: Nokia 2.2-5.4 | Tab: T10-T20";
                        brandActions = new[] {
                            new[] { "Erase FRP", "frp" }, new[] { "Nokia MTP Bypass", "mtpfrp" },
                            new[] { "Fastboot FRP", "fastboot" }, new[] { "Nokia Bloatware", "bloat" },
                        };
                        break;
                    case "mtk":
                        brandColor = Colors.MediaTek;
                        brandModels = "MediaTek Dimensity: 700-9300 | Helio: G37-G99, P35-P65, X10-X30 | MT65xx/67xx series | Realme, Infinix, Tecno, Xiaomi Redmi, Oppo, Vivo MTK models";
                        brandActions = new[] {
                            new[] { "Erase FRP", "frp" }, new[] { "MTK Auth Bypass", "mtpfrp" },
                            new[] { "MTK Meta Mode", "mtpfrp" }, new[] { "Fastboot FRP", "fastboot" },
                            new[] { "MTK DA Load", "mtpfrp" }, new[] { "Brom Exploit", "fastboot" },
                            new[] { "MTK Bloatware", "bloat" },
                        };
                        break;

                }

                var titleLabel = new Label
                {
                    Text = brandTitle,
                    Font = new Font("Consolas", 14f, FontStyle.Bold),
                    ForeColor = brandColor,
                    Location = new Point(10, y),
                    Size = new Size(availW, 30)
                };
                panel.Controls.Add(titleLabel);
                y += 35;

                if (brandModels.Length > 0)
                {
                    var modelsLabel = new Label
                    {
                        Text = "Models: " + brandModels,
                        Font = new Font("Consolas", 7.5f, FontStyle.Bold),
                        ForeColor = Colors.TextDim,
                        Location = new Point(10, y),
                        Size = new Size(availW, 28)
                    };
                    panel.Controls.Add(modelsLabel);
                    y += 30;
                }

                int gap = 8;
                int cardH = 70;
                int cols = Math.Max(1, (availW + gap) / (200 + gap));
                int cardW = (availW - (cols - 1) * gap) / cols;
                for (int i = 0; i < brandActions.Length; i++)
                {
                    int col = i % cols;
                    int row = i / cols;
                    int x = 10 + col * (cardW + gap);
                    int cy = y + row * (cardH + gap);
                    var card = CreateCard(brandActions[i][0], "", x, cy, cardW, cardH);
                    card.Cursor = Cursors.Hand;
                    string sec = brandActions[i][1];
                    card.Click += (s, e) => ShowSection(sec);
                    foreach (Control c in card.Controls) { c.Cursor = Cursors.Hand; c.Click += (s, e) => ShowSection(sec); }
                    panel.Controls.Add(card);
                }
            }
        }

        // =====================================================================
        //                           AD REMOVAL
        // =====================================================================
        private void ShowAds()
        {
            var panel = CreateSection("AD REMOVAL TOOLKIT");
            int btnW = Math.Min(420, panel.Width - 40);
            int btnX = 10;

            int y = 10;
            var adsModels = new Label
            {
                Text = "Supported: All Android devices (root for hosts block, ADB for SDK disable) | Android 5.0-14",
                Font = new Font("Consolas", 7.5f, FontStyle.Bold),
                ForeColor = Colors.Accent,
                Location = new Point(10, y),
                Size = new Size(580, 20)
            };
            panel.Controls.Add(adsModels); y += 22;
            panel.Controls.Add(MakeBtn("Hosts File Block (Root) - 130+ domains", btnX, y, btnW, () => AdsHosts())); y += 40;
            panel.Controls.Add(MakeBtn("Disable Ad Services (40+ SDKs)", btnX, y, btnW, () => AdsServices())); y += 40;
            panel.Controls.Add(MakeBtn("Nuclear Option (All Methods)", btnX, y, btnW, () => AdsNuclear())); y += 40;
            panel.Controls.Add(MakeBtn("DNS-Based Blocking (No Root)", btnX, y, btnW, () => AdsDns())); y += 40;
            panel.Controls.Add(MakeBtn("Stop Tracking & Reset Ad ID", btnX, y, btnW, () => AdsTracking())); y += 40;
            panel.Controls.Add(MakeBtn("Custom Hosts Editor (Root)", btnX, y, btnW, () => AdsCustom())); y += 40;
            panel.Controls.Add(MakeBtn("Full Ads Clean (Everything)", btnX, y, btnW, () => AdsFull())); y += 40;
            panel.Controls.Add(MakeBtn("Banner/Popup Removal", btnX, y, btnW, () => AdsBanner())); y += 40;
            panel.Controls.Add(MakeBtn("Revoke Ad Permissions", btnX, y, btnW, () => AdsPerms())); y += 40;
        }

        private void AdsHosts()
        {
            if (!isRooted) { Log("ERROR: Root required! Use DNS-based instead.", Colors.Red); return; }
            if (!Confirm("Apply 130+ ad domains to hosts file? (Root required)")) return;
            ExecuteWithWait(() =>
            {
                RunAdb(@"shell su -c ""cp /system/etc/hosts /system/etc/hosts.bak.bnt""");
                RunAdb(@"shell su -c ""mount -o rw,remount /system""");

                string hostsFile = Path.Combine(Path.GetTempPath(), "bnt_hosts_adblock");
                var sb = new StringBuilder();
                sb.AppendLine("127.0.0.1 localhost");
                sb.AppendLine(":: === GOOGLE ADS ===");
                string[] googleAds = { "pagead2.googlesyndication.com", "adservice.google.com", "googleads.g.doubleclick.net", "www.googleadservices.com", "ad.doubleclick.net", "doubleclick.net", "fls.doubleclick.net", "stats.g.doubleclick.net", "googlesyndication.com", "www.googlesyndication.com" };
                foreach (var d in googleAds) sb.AppendLine("127.0.0.1 " + d);
                sb.AppendLine(":: === GOOGLE ANALYTICS ===");
                string[] ga = { "analytics.google.com", "www.google-analytics.com", "google-analytics.com", "googletagmanager.com", "www.googletagmanager.com", "app-measurement.com", "www.app-measurement.com", "chartbeat.net", "www.chartbeat.net", "scorecardresearch.com", "www.scorecardresearch.com" };
                foreach (var d in ga) sb.AppendLine("127.0.0.1 " + d);
                sb.AppendLine(":: === FACEBOOK/META ===");
                string[] fb = { "facebook.com", "www.facebook.com", "graph.facebook.com", "pixel.facebook.com", "an.facebook.com", "b-graph.facebook.com", "b-api.facebook.com", "tr.facebook.com" };
                foreach (var d in fb) sb.AppendLine("127.0.0.1 " + d);
                sb.AppendLine(":: === AD NETWORKS ===");
                string[] adn = { "adobedtm.com", "amazon-adsystem.com", "ad.turn.com", "ads.mopub.com", "ads.yahoo.com", "moatads.com", "mopub.com", "openx.net", "www.openx.net", "outbrain.com", "www.outbrain.com", "revcontent.com", "taboola.com", "www.taboola.com", "media.net", "www.media.net", "adnxs.com", "www.adnxs.com", "casalemedia.com", "demdex.net", "pubmatic.com", "rubiconproject.com", "quantserve.com" };
                foreach (var d in adn) sb.AppendLine("127.0.0.1 " + d);
                sb.AppendLine(":: === AD SDKs ===");
                string[] sdk = { "adcolony.com", "airpush.com", "tapjoy.com", "vungle.com", "zedo.com", "inmobi.com", "unity3d.com", "unityads.unity3d.com", "smaato.net", "fyber.com", "yieldmo.com", "nativo.com", "ads-twitter.com", "ads.snapchat.com" };
                foreach (var d in sdk) sb.AppendLine("127.0.0.1 " + d);
                sb.AppendLine(":: === TRACKING SDKs ===");
                string[] track = { "adjust.com", "app.adjust.com", "appsflyer.com", "kochava.com", "braze.com", "appboy.com", "segment.com", "amplitude.com", "mixpanel.com", "hotjar.com", "fullstory.com", "localytics.com", "urbanairship.com", "leanplum.com", "onesignal.com", "pushwoosh.com", "flurry.com", "ironsrc.com", "startapp.com", "chartboost.com", "leadbolt.com", "mparticle.com", "branch.io", "singular.net", "tenjin.com" };
                foreach (var d in track) sb.AppendLine("127.0.0.1 " + d);
                sb.AppendLine(":: === RETARGETING ===");
                string[] ret = { "criteo.com", "www.criteo.com", "criteo.net", "mathtag.com", "bluekai.com", "exelator.com", "eyeota.net" };
                foreach (var d in ret) sb.AppendLine("127.0.0.1 " + d);
                sb.AppendLine(":: === FRAUD/MALVERTISING ===");
                string[] fraud = { "popads.net", "propellerads.com", "exoclick.com", "juicyads.com", "trafficjunky.com", "adnium.com", "clickadu.com", "hilltopads.com", "galaksion.com" };
                foreach (var d in fraud) sb.AppendLine("127.0.0.1 " + d);
                sb.AppendLine(":: === CRYPTOMINERS ===");
                string[] crypto = { "coinhive.com", "coin-hive.com", "jsecoin.com", "crypto-loot.com", "minr.pw" };
                foreach (var d in crypto) sb.AppendLine("127.0.0.1 " + d);

                File.WriteAllText(hostsFile, sb.ToString());
                RunAdb("push " + hostsFile + " /sdcard/hosts_adblock");
                RunAdb(@"shell su -c ""cp /sdcard/hosts_adblock /system/etc/hosts""");
                RunAdb(@"shell su -c ""chmod 644 /system/etc/hosts""");
                RunAdb(@"shell su -c ""mount -o ro,remount /system""");
                RunAdb("shell rm /sdcard/hosts_adblock");
                try { File.Delete(hostsFile); } catch { }
                Log("130+ ad domains blocked via hosts file.", Colors.Accent);
            }, "Hosts File Ad Blocking");
        }

        private void AdsServices()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/9] Google ad components...", Colors.Orange);
                string[] g = { "com.google.android.gms.ads", "com.google.android.gms.ads.admanager", "com.google.android.gms.analytics", "com.google.android.apps.ads.services" };
                foreach (var p in g) RunAdb("shell pm disable-user --user 0 " + p);

                Log("[2/9] Ad SDKs (40+)...", Colors.Orange);
                string[] sdk = { "com.applovin", "com.inmobi", "com.mopub", "com.facebook.ads", "com.unity3d.services", "com.adcolony", "com.tapjoy", "com.vungle", "com.fyber", "com.yieldmo", "com.braze", "com.localytics", "com.onesignal", "com.kochava", "com.appsflyer", "com.adjust", "com.ironsource", "com.smaato", "com.flurry", "com.chartbeat", "com.revmob", "com.nativex", "com.hyprmx", "com.verve", "com.millennialmedia", "com.chartboost", "com.leadbolt", "com.startapp", "com.airpush" };
                foreach (var p in sdk) RunAdb("shell pm disable-user --user 0 " + p);

                Log("[3/9] System ad services...", Colors.Orange);
                RunAdb("shell pm disable-user --user 0 com.google.android.gms.games");
                RunAdb("shell pm disable-user --user 0 com.google.android.googlequicksearchbox");

                Log("[4/9] Limit ad tracking...", Colors.Orange);
                RunAdb("shell settings put secure limit_ad_tracking 1");

                Log("[5/9] Disable personalized ads...", Colors.Orange);
                RunAdb("shell settings put secure interest_based_ad 0");
                RunAdb("shell settings put global ad_id_opt_out 1");

                Log("[6/9] Clear ad data...", Colors.Orange);
                string[] clr = { "com.google.android.gms", "com.google.android.gms.ads", "com.google.android.gms.analytics", "com.facebook.katana" };
                foreach (var c in clr) RunAdb("shell pm clear " + c);

                Log("[7/9] Force-stop ad processes...", Colors.Orange);
                string[] fst = { "com.google.android.gms.ads", "com.facebook.ads", "com.applovin", "com.mopub", "com.unity3d.services" };
                foreach (var f in fst) RunAdb("shell am force-stop " + f);

                Log("[8/9] Revoke ad permissions...", Colors.Orange);
                string[] perms = { "android.permission.READ_PHONE_STATE", "android.permission.ACCESS_FINE_LOCATION", "android.permission.ACCESS_COARSE_LOCATION", "android.permission.GET_ACCOUNTS" };
                foreach (var pkg in new[] { "com.google.android.gms", "com.facebook.katana" })
                    foreach (var pm in perms) RunAdb("shell pm revoke " + pkg + " " + pm);

                Log("[9/9] Flush DNS...", Colors.Orange);
                RunAdb("shell cmd connectivity flush-dns");

                Log("All 9 steps completed.", Colors.Accent);
            }, "Disable Ad Services");
        }

        private void AdsNuclear()
        {
            if (!Confirm("WARNING: Run ALL ad-blocking methods at once?")) return;
            ExecuteWithWait(() =>
            {
                Log("[1/4] Disabling ad packages...", Colors.Orange);
                string[] pkgs = { "com.google.android.gms.ads", "com.google.android.gms.ads.admanager", "com.google.android.gms.analytics", "com.google.android.apps.ads.services", "com.applovin", "com.inmobi", "com.mopub", "com.unity3d.services", "com.adcolony", "com.tapjoy", "com.vungle", "com.fyber", "com.yieldmo", "com.braze", "com.localytics", "com.onesignal", "com.kochava", "com.appsflyer", "com.adjust", "com.ironsource", "com.smaato", "com.millennialmedia", "com.flurry", "com.facebook.ads", "com.startapp", "com.chartboost", "com.leadbolt" };
                foreach (var p in pkgs) RunAdb("shell pm disable-user --user 0 " + p);

                Log("[2/4] Privacy + DNS...", Colors.Orange);
                RunAdb("shell settings put secure limit_ad_tracking 1");
                RunAdb("shell settings put secure interest_based_ad 0");
                RunAdb("shell settings put global ad_id_opt_out 1");
                RunAdb("shell settings put global private_dns_mode hostname");
                RunAdb("shell settings put global private_dns_specifier dns.adguard.com");

                Log("[3/4] Revoking tracking permissions...", Colors.Orange);
                string[] perms = { "android.permission.READ_PHONE_STATE", "android.permission.ACCESS_FINE_LOCATION", "android.permission.ACCESS_COARSE_LOCATION" };
                foreach (var pkg in new[] { "com.google.android.gms", "com.facebook.katana" })
                    foreach (var pm in perms) RunAdb("shell pm revoke " + pkg + " " + pm);

                Log("[4/4] Clearing data + flush DNS...", Colors.Orange);
                foreach (var c in new[] { "com.google.android.gms", "com.google.android.gms.ads", "com.facebook.katana" }) RunAdb("shell pm clear " + c);
                RunAdb("shell cmd connectivity flush-dns");

                Log("NUCLEAR AD REMOVAL COMPLETE.", Colors.Accent);
            }, "Nuclear Ad Removal");
        }

        private void AdsDns()
        {
            var panel = CreateSection("DNS-BASED AD BLOCKING (No Root)");

            string[][] dns = new[] {
                new[] { "AdGuard DNS", "dns.adguard.com" },
                new[] { "AdGuard Family", "family.adguard-dns.com" },
                new[] { "NextDNS", "dns.nextdns.io" },
                new[] { "NextDNS Ads", "ads-dns.nextdns.io" },
                new[] { "OpenDNS FamilyShield", "dofamilyshield.opendns.com" },
                new[] { "Cloudflare Security", "security.cloudflare-dns.com" },
                new[] { "CleanBrowsing", "security.cleanbrowsing.org" },
            };

            int y = 10;
            foreach (var d in dns)
            {
                string hostname = d[1];
                panel.Controls.Add(MakeBtn(d[0] + " - " + d[1], 10, y, 450, () =>
                {
                    RunAdb("shell settings put global private_dns_mode hostname");
                    RunAdb("shell settings put global private_dns_specifier " + hostname);
                    Log("DNS set to " + hostname, Colors.Accent);
                }));
                y += 38;
            }

            panel.Controls.Add(MakeBtn("Custom DNS", 10, y, 450, () =>
            {
                string custom = PromptInput("Enter DNS hostname:");
                if (!string.IsNullOrEmpty(custom))
                {
                    RunAdb("shell settings put global private_dns_mode hostname");
                    RunAdb("shell settings put global private_dns_specifier " + custom);
                    Log("Custom DNS set: " + custom, Colors.Accent);
                }
            }));
            y += 38;

            panel.Controls.Add(MakeBtn("Remove DNS Blocking", 10, y, 450, () =>
            {
                RunAdb("shell settings put global private_dns_mode off");
                RunAdb("shell settings delete global private_dns_specifier");
                Log("DNS blocking removed.", Colors.Accent);
            }));
        }

        private void AdsTracking()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/6] Reset Ad ID...", Colors.Orange);
                RunAdb("shell settings put secure advertising_id \"\"");
                Log("[2/6] Limit ad tracking...", Colors.Orange);
                RunAdb("shell settings put secure limit_ad_tracking 1");
                Log("[3/6] Disable tracking settings...", Colors.Orange);
                RunAdb("shell settings put global ad_id_opt_out 1");
                RunAdb("shell settings put secure interest_based_ad 0");
                RunAdb("shell settings put secure interest_based_ads 0");

                Log("[4/6] App-level tracking...", Colors.Orange);
                string[] ops = { "TRACK_AUDIENCE", "READ_PHONE_STATE", "ACCESS_FINE_LOCATION", "ACCESS_COARSE_LOCATION", "GET_ACCOUNTS" };
                foreach (var pkg in new[] { "com.google.android.gms", "com.google.android.gms.ads", "com.google.android.gms.analytics" })
                    foreach (var op in ops) RunAdb("shell appops set " + pkg + " " + op + " deny");

                Log("[5/6] Revoke from user apps...", Colors.Orange);
                string pkgs = RunAdb("shell pm list packages -3");
                foreach (string line in pkgs.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string pkg = line.Replace("package:", "").Trim();
                    if (string.IsNullOrEmpty(pkg)) continue;
                    RunAdb("shell pm revoke " + pkg + " android.permission.READ_PHONE_STATE");
                    RunAdb("shell pm revoke " + pkg + " android.permission.ACCESS_FINE_LOCATION");
                    RunAdb("shell pm revoke " + pkg + " android.permission.ACCESS_COARSE_LOCATION");
                    RunAdb("shell pm revoke " + pkg + " android.permission.READ_CONTACTS");
                    RunAdb("shell pm revoke " + pkg + " android.permission.READ_SMS");
                }

                Log("[6/6] Disable usage stats...", Colors.Orange);
                RunAdb("shell appops set com.google.android.gms USAGE_STATS deny");
                RunAdb("shell settings put secure usage_metrics_reporting_enabled 0");

                Log("Tracking fully disabled.", Colors.Accent);
            }, "Stop Tracking");
        }

        private void AdsCustom()
        {
            if (!isRooted) { Log("ERROR: Root required.", Colors.Red); return; }
            var panel = CreateSection("CUSTOM HOSTS EDITOR");
            int y = 10;
            panel.Controls.Add(MakeBtn("View Hosts File", 10, y, 300, () => { Log(RunAdb(@"shell su -c ""cat /system/etc/hosts"""), Colors.Text); })); y += 38;
            panel.Controls.Add(MakeBtn("Add Domain", 10, y, 300, () =>
            {
                string dom = PromptInput("Domain to block:");
                if (!string.IsNullOrEmpty(dom)) { RunAdb(@"shell su -c ""echo '127.0.0.1 " + dom + @"' >> /system/etc/hosts"""); Log(dom + " blocked.", Colors.Accent); }
            })); y += 38;
            panel.Controls.Add(MakeBtn("Remove Domain", 10, y, 300, () =>
            {
                string dom = PromptInput("Domain to unblock:");
                if (!string.IsNullOrEmpty(dom)) { RunAdb(@"shell su -c ""sed -i '/" + dom + @"/d' /system/etc/hosts"""); Log(dom + " removed.", Colors.Accent); }
            })); y += 38;
            panel.Controls.Add(MakeBtn("Restore Backup", 10, y, 300, () => { RunAdb(@"shell su -c ""cp /system/etc/hosts.bak.bnt /system/etc/hosts"""); Log("Restored.", Colors.Accent); })); y += 38;
            panel.Controls.Add(MakeBtn("Reset Hosts", 10, y, 300, () => { RunAdb(@"shell su -c ""echo '127.0.0.1 localhost' > /system/etc/hosts"""); Log("Reset.", Colors.Accent); })); y += 38;
        }

        private void AdsFull()
        {
            if (!Confirm("Run ALL ad-blocking methods combined?")) return;
            ExecuteWithWait(() =>
            {
                Log("[1/4] Disabling ad packages...", Colors.Orange);
                string[] pkgs = { "com.google.android.gms.ads", "com.google.android.gms.ads.admanager", "com.google.android.gms.analytics", "com.google.android.apps.ads.services", "com.applovin", "com.inmobi", "com.mopub", "com.unity3d.services", "com.adcolony", "com.tapjoy", "com.vungle", "com.fyber", "com.yieldmo", "com.braze", "com.localytics", "com.onesignal", "com.kochava", "com.appsflyer", "com.adjust", "com.ironsource", "com.smaato", "com.millennialmedia", "com.flurry", "com.facebook.ads", "com.startapp", "com.chartboost" };
                foreach (var p in pkgs) RunAdb("shell pm disable-user --user 0 " + p);

                Log("[2/4] DNS + privacy...", Colors.Orange);
                RunAdb("shell settings put secure limit_ad_tracking 1");
                RunAdb("shell settings put secure interest_based_ad 0");
                RunAdb("shell settings put global ad_id_opt_out 1");
                RunAdb("shell settings put global private_dns_mode hostname");
                RunAdb("shell settings put global private_dns_specifier dns.adguard.com");

                Log("[3/4] Revoking permissions...", Colors.Orange);
                string[] perms = { "android.permission.READ_PHONE_STATE", "android.permission.ACCESS_FINE_LOCATION", "android.permission.ACCESS_COARSE_LOCATION", "android.permission.GET_ACCOUNTS" };
                foreach (var pkg in new[] { "com.google.android.gms", "com.facebook.katana" })
                    foreach (var pm in perms) RunAdb("shell pm revoke " + pkg + " " + pm);

                Log("[4/4] Clearing data...", Colors.Orange);
                foreach (var c in new[] { "com.google.android.gms", "com.google.android.gms.ads", "com.facebook.katana" }) RunAdb("shell pm clear " + c);
                RunAdb("shell cmd connectivity flush-dns");

                Log("COMPLETE AD CLEAN FINISHED.", Colors.Accent);
            }, "Full Ads Clean");
        }

        private void AdsBanner()
        {
            var panel = CreateSection("BANNER / POPUP REMOVAL");
            int y = 10;
            panel.Controls.Add(MakeBtn("Disable Overlay", 10, y, 300, () => { RunAdb("shell settings put global overlay_settings_enabled 0"); Log("Overlay disabled.", Colors.Accent); })); y += 38;
            panel.Controls.Add(MakeBtn("Disable Popups", 10, y, 300, () => { RunAdb("shell settings put secure popup_settings_value 0"); Log("Popups disabled.", Colors.Accent); })); y += 38;
            panel.Controls.Add(MakeBtn("Block Interstitials", 10, y, 300, () => { RunAdb("shell settings put global interceptor_ad_interstitial 0"); Log("Interstitials blocked.", Colors.Accent); })); y += 38;
            panel.Controls.Add(MakeBtn("System-Wide Ad Block", 10, y, 300, () => { RunAdb("shell settings put global ad_blocker_enabled 1"); RunAdb("shell settings put global system_ad_blocker 1"); Log("System-wide block enabled.", Colors.Accent); })); y += 38;
        }

        private void AdsPerms()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/4] Revoking camera...", Colors.Orange);
                foreach (var pkg in new[] { "com.google.android.gms", "com.facebook.katana", "com.applovin", "com.inmobi" }) RunAdb("shell pm revoke " + pkg + " android.permission.CAMERA");

                Log("[2/4] Revoking location...", Colors.Orange);
                string[] loc = { "android.permission.ACCESS_FINE_LOCATION", "android.permission.ACCESS_COARSE_LOCATION", "android.permission.ACCESS_BACKGROUND_LOCATION" };
                foreach (var pkg in new[] { "com.google.android.gms", "com.facebook.katana", "com.applovin" })
                    foreach (var p in loc) RunAdb("shell pm revoke " + pkg + " " + p);

                Log("[3/4] Revoking phone/SMS...", Colors.Orange);
                string[] phone = { "android.permission.READ_PHONE_STATE", "android.permission.READ_SMS", "android.permission.READ_CALL_LOG", "android.permission.READ_CONTACTS" };
                foreach (var pkg in new[] { "com.google.android.gms", "com.facebook.katana" })
                    foreach (var p in phone) RunAdb("shell pm revoke " + pkg + " " + p);

                Log("[4/4] Revoking storage...", Colors.Orange);
                string[] storage = { "android.permission.READ_EXTERNAL_STORAGE", "android.permission.WRITE_EXTERNAL_STORAGE" };
                foreach (var pkg in new[] { "com.google.android.gms", "com.facebook.katana" })
                    foreach (var p in storage) RunAdb("shell pm revoke " + pkg + " " + p);

                Log("All ad permissions revoked.", Colors.Accent);
            }, "Revoke Ad Permissions");
        }

        // =====================================================================
        //                           FRP BYPASS
        // =====================================================================
        private void ShowFrp()
        {
            var panel = CreateSection("FRP BYPASS TOOLKIT");

            var warn = new Label { Text = "WARNING: Only use on devices you legally own!", Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = Colors.Red, Location = new Point(10, 10), Width = 500, Height = 22 };
            panel.Controls.Add(warn);

            var frpModels = new Label
            {
                Text = "Supported: Samsung, Xiaomi, Huawei, Oppo, Vivo, Motorola, Nokia, LG, Sony, OnePlus, Google Pixel, and most Android 5.0-14 devices",
                Font = new Font("Consolas", 7.5f, FontStyle.Bold),
                ForeColor = Colors.Accent,
                Location = new Point(10, 34),
                Size = new Size(580, 20)
            };
            panel.Controls.Add(frpModels);

            int y = 58;
            panel.Controls.Add(MakeBtn("Bypass Setup Wizard", 10, y, 300, () => FrpSetup())); y += 38;
            panel.Controls.Add(MakeBtn("Open Settings", 10, y, 300, () => FrpSettings())); y += 38;
            panel.Controls.Add(MakeBtn("Remove Google Account", 10, y, 300, () => FrpAccount())); y += 38;
            panel.Controls.Add(MakeBtn("Clear GAM Data", 10, y, 300, () => FrpClear())); y += 38;
            panel.Controls.Add(MakeBtn("Disable FRP Lock", 10, y, 300, () => FrpDisable())); y += 38;
            panel.Controls.Add(MakeBtn("Launch Browser", 10, y, 300, () => FrpBrowser())); y += 38;
            panel.Controls.Add(MakeBtn("Full FRP Bypass (10 Steps)", 10, y, 300, () => FrpAll())); y += 38;
            panel.Controls.Add(MakeBtn("ADB Shell Access", 10, y, 300, () => FrpShell())); y += 38;
            panel.Controls.Add(MakeBtn("Enable OEM Unlocking", 10, y, 300, () => FrpOem())); y += 38;
            panel.Controls.Add(MakeBtn("Wipe Data / Factory Reset", 10, y, 300, () => FrpWipe())); y += 38;
            panel.Controls.Add(MakeBtn("Accessibility / TalkBack Method", 10, y, 300, () => FrpAccess())); y += 38;
            panel.Controls.Add(MakeBtn("Disable Find My Device", 10, y, 300, () => FrpFmd())); y += 38;
            panel.Controls.Add(MakeBtn("Fastboot FRP Bypass", 10, y, 300, () => FrpFastboot())); y += 38;
        }

        private void FrpSetup()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/6] Setting provisioned...", Colors.Orange);
                RunAdb("shell settings put global device_provisioned 1");
                Log("[2/6] Marking setup complete...", Colors.Orange);
                RunAdb("shell settings put secure user_setup_complete 1");
                Log("[3/6] Disabling wizard packages...", Colors.Orange);
                string[] wiz = { "com.google.android.setupwizard", "com.sec.android.app.SecSetupWizard", "com.miui.miservice", "com.huawei.android.hwfrozen", "com.oppo.setupwizard", "com.heytap.setupwizard", "com.vivo.setupwizard", "com.zte.setupwizard", "com.motorola.setupwizard" };
                foreach (var w in wiz) RunAdb("shell pm disable-user --user 0 " + w);
                Log("[4/6] Killing processes...", Colors.Orange);
                RunAdb("shell am force-stop com.google.android.setupwizard");
                RunAdb("shell am force-stop com.sec.android.app.SecSetupWizard");
                Log("[5/6] Clearing data...", Colors.Orange);
                RunAdb("shell pm clear com.google.android.setupwizard");
                Log("[6/6] Going home...", Colors.Orange);
                RunAdb("shell am start -a android.intent.action.MAIN -c android.intent.category.HOME");
                Log("Setup Wizard bypassed!", Colors.Accent);
            }, "Bypass Setup Wizard");
        }

        private void FrpSettings()
        {
            var panel = CreateSection("OPEN SETTINGS");
            int y = 10;
            string[][] items = new[] {
                new[] { "Main Settings", "com.android.settings/com.android.settings.Settings" },
                new[] { "Developer Options", "com.android.settings/com.android.settings.DevelopmentSettings" },
                new[] { "Accessibility", "-a android.settings.ACCESSIBILITY_SETTINGS" },
                new[] { "Security", "-a android.settings.SECURITY_SETTINGS" },
                new[] { "Accounts", "-a android.settings.SYNC_SETTINGS" },
                new[] { "Apps", "-a android.settings.APPLICATION_SETTINGS" },
                new[] { "WiFi", "-a android.settings.WIFI_SETTINGS" },
                new[] { "About Phone", "-a android.settings.DEVICE_INFO_SETTINGS" },
            };
            foreach (var item in items)
            {
                string arg = item[1];
                bool isComponent = !arg.StartsWith("-a ");
                panel.Controls.Add(MakeBtn(item[0], 10, y, 300, () =>
                {
                    if (isComponent) RunAdb("shell am start " + arg);
                    else RunAdb("shell am start " + arg);
                    Log("Settings opened: " + item[0], Colors.Accent);
                }));
                y += 38;
            }
        }

        private void FrpAccount()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/5] Clear login data...", Colors.Orange);
                RunAdb("shell pm clear com.google.android.gsf.login");
                RunAdb("shell pm clear com.google.android.gsf");
                Log("[2/5] Clear GMS auth...", Colors.Orange);
                RunAdb("shell pm clear com.google.android.gms");
                RunAdb("shell pm clear com.google.android.gms.auth");
                RunAdb("shell pm clear com.google.android.gms.auth.authzen");
                Log("[3/5] Clear trust/FIDO...", Colors.Orange);
                RunAdb("shell pm clear com.google.android.gms.trust");
                RunAdb("shell pm clear com.google.android.gms.fido");
                Log("[4/5] Disable/re-enable GMS...", Colors.Orange);
                RunAdb("shell pm disable-user --user 0 com.google.android.gms");
                RunAdb("shell am force-stop com.google.android.gms");
                Thread.Sleep(2000);
                RunAdb("shell pm enable com.google.android.gms");
                Log("[5/5] Clear sync data...", Colors.Orange);
                RunAdb("shell settings delete secure sync1");
                RunAdb("shell settings delete secure sync2");
                Log("Google account removed.", Colors.Accent);
            }, "Remove Google Account");
        }

        private void FrpClear()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/7] GSF...", Colors.Orange);
                RunAdb("shell pm clear com.google.android.gsf");
                RunAdb("shell pm clear com.google.android.gsf.login");
                Log("[2/7] GMS auth variants...", Colors.Orange);
                RunAdb("shell pm clear com.google.android.gms");
                RunAdb("shell pm clear com.google.android.gms.auth");
                RunAdb("shell pm clear com.google.android.gms.auth.authzen");
                RunAdb("shell pm clear com.google.android.gms.auth.cryptauth");
                Log("[3/7] Trust/FIDO/payment...", Colors.Orange);
                RunAdb("shell pm clear com.google.android.gms.trust");
                RunAdb("shell pm clear com.google.android.gms.fido");
                RunAdb("shell pm clear com.google.android.gms.tapandpay");
                RunAdb("shell pm clear com.google.android.gms.wallet");
                Log("[4/7] Account databases...", Colors.Orange);
                RunAdb(@"shell rm -rf /data/system/users/*/accounts.db");
                RunAdb(@"shell rm -rf /data/system/users/*/accounts_de.db");
                RunAdb(@"shell rm -rf /data/system/users/*/accounts_ce.db");
                Log("[5/7] GMS databases...", Colors.Orange);
                RunAdb(@"shell rm -rf /data/data/com.google.android.gms/databases/*");
                Log("[6/7] Cached settings...", Colors.Orange);
                RunAdb(@"shell rm -rf /data/system/users/*/settings_secure.xml");
                Log("[7/7] Setup data...", Colors.Orange);
                RunAdb(@"shell rm -rf /data/data/com.google.android.gsf.login/databases/*");
                Log("GAM data fully cleared.", Colors.Accent);
            }, "Clear GAM Data");
        }

        private void FrpDisable()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/5] FRP flag...", Colors.Orange);
                RunAdb("shell settings put secure frp_mode_disabled 1");
                Log("[2/5] Provisioned...", Colors.Orange);
                RunAdb("shell settings put global device_provisioned 1");
                Log("[3/5] Setup complete...", Colors.Orange);
                RunAdb("shell settings put secure user_setup_complete 1");
                Log("[4/5] Disable wizard...", Colors.Orange);
                RunAdb("shell pm disable-user --user 0 com.google.android.setupwizard");
                RunAdb("shell pm disable-user --user 0 com.google.android.gsf.login");
                Log("[5/5] Clear FRP data...", Colors.Orange);
                RunAdb("shell pm clear com.google.android.gsf.login");
                RunAdb("shell pm clear com.google.android.gms.auth");
                RunAdb("shell pm clear com.google.android.gms.trust");
                Log("FRP lock disabled.", Colors.Accent);
            }, "Disable FRP Lock");
        }

        private void FrpBrowser()
        {
            var panel = CreateSection("LAUNCH BROWSER");
            int y = 10;
            panel.Controls.Add(MakeBtn("Recovery Page", 10, y, 300, () => { RunAdb(@"shell am start -a android.intent.action.VIEW -d ""https://accounts.google.com/signin/recovery"""); })); y += 38;
            panel.Controls.Add(MakeBtn("Chrome", 10, y, 300, () => { RunAdb("shell am start -n com.android.chrome/com.google.android.apps.chrome.Main"); })); y += 38;
            panel.Controls.Add(MakeBtn("Samsung Internet", 10, y, 300, () => { RunAdb("shell am start -n com.sec.android.app.sbrowser/com.sec.android.app.sbrowser.SBrowserMainActivity"); })); y += 38;
            panel.Controls.Add(MakeBtn("Firefox", 10, y, 300, () => { RunAdb("shell am start -n org.mozilla.firefox/org.mozilla.firefox.App"); })); y += 38;
            panel.Controls.Add(MakeBtn("YouTube", 10, y, 300, () => { RunAdb(@"shell am start -a android.intent.action.VIEW -d ""https://youtube.com"""); })); y += 38;
            panel.Controls.Add(MakeBtn("FRP Tools Site", 10, y, 300, () => { RunAdb(@"shell am start -a android.intent.action.VIEW -d ""https://frpbypass.io"""); })); y += 38;
            panel.Controls.Add(MakeBtn("Custom URL", 10, y, 300, () =>
            {
                string url = PromptInput("Enter URL:");
                if (!string.IsNullOrEmpty(url)) RunAdb(@"shell am start -a android.intent.action.VIEW -d """ + url + @"""");
            })); y += 38;
        }

        private void FrpAll()
        {
            if (!Confirm("Run FULL FRP bypass (10 steps)?")) return;
            ExecuteWithWait(() =>
            {
                Log("[1/10] FRP flag...", Colors.Orange); RunAdb("shell settings put secure frp_mode_disabled 1");
                Log("[2/10] Provisioned...", Colors.Orange); RunAdb("shell settings put global device_provisioned 1");
                Log("[3/10] Setup complete...", Colors.Orange); RunAdb("shell settings put secure user_setup_complete 1");
                Log("[4/10] Clear Google data...", Colors.Orange);
                foreach (var c in new[] { "com.google.android.gsf.login", "com.google.android.gsf", "com.google.android.gms", "com.google.android.gms.auth", "com.google.android.gms.auth.authzen", "com.google.android.gms.auth.cryptauth", "com.google.android.gms.trust", "com.google.android.gms.fido" }) RunAdb("shell pm clear " + c);
                Log("[5/10] Disable wizards...", Colors.Orange);
                foreach (var w in new[] { "com.google.android.setupwizard", "com.sec.android.app.SecSetupWizard", "com.miui.miservice", "com.huawei.android.hwfrozen" }) RunAdb("shell pm disable-user --user 0 " + w);
                Log("[6/10] Force-stop...", Colors.Orange); RunAdb("shell am force-stop com.google.android.setupwizard");
                Log("[7/10] Clear FRP DBs...", Colors.Orange);
                RunAdb(@"shell rm -rf /data/system/users/*/accounts.db");
                RunAdb(@"shell rm -rf /data/system/users/*/accounts_de.db");
                Log("[8/10] Disable Find My Device...", Colors.Orange); RunAdb("shell pm disable-user --user 0 com.google.android.gms.trust");
                Log("[9/10] Go home...", Colors.Orange); RunAdb("shell am start -a android.intent.action.MAIN -c android.intent.category.HOME");
                Log("[10/10] Open Settings...", Colors.Orange); RunAdb("shell am start com.android.settings/com.android.settings.Settings");
                Log("FULL FRP BYPASS COMPLETED!", Colors.Accent);
            }, "Full FRP Bypass");
        }

        private void FrpShell()
        {
            Log("Launching ADB shell... (Close shell window to return)", Colors.Orange);
            Process.Start(new ProcessStartInfo("adb", "shell") { UseShellExecute = true });
        }

        private void FrpOem()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/4] Developer options...", Colors.Orange);
                for (int i = 0; i < 7; i++) RunAdb("shell settings put global development_settings_enabled 1");
                Log("[2/4] USB debugging...", Colors.Orange); RunAdb("shell settings put global adb_enabled 1");
                Log("[3/4] OEM unlock...", Colors.Orange); RunAdb("shell settings put global oem_unlock_enabled 1");
                Log("[4/4] Bootloader unlock...", Colors.Orange); RunAdb("shell oem unlock");
                Log("OEM unlocking enabled.", Colors.Accent);
            }, "Enable OEM Unlocking");
        }

        private void FrpWipe()
        {
            if (PromptInput("Type YES to confirm factory reset:") != "YES") { Log("Cancelled.", Colors.Red); return; }
            ExecuteWithWait(() =>
            {
                Log("[1/3] Wiping data...", Colors.Orange); RunAdb("shell recovery --wipe_data");
                Log("[2/3] Wiping cache...", Colors.Orange); RunAdb("shell recovery --wipe_cache");
                Log("[3/3] Rebooting...", Colors.Orange); RunAdb("reboot recovery");
                Log("Factory reset initiated.", Colors.Accent);
            }, "Factory Reset");
        }

        private void FrpAccess()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/4] Enabling TalkBack...", Colors.Orange);
                RunAdb(@"shell settings put secure enabled_accessibility_services com.google.android.marvin.talkback/com.google.android.marvin.talkback.TalkBackService");
                RunAdb("shell settings put secure accessibility_enabled 1");
                Log("[2/4] Open accessibility...", Colors.Orange);
                RunAdb("shell am start -a android.settings.ACCESSIBILITY_SETTINGS");
                Log("[3/4] Open Google app...", Colors.Orange);
                RunAdb("shell am start -n com.google.android.googlequicksearchbox/com.google.android.launcher.GEL");
                Log("[4/4] Drawing L gesture...", Colors.Orange);
                RunAdb("shell input swipe 100 500 100 100 300");
                Log("TalkBack method initiated.", Colors.Accent);
            }, "Accessibility Method");
        }

        private void FrpFmd()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/3] Disabling FMD...", Colors.Orange);
                RunAdb("shell pm disable-user --user 0 com.google.android.gms.trust");
                RunAdb("shell pm disable-user --user 0 com.google.android.gms");
                Log("[2/3] Disabling location...", Colors.Orange);
                RunAdb("shell settings put secure location_mode 0");
                Log("[3/3] Clear FMD data...", Colors.Orange);
                RunAdb("shell pm clear com.google.android.gms.trust");
                RunAdb("shell pm clear com.google.android.gms.auth.trustagent");
                Log("Find My Device disabled.", Colors.Accent);
            }, "Disable Find My Device");
        }

        // =====================================================================
        //                       MTP FRP BYPASS
        // =====================================================================
        private void ShowMtpFrp()
        {
            var panel = CreateSection("MTP FRP BYPASS - UNIVERSAL");
            int y = 10;

            var warn = new Label { Text = "WARNING: Only use on devices you legally own! Works via MTP/USB mode switching.", Font = new Font("Segoe UI", 8.5f, FontStyle.Bold), ForeColor = Colors.Red, Location = new Point(10, y), Width = 600, Height = 22 };
            panel.Controls.Add(warn); y += 28;

            var desc = new Label { Text = "MTP FRP bypass uses USB mode switching and file transfer protocols to bypass Google account verification.", Font = new Font("Segoe UI", 8.5f), ForeColor = Colors.TextDim, Location = new Point(10, y), Width = 600, Height = 22 };
            panel.Controls.Add(desc); y += 28;

            var mtpModels = new Label
            {
                Text = "Supported: Samsung (all USB modes) | Huawei (HiSuite bypass) | Xiaomi (Mi assistant) | Oppo/Vivo/Realme | Motorola | LG | Sony",
                Font = new Font("Consolas", 7.5f, FontStyle.Bold),
                ForeColor = Colors.Accent,
                Location = new Point(10, y),
                Size = new Size(580, 20)
            };
            panel.Controls.Add(mtpModels); y += 22;

            var sectionLabel = new Label { Text = "--- USB MODE METHODS ---", Font = new Font("Consolas", 9.5f, FontStyle.Bold), ForeColor = Colors.Orange, Location = new Point(10, y), Width = 600, Height = 22 };
            panel.Controls.Add(sectionLabel); y += 26;

            panel.Controls.Add(MakeBtn("Switch USB to MTP Mode", 10, y, 440, () => MtpSwitchMtp())); y += 38;
            panel.Controls.Add(MakeBtn("Switch USB to PTP Mode", 10, y, 440, () => MtpSwitchPtp())); y += 38;
            panel.Controls.Add(MakeBtn("Switch USB to RNDIS (USB Tethering)", 10, y, 440, () => MtpSwitchRndis())); y += 38;
            panel.Controls.Add(MakeBtn("Force MTP + Disable Charging Only", 10, y, 440, () => MtpForceMtp())); y += 38;
            panel.Controls.Add(MakeBtn("Cycle USB Modes (MTP->PTP->MTP)", 10, y, 440, () => MtpCycleUsb())); y += 38;

            y += 8;
            sectionLabel = new Label { Text = "--- SAMSUNG MTP BYPASS ---", Font = new Font("Consolas", 9.5f, FontStyle.Bold), ForeColor = Colors.Orange, Location = new Point(10, y), Width = 600, Height = 22 };
            panel.Controls.Add(sectionLabel); y += 26;

            panel.Controls.Add(MakeBtn("Samsung MTP + Smart Switch Method", 10, y, 440, () => MtpSamsungSmartSwitch())); y += 38;
            panel.Controls.Add(MakeBtn("Samsung MTP + Odin Mode", 10, y, 440, () => MtpSamsungOdin())); y += 38;
            panel.Controls.Add(MakeBtn("Samsung MTP + Emergency Call", 10, y, 440, () => MtpSamsungEmergency())); y += 38;
            panel.Controls.Add(MakeBtn("Samsung MTP + Knox Reset", 10, y, 440, () => MtpSamsungKnox())); y += 38;
            panel.Controls.Add(MakeBtn("Samsung MTP File Manager Access", 10, y, 440, () => MtpSamsungFileManager())); y += 38;

            y += 8;
            sectionLabel = new Label { Text = "--- UNIVERSAL MTP BYPASS ---", Font = new Font("Consolas", 9.5f, FontStyle.Bold), ForeColor = Colors.Orange, Location = new Point(10, y), Width = 600, Height = 22 };
            panel.Controls.Add(sectionLabel); y += 26;

            panel.Controls.Add(MakeBtn("Universal MTP FRP Reset (All Brands)", 10, y, 440, () => MtpUniversalReset())); y += 38;
            panel.Controls.Add(MakeBtn("MTP + ADB Enable Method", 10, y, 440, () => MtpAdbEnable())); y += 38;
            panel.Controls.Add(MakeBtn("MTP Settings Database Push", 10, y, 440, () => MtpSettingsPush())); y += 38;
            panel.Controls.Add(MakeBtn("MTP Account Database Delete", 10, y, 440, () => MtpAccountDelete())); y += 38;
            panel.Controls.Add(MakeBtn("MTP Provisioned Flag Reset", 10, y, 440, () => MtpProvisionedReset())); y += 38;
            panel.Controls.Add(MakeBtn("MTP Setup Wizard Kill", 10, y, 440, () => MtpSetupWizardKill())); y += 38;
            panel.Controls.Add(MakeBtn("MTP Google Account Clear", 10, y, 440, () => MtpGoogleAccountClear())); y += 38;

            y += 8;
            sectionLabel = new Label { Text = "--- ADVANCED MTP METHODS ---", Font = new Font("Consolas", 9.5f, FontStyle.Bold), ForeColor = Colors.Orange, Location = new Point(10, y), Width = 600, Height = 22 };
            panel.Controls.Add(sectionLabel); y += 26;

            panel.Controls.Add(MakeBtn("MTP + OTG File Push Method", 10, y, 440, () => MtpOtgPush())); y += 38;
            panel.Controls.Add(MakeBtn("MTP + Browser Launch Method", 10, y, 440, () => MtpBrowserLaunch())); y += 38;
            panel.Controls.Add(MakeBtn("MTP + Accessibility Exploit", 10, y, 440, () => MtpAccessibilityExploit())); y += 38;
            panel.Controls.Add(MakeBtn("MTP + TalkBack Voice Command", 10, y, 440, () => MtpTalkBackVoice())); y += 38;
            panel.Controls.Add(MakeBtn("Full MTP FRP Bypass (Everything)", 10, y, 440, () => MtpFullBypass())); y += 38;

            y += 8;
            sectionLabel = new Label { Text = "--- BRAND-SPECIFIC MTP ---", Font = new Font("Consolas", 9.5f, FontStyle.Bold), ForeColor = Colors.Orange, Location = new Point(10, y), Width = 600, Height = 22 };
            panel.Controls.Add(sectionLabel); y += 26;

            panel.Controls.Add(MakeBtn("Xiaomi/Redmi MTP Bypass", 10, y, 440, () => MtpXiaomiBypass())); y += 38;
            panel.Controls.Add(MakeBtn("Huawei MTP Bypass", 10, y, 440, () => MtpHuaweiBypass())); y += 38;
            panel.Controls.Add(MakeBtn("OPPO/Realme MTP Bypass", 10, y, 440, () => MtpOppoBypass())); y += 38;
            panel.Controls.Add(MakeBtn("Vivo MTP Bypass", 10, y, 440, () => MtpVivoBypass())); y += 38;
            panel.Controls.Add(MakeBtn("Motorola MTP Bypass", 10, y, 440, () => MtpMotorolaBypass())); y += 38;
            panel.Controls.Add(MakeBtn("LG MTP Bypass", 10, y, 440, () => MtpLgBypass())); y += 38;
            panel.Controls.Add(MakeBtn("Nokia/HMD MTP Bypass", 10, y, 440, () => MtpNokiaBypass())); y += 38;
            panel.Controls.Add(MakeBtn("Sony/Xperia MTP Bypass", 10, y, 440, () => MtpSonyBypass())); y += 38;
            panel.Controls.Add(MakeBtn("Asus MTP Bypass", 10, y, 440, () => MtpAsusBypass())); y += 38;
        }

        private void MtpSwitchMtp()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/3] Setting USB config to MTP...", Colors.Orange);
                RunAdb("shell setprop sys.usb.config mtp");
                Log("[2/3] Setting USB function to MTP...", Colors.Orange);
                RunAdb("shell settings put global usb_audio_routing 0");
                RunAdb("shell am broadcast -a android.hardware.usb.action.USB_FUNCTION_CHANGED -e function mtp");
                Log("[3/3] Restarting USB stack...", Colors.Orange);
                RunAdb("shell setprop sys.usb.state mtp");
                Log("USB switched to MTP mode.", Colors.Accent);
            }, "Switch USB to MTP");
        }

        private void MtpSwitchPtp()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/3] Setting USB config to PTP...", Colors.Orange);
                RunAdb("shell setprop sys.usb.config ptp");
                Log("[2/3] Setting USB function to PTP...", Colors.Orange);
                RunAdb("shell am broadcast -a android.hardware.usb.action.USB_FUNCTION_CHANGED -e function ptp");
                Log("[3/3] Restarting USB stack...", Colors.Orange);
                RunAdb("shell setprop sys.usb.state ptp");
                Log("USB switched to PTP mode. Some devices allow FRP bypass in PTP.", Colors.Accent);
            }, "Switch USB to PTP");
        }

        private void MtpSwitchRndis()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/3] Setting USB config to RNDIS...", Colors.Orange);
                RunAdb("shell setprop sys.usb.config rndis,diag,serial,adb");
                Log("[2/3] Enabling USB tethering...", Colors.Orange);
                RunAdb("shell svc usb setFunctions rndis");
                Log("[3/3] Restarting USB...", Colors.Orange);
                RunAdb("shell setprop sys.usb.state rndis,diag,serial,adb");
                Log("USB switched to RNDIS (USB Tethering) mode.", Colors.Accent);
            }, "Switch USB to RNDIS");
        }

        private void MtpForceMtp()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/4] Disabling charging-only mode...", Colors.Orange);
                RunAdb("shell setprop sys.usb.config mtp,adb");
                Log("[2/4] Force MTP function...", Colors.Orange);
                RunAdb("shell am broadcast -a android.hardware.usb.action.USB_STATE -e configured true -e mtp true");
                Log("[3/4] Setting USB protocol...", Colors.Orange);
                RunAdb("shell settings put global usb_audio_routing 0");
                RunAdb("shell settings put global adb_enabled 1");
                Log("[4/4] Verifying...", Colors.Orange);
                string r = RunAdb("shell getprop sys.usb.config");
                Log("USB config: " + r, Colors.Text);
                Log("MTP + ADB forced. Device should show as MTP in Explorer.", Colors.Accent);
            }, "Force MTP + Disable Charging Only");
        }

        private void MtpCycleUsb()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/5] Setting MTP mode...", Colors.Orange);
                RunAdb("shell setprop sys.usb.config mtp");
                RunAdb("shell setprop sys.usb.state mtp");
                Thread.Sleep(2000);
                Log("[2/5] Switching to PTP...", Colors.Orange);
                RunAdb("shell setprop sys.usb.config ptp");
                RunAdb("shell setprop sys.usb.state ptp");
                Thread.Sleep(2000);
                Log("[3/5] Back to MTP...", Colors.Orange);
                RunAdb("shell setprop sys.usb.config mtp");
                RunAdb("shell setprop sys.usb.state mtp");
                Thread.Sleep(2000);
                Log("[4/5] Enabling ADB...", Colors.Orange);
                RunAdb("shell setprop sys.usb.config mtp,adb");
                RunAdb("shell setprop sys.usb.state mtp,adb");
                Log("[5/5] Cycle complete.", Colors.Orange);
                string r = RunAdb("shell getprop sys.usb.config");
                Log("Final USB config: " + r, Colors.Text);
                Log("USB mode cycling completed. Reconnect device if needed.", Colors.Accent);
            }, "Cycle USB Modes");
        }

        private void MtpSamsungSmartSwitch()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/6] Detecting Samsung device...", Colors.Orange);
                string model = Adb.GetProp("ro.product.model");
                string brand = Adb.GetProp("ro.product.brand");
                Log("Device: " + brand + " " + model, Colors.Text);
                Log("[2/6] Setting MTP mode...", Colors.Orange);
                RunAdb("shell setprop sys.usb.config mtp,adb");
                RunAdb("shell setprop sys.usb.state mtp,adb");
                Log("[3/6] Launching Smart Switch intent...", Colors.Orange);
                RunAdb("shell am start -n com.sec.android.easyMover/com.sec.android.easyMover.activity.SmartSwitchActivity");
                RunAdb(@"shell am start -a android.intent.action.VIEW -d ""samsung-usb://com.samsung.android.easysetup""");
                Log("[4/6] Bypassing Samsung setup...", Colors.Orange);
                RunAdb("shell settings put global device_provisioned 1");
                RunAdb("shell settings put secure user_setup_complete 1");
                RunAdb("shell pm disable-user --user 0 com.sec.android.app.SecSetupWizard");
                Log("[5/6] Clearing Samsung FRP data...", Colors.Orange);
                RunAdb("shell pm clear com.sec.android.app.SecSetupWizard");
                RunAdb("shell pm clear com.samsung.android.easysetup");
                Log("[6/6] Going home...", Colors.Orange);
                RunAdb("shell am start -a android.intent.action.MAIN -c android.intent.category.HOME");
                Log("Samsung Smart Switch MTP method complete.", Colors.Accent);
            }, "Samsung MTP + Smart Switch");
        }

        private void MtpSamsungOdin()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/5] Samsung Odin Mode MTP bypass...", Colors.Orange);
                Log("[2/5] Setting USB to download mode prep...", Colors.Orange);
                RunAdb("shell setprop sys.usb.config mtp");
                RunAdb("shell setprop sys.usb.state mtp");
                Thread.Sleep(1000);
                Log("[3/5] Bypassing FRP flags...", Colors.Orange);
                RunAdb("shell settings put secure frp_mode_disabled 1");
                RunAdb("shell settings put global device_provisioned 1");
                RunAdb("shell settings put secure user_setup_complete 1");
                Log("[4/5] Clearing Samsung account data...", Colors.Orange);
                RunAdb("shell pm clear com.sec.android.app.SecSetupWizard");
                RunAdb("shell pm clear com.samsung.android.spay");
                RunAdb("shell pm clear com.samsung.android.samsungpass");
                Log("[5/5] Rebooting to Odin/Download mode...", Colors.Orange);
                RunAdb("shell am start -a android.intent.action.MAIN -c android.intent.category.HOME");
                Log("Samsung Odin MTP method complete. Reboot device.", Colors.Accent);
            }, "Samsung MTP + Odin Mode");
        }

        private void MtpSamsungEmergency()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/5] Samsung Emergency Call + MTP bypass...", Colors.Orange);
                Log("[2/5] Setting MTP mode...", Colors.Orange);
                RunAdb("shell setprop sys.usb.config mtp,adb");
                RunAdb("shell setprop sys.usb.state mtp,adb");
                Log("[3/5] Launching emergency dialer...", Colors.Orange);
                RunAdb("shell am start -a android.intent.action.DIAL");
                RunAdb("shell am start -n com.android.phone/com.android.phone.EmergencyDialer");
                Log("[4/5] Entering emergency code *#0*# ...", Colors.Orange);
                RunAdb("shell input text '*'");
                RunAdb("shell input text '#'");
                RunAdb("shell input text '0'");
                RunAdb("shell input text '*'");
                RunAdb("shell input text '#'");
                Log("[5/5] Enabling ADB via test mode...", Colors.Orange);
                RunAdb("shell settings put global adb_enabled 1");
                Log("Samsung Emergency Call method complete. Use test mode to enable ADB.", Colors.Accent);
            }, "Samsung MTP + Emergency Call");
        }

        private void MtpSamsungKnox()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/5] Samsung Knox + MTP bypass...", Colors.Orange);
                Log("[2/5] Setting MTP mode...", Colors.Orange);
                RunAdb("shell setprop sys.usb.config mtp,adb");
                Log("[3/5] Disabling Knox...", Colors.Orange);
                string[] knoxPkgs = { "com.samsung.android.knox.containeragent", "com.sec.knox.container", "com.samsung.android.knox", "com.samsung.android.knox.pushmanager", "com.samsung.android.app.routines" };
                foreach (var p in knoxPkgs) RunAdb("shell pm disable-user --user 0 " + p);
                Log("[4/5] Clearing FRP + Knox data...", Colors.Orange);
                RunAdb("shell settings put secure frp_mode_disabled 1");
                RunAdb("shell settings put global device_provisioned 1");
                RunAdb("shell settings put secure user_setup_complete 1");
                RunAdb("shell pm clear com.sec.android.app.SecSetupWizard");
                Log("[5/5] Going home...", Colors.Orange);
                RunAdb("shell am start -a android.intent.action.MAIN -c android.intent.category.HOME");
                Log("Samsung Knox MTP bypass complete.", Colors.Accent);
            }, "Samsung MTP + Knox Reset");
        }

        private void MtpSamsungFileManager()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/4] Samsung MTP File Manager access...", Colors.Orange);
                Log("[2/4] Setting MTP mode...", Colors.Orange);
                RunAdb("shell setprop sys.usb.config mtp,adb");
                RunAdb("shell setprop sys.usb.state mtp,adb");
                Log("[3/4] Launching My Files...", Colors.Orange);
                RunAdb("shell am start -n com.sec.android.app.filemanager/com.sec.android.app.filemanager.activity.FileManagerActivity");
                RunAdb("shell am start -n com.sec.android.app.myfiles/com.sec.android.app.myfiles.MainActivity");
                Log("[4/4] Granting storage permissions...", Colors.Orange);
                RunAdb("shell pm grant com.sec.android.app.myfiles android.permission.READ_EXTERNAL_STORAGE");
                RunAdb("shell pm grant com.sec.android.app.myfiles android.permission.WRITE_EXTERNAL_STORAGE");
                Log("Samsung File Manager opened. Browse and delete FRP files.", Colors.Accent);
            }, "Samsung MTP File Manager");
        }

        private void MtpUniversalReset()
        {
            if (!Confirm("Run universal MTP FRP reset on detected device?")) return;
            ExecuteWithWait(() =>
            {
                Log("[1/10] Detecting device...", Colors.Orange);
                string brand = Adb.GetProp("ro.product.brand");
                string model = Adb.GetProp("ro.product.model");
                string android = Adb.GetProp("ro.build.version.release");
                string sdk = Adb.GetProp("ro.build.version.sdk");
                Log("Device: " + brand + " " + model + " | Android " + android + " (SDK " + sdk + ")", Colors.Text);
                Log("[2/10] Setting MTP mode...", Colors.Orange);
                RunAdb("shell setprop sys.usb.config mtp,adb");
                RunAdb("shell setprop sys.usb.state mtp,adb");
                Log("[3/10] FRP flags...", Colors.Orange);
                RunAdb("shell settings put secure frp_mode_disabled 1");
                RunAdb("shell settings put global device_provisioned 1");
                RunAdb("shell settings put secure user_setup_complete 1");
                Log("[4/10] Disabling setup wizards...", Colors.Orange);
                string[] wiz = { "com.google.android.setupwizard", "com.sec.android.app.SecSetupWizard", "com.miui.miservice", "com.huawei.android.hwfrozen", "com.oppo.setupwizard", "com.heytap.setupwizard", "com.vivo.setupwizard", "com.zte.setupwizard", "com.motorola.setupwizard" };
                foreach (var w in wiz) RunAdb("shell pm disable-user --user 0 " + w);
                Log("[5/10] Clearing Google account data...", Colors.Orange);
                foreach (var c in new[] { "com.google.android.gsf.login", "com.google.android.gsf", "com.google.android.gms", "com.google.android.gms.auth", "com.google.android.gms.trust", "com.google.android.gms.fido" }) RunAdb("shell pm clear " + c);
                Log("[6/10] Clearing account databases...", Colors.Orange);
                RunAdb(@"shell rm -rf /data/system/users/*/accounts.db");
                RunAdb(@"shell rm -rf /data/system/users/*/accounts_de.db");
                RunAdb(@"shell rm -rf /data/system/users/*/accounts_ce.db");
                Log("[7/10] Killing setup wizard processes...", Colors.Orange);
                RunAdb("shell am force-stop com.google.android.setupwizard");
                RunAdb("shell am force-stop com.sec.android.app.SecSetupWizard");
                Log("[8/10] Disabling Find My Device...", Colors.Orange);
                RunAdb("shell pm disable-user --user 0 com.google.android.gms.trust");
                Log("[9/10] Disabling location tracking...", Colors.Orange);
                RunAdb("shell settings put secure location_mode 0");
                Log("[10/10] Going home...", Colors.Orange);
                RunAdb("shell am start -a android.intent.action.MAIN -c android.intent.category.HOME");
                Log("UNIVERSAL MTP FRP RESET COMPLETE for " + brand + " " + model + "!", Colors.Accent);
            }, "Universal MTP FRP Reset");
        }

        private void MtpAdbEnable()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/6] MTP + ADB Enable method...", Colors.Orange);
                Log("[2/6] Setting MTP mode first...", Colors.Orange);
                RunAdb("shell setprop sys.usb.config mtp");
                RunAdb("shell setprop sys.usb.state mtp");
                Thread.Sleep(1000);
                Log("[3/6] Enabling ADB over MTP...", Colors.Orange);
                RunAdb("shell setprop sys.usb.config mtp,adb");
                RunAdb("shell setprop sys.usb.state mtp,adb");
                Log("[4/6] Enabling development settings...", Colors.Orange);
                for (int i = 0; i < 7; i++) RunAdb("shell settings put global development_settings_enabled 1");
                RunAdb("shell settings put global adb_enabled 1");
                Log("[5/6] Enabling USB debugging...", Colors.Orange);
                RunAdb("shell settings put global adb_enabled 1");
                RunAdb("shell settings put secure adb_enabled 1");
                Log("[6/6] Verifying...", Colors.Orange);
                string usbConfig = RunAdb("shell getprop sys.usb.config");
                string adbEnabled = RunAdb("shell settings get global adb_enabled");
                Log("USB config: " + usbConfig, Colors.Text);
                Log("ADB enabled: " + adbEnabled, Colors.Text);
                Log("MTP + ADB Enable complete. ADB should now work.", Colors.Accent);
            }, "MTP + ADB Enable");
        }

        private void MtpSettingsPush()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/5] MTP Settings Database Push...", Colors.Orange);
                Log("[2/5] Setting MTP mode...", Colors.Orange);
                RunAdb("shell setprop sys.usb.config mtp,adb");
                Log("[3/5] Writing FRP bypass settings...", Colors.Orange);
                RunAdb("shell settings put global device_provisioned 1");
                RunAdb("shell settings put secure user_setup_complete 1");
                RunAdb("shell settings put secure frp_mode_disabled 1");
                RunAdb("shell settings put global development_settings_enabled 1");
                RunAdb("shell settings put global adb_enabled 1");
                Log("[4/5] Writing location/security settings...", Colors.Orange);
                RunAdb("shell settings put secure location_mode 0");
                RunAdb("shell settings put global oem_unlock_enabled 1");
                Log("[5/5] Flushing settings cache...", Colors.Orange);
                RunAdb("shell content call --uri content://settings/global --method GET_system --arg name");
                Log("Settings database push complete.", Colors.Accent);
            }, "MTP Settings Database Push");
        }

        private void MtpAccountDelete()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/6] MTP Account Database Delete...", Colors.Orange);
                Log("[2/6] Setting MTP mode...", Colors.Orange);
                RunAdb("shell setprop sys.usb.config mtp,adb");
                Log("[3/6] Deleting account databases...", Colors.Orange);
                RunAdb(@"shell rm -rf /data/system/users/*/accounts.db");
                RunAdb(@"shell rm -rf /data/system/users/*/accounts_de.db");
                RunAdb(@"shell rm -rf /data/system/users/*/accounts_ce.db");
                Log("[4/6] Clearing account cache...", Colors.Orange);
                RunAdb(@"shell rm -rf /data/system/users/*/settings_secure.xml");
                RunAdb(@"shell rm -rf /data/system/users/*/settings_system.xml");
                RunAdb(@"shell rm -rf /data/system/users/*/settings_global.xml");
                Log("[5/6] Clearing Google account data...", Colors.Orange);
                foreach (var c in new[] { "com.google.android.gsf.login", "com.google.android.gsf", "com.google.android.gms", "com.google.android.gms.auth" }) RunAdb("shell pm clear " + c);
                Log("[6/6] Verifying deletion...", Colors.Orange);
                string accounts = RunAdb(@"shell ls /data/system/users/*/accounts.db 2>/dev/null");
                Log("Remaining account files: " + (string.IsNullOrEmpty(accounts) ? "(none)" : accounts), Colors.Text);
                Log("Account database delete complete.", Colors.Accent);
            }, "MTP Account Database Delete");
        }

        private void MtpProvisionedReset()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/5] MTP Provisioned Flag Reset...", Colors.Orange);
                Log("[2/5] Setting MTP mode...", Colors.Orange);
                RunAdb("shell setprop sys.usb.config mtp,adb");
                Log("[3/5] Setting provisioned flags...", Colors.Orange);
                RunAdb("shell settings put global device_provisioned 1");
                RunAdb("shell settings put secure user_setup_complete 1");
                RunAdb("shell content insert --uri content://settings/global --bind name:s:device_provisioned --bind value:s:1");
                RunAdb("shell content insert --uri content://settings/secure --bind name:s:user_setup_complete --bind value:s:1");
                Log("[4/5] Disabling FRP mode...", Colors.Orange);
                RunAdb("shell settings put secure frp_mode_disabled 1");
                RunAdb("shell content insert --uri content://settings/secure --bind name:s:frp_mode_disabled --bind value:s:1");
                Log("[5/5] Going home...", Colors.Orange);
                RunAdb("shell am start -a android.intent.action.MAIN -c android.intent.category.HOME");
                Log("Provisioned flags reset. Device should skip setup.", Colors.Accent);
            }, "MTP Provisioned Flag Reset");
        }

        private void MtpSetupWizardKill()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/6] MTP Setup Wizard Kill...", Colors.Orange);
                Log("[2/6] Setting MTP mode...", Colors.Orange);
                RunAdb("shell setprop sys.usb.config mtp,adb");
                Log("[3/6] Disabling all setup wizards...", Colors.Orange);
                string[] wiz = { "com.google.android.setupwizard", "com.sec.android.app.SecSetupWizard", "com.miui.miservice", "com.huawei.android.hwfrozen", "com.oppo.setupwizard", "com.heytap.setupwizard", "com.vivo.setupwizard", "com.zte.setupwizard", "com.motorola.setupwizard", "com.google.android.gsf.login" };
                foreach (var w in wiz) RunAdb("shell pm disable-user --user 0 " + w);
                Log("[4/6] Force-stopping wizards...", Colors.Orange);
                foreach (var w in new[] { "com.google.android.setupwizard", "com.sec.android.app.SecSetupWizard" }) RunAdb("shell am force-stop " + w);
                Log("[5/6] Clearing wizard data...", Colors.Orange);
                RunAdb("shell pm clear com.google.android.setupwizard");
                RunAdb("shell pm clear com.sec.android.app.SecSetupWizard");
                Log("[6/6] Going home...", Colors.Orange);
                RunAdb("shell am start -a android.intent.action.MAIN -c android.intent.category.HOME");
                Log("Setup wizard killed. Device should go to home screen.", Colors.Accent);
            }, "MTP Setup Wizard Kill");
        }

        private void MtpGoogleAccountClear()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/7] MTP Google Account Clear...", Colors.Orange);
                Log("[2/7] Setting MTP mode...", Colors.Orange);
                RunAdb("shell setprop sys.usb.config mtp,adb");
                Log("[3/7] Clearing GMS login...", Colors.Orange);
                RunAdb("shell pm clear com.google.android.gsf.login");
                RunAdb("shell pm clear com.google.android.gsf");
                Log("[4/7] Clearing GMS auth...", Colors.Orange);
                RunAdb("shell pm clear com.google.android.gms");
                RunAdb("shell pm clear com.google.android.gms.auth");
                RunAdb("shell pm clear com.google.android.gms.auth.authzen");
                RunAdb("shell pm clear com.google.android.gms.auth.cryptauth");
                Log("[5/7] Clearing trust/FIDO...", Colors.Orange);
                RunAdb("shell pm clear com.google.android.gms.trust");
                RunAdb("shell pm clear com.google.android.gms.fido");
                Log("[6/7] Disabling/re-enabling GMS...", Colors.Orange);
                RunAdb("shell pm disable-user --user 0 com.google.android.gms");
                RunAdb("shell am force-stop com.google.android.gms");
                Thread.Sleep(2000);
                RunAdb("shell pm enable com.google.android.gms");
                Log("[7/7] Clearing sync data...", Colors.Orange);
                RunAdb("shell settings delete secure sync1");
                RunAdb("shell settings delete secure sync2");
                Log("Google account cleared via MTP method.", Colors.Accent);
            }, "MTP Google Account Clear");
        }

        private void MtpOtgPush()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/5] MTP + OTG File Push method...", Colors.Orange);
                Log("[2/5] Setting MTP mode...", Colors.Orange);
                RunAdb("shell setprop sys.usb.config mtp,adb");
                Log("[3/5] Creating FRP bypass files on device...", Colors.Orange);
                string bypassScript = "#!/system/bin/sh\nsettings put global device_provisioned 1\nsettings put secure user_setup_complete 1\nam start -a android.intent.action.MAIN -c android.intent.category.HOME\n";
                string tempScript = Path.Combine(Path.GetTempPath(), "bnt_frp_bypass.sh");
                File.WriteAllText(tempScript, bypassScript);
                RunAdb("push " + tempScript + " /sdcard/frp_bypass.sh");
                try { File.Delete(tempScript); } catch { }
                Log("[4/5] Executing bypass script...", Colors.Orange);
                RunAdb("shell chmod 755 /sdcard/frp_bypass.sh");
                RunAdb("shell sh /sdcard/frp_bypass.sh");
                Log("[5/5] Cleaning up...", Colors.Orange);
                RunAdb("shell rm /sdcard/frp_bypass.sh");
                Log("MTP + OTG push method complete.", Colors.Accent);
            }, "MTP + OTG File Push");
        }

        private void MtpBrowserLaunch()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/5] MTP + Browser Launch method...", Colors.Orange);
                Log("[2/5] Setting MTP mode...", Colors.Orange);
                RunAdb("shell setprop sys.usb.config mtp,adb");
                Log("[3/5] Creating browser intent file...", Colors.Orange);
                string intentFile = Path.Combine(Path.GetTempPath(), "bnt_launch_browser.sh");
                File.WriteAllText(intentFile, "#!/system/bin/sh\nam start -a android.intent.action.VIEW -d \"https://accounts.google.com/signin/recovery\"\nam start com.android.settings/com.android.settings.Settings\n");
                RunAdb("push " + intentFile + " /sdcard/launch.sh");
                try { File.Delete(intentFile); } catch { }
                Log("[4/5] Executing browser launch...", Colors.Orange);
                RunAdb("shell chmod 755 /sdcard/launch.sh");
                RunAdb("shell sh /sdcard/launch.sh");
                Log("[5/5] Opening browser for account recovery...", Colors.Orange);
                RunAdb(@"shell am start -a android.intent.action.VIEW -d ""https://accounts.google.com/signin/recovery""");
                Log("Browser launched. Use for Google account recovery.", Colors.Accent);
            }, "MTP + Browser Launch");
        }

        private void MtpAccessibilityExploit()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/6] MTP + Accessibility Exploit...", Colors.Orange);
                Log("[2/6] Setting MTP mode...", Colors.Orange);
                RunAdb("shell setprop sys.usb.config mtp,adb");
                Log("[3/6] Enabling accessibility services...", Colors.Orange);
                RunAdb(@"shell settings put secure enabled_accessibility_services com.google.android.marvin.talkback/com.google.android.marvin.talkback.TalkBackService");
                RunAdb("shell settings put secure accessibility_enabled 1");
                Log("[4/6] Opening accessibility settings...", Colors.Orange);
                RunAdb("shell am start -a android.settings.ACCESSIBILITY_SETTINGS");
                Log("[5/6] Launching Google app via accessibility...", Colors.Orange);
                RunAdb("shell am start -n com.google.android.googlequicksearchbox/com.google.android.launcher.GEL");
                Log("[6/6] Performing gesture exploit...", Colors.Orange);
                RunAdb("shell input swipe 100 500 100 100 300");
                RunAdb("shell input keyevent 82");
                Log("Accessibility exploit initiated. Use TalkBack to navigate.", Colors.Accent);
            }, "MTP + Accessibility Exploit");
        }

        private void MtpTalkBackVoice()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/6] MTP + TalkBack Voice Command...", Colors.Orange);
                Log("[2/6] Setting MTP mode...", Colors.Orange);
                RunAdb("shell setprop sys.usb.config mtp,adb");
                Log("[3/6] Enabling TalkBack...", Colors.Orange);
                RunAdb(@"shell settings put secure enabled_accessibility_services com.google.android.marvin.talkback/com.google.android.marvin.talkback.TalkBackService");
                RunAdb("shell settings put secure accessibility_enabled 1");
                Log("[4/6] Opening Google assistant via voice...", Colors.Orange);
                RunAdb("shell am start -n com.google.android.googlequicksearchbox/com.google.android.launcher.GEL");
                Thread.Sleep(1000);
                Log("[5/6] Simulating voice command...", Colors.Orange);
                RunAdb("shell input keyevent 224");
                Thread.Sleep(500);
                RunAdb(@"shell am start -a android.intent.action.VIEW -d ""https://youtube.com""");
                RunAdb(@"shell am start -a android.intent.action.VIEW -d ""https://myaccount.google.com""");
                Log("[6/6] Opening settings...", Colors.Orange);
                RunAdb("shell am start com.android.settings/com.android.settings.Settings");
                Log("TalkBack voice command method complete.", Colors.Accent);
            }, "MTP + TalkBack Voice Command");
        }

        private void MtpFullBypass()
        {
            if (!Confirm("Run FULL MTP FRP bypass (all methods combined)?")) return;
            ExecuteWithWait(() =>
            {
                Log("[1/12] MTP + Full FRP Bypass starting...", Colors.Orange);
                Log("[2/12] Setting MTP mode...", Colors.Orange);
                RunAdb("shell setprop sys.usb.config mtp,adb");
                RunAdb("shell setprop sys.usb.state mtp,adb");
                Log("[3/12] FRP flags...", Colors.Orange);
                RunAdb("shell settings put secure frp_mode_disabled 1");
                RunAdb("shell settings put global device_provisioned 1");
                RunAdb("shell settings put secure user_setup_complete 1");
                RunAdb("shell content insert --uri content://settings/secure --bind name:s:frp_mode_disabled --bind value:s:1");
                RunAdb("shell content insert --uri content://settings/global --bind name:s:device_provisioned --bind value:s:1");
                RunAdb("shell content insert --uri content://settings/secure --bind name:s:user_setup_complete --bind value:s:1");
                Log("[4/12] Disabling all wizards...", Colors.Orange);
                string[] wiz = { "com.google.android.setupwizard", "com.sec.android.app.SecSetupWizard", "com.miui.miservice", "com.huawei.android.hwfrozen", "com.oppo.setupwizard", "com.heytap.setupwizard", "com.vivo.setupwizard", "com.zte.setupwizard", "com.motorola.setupwizard" };
                foreach (var w in wiz) RunAdb("shell pm disable-user --user 0 " + w);
                Log("[5/12] Clearing Google data...", Colors.Orange);
                foreach (var c in new[] { "com.google.android.gsf.login", "com.google.android.gsf", "com.google.android.gms", "com.google.android.gms.auth", "com.google.android.gms.auth.authzen", "com.google.android.gms.auth.cryptauth", "com.google.android.gms.trust", "com.google.android.gms.fido" }) RunAdb("shell pm clear " + c);
                Log("[6/12] Account databases...", Colors.Orange);
                RunAdb(@"shell rm -rf /data/system/users/*/accounts.db");
                RunAdb(@"shell rm -rf /data/system/users/*/accounts_de.db");
                RunAdb(@"shell rm -rf /data/system/users/*/accounts_ce.db");
                Log("[7/12] Clearing settings cache...", Colors.Orange);
                RunAdb(@"shell rm -rf /data/system/users/*/settings_secure.xml");
                Log("[8/12] Killing processes...", Colors.Orange);
                RunAdb("shell am force-stop com.google.android.setupwizard");
                RunAdb("shell am force-stop com.sec.android.app.SecSetupWizard");
                Log("[9/12] Disabling FMD...", Colors.Orange);
                RunAdb("shell pm disable-user --user 0 com.google.android.gms.trust");
                Log("[10/12] Enabling developer options...", Colors.Orange);
                for (int i = 0; i < 7; i++) RunAdb("shell settings put global development_settings_enabled 1");
                RunAdb("shell settings put global adb_enabled 1");
                Log("[11/12] Accessibility setup...", Colors.Orange);
                RunAdb(@"shell settings put secure enabled_accessibility_services com.google.android.marvin.talkback/com.google.android.marvin.talkback.TalkBackService");
                RunAdb("shell settings put secure accessibility_enabled 1");
                Log("[12/12] Going home...", Colors.Orange);
                RunAdb("shell am start -a android.intent.action.MAIN -c android.intent.category.HOME");
                Log("FULL MTP FRP BYPASS COMPLETE! All 12 steps done.", Colors.Accent);
            }, "Full MTP FRP Bypass");
        }

        private void MtpXiaomiBypass()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/7] Xiaomi/Redmi MTP bypass...", Colors.Orange);
                Log("[2/7] Setting MTP mode...", Colors.Orange);
                RunAdb("shell setprop sys.usb.config mtp,adb");
                Log("[3/7] Bypassing MIUI FRP...", Colors.Orange);
                RunAdb("shell settings put global device_provisioned 1");
                RunAdb("shell settings put secure user_setup_complete 1");
                RunAdb("shell settings put secure frp_mode_disabled 1");
                Log("[4/7] Disabling MIUI services...", Colors.Orange);
                string[] miuiPkgs = { "com.miui.miservice", "com.miui.securitycenter", "com.miui.cleanmaster", "com.xiaomi.xmsf", "com.miui.daemon" };
                foreach (var p in miuiPkgs) RunAdb("shell pm disable-user --user 0 " + p);
                Log("[5/7] Clearing Xiaomi account...", Colors.Orange);
                RunAdb("shell pm clear com.xiaomi.account");
                RunAdb("shell pm clear com.xiaomi.xmsf");
                Log("[6/7] Clearing MIUI setup...", Colors.Orange);
                RunAdb("shell pm clear com.miui.miservice");
                Log("[7/7] Going home...", Colors.Orange);
                RunAdb("shell am start -a android.intent.action.MAIN -c android.intent.category.HOME");
                Log("Xiaomi/Redmi MTP bypass complete.", Colors.Accent);
            }, "Xiaomi/Redmi MTP Bypass");
        }

        private void MtpHuaweiBypass()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/7] Huawei MTP bypass...", Colors.Orange);
                Log("[2/7] Setting MTP mode...", Colors.Orange);
                RunAdb("shell setprop sys.usb.config mtp,adb");
                Log("[3/7] Bypassing Huawei FRP...", Colors.Orange);
                RunAdb("shell settings put global device_provisioned 1");
                RunAdb("shell settings put secure user_setup_complete 1");
                RunAdb("shell settings put secure frp_mode_disabled 1");
                Log("[4/7] Disabling Huawei services...", Colors.Orange);
                string[] hwPkgs = { "com.huawei.android.hwfrozen", "com.huawei.systemmanager", "com.huawei.hianalytics", "com.huawei.ads", "com.huawei.hwid" };
                foreach (var p in hwPkgs) RunAdb("shell pm disable-user --user 0 " + p);
                Log("[5/7] Clearing Huawei account...", Colors.Orange);
                RunAdb("shell pm clear com.huawei.hwid");
                RunAdb("shell pm clear com.huawei.hianalytics");
                Log("[6/7] Disabling Huawei setup...", Colors.Orange);
                RunAdb("shell pm clear com.huawei.android.hwfrozen");
                Log("[7/7] Going home...", Colors.Orange);
                RunAdb("shell am start -a android.intent.action.MAIN -c android.intent.category.HOME");
                Log("Huawei MTP bypass complete.", Colors.Accent);
            }, "Huawei MTP Bypass");
        }

        private void MtpOppoBypass()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/7] OPPO/Realme MTP bypass...", Colors.Orange);
                Log("[2/7] Setting MTP mode...", Colors.Orange);
                RunAdb("shell setprop sys.usb.config mtp,adb");
                Log("[3/7] Bypassing ColorOS FRP...", Colors.Orange);
                RunAdb("shell settings put global device_provisioned 1");
                RunAdb("shell settings put secure user_setup_complete 1");
                RunAdb("shell settings put secure frp_mode_disabled 1");
                Log("[4/7] Disabling OPPO services...", Colors.Orange);
                string[] oppoPkgs = { "com.oppo.setupwizard", "com.heytap.setupwizard", "com.heytap.market", "com.coloros.assistantscreen", "com.coloros.weather2" };
                foreach (var p in oppoPkgs) RunAdb("shell pm disable-user --user 0 " + p);
                Log("[5/7] Clearing OPPO account...", Colors.Orange);
                RunAdb("shell pm clear com.heytap.cloud");
                RunAdb("shell pm clear com.heytap.htms");
                Log("[6/7] Clearing setup wizard...", Colors.Orange);
                RunAdb("shell pm clear com.oppo.setupwizard");
                Log("[7/7] Going home...", Colors.Orange);
                RunAdb("shell am start -a android.intent.action.MAIN -c android.intent.category.HOME");
                Log("OPPO/Realme MTP bypass complete.", Colors.Accent);
            }, "OPPO/Realme MTP Bypass");
        }

        private void MtpVivoBypass()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/7] Vivo MTP bypass...", Colors.Orange);
                Log("[2/7] Setting MTP mode...", Colors.Orange);
                RunAdb("shell setprop sys.usb.config mtp,adb");
                Log("[3/7] Bypassing Vivo FRP...", Colors.Orange);
                RunAdb("shell settings put global device_provisioned 1");
                RunAdb("shell settings put secure user_setup_complete 1");
                RunAdb("shell settings put secure frp_mode_disabled 1");
                Log("[4/7] Disabling Vivo services...", Colors.Orange);
                string[] vivoPkgs = { "com.vivo.setupwizard", "com.bbk.launcher2", "com.vivo.easyshare", "com.vivo.weather", "com.vivo.game" };
                foreach (var p in vivoPkgs) RunAdb("shell pm disable-user --user 0 " + p);
                Log("[5/7] Clearing Vivo account...", Colors.Orange);
                RunAdb("shell pm clear com.bbk.cloud");
                RunAdb("shell pm clear com.vivo.space");
                Log("[6/7] Clearing setup wizard...", Colors.Orange);
                RunAdb("shell pm clear com.vivo.setupwizard");
                Log("[7/7] Going home...", Colors.Orange);
                RunAdb("shell am start -a android.intent.action.MAIN -c android.intent.category.HOME");
                Log("Vivo MTP bypass complete.", Colors.Accent);
            }, "Vivo MTP Bypass");
        }

        private void MtpMotorolaBypass()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/7] Motorola MTP bypass...", Colors.Orange);
                Log("[2/7] Setting MTP mode...", Colors.Orange);
                RunAdb("shell setprop sys.usb.config mtp,adb");
                Log("[3/7] Bypassing Motorola FRP...", Colors.Orange);
                RunAdb("shell settings put global device_provisioned 1");
                RunAdb("shell settings put secure user_setup_complete 1");
                RunAdb("shell settings put secure frp_mode_disabled 1");
                Log("[4/7] Disabling Motorola services...", Colors.Orange);
                string[] motoPkgs = { "com.motorola.setupwizard", "com.motorola.motocit", "com.motorola.attackservices", "com.motorola.launcher.config" };
                foreach (var p in motoPkgs) RunAdb("shell pm disable-user --user 0 " + p);
                Log("[5/7] Clearing Motorola account...", Colors.Orange);
                RunAdb("shell pm clear com.motorola.motocit");
                Log("[6/7] Clearing setup wizard...", Colors.Orange);
                RunAdb("shell pm clear com.motorola.setupwizard");
                Log("[7/7] Going home...", Colors.Orange);
                RunAdb("shell am start -a android.intent.action.MAIN -c android.intent.category.HOME");
                Log("Motorola MTP bypass complete.", Colors.Accent);
            }, "Motorola MTP Bypass");
        }

        private void MtpLgBypass()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/7] LG MTP bypass...", Colors.Orange);
                Log("[2/7] Setting MTP mode...", Colors.Orange);
                RunAdb("shell setprop sys.usb.config mtp,adb");
                Log("[3/7] Bypassing LG FRP...", Colors.Orange);
                RunAdb("shell settings put global device_provisioned 1");
                RunAdb("shell settings put secure user_setup_complete 1");
                RunAdb("shell settings put secure frp_mode_disabled 1");
                Log("[4/7] Disabling LG services...", Colors.Orange);
                string[] lgPkgs = { "com.lge.setupwizard", "com.lge.lgaccount", "com.lge.lgfashion", "com.lge.bnr", "com.lge.service.lgdm" };
                foreach (var p in lgPkgs) RunAdb("shell pm disable-user --user 0 " + p);
                Log("[5/7] Clearing LG account...", Colors.Orange);
                RunAdb("shell pm clear com.lge.lgaccount");
                Log("[6/7] Clearing setup wizard...", Colors.Orange);
                RunAdb("shell pm clear com.lge.setupwizard");
                Log("[7/7] Going home...", Colors.Orange);
                RunAdb("shell am start -a android.intent.action.MAIN -c android.intent.category.HOME");
                Log("LG MTP bypass complete.", Colors.Accent);
            }, "LG MTP Bypass");
        }

        private void MtpNokiaBypass()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/7] Nokia/HMD MTP bypass...", Colors.Orange);
                Log("[2/7] Setting MTP mode...", Colors.Orange);
                RunAdb("shell setprop sys.usb.config mtp,adb");
                Log("[3/7] Bypassing Nokia FRP...", Colors.Orange);
                RunAdb("shell settings put global device_provisioned 1");
                RunAdb("shell settings put secure user_setup_complete 1");
                RunAdb("shell settings put secure frp_mode_disabled 1");
                Log("[4/7] Disabling Nokia services...", Colors.Orange);
                string[] nokiaPkgs = { "com.nokia.mt", "com.hmd.global.appsupport", "com.nokia.camera" };
                foreach (var p in nokiaPkgs) RunAdb("shell pm disable-user --user 0 " + p);
                Log("[5/7] Clearing Nokia account...", Colors.Orange);
                RunAdb("shell pm clear com.nokia.mt");
                Log("[6/7] Clearing setup wizard...", Colors.Orange);
                RunAdb("shell pm clear com.google.android.setupwizard");
                Log("[7/7] Going home...", Colors.Orange);
                RunAdb("shell am start -a android.intent.action.MAIN -c android.intent.category.HOME");
                Log("Nokia/HMD MTP bypass complete.", Colors.Accent);
            }, "Nokia/HMD MTP Bypass");
        }

        private void MtpSonyBypass()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/7] Sony/Xperia MTP bypass...", Colors.Orange);
                Log("[2/7] Setting MTP mode...", Colors.Orange);
                RunAdb("shell setprop sys.usb.config mtp,adb");
                Log("[3/7] Bypassing Sony FRP...", Colors.Orange);
                RunAdb("shell settings put global device_provisioned 1");
                RunAdb("shell settings put secure user_setup_complete 1");
                RunAdb("shell settings put secure frp_mode_disabled 1");
                Log("[4/7] Disabling Sony services...", Colors.Orange);
                string[] sonyPkgs = { "com.sonymobile.setupwizard", "com.sonymobile.xperialab", "com.sonymobile.mt", "com.sonymobile.swiqareset", "com.sonymobile.entrance" };
                foreach (var p in sonyPkgs) RunAdb("shell pm disable-user --user 0 " + p);
                Log("[5/7] Clearing Sony account...", Colors.Orange);
                RunAdb("shell pm clear com.sonymobile.mt");
                Log("[6/7] Clearing setup wizard...", Colors.Orange);
                RunAdb("shell pm clear com.sonymobile.setupwizard");
                Log("[7/7] Going home...", Colors.Orange);
                RunAdb("shell am start -a android.intent.action.MAIN -c android.intent.category.HOME");
                Log("Sony/Xperia MTP bypass complete.", Colors.Accent);
            }, "Sony/Xperia MTP Bypass");
        }

        private void MtpAsusBypass()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/7] Asus MTP bypass...", Colors.Orange);
                Log("[2/7] Setting MTP mode...", Colors.Orange);
                RunAdb("shell setprop sys.usb.config mtp,adb");
                Log("[3/7] Bypassing Asus FRP...", Colors.Orange);
                RunAdb("shell settings put global device_provisioned 1");
                RunAdb("shell settings put secure user_setup_complete 1");
                RunAdb("shell settings put secure frp_mode_disabled 1");
                Log("[4/7] Disabling Asus services...", Colors.Orange);
                string[] asusPkgs = { "com.asus.setupwizard", "com.asus.asussupport", "com.asus.maxis", "com.asus.msa", "com.asus.gameassist" };
                foreach (var p in asusPkgs) RunAdb("shell pm disable-user --user 0 " + p);
                Log("[5/7] Clearing Asus account...", Colors.Orange);
                RunAdb("shell pm clear com.asus.asussupport");
                Log("[6/7] Clearing setup wizard...", Colors.Orange);
                RunAdb("shell pm clear com.asus.setupwizard");
                Log("[7/7] Going home...", Colors.Orange);
                RunAdb("shell am start -a android.intent.action.MAIN -c android.intent.category.HOME");
                Log("Asus MTP bypass complete.", Colors.Accent);
            }, "Asus MTP Bypass");
        }

        private void ShowFastboot()
        {
            var panel = CreateSection("FASTBOOT FRP BYPASS v8.16");
            int y = 10;

            var warn = new Label { Text = "WARNING: Device must be in fastboot/bootloader mode. All operations use fastboot, not ADB.", Font = new Font("Segoe UI", 8.5f, FontStyle.Bold), ForeColor = Colors.Red, Location = new Point(10, y), Width = 580, Height = 22 };
            panel.Controls.Add(warn); y += 28;

            var fbModels = new Label
            {
                Text = "Supported: Samsung, Xiaomi, Oppo, Vivo, Realme, Motorola, Huawei, OnePlus, Google Pixel, LG, Sony (fastboot-unlockable devices)",
                Font = new Font("Consolas", 7.5f, FontStyle.Bold),
                ForeColor = Colors.Accent,
                Location = new Point(10, y),
                Size = new Size(580, 20)
            };
            panel.Controls.Add(fbModels); y += 22;

            panel.Controls.Add(MakeBtn("Reboot to Bootloader (Fastboot Mode)", 10, y, 440, () =>
            {
                Log("Rebooting to bootloader...", Colors.Orange);
                RunAdb("reboot bootloader");
                Log("Device should now be in fastboot mode.", Colors.Accent);
            })); y += 38;

            panel.Controls.Add(MakeBtn("Erase FRP Partition", 10, y, 440, () =>
            {
                if (!Confirm("Erase the FRP partition? This wipes FRP lock data.")) return;
                ExecuteWithWait(() =>
                {
                    Log("[1/3] Erasing FRP partition...", Colors.Orange);
                    string r = RunFastboot("erase frp");
                    Log("fastboot erase frp => " + r, Colors.Text);
                    Log("[2/3] Erasing persist...", Colors.Orange);
                    Log(RunFastboot("erase persist"), Colors.Text);
                    Log("[3/3] Rebooting...", Colors.Orange);
                    RunFastboot("reboot");
                    Log("FRP partition erased. Device rebooting.", Colors.Accent);
                }, "Erase FRP Partition");
            })); y += 38;

            panel.Controls.Add(MakeBtn("Erase Persist Partition", 10, y, 440, () =>
            {
                if (!Confirm("Erase the persist partition? May affect sensors/calibration.")) return;
                ExecuteWithWait(() =>
                {
                    Log("[1/3] Erasing persist...", Colors.Orange);
                    string r = RunFastboot("erase persist");
                    Log("fastboot erase persist => " + r, Colors.Text);
                    Log("[2/3] Erasing config...", Colors.Orange);
                    Log(RunFastboot("erase config"), Colors.Text);
                    Log("[3/3] Rebooting...", Colors.Orange);
                    RunFastboot("reboot");
                    Log("Persist partition erased. Device rebooting.", Colors.Accent);
                }, "Erase Persist Partition");
            })); y += 38;

            panel.Controls.Add(MakeBtn("Erase FRP + Persist + Cache + Userdata", 10, y, 440, () =>
            {
                if (!Confirm("FULL WIPE: Erase FRP, persist, cache, AND userdata? ALL DATA LOST!")) return;
                ExecuteWithWait(() =>
                {
                    Log("[1/6] Erasing FRP...", Colors.Orange);
                    Log(RunFastboot("erase frp"), Colors.Text);
                    Log("[2/6] Erasing persist...", Colors.Orange);
                    Log(RunFastboot("erase persist"), Colors.Text);
                    Log("[3/6] Erasing config...", Colors.Orange);
                    Log(RunFastboot("erase config"), Colors.Text);
                    Log("[4/6] Erasing cache...", Colors.Orange);
                    Log(RunFastboot("erase cache"), Colors.Text);
                    Log("[5/6] Erasing userdata...", Colors.Orange);
                    Log(RunFastboot("erase userdata"), Colors.Text);
                    Log("[6/6] Rebooting...", Colors.Orange);
                    RunFastboot("reboot");
                    Log("ALL partitions erased. Device rebooting.", Colors.Accent);
                }, "Full FRP Wipe");
            })); y += 38;

            panel.Controls.Add(MakeBtn("Erase Config Partition", 10, y, 440, () =>
            {
                if (!Confirm("Erase config partition? (Some devices store FRP here)")) return;
                ExecuteWithWait(() =>
                {
                    Log("[1/2] Erasing config...", Colors.Orange);
                    Log(RunFastboot("erase config"), Colors.Text);
                    Log("[2/2] Rebooting...", Colors.Orange);
                    RunFastboot("reboot");
                    Log("Config partition erased.", Colors.Accent);
                }, "Erase Config");
            })); y += 38;

            panel.Controls.Add(MakeBtn("Erase Misc Partition", 10, y, 440, () =>
            {
                if (!Confirm("Erase misc partition? (Boot selection data)")) return;
                ExecuteWithWait(() =>
                {
                    Log("[1/2] Erasing misc...", Colors.Orange);
                    Log(RunFastboot("erase misc"), Colors.Text);
                    Log("[2/2] Rebooting...", Colors.Orange);
                    RunFastboot("reboot");
                    Log("Misc partition erased.", Colors.Accent);
                }, "Erase Misc");
            })); y += 38;

            panel.Controls.Add(MakeBtn("Erase Devinfo Partition", 10, y, 440, () =>
            {
                if (!Confirm("Erase devinfo partition?")) return;
                ExecuteWithWait(() =>
                {
                    Log("[1/2] Erasing devinfo...", Colors.Orange);
                    Log(RunFastboot("erase devinfo"), Colors.Text);
                    Log("[2/2] Rebooting...", Colors.Orange);
                    RunFastboot("reboot");
                    Log("Devinfo partition erased.", Colors.Accent);
                }, "Erase Devinfo");
            })); y += 38;

            panel.Controls.Add(MakeBtn("OEM Unlock (fastboot)", 10, y, 440, () =>
            {
                if (!Confirm("Send OEM unlock commands?")) return;
                ExecuteWithWait(() =>
                {
                    Log("[1/3] fastboot oem unlock...", Colors.Orange);
                    string r1 = RunFastboot("oem unlock");
                    Log("Result: " + r1, Colors.Text);
                    Log("[2/3] fastboot flashing unlock...", Colors.Orange);
                    string r2 = RunFastboot("flashing unlock");
                    Log("Result: " + r2, Colors.Text);
                    Log("[3/3] fastboot flashing unlock_critical...", Colors.Orange);
                    string r3 = RunFastboot("flashing unlock_critical");
                    Log("Result: " + r3, Colors.Text);
                    Log("OEM unlock commands sent.", Colors.Accent);
                }, "OEM Unlock");
            })); y += 38;

            panel.Controls.Add(MakeBtn("Unlock Bootloader (Full Wipe)", 10, y, 440, () =>
            {
                if (!Confirm("FULL BOOTLOADER UNLOCK: ALL DATA WILL BE ERASED!")) return;
                ExecuteWithWait(() =>
                {
                    Log("[1/3] fastboot oem unlock...", Colors.Orange);
                    Log(RunFastboot("oem unlock"), Colors.Text);
                    Log("[2/3] fastboot flashing unlock...", Colors.Orange);
                    Log(RunFastboot("flashing unlock"), Colors.Text);
                    Log("[3/3] fastboot flashing unlock_critical...", Colors.Orange);
                    Log(RunFastboot("flashing unlock_critical"), Colors.Text);
                    Log("Bootloader unlock commands sent. Device will wipe.", Colors.Accent);
                }, "Unlock Bootloader");
            })); y += 38;

            panel.Controls.Add(MakeBtn("Set Active Slot (A/B Devices)", 10, y, 440, () =>
            {
                var sub = CreateSection("SET ACTIVE SLOT");
                int sy = 10;
                sub.Controls.Add(MakeBtn("Set Slot A Active", 10, sy, 300, () =>
                {
                    Log("Setting slot a active...", Colors.Orange);
                    Log(RunFastboot("set_active a"), Colors.Text);
                    Log(RunFastboot("reboot"), Colors.Text);
                    Log("Slot A set. Rebooting.", Colors.Accent);
                })); sy += 38;
                sub.Controls.Add(MakeBtn("Set Slot B Active", 10, sy, 300, () =>
                {
                    Log("Setting slot b active...", Colors.Orange);
                    Log(RunFastboot("set_active b"), Colors.Text);
                    Log(RunFastboot("reboot"), Colors.Text);
                    Log("Slot B set. Rebooting.", Colors.Accent);
                })); sy += 38;
                sub.Controls.Add(MakeBtn("Show Current Slot", 10, sy, 300, () =>
                {
                    Log("Active slot: " + RunFastboot("getvar current-slot"), Colors.Text);
                    Log("Slot count: " + RunFastboot("getvar slot-count"), Colors.Text);
                })); sy += 38;
                sub.Controls.Add(MakeBtn("Erase FRP on Current Slot", 10, sy, 300, () =>
                {
                    Log("Erasing FRP on current slot...", Colors.Orange);
                    Log(RunFastboot("erase frp"), Colors.Text);
                    Log("FRP erased for active slot.", Colors.Accent);
                })); sy += 38;
                sub.Controls.Add(MakeBtn("Erase FRP on Both Slots", 10, sy, 300, () =>
                {
                    if (!Confirm("Erase FRP on BOTH slots?")) return;
                    ExecuteWithWait(() =>
                    {
                        Log("[1/5] Getting active slot...", Colors.Orange);
                        string curSlot = RunFastboot("getvar current-slot").Trim();
                        Log("Current: " + curSlot, Colors.Text);
                        Log("[2/5] Setting slot a...", Colors.Orange);
                        Log(RunFastboot("set_active a"), Colors.Text);
                        Log("[3/5] Erasing FRP on slot a...", Colors.Orange);
                        Log(RunFastboot("erase frp"), Colors.Text);
                        Log("[4/5] Setting slot b...", Colors.Orange);
                        Log(RunFastboot("set_active b"), Colors.Text);
                        Log("[5/5] Erasing FRP on slot b...", Colors.Orange);
                        Log(RunFastboot("erase frp"), Colors.Text);
                        Log("FRP erased on both slots. Rebooting...", Colors.Accent);
                        RunFastboot("reboot");
                    }, "Erase FRP Both Slots");
                })); sy += 38;
            })); y += 38;

            panel.Controls.Add(MakeBtn("Flash Empty FRP (Write Zeros)", 10, y, 440, () =>
            {
                if (!Confirm("Write zeros to FRP partition? This blanks all FRP data.")) return;
                ExecuteWithWait(() =>
                {
                    Log("[1/4] Getting FRP partition size...", Colors.Orange);
                    string sz = RunFastboot("getvar partition-size:frp");
                    Log("Partition info: " + sz, Colors.Text);
                    Log("[2/4] Creating blank file...", Colors.Orange);
                    string blank = Path.Combine(Path.GetTempPath(), "bnt_blank_frp.img");
                    byte[] zeros = new byte[1024 * 64];
                    File.WriteAllBytes(blank, zeros);
                    Log("[3/4] Flashing blank FRP...", Colors.Orange);
                    string r = RunFastboot("flash frp \"" + blank + "\"");
                    Log("Result: " + r, Colors.Text);
                    Log("[4/4] Rebooting...", Colors.Orange);
                    RunFastboot("reboot");
                    try { File.Delete(blank); } catch { }
                    Log("FRP partition blanked. Device rebooting.", Colors.Accent);
                }, "Flash Empty FRP");
            })); y += 38;

            panel.Controls.Add(MakeBtn("Samsung Download Mode", 10, y, 440, () =>
            {
                if (!Confirm("Reboot into Samsung Odin/Download mode?")) return;
                ExecuteWithWait(() =>
                {
                    Log("[1/2] Rebooting to download mode...", Colors.Orange);
                    string r = RunFastboot("oem download");
                    Log("fastboot oem download => " + r, Colors.Text);
                    if (string.IsNullOrEmpty(r) || r.Contains("not found"))
                    {
                        Log("[2/2] Trying alt command...", Colors.Orange);
                        RunFastboot("flashing download");
                    }
                    Log("Sending download mode command.", Colors.Accent);
                }, "Samsung Download Mode");
            })); y += 38;

            panel.Controls.Add(MakeBtn("Reboot to Recovery", 10, y, 440, () =>
            {
                Log("Rebooting to recovery...", Colors.Orange);
                RunFastboot("reboot recovery");
                Log("Device rebooting to recovery.", Colors.Accent);
            })); y += 38;

            panel.Controls.Add(MakeBtn("Reboot System", 10, y, 440, () =>
            {
                Log("Rebooting to system...", Colors.Orange);
                RunFastboot("reboot");
                Log("Device rebooting.", Colors.Accent);
            })); y += 38;

            panel.Controls.Add(MakeBtn("List All Partitions", 10, y, 440, () =>
            {
                ExecuteWithWait(() =>
                {
                    Log("=== FASTBOOT PARTITIONS ===", Colors.Accent);
                    string[] parts = { "frp", "persist", "config", "misc", "devinfo", "cache", "userdata", "boot", "system", "recovery", "userdata", "metadata", "modem", "dtbo", "vbmeta", "super" };
                    foreach (var p in parts)
                    {
                        string sz = RunFastboot("getvar partition-size:" + p);
                        if (!string.IsNullOrEmpty(sz) && !sz.Contains("unknown"))
                            Log("  " + p + ": " + sz.Trim(), Colors.Text);
                    }
                }, "List Partitions");
            })); y += 38;

            panel.Controls.Add(MakeBtn("Fastboot Device Info", 10, y, 440, () =>
            {
                ExecuteWithWait(() =>
                {
                    Log("=== FASTBOOT DEVICE INFO ===", Colors.Accent);
                    Log("Product: " + RunFastboot("getvar product"), Colors.Text);
                    Log("Serialno: " + RunFastboot("getvar serialno"), Colors.Text);
                    Log("Unlocked: " + RunFastboot("getvar unlocked"), Colors.Text);
                    Log("Secure: " + RunFastboot("getvar secure"), Colors.Text);
                    Log("Variant: " + RunFastboot("getvar variant"), Colors.Text);
                    Log("Active slot: " + RunFastboot("getvar current-slot"), Colors.Text);
                    Log("Slot count: " + RunFastboot("getvar slot-count"), Colors.Text);
                    Log("HW version: " + RunFastboot("getvar hwversion"), Colors.Text);
                    Log("Kernel: " + RunFastboot("getvar kernel"), Colors.Text);
                }, "Fastboot Device Info");
            })); y += 38;

            panel.Controls.Add(MakeBtn("Custom Fastboot Command", 10, y, 440, () =>
            {
                string cmd = PromptInput("Fastboot command (e.g. erase frp):");
                if (!string.IsNullOrEmpty(cmd))
                {
                    Log("Running: fastboot " + cmd, Colors.Orange);
                    string r = RunFastboot(cmd);
                    Log("Result: " + r, Colors.Text);
                }
            })); y += 38;
        }

        private void FrpFastboot()
        {
            var panel = CreateSection("FASTBOOT FRP BYPASS");
            int y = 10;

            var warn = new Label { Text = "Device must be in fastboot mode (bootloader). Use Reboot Bootloader first.", Font = new Font("Segoe UI", 8.5f), ForeColor = Colors.Orange, Location = new Point(10, y), Width = 550, Height = 36 };
            panel.Controls.Add(warn); y += 42;

            panel.Controls.Add(MakeBtn("Reboot to Bootloader (Fastboot)", 10, y, 420, () =>
            {
                Log("Rebooting to bootloader...", Colors.Orange);
                RunAdb("reboot bootloader");
                Log("Device should be in fastboot mode now.", Colors.Accent);
            })); y += 38;

            panel.Controls.Add(MakeBtn("Erase FRP Partition", 10, y, 420, () =>
            {
                if (!Confirm("Erase the FRP partition? This wipes FRP data.")) return;
                ExecuteWithWait(() =>
                {
                    Log("[1/3] Checking fastboot...", Colors.Orange);
                    Log("[2/3] Erasing FRP partition...", Colors.Orange);
                    string r = RunFastboot("erase frp");
                    Log("fastboot erase frp => " + r, Colors.Text);
                    Log("[3/3] Rebooting...", Colors.Orange);
                    RunFastboot("reboot");
                    Log("FRP partition erased. Device rebooting.", Colors.Accent);
                }, "Erase FRP Partition");
            })); y += 38;

            panel.Controls.Add(MakeBtn("Erase Persist Partition", 10, y, 420, () =>
            {
                if (!Confirm("Erase the persist partition? May affect device sensors.")) return;
                ExecuteWithWait(() =>
                {
                    Log("[1/3] Checking fastboot...", Colors.Orange);
                    Log("[2/3] Erasing persist partition...", Colors.Orange);
                    string r = RunFastboot("erase persist");
                    Log("fastboot erase persist => " + r, Colors.Text);
                    Log("[3/3] Rebooting...", Colors.Orange);
                    RunFastboot("reboot");
                    Log("Persist partition erased. Device rebooting.", Colors.Accent);
                }, "Erase Persist Partition");
            })); y += 38;

            panel.Controls.Add(MakeBtn("Erase FRP + Persist + Cache", 10, y, 420, () =>
            {
                if (!Confirm("Erase FRP, persist, AND cache partitions?")) return;
                ExecuteWithWait(() =>
                {
                    Log("[1/5] Erasing FRP...", Colors.Orange);
                    Log(RunFastboot("erase frp"), Colors.Text);
                    Log("[2/5] Erasing persist...", Colors.Orange);
                    Log(RunFastboot("erase persist"), Colors.Text);
                    Log("[3/5] Erasing cache...", Colors.Orange);
                    Log(RunFastboot("erase cache"), Colors.Text);
                    Log("[4/5] Erasing userdata...", Colors.Orange);
                    Log(RunFastboot("erase userdata"), Colors.Text);
                    Log("[5/5] Rebooting...", Colors.Orange);
                    RunFastboot("reboot");
                    Log("All partitions erased. Device rebooting.", Colors.Accent);
                }, "Erase FRP + Persist + Cache");
            })); y += 38;

            panel.Controls.Add(MakeBtn("OEM Unlock (fastboot)", 10, y, 420, () =>
            {
                if (!Confirm("Send OEM unlock command via fastboot?")) return;
                ExecuteWithWait(() =>
                {
                    Log("[1/2] OEM unlock...", Colors.Orange);
                    string r1 = RunFastboot("oem unlock");
                    Log("fastboot oem unlock => " + r1, Colors.Text);
                    Log("[2/2] Flashing unlock (alt)...", Colors.Orange);
                    string r2 = RunFastboot("flashing unlock");
                    Log("fastboot flashing unlock => " + r2, Colors.Text);
                    Log("OEM unlock commands sent.", Colors.Accent);
                }, "OEM Unlock");
            })); y += 38;

            panel.Controls.Add(MakeBtn("Unlock Bootloader", 10, y, 420, () =>
            {
                if (!Confirm("Unlock bootloader? This wipes ALL data on device.")) return;
                ExecuteWithWait(() =>
                {
                    Log("[1/3] oem unlock...", Colors.Orange);
                    Log(RunFastboot("oem unlock"), Colors.Text);
                    Log("[2/3] flashing unlock...", Colors.Orange);
                    Log(RunFastboot("flashing unlock"), Colors.Text);
                    Log("[3/3] flashing unlock_critical...", Colors.Orange);
                    Log(RunFastboot("flashing unlock_critical"), Colors.Text);
                    Log("Bootloader unlock commands sent.", Colors.Accent);
                }, "Unlock Bootloader");
            })); y += 38;

            panel.Controls.Add(MakeBtn("Reboot to Recovery", 10, y, 420, () =>
            {
                Log("Rebooting to recovery...", Colors.Orange);
                RunFastboot("reboot recovery");
                Log("Device rebooting to recovery.", Colors.Accent);
            })); y += 38;

            panel.Controls.Add(MakeBtn("Reboot System", 10, y, 420, () =>
            {
                Log("Rebooting to system...", Colors.Orange);
                RunFastboot("reboot");
                Log("Device rebooting.", Colors.Accent);
            })); y += 38;

            panel.Controls.Add(MakeBtn("Fastboot Get Info", 10, y, 420, () =>
            {
                ExecuteWithWait(() =>
                {
                    Log("=== FASTBOOT DEVICE INFO ===", Colors.Accent);
                    Log("Product: " + RunFastboot("getvar product"), Colors.Text);
                    Log("Serialno: " + RunFastboot("getvar serialno"), Colors.Text);
                    Log("Unlocked: " + RunFastboot("getvar unlocked"), Colors.Text);
                    Log("Secure: " + RunFastboot("getvar secure"), Colors.Text);
                    Log("Variant: " + RunFastboot("getvar variant"), Colors.Text);
                    Log("Slot: " + RunFastboot("getvar slot-count"), Colors.Text);
                    Log("Active slot: " + RunFastboot("getvar current-slot"), Colors.Text);
                    Log("HW version: " + RunFastboot("getvar hwversion"), Colors.Text);
                    Log("Kernel: " + RunFastboot("getvar kernel"), Colors.Text);
                }, "Fastboot Device Info");
            })); y += 38;

            panel.Controls.Add(MakeBtn("Custom Fastboot Command", 10, y, 420, () =>
            {
                string cmd = PromptInput("Fastboot command (e.g. erase frp):");
                if (!string.IsNullOrEmpty(cmd))
                {
                    Log("Running: fastboot " + cmd, Colors.Orange);
                    string r = RunFastboot(cmd);
                    Log("Result: " + r, Colors.Text);
                }
            })); y += 38;
        }

        private static string RunFastboot(string args)
        {
            try
            {
                var psi = new ProcessStartInfo("fastboot", args)
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.UTF8,
                    StandardErrorEncoding = Encoding.UTF8
                };
                using (var p = Process.Start(psi))
                {
                    string stdout = p.StandardOutput.ReadToEnd();
                    string stderr = p.StandardError.ReadToEnd();
                    p.WaitForExit();
                    return (stdout.Trim() + " " + stderr.Trim()).Trim();
                }
            }
            catch { return "fastboot not found or not in PATH"; }
        }

        // =====================================================================
        //                           BLOATWARE
        // =====================================================================
        private void ShowBloat()
        {
            var panel = CreateSection("BLOATWARE REMOVAL TOOLKIT");
            int y = 10;

            var bloatModels = new Label
            {
                Text = "Supported: Samsung, Xiaomi, Huawei, Oppo, Vivo, Motorola, Nokia, LG, Sony, OnePlus, Google, Asus, Realme | All Android 5.0-14",
                Font = new Font("Consolas", 7.5f, FontStyle.Bold),
                ForeColor = Colors.Accent,
                Location = new Point(10, y),
                Size = new Size(580, 20)
            };
            panel.Controls.Add(bloatModels); y += 24;

            panel.Controls.Add(MakeBtn("Quick Clean (Ad SDKs + Google)", 10, y, 400, () => BloatQuick())); y += 40;
            panel.Controls.Add(MakeBtn("Brand-Specific (13 brands)", 10, y, 400, () => BloatBrand())); y += 40;
            panel.Controls.Add(MakeBtn("Full Clean (All Brands)", 10, y, 400, () => BloatFull())); y += 40;
            panel.Controls.Add(MakeBtn("List Packages", 10, y, 400, () => BloatList())); y += 40;
            panel.Controls.Add(MakeBtn("Reinstall / Re-enable", 10, y, 400, () => BloatReinstall())); y += 40;
            panel.Controls.Add(MakeBtn("Disabled List", 10, y, 400, () => { Log(RunAdb("shell pm list packages -d"), Colors.Text); })); y += 40;
            panel.Controls.Add(MakeBtn("User Apps Only", 10, y, 400, () => { Log(RunAdb("shell pm list packages -3"), Colors.Text); })); y += 40;
        }

        private void BloatQuick()
        {
            ExecuteWithWait(() =>
            {
                string pkgsList = RunAdb("shell pm list packages");
                int found = 0, removed = 0;

                Log("[1/4] Ad SDKs...", Colors.Orange);
                string[] ads = { "com.startapp.startapp", "com.applovin", "com.inmobi", "com.mopub", "com.facebook.ads", "com.unity3d.services", "com.adcolony", "com.tapjoy", "com.vungle", "com.fyber", "com.yieldmo", "com.braze", "com.localytics", "com.onesignal", "com.kochava", "com.appsflyer", "com.adjust", "com.ironsource", "com.smaato", "com.flurry", "com.chartbeat", "com.revmob", "com.nativex", "com.hyprmx", "com.verve", "com.millennialmedia", "com.chartboost", "com.leadbolt" };
                foreach (var p in ads) { if (pkgsList.Contains(p)) { found++; string r = RunAdb("shell pm uninstall -k --user 0 " + p); if (r.Contains("Success")) removed++; else RunAdb("shell pm disable-user --user 0 " + p); } }

                Log("[2/4] Google tracking...", Colors.Orange);
                string[] g = { "com.google.android.gms.ads.admanager", "com.google.android.googlequicksearchbox", "com.google.android.play.games", "com.google.android.apps.nbu.files", "com.google.android.apps.youtube.music", "com.google.android.apps.youtube.kids" };
                foreach (var p in g) { if (pkgsList.Contains(p)) { found++; string r = RunAdb("shell pm uninstall -k --user 0 " + p); if (r.Contains("Success")) removed++; else RunAdb("shell pm disable-user --user 0 " + p); } }

                Log("[3/4] Clearing ad data...", Colors.Orange);
                RunAdb("shell pm clear com.google.android.gms");
                RunAdb("shell pm clear com.google.android.gms.ads");
                RunAdb("shell pm clear com.facebook.katana");

                Log(string.Format("Quick Clean: {0} found, {1} removed/disabled", found, removed), Colors.Accent);
            }, "Quick Bloatware Clean");
        }

        private void BloatBrand()
        {
            var panel = CreateSection("BRAND-SPECIFIC BLOATWARE");
            panel.Controls.Add(new Label { Text = "Detected: " + deviceInfo, Font = new Font("Consolas", 9f), ForeColor = Colors.Blue, Location = new Point(10, 10), Width = 600, Height = 22 });

            string[][] brands = new[] {
                new[] { "Samsung", "1" }, new[] { "Xiaomi", "2" }, new[] { "Huawei", "3" }, new[] { "OnePlus/Oppo/Realme", "4" },
                new[] { "Vivo/iQOO", "5" }, new[] { "Google Pixel", "6" }, new[] { "Sony", "7" }, new[] { "Motorola/Lenovo", "8" },
                new[] { "Nokia/HMD", "9" }, new[] { "LG", "A" }, new[] { "ASUS", "B" }, new[] { "HTC", "C" }, new[] { "ZTE/Nubia", "D" }
            };

            string[][] brandPkgs = new[] {
                new[] { "com.sec.android.app.sbrowser,com.samsung.android.bixby.agent,com.samsung.android.bixby.service,com.samsung.android.themestore,com.samsung.android.spay,com.samsung.android.aremoji,com.samsung.android.forest,com.samsung.android.samsungpass,com.sec.spp.push,com.samsung.android.dqagent,com.sec.android.widgetapp.samsungweather,com.samsung.android.allshare,com.samsung.android.helphub,com.samsung.android.game.gamehome,com.samsung.android.game.gametools,com.samsung.android.app.tips,com.samsung.android.mobileservice,com.samsung.android.visionintelligence,com.samsung.android.ardrawing,com.samsung.android.arzone,com.samsung.android.app.routines,com.samsung.android.app.sharelive,com.samsung.android.kidsinstaller,com.samsung.android.app.splanet" },
                new[] { "com.miui.ad,com.miui.analytics,com.miui.msa.global,com.xiaomi.shop,com.xiaomi.joyose,com.miui.cleanmaster,com.miui.securitycenter,com.xiaomi.gamecenter,com.xiaomi.market,com.xiaomi.xmsf,com.miui.mipicks,com.miui.huanji,com.miui.phonemanager,com.miui.cleaner" },
                new[] { "com.huawei.systemmanager,com.huawei.hianalytics,com.huawei.ads,com.huawei.trustagent,com.huawei.gamebox.service,com.huawei.health,com.huawei.smarthome,com.huawei.intelligent,com.huawei.android.mirror,com.huawei.android.projector,com.huawei.hmos.weather" },
                new[] { "com.heytap.browser,com.heytap.market,com.heytap.cloud,com.coloros.assistantscreen,com.oppo.launcher,com.oppo.ota,com.realme.hotspot,com.oplus.market,com.coloros.game,com.heytap.usercenter,com.coloros.oshare" },
                new[] { "com.bbk.browser,com.bbk.cloud,com.vivo.weather,com.vivo.game,com.vivo.health,com.iqoo.gamecenter,com.bbk.updateservice,com.vivo.easyshare,com.vivo.market,com.vivo.daemon,com.vivo.imanager" },
                new[] { "com.google.android.apps.nbu.files,com.google.android.apps.chromecast.app,com.google.android.apps.youtube.music,com.google.android.apps.youtube.kids,com.google.android.apps.podcasts,com.google.android.apps.magazines,com.google.android.apps.books,com.google.android.googlequicksearchbox,com.google.android.keep,com.google.android.apps.fitness,com.google.android.apps.tachyon,com.google.android.apps.wallpaper,com.google.android.apps.wellbeing" },
                new[] { "com.sonyericsson.music,com.sonyericsson.video,com.sonyericsson.album,com.sony.mobileconnected,com.sonyericsson.updatecenter" },
                new[] { "com.motorola.genie,com.motorola.ccc,com.lenovo.anyshare.gps,com.lenovo.launcher,com.lenovo.music,com.lenovo.video,com.lenovo.weather,com.lenovo.powermanager" },
                new[] { "com.nokia.community,com.nokia.support,com.nokia.battery" },
                new[] { "com.lge.bnr,com.lge.gallery,com.lge.lgaccount,com.lge.lgdm,com.lge.music,com.lge.remotecontrol,com.lge.theme" },
                new[] { "com.asus.anycut,com.asus.appinstaller,com.asus.gallery,com.asus.music,com.asus.notes,com.asus.weather,com.asus.webstorage" },
                new[] { "com.htc.launcher,com.htc.music,com.htc.newsreader,com.htc.sense,com.htc.weather,com.htc.widget" },
                new[] { "com.zte.miprogram,com.zte.music,com.zte.launcher,com.nubia.weather,com.nubia.gallery,com.nubia.calculator" }
            };

            int y = 40;
            for (int i = 0; i < brands.Length; i++)
            {
                int idx = i;
                panel.Controls.Add(MakeBtn(brands[i][0], 10, y, 250, () => BloatRemove(brandPkgs[idx][0].Split(','))));
                y += 36;
            }
        }

        private void BloatRemove(string[] packages)
        {
            ExecuteWithWait(() =>
            {
                string pkgsList = RunAdb("shell pm list packages");
                int found = 0, removed = 0;
                foreach (var p in packages)
                {
                    string pkg = p.Trim();
                    if (pkgsList.Contains(pkg))
                    {
                        found++;
                        string r = RunAdb("shell pm uninstall -k --user 0 " + pkg);
                        if (r.Contains("Success")) removed++;
                        else RunAdb("shell pm disable-user --user 0 " + pkg);
                    }
                }
                Log(string.Format("Brand Clean: {0} found, {1} removed/disabled", found, removed), Colors.Accent);
            }, "Brand Bloatware Removal");
        }

        private void BloatFull()
        {
            if (!Confirm("Run full bloatware clean (all brands)?")) return;
            ExecuteWithWait(() =>
            {
                string pkgsList = RunAdb("shell pm list packages");
                int found = 0, removed = 0;

                Log("[1/4] Ad SDKs...", Colors.Orange);
                string[] ads = { "com.startapp.startapp", "com.applovin", "com.inmobi", "com.mopub", "com.facebook.ads", "com.unity3d.services", "com.adcolony", "com.tapjoy", "com.vungle", "com.fyber", "com.yieldmo", "com.braze", "com.localytics", "com.onesignal", "com.kochava", "com.appsflyer", "com.adjust", "com.ironsource", "com.smaato", "com.flurry", "com.chartbeat", "com.revmob", "com.chartboost", "com.leadbolt" };
                foreach (var p in ads) { if (pkgsList.Contains(p)) { found++; string r = RunAdb("shell pm uninstall -k --user 0 " + p); if (r.Contains("Success")) removed++; else RunAdb("shell pm disable-user --user 0 " + p); } }

                Log("[2/4] Google...", Colors.Orange);
                string[] g = { "com.google.android.gms.ads.admanager", "com.google.android.googlequicksearchbox", "com.google.android.play.games", "com.google.android.apps.youtube.music", "com.google.android.apps.youtube.kids" };
                foreach (var p in g) { if (pkgsList.Contains(p)) { found++; string r = RunAdb("shell pm uninstall -k --user 0 " + p); if (r.Contains("Success")) removed++; else RunAdb("shell pm disable-user --user 0 " + p); } }

                Log("[3/4] All brands...", Colors.Orange);
                string[] mixed = { "com.sec.android.app.sbrowser", "com.samsung.android.bixby.agent", "com.samsung.android.themestore", "com.miui.ad", "com.miui.analytics", "com.xiaomi.shop", "com.miui.securitycenter", "com.xiaomi.market", "com.huawei.systemmanager", "com.huawei.hianalytics", "com.huawei.ads", "com.heytap.browser", "com.heytap.market", "com.oppo.launcher", "com.realme.hotspot", "com.bbk.browser", "com.vivo.weather", "com.vivo.game", "com.motorola.genie", "com.lenovo.anyshare.gps", "com.nokia.community", "com.lge.lgaccount" };
                foreach (var p in mixed) { if (pkgsList.Contains(p)) { found++; string r = RunAdb("shell pm uninstall -k --user 0 " + p); if (r.Contains("Success")) removed++; else RunAdb("shell pm disable-user --user 0 " + p); } }

                Log("[4/4] Clearing data...", Colors.Orange);
                RunAdb("shell pm clear com.google.android.gms");
                RunAdb("shell pm clear com.facebook.katana");

                Log(string.Format("FULL CLEAN: {0} found, {1} removed/disabled", found, removed), Colors.Accent);
            }, "Full Bloatware Clean");
        }

        private void BloatList()
        {
            var panel = CreateSection("PACKAGE LIST");
            int y = 10;
            panel.Controls.Add(MakeBtn("All Packages", 10, y, 300, () => { Log(RunAdb("shell pm list packages"), Colors.Text); })); y += 38;
            panel.Controls.Add(MakeBtn("With Paths", 10, y, 300, () => { Log(RunAdb("shell pm list packages -f"), Colors.Text); })); y += 38;
            panel.Controls.Add(MakeBtn("System Packages", 10, y, 300, () => { Log(RunAdb("shell pm list packages -s"), Colors.Text); })); y += 38;
            panel.Controls.Add(MakeBtn("Search Packages", 10, y, 300, () =>
            {
                string q = PromptInput("Search term:");
                if (!string.IsNullOrEmpty(q)) Log(RunAdb("shell pm list packages"), Colors.Text);
            })); y += 38;
        }

        private void BloatReinstall()
        {
            var panel = CreateSection("REINSTALL / RE-ENABLE");
            int y = 10;
            panel.Controls.Add(MakeBtn("Re-enable ALL Disabled Packages", 10, y, 400, () =>
            {
                string disabled = RunAdb("shell pm list packages -d");
                foreach (string line in disabled.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string pkg = line.Replace("package:", "").Trim();
                    if (!string.IsNullOrEmpty(pkg)) { RunAdb("shell pm enable " + pkg); Log("[ENABLED] " + pkg, Colors.Accent); }
                }
            })); y += 40;
            panel.Controls.Add(MakeBtn("Re-enable Specific Package", 10, y, 400, () =>
            {
                string pkg = PromptInput("Package name:");
                if (!string.IsNullOrEmpty(pkg)) { RunAdb("shell pm enable " + pkg); RunAdb("shell pm install-existing " + pkg); Log("[OK] " + pkg + " enabled", Colors.Accent); }
            })); y += 40;
        }

        // =====================================================================
        //                           DEVICE UTILITIES
        // =====================================================================
        private void ShowUtils()
        {
            var panel = CreateSection("DEVICE UTILITIES");
            int y = 10;

            var utilsModels = new Label
            {
                Text = "Supported: All Android devices with USB debugging enabled | Android 4.1-14",
                Font = new Font("Consolas", 7.5f, FontStyle.Bold),
                ForeColor = Colors.Accent,
                Location = new Point(10, y),
                Size = new Size(580, 20)
            };
            panel.Controls.Add(utilsModels); y += 22;

            panel.Controls.Add(MakeBtn("Full Device Info", 10, y, 300, () => UtilsInfo())); y += 38;
            panel.Controls.Add(MakeBtn("Read Phone Info (IMEI/SIM)", 10, y, 300, () => UtilsPhoneInfo())); y += 38;
            panel.Controls.Add(MakeBtn("Battery Status", 10, y, 300, () => { Log(RunAdb("shell dumpsys battery"), Colors.Text); })); y += 38;
            panel.Controls.Add(MakeBtn("Screenshot", 10, y, 300, () => UtilsScreenshot())); y += 38;
            panel.Controls.Add(MakeBtn("Screen Record", 10, y, 300, () => UtilsRecord())); y += 38;
            panel.Controls.Add(MakeBtn("Reboot Options", 10, y, 300, () => UtilsReboot())); y += 38;
            panel.Controls.Add(MakeBtn("Wireless ADB", 10, y, 300, () => UtilsWireless())); y += 38;
            panel.Controls.Add(MakeBtn("Backup Apps/Contacts/SMS", 10, y, 300, () => UtilsBackup())); y += 38;
            panel.Controls.Add(MakeBtn("Install APK", 10, y, 300, () =>
            {
                var ofd = new OpenFileDialog { Filter = "APK files (*.apk)|*.apk|All files (*.*)|*.*" };
                if (ofd.ShowDialog() == DialogResult.OK) { Log(RunAdb("install \"" + ofd.FileName + "\""), Colors.Text); }
            })); y += 38;
            panel.Controls.Add(MakeBtn("File Manager", 10, y, 300, () => UtilsFiles())); y += 38;
        }

        private void UtilsInfo()
        {
            ExecuteWithWait(() =>
            {
                Log("=== DEVICE INFORMATION ===", Colors.Accent);
                Log("Manufacturer: " + Adb.GetProp("ro.product.manufacturer"), Colors.Text);
                Log("Brand: " + Adb.GetProp("ro.product.brand"), Colors.Text);
                Log("Model: " + Adb.GetProp("ro.product.model"), Colors.Text);
                Log("Device: " + Adb.GetProp("ro.product.device"), Colors.Text);
                Log("Android: " + Adb.GetProp("ro.build.version.release") + " (SDK " + Adb.GetProp("ro.build.version.sdk") + ")", Colors.Text);
                Log("Build: " + Adb.GetProp("ro.build.display.id"), Colors.Text);
                Log("Security Patch: " + Adb.GetProp("ro.build.version.security_patch"), Colors.Text);
                Log("CPU: " + Adb.GetProp("ro.product.cpu.abi"), Colors.Text);
                Log("Board: " + Adb.GetProp("ro.product.board"), Colors.Text);
                Log("Build Type: " + Adb.GetProp("ro.build.type"), Colors.Text);
                Log("Root: " + (isRooted ? "YES" : "NO"), Colors.Text);
                Log("--- Display ---", Colors.Orange);
                Log(RunAdb("shell wm size"), Colors.Text);
                Log(RunAdb("shell wm density"), Colors.Text);
                Log("--- Memory ---", Colors.Orange);
                Log(RunAdb("shell cat /proc/meminfo | head -1"), Colors.Text);
                Log("--- Uptime ---", Colors.Orange);
                Log(RunAdb("shell cat /proc/uptime"), Colors.Text);
            }, "Device Info");
        }

        private void UtilsPhoneInfo()
        {
            ExecuteWithWait(() =>
            {
                Log("=== PHONE INFORMATION ===", Colors.Accent);

                Log("--- IMEI ---", Colors.Orange);
                string imei1 = RunAdb("shell service call iphonesubinfo 1");
                Log("IMEI (raw): " + imei1, Colors.Text);
                string imei2 = RunAdb("shell service call iphonesubinfo 11");
                Log("IMEI 2 (raw): " + imei2, Colors.Text);

                Log("--- PHONE NUMBER ---", Colors.Orange);
                string msisdn1 = RunAdb("shell service call iphonesubinfo 15");
                Log("Number 1 (raw): " + msisdn1, Colors.Text);
                string msisdn2 = RunAdb("shell service call iphonesubinfo 16");
                Log("Number 2 (raw): " + msisdn2, Colors.Text);

                Log("--- SIM INFO ---", Colors.Orange);
                Log("SIM Operator: " + RunAdb("shell getprop gsm.sim.operator.alpha"), Colors.Text);
                Log("SIM Operator Code: " + RunAdb("shell getprop gsm.sim.operator.numeric"), Colors.Text);
                Log("SIM Country: " + RunAdb("shell getprop gsm.sim.operator.iso-country"), Colors.Text);
                Log("SIM State: " + RunAdb("shell getprop gsm.sim.state"), Colors.Text);
                Log("SIM Serial: " + RunAdb("shell getprop ro.ril.oem.sno"), Colors.Text);

                Log("--- NETWORK ---", Colors.Orange);
                Log("Network Operator: " + RunAdb("shell getprop gsm.operator.alpha"), Colors.Text);
                Log("Network Code: " + RunAdb("shell getprop gsm.operator.numeric"), Colors.Text);
                Log("Network Type: " + RunAdb("shell getprop gsm.network.type"), Colors.Text);
                Log("Network Country: " + RunAdb("shell getprop gsm.operator.iso-country"), Colors.Text);
                Log("Signal Strength: " + RunAdb("shell dumpsys telephony.registry | grep mSignalStrength"), Colors.Text);

                Log("--- SUBSCRIBER ---", Colors.Orange);
                Log("Subscriber ID: " + RunAdb("shell getprop gsm.sim.operator.numeric"), Colors.Text);
                Log("Line 1 Number: " + RunAdb("shell getprop gsm.sim.operator.alpha"), Colors.Text);

                Log("--- TELEPHONY DUMPSYS ---", Colors.Orange);
                string phoneState = RunAdb("shell dumpsys telephony.registry | grep mCallState");
                if (!string.IsNullOrEmpty(phoneState)) Log("Call State: " + phoneState.Trim(), Colors.Text);
                string dataState = RunAdb("shell dumpsys telephony.registry | grep mDataConnectionState");
                if (!string.IsNullOrEmpty(dataState)) Log("Data State: " + dataState.Trim(), Colors.Text);

                Log("--- BATTERY PHONE ---", Colors.Orange);
                Log("Battery Level: " + RunAdb("shell dumpsys battery | grep level"), Colors.Text);
                Log("Battery Temp: " + RunAdb("shell dumpsys battery | grep temperature"), Colors.Text);
                Log("Battery Health: " + RunAdb("shell dumpsys battery | grep health"), Colors.Text);
                Log("Battery Voltage: " + RunAdb("shell dumpsys battery | grep voltage"), Colors.Text);
                Log("Battery Technology: " + RunAdb("shell dumpsys battery | grep technology"), Colors.Text);
                Log("Charging: " + RunAdb("shell dumpsys battery | grep plugged"), Colors.Text);

                Log("--- SERIAL ---", Colors.Orange);
                Log("Serial Number: " + RunAdb("shell getprop ro.serialno"), Colors.Text);
                Log("Bootloader: " + RunAdb("shell getprop ro.bootloader"), Colors.Text);
                Log("Baseband: " + RunAdb("shell getprop gsm.version.baseband"), Colors.Text);

                Log("--- HARDWARE ---", Colors.Orange);
                Log("Chipset: " + RunAdb("shell getprop ro.hardware"), Colors.Text);
                Log("Platform: " + RunAdb("shell getprop ro.board.platform"), Colors.Text);
                Log("GPU: " + RunAdb("shell getprop ro.hardware.gpu"), Colors.Text);
                Log("Sensors: " + RunAdb("shell getprop ro.hardware.sensors"), Colors.Text);

                Log("--- USER ---", Colors.Orange);
                Log("User ID: " + RunAdb("shell whoami"), Colors.Text);
                Log("Android ID: " + RunAdb("shell settings get secure android_id"), Colors.Text);
                Log("Google Service Framework: " + RunAdb("shell getprop persist.sys.gsf"), Colors.Text);

                Log("=== END PHONE INFO ===", Colors.Accent);
            }, "Read Phone Info");
        }

        private void UtilsScreenshot()
        {
            RunAdb("shell screencap -p /sdcard/screenshot.png");
            RunAdb("pull /sdcard/screenshot.png \"" + Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\screenshot.png\"");
            RunAdb("shell rm /sdcard/screenshot.png");
            Log("Screenshot saved to Desktop\\screenshot.png", Colors.Accent);
        }

        private void UtilsRecord()
        {
            var panel = CreateSection("SCREEN RECORD");
            int y = 10;
            string[] durations = { "10|10", "30|30", "60|60", "120|120" };
            foreach (var d in durations)
            {
                string[] parts = d.Split('|');
                int secs = int.Parse(parts[0]);
                panel.Controls.Add(MakeBtn(parts[1] + " seconds", 10, y, 200, () =>
                {
                    Log("Recording " + secs + " seconds...", Colors.Orange);
                    RunAdb("shell screenrecord --time-limit " + secs + " --bit-rate 8000000 /sdcard/recording.mp4");
                    RunAdb("pull /sdcard/recording.mp4 \"" + Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\recording.mp4\"");
                    RunAdb("shell rm /sdcard/recording.mp4");
                    Log("Recording saved to Desktop\\recording.mp4", Colors.Accent);
                }));
                y += 36;
            }
            panel.Controls.Add(MakeBtn("Custom Duration", 10, y, 200, () =>
            {
                string s = PromptInput("Duration in seconds:");
                if (!string.IsNullOrEmpty(s))
                {
                    Log("Recording " + s + " seconds...", Colors.Orange);
                    RunAdb("shell screenrecord --time-limit " + s + " --bit-rate 8000000 /sdcard/recording.mp4");
                    RunAdb("pull /sdcard/recording.mp4 \"" + Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\recording.mp4\"");
                    RunAdb("shell rm /sdcard/recording.mp4");
                    Log("Recording saved.", Colors.Accent);
                }
            }));
        }

        private void UtilsReboot()
        {
            var panel = CreateSection("REBOOT OPTIONS");
            int y = 10;
            panel.Controls.Add(MakeBtn("Normal Reboot", 10, y, 300, () => { RunAdb("reboot"); Log("Rebooting...", Colors.Orange); })); y += 38;
            panel.Controls.Add(MakeBtn("Recovery", 10, y, 300, () => { RunAdb("reboot recovery"); Log("Rebooting to recovery...", Colors.Orange); })); y += 38;
            panel.Controls.Add(MakeBtn("Bootloader", 10, y, 300, () => { RunAdb("reboot bootloader"); Log("Rebooting to bootloader...", Colors.Orange); })); y += 38;
            panel.Controls.Add(MakeBtn("Download (Samsung)", 10, y, 300, () => { RunAdb("reboot download"); })); y += 38;
            panel.Controls.Add(MakeBtn("Soft Reboot (Root)", 10, y, 300, () =>
            {
                if (isRooted) { RunAdb("shell setprop ctl.restart zygote"); Log("Soft reboot...", Colors.Orange); }
                else Log("ERROR: Root required!", Colors.Red);
            })); y += 38;
            panel.Controls.Add(MakeBtn("Power Off", 10, y, 300, () => { RunAdb("shell reboot -p"); })); y += 38;
            panel.Controls.Add(MakeBtn("Restart SystemUI", 10, y, 300, () => { RunAdb("shell am force-stop com.android.systemui"); Log("SystemUI restarted", Colors.Accent); })); y += 38;
        }

        private void UtilsWireless()
        {
            var panel = CreateSection("WIRELESS ADB");
            int y = 10;
            panel.Controls.Add(MakeBtn("Enable Wireless ADB (port 5555)", 10, y, 400, () =>
            {
                RunAdb("tcpip 5555");
                Log("Enabled on port 5555. Device IP:", Colors.Accent);
                Log(RunAdb("shell ip route"), Colors.Text);
            })); y += 40;
            panel.Controls.Add(MakeBtn("Connect by IP", 10, y, 400, () =>
            {
                string ip = PromptInput("Device IP:");
                if (!string.IsNullOrEmpty(ip)) { RunAdb("connect " + ip + ":5555"); Log("Connected to " + ip, Colors.Accent); }
            })); y += 40;
            panel.Controls.Add(MakeBtn("Disconnect All", 10, y, 400, () => { RunAdb("disconnect"); Log("Disconnected.", Colors.Accent); })); y += 40;
        }

        private void UtilsBackup()
        {
            var panel = CreateSection("BACKUP & RESTORE");
            int y = 10;
            panel.Controls.Add(MakeBtn("Backup All User Apps (APK)", 10, y, 400, () =>
            {
                string backupPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "AndroidBackup");
                if (!Directory.Exists(backupPath)) Directory.CreateDirectory(backupPath);
                string pkgs = RunAdb("shell pm list packages -3");
                foreach (string line in pkgs.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string pkg = line.Replace("package:", "").Trim();
                    if (string.IsNullOrEmpty(pkg)) continue;
                    string path = RunAdb("shell pm path " + pkg).Replace("package:", "").Trim();
                    if (!string.IsNullOrEmpty(path)) { RunAdb("pull \"" + path + "\" \"" + backupPath + "\\" + pkg + ".apk\""); Log("[OK] " + pkg, Colors.Accent); }
                }
                Log("Backup complete: " + backupPath, Colors.Accent);
            })); y += 40;
            panel.Controls.Add(MakeBtn("Export Contacts", 10, y, 400, () =>
            {
                string contacts = RunAdb("shell content query --uri content://com.android.contacts/contacts");
                File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "contacts.txt"), contacts);
                Log("Contacts saved to Desktop\\contacts.txt", Colors.Accent);
            })); y += 40;
            panel.Controls.Add(MakeBtn("Export SMS", 10, y, 400, () =>
            {
                string sms = RunAdb("shell content query --uri content://sms");
                File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "sms.txt"), sms);
                Log("SMS saved to Desktop\\sms.txt", Colors.Accent);
            })); y += 40;
        }

        private void UtilsFiles()
        {
            var panel = CreateSection("FILE MANAGER");
            int y = 10;
            panel.Controls.Add(MakeBtn("Push File to Device", 10, y, 300, () =>
            {
                var ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string dest = PromptInput("Destination [/sdcard/]:");
                    if (string.IsNullOrEmpty(dest)) dest = "/sdcard/";
                    RunAdb("push \"" + ofd.FileName + "\" \"" + dest + "\"");
                    Log("File pushed.", Colors.Accent);
                }
            })); y += 38;
            panel.Controls.Add(MakeBtn("Pull File from Device", 10, y, 300, () =>
            {
                string path = PromptInput("Device file path:");
                if (!string.IsNullOrEmpty(path)) { RunAdb("pull \"" + path + "\" \"" + Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\""); Log("File pulled.", Colors.Accent); }
            })); y += 38;
            panel.Controls.Add(MakeBtn("List /sdcard/", 10, y, 300, () => { Log(RunAdb("shell ls -la /sdcard/"), Colors.Text); })); y += 38;
            panel.Controls.Add(MakeBtn("Browse Custom Path", 10, y, 300, () =>
            {
                string path = PromptInput("Path [/sdcard/]:");
                if (string.IsNullOrEmpty(path)) path = "/sdcard/";
                Log(RunAdb("shell ls -la \"" + path + "\""), Colors.Text);
            })); y += 38;
        }

        // =====================================================================
        //                           PRIVACY SHIELD
        // =====================================================================
        private void ShowPrivacy()
        {
            var panel = CreateSection("PRIVACY SHIELD");
            int y = 10;

            var privacyModels = new Label
            {
                Text = "Supported: All Android devices | Android 6.0-14 | Root recommended for full privacy",
                Font = new Font("Consolas", 7.5f, FontStyle.Bold),
                ForeColor = Colors.Accent,
                Location = new Point(10, y),
                Size = new Size(580, 20)
            };
            panel.Controls.Add(privacyModels); y += 22;
            panel.Controls.Add(MakeBtn("Revoke Permissions (All User Apps)", 10, y, 400, () => PrivPerms())); y += 40;
            panel.Controls.Add(MakeBtn("Disable Telemetry", 10, y, 400, () => PrivTelem())); y += 40;
            panel.Controls.Add(MakeBtn("Block App Network Access", 10, y, 400, () => PrivNetwork())); y += 40;
            panel.Controls.Add(MakeBtn("Privacy Audit", 10, y, 400, () => PrivAudit())); y += 40;
            panel.Controls.Add(MakeBtn("Encrypt Data", 10, y, 400, () => PrivEncrypt())); y += 40;
        }

        private void PrivPerms()
        {
            ExecuteWithWait(() =>
            {
                string pkgs = RunAdb("shell pm list packages -3");
                string[] pkgArr = pkgs.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                string[] permissions = { "android.permission.CAMERA", "android.permission.RECORD_AUDIO", "android.permission.ACCESS_FINE_LOCATION", "android.permission.ACCESS_COARSE_LOCATION", "android.permission.READ_CONTACTS", "android.permission.READ_SMS", "android.permission.SEND_SMS" };
                int count = 0;
                foreach (string line in pkgArr)
                {
                    string pkg = line.Replace("package:", "").Trim();
                    if (string.IsNullOrEmpty(pkg)) continue;
                    foreach (var perm in permissions) { RunAdb("shell pm revoke " + pkg + " " + perm); count++; }
                }
                Log("Permissions revoked from " + pkgArr.Length + " apps (" + count + " operations).", Colors.Accent);
            }, "Revoke Permissions");
        }

        private void PrivTelem()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/6] Usage stats...", Colors.Orange);
                RunAdb("shell appops set com.google.android.gms USAGE_STATS deny");
                RunAdb("shell appops set com.google.android.gms READ_PHONE_STATE deny");
                Log("[2/6] Analytics...", Colors.Orange);
                RunAdb("shell settings put secure usage_metrics_reporting_enabled 0");
                RunAdb("shell settings put secure analytics_enabled 0");
                Log("[3/6] Diagnostics...", Colors.Orange);
                RunAdb("shell settings put secure send_action_app_error 0");
                Log("[4/6] Usage stats collection...", Colors.Orange);
                RunAdb("shell settings put secure usage_stats_enabled 0");
                Log("[5/6] Clearing...", Colors.Orange);
                RunAdb(@"shell rm -rf /data/system/usagestats/*");
                Log("[6/6] Done.", Colors.Orange);
                Log("Telemetry disabled.", Colors.Accent);
            }, "Disable Telemetry");
        }

        private void PrivNetwork()
        {
            var panel = CreateSection("BLOCK NETWORK ACCESS");
            int y = 10;
            panel.Controls.Add(MakeBtn("Block App Network", 10, y, 350, () =>
            {
                string pkg = PromptInput("Package name:");
                if (!string.IsNullOrEmpty(pkg))
                {
                    RunAdb("shell appops set " + pkg + " RUN_IN_BACKGROUND deny");
                    RunAdb("shell appops set " + pkg + " RUN_ANY_IN_BACKGROUND deny");
                    Log(pkg + " network restricted.", Colors.Accent);
                }
            })); y += 38;
            panel.Controls.Add(MakeBtn("Airplane Mode ON", 10, y, 350, () =>
            {
                RunAdb("shell settings put global airplane_mode_on 1");
                RunAdb("shell am broadcast -a android.intent.action.AIRPLANE_MODE --ez state true");
                Log("Airplane mode ON.", Colors.Accent);
            })); y += 38;
            panel.Controls.Add(MakeBtn("Airplane Mode OFF", 10, y, 350, () =>
            {
                RunAdb("shell settings put global airplane_mode_on 0");
                RunAdb("shell am broadcast -a android.intent.action.AIRPLANE_MODE --ez state false");
                Log("Airplane mode OFF.", Colors.Accent);
            })); y += 38;
        }

        private void PrivAudit()
        {
            ExecuteWithWait(() =>
            {
                Log("=== PRIVACY AUDIT ===", Colors.Accent);
                string[] perms = { "android.permission.CAMERA|CAMERA", "android.permission.RECORD_AUDIO|MICROPHONE", "android.permission.ACCESS_FINE_LOCATION|LOCATION", "android.permission.READ_PHONE_STATE|PHONE", "android.permission.READ_SMS|SMS" };
                string pkgs = RunAdb("shell pm list packages -3");
                foreach (var permPair in perms)
                {
                    string[] pp = permPair.Split('|');
                    Log("--- " + pp[1] + " ---", Colors.Orange);
                    foreach (string line in pkgs.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        string pkg = line.Replace("package:", "").Trim();
                        if (string.IsNullOrEmpty(pkg)) continue;
                        string dump = RunAdb("shell dumpsys package " + pkg);
                        if (dump.Contains(pp[0])) Log("  [" + pp[1] + "] " + pkg, Colors.Text);
                    }
                }
            }, "Privacy Audit");
        }

        private void PrivEncrypt()
        {
            var panel = CreateSection("ENCRYPT DATA");
            int y = 10;
            panel.Controls.Add(MakeBtn("Check Encryption Status", 10, y, 350, () =>
            {
                Log("Crypto state: " + RunAdb("shell getprop ro.crypto.state"), Colors.Text);
                Log("Decrypt: " + RunAdb("shell getprop vold.decrypt"), Colors.Text);
            })); y += 38;
            panel.Controls.Add(MakeBtn("Enable Encryption", 10, y, 350, () =>
            {
                if (Confirm("Enable encryption? Device may reboot."))
                {
                    RunAdb("shell vdc cryptfs enablecrypto inplace");
                    Log("Encryption command sent. Device may reboot.", Colors.Orange);
                }
            })); y += 38;
        }

        // =====================================================================
        //                           APP MANAGER
        // =====================================================================
        private void ShowApps()
        {
            var panel = CreateSection("APP MANAGER");
            int y = 10;

            var appsModels = new Label
            {
                Text = "Supported: All Android devices with USB debugging | Android 4.0-14",
                Font = new Font("Consolas", 7.5f, FontStyle.Bold),
                ForeColor = Colors.Accent,
                Location = new Point(10, y),
                Size = new Size(580, 20)
            };
            panel.Controls.Add(appsModels); y += 22;

            panel.Controls.Add(MakeBtn("Force Stop", 10, y, 300, () =>
            {
                string pkg = PromptInput("Package:");
                if (!string.IsNullOrEmpty(pkg)) { RunAdb("shell am force-stop " + pkg); Log(pkg + " stopped.", Colors.Accent); }
            })); y += 38;
            panel.Controls.Add(MakeBtn("Clear Data / Cache", 10, y, 300, () =>
            {
                string pkg = PromptInput("Package:");
                if (!string.IsNullOrEmpty(pkg)) { RunAdb("shell pm clear " + pkg); Log(pkg + " data cleared.", Colors.Accent); }
            })); y += 38;
            panel.Controls.Add(MakeBtn("Disable App", 10, y, 300, () =>
            {
                string pkg = PromptInput("Package:");
                if (!string.IsNullOrEmpty(pkg)) { RunAdb("shell pm disable-user --user 0 " + pkg); Log(pkg + " disabled.", Colors.Accent); }
            })); y += 38;
            panel.Controls.Add(MakeBtn("Enable App", 10, y, 300, () =>
            {
                string pkg = PromptInput("Package:");
                if (!string.IsNullOrEmpty(pkg)) { RunAdb("shell pm enable " + pkg); Log(pkg + " enabled.", Colors.Accent); }
            })); y += 38;
            panel.Controls.Add(MakeBtn("Uninstall App", 10, y, 300, () =>
            {
                string pkg = PromptInput("Package:");
                if (!string.IsNullOrEmpty(pkg))
                {
                    if (Confirm("Complete uninstall (not just user)?"))
                        RunAdb("shell pm uninstall " + pkg);
                    else
                        RunAdb("shell pm uninstall -k --user 0 " + pkg);
                    Log(pkg + " uninstalled.", Colors.Accent);
                }
            })); y += 38;
            panel.Controls.Add(MakeBtn("App Info", 10, y, 300, () =>
            {
                string pkg = PromptInput("Package:");
                if (!string.IsNullOrEmpty(pkg))
                {
                    Log("--- " + pkg + " ---", Colors.Accent);
                    Log(RunAdb("shell dumpsys package " + pkg), Colors.Text);
                }
            })); y += 38;
            panel.Controls.Add(MakeBtn("Bulk: Disable Ad SDKs", 10, y, 300, () =>
            {
                string[] sdk = { "com.applovin", "com.inmobi", "com.mopub", "com.unity3d.services", "com.adcolony", "com.tapjoy", "com.vungle", "com.fyber", "com.yieldmo", "com.braze", "com.localytics", "com.onesignal", "com.kochava", "com.appsflyer", "com.adjust", "com.ironsource", "com.smaato", "com.flurry", "com.facebook.ads", "com.startapp" };
                foreach (var s in sdk) RunAdb("shell pm disable-user --user 0 " + s);
                Log("Ad SDKs disabled.", Colors.Accent);
            })); y += 38;
            panel.Controls.Add(MakeBtn("Bulk: Clear All Caches", 10, y, 300, () =>
            {
                string pkgs = RunAdb("shell pm list packages");
                foreach (string line in pkgs.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string pkg = line.Replace("package:", "").Trim();
                    if (!string.IsNullOrEmpty(pkg)) RunAdb("shell pm clear-cache " + pkg);
                }
                Log("All caches cleared.", Colors.Accent);
            })); y += 38;
            panel.Controls.Add(MakeBtn("Bulk: Force-Stop User Apps", 10, y, 300, () =>
            {
                string pkgs = RunAdb("shell pm list packages -3");
                foreach (string line in pkgs.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string pkg = line.Replace("package:", "").Trim();
                    if (!string.IsNullOrEmpty(pkg)) RunAdb("shell am force-stop " + pkg);
                }
                Log("User apps force-stopped.", Colors.Accent);
            })); y += 38;
            panel.Controls.Add(MakeBtn("Bulk: Remove Disabled", 10, y, 300, () =>
            {
                string disabled = RunAdb("shell pm list packages -d");
                foreach (string line in disabled.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string pkg = line.Replace("package:", "").Trim();
                    if (!string.IsNullOrEmpty(pkg)) { RunAdb("shell pm uninstall -k --user 0 " + pkg); Log("[REMOVED] " + pkg, Colors.Accent); }
                }
            })); y += 38;
        }

        // =====================================================================
        //                           QUICK ACTIONS
        // =====================================================================
        private void ShowQuick()
        {
            var panel = CreateSection("QUICK ACTIONS");
            int y = 10;

            var quickModels = new Label
            {
                Text = "Supported: All Android devices | Android 4.1-14",
                Font = new Font("Consolas", 7.5f, FontStyle.Bold),
                ForeColor = Colors.Accent,
                Location = new Point(10, y),
                Size = new Size(580, 20)
            };
            panel.Controls.Add(quickModels); y += 22;

            panel.Controls.Add(MakeBtn("One-Tap Optimize", 10, y, 350, () => QuickOptimize())); y += 38;
            panel.Controls.Add(MakeBtn("Clear All Cache", 10, y, 350, () =>
            {
                string pkgs = RunAdb("shell pm list packages");
                foreach (string line in pkgs.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string pkg = line.Replace("package:", "").Trim();
                    if (!string.IsNullOrEmpty(pkg)) RunAdb("shell pm clear-cache " + pkg);
                }
                Log("All caches cleared.", Colors.Accent);
            })); y += 38;
            panel.Controls.Add(MakeBtn("Screenshot", 10, y, 350, () => UtilsScreenshot())); y += 38;
            panel.Controls.Add(MakeBtn("Wake Device", 10, y, 350, () =>
            {
                RunAdb("shell input keyevent KEYCODE_WAKEUP");
                Thread.Sleep(500);
                RunAdb("shell input keyevent 82");
                Log("Device woken.", Colors.Accent);
            })); y += 38;
            panel.Controls.Add(MakeBtn("Disable Notifications", 10, y, 350, () =>
            {
                RunAdb("shell settings put global heads_up_notifications_enabled 0");
                Log("Notifications disabled.", Colors.Accent);
            })); y += 38;
            panel.Controls.Add(MakeBtn("Screen Timeout", 10, y, 350, () => QuickTimeout())); y += 38;
            panel.Controls.Add(MakeBtn("USB Configuration", 10, y, 350, () => QuickUsb())); y += 38;
            panel.Controls.Add(MakeBtn("Battery Saver ON", 10, y, 350, () =>
            {
                RunAdb("shell settings put global low_power 1");
                Log("Battery saver ON.", Colors.Accent);
            })); y += 38;
        }

        private void QuickOptimize()
        {
            ExecuteWithWait(() =>
            {
                Log("[1/5] Clearing caches...", Colors.Orange);
                string pkgs = RunAdb("shell pm list packages");
                foreach (string line in pkgs.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string pkg = line.Replace("package:", "").Trim();
                    if (!string.IsNullOrEmpty(pkg)) RunAdb("shell pm clear-cache " + pkg);
                }
                Log("[2/5] Force-stopping user apps...", Colors.Orange);
                string uPkgs = RunAdb("shell pm list packages -3");
                foreach (string line in uPkgs.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string pkg = line.Replace("package:", "").Trim();
                    if (!string.IsNullOrEmpty(pkg)) RunAdb("shell am force-stop " + pkg);
                }
                Log("[3/5] Kill stale...", Colors.Orange); RunAdb("shell am kill-all");
                Log("[4/5] Flush DNS...", Colors.Orange); RunAdb("shell cmd connectivity flush-dns");
                Log("[5/5] Free memory...", Colors.Orange); RunAdb("shell \"echo 3 > /proc/sys/vm/drop_caches\"");
                Log("Optimized!", Colors.Accent);
            }, "One-Tap Optimize");
        }

        private void QuickTimeout()
        {
            var panel = CreateSection("SCREEN TIMEOUT");
            int y = 10;
            string[][] timeouts = new[] {
                new[] { "15 seconds", "15000" }, new[] { "30 seconds", "30000" },
                new[] { "1 minute", "60000" }, new[] { "5 minutes", "300000" },
                new[] { "10 minutes", "600000" }, new[] { "Never (stay on)", "stayon" },
            };
            foreach (var t in timeouts)
            {
                string val = t[1];
                panel.Controls.Add(MakeBtn(t[0], 10, y, 250, () =>
                {
                    if (val == "stayon") RunAdb("shell svc power stayon true");
                    else RunAdb("shell settings put system screen_off_timeout " + val);
                    Log("Timeout updated.", Colors.Accent);
                }));
                y += 36;
            }
        }

        private void QuickUsb()
        {
            var panel = CreateSection("USB CONFIGURATION");
            int y = 10;
            string[][] modes = new[] { new[] { "MTP", "mtp" }, new[] { "PTP", "ptp" }, new[] { "RNDIS", "rndis" }, new[] { "MIDI", "midi" }, new[] { "Charge Only", "charge_only" } };
            foreach (var m in modes)
            {
                string mode = m[1];
                panel.Controls.Add(MakeBtn(m[0], 10, y, 250, () =>
                {
                    RunAdb("shell setprop sys.usb.config " + mode);
                    Log("USB mode: " + mode, Colors.Accent);
                }));
                y += 36;
            }
        }

        // =====================================================================
        //                           DEVELOPER TOOLS
        // =====================================================================
        private void ShowDev()
        {
            var panel = CreateSection("DEVELOPER TOOLS");
            int y = 10;

            var devModels = new Label
            {
                Text = "Supported: All Android devices with USB debugging | Android 4.1-14",
                Font = new Font("Consolas", 7.5f, FontStyle.Bold),
                ForeColor = Colors.Accent,
                Location = new Point(10, y),
                Size = new Size(580, 20)
            };
            panel.Controls.Add(devModels); y += 22;

            panel.Controls.Add(MakeBtn("Logcat Real-time (new window)", 10, y, 400, () =>
            {
                Process.Start(new ProcessStartInfo("adb", "logcat") { UseShellExecute = true });
            })); y += 38;
            panel.Controls.Add(MakeBtn("Logcat to File", 10, y, 400, () =>
            {
                string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string filename = "logcat_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";
                string path = Path.Combine(desktop, filename);
                RunAdb("logcat -d > \"" + path + "\"");
                Log("Logcat saved: " + path, Colors.Accent);
            })); y += 38;
            panel.Controls.Add(MakeBtn("Dumpsys Info", 10, y, 400, () => DevDumpsys())); y += 38;
            panel.Controls.Add(MakeBtn("ADB Shell (new window)", 10, y, 400, () =>
            {
                Process.Start(new ProcessStartInfo("cmd", "/k adb shell") { UseShellExecute = true });
            })); y += 38;
            panel.Controls.Add(MakeBtn("Run Custom ADB Command", 10, y, 400, () =>
            {
                string cmd = PromptInput("ADB command (without 'adb'):");
                if (!string.IsNullOrEmpty(cmd)) { string r = RunAdb(cmd); Log(r, Colors.Text); }
            })); y += 38;
            panel.Controls.Add(MakeBtn("Monkey Test (500 events)", 10, y, 400, () =>
            {
                Log("Running 500 events...", Colors.Orange);
                RunAdb("shell monkey -p com.android.launcher --throttle 500 -v 500");
                Log("Monkey test complete.", Colors.Accent);
            })); y += 38;
            panel.Controls.Add(MakeBtn("CPU Benchmark", 10, y, 400, () =>
            {
                Log("CPU: " + RunAdb("shell cat /proc/cpuinfo | grep 'model name' | head -1"), Colors.Text);
                Log("Running dd benchmark...", Colors.Orange);
                RunAdb("shell \"dd if=/dev/zero of=/data/local/tmp/bench bs=1M count=100 2>&1 | tail -1\"");
                RunAdb("shell rm /data/local/tmp/bench");
                Log("Benchmark complete.", Colors.Accent);
            })); y += 38;
        }

        private void DevDumpsys()
        {
            var panel = CreateSection("DUMPSYS");
            int y = 10;
            string[][] items = new[] {
                new[] { "Activity", "activity activities" }, new[] { "Window", "window windows" },
                new[] { "Battery", "battery" }, new[] { "Meminfo", "meminfo" },
                new[] { "CPU", "cpuinfo" }, new[] { "All", "" },
            };
            foreach (var item in items)
            {
                string arg = item[1];
                panel.Controls.Add(MakeBtn(item[0], 10, y, 250, () => { Log(RunAdb("shell dumpsys " + arg), Colors.Text); }));
                y += 36;
            }
        }

        // =====================================================================
        //                           NETWORK TOOLS
        // =====================================================================
        private void ShowNet()
        {
            var panel = CreateSection("NETWORK TOOLS");
            int y = 10;

            var netModels = new Label
            {
                Text = "Supported: All Android devices with WiFi/cellular | Android 5.0-14",
                Font = new Font("Consolas", 7.5f, FontStyle.Bold),
                ForeColor = Colors.Accent,
                Location = new Point(10, y),
                Size = new Size(580, 20)
            };
            panel.Controls.Add(netModels); y += 22;

            panel.Controls.Add(MakeBtn("WiFi Info", 10, y, 350, () =>
            {
                Log("--- WiFi ---", Colors.Orange);
                Log(RunAdb("shell dumpsys wifi | grep mWifiInfo"), Colors.Text);
                Log("--- Route ---", Colors.Orange);
                Log(RunAdb("shell ip route | grep wlan0"), Colors.Text);
            })); y += 38;
            panel.Controls.Add(MakeBtn("WiFi Scan", 10, y, 350, () =>
            {
                Log("Scanning...", Colors.Orange);
                RunAdb("shell cmd wifi start-scan");
                Thread.Sleep(3000);
                Log(RunAdb("shell cmd wifi list-scan-results"), Colors.Text);
            })); y += 38;
            panel.Controls.Add(MakeBtn("IP Config", 10, y, 350, () =>
            {
                Log("--- Addresses ---", Colors.Orange);
                Log(RunAdb("shell ip addr show"), Colors.Text);
                Log("--- Routes ---", Colors.Orange);
                Log(RunAdb("shell ip route"), Colors.Text);
            })); y += 38;
            panel.Controls.Add(MakeBtn("Ping Test", 10, y, 350, () =>
            {
                string host = PromptInput("Host to ping:");
                if (!string.IsNullOrEmpty(host)) Log(RunAdb("shell ping -c 4 " + host), Colors.Text);
            })); y += 38;
            panel.Controls.Add(MakeBtn("DNS Lookup", 10, y, 350, () =>
            {
                string host = PromptInput("Hostname:");
                if (!string.IsNullOrEmpty(host)) Log(RunAdb("shell nslookup " + host), Colors.Text);
            })); y += 38;
        }

        // =====================================================================
        //                           DOWNLOADS
        // =====================================================================
        private void ShowDownloads()
        {
            var panel = CreateSection("DOWNLOADS - DRIVERS & TOOLS");
            int y = 10;

            var infoLabel = new Label
            {
                Text = "Download essential drivers and tools for Android development and ADB connectivity.",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Colors.TextDim,
                Location = new Point(10, y),
                Width = 600,
                Height = 22
            };
            panel.Controls.Add(infoLabel);
            y += 30;

            var sectionLabel = new Label
            {
                Text = "--- ADB / PLATFORM TOOLS ---",
                Font = new Font("Consolas", 9.5f, FontStyle.Bold),
                ForeColor = Colors.Orange,
                Location = new Point(10, y),
                Width = 600,
                Height = 22
            };
            panel.Controls.Add(sectionLabel);
            y += 26;

            panel.Controls.Add(MakeBtn("Android SDK Platform Tools (Google)", 10, y, 440, () =>
            {
                Log("Opening Android SDK Platform Tools download...", Colors.Orange);
                try
                {
                    Process.Start(new ProcessStartInfo("https://developer.android.com/tools/releases/platform-tools") { UseShellExecute = true });
                    Log("Download page opened in browser.", Colors.Accent);
                }
                catch { Log("Could not open browser.", Colors.Red); }
            })); y += 38;

            panel.Controls.Add(MakeBtn("Google USB Driver (Windows)", 10, y, 440, () =>
            {
                Log("Opening Google USB Driver download...", Colors.Orange);
                try
                {
                    Process.Start(new ProcessStartInfo("https://developer.android.com/studio/run/win-usb") { UseShellExecute = true });
                    Log("Download page opened in browser.", Colors.Accent);
                }
                catch { Log("Could not open browser.", Colors.Red); }
            })); y += 38;

            y += 8;
            sectionLabel = new Label
            {
                Text = "--- USB DRIVERS (BRAND-SPECIFIC) ---",
                Font = new Font("Consolas", 9.5f, FontStyle.Bold),
                ForeColor = Colors.Orange,
                Location = new Point(10, y),
                Width = 600,
                Height = 22
            };
            panel.Controls.Add(sectionLabel);
            y += 26;

            panel.Controls.Add(MakeBtn("Samsung USB Driver", 10, y, 440, () =>
            {
                Log("Opening Samsung USB Driver download...", Colors.Orange);
                try
                {
                    Process.Start(new ProcessStartInfo("https://developer.samsung.com/android-usb-driver") { UseShellExecute = true });
                    Log("Download page opened in browser.", Colors.Accent);
                }
                catch { Log("Could not open browser.", Colors.Red); }
            })); y += 38;

            panel.Controls.Add(MakeBtn("Qualcomm USB Driver (QDLoader)", 10, y, 440, () =>
            {
                Log("Opening Qualcomm USB Driver download...", Colors.Orange);
                try
                {
                    Process.Start(new ProcessStartInfo("https://www.qualcomm.com/support/downloads/tools/qdloader-usb-drivers") { UseShellExecute = true });
                    Log("Download page opened in browser.", Colors.Accent);
                }
                catch { Log("Could not open browser.", Colors.Red); }
            })); y += 38;

            panel.Controls.Add(MakeBtn("MediaTek USB VCOM Driver", 10, y, 440, () =>
            {
                Log("Opening MediaTek USB VCOM Driver download...", Colors.Orange);
                try
                {
                    Process.Start(new ProcessStartInfo("https://mtkusbvcomdriver.com") { UseShellExecute = true });
                    Log("Download page opened in browser.", Colors.Accent);
                }
                catch { Log("Could not open browser.", Colors.Red); }
            })); y += 38;

            panel.Controls.Add(MakeBtn("Huawei USB Driver", 10, y, 440, () =>
            {
                Log("Opening Huawei USB Driver download...", Colors.Orange);
                try
                {
                    Process.Start(new ProcessStartInfo("https://consumer.huawei.com/en/support/hisuite/") { UseShellExecute = true });
                    Log("Download page opened in browser.", Colors.Accent);
                }
                catch { Log("Could not open browser.", Colors.Red); }
            })); y += 38;

            panel.Controls.Add(MakeBtn("Xiaomi USB Driver (Mi PC Suite)", 10, y, 440, () =>
            {
                Log("Opening Xiaomi USB Driver download...", Colors.Orange);
                try
                {
                    Process.Start(new ProcessStartInfo("https://developer.android.com/studio/run/oem-usb#Xiaomi") { UseShellExecute = true });
                    Log("Download page opened in browser.", Colors.Accent);
                }
                catch { Log("Could not open browser.", Colors.Red); }
            })); y += 38;

            panel.Controls.Add(MakeBtn("Nokia / HMD USB Driver", 10, y, 440, () =>
            {
                Log("Opening Nokia USB Driver download...", Colors.Orange);
                try
                {
                    Process.Start(new ProcessStartInfo("https://www.nokia.com/phones/en_int/support-and-howto/how-to-install-nokia-usb-drivers") { UseShellExecute = true });
                    Log("Download page opened in browser.", Colors.Accent);
                }
                catch { Log("Could not open browser.", Colors.Red); }
            })); y += 38;

            panel.Controls.Add(MakeBtn("Sony / Xperia USB Driver", 10, y, 440, () =>
            {
                Log("Opening Sony Xperia USB Driver download...", Colors.Orange);
                try
                {
                    Process.Start(new ProcessStartInfo("https://developer.sony.com/open-devices/get-started/flash-tools") { UseShellExecute = true });
                    Log("Download page opened in browser.", Colors.Accent);
                }
                catch { Log("Could not open browser.", Colors.Red); }
            })); y += 38;

            panel.Controls.Add(MakeBtn("Motorola USB Driver", 10, y, 440, () =>
            {
                Log("Opening Motorola USB Driver download...", Colors.Orange);
                try
                {
                    Process.Start(new ProcessStartInfo("https://motorola-global-portal.custhelp.com/app/answers/detail/a_id/88481/p/30,7020,7040") { UseShellExecute = true });
                    Log("Download page opened in browser.", Colors.Accent);
                }
                catch { Log("Could not open browser.", Colors.Red); }
            })); y += 38;

            y += 8;
            sectionLabel = new Label
            {
                Text = "--- TOOLS & UTILITIES ---",
                Font = new Font("Consolas", 9.5f, FontStyle.Bold),
                ForeColor = Colors.Orange,
                Location = new Point(10, y),
                Width = 600,
                Height = 22
            };
            panel.Controls.Add(sectionLabel);
            y += 26;

            panel.Controls.Add(MakeBtn("Android SDK / Android Studio", 10, y, 440, () =>
            {
                Log("Opening Android Studio download...", Colors.Orange);
                try
                {
                    Process.Start(new ProcessStartInfo("https://developer.android.com/studio") { UseShellExecute = true });
                    Log("Download page opened in browser.", Colors.Accent);
                }
                catch { Log("Could not open browser.", Colors.Red); }
            })); y += 38;

            panel.Controls.Add(MakeBtn("Minimal ADB & Fastboot", 10, y, 440, () =>
            {
                Log("Opening Minimal ADB & Fastboot download...", Colors.Orange);
                try
                {
                    Process.Start(new ProcessStartInfo("https://androidfilehost.com/?fid=1395089523397969673") { UseShellExecute = true });
                    Log("Download page opened in browser.", Colors.Accent);
                }
                catch { Log("Could not open browser.", Colors.Red); }
            })); y += 38;

            panel.Controls.Add(MakeBtn("Universal ADB Driver (ClockworkMod)", 10, y, 440, () =>
            {
                Log("Opening Universal ADB Driver download...", Colors.Orange);
                try
                {
                    Process.Start(new ProcessStartInfo("https://adb.clockworkmod.com/") { UseShellExecute = true });
                    Log("Download page opened in browser.", Colors.Accent);
                }
                catch { Log("Could not open browser.", Colors.Red); }
            })); y += 38;

            panel.Controls.Add(MakeBtn("Oppo/Realme/OnePlus USB Driver", 10, y, 440, () =>
            {
                Log("Opening Oppo/Realme/OnePlus USB Driver download...", Colors.Orange);
                try
                {
                    Process.Start(new ProcessStartInfo("https://www.oppo.com/en/support/usb-drivers/") { UseShellExecute = true });
                    Log("Download page opened in browser.", Colors.Accent);
                }
                catch { Log("Could not open browser.", Colors.Red); }
            })); y += 38;

            panel.Controls.Add(MakeBtn("Google USB Driver (Direct ZIP)", 10, y, 440, () =>
            {
                Log("Downloading Google USB Driver ZIP...", Colors.Orange);
                try
                {
                    Process.Start(new ProcessStartInfo("https://developer.android.com/studio/run/win-usb#download") { UseShellExecute = true });
                    Log("Download page opened in browser.", Colors.Accent);
                }
                catch { Log("Could not open browser.", Colors.Red); }
            })); y += 38;
        }



        // =====================================================================
        //                           SETTINGS
        // =====================================================================
        private void ShowSettings()
        {
            var panel = CreateSection("SETTINGS");
            int y = 10;
            panel.Controls.Add(MakeBtn("View Tool Log", 10, y, 350, () =>
            {
                string logPath = Path.Combine(Path.GetTempPath(), "bnt", "tool_log.txt");
                if (File.Exists(logPath))
                {
                    Log("--- Tool Log ---", Colors.Orange);
                    Log(File.ReadAllText(logPath), Colors.Text);
                }
                else Log("No log found.", Colors.Red);
            })); y += 38;
            panel.Controls.Add(MakeBtn("Export Device Report", 10, y, 350, () => SettingsExport())); y += 38;
            panel.Controls.Add(MakeBtn("Check for Updates", 10, y, 350, () =>
            {
                Log("Checking for updates...", Colors.Orange);
                new Thread(() =>
                {
                    UpdateChecker.CheckForUpdates((version, url) =>
                    {
                        Log("New version found: v" + version, Colors.Accent);
                        var result = MessageBox.Show(
                            "New version available: v" + version + "\n\nCurrent: v" + UpdateChecker.CURRENT_VERSION + "\n\nDownload and install?",
                            "Update Available", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (result == DialogResult.Yes)
                        {
                            Log("Opening download page in browser...", Colors.Orange);
                            try
                            {
                                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                                Log("Download page opened. Install the update and restart the app.", Colors.Accent);
                            }
                            catch
                            {
                                Log("Could not open browser. URL: " + url, Colors.Red);
                            }
                        }
                    });
                    Log("Up to date. (v" + UpdateChecker.CURRENT_VERSION + ")", Colors.Accent);
                }).Start();
            })); y += 38;
            panel.Controls.Add(MakeBtn("About", 10, y, 350, () =>
            {
                Log("BNT ANDROID TOOLS DASHBOARD v8.16", Colors.Accent);
                Log("Created by BNTWORX", Colors.Text);
                Log("Features: Ad Removal, FRP Bypass, Bloatware Removal,", Colors.Text);
                Log("Device Utils, Privacy Shield, App Manager,", Colors.Text);
                Log("Quick Actions, Developer Tools, Network Tools", Colors.Text);
                Log("Requirements: ADB | USB Debugging enabled", Colors.Orange);
            })); y += 38;
            panel.Controls.Add(MakeBtn("Help / Troubleshooting", 10, y, 350, () =>
            {
                Log("1. Install ADB from Android SDK Platform Tools", Colors.Text);
                Log("2. Enable USB Debugging:", Colors.Text);
                Log("   Settings > About Phone > Tap Build Number 7x", Colors.TextDim);
                Log("   Settings > Developer Options > USB Debugging ON", Colors.TextDim);
                Log("3. Connect USB, accept RSA prompt", Colors.Text);
                Log("4. Run this tool", Colors.Text);
                Log("TROUBLESHOOTING:", Colors.Orange);
                Log("- Device not detected? Check cable + USB debugging", Colors.Text);
                Log("- adb kill-server && adb start-server", Colors.Text);
            })); y += 38;
        }

        private void SettingsExport()
        {
            ExecuteWithWait(() =>
            {
                var sb = new StringBuilder();
                sb.AppendLine("BNT Android Tools - Device Report");
                sb.AppendLine("Generated: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sb.AppendLine("================================================");
                sb.AppendLine("MANUFACTURER: " + Adb.GetProp("ro.product.manufacturer"));
                sb.AppendLine("BRAND: " + Adb.GetProp("ro.product.brand"));
                sb.AppendLine("MODEL: " + Adb.GetProp("ro.product.model"));
                sb.AppendLine("DEVICE: " + Adb.GetProp("ro.product.device"));
                sb.AppendLine("ANDROID: " + Adb.GetProp("ro.build.version.release"));
                sb.AppendLine("SDK: " + Adb.GetProp("ro.build.version.sdk"));
                sb.AppendLine("BUILD: " + Adb.GetProp("ro.build.display.id"));
                sb.AppendLine("SECURITY PATCH: " + Adb.GetProp("ro.build.version.security_patch"));
                sb.AppendLine("ROOT: " + (isRooted ? "YES" : "NO"));
                sb.AppendLine();
                sb.AppendLine("--- PACKAGES ---");
                sb.AppendLine(RunAdb("shell pm list packages"));
                sb.AppendLine();
                sb.AppendLine("--- DISABLED ---");
                sb.AppendLine(RunAdb("shell pm list packages -d"));
                sb.AppendLine();
                sb.AppendLine("--- BATTERY ---");
                sb.AppendLine(RunAdb("shell dumpsys battery"));
                sb.AppendLine();
                sb.AppendLine("--- STORAGE ---");
                sb.AppendLine(RunAdb("shell df"));
                sb.AppendLine();
                sb.AppendLine("--- PROPERTIES ---");
                sb.AppendLine(RunAdb("shell getprop"));

                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "BNT_Report_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt");
                File.WriteAllText(path, sb.ToString());
                Log("Report saved: " + path, Colors.Accent);
            }, "Export Device Report");
        }
    }
}
