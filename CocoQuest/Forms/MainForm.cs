using CocoQuest.Models;
using CocoQuest.Services;

namespace CocoQuest
{
    public partial class MainForm : Form
    {

        private Panel topPanel;
        private Panel habitsPanel;
        private Panel waterPanel;
        private Panel workoutPanel;
        private Panel bottomPanel;

        private Color bg = Color.FromArgb(58, 31, 105);
        private Color panel = Color.FromArgb(108, 69, 176);
        private Color accent = Color.FromArgb(0, 212, 170);
        private Color text = Color.White;

        //private int xp = 120;
        private int xpMax = 500;
        //private double water = 1.2;
        private double waterMax = 2.0;
        //private int coconuts = 120;
        private int currentLevel = 1;

        private Panel xpFill;
        private Label levelLabel;
        private Label xpLabel;
        private Label waterLabel;
        private ProgressBar waterBar;
        private Label coconutsLabel;

        private UserState state;
        private SaveService saveService = new SaveService();

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SaveGame();
            base.OnFormClosing(e);
        }

        public MainForm()
        {
            InitializeComponent();
            state = saveService.Load();
            BuildUI();
        }

        private void SaveGame()
        {
            saveService.Save(state);
        }

        private void ClampValues()
        {
            if (state.XP < 0) state.XP = 0;
            if (state.Water < 0) state.Water = 0;
            if (state.Cocoonuts < 0) state.Cocoonuts = 0;
            SaveGame();
        }

        private Panel CreatePanel(int x, int y, int w, int h)
        {
            return new Panel
            {
                Size = new Size(w, h),
                Location = new Point(x, y),
                BackColor = panel
            };
        }

        private int CalculateXpWidth(int xp, int xpMax, int maxWidth)
        {
            return (int)((double)xp / xpMax * maxWidth);
        }

        private int GetLevel()
        {
            return (state.XP / 500) + 1;
        }

        private void OnLevelUp()
        {
            MessageBox.Show($"LEVEL UP! 🎉 Теперь уровень {currentLevel}");

            state.Cocoonuts += 50;
            state.TotalCoconutsEarned += 10;
            SaveGame();
        }

        private void UpdateUI()
        {
            int level = GetLevel();

            levelLabel.Text = $"Level {state.CurrentLevel}";
            xpFill.Size = new Size(CalculateXpWidth(state.XP % 500, 500, 400), 20);

            waterLabel.Text = $"💧 Water: {state.Water} / {state.WaterMax} L";
            waterBar.Value = Math.Min(100, (int)(state.Water / state.WaterMax * 100));

            coconutsLabel.Text = $"🥥 {state.Cocoonuts}";

            int newLevel = GetLevel();

            if (newLevel != state.CurrentLevel)
            {
                state.CurrentLevel = newLevel;
                OnLevelUp();
            }

            ClampValues();
            SaveGame();
        }

