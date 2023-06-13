using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sudokukuku
{
    public partial class Form2 : Form
    {
        const int n = 3; // 일반적으로 스도쿠 게임은 3x3 크기의 작은 그리드를 가지며, 따라서 n의 값은 3임.
        const int sizeButton = 50; // 버튼 사이즈 지정.
        public int[,] map = new int[n * n, n * n];
        public Button[,] buttons = new Button[n * n, n * n];

        public Form2()
        {
            InitializeComponent();
            GenerateMap();
        }

        public void GenerateMap() // 메인 지도 생성
        {
            for (int i = 0; i < n * n; i++) // 0부터 n*n 크기의 스도쿠 맵 생성 // i는 행
            {
                for (int j = 0; j < n * n; j++) // j는 열
                {
                    map[i, j] = (i * n + i / n + j) % (n * n) + 1;
                    // i * n: 현재 행의 큰 그리드 번호, i / n: 현재 행의 작은 그리드 내에서의 행 번호,
                    // 계산된 행 번호에 현재 열의 인덱스를 합산하고, n * n으로 나눈 나머지를 취합.
                    buttons[i, j] = new Button(); // buttons 배열에 각 셀(Button)을 저장
                }
            }
            MixButton();
            CreateMap();
            HideCells();
        }

        public void MixButton()
        {
            Random r = new Random();
            var block1 = r.Next(0, n);
            var block2 = r.Next(0, n);
            while (block1 == block2)
                block2 = r.Next(0, n);
            block1 *= n;
            block2 *= n;

            for (int i = 0; i < n * n; i++)
            {
                var k = block2;
                for (int j = block1; j < block1 + n; j++)
                {
                    var temp = map[i, j];
                    map[i, j] = map[i, k];
                    map[i, k] = temp;
                    k++;
                }
            }
        }

        public void HideCells() // 안 눌리는 숨겨진 버튼(셀) // 스도쿠 맵에서 일부 셀을 숨김.
        {
            int N = 50;
            Random r = new Random();
            for (int i = 0; i < n * n; i++)
            {
                for (int j = 0; j < n * n; j++)
                {
                    if (!string.IsNullOrEmpty(buttons[i, j].Text))
                    {
                        int a = r.Next(0, 3); // 0 이상 3 미만의 무작위 정수를 생성
                        buttons[i, j].Text = a == 0 ? "" : buttons[i, j].Text; // a가 0인 경우에만 해당 셀을 활성화시키고, 나머지 경우에는 비활성화시킵니다.
                        buttons[i, j].Enabled = a == 0 ? true : false;

                        if (a == 0)
                            N--;
                        if (N <= 0)
                            break;
                    }
                }
                if (N <= 0)
                    break;
            }
        }

        public void CreateMap() // 시작 버튼 누르면 스도쿠 시작
        {
            for (int i = 0; i < n * n; i++)
            {
                for (int j = 0; j < n * n; j++)
                {
                    Button button = new Button();
                    button.BackColor = Color.FloralWhite;
                    buttons[i, j] = button;
                    button.Size = new Size(sizeButton, sizeButton);
                    button.Text = map[i, j].ToString();
                    button.Click += OnCellPressed;
                    button.Location = new Point(j * sizeButton, i * sizeButton);
                    this.Controls.Add(button);
                }
            }
        }

        public void OnCellPressed(object sender, EventArgs e) // 버튼을 누를 때 1씩 증가
        {
            Button pressedButton = sender as Button;
            string buttonText = pressedButton.Text;
            if (string.IsNullOrEmpty(buttonText))
            {
                pressedButton.Text = "1";
            }
            else
            {
                int num = int.Parse(buttonText);
                num++;
                if (num == 10)
                    num = 1;
                pressedButton.Text = num.ToString();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < n * n; i++)
            {
                for (int j = 0; j < n * n; j++)
                {
                    var btnText = buttons[i, j].Text;
                    if (btnText != map[i, j].ToString())
                    {
                        MessageBox.Show("아직 완성되지 않았습니다.");
                        return;
                    }
                }
            }

            DialogResult result =  MessageBox.Show("스도쿠 성공! 다시 하시겠습니까?", "Excellent!!", MessageBoxButtons.YesNo);
            for (int i = 0; i < n * n; i++)
            {
                for (int j = 0; j < n * n; j++)
                {
                    if(result == DialogResult.Yes)
                        this.Controls.Remove(buttons[i, j]);

                    else if (result == DialogResult.No)                    
                        this.Close();                   
                }
                GenerateMap();
            }
        }
    }
}
