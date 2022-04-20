using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace TV_laba1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            chart1.Series.Clear();
            chart2.Series.Clear();
        }

        private Random Randomizer = new Random();
        private int count_of_exp;
        private int count_of_sers;
        private int good_exp = 0;
        private int X;
        private double Y;
        private const double Exp_one = 0.5;
        private double Exp_two;
        private double eps;
        private Table table = new Table();
        private double sum = 0;
        private double middle;
        private double ver_otkl;
        private double aver_otkl;
        private int del_range;
        private List<double> max = new List<double>();
        private List<double> min = new List<double>();
        private int time;
        private double pos_to_wall;
        private double from_wall;

        private int distance;

        private void get_data(int i) {
            #region getting_data

            count_of_exp = System.Convert.ToInt32(number_of_experiments.Text);
            count_of_sers = System.Convert.ToInt32(number_of_series.Text);
            try
            {
                ver_otkl = Convert.ToDouble(textBox1.Text.Replace('.', ','));
                aver_otkl = 1 - ver_otkl;
            }
            catch (FormatException)
            {
                ver_otkl = 1;
                aver_otkl = 0;
            }
            del_range = (int)(count_of_sers * (aver_otkl / 2));

            try
            {
                if (i == 1)
                {
                    time = System.Convert.ToInt32(textBox2.Text);
                }
            }
            catch (FormatException)
            {
                time = 15;
            }

            try
            {
                if (i == 2)
                {
                    distance = System.Convert.ToInt32(textBox3.Text);
                }
            }
            catch (FormatException)
            {
                distance = 1;
            }
            try
            {
                if (i == 2)
                {
                    pos_to_wall = System.Convert.ToDouble(textBox4.Text);
                    from_wall = 1 - pos_to_wall;
                }
            }
            catch (FormatException)
            {
                pos_to_wall = 0.33;
                from_wall = 0.66;
            }
            #endregion


        }

        private void button1_Click(object sender, EventArgs e)
        {
            get_data(0);

            #region charts
            /*очистка графиков*/

            chart1.Series.Clear();
            chart2.Series.Clear();

            chart1.ChartAreas.Clear();
            chart1.ChartAreas.Add("area1");

            chart1.ChartAreas[0].AxisY.Maximum = 1;
            chart1.ChartAreas[0].AxisY.Minimum = 0;

            chart1.ChartAreas[0].AxisX.MajorGrid.Interval = 0.5;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.GhostWhite;
            chart1.ChartAreas[0].AxisY.MajorGrid.Interval = 0.5;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;


            chart2.ChartAreas.Clear();
            chart2.ChartAreas.Add("area2");

            chart2.ChartAreas[0].AxisY.Maximum = 1.2;
            chart2.ChartAreas[0].AxisY.Minimum = 0;

            chart2.ChartAreas[0].AxisY.MajorGrid.Interval = 0.1;
            chart2.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.GhostWhite;
            chart2.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;


            chart2.Series.Add("Отклонение");
            chart2.Series.Add("Погрешность");
            chart2.Series.Add("Практическая оценка");
            chart2.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            chart2.Series[0].BorderWidth = 2;
            chart2.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            chart2.Series[1].BorderWidth = 2;
            chart2.Series[2].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            chart2.Series[2].BorderWidth = 2;
            #endregion charts

            #region logic
            for (int i = 0; i < count_of_sers; i++) {
                chart1.Series.Add(i.ToString());
                chart1.Series[i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
                chart1.Legends.Clear();

                good_exp = 0;
                sum = 0;

                for (int j = 1; j <= count_of_exp; j++) {
                    X = Randomizer.Next(0, 2);
                    if (X == 1) good_exp++;
                    Y = (double)good_exp / j;
                    sum += Math.Round(Y, 4);
                    middle = (sum / j);
                    chart1.Series[i].Points.AddXY(j, Y);

                    double result = table.get_by_y(ver_otkl / 2);

                    eps = result * Math.Pow((0.25 / j), .5);


                    if (i == count_of_sers - 1 && textBox1.Text != "") {
                        try
                        {
                            chart2.Series[0].Points.AddXY(j, Math.Round(Math.Abs(middle / count_of_sers) / 2, 4));
                            chart2.Series[1].Points.AddXY(j, Math.Abs(Math.Round(eps, 4)));
                        }
                        catch (ArgumentOutOfRangeException) { }
                    }
                }
            }
            #endregion logic

            #region chart2

            try
            {
                chart1.Series.Add("Рукав1");
                chart1.Series.Add("Рукав2");
                chart1.Series[count_of_sers].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
                chart1.Series[count_of_sers].BorderWidth = 2;
                chart1.Series[count_of_sers].Color = Color.Orange;
                chart1.Series[count_of_sers + 1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
                chart1.Series[count_of_sers + 1].BorderWidth = 2;
                chart1.Series[count_of_sers + 1].Color = Color.Orange;

                chart1.Series.Add("sr");
                chart1.Series[count_of_sers + 2].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
                chart1.Series[count_of_sers + 2].Color = Color.Black;
                chart1.Series[count_of_sers + 2].BorderWidth = 2;
            }
            catch (ArgumentOutOfRangeException) { 
            
            }
            #endregion chart2

            #region logic1
            try
            {
                for (int i = 1; i <= count_of_exp; i++)
                {
                    min.Clear();
                    max.Clear();
                    for (int j = 0; j < count_of_sers; j++)
                    {
                        min.Add(chart1.Series[j].Points[i - 1].YValues[0]);
                        max.Add(chart1.Series[j].Points[i - 1].YValues[0]);
                    }

                    //chart2.Series[0].Points.AddXY(i, Math.Round(Exp_two - Math.Abs(max.Sum() / count_of_sers), 4));
                    chart1.Series[count_of_sers + 2].Points.AddXY(i, max.Sum() / count_of_sers);

                    min.Sort();
                    min.RemoveRange(0, del_range);
                    max.Sort();
                    max.RemoveRange(count_of_sers - del_range, del_range);


                    chart1.Series[count_of_sers].Points.AddXY(i, min.Min());
                    chart1.Series[count_of_sers + 1].Points.AddXY(i, max.Max());

                    chart2.Series[2].Points.AddXY(i, Math.Round((max.Max() - min.Min()) / 2, 4));
                }
            }
            catch (ArgumentOutOfRangeException) { 
            
            }

            #endregion logic1

            chart1.ChartAreas[0].AxisX.IsLogarithmic = true;
            chart2.ChartAreas[0].AxisX.IsLogarithmic = true;

            label3.Text = "Результат: " + Math.Round(Y, 4) + " ± " + Math.Round(eps, 4);
            label4.Text = "Отклонение от теоретического значения: " + Math.Round(Math.Abs(Exp_one - Y), 4);
            label5.Text = "Теоретическое значение: " + Exp_one;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            get_data(1);
            int X1;

            #region chart
            chart1.Series.Clear();
            chart1.ChartAreas[0].AxisX.IsLogarithmic = false;

            chart2.Series.Clear();
            chart2.ChartAreas[0].AxisX.IsLogarithmic = false;
            chart2.ChartAreas[0].AxisY.Minimum = 0;

            chart2.Series.Add("Отклонение");
            chart2.Series.Add("Погрешность");
            chart2.Series.Add("Практическая оценка");
            chart2.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            chart2.Series[0].BorderWidth = 2;
            chart2.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            chart2.Series[1].BorderWidth = 2;
            chart2.Series[2].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            chart2.Series[2].BorderWidth = 2;

            chart3.Series.Clear();
            chart3.ChartAreas.Clear();
            chart3.ChartAreas.Add("meets");
            chart3.ChartAreas[0].AxisX.Maximum = 60;
            chart3.ChartAreas[0].AxisX.Minimum = 0;
            chart3.ChartAreas[0].AxisY.Maximum = 60;
            chart3.ChartAreas[0].AxisY.Minimum = 0;
            chart3.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightBlue;
            chart3.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightBlue;
            chart3.Series.Add("meets");
            chart3.Legends.Clear();
            chart3.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;

            Exp_two = (3600 - (60 - time) * (60 - time)) / (double)3600;
            for (int i = 0; i < 60; i++)
            {
                for (int j = 0; j < 60; j++)
                {
                    if (Math.Abs(i - j) < time)
                    {
                        chart3.Series[0].Points.AddXY(i, j);
                    }
                }
            }

            #endregion

            #region logic
            for (int i = 0; i < count_of_sers; i++)
            {
                chart1.Series.Add(i.ToString());
                chart1.Series[i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
                chart1.Legends.Clear();

                good_exp = 0;
                sum = 0;

                for (int j = 1; j <= count_of_exp; j++)
                {
                    X = Randomizer.Next(0, 60);
                    X1 = Randomizer.Next(0, 60);
                    if (Math.Abs(X - X1) < time) good_exp++;
                    Y = (double)good_exp / j;
                    sum += Math.Round(Y, 4);
                    middle = (sum / j);
                    chart1.Series[i].Points.AddXY(j, Y);

                    double result = table.get_by_y(ver_otkl / 2);

                    eps = result * Math.Pow(Exp_two * (1 - Exp_two) / j, .5);

                    if (i == count_of_sers - 1 && textBox1.Text != "")
                    {
                        try
                        {
                            chart2.Series[1].Points.AddXY(j, Math.Abs(Math.Round(eps, 4)));
                        }
                        catch (ArgumentOutOfRangeException) { }
                    }
                }
            }
            chart1.ChartAreas[0].AxisX.IsLogarithmic = true;
            chart2.ChartAreas[0].AxisX.IsLogarithmic = true;

            #endregion

            #region logic1
            chart1.Series.Add("рукав1");
            chart1.Series[count_of_sers].Color = Color.Orange;
            chart1.Series[count_of_sers].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            chart1.Series[count_of_sers].BorderWidth = 2;
            chart1.Series.Add("рукав2");
            chart1.Series[count_of_sers+1].Color = Color.Orange;
            chart1.Series[count_of_sers+1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            chart1.Series[count_of_sers+1].BorderWidth = 2;
            chart1.Series.Add("sr");
            chart1.Series[count_of_sers + 2].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            chart1.Series[count_of_sers + 2].Color = Color.Black;
            chart1.Series[count_of_sers + 2].BorderWidth = 2;
            try
            {
                for (int i = 1; i <= count_of_exp; i++)
                {
                    min.Clear();
                    max.Clear();
                    for (int j = 0; j < count_of_sers; j++)
                    {
                        min.Add(chart1.Series[j].Points[i - 1].YValues[0]);
                        max.Add(chart1.Series[j].Points[i - 1].YValues[0]);
                    }

                    chart1.Series[count_of_sers + 2].Points.AddXY(i, max.Sum() / count_of_sers);
                    chart2.Series[0].Points.AddXY(i, Math.Round(Exp_two - Math.Abs(max.Sum() / count_of_sers), 4));


                    min.Sort();
                    min.RemoveRange(0, del_range);
                    max.Sort();
                    max.RemoveRange(count_of_sers - del_range, del_range);

                    

                    chart1.Series[count_of_sers].Points.AddXY(i, min.Min());
                    chart1.Series[count_of_sers + 1].Points.AddXY(i, max.Max());

                    // практическая оценка
                    chart2.Series[2].Points.AddXY(i, Math.Round((max.Max() - min.Min()) / 2, 4));
                }
            }
            catch (ArgumentOutOfRangeException)
            {

            }

            #endregion logic1

            label3.Text = "Результат: " + Math.Round(Y, 4) + " ± " + Math.Round(eps, 4);
            label4.Text = "Отклонение от теоретического значения: " + Math.Round(Math.Abs(Exp_two - Y), 4);
            label5.Text = "Теоретическое значение: " + Math.Round(Exp_two, 3);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            get_data(2);
            int cycle = Convert.ToInt32(textBox5.Text);

            #region charts
            /*очистка графиков*/

            chart1.Series.Clear();
            chart2.Series.Clear();

            chart1.ChartAreas.Clear();
            chart1.ChartAreas.Add("area1");

            chart1.ChartAreas[0].AxisY.Maximum = 1;
            chart1.ChartAreas[0].AxisY.Minimum = 0;

            chart1.ChartAreas[0].AxisX.MajorGrid.Interval = 0.5;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.GhostWhite;
            chart1.ChartAreas[0].AxisY.MajorGrid.Interval = 0.5;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;


            chart2.ChartAreas.Clear();
            chart2.ChartAreas.Add("area2");

            chart2.ChartAreas[0].AxisY.Maximum = 1.2;
            chart2.ChartAreas[0].AxisY.Minimum = 0;

            chart2.ChartAreas[0].AxisY.MajorGrid.Interval = 0.1;
            chart2.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.GhostWhite;
            chart2.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;


            chart2.Series.Add("Погрешность");
            chart2.Series.Add("Практическая оценка");
            chart2.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            chart2.Series[0].BorderWidth = 2;
            chart2.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            chart2.Series[1].BorderWidth = 2;
            #endregion charts

            #region logic
            for (int i = 0; i < count_of_sers; i++)
            {
                chart1.Series.Add(i.ToString());
                chart1.Series[i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
                chart1.Legends.Clear();

                good_exp = 0;
                sum = 0;

                for (int j = 1; j <= count_of_exp; j++)
                {

                    for (int o = 0; o < cycle; o++) {
                        if (Randomizer.NextDouble() < pos_to_wall) distance--;
                        else distance++;

                        if (distance <= 0)
                        {
                            good_exp++;
                            break;
                        }
                    }

                    try
                    {
                        distance = Convert.ToInt32(textBox3.Text);
                    }
                    catch (FormatException)
                    {
                        distance = 1;
                    }

                    Y = (double)good_exp / j;
                    sum += Math.Round(Y, 4);
                    middle = (sum / j);
                    chart1.Series[i].Points.AddXY(j, Y);

                    double result = table.get_by_y(ver_otkl / 2);

                    eps = result * Math.Pow(pos_to_wall * from_wall / j, .5);


                    if (i == count_of_sers - 1 && textBox1.Text != "")
                    {
                        try
                        {
                            chart2.Series[0].Points.AddXY(j, Math.Abs(Math.Round(eps, 4)));
                        }
                        catch (ArgumentOutOfRangeException) { }
                    }
                }
            }
            #endregion logic

            #region chart2
            try
            {
                chart1.Series.Add("рукав1");
                chart1.Series[count_of_sers].Color = Color.Orange;
                chart1.Series[count_of_sers].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
                chart1.Series[count_of_sers].BorderWidth = 2;
                chart1.Series.Add("рукав2");
                chart1.Series[count_of_sers + 1].Color = Color.Orange;
                chart1.Series[count_of_sers + 1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
                chart1.Series[count_of_sers + 1].BorderWidth = 2;
                chart1.Series.Add("sr");
                chart1.Series[count_of_sers + 2].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
                chart1.Series[count_of_sers + 2].Color = Color.Black;
                chart1.Series[count_of_sers + 2].BorderWidth = 2;
            }
            catch (ArgumentOutOfRangeException)
            {

            }
            #endregion chart2

            #region logic1
            try
            {
                for (int i = 1; i <= count_of_exp; i++)
                {
                    min.Clear();
                    max.Clear();
                    for (int j = 0; j < count_of_sers; j++)
                    {
                        min.Add(chart1.Series[j].Points[i - 1].YValues[0]);
                        max.Add(chart1.Series[j].Points[i - 1].YValues[0]);
                    }

                    chart1.Series[count_of_sers + 2].Points.AddXY(i, max.Sum() / count_of_sers);


                    min.Sort();
                    min.RemoveRange(0, del_range);
                    max.Sort();
                    max.RemoveRange(count_of_sers - del_range, del_range);



                    chart1.Series[count_of_sers].Points.AddXY(i, min.Min());
                    chart1.Series[count_of_sers + 1].Points.AddXY(i, max.Max());

                    // практическая оценка
                    chart2.Series[1].Points.AddXY(i, Math.Round((max.Max() - min.Min()) / 2, 4));
                }
            }
            catch (ArgumentOutOfRangeException)
            {

            }

            #endregion logic1

            chart1.ChartAreas[0].AxisX.IsLogarithmic = true;
            chart2.ChartAreas[0].AxisX.IsLogarithmic = true;
            label3.Text = "Результат: " + Math.Round(Y, 4) + " ± " + Math.Round(eps, 4);
            label4.Text = " ";
            label5.Text = " ";
        }
    }

    class Table
    {
        public Table()
        {
            this.TABL(0.01);
        }
        private List<List<double>> GET = new List<List<double>>();

        public double func_laplace(double a)
        {
            double result = 0;
            result = Math.Exp((-a * a) / 2);
            return result;
        }


        private double Simpson(double a, double b, int n)
        {
            var h = (b - a) / n;
            double sum1 = 0;
            double sum2 = 0;
            for (var k = 1; k <= n; k++)
            {
                var xk = a + k * h;
                if (k <= n - 1)
                {
                    sum1 += func_laplace(xk);
                }

                var xk_1 = a + (k - 1) * h;
                sum2 += func_laplace((xk + xk_1) / 2);
            }

            var result = h / 3 * (1 / 2 * func_laplace(a) + sum1 + 2 * sum2 + 1 / 2 * func_laplace(b));
            return result * 1.0 / Math.Pow(2 * Math.PI, .5);
        }
        private void TABL(double precision)
        {
            double i = 0;
            while (i < 5)
            {
                double result = Simpson(0, i, 100);
                this.add(i, Math.Round(result, 3));
                i += precision;
                i = Math.Round(i, 3);
            }
        }
        public void add(double x, double res)
        {
            var tmp = new List<double>();
            tmp.Add(x);
            tmp.Add(res);
            GET.Add(tmp);
        }

        public void insertion_sort() {
            for (int i = 1; i < GET.Count; i++)
            {
                {
                    double cur = GET[i][0];
                    int j = i;
                    while (j > 0 && cur < GET[j - 1][0])
                    {
                        GET[j][0] = GET[j - 1][0];
                        j--;
                    }
                    GET[j][0] = cur;
                }
            }
        }

        public double get_by_y(double res)
        {
            insertion_sort();
            for (int i = 1; i < GET.Count() - 1; i++)
            
            {
                if (GET[i][1] == res)
                {
                    return GET[i][0];
                }
                else if (GET[i][1] < res + 0.01 && GET[i][1] > res - 0.01)
                {
                    return GET[i][0];
                }
            }
            return 0;
        }
    }
}