        private void BuildUI() 
        {
            this.SuspendLayout();
            this.Text = "CocoQuest";
            this.Size = new Size(900, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = bg;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            CreateTopPanel();
            CreateHabitsPanel();
            CreateWaterPanel();
            CreateWorkoutPanel();
            CreateBottomPanel();
            this.ResumeLayout(false);
        }

        private void CreateTopPanel() 
        {
            topPanel = new Panel
            {
                Size = new Size(860, 120),
                Location = new Point(20, 10),
                BackColor = Color.FromArgb(37, 37, 37)
            };

            Label title = new Label
            {
                Text = "CocoQuest",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };

            Label level = new Label
            {
                Text = "Level 1",
                ForeColor = Color.White,
                Location = new Point(10, 40),
                AutoSize = true
            };
            levelLabel = level;
            level.Text = $"Level {GetLevel()}";

            Panel xpBg = new Panel
            {
                Size = new Size(400, 20),
                Location = new Point(10, 70),
                BackColor = Color.FromArgb(60, 60, 60)
            };

            xpFill = new Panel
            {
                Size = new Size(CalculateXpWidth(state.XP, xpMax, 400), 20),
                Location = new Point(0, 0),
                BackColor = accent
            };

            int xpInLevel = state.XP % 500;
            xpFill.Size = new Size(CalculateXpWidth(xpInLevel, 500, 400), 20);

            xpBg.Controls.Add(xpFill);
            topPanel.Controls.Add(xpBg);

            topPanel.Controls.Add(title);
            topPanel.Controls.Add(level);

            this.Controls.Add(topPanel);
        }

        private void CreateHabitsPanel() 
        {
            habitsPanel = new Panel
            {
                Size = new Size(420, 250),
                Location = new Point(20, 140),
                BackColor = Color.FromArgb(37, 37, 37)
            };

            string[] habits = { "Белок", "Сон", "Прогулка", "Тренировка" };

            int y = 20;

            foreach (var h in habits)
            {
                CheckBox cb = new CheckBox
                {
                    Text = h,
                    ForeColor = Color.White,
                    Location = new Point(10, y),
                    AutoSize = true
                };

                cb.CheckedChanged += (s, e) =>
                {
                    if (cb.Checked)
                    {
                        state.XP += 20;
                        state.Cocoonuts += 2;
                        state.TotalCoconutsEarned += 10;
                    }
                    else
                    {
                        state.XP -= 20;
                        state.Cocoonuts -= 2;
                        state.TotalCoconutsEarned += 10;
                    }

                    UpdateUI();
                    SaveGame();
                };

                habitsPanel.Controls.Add(cb);
                y += 40;
            }

            this.Controls.Add(habitsPanel);
        }

        private void CreateWaterPanel() 
        {
            waterPanel = new Panel
            {
                Size = new Size(420, 120),
                Location = new Point(460, 140),
                BackColor = Color.FromArgb(37, 37, 37)
            };

            waterLabel = new Label
            {
                Text = $"💧 Water: {state.Water} / {waterMax} L",
                ForeColor = text,
                Location = new Point(10, 10),
                AutoSize = true
            };

            waterBar = new ProgressBar
            {
                Value = (int)(state.Water / waterMax * 100),
                Size = new Size(380, 20),
                Location = new Point(10, 50)
            };

            Button addWater = new Button
            {
                Text = "+0.25L",
                Location = new Point(300, 80),
                Size = new Size(100, 25)
            };

            addWater.Click += (s, e) =>
            {
                state.Water += 0.25;

                if (state.Water > waterMax)
                    state.Water = waterMax;

                state.XP += 10;
                state.Cocoonuts += 1;

                UpdateUI();
            };

            waterPanel.Controls.Add(addWater);

            waterPanel.Controls.Add(waterLabel);
            waterPanel.Controls.Add(waterBar);

            this.Controls.Add(waterPanel);
        }

        private void CreateWorkoutPanel() 
        {
            workoutPanel = new Panel
            {
                Size = new Size(860, 100),
                Location = new Point(20, 410),
                BackColor = Color.FromArgb(37, 37, 37)
            };

            Label status = new Label
            {
                Text = "🏋️ Тренировка: НЕ СДЕЛАНО",
                ForeColor = Color.White,
                Location = new Point(10, 10),
                AutoSize = true
            };

            Button done = new Button
            {
                Text = "Сделано!",
                Location = new Point(10, 40)
            };

            done.Click += (s, e) =>
            {
                state.XP += 150;
                state.Cocoonuts += 10;

                status.Text = "🏋️ Тренировка: СДЕЛАНО";

                UpdateUI();
            };

            workoutPanel.Controls.Add(done);

            workoutPanel.Controls.Add(status);
            this.Controls.Add(workoutPanel);
        }

        private void CreateBottomPanel() 
        {
            bottomPanel = new Panel
            {
                Size = new Size(860, 80),
                Location = new Point(20, 520),
                BackColor = Color.FromArgb(37, 37, 37)
            };

            coconutsLabel = new Label
            {
                Text = $"🥥 {state.Cocoonuts}",
                ForeColor = text,
                Location = new Point(10, 20),
                AutoSize = true
            };

            Button btn1 = new Button { Text = "Прогресс", Location = new Point(200, 20) };
            Button btn2 = new Button { Text = "Тренировки", Location = new Point(320, 20) };
            Button btn3 = new Button { Text = "Питание", Location = new Point(460, 20) };

            bottomPanel.Controls.Add(coconutsLabel);
            bottomPanel.Controls.Add(btn1);
            bottomPanel.Controls.Add(btn2);
            bottomPanel.Controls.Add(btn3);

            this.Controls.Add(bottomPanel);
        }


    }
}
