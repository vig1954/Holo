using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Numerics;
using ClassLibrary;
using System.Diagnostics;

/// Программы формы ADD_Cmplx - Арифметические операции над массивами

namespace rab1
{
    class ADD_Math
    {

        public static void ABS_D(int k1)             // Абсолютное значение
        {
            if (Form1.zArrayDescriptor[k1] == null) { MessageBox.Show("ABS_D zArrayDescriptor[" + k1 + "] == NULL"); return; }
            int nx = Form1.zArrayDescriptor[k1].width;
            int ny = Form1.zArrayDescriptor[k1].height;

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                {
                    Form1.zArrayDescriptor[k1].array[i, j] = Math.Abs(Form1.zArrayDescriptor[k1].array[i, j]);
                   
                }

        }

        public static void Pirs_D(int k1, int k2)             // Линейный коэффициент корреляции r-Пирсона между двумя массивами
        {
            k1--; k2--;                                   // Массив 1 ->  0
            if (Form1.zArrayDescriptor[k1] == null) { MessageBox.Show("Pirs_D zArrayDescriptor[" + k1 + "] == NULL"); return; }
            if (Form1.zArrayDescriptor[k2] == null) { MessageBox.Show("Pirs_D zArrayDescriptor[" + k2 + "] == NULL"); return; }

            int nx = Form1.zArrayDescriptor[k1].width;
            int ny = Form1.zArrayDescriptor[k2].height;

            // MessageBox.Show("Pirs_D");

            double s1 = 0;
            double s2 = 0;
            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                {
                    s1 += Form1.zArrayDescriptor[k1].array[i, j];
                    s2 += Form1.zArrayDescriptor[k2].array[i, j];
                }
            s1 = s1 / (nx * ny); s2 = s2 / (nx * ny);
            //MessageBox.Show("s1 = " + s1);

            double r;
            double ch1 = 0;
            double zn1 = 0;
            double zn2 = 0;

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                {
                    double c1 = Form1.zArrayDescriptor[k1].array[i, j] - s1;
                    double c2 = Form1.zArrayDescriptor[k2].array[i, j] - s2;
                    ch1 += c1 * c2;
                    zn1 += c1 * c1; zn2 += c2 * c2;
                }

            r = ch1 / Math.Sqrt(zn1 * zn2);
            MessageBox.Show("Pirs_D s1 = " + s1 + " s2 = " + s2 + " r = " + r);
        }

        public static void ROR_D(int k1)             // Циклический сдвиг вправо zArrayDescriptor[regImage]
        {

            if (Form1.zArrayDescriptor[Form1.regImage] == null) { MessageBox.Show(" ROR_D  zArrayDescriptor [" + Form1.regImage + "] == NULL"); return; }

            int nx = Form1.zArrayDescriptor[Form1.regImage].width;
            int ny = Form1.zArrayDescriptor[Form1.regImage].height;
            ZArrayDescriptor zArray = new ZArrayDescriptor(nx, ny);

            for (int i = 0; i < nx - k1; i++)
                for (int j = 0; j < ny; j++)
                    zArray.array[i + k1, j] = Form1.zArrayDescriptor[Form1.regImage].array[i, j];

            for (int i = 0; i < k1; i++)
                for (int j = 0; j < ny; j++)
                    zArray.array[i, j] = Form1.zArrayDescriptor[Form1.regImage].array[nx + i - k1, j];

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                    Form1.zArrayDescriptor[Form1.regImage].array[i, j] = zArray.array[i, j];
  
           
        }

        public static void ROL_D(int k1)             // Циклический сдвиг влево zArrayDescriptor[regImage]
        {
            if (Form1.zArrayDescriptor[Form1.regImage] == null) { MessageBox.Show(" ROR_D  zArrayDescriptor [" + Form1.regImage + "] == NULL"); return; }

            int nx = Form1.zArrayDescriptor[Form1.regImage].width;
            int ny = Form1.zArrayDescriptor[Form1.regImage].height;
            ZArrayDescriptor zArray = new ZArrayDescriptor(nx, ny);

            for (int i = 0; i < nx - k1; i++)
                for (int j = 0; j < ny; j++)
                    zArray.array[i, j] = Form1.zArrayDescriptor[Form1.regImage].array[k1 + i, j];

            for (int i = k1; i > 0; i--)
                for (int j = 0; j < ny; j++)
                    zArray.array[nx - i, j] = Form1.zArrayDescriptor[Form1.regImage].array[k1 - i, j];

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                    Form1.zArrayDescriptor[Form1.regImage].array[i, j] = zArray.array[i, j];
        }



