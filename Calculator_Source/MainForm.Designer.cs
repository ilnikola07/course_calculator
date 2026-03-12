namespace course_calculator
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtExpression = new TextBox();
            txtResult = new TextBox();
            pnlKeypad = new Panel();
            btnCom = new Button();
            btnBackspace = new Button();
            btnCloseBracket = new Button();
            btnOpenBracket = new Button();
            btnDiv = new Button();
            btnMul = new Button();
            btnSub = new Button();
            btnAdd = new Button();
            labelBase = new Label();
            btnCalculate = new Button();
            btnClear = new Button();
            btn3 = new Button();
            btn6 = new Button();
            btn9 = new Button();
            btn8 = new Button();
            btn7 = new Button();
            btn5 = new Button();
            btn4 = new Button();
            btn0 = new Button();
            btn1 = new Button();
            btn2 = new Button();
            btnPow = new Button();
            panel1 = new Panel();
            btnLn = new Button();
            btnLog = new Button();
            btnAbs = new Button();
            btnSqrt = new Button();
            btnTan = new Button();
            btnCos = new Button();
            labelFunc = new Label();
            btnSin = new Button();
            panel3 = new Panel();
            labelPer = new Label();
            dataGridViewPer = new DataGridView();
            colName = new DataGridViewTextBoxColumn();
            colValue = new DataGridViewTextBoxColumn();
            btnRef = new Button();
            btnHistory = new Button();
            labelBtn = new Label();
            pnlKeypad.SuspendLayout();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewPer).BeginInit();
            SuspendLayout();
            // 
            // txtExpression
            // 
            txtExpression.Location = new Point(13, 12);
            txtExpression.Name = "txtExpression";
            txtExpression.Size = new Size(420, 23);
            txtExpression.TabIndex = 0;
            // 
            // txtResult
            // 
            txtResult.Location = new Point(454, 12);
            txtResult.Name = "txtResult";
            txtResult.ReadOnly = true;
            txtResult.Size = new Size(197, 23);
            txtResult.TabIndex = 1;
            // 
            // pnlKeypad
            // 
            pnlKeypad.BackColor = SystemColors.ButtonShadow;
            pnlKeypad.Controls.Add(btnCom);
            pnlKeypad.Controls.Add(btnBackspace);
            pnlKeypad.Controls.Add(btnCloseBracket);
            pnlKeypad.Controls.Add(btnOpenBracket);
            pnlKeypad.Controls.Add(btnDiv);
            pnlKeypad.Controls.Add(btnMul);
            pnlKeypad.Controls.Add(btnSub);
            pnlKeypad.Controls.Add(btnAdd);
            pnlKeypad.Controls.Add(labelBase);
            pnlKeypad.Controls.Add(btnCalculate);
            pnlKeypad.Controls.Add(btnClear);
            pnlKeypad.Controls.Add(btn3);
            pnlKeypad.Controls.Add(btn6);
            pnlKeypad.Controls.Add(btn9);
            pnlKeypad.Controls.Add(btn8);
            pnlKeypad.Controls.Add(btn7);
            pnlKeypad.Controls.Add(btn5);
            pnlKeypad.Controls.Add(btn4);
            pnlKeypad.Controls.Add(btn0);
            pnlKeypad.Controls.Add(btn1);
            pnlKeypad.Controls.Add(btn2);
            pnlKeypad.Location = new Point(13, 51);
            pnlKeypad.Name = "pnlKeypad";
            pnlKeypad.Size = new Size(239, 207);
            pnlKeypad.TabIndex = 2;
            // 
            // btnCom
            // 
            btnCom.Location = new Point(13, 160);
            btnCom.Name = "btnCom";
            btnCom.Size = new Size(38, 32);
            btnCom.TabIndex = 20;
            btnCom.Text = ",";
            btnCom.UseVisualStyleBackColor = true;
            // 
            // btnBackspace
            // 
            btnBackspace.Location = new Point(145, 160);
            btnBackspace.Name = "btnBackspace";
            btnBackspace.Size = new Size(38, 32);
            btnBackspace.TabIndex = 19;
            btnBackspace.Text = "С";
            btnBackspace.UseVisualStyleBackColor = true;
            // 
            // btnCloseBracket
            // 
            btnCloseBracket.Location = new Point(189, 122);
            btnCloseBracket.Name = "btnCloseBracket";
            btnCloseBracket.Size = new Size(38, 32);
            btnCloseBracket.TabIndex = 17;
            btnCloseBracket.Text = ")";
            btnCloseBracket.UseVisualStyleBackColor = true;
            // 
            // btnOpenBracket
            // 
            btnOpenBracket.Location = new Point(145, 122);
            btnOpenBracket.Name = "btnOpenBracket";
            btnOpenBracket.Size = new Size(38, 32);
            btnOpenBracket.TabIndex = 16;
            btnOpenBracket.Text = "(";
            btnOpenBracket.UseVisualStyleBackColor = true;
            // 
            // btnDiv
            // 
            btnDiv.Location = new Point(189, 84);
            btnDiv.Name = "btnDiv";
            btnDiv.Size = new Size(38, 32);
            btnDiv.TabIndex = 15;
            btnDiv.Text = "/";
            btnDiv.UseVisualStyleBackColor = true;
            // 
            // btnMul
            // 
            btnMul.Location = new Point(189, 46);
            btnMul.Name = "btnMul";
            btnMul.Size = new Size(38, 32);
            btnMul.TabIndex = 14;
            btnMul.Text = "*";
            btnMul.UseVisualStyleBackColor = true;
            // 
            // btnSub
            // 
            btnSub.Location = new Point(145, 84);
            btnSub.Name = "btnSub";
            btnSub.Size = new Size(38, 32);
            btnSub.TabIndex = 13;
            btnSub.Text = "-";
            btnSub.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(145, 46);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(38, 32);
            btnAdd.TabIndex = 12;
            btnAdd.Text = "+";
            btnAdd.UseVisualStyleBackColor = true;
            // 
            // labelBase
            // 
            labelBase.AutoSize = true;
            labelBase.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 204);
            labelBase.Location = new Point(13, 9);
            labelBase.Name = "labelBase";
            labelBase.Size = new Size(116, 21);
            labelBase.TabIndex = 11;
            labelBase.Text = "Базовый счёт";
            // 
            // btnCalculate
            // 
            btnCalculate.Location = new Point(101, 160);
            btnCalculate.Name = "btnCalculate";
            btnCalculate.Size = new Size(38, 32);
            btnCalculate.TabIndex = 10;
            btnCalculate.Text = "=";
            btnCalculate.UseVisualStyleBackColor = true;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(189, 160);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(38, 32);
            btnClear.TabIndex = 9;
            btnClear.Text = "СЕ";
            btnClear.UseVisualStyleBackColor = true;
            // 
            // btn3
            // 
            btn3.Location = new Point(101, 46);
            btn3.Name = "btn3";
            btn3.Size = new Size(38, 32);
            btn3.TabIndex = 8;
            btn3.Text = "3";
            btn3.UseVisualStyleBackColor = true;
            // 
            // btn6
            // 
            btn6.Location = new Point(101, 84);
            btn6.Name = "btn6";
            btn6.Size = new Size(38, 32);
            btn6.TabIndex = 7;
            btn6.Text = "6";
            btn6.UseVisualStyleBackColor = true;
            // 
            // btn9
            // 
            btn9.Location = new Point(101, 122);
            btn9.Name = "btn9";
            btn9.Size = new Size(38, 32);
            btn9.TabIndex = 3;
            btn9.Text = "9";
            btn9.UseVisualStyleBackColor = true;
            // 
            // btn8
            // 
            btn8.Location = new Point(57, 122);
            btn8.Name = "btn8";
            btn8.Size = new Size(38, 32);
            btn8.TabIndex = 6;
            btn8.Text = "8";
            btn8.UseVisualStyleBackColor = true;
            // 
            // btn7
            // 
            btn7.Location = new Point(13, 122);
            btn7.Name = "btn7";
            btn7.Size = new Size(38, 32);
            btn7.TabIndex = 5;
            btn7.Text = "7";
            btn7.UseVisualStyleBackColor = true;
            // 
            // btn5
            // 
            btn5.Location = new Point(57, 84);
            btn5.Name = "btn5";
            btn5.Size = new Size(38, 32);
            btn5.TabIndex = 4;
            btn5.Text = "5";
            btn5.UseVisualStyleBackColor = true;
            // 
            // btn4
            // 
            btn4.Location = new Point(13, 84);
            btn4.Name = "btn4";
            btn4.Size = new Size(38, 32);
            btn4.TabIndex = 3;
            btn4.Text = "4";
            btn4.UseVisualStyleBackColor = true;
            // 
            // btn0
            // 
            btn0.Location = new Point(57, 160);
            btn0.Name = "btn0";
            btn0.Size = new Size(38, 32);
            btn0.TabIndex = 2;
            btn0.Text = "0";
            btn0.UseVisualStyleBackColor = true;
            // 
            // btn1
            // 
            btn1.Location = new Point(13, 46);
            btn1.Name = "btn1";
            btn1.Size = new Size(38, 32);
            btn1.TabIndex = 1;
            btn1.Text = "1";
            btn1.UseVisualStyleBackColor = true;
            // 
            // btn2
            // 
            btn2.Location = new Point(57, 46);
            btn2.Name = "btn2";
            btn2.Size = new Size(38, 32);
            btn2.TabIndex = 0;
            btn2.Text = "2";
            btn2.UseVisualStyleBackColor = true;
            // 
            // btnPow
            // 
            btnPow.Location = new Point(13, 46);
            btnPow.Name = "btnPow";
            btnPow.Size = new Size(59, 32);
            btnPow.TabIndex = 18;
            btnPow.Text = "pow";
            btnPow.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ButtonShadow;
            panel1.Controls.Add(btnLn);
            panel1.Controls.Add(btnPow);
            panel1.Controls.Add(btnLog);
            panel1.Controls.Add(btnAbs);
            panel1.Controls.Add(btnSqrt);
            panel1.Controls.Add(btnTan);
            panel1.Controls.Add(btnCos);
            panel1.Controls.Add(labelFunc);
            panel1.Controls.Add(btnSin);
            panel1.Location = new Point(280, 51);
            panel1.Name = "panel1";
            panel1.Size = new Size(153, 207);
            panel1.TabIndex = 3;
            // 
            // btnLn
            // 
            btnLn.Location = new Point(13, 160);
            btnLn.Name = "btnLn";
            btnLn.Size = new Size(59, 32);
            btnLn.TabIndex = 17;
            btnLn.Text = "ln";
            btnLn.UseVisualStyleBackColor = true;
            // 
            // btnLog
            // 
            btnLog.Location = new Point(13, 122);
            btnLog.Name = "btnLog";
            btnLog.Size = new Size(59, 32);
            btnLog.TabIndex = 16;
            btnLog.Text = "log";
            btnLog.UseVisualStyleBackColor = true;
            // 
            // btnAbs
            // 
            btnAbs.Location = new Point(78, 46);
            btnAbs.Name = "btnAbs";
            btnAbs.Size = new Size(59, 32);
            btnAbs.TabIndex = 15;
            btnAbs.Text = "abs";
            btnAbs.UseVisualStyleBackColor = true;
            // 
            // btnSqrt
            // 
            btnSqrt.Location = new Point(13, 84);
            btnSqrt.Name = "btnSqrt";
            btnSqrt.Size = new Size(59, 32);
            btnSqrt.TabIndex = 14;
            btnSqrt.Text = "sqrt";
            btnSqrt.UseVisualStyleBackColor = true;
            // 
            // btnTan
            // 
            btnTan.Location = new Point(78, 160);
            btnTan.Name = "btnTan";
            btnTan.Size = new Size(59, 32);
            btnTan.TabIndex = 13;
            btnTan.Text = "tan";
            btnTan.UseVisualStyleBackColor = true;
            // 
            // btnCos
            // 
            btnCos.Location = new Point(78, 122);
            btnCos.Name = "btnCos";
            btnCos.Size = new Size(59, 32);
            btnCos.TabIndex = 12;
            btnCos.Text = "cos";
            btnCos.UseVisualStyleBackColor = true;
            // 
            // labelFunc
            // 
            labelFunc.AutoSize = true;
            labelFunc.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 204);
            labelFunc.Location = new Point(13, 9);
            labelFunc.Name = "labelFunc";
            labelFunc.Size = new Size(76, 21);
            labelFunc.TabIndex = 11;
            labelFunc.Text = "Функции";
            // 
            // btnSin
            // 
            btnSin.Location = new Point(78, 84);
            btnSin.Name = "btnSin";
            btnSin.Size = new Size(59, 32);
            btnSin.TabIndex = 1;
            btnSin.Text = "sin";
            btnSin.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            panel3.BackColor = SystemColors.ButtonShadow;
            panel3.Controls.Add(labelPer);
            panel3.Controls.Add(dataGridViewPer);
            panel3.Controls.Add(btnRef);
            panel3.Controls.Add(btnHistory);
            panel3.Controls.Add(labelBtn);
            panel3.Location = new Point(454, 51);
            panel3.Name = "panel3";
            panel3.Size = new Size(197, 207);
            panel3.TabIndex = 5;
            // 
            // labelPer
            // 
            labelPer.AutoSize = true;
            labelPer.Location = new Point(13, 46);
            labelPer.Name = "labelPer";
            labelPer.Size = new Size(106, 15);
            labelPer.TabIndex = 15;
            labelPer.Text = "Ввод переменных";
            // 
            // dataGridViewPer
            // 
            dataGridViewPer.BorderStyle = BorderStyle.None;
            dataGridViewPer.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewPer.Columns.AddRange(new DataGridViewColumn[] { colName, colValue });
            dataGridViewPer.Location = new Point(13, 68);
            dataGridViewPer.Name = "dataGridViewPer";
            dataGridViewPer.RowHeadersVisible = false;
            dataGridViewPer.Size = new Size(172, 75);
            dataGridViewPer.TabIndex = 14;
            // 
            // colName
            // 
            colName.HeaderText = "Имя";
            colName.Name = "colName";
            colName.Width = 50;
            // 
            // colValue
            // 
            colValue.HeaderText = "Значение";
            colValue.Name = "colValue";
            // 
            // btnRef
            // 
            btnRef.Location = new Point(13, 170);
            btnRef.Name = "btnRef";
            btnRef.Size = new Size(172, 22);
            btnRef.TabIndex = 13;
            btnRef.Text = "О приложении\r\n";
            btnRef.UseVisualStyleBackColor = true;
            btnRef.Click += btnRef_Click;
            // 
            // btnHistory
            // 
            btnHistory.Location = new Point(13, 142);
            btnHistory.Name = "btnHistory";
            btnHistory.Size = new Size(172, 22);
            btnHistory.TabIndex = 12;
            btnHistory.Text = "Посмотреть историю";
            btnHistory.UseVisualStyleBackColor = true;
            btnHistory.Click += btnHistory_Click;
            // 
            // labelBtn
            // 
            labelBtn.AutoSize = true;
            labelBtn.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 204);
            labelBtn.Location = new Point(13, 10);
            labelBtn.Name = "labelBtn";
            labelBtn.Size = new Size(172, 21);
            labelBtn.TabIndex = 11;
            labelBtn.Text = "Другие возможности";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(678, 280);
            Controls.Add(panel3);
            Controls.Add(panel1);
            Controls.Add(pnlKeypad);
            Controls.Add(txtResult);
            Controls.Add(txtExpression);
            Name = "MainForm";
            Text = "Ulitka Soft - Курсовая \"Калькулятор\"";
            pnlKeypad.ResumeLayout(false);
            pnlKeypad.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewPer).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtExpression;
        private TextBox txtResult;
        private Panel pnlKeypad;
        private Button btn0;
        private Button btn1;
        private Button btn2;
        private Button btn9;
        private Button btn8;
        private Button btn7;
        private Button btn5;
        private Button btn4;
        private Button btn3;
        private Button btn6;
        private Button btnCalculate;
        private Button btnClear;
        private Button btnMul;
        private Button btnSub;
        private Button btnAdd;
        private Label labelBase;
        private Button btnPow;
        private Button btnCloseBracket;
        private Button btnOpenBracket;
        private Button btnDiv;
        private Panel panel1;
        private Label labelFunc;
        private Button btnSin;
        private Button btnBackspace;
        private Button btnAbs;
        private Button btnSqrt;
        private Button btnTan;
        private Button btnCos;
        private Button btnLn;
        private Button btnLog;
        private Button btnCom;
        private Panel panel3;
        private Label labelBtn;
        private Button btnRef;
        private Button btnHistory;
        private DataGridView dataGridViewPer;
        private Label labelPer;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colValue;
    }
}
