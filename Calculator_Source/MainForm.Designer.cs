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
            panelKeypad = new Panel();
            btnCom = new Button();
            btnPow = new Button();
            btnCloseBracket = new Button();
            btnSqrt = new Button();
            btnOpenBracket = new Button();
            btnDiv = new Button();
            btnMul = new Button();
            btnSub = new Button();
            btnAdd = new Button();
            labelBase = new Label();
            btnEquals = new Button();
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
            btnBackspace = new Button();
            btnClear = new Button();
            panelFun = new Panel();
            btnLn = new Button();
            btnLog = new Button();
            btnAbs = new Button();
            btnTan = new Button();
            btnCos = new Button();
            labelFunc = new Label();
            btnSin = new Button();
            panelElse = new Panel();
            btnExit = new Button();
            dataGridViewPer = new DataGridView();
            colName = new DataGridViewTextBoxColumn();
            colValue = new DataGridViewTextBoxColumn();
            btnRef = new Button();
            btnHistory = new Button();
            labelBtn = new Label();
            panelKeypad.SuspendLayout();
            panelFun.SuspendLayout();
            panelElse.SuspendLayout();
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
            // panelKeypad
            // 
            panelKeypad.BackColor = SystemColors.ButtonShadow;
            panelKeypad.Controls.Add(btnCom);
            panelKeypad.Controls.Add(btnPow);
            panelKeypad.Controls.Add(btnCloseBracket);
            panelKeypad.Controls.Add(btnSqrt);
            panelKeypad.Controls.Add(btnOpenBracket);
            panelKeypad.Controls.Add(btnDiv);
            panelKeypad.Controls.Add(btnMul);
            panelKeypad.Controls.Add(btnSub);
            panelKeypad.Controls.Add(btnAdd);
            panelKeypad.Controls.Add(labelBase);
            panelKeypad.Controls.Add(btnEquals);
            panelKeypad.Controls.Add(btn3);
            panelKeypad.Controls.Add(btn6);
            panelKeypad.Controls.Add(btn9);
            panelKeypad.Controls.Add(btn8);
            panelKeypad.Controls.Add(btn7);
            panelKeypad.Controls.Add(btn5);
            panelKeypad.Controls.Add(btn4);
            panelKeypad.Controls.Add(btn0);
            panelKeypad.Controls.Add(btn1);
            panelKeypad.Controls.Add(btn2);
            panelKeypad.Location = new Point(13, 51);
            panelKeypad.Name = "panelKeypad";
            panelKeypad.Size = new Size(239, 207);
            panelKeypad.TabIndex = 2;
            // 
            // btnCom
            // 
            btnCom.Location = new Point(13, 150);
            btnCom.Name = "btnCom";
            btnCom.Size = new Size(38, 32);
            btnCom.TabIndex = 20;
            btnCom.Text = ".";
            btnCom.UseVisualStyleBackColor = true;
            btnCom.Click += BtnDigit_Click;
            // 
            // btnPow
            // 
            btnPow.Location = new Point(189, 112);
            btnPow.Name = "btnPow";
            btnPow.Size = new Size(38, 32);
            btnPow.TabIndex = 18;
            btnPow.Text = "^";
            btnPow.UseVisualStyleBackColor = true;
            btnPow.Click += FunctionButton;
            // 
            // btnCloseBracket
            // 
            btnCloseBracket.Location = new Point(189, 150);
            btnCloseBracket.Name = "btnCloseBracket";
            btnCloseBracket.Size = new Size(38, 32);
            btnCloseBracket.TabIndex = 17;
            btnCloseBracket.Text = ")";
            btnCloseBracket.UseVisualStyleBackColor = true;
            btnCloseBracket.Click += BtnParenthesis_Click;
            // 
            // btnSqrt
            // 
            btnSqrt.Location = new Point(145, 112);
            btnSqrt.Name = "btnSqrt";
            btnSqrt.Size = new Size(38, 32);
            btnSqrt.TabIndex = 14;
            btnSqrt.Text = "√";
            btnSqrt.UseVisualStyleBackColor = true;
            btnSqrt.Click += FunctionButton;
            // 
            // btnOpenBracket
            // 
            btnOpenBracket.Location = new Point(145, 150);
            btnOpenBracket.Name = "btnOpenBracket";
            btnOpenBracket.Size = new Size(38, 32);
            btnOpenBracket.TabIndex = 16;
            btnOpenBracket.Text = "(";
            btnOpenBracket.UseVisualStyleBackColor = true;
            btnOpenBracket.Click += BtnParenthesis_Click;
            // 
            // btnDiv
            // 
            btnDiv.Location = new Point(189, 74);
            btnDiv.Name = "btnDiv";
            btnDiv.Size = new Size(38, 32);
            btnDiv.TabIndex = 15;
            btnDiv.Text = "/";
            btnDiv.UseVisualStyleBackColor = true;
            btnDiv.Click += BtnOperator_Click;
            // 
            // btnMul
            // 
            btnMul.Location = new Point(189, 36);
            btnMul.Name = "btnMul";
            btnMul.Size = new Size(38, 32);
            btnMul.TabIndex = 14;
            btnMul.Text = "*";
            btnMul.UseVisualStyleBackColor = true;
            btnMul.Click += BtnOperator_Click;
            // 
            // btnSub
            // 
            btnSub.Location = new Point(145, 74);
            btnSub.Name = "btnSub";
            btnSub.Size = new Size(38, 32);
            btnSub.TabIndex = 13;
            btnSub.Text = "-";
            btnSub.UseVisualStyleBackColor = true;
            btnSub.Click += BtnOperator_Click;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(145, 36);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(38, 32);
            btnAdd.TabIndex = 12;
            btnAdd.Text = "+";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += BtnOperator_Click;
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
            // btnEquals
            // 
            btnEquals.Location = new Point(101, 150);
            btnEquals.Name = "btnEquals";
            btnEquals.Size = new Size(38, 32);
            btnEquals.TabIndex = 10;
            btnEquals.Text = "=";
            btnEquals.UseVisualStyleBackColor = true;
            btnEquals.Click += btnEquals_Click;
            // 
            // btn3
            // 
            btn3.Location = new Point(101, 36);
            btn3.Name = "btn3";
            btn3.Size = new Size(38, 32);
            btn3.TabIndex = 8;
            btn3.Text = "3";
            btn3.UseVisualStyleBackColor = true;
            btn3.Click += BtnDigit_Click;
            // 
            // btn6
            // 
            btn6.Location = new Point(101, 74);
            btn6.Name = "btn6";
            btn6.Size = new Size(38, 32);
            btn6.TabIndex = 7;
            btn6.Text = "6";
            btn6.UseVisualStyleBackColor = true;
            btn6.Click += BtnDigit_Click;
            // 
            // btn9
            // 
            btn9.Location = new Point(101, 112);
            btn9.Name = "btn9";
            btn9.Size = new Size(38, 32);
            btn9.TabIndex = 3;
            btn9.Text = "9";
            btn9.UseVisualStyleBackColor = true;
            btn9.Click += BtnDigit_Click;
            // 
            // btn8
            // 
            btn8.Location = new Point(57, 112);
            btn8.Name = "btn8";
            btn8.Size = new Size(38, 32);
            btn8.TabIndex = 6;
            btn8.Text = "8";
            btn8.UseVisualStyleBackColor = true;
            btn8.Click += BtnDigit_Click;
            // 
            // btn7
            // 
            btn7.Location = new Point(13, 112);
            btn7.Name = "btn7";
            btn7.Size = new Size(38, 32);
            btn7.TabIndex = 5;
            btn7.Text = "7";
            btn7.UseVisualStyleBackColor = true;
            btn7.Click += BtnDigit_Click;
            // 
            // btn5
            // 
            btn5.Location = new Point(57, 74);
            btn5.Name = "btn5";
            btn5.Size = new Size(38, 32);
            btn5.TabIndex = 4;
            btn5.Text = "5";
            btn5.UseVisualStyleBackColor = true;
            btn5.Click += BtnDigit_Click;
            // 
            // btn4
            // 
            btn4.Location = new Point(13, 74);
            btn4.Name = "btn4";
            btn4.Size = new Size(38, 32);
            btn4.TabIndex = 3;
            btn4.Text = "4";
            btn4.UseVisualStyleBackColor = true;
            btn4.Click += BtnDigit_Click;
            // 
            // btn0
            // 
            btn0.Location = new Point(57, 150);
            btn0.Name = "btn0";
            btn0.Size = new Size(38, 32);
            btn0.TabIndex = 2;
            btn0.Text = "0";
            btn0.UseVisualStyleBackColor = true;
            btn0.Click += BtnDigit_Click;
            // 
            // btn1
            // 
            btn1.Location = new Point(13, 36);
            btn1.Name = "btn1";
            btn1.Size = new Size(38, 32);
            btn1.TabIndex = 1;
            btn1.Text = "1";
            btn1.UseVisualStyleBackColor = true;
            btn1.Click += BtnDigit_Click;
            // 
            // btn2
            // 
            btn2.Location = new Point(57, 36);
            btn2.Name = "btn2";
            btn2.Size = new Size(38, 32);
            btn2.TabIndex = 0;
            btn2.Text = "2";
            btn2.UseVisualStyleBackColor = true;
            btn2.Click += BtnDigit_Click;
            // 
            // btnBackspace
            // 
            btnBackspace.Location = new Point(15, 36);
            btnBackspace.Name = "btnBackspace";
            btnBackspace.Size = new Size(59, 32);
            btnBackspace.TabIndex = 19;
            btnBackspace.Text = "С";
            btnBackspace.UseVisualStyleBackColor = true;
            btnBackspace.Click += btnBackspace_Click;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(80, 36);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(59, 32);
            btnClear.TabIndex = 9;
            btnClear.Text = "СЕ";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // panelFun
            // 
            panelFun.BackColor = SystemColors.ButtonShadow;
            panelFun.Controls.Add(btnLn);
            panelFun.Controls.Add(btnBackspace);
            panelFun.Controls.Add(btnLog);
            panelFun.Controls.Add(btnAbs);
            panelFun.Controls.Add(btnTan);
            panelFun.Controls.Add(btnCos);
            panelFun.Controls.Add(labelFunc);
            panelFun.Controls.Add(btnSin);
            panelFun.Controls.Add(btnClear);
            panelFun.Location = new Point(280, 51);
            panelFun.Name = "panelFun";
            panelFun.Size = new Size(153, 207);
            panelFun.TabIndex = 3;
            // 
            // btnLn
            // 
            btnLn.Location = new Point(15, 150);
            btnLn.Name = "btnLn";
            btnLn.Size = new Size(59, 32);
            btnLn.TabIndex = 17;
            btnLn.Text = "ln";
            btnLn.UseVisualStyleBackColor = true;
            btnLn.Click += FunctionButton;
            // 
            // btnLog
            // 
            btnLog.Location = new Point(15, 112);
            btnLog.Name = "btnLog";
            btnLog.Size = new Size(59, 32);
            btnLog.TabIndex = 16;
            btnLog.Text = "log";
            btnLog.UseVisualStyleBackColor = true;
            btnLog.Click += FunctionButton;
            // 
            // btnAbs
            // 
            btnAbs.Location = new Point(15, 74);
            btnAbs.Name = "btnAbs";
            btnAbs.Size = new Size(59, 32);
            btnAbs.TabIndex = 15;
            btnAbs.Text = "abs";
            btnAbs.UseVisualStyleBackColor = true;
            btnAbs.Click += FunctionButton;
            // 
            // btnTan
            // 
            btnTan.Location = new Point(80, 150);
            btnTan.Name = "btnTan";
            btnTan.Size = new Size(59, 32);
            btnTan.TabIndex = 13;
            btnTan.Text = "tg";
            btnTan.UseVisualStyleBackColor = true;
            btnTan.Click += FunctionButton;
            // 
            // btnCos
            // 
            btnCos.Location = new Point(80, 112);
            btnCos.Name = "btnCos";
            btnCos.Size = new Size(59, 32);
            btnCos.TabIndex = 12;
            btnCos.Text = "cos";
            btnCos.UseVisualStyleBackColor = true;
            btnCos.Click += FunctionButton;
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
            btnSin.Location = new Point(80, 74);
            btnSin.Name = "btnSin";
            btnSin.Size = new Size(59, 32);
            btnSin.TabIndex = 1;
            btnSin.Text = "sin";
            btnSin.UseVisualStyleBackColor = true;
            btnSin.Click += FunctionButton;
            // 
            // panelElse
            // 
            panelElse.BackColor = SystemColors.ButtonShadow;
            panelElse.Controls.Add(btnExit);
            panelElse.Controls.Add(dataGridViewPer);
            panelElse.Controls.Add(btnRef);
            panelElse.Controls.Add(btnHistory);
            panelElse.Controls.Add(labelBtn);
            panelElse.Location = new Point(454, 51);
            panelElse.Name = "panelElse";
            panelElse.Size = new Size(197, 207);
            panelElse.TabIndex = 5;
            // 
            // btnExit
            // 
            btnExit.Location = new Point(13, 167);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(172, 22);
            btnExit.TabIndex = 14;
            btnExit.Text = "Выход";
            btnExit.UseVisualStyleBackColor = true;
            btnExit.Click += btnExit_Click;
            // 
            // dataGridViewPer
            // 
            dataGridViewPer.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewPer.BorderStyle = BorderStyle.None;
            dataGridViewPer.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewPer.Columns.AddRange(new DataGridViewColumn[] { colName, colValue });
            dataGridViewPer.Location = new Point(13, 36);
            dataGridViewPer.Name = "dataGridViewPer";
            dataGridViewPer.RowHeadersVisible = false;
            dataGridViewPer.Size = new Size(172, 80);
            dataGridViewPer.TabIndex = 14;
            dataGridViewPer.CellValidating += dataGridViewPer_CellValidating;
            dataGridViewPer.KeyDown += dataGridViewPer_KeyDown;
            // 
            // colName
            // 
            colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colName.FillWeight = 80F;
            colName.HeaderText = "Переменная";
            colName.Name = "colName";
            colName.Width = 101;
            // 
            // colValue
            // 
            colValue.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colValue.FillWeight = 70F;
            colValue.HeaderText = "Значение";
            colValue.Name = "colValue";
            // 
            // btnRef
            // 
            btnRef.Location = new Point(103, 122);
            btnRef.Name = "btnRef";
            btnRef.Size = new Size(82, 39);
            btnRef.TabIndex = 13;
            btnRef.Text = "Что это такое?";
            btnRef.UseVisualStyleBackColor = true;
            btnRef.Click += btnRef_Click;
            // 
            // btnHistory
            // 
            btnHistory.Location = new Point(13, 122);
            btnHistory.Name = "btnHistory";
            btnHistory.Size = new Size(84, 39);
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
            Controls.Add(panelElse);
            Controls.Add(panelFun);
            Controls.Add(panelKeypad);
            Controls.Add(txtResult);
            Controls.Add(txtExpression);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainForm";
            Text = "Ulitka Soft - Курсовая \"Калькулятор\"";
            panelKeypad.ResumeLayout(false);
            panelKeypad.PerformLayout();
            panelFun.ResumeLayout(false);
            panelFun.PerformLayout();
            panelElse.ResumeLayout(false);
            panelElse.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewPer).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtExpression;
        private TextBox txtResult;
        private Panel panelKeypad;
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
        private Button btnEquals;
        private Button btnClear;
        private Button btnMul;
        private Button btnSub;
        private Button btnAdd;
        private Label labelBase;
        private Button btnPow;
        private Button btnCloseBracket;
        private Button btnOpenBracket;
        private Button btnDiv;
        private Panel panelFun;
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
        private Panel panelElse;
        private Label labelBtn;
        private Button btnRef;
        private Button btnHistory;
        private DataGridView dataGridViewPer;
        private Button btnExit;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colValue;
    }
}