        public static void ROR_C(int k1)             // Циклический сдвиг вправо  комплексных чисел
        {
            
            if (Form1.zComplex[Form1.regComplex] == null) { MessageBox.Show("ROR_C:  zComplex[" + Form1.regComplex + "] == NULL"); return; }

            int nx = Form1.zComplex[Form1.regComplex].width;
            int ny = Form1.zComplex[Form1.regComplex].height;

            ZComplexDescriptor zArray = new ZComplexDescriptor(nx, ny);

            for (int i = 0; i < nx - k1; i++)
                for (int j = 0; j < ny; j++)
                    zArray.array[i + k1, j] = Form1.zComplex[Form1.regComplex].array[i, j];

            for (int i = 0; i < k1; i++)
                for (int j = 0; j < ny; j++)
                    zArray.array[i, j] = Form1.zComplex[Form1.regComplex].array[nx + i - k1, j];

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                    Form1.zComplex[Form1.regComplex].array[i, j] = zArray.array[i, j];
        }


        public static void ROL_C(int k1)             // Циклический сдвиг влево комплексных чисел
        {
            if (Form1.zComplex[Form1.regComplex] == null) { MessageBox.Show("ROR_C:  zComplex[" + Form1.regImage + "] == NULL"); return; }

            int nx = Form1.zComplex[Form1.regComplex].width;
            int ny = Form1.zComplex[Form1.regComplex].height;

            ZComplexDescriptor zArray = new ZComplexDescriptor(nx, ny);

            for (int i = 0; i < nx - k1; i++)
                for (int j = 0; j < ny; j++)
                    zArray.array[i, j] = Form1.zComplex[Form1.regComplex].array[k1 + i, j];

            for (int i = k1; i > 0; i--)
                for (int j = 0; j < ny; j++)
                    zArray.array[nx - i, j] = Form1.zComplex[Form1.regComplex].array[k1 - i, j];

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                    Form1.zComplex[Form1.regComplex].array[i, j] = zArray.array[i, j];
        }

        public static void TRNS_D()             // Транспонирование zArrayDescriptor[regImage]
        {
            if (Form1.zArrayDescriptor[Form1.regImage] == null) { MessageBox.Show(" ROR_TRNS  zArrayDescriptor [" + Form1.regImage + "] == NULL"); return; }

            int nx = Form1.zArrayDescriptor[Form1.regImage].width;
            int ny = Form1.zArrayDescriptor[Form1.regImage].height;

            ZArrayDescriptor zArray = new ZArrayDescriptor(ny, nx);

            for (int i = 0; i < ny; i++)
                for (int j = 0; j < nx; j++)
                    zArray.array[i, j] = Form1.zArrayDescriptor[Form1.regImage].array[j, i];

            Form1.zArrayDescriptor[Form1.regImage] = zArray;
            
        }

        public static void ROT180_D()             // Поворот zArrayDescriptor[regImage] на 180 градусов
        {
            if (Form1.zArrayDescriptor[Form1.regImage] == null) { MessageBox.Show(" ROR_TRNS  zArrayDescriptor [" + Form1.regImage + "] == NULL"); return; }

            int nx = Form1.zArrayDescriptor[Form1.regImage].width;
            int ny = Form1.zArrayDescriptor[Form1.regImage].height;

            ZArrayDescriptor zArray = new ZArrayDescriptor(nx, ny);

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                    zArray.array[i, j] = Form1.zArrayDescriptor[Form1.regImage].array[i, ny-1-j];
            //MessageBox.Show(" ROR_TRNS 1");
            
                        Form1.zArrayDescriptor[Form1.regImage] = zArray;
            //MessageBox.Show(" ROR_TRNS 2");
        }
        public static void ADD_D(int k1, int k2, int k3)             // Сложить два вещественных массива
        {
            k1--; k2--; k3--;                                   // Массив 1 ->  0
            if (Form1.zArrayDescriptor[k1] == null) { MessageBox.Show("zArrayDescriptor [" + k1 + "] == NULL"); return; }
            if (Form1.zArrayDescriptor[k2] == null) { MessageBox.Show("zArrayDescriptor [" + k2 + "] == NULL"); return; }
            int nx = Form1.zArrayDescriptor[k1].width;
            int ny = Form1.zArrayDescriptor[k1].height;
            Form1.zArrayDescriptor[k3] = new ZArrayDescriptor(nx, ny);

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                    Form1.zArrayDescriptor[k3].array[i, j] = Form1.zArrayDescriptor[k2].array[i, j] + Form1.zArrayDescriptor[k1].array[i, j];

        }

