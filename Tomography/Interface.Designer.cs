namespace Tomography
{
    /// <summary>
    /// Интерфейс
    /// </summary>
    partial class MainClass
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainClass));
            this.label1 = new System.Windows.Forms.Label();
            this.pictureB = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.gBoxGridInfo = new System.Windows.Forms.GroupBox();
            this.gBoxInfo = new System.Windows.Forms.GroupBox();
            this.groupBSLA = new System.Windows.Forms.GroupBox();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.richTBResult = new System.Windows.Forms.RichTextBox();
            this.openFile = new System.Windows.Forms.OpenFileDialog();
            this.menuSt = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveКакToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFiguresToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.dataToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveDataExcelСДатчиковExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFile = new System.Windows.Forms.SaveFileDialog();
            this.gBoxGrid = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.tBoxSize = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridViewFigures = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonAddFigure = new System.Windows.Forms.Button();
            this.tSButtonCalculate = new System.Windows.Forms.ToolStripButton();
            this.tSButtonGrid = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tSButtonCleaner = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            ((System.ComponentModel.ISupportInitialize)(this.pictureB)).BeginInit();
            this.gBoxGridInfo.SuspendLayout();
            this.gBoxInfo.SuspendLayout();
            this.groupBSLA.SuspendLayout();
            this.menuSt.SuspendLayout();
            this.gBoxGrid.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFigures)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(6, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "Time";
            // 
            // pictureB
            // 
            this.pictureB.BackColor = System.Drawing.SystemColors.Control;
            this.pictureB.Location = new System.Drawing.Point(233, 3);
            this.pictureB.Name = "pictureB";
            this.pictureB.Size = new System.Drawing.Size(728, 545);
            this.pictureB.TabIndex = 2;
            this.pictureB.TabStop = false;
            this.pictureB.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureB_MouseMove);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(6, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 14);
            this.label3.TabIndex = 7;
            this.label3.Text = "Triangle count";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(6, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 14);
            this.label4.TabIndex = 8;
            this.label4.Text = "Point count";
            // 
            // gBoxGridInfo
            // 
            this.gBoxGridInfo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.gBoxGridInfo.Controls.Add(this.label4);
            this.gBoxGridInfo.Controls.Add(this.label3);
            this.gBoxGridInfo.Controls.Add(this.label1);
            this.gBoxGridInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gBoxGridInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gBoxGridInfo.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gBoxGridInfo.Location = new System.Drawing.Point(3, 466);
            this.gBoxGridInfo.Name = "gBoxGridInfo";
            this.gBoxGridInfo.Size = new System.Drawing.Size(224, 82);
            this.gBoxGridInfo.TabIndex = 9;
            this.gBoxGridInfo.TabStop = false;
            this.gBoxGridInfo.Text = "Сетка";
            // 
            // gBoxInfo
            // 
            this.gBoxInfo.Controls.Add(this.groupBSLA);
            this.gBoxInfo.Controls.Add(this.richTBResult);
            this.gBoxInfo.Controls.Add(this.gBoxGridInfo);
            this.gBoxInfo.Dock = System.Windows.Forms.DockStyle.Right;
            this.gBoxInfo.Font = new System.Drawing.Font("Arial Narrow", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gBoxInfo.Location = new System.Drawing.Point(964, 49);
            this.gBoxInfo.Name = "gBoxInfo";
            this.gBoxInfo.Size = new System.Drawing.Size(230, 551);
            this.gBoxInfo.TabIndex = 19;
            this.gBoxInfo.TabStop = false;
            this.gBoxInfo.Text = "Данные";
            this.gBoxInfo.Visible = false;
            // 
            // groupBSLA
            // 
            this.groupBSLA.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.groupBSLA.Controls.Add(this.label25);
            this.groupBSLA.Controls.Add(this.label26);
            this.groupBSLA.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBSLA.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBSLA.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBSLA.Location = new System.Drawing.Point(3, 402);
            this.groupBSLA.Name = "groupBSLA";
            this.groupBSLA.Size = new System.Drawing.Size(224, 64);
            this.groupBSLA.TabIndex = 10;
            this.groupBSLA.TabStop = false;
            this.groupBSLA.Text = "СЛАУ (время)";
            this.groupBSLA.Visible = false;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label25.Location = new System.Drawing.Point(6, 43);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(22, 14);
            this.label25.TabIndex = 8;
            this.label25.Text = "Sol";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label26.Location = new System.Drawing.Point(6, 23);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(19, 14);
            this.label26.TabIndex = 7;
            this.label26.Text = "Fill";
            // 
            // richTBResult
            // 
            this.richTBResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTBResult.BackColor = System.Drawing.SystemColors.HighlightText;
            this.richTBResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTBResult.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.richTBResult.Location = new System.Drawing.Point(6, 20);
            this.richTBResult.Name = "richTBResult";
            this.richTBResult.Size = new System.Drawing.Size(218, 353);
            this.richTBResult.TabIndex = 10;
            this.richTBResult.Text = "";
            this.richTBResult.Visible = false;
            // 
            // openFile
            // 
            this.openFile.FileName = "openFile";
            // 
            // menuSt
            // 
            this.menuSt.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuSt.Location = new System.Drawing.Point(0, 0);
            this.menuSt.Name = "menuSt";
            this.menuSt.Size = new System.Drawing.Size(1194, 24);
            this.menuSt.TabIndex = 20;
            this.menuSt.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveAllToolStripMenuItem,
            this.saveКакToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.fileToolStripMenuItem.Text = "Файл";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.openToolStripMenuItem.Text = "Открыть список фигур";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveAllToolStripMenuItem
            // 
            this.saveAllToolStripMenuItem.Name = "saveAllToolStripMenuItem";
            this.saveAllToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.saveAllToolStripMenuItem.Text = "Сохранить";
            this.saveAllToolStripMenuItem.Click += new System.EventHandler(this.saveAllToolStripMenuItem_Click);
            // 
            // saveКакToolStripMenuItem
            // 
            this.saveКакToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveFiguresToolStripMenuItem1,
            this.dataToolStripMenuItem1,
            this.saveDataExcelСДатчиковExcelToolStripMenuItem});
            this.saveКакToolStripMenuItem.Name = "saveКакToolStripMenuItem";
            this.saveКакToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.saveКакToolStripMenuItem.Text = "Сохранить как";
            // 
            // saveFiguresToolStripMenuItem1
            // 
            this.saveFiguresToolStripMenuItem1.Name = "saveFiguresToolStripMenuItem1";
            this.saveFiguresToolStripMenuItem1.Size = new System.Drawing.Size(216, 22);
            this.saveFiguresToolStripMenuItem1.Text = "Список фигур";
            this.saveFiguresToolStripMenuItem1.Click += new System.EventHandler(this.saveFiguresToolStripMenuItem1_Click);
            // 
            // dataToolStripMenuItem1
            // 
            this.dataToolStripMenuItem1.Name = "dataToolStripMenuItem1";
            this.dataToolStripMenuItem1.Size = new System.Drawing.Size(216, 22);
            this.dataToolStripMenuItem1.Text = "Данные с датчиков (txt)";
            this.dataToolStripMenuItem1.Click += new System.EventHandler(this.saveDataToolStripMenuItem1_Click);
            // 
            // saveDataExcelСДатчиковExcelToolStripMenuItem
            // 
            this.saveDataExcelСДатчиковExcelToolStripMenuItem.Name = "saveDataExcelСДатчиковExcelToolStripMenuItem";
            this.saveDataExcelСДатчиковExcelToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.saveDataExcelСДатчиковExcelToolStripMenuItem.Text = "Данные с датчиков (Excel)";
            this.saveDataExcelСДатчиковExcelToolStripMenuItem.Click += new System.EventHandler(this.saveDataExcelСДатчиковExcelToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.closeToolStripMenuItem.Text = "Выход";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // gBoxGrid
            // 
            this.gBoxGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gBoxGrid.Controls.Add(this.tableLayoutPanel7);
            this.gBoxGrid.Controls.Add(this.groupBox3);
            this.gBoxGrid.Font = new System.Drawing.Font("Arial Narrow", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gBoxGrid.Location = new System.Drawing.Point(3, 454);
            this.gBoxGrid.Name = "gBoxGrid";
            this.gBoxGrid.Size = new System.Drawing.Size(218, 88);
            this.gBoxGrid.TabIndex = 25;
            this.gBoxGrid.TabStop = false;
            this.gBoxGrid.Text = "Параметры сетки";
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 2;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel7.Controls.Add(this.label6, 0, 1);
            this.tableLayoutPanel7.Controls.Add(this.tBoxSize, 1, 0);
            this.tableLayoutPanel7.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(3, 17);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 2;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(212, 68);
            this.tableLayoutPanel7.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Left;
            this.label6.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(3, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 20);
            this.label6.TabIndex = 27;
            this.label6.Text = "label6";
            // 
            // tBoxSize
            // 
            this.tBoxSize.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.tBoxSize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tBoxSize.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tBoxSize.Location = new System.Drawing.Point(160, 13);
            this.tBoxSize.Name = "tBoxSize";
            this.tBoxSize.Size = new System.Drawing.Size(49, 21);
            this.tBoxSize.TabIndex = 26;
            this.tBoxSize.Text = "3";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(3, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 28);
            this.label2.TabIndex = 25;
            this.label2.Text = "Шаг дискретизации:\r\n(>0)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox3
            // 
            this.groupBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox3.Font = new System.Drawing.Font("Arial Narrow", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox3.Location = new System.Drawing.Point(6, 474);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(151, 82);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Результат";
            this.groupBox3.Visible = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label9.Location = new System.Drawing.Point(6, 36);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(60, 14);
            this.label9.TabIndex = 8;
            this.label9.Text = "Point count";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label10.Location = new System.Drawing.Point(6, 56);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(29, 14);
            this.label10.TabIndex = 1;
            this.label10.Text = "Time";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label11.Location = new System.Drawing.Point(6, 16);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(75, 14);
            this.label11.TabIndex = 7;
            this.label11.Text = "Triangle count";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 230F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pictureB, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 49);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 551F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(964, 551);
            this.tableLayoutPanel1.TabIndex = 29;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.gBoxGrid, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(224, 545);
            this.tableLayoutPanel2.TabIndex = 30;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.dataGridViewFigures, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonAddFigure, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(218, 430);
            this.tableLayoutPanel3.TabIndex = 26;
            // 
            // dataGridViewFigures
            // 
            this.dataGridViewFigures.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewFigures.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridViewFigures.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dataGridViewFigures.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewFigures.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewFigures.ColumnHeadersVisible = false;
            this.dataGridViewFigures.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewFigures.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewFigures.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewFigures.GridColor = System.Drawing.SystemColors.ControlLightLight;
            this.dataGridViewFigures.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewFigures.Name = "dataGridViewFigures";
            this.dataGridViewFigures.RowHeadersVisible = false;
            this.dataGridViewFigures.Size = new System.Drawing.Size(212, 389);
            this.dataGridViewFigures.TabIndex = 22;
            this.dataGridViewFigures.Click += new System.EventHandler(this.dataGridViewFigures_Click);
            // 
            // Column1
            // 
            this.Column1.Frozen = true;
            this.Column1.HeaderText = "";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 21;
            // 
            // buttonAddFigure
            // 
            this.buttonAddFigure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAddFigure.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAddFigure.Location = new System.Drawing.Point(3, 398);
            this.buttonAddFigure.Name = "buttonAddFigure";
            this.buttonAddFigure.Size = new System.Drawing.Size(212, 29);
            this.buttonAddFigure.TabIndex = 21;
            this.buttonAddFigure.Text = "Добавить фигуру";
            this.buttonAddFigure.UseVisualStyleBackColor = true;
            this.buttonAddFigure.Click += new System.EventHandler(this.buttonAddFigure_Click);
            // 
            // tSButtonCalculate
            // 
            this.tSButtonCalculate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tSButtonCalculate.Enabled = false;
            this.tSButtonCalculate.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tSButtonCalculate.Image = ((System.Drawing.Image)(resources.GetObject("tSButtonCalculate.Image")));
            this.tSButtonCalculate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tSButtonCalculate.Name = "tSButtonCalculate";
            this.tSButtonCalculate.Size = new System.Drawing.Size(54, 22);
            this.tSButtonCalculate.Text = "Расчет";
            this.tSButtonCalculate.Click += new System.EventHandler(this.tSButtonCalculate_Click);
            // 
            // tSButtonGrid
            // 
            this.tSButtonGrid.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tSButtonGrid.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tSButtonGrid.Image = ((System.Drawing.Image)(resources.GetObject("tSButtonGrid.Image")));
            this.tSButtonGrid.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tSButtonGrid.Name = "tSButtonGrid";
            this.tSButtonGrid.Size = new System.Drawing.Size(115, 22);
            this.tSButtonGrid.Text = "Построить сетку";
            this.tSButtonGrid.Click += new System.EventHandler(this.tSButtonGrid_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tSButtonCleaner
            // 
            this.tSButtonCleaner.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tSButtonCleaner.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tSButtonCleaner.Image = ((System.Drawing.Image)(resources.GetObject("tSButtonCleaner.Image")));
            this.tSButtonCleaner.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tSButtonCleaner.Name = "tSButtonCleaner";
            this.tSButtonCleaner.Size = new System.Drawing.Size(63, 22);
            this.tSButtonCleaner.Text = "Очистка";
            this.tSButtonCleaner.Click += new System.EventHandler(this.tSButtonCleaner_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tSButtonCalculate,
            this.tSButtonGrid,
            this.toolStripSeparator1,
            this.tSButtonCleaner});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1194, 25);
            this.toolStrip1.TabIndex = 21;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // MainClass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1194, 600);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.gBoxInfo);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuSt);
            this.MainMenuStrip = this.menuSt;
            this.Name = "MainClass";
            this.Text = "ver04.22";
            ((System.ComponentModel.ISupportInitialize)(this.pictureB)).EndInit();
            this.gBoxGridInfo.ResumeLayout(false);
            this.gBoxGridInfo.PerformLayout();
            this.gBoxInfo.ResumeLayout(false);
            this.groupBSLA.ResumeLayout(false);
            this.groupBSLA.PerformLayout();
            this.menuSt.ResumeLayout(false);
            this.menuSt.PerformLayout();
            this.gBoxGrid.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel7.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFigures)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox gBoxGridInfo;
        private System.Windows.Forms.GroupBox gBoxInfo;
        private System.Windows.Forms.OpenFileDialog openFile;
        private System.Windows.Forms.MenuStrip menuSt;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveКакToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFile;
        private System.Windows.Forms.ToolStripMenuItem saveFiguresToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem dataToolStripMenuItem1;
        private System.Windows.Forms.TextBox tBoxSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox gBoxGrid;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox groupBSLA;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.RichTextBox richTBResult;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ToolStripButton tSButtonCalculate;
        private System.Windows.Forms.ToolStripButton tSButtonGrid;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tSButtonCleaner;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button buttonAddFigure;
        private System.Windows.Forms.ToolStripMenuItem saveDataExcelСДатчиковExcelToolStripMenuItem;
        private System.Windows.Forms.DataGridView dataGridViewFigures;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
    }
}

