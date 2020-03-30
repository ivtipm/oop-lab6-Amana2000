using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MathNewLib; //БИБЛИОТЕКА С КЛАССОМ МАТРИЦ

namespace MatrixWPF_test5
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Переменные типа "Матрица", видимые на всей форме
        public MathNewLib.Matrix Matrix1; //Сюда загрузится первая сгенерированная матрица
        public MathNewLib.Matrix Matrix2; //Сюда загрузится вторая сгенерированная матрица
        public MathNewLib.Matrix Matrix3; //Сюда загрузится матрица, полученная после арифметических действий межу M1 и M2

        public MainWindow()
        {
            InitializeComponent(); //При первоначальном создании главной формы текст об ошибках скрывается
            Lbl_1_Error.Visibility = Visibility.Hidden;
            Lbl_2_Error.Visibility = Visibility.Hidden;
            Lbl_3_Error.Visibility = Visibility.Hidden;
        }

        //Генерирует первую матрицу-------------------------------------
        public void Btn_GenMatr1_Click(object sender, RoutedEventArgs e)
        {
            Lbl_1_Error.Visibility = Visibility.Hidden; //Скрывает сообщение об ошибке, т.к. ошибка ещё не случилась

            try //Чтобы не вылетало при создании матрицы
            {
                int m = Convert.ToInt32(Tbx_Matrix1_M.Text); //m и n - размеры матрицы, берём String значения из TextBox и конвертируем их в Int
                int n = Convert.ToInt32(Tbx_Matrix1_N.Text);
                MathNewLib.Matrix M1 = new MathNewLib.Matrix(m, n); //Создаём матрицу (m,n)

                int min = Convert.ToInt32(Tbx_Matrix1_Min.Text); //max и min - диапазон случайных значений для элементов матрицы
                int max = Convert.ToInt32(Tbx_Matrix1_Max.Text);
                M1.Random(min, max); //Заполняем матрицу случайными элементами из диапазона (min,max)

                Matrix1 = M1; //Отправляем матрицу в Matrix1, которая видна на всей форме (Чтобы потом её использовать для вычислений, которые производятся в коде на другой кнопке)

                LoadMatrixToDataGrid(M1, DtGrd_1); //Метод для загрузки матрицы в DataGrid (M1 - матрица, DtGrd_1 - DataGrid)
                try
                {
                    LoadDetToLabel(M1.CalculateDeterminant(), lbl_Det); //Пробуем рассчитать определитель для матрицы
                }
                catch (Exception)
                {
                    lbl_Det.Content = "Определитель: " + "Нельзя посчитать"; //Если не получилось, значит матрица не квадратная
                }
            }
            catch //Если произошли ошибки, то показать сообщение об ошибке при создании матрицы
            {
                Lbl_1_Error.Visibility = Visibility.Visible;
                DtGrd_1.ItemsSource = " ";
            }
        }

        //Сбрасывает поля для генерации первой матрицы к значениям по умолчанию
        private void Btn_GenMatr1_Reset_Click(object sender, RoutedEventArgs e)
        {
            Lbl_1_Error.Visibility = Visibility.Hidden;
            lbl_Det.Content = "Определитель: ";
            Tbx_Matrix1_M.Text = Tbx_Matrix1_N.Text = "3";
            Tbx_Matrix1_Min.Text = "-9"; Tbx_Matrix1_Max.Text = "9";
            DtGrd_1.ItemsSource = " ";
        }

        //Метод для загрузки матрицы в DataGrid
        void LoadMatrixToDataGrid(MathNewLib.Matrix data, DataGrid DG)
        {
            double[,] _dataArray; //Временная матрица
            DataView _dataView;   //Сюда в конце загрузится матрица и мы сможем подключить её к DataGrid.ItemSource

            _dataArray = data.GetData(); //Передаём элементы матрицы во временную матрицу

            /*Локальные переменные можно объявлять без указания конкретного типа. 
             * Ключевое слово VAR указывает, что компилятор должен вывести тип переменной 
             * из выражения справа от оператора инициализации.
             */
            var array = _dataArray;
            var rows = array.GetLength(0);
            var columns = array.GetLength(1);

            var Dt = new DataTable(); //Создастся таблица с элементами матрицы, которая загрузится в DataView

            // Создаются столбцы с названиями "1", "2", "3", ...
            for (var c = 0; c < columns; c++)
            {
                Dt.Columns.Add(new DataColumn((c + 1).ToString())); //Добавляем новый столбец в DataTable
            }
            // Создаются и заполняются данными строки 
            for (var r = 0; r < rows; r++)
            {
                var newRow = Dt.NewRow();
                for (var c = 0; c < columns; c++)
                {
                    newRow[c] = array[r, c];
                }
                Dt.Rows.Add(newRow); //Добавляем новую строчку в DataTable
            }
            _dataView = Dt.DefaultView;

            DG.ItemsSource = _dataView; //Теперь матрица загружена в DataGrid
        }

        //Метод позволяет загрузить значение определителя матрицы в Label
        void LoadDetToLabel(double det, Label lbl)
        {
            try
            {
                lbl.Content = "Определитель: " + det;
            }
            catch (Exception)
            {
                lbl.Content = "Определитель: " + "Нельзя посчитать";
            }
        }


        public void Btn_GenMatr2_Click(object sender, RoutedEventArgs e)
        {
            Lbl_2_Error.Visibility = Visibility.Hidden;

            try
            {
                int m = Convert.ToInt32(Tbx_Matrix2_M.Text);
                int n = Convert.ToInt32(Tbx_Matrix2_N.Text);
                MathNewLib.Matrix M2 = new MathNewLib.Matrix(m, n);

                int min = Convert.ToInt32(Tbx_Matrix2_Min.Text);
                int max = Convert.ToInt32(Tbx_Matrix2_Max.Text);

                M2.Random(min, max);
                Matrix2 = M2;

                LoadMatrixToDataGrid(M2, DtGrd_2);
                try
                {
                    LoadDetToLabel(M2.CalculateDeterminant(), lbl_Det_2);
                }
                catch (Exception)
                {
                    lbl_Det_2.Content = "Определитель: " + "Нельзя посчитать";                    
                }
            }
            catch
            {
                Lbl_2_Error.Visibility = Visibility.Visible;
                DtGrd_2.ItemsSource = " ";
            }
        }

        private void Btn_GenMatr2_Reset_Click(object sender, RoutedEventArgs e)
        {
            Lbl_2_Error.Visibility = Visibility.Hidden;
            lbl_Det_2.Content = "Определитель: ";
            Tbx_Matrix2_M.Text = Tbx_Matrix2_N.Text = "3";
            Tbx_Matrix2_Min.Text = "-9"; Tbx_Matrix2_Max.Text = "9";
            DtGrd_2.ItemsSource = " ";
        }

        //Складывает 2 матрицы
        private void Btn_GenMatr3_Plus_Click(object sender, RoutedEventArgs e)
        {
            Lbl_3_Error.Visibility = Visibility.Hidden;
            try //Попытка сложить две матрицы
            {
                Matrix3 = Matrix1 + Matrix2;
                LoadMatrixToDataGrid(Matrix3, DtGrd_3);
                try
                {
                    LoadDetToLabel(Matrix3.CalculateDeterminant(), lbl_Det_3);
                }
                catch (Exception)
                {
                    lbl_Det_3.Content = "Определитель: " + "Нельзя посчитать";
                }
            }
            catch(Exception) //Исключение случится, если матрицы не одинакового размера
            {
                Lbl_3_Error.Visibility = Visibility.Visible;
                DtGrd_3.ItemsSource = " ";
            }
        }

        private void Btn_GenMatr3_AminusB_Click(object sender, RoutedEventArgs e)
        {
            Lbl_3_Error.Visibility = Visibility.Hidden;
            try
            {
                Matrix3 = Matrix1 - Matrix2;
                LoadMatrixToDataGrid(Matrix3, DtGrd_3);
                try
                {
                    LoadDetToLabel(Matrix3.CalculateDeterminant(), lbl_Det_3);
                }
                catch (Exception)
                {
                    lbl_Det_3.Content = "Определитель: " + "Нельзя посчитать";
                }
            }
            catch (Exception)
            {
                Lbl_3_Error.Visibility = Visibility.Visible;
                DtGrd_3.ItemsSource = " ";
            }
        }

        private void Btn_GenMatr3_BminusA_Click(object sender, RoutedEventArgs e)
        {
            Lbl_3_Error.Visibility = Visibility.Hidden;
            try
            {
                Matrix3 = Matrix2 - Matrix1;
                LoadMatrixToDataGrid(Matrix3, DtGrd_3);
                try
                {
                    LoadDetToLabel(Matrix3.CalculateDeterminant(), lbl_Det_3);
                }
                catch (Exception)
                {
                    lbl_Det_3.Content = "Определитель: " + "Нельзя посчитать";
                }
            }
            catch (Exception)
            {
                Lbl_3_Error.Visibility = Visibility.Visible;
                DtGrd_3.ItemsSource = " ";
            }
        }

        private void Btn_GenMatr3_AmulB_Click(object sender, RoutedEventArgs e)
        {
            Lbl_3_Error.Visibility = Visibility.Hidden;
            try
            {
                Matrix3 = Matrix1 * Matrix2;
                LoadMatrixToDataGrid(Matrix3, DtGrd_3);
                try
                {
                    LoadDetToLabel(Matrix3.CalculateDeterminant(), lbl_Det_3);
                }
                catch (Exception)
                {
                    lbl_Det_3.Content = "Определитель: " + "Нельзя посчитать";
                }
            }
            catch (Exception)
            {
                Lbl_3_Error.Visibility = Visibility.Visible;
                DtGrd_3.ItemsSource = " ";
            }
        }

        private void Btn_GenMatr3_BmulA_Click(object sender, RoutedEventArgs e)
        {
            Lbl_3_Error.Visibility = Visibility.Hidden;
            try
            {
                Matrix3 = Matrix2 * Matrix1;
                LoadMatrixToDataGrid(Matrix3, DtGrd_3);
                try
                {
                    LoadDetToLabel(Matrix3.CalculateDeterminant(), lbl_Det_3);
                }
                catch (Exception)
                {
                    lbl_Det_3.Content = "Определитель: " + "Нельзя посчитать";
                }
            }
            catch (Exception)
            {
                Lbl_3_Error.Visibility = Visibility.Visible;
                DtGrd_3.ItemsSource = " ";
            }
        }

        //Умнножает первую матрицу на число N
        private void Btn_GenMatr3_AmulN_Click(object sender, RoutedEventArgs e)
        {
            Lbl_3_Error.Visibility = Visibility.Hidden;
            try
            {
                double Number = Convert.ToDouble(Tbx_Matrix3_Nmul.Text); //Получаем число N из текстбокса
                Matrix3 = Matrix1 * Number;
                LoadMatrixToDataGrid(Matrix3, DtGrd_3);
                try
                {
                    LoadDetToLabel(Matrix3.CalculateDeterminant(), lbl_Det_3);
                }
                catch (Exception)
                {
                    lbl_Det_3.Content = "Определитель: " + "Нельзя посчитать";
                }
            }
            catch (Exception)
            {
                Lbl_3_Error.Visibility = Visibility.Visible;
                DtGrd_3.ItemsSource = " ";
            }
        }

        private void Btn_GenMatr3_BmulN_Click(object sender, RoutedEventArgs e)
        {
            Lbl_3_Error.Visibility = Visibility.Hidden;
            try
            {
                double Number = Convert.ToDouble(Tbx_Matrix3_Nmul.Text);
                Matrix3 = Matrix2 * Number;
                LoadMatrixToDataGrid(Matrix3, DtGrd_3);
                try
                {
                    LoadDetToLabel(Matrix3.CalculateDeterminant(), lbl_Det_3);
                }
                catch(Exception)
                {
                    lbl_Det_3.Content = "Определитель: " + "Нельзя посчитать";
                }
            }
            catch (Exception)
            {
                Lbl_3_Error.Visibility = Visibility.Visible;
                DtGrd_3.ItemsSource = " ";
            }
        }
    }
}