        public static void Sub_D1(int k1, int k2, int k3)             // Вычесть два вещественных массива (3 аргумента)
        {
            k1--; k2--; k3--;                                   // Массив 1 ->  0

            //MessageBox.Show(" Main   k1=" + k1 + " k2 =" + k2 + " k3 =" + k3);
            if (Form1.zArrayDescriptor[k1] == null) { MessageBox.Show("Sub_D zArrayDescriptor [" + k1 + "] == NULL"); return; }
            if (Form1.zArrayDescriptor[k2] == null) { MessageBox.Show("Sub_D zArrayDescriptor [" + k2 + "] == NULL"); return; }
            int nx = Form1.zArrayDescriptor[k1].width;
            int ny = Form1.zArrayDescriptor[k1].height;

            Form1.zArrayDescriptor[k3] = new ZArrayDescriptor(nx, ny);
           
            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                    Form1.zArrayDescriptor[k3].array[i, j] = Form1.zArrayDescriptor[k1].array[i, j] - Form1.zArrayDescriptor[k2].array[i, j];
        }
        public static void ADD_C(int k1, int k2)             // Накопление += комплексных массивов
        {
            k1--; k2--;                                   // Массив 1 ->  0
            if (Form1.zComplex[k1] == null) { MessageBox.Show("zComplex[" + k1 + "] == NULL"); return; }
            int nx = Form1.zComplex[k1].width;
            int ny = Form1.zComplex[k1].height;
            if (Form1.zComplex[k2] == null) { Form1.zComplex[k2] = new ZComplexDescriptor(nx, ny); }

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                    Form1.zComplex[k2].array[i, j] = Form1.zComplex[k2].array[i, j] + Form1.zComplex[k1].array[i, j];
        }

        public static void Send_C(int k1, int k2)   // Пересылка комплексных массивов
        {
            k1--; k2--;                                   // Массив 1 ->  0
            //MessageBox.Show("k1= " + k1 + " --> k2= " + k2);
            if (Form1.zComplex[k1] == null) { MessageBox.Show("zComplex[" + k1 + "] == NULL"); return; }
            int nx = Form1.zComplex[k1].width;
            int ny = Form1.zComplex[k1].height;

            Form1.zComplex[k2] = new ZComplexDescriptor(nx, ny);

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                    Form1.zComplex[k2].array[i, j] = Form1.zComplex[k1].array[i, j];
        }

        public static void Sub_C(int k3, int k4, int k5)             // Вычесть два комплексных массива
        {
            k3--; k4--; k5--;                                  // Массив 1 ->  0

            MessageBox.Show("k3= " + k3 + " - k4= " + k4 + " = k5= " + k5);

            if (Form1.zComplex[k3] == null) { MessageBox.Show("zComplex[" + k3 + "] == NULL"); return; }
            if (Form1.zComplex[k4] == null) { MessageBox.Show("zComplex[" + k4 + "] == NULL"); return; }
            int nx = Form1.zComplex[k3].width;
            int ny = Form1.zComplex[k3].height;

            int nx1 = Form1.zComplex[k4].width;
            int ny1 = Form1.zComplex[k4].height;

            if ((nx != nx1) || (ny != ny1)) { MessageBox.Show("Размеры массивов не равны"); return; }

            ZComplexDescriptor a = new ZComplexDescriptor(nx, ny);

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                    a.array[i, j] = Form1.zComplex[k3].array[i, j] - Form1.zComplex[k4].array[i, j];

            //zComplex[k5] = new ZComplexDescriptor(nx, ny);
            Form1.zComplex[k5] = a;
           
        }

