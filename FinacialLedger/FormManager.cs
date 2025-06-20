namespace Proj_FinacialLedger_202444004
{
    public partial class FormManager: Form 
    {
        FinacialLedger finacialLedger;
        public string PATH
        {
            get
            {
                var path = "c:\\AccountData";
                if (false == Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
        }
        private void OpenInfo(int year)
        { 
            String filename = Path.Combine(PATH, year.ToString() + ".txt");
            if (true == System.IO.File.Exists(filename))
            {
                try
                {
                    bool first = true;
                    using (var fs = new FileStream(filename, FileMode.Open))
                    {
                        using (var sr = new StreamReader(fs))
                        {
                            while (false == sr.EndOfStream)
                            {
                                var data = sr.ReadLine();
                                if (first)
                                {
                                    int.TryParse(data.Trim(), out int t);
                                    finacialLedger = new FinacialLedger(year, t);
                                    tbxTargetAmount.Text = data.Trim();
                                    first = false;
                                } else {
                                    var spData = data.Trim().Split(new char[] { ',' });
                                    DateTime t = DateTime.ParseExact(spData[1], "yyyyMMdd HHmmss", CultureInfo.InvariantCulture);
                                    int.TryParse(spData[3].Trim(), out int money);
                                    if (spData[0] == "수입")
                                    {
                                        finacialLedger.RegIncome(t, spData[2], money);
                                        lbIncome.Items.Add(data);
                                    } else {
                                        finacialLedger.RegExpenditure(t, spData[2], money);
                                        lbExpense.Items.Add(data);
                                    }
                                    lbAll.Items.Add(data);
                                }   
                            }
                        }
                        calcTotal();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
        private void SaveInfo(int year, int targetAmount)
        {
            finacialLedger = new FinacialLedger(year, targetAmount);
            String filename = Path.Combine(PATH, year.ToString() + ".txt");
            //참고
            try
            {
                using (var fs = new FileStream(filename, FileMode.Append))
                {//열고
                    using (var sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(finacialLedger.TargetAmount);
                        for(int i=-0; i < lbAll.Items.Count; i++) { 
                            sw.WriteLine(lbAll.Items[i]);
                        }
                    }
                }//닫고
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public FormManager()
        {
            InitializeComponent();
            //finacialLedger = new FinacialLedger();
            //info = new List<Info>();
        }

        private void btnInitialize_Click(object sender, EventArgs e)
        {
            tbxYear.Text = "";
            tbxTargetAmount.Text = "0";
            lbIncome.Items.Clear();
            lbExpense.Items.Clear();
            lbAll.Items.Clear();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbxYear.Text))
            {
                MessageBox.Show("연도 입력");
                tbxYear.Focus();
                return;
            }
            int.TryParse(tbxYear.Text.Trim(), out int year);
            OpenInfo(year);
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbxYear.Text))
            {
                MessageBox.Show("연도 입력");
                tbxYear.Focus();
                return;
            }
            if (string.IsNullOrEmpty(tbxTargetAmount.Text))
            {
                MessageBox.Show("목표액 입력");
                tbxTargetAmount.Focus();
                return;
            }
            int.TryParse(tbxYear.Text.Trim(), out int year);
            int.TryParse(tbxTargetAmount.Text.Trim(), out int targetAmount);
            SaveInfo(year, targetAmount);
        }
        void calcTotal() {
            int totalIncome = 0;
            for (int i = 0; i < lbIncome.Items.Count; i++)
            {
                String tmp = lbIncome.Items[i] as String;
                String[] ie = tmp.Split(new char[] { ',' });
                int.TryParse(ie[3], out int tmpIncome);
                totalIncome += tmpIncome;
            }
            lblTotalIncome.Text = "총 수입: " + totalIncome.ToString() + "원";

            int totalEx = 0;
            for (int i = 0; i < lbExpense.Items.Count; i++)
            {
                String tmp = lbExpense.Items[i] as String;
                String[] ie = tmp.Split(new char[] { ',' });
                int.TryParse(ie[3], out int tmpEx);
                totalEx += tmpEx;
            }
            lblTotalExpense.Text = "총 지출: " + totalEx.ToString() + "원";
            lblBalance.Text = "잔액: " + (totalIncome - totalEx).ToString();
            if ((totalIncome - totalEx) > 0) { 
                lblStatus.Text = "상태:흑자";
                lblStatus.ForeColor = Color.Blue;
            } else {
                lblStatus.Text = "상태:적자";
                lblStatus.ForeColor = Color.Red;
            }
        }
        private void btnRegisterIncome_Click(object sender, EventArgs e)
        {
            checkIncomeEmpty(); //textbox check
            //날짜, 내역, 금액
            //날짜: 년, 월, 일 => 20250101 형태로
            //내역: 내역
            //금액: 숫자
            
            DateTime d = timeFormat(tbxIncomeYear.Text, tbxIncomeMonth.Text, tbxIncomeDay.Text); 
            //int.TryParse(tbxIncomeYear.Text.Trim(), out int year);
            //int.TryParse(tbxIncomeMonth.Text.Trim(), out int month);
            //int.TryParse(tbxIncomeDay.Text.Trim(), out int day);

            int.TryParse(tbxIncomeAmount.Text.Trim(), out int amount);

            FinacialLedger f = new FinacialLedger(0);
            //DateTime d = new DateTime(year, month, day); 
            f.RegIncome(d, tbxIncomeDetail.Text, amount);

            //Info info = new Info(tbxIncomeDetail.Text.Trim(), amount);
            
            string income = d.ToString("yyyyMMdd HHmmss") + "," + tbxIncomeDetail.Text + "," + amount.ToString(); //info.ToString();
            
            lbIncome.Items.Add("수입," + income);
            lbAll.Items.Add("수입,"+income);
            calcTotal();

        }

        private DateTime timeFormat(string year, string month, string day)
        {
            int.TryParse(year.Trim(), out int y);
            int.TryParse(month.Trim(), out int m);
            int.TryParse(day.Trim(), out int d);

            DateTime now = DateTime.Now; // 현재 시간 가져오기
            DateTime dateTime = new DateTime(y, m, d, now.Hour, now.Minute, now.Second);

            return dateTime;
        }

        private void checkIncomeEmpty()
        {
            if (string.IsNullOrEmpty(tbxIncomeYear.Text))
            {
                MessageBox.Show("연도 입력");
                tbxIncomeYear.Focus();
                return;
            }
            if (string.IsNullOrEmpty(tbxIncomeMonth.Text))
            {
                MessageBox.Show("월 입력");
                tbxIncomeMonth.Focus();
                return;
            }
            if (string.IsNullOrEmpty(tbxIncomeDay.Text))
            {
                MessageBox.Show("일 입력");
                tbxIncomeDay.Focus();
                return;
            }
            if (string.IsNullOrEmpty(tbxIncomeDetail.Text))
            {
                MessageBox.Show("내역 입력");
                tbxIncomeDetail.Focus();
                return;
            }
            if (string.IsNullOrEmpty(tbxIncomeAmount.Text))
            {
                MessageBox.Show("금액 입력");
                tbxIncomeAmount.Focus();
                return;
            } 
        }

        private void btnRegisterExpense_Click(object sender, EventArgs e)
        {
            checkExpenseEmpty();

            DateTime d = timeFormat(tbxExpenseYear.Text, tbxExpenseMonth.Text, tbxExpenseDay.Text);

            int.TryParse(tbxExpenseAmount.Text.Trim(), out int amount);

            FinacialLedger f = new FinacialLedger(0);
            f.RegExpenditure(d, tbxExpenseDetail.Text, amount);
            //Info info = new Info(tbxExpenseDetail.Text.Trim(), amount);
            string expenditure = d.ToString("yyyyMMdd HHmmss") + "," + tbxExpenseDetail.Text.Trim()+","+ amount; //info.ToString();

            lbExpense.Items.Add("지출," + expenditure);
            lbAll.Items.Add("지출," + expenditure);
            calcTotal();
        }

        private void checkExpenseEmpty()
        {
            if (string.IsNullOrEmpty(tbxExpenseYear.Text))
            {
                MessageBox.Show("연도 입력");
                tbxExpenseYear.Focus();
                return;
            }
            if (string.IsNullOrEmpty(tbxExpenseMonth.Text))
            {
                MessageBox.Show("월 입력");
                tbxExpenseMonth.Focus();
                return;
            }
            if (string.IsNullOrEmpty(tbxExpenseDay.Text))
            {
                MessageBox.Show("일 입력");
                tbxExpenseDay.Focus();
                return;
            }
            if (string.IsNullOrEmpty(tbxExpenseDetail.Text))
            {
                MessageBox.Show("내역 입력");
                tbxExpenseDetail.Focus();
                return;
            }
            if (string.IsNullOrEmpty(tbxExpenseAmount.Text))
            {
                MessageBox.Show("금액 입력");
                tbxExpenseAmount.Focus();
                return;
            }
        }

        private void btnQuickIncome_Click(object sender, EventArgs e)
        {
            checkQuickEmpty();

            DateTime d = DateTime.Now;

            int.TryParse(tbxQuickAmount.Text.Trim(), out int amount);

            FinacialLedger f = new FinacialLedger(0);
            f.RegIncome(d, tbxQuickDetail.Text, amount);
            Info info = new Info(tbxQuickDetail.Text.Trim(), amount);

            string income = d.ToString("yyyyMMdd HHmmss") + "," + tbxQuickDetail.Text.Trim()+","+amount;//info.ToString();

            lbIncome.Items.Add("수입," + income);
            lbAll.Items.Add("수입," + income);
            calcTotal();
        }

        private void checkQuickEmpty()
        {
            if (string.IsNullOrEmpty(tbxQuickDetail.Text))
            {
                MessageBox.Show("내역 입력");
                tbxQuickDetail.Focus();
                return;
            }
            if (string.IsNullOrEmpty(tbxQuickAmount.Text))
            {
                MessageBox.Show("금액 입력");
                tbxQuickAmount.Focus();
                return;
            }
            
        }

        private void btnQuickExpense_Click(object sender, EventArgs e)
        {
            checkQuickEmpty();
            
            DateTime d = DateTime.Now;

            int.TryParse(tbxQuickAmount.Text.Trim(), out int amount);

            FinacialLedger f = new FinacialLedger(0);
            f.RegExpenditure(d, tbxQuickDetail.Text, amount);
            Info info = new Info(tbxQuickDetail.Text.Trim(), amount);

            string expenditure = d.ToString("yyyyMMdd HHmmss") + "," + tbxQuickDetail.Text.Trim() + ","  + amount;//info.ToString();


            lbExpense.Items.Add("지출," + expenditure);
            lbAll.Items.Add("지출," + expenditure);
            calcTotal();
        }
    }
}