        public static void Add_C(int k3, int k4, int k5)             // Сложить два комплексных массива
        {
            k3--; k4--; k5--;                                  // Массив 1 ->  0

            MessageBox.Show("k3= " + k3 + " - k4= " + k4 + " = k5= " + k5);

            if (Form1.zComplex[k3] == null) { MessageBox.Show("zComplex[" + k3 + "] == NULL"); return; }
            if (Form1.zComplex[k4] == null) { MessageBox.Show("zComplex[" + k4 + "] == NULL"); return; }
            int nx = Form1.zComplex[k3].width;
            int ny = Form1.zComplex[k3].height;

            int nx1 = Form1.zComplex[k4].width;
            int ny1 = Form1.zComplex[k4].height;

            if ((nx != nx1) || (ny != ny1)) { MessageBox.Show("Размеры массивов не равны"); return; }

            ZComplexDescriptor a = new ZComplexDescriptor(nx, ny);

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                    a.array[i, j] = Form1.zComplex[k3].array[i, j] + Form1.zComplex[k4].array[i, j];

            //zComplex[k5] = new ZComplexDescriptor(nx, ny);
            Form1.zComplex[k5] = a;
     
        }

        public static void Mul_C(int k3, int k4, int k5, ProgressBar progressBar1)             // Умножить два комплексных массива
        {
            k3--; k4--; k5--;                                  // Массив 1 ->  0

            //MessageBox.Show("k3= " + k3 + " - k4= " + k4 + " = k5= " + k5);

            if (Form1.zComplex[k3] == null) { MessageBox.Show("Mul_C zComplex[" + k3 + "] == NULL"); return; }
            if (Form1.zComplex[k4] == null) { MessageBox.Show("Mul_C zComplex[" + k4 + "] == NULL"); return; }

            int nx = Form1.zComplex[k3].width;
            int ny = Form1.zComplex[k3].height;

            int nx1 = Form1.zComplex[k4].width;
            int ny1 = Form1.zComplex[k4].height;

            if ((nx != nx1) || (ny != ny1)) { MessageBox.Show("Mul_C Размеры массивов не согласованы"); return; }

            ZComplexDescriptor a = new ZComplexDescriptor(nx, nx);         // Результирующая матрица
            ZComplexDescriptor b = new ZComplexDescriptor(ny, nx);

            for (int i = 0; i < nx; i++)                                    // Транспонирование второго массива
                for (int j = 0; j < ny; j++)
                    b.array[j, i] = Form1.zComplex[k3].array[i, j];

            progressBar1.Visible = true;
            progressBar1.Minimum = 1;
            progressBar1.Maximum = nx;
            progressBar1.Value = 1;
            progressBar1.Step = 1;

            Complex s0 = new Complex(0, 0);
            for (int i = 0; i < nx; i++)
            {
                for (int j = 0; j < nx; j++)
                {
                    Complex s = s0;
                    for (int y = 0; y < ny; y++)
                        s += b.array[y, j] * Form1.zComplex[k4].array[i, y];  // Строка на столбец
                    a.array[i, j] = s;
                }
                progressBar1.PerformStep();
            }
            progressBar1.Value = 1;
            Form1.zComplex[k5] = a;
            
        }

        public static void Mul_D(int k3, int k4, int k5, ProgressBar progressBar1)             // Умножить два вещественных массива (Транспонировать не надо)
        {
            k3--; k4--; k5--;                                  // Массив 1 ->  0

            //MessageBox.Show("k3= " + k3 + " - k4= " + k4 + " = k5= " + k5);

            if (Form1.zArrayDescriptor[k3] == null) { MessageBox.Show("Mul_C zComplex[" + k3 + "] == NULL"); return; }
            if (Form1.zArrayDescriptor[k4] == null) { MessageBox.Show("Mul_C zComplex[" + k4 + "] == NULL"); return; }

            int nx = Form1.zArrayDescriptor[k3].width;
            int ny = Form1.zArrayDescriptor[k3].height;

            int nx1 = Form1.zArrayDescriptor[k4].width;
            int ny1 = Form1.zArrayDescriptor[k4].height;

            if ((nx != nx1) || (ny != ny1)) { MessageBox.Show("Mul_C Размеры массивов не согласованы"); return; }

            ZArrayDescriptor a = new ZArrayDescriptor(nx, nx);         // Результирующая матрица
            ZArrayDescriptor b = new ZArrayDescriptor(ny, nx);

            for (int i = 0; i < nx; i++)                                    // Транспонирование второго массива
                for (int j = 0; j < ny; j++)
                    b.array[j, i] = Form1.zArrayDescriptor[k3].array[i, j];

            progressBar1.Visible = true;
            progressBar1.Minimum = 1;
            progressBar1.Maximum = nx;
            progressBar1.Value = 1;
            progressBar1.Step = 1;

          
            for (int i = 0; i < nx; i++)
            {
                for (int j = 0; j < nx; j++)
                {
                   double s = 0;
                    for (int y = 0; y < ny; y++)
                        s += b.array[y, j] * Form1.zArrayDescriptor[k4].array[i, y];  // Строка на столбец
                    a.array[i, j] = s;
                }
                progressBar1.PerformStep();
            }
            progressBar1.Value = 1;
            Form1.zArrayDescriptor[k5] = a;

        }

        public static void Conv_D(int k3, int k4, int k5, ProgressBar progressBar1)             // Корреляция двух вещественных массивов
        {
            k3--; k4--; k5--;                                  // Массив 1 ->  0

            //MessageBox.Show("k3= " + k3 + " - k4= " + k4 + " = k5= " + k5);

            if (Form1.zArrayDescriptor[k3] == null) { MessageBox.Show("Mul_C zComplex[" + k3 + "] == NULL"); return; }
            if (Form1.zArrayDescriptor[k4] == null) { MessageBox.Show("Mul_C zComplex[" + k4 + "] == NULL"); return; }

            int nx = Form1.zArrayDescriptor[k3].width;
            int ny = Form1.zArrayDescriptor[k3].height;

            int nx1 = Form1.zArrayDescriptor[k4].width;
            int ny1 = Form1.zArrayDescriptor[k4].height;

            if ((nx != nx1) || (ny != ny1)) { MessageBox.Show("Mul_C Размеры массивов не согласованы"); return; }


            progressBar1.Visible = true;
            progressBar1.Minimum = 1;
            progressBar1.Maximum = 3;
            progressBar1.Value = 1;
            progressBar1.Step = 1;


            ZArrayDescriptor b = new ZArrayDescriptor(ny, nx);

            for (int i = 0; i < nx; i++)                                    // Транспонирование первого массива
                for (int j = 0; j < ny; j++)
                    b.array[j, i] = Form1.zArrayDescriptor[k3].array[i, j];
            progressBar1.PerformStep();                                     // ------------------------------------ 1 шаг

            ZComplexDescriptor zCmpl1 = new ZComplexDescriptor(b);  // Конструктор для заполнения реальной части и мнимой части = 0
            ZComplexDescriptor zCmpl2 = new ZComplexDescriptor(Form1.zArrayDescriptor[k4]);

            zCmpl1 = FurieN.BPF2(zCmpl1); progressBar1.PerformStep();       // ------------------------------------ 2 шаг
            zCmpl2 = FurieN.BPF2(zCmpl2); progressBar1.PerformStep();       // ------------------------------------ 3 шаг

            ZComplexDescriptor a = new ZComplexDescriptor(nx, nx);          // Результирующая матрица (ny x nx)*(nx * ny) = (nx x nx)

            progressBar1.Maximum = nx;
            progressBar1.Value = 1;

            Complex s0 = new Complex(0, 0);
            for (int i = 0; i < nx; i++)
            {
                for (int j = 0; j < nx; j++)
                {
                    Complex s = s0;
                    for (int y = 0; y < ny; y++)
                        s += zCmpl1.array[y, j] * zCmpl2.array[i, y];     // Строка на столбец
                    a.array[i, j] = s;
                }
                progressBar1.PerformStep();
            }
            progressBar1.Value = 1;

            a = FurieN.BPF2(a);                                             // ------------------------------------ 5 шаг
            Form1.zArrayDescriptor[k5] = Furie.zAmplituda(a);

            //Vizual_regImage(k5);
            //Complex_pictureBox(k5);
        }
        public static void Div_D(int k1, int k2, int k3)             // Разделить два вещественных массива (3 аргумента)
        {
            k1--; k2--; k3--;                                   // Массив 1 ->  0

            //MessageBox.Show(" Main   k1=" + k1 + " k2 =" + k2 + " k3 =" + k3);
            if (Form1.zArrayDescriptor[k1] == null) { MessageBox.Show("zArrayDescriptor [" + k1 + "] == NULL"); return; }
            if (Form1.zArrayDescriptor[k2] == null) { MessageBox.Show("zArrayDescriptor [" + k2 + "] == NULL"); return; }
            int nx = Form1.zArrayDescriptor[k1].width;
            int ny = Form1.zArrayDescriptor[k1].height;
            Form1.zArrayDescriptor[k3] = new ZArrayDescriptor(nx, ny);

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                {
                    double b = Form1.zArrayDescriptor[k2].array[i, j];
                    if (b != 0)
                        Form1.zArrayDescriptor[k3].array[i, j] = Form1.zArrayDescriptor[k1].array[i, j] / b;
                }
           // Vizual_regImage(k3);
        }

        public static void Div_C(int k3, int k4, int k5)             // Разделить два комплексных массива поэлементно Form1.zComplex[k]
        {
            k3--; k4--; k5--;                                  // Массив 1 ->  0

            //MessageBox.Show("k3= " + k3 + " - k4= " + k4 + " = k5= " + k5);

            if (Form1.zComplex[k3] == null) { MessageBox.Show("zComplex[" + k3 + "] == NULL"); return; }
            if (Form1.zComplex[k4] == null) { MessageBox.Show("zComplex[" + k4 + "] == NULL"); return; }
            int nx = Form1.zComplex[k3].width;
            int ny = Form1.zComplex[k3].height;

            int nx1 = Form1.zComplex[k4].width;
            int ny1 = Form1.zComplex[k4].height;

            if ((nx != nx1) || (ny != ny1)) { MessageBox.Show("Размеры массивов не равны"); return; }

            ZComplexDescriptor a = new ZComplexDescriptor(nx, ny);

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                    // a.array[i, j] = zComplex[k3].array[i, j] / zComplex[k4].array[i, j];
                    a.array[i, j] = Div(Form1.zComplex[k3].array[i, j], Form1.zComplex[k4].array[i, j]);

            //zComplex[k5] = new ZComplexDescriptor(nx, ny);
            Form1.zComplex[k5] = a;
           // Complex_pictureBox(k5);
        }

        public static ZComplexDescriptor Div_CMPLX(ZComplexDescriptor a1, ZComplexDescriptor a2)             // Разделить два комплексных массива поэлементно 
        {
            

            //MessageBox.Show("k3= " + k3 + " - k4= " + k4 + " = k5= " + k5);

            if (a1 == null) { MessageBox.Show("Div_CMPLX a1 == NULL"); return null; }
            if (a2 == null) { MessageBox.Show("Div_CMPLX a2 == NULL"); return null; }
            
            int nx = a1.width;
            int ny = a1.height;

            int nx1 = a2.width;
            int ny1 = a2.height;

            if ((nx != nx1) || (ny != ny1)) { MessageBox.Show("Div_CMPLX Размеры массивов не равны"); return null; }

            ZComplexDescriptor a = new ZComplexDescriptor(nx, ny);

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                    a.array[i, j] = Div(a1.array[i, j], a2.array[i, j]); 
            return a; 
        }

        private static Complex Div(Complex a, Complex b)  // Деление комплексных чисел 
        {
            Complex res = new Complex(0, 0);
            double D = b.Real * b.Real + b.Imaginary * b.Imaginary;
            if (D != 0)
            {
                double d = (a.Real * b.Real + a.Imaginary * b.Imaginary) / D;
                double im = (a.Imaginary * b.Real - a.Real * b.Imaginary) / D;
                res = new Complex(d, im);
            }

            return res;
        }


        //-------------------------------------------------------------------------------------------------------------------------------
    }
}
