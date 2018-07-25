using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Numerics;
using ClassLibrary;
using System.Diagnostics;


namespace rab1.Forms
{
    class ATAN_PSI
    {
        // Нахождение ортогонального вектора

        public static double[] Vector_orto(double[] vect)
        {
            int n = vect.Length;
            double[] v_sdv = new double[n];

            v_sdv[0] = vect[1] - vect[n - 1];
            v_sdv[n - 1] = vect[0] - vect[n - 2];
            for (int ii = 1; ii < n - 1; ii++) { v_sdv[ii] = vect[ii + 1] - vect[ii - 1]; }

            return v_sdv;
        }

        // Умножение векторов

        public static double Vector_Mul(double[] vect1, double[] vect2)
        {
            int n = vect1.Length;
            double s = 0;

            for (int ii = 0; ii < n; ii++) { s += vect1[ii] * vect2[ii]; }

            return s;
        }
        // Определение амплитуды и фазы по 4 картинам (PSI), записанным в 9, 10, 11, и 12 окнах
        public static ZComplexDescriptor ATAN_891011(ZArrayDescriptor[] zArrayPicture, ProgressBar progressBar1, double[] fzz, double amplit=0.5)
        {
            // 8, 9, 10, 11   ->    Complex[1] 

            int w1 = zArrayPicture[8].width;
            int h1 = zArrayPicture[8].height;
            // MessageBox.Show("width= " + w1 + "height= " + h1);
            MessageBox.Show("fzz[i]= " + fzz[0] + "  " + fzz[1] + "  " + fzz[2] + "  " + fzz[3]);

            double Ar = SumClass.getAverage(zArrayPicture[8]);

            ZComplexDescriptor faza = new ZComplexDescriptor(w1, h1);

            int n_sdv = 4;                                                       // Число фазовых сдвигов

            double[] i_sdv = new double[4];
            double[] k_sin = new double[4];
            double[] k_cos = new double[4];
          

            for (int i = 0; i < n_sdv; i++)
            {
                k_sin[i] = Math.Sin(fzz[i]);
                k_cos[i] = Math.Cos(fzz[i]);
            }

            double[] sin_orto = Vector_orto(k_sin);                              // Ортогональные вектора
            double[] cos_orto = Vector_orto(k_cos);
            double znmt = Math.Abs(Vector_Mul(k_cos, sin_orto));

            //ZArrayDescriptor amp = new ZArrayDescriptor(zArrayPicture[1 * 4 + 2]);
            //ZArrayDescriptor ampf = new ZArrayDescriptor(zArrayPicture[1 * 4 + 3]);

            progressBar1.Visible = true;
            progressBar1.Minimum = 1;
            progressBar1.Maximum = w1 - 1;
            progressBar1.Value = 1;
            progressBar1.Step = 1;
            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {

                    i_sdv[0] = zArrayPicture[8].array[i, j];
                    i_sdv[1] = zArrayPicture[9].array[i, j];
                    i_sdv[2] = zArrayPicture[10].array[i, j];
                    i_sdv[3] = zArrayPicture[11].array[i, j];

                    double[] i_sdv_orto = Vector_orto(i_sdv);                              // Ортогональные вектора
                   
                    double[] v_sdv = Vector_orto(i_sdv);                 // ------  Формула расшифровки фазы
                    double fz1 = Vector_Mul(v_sdv, k_sin);
                    double fz2 = Vector_Mul(v_sdv, k_cos);
                    
                    //double fz1 = Vector_Mul(i_sdv, sin_orto);
                    //double fz2 = Vector_Mul(i_sdv, cos_orto);]

                    double a = Math.Atan2(fz2, fz1);

                    //double[] cos_orto = Vector_orto(k_cos);              // ------  Формула расшифровки амплитуды                   
                    //double znmt = Vector_Mul(sin_orto, k_cos);
                    double am = Math.Sqrt(fz1 * fz1 + fz2 * fz2) / znmt;
                    //am = am / (2 * amplit);
                    am = am / (2 * Ar);
                    //a= ampf.array[i, j];
                    // am = amp.array[i, j];
                    //double am = 255;

                    faza.array[i, j] = Complex.FromPolarCoordinates(am, a);
                    
                }
                progressBar1.PerformStep();
            }
            progressBar1.Value = 1;
            return faza;
        }

        // Без ProgressBar

  /*      public static ZComplexDescriptor ATAN_9101112(double am, int k1, int k2, int k3, int k4, ZArrayDescriptor[] zArrayDescriptor, double[] fz)
        {
            int NX = zArrayDescriptor[k1].width;
            int NY = zArrayDescriptor[k1].height;

            ZComplexDescriptor cmpl = new ZComplexDescriptor(NX, NY);       // Выходной массив
            ZArrayDescriptor[] zArray = new ZArrayDescriptor[4];
            zArray[0] = zArrayDescriptor[k1];
            zArray[1] = zArrayDescriptor[k2];
            zArray[2] = zArrayDescriptor[k3];
            zArray[3] = zArrayDescriptor[k4];
            cmpl = ATAN_PSI.ATAN_ar(zArray, fz, am);
            return cmpl;
        }
*/
        public static ZComplexDescriptor ATAN_8_11(int k1, int k2, int k3, int k4, ZArrayDescriptor[] zArrayPicture,  double[] fzz, double amplit = 255)
        {
            // 8, 9, 10, 11   ->    Complex[1] 

            int w1 = zArrayPicture[k1].width;
            int h1 = zArrayPicture[k1].height;
            // MessageBox.Show("width= " + w1 + "height= " + h1);

            ZComplexDescriptor faza = new ZComplexDescriptor(w1, h1);

            int n_sdv = 4;                                                       // Число фазовых сдвигов

            double[] i_sdv = new double[4];
            double[] k_sin = new double[4];
            double[] k_cos = new double[4];


            for (int i = 0; i < n_sdv; i++)
            {
                k_sin[i] = Math.Sin(fzz[i]);
                k_cos[i] = Math.Cos(fzz[i]);
            }

            double[] sin_orto = Vector_orto(k_sin);                              // Ортогональные вектора
            double[] cos_orto = Vector_orto(k_cos);
            double znmt = Math.Abs(Vector_Mul(sin_orto, k_cos));

            //ZArrayDescriptor amp = new ZArrayDescriptor(zArrayPicture[1 * 4 + 2]);
            //ZArrayDescriptor ampf = new ZArrayDescriptor(zArrayPicture[1 * 4 + 3]);

           
            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {

                    i_sdv[0] = zArrayPicture[k1].array[i, j];
                    i_sdv[1] = zArrayPicture[k2].array[i, j];
                    i_sdv[2] = zArrayPicture[k3].array[i, j];
                    i_sdv[3] = zArrayPicture[k4].array[i, j];

                    //double[] v_sdv = Vector_orto(i_sdv);                 // ------  Формула расшифровки фазы
                    //double fz1 = Vector_Mul(v_sdv, k_sin);
                    //double fz2 = Vector_Mul(v_sdv, k_cos);
                    double fz1 = Vector_Mul(i_sdv, sin_orto);
                    double fz2 = Vector_Mul(i_sdv, cos_orto);
                    double a = Math.Atan2(fz2, fz1);

                    //double[] cos_orto = Vector_orto(k_cos);              // ------  Формула расшифровки амплитуды                   
                    //double znmt = Vector_Mul(sin_orto, k_cos);
                    double am = Math.Sqrt(fz1 * fz1 + fz2 * fz2) / znmt;
                    //am = am / (2 * amplit);

                    //a= ampf.array[i, j];
                    // am = amp.array[i, j];
                    //am = 255;

                    faza.array[i, j] = Complex.FromPolarCoordinates(am, a);

                }
                
            }
          
            return faza;
        }

        // Отличается от предыдущего тем, что массивы 0,1,2,3 (а не 8,9,10,11)
  
        public static ZComplexDescriptor ATAN_ar(ZArrayDescriptor[] zArrayPicture, double[] fzz, double amplit=255)
        {
            
            int w1 = zArrayPicture[0].width;
            int h1 = zArrayPicture[0].height;
            //MessageBox.Show(" ATAN_ar w1 " + w1+ "  w1 "+ h1);
            ZComplexDescriptor faza = new ZComplexDescriptor(w1, h1);

            int n_sdv = 4;                                                       // Число фазовых сдвигов

            double[] i_sdv = new double[4];
            double[] k_sin = new double[4];
            double[] k_cos = new double[4];


            for (int i = 0; i < n_sdv; i++)
            {
                k_sin[i] = Math.Sin(fzz[i]);
                k_cos[i] = Math.Cos(fzz[i]);
            }
            //ZArrayDescriptor amp = new ZArrayDescriptor(zArrayPicture[1 * 4 + 2]);
            //ZArrayDescriptor ampf = new ZArrayDescriptor(zArrayPicture[1 * 4 + 3]);
            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {

                    i_sdv[0] = zArrayPicture[0].array[i, j];
                    i_sdv[1] = zArrayPicture[1].array[i, j];
                    i_sdv[2] = zArrayPicture[2].array[i, j];
                    i_sdv[3] = zArrayPicture[3].array[i, j];

                    double[] v_sdv = Vector_orto(i_sdv);                 // ------  Формула расшифровки фазы
                    double fz1 = Vector_Mul(v_sdv, k_sin);
                    double fz2 = Vector_Mul(v_sdv, k_cos);
                    double a = Math.Atan2(fz1, fz2);

                    double[] cos_orto = Vector_orto(k_cos);              // ------  Формула расшифровки амплитуды                   
                    double znmt = Vector_Mul(cos_orto, k_sin);
                    double am = Math.Sqrt(fz1 * fz1 + fz2 * fz2) / Math.Abs(znmt);
                    //am = am / (2 * amplit);

                    //a= ampf.array[i, j];
                    // am = amp.array[i, j];
                    //am = 255;

                    faza.array[i, j] = Complex.FromPolarCoordinates(am, a);

                }
            }

            return faza;
        }

        // Определение фазы от 0 до 2pi

        public static ZArrayDescriptor ATAN(ZArrayDescriptor[] zArrayPicture, int k1, int k2, int k3, int k4, double[] fz)
        {
            // 1, 2, 3, 4   ->    Complex[1] 

            //MessageBox.Show("k1= " + k1+" k2= " + k2+" k3= " + k3+" k4= " + k4);
            int w1 = zArrayPicture[k1].width;
            int h1 = zArrayPicture[k2].height;

            ZArrayDescriptor faza = new ZArrayDescriptor(w1, h1);

            int n_sdv = 4;                                                       // Число фазовых сдвигов

            double[] i_sdv = new double[4];
            double[] k_sin = new double[4];
            double[] k_cos = new double[4];


            for (int i = 0; i < n_sdv; i++)
            {
                k_sin[i] = Math.Sin(fz[i]);
                k_cos[i] = Math.Cos(fz[i]);
            }

            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {

                    i_sdv[0] = zArrayPicture[k1].array[i, j];
                    i_sdv[1] = zArrayPicture[k2].array[i, j];
                    i_sdv[2] = zArrayPicture[k3].array[i, j];
                    i_sdv[3] = zArrayPicture[k4].array[i, j];

                    double[] v_sdv = Vector_orto(i_sdv);                // ------  Формула расшифровки фазы
                    double fz1 = Vector_Mul(v_sdv, k_sin); // +3 * Math.PI / 2;
                    double fz2 = Vector_Mul(v_sdv, k_cos)  ;
                    faza.array[i, j] = 2*Math.PI - (Math.Atan2(fz1, fz2) + Math.PI);
                }
            }

            return faza;
        }


      

        /// <summary>
        ///  Быстрое преобразование PSI для четырех сдвигов
        /// </summary>
        /// <param name="zArrayPicture"></param>
        /// <param name="progressBar1"></param>
        /// <param name="fzz"></param>
        /// <param name="amplit"></param>
        /// <returns></returns>
        public static ZArrayDescriptor ATAN_quick( ZArrayDescriptor[] zArrayDescriptor,  ProgressBar progressBar1, double[] fzz, double xmax, double lambda, double d, double amplit)
        {

       
            // 8, 9, 10, 11   ->    Complex[1] 

            int w1 = zArrayDescriptor[8].width;
            int h1 = zArrayDescriptor[8].height;
            System.Diagnostics.Stopwatch sw = new Stopwatch();
            sw.Start();
            //MessageBox.Show("width= " + w1 + "height= " + h1);

            ZComplexDescriptor zComplex_rab = new ZComplexDescriptor(w1, h1);
            /*
                        double i_sdv1;                                                   //PSI
                        double i_sdv2;
                        double i_sdv3;
                        double i_sdv4;
                        //double[] i_s = new double[w1];              // Числитель
                        //double[] i_c = new double[w1];              // Знаменатель

                        double k_sin1 = Math.Sin(fzz[0]);
                        double k_sin2 = Math.Sin(fzz[1]);
                        double k_sin3 = Math.Sin(fzz[2]);
                        double k_sin4 = Math.Sin(fzz[3]);

                        double k_cos1 = Math.Cos(fzz[0]);
                        double k_cos2 = Math.Cos(fzz[1]);
                        double k_cos3 = Math.Cos(fzz[2]);
                        double k_cos4 = Math.Cos(fzz[3]);

                        //double znmt = Math.Abs((k_sin2 - k_sin4) * k_cos1 + (k_sin3 - k_sin1) * k_cos2 + (k_sin4 - k_sin2) * k_cos3 + (k_sin1 - k_sin3) * k_cos4);
                        double znmt = Math.Abs((k_cos2 - k_cos4) * k_sin1 + (k_cos3 - k_cos1) * k_sin2 + (k_cos4 - k_cos2) * k_sin3 + (k_cos1 - k_cos3) * k_sin4);
                        znmt = znmt * (2 * amplit);        

                        progressBar1.Visible = true;
                        progressBar1.Minimum = 1;
                        progressBar1.Maximum = w1 - 1;
                        progressBar1.Value = 1;
                        progressBar1.Step = 1;

                        


                        for (int j = 0; j < h1; j++) 
                        {
                            for (int i = 0; i < w1; i++)
                            {
                                i_sdv1 = zArrayDescriptor[8].array[i, j];
                                i_sdv2 = zArrayDescriptor[9].array[i, j];
                                i_sdv3 = zArrayDescriptor[10].array[i, j];
                                i_sdv4 = zArrayDescriptor[11].array[i, j];

                                double i1 = i_sdv2 - i_sdv4;
                                double i2 = i_sdv3 - i_sdv1;
                                double i3 = i_sdv4 - i_sdv2;
                                double i4 = i_sdv1 - i_sdv3;

                                double i_s = i1 * k_sin1 + i2 * k_sin2 + i3 * k_sin3 + i4 * k_sin4;
                                double i_c = i1 * k_cos1 + i2 * k_sin2 + i3 * k_sin3 + i4 * k_sin4;

                               double faza = Math.Atan2(i_c, i_s);                            // Фаза
                               //double faza = Math.Atan2(i_s, i_c); 
                               double am = Math.Sqrt(i_s * i_s + i_c * i_c) / znmt;           // Амплитуда

                               zComplex_rab.array[i, j] = Complex.FromPolarCoordinates(am, faza);

                            }
                           progressBar1.PerformStep();                            
                         }
                        progressBar1.Value = 1;
            */
           
            zComplex_rab = ATAN_PSI.ATAN_891011(zArrayDescriptor, progressBar1, fzz, amplit);  // PSI
            //zComplex[1] = zComplex_rab;

            ZArrayDescriptor zArrayPicture = new ZArrayDescriptor(w1, h1);

            zComplex_rab = FurieN.FrenelTransformN(zComplex_rab, lambda, d, xmax);              // Преобразование Френеля с четным количеством точек

            for (int i = 0; i < w1; i++)                                                        // Амплитуду в главное окно
                for (int j = 0; j < h1; j++)
                    zArrayPicture.array[i, j] = zComplex_rab.array[i, j].Magnitude;

            zArrayPicture = Furie.Invers_Double(zArrayPicture);
            
           
            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            MessageBox.Show("PSI+Frenel Время Минут: " + ts.Minutes + "   Время сек: " + ts.Seconds + "   Время миллисек: " + ts.Milliseconds);

            //MessageBox.Show(" Lambda= " + lambda + " d= " + d + " dx= " + xmax);

            return zArrayPicture;
            }
           

        //-----------------------------------------------------------------------------------------
        //          Приведение к диапазону
        //-----------------------------------------------------------------------------------------
        public static void Diapazon(ZArrayDescriptor[] zArrayPicture, int imax)
        {
            
            int nx = zArrayPicture[8].width;
            int ny = zArrayPicture[8].height;
             //MessageBox.Show("max= " + max + "min= "+min);
            for (int k = 0; k < 4; k++)
            {
                double max = SumClass.getMax(zArrayPicture[8 + k]);
                double min = SumClass.getMin(zArrayPicture[8 + k]);
                for (int i = 0; i < nx; i++)
                    for (int j = 0; j < ny; j++)
                    {   
                        int c =(int)( (zArrayPicture[8 + k].array[i, j] - min) * imax / (max - min));
                        zArrayPicture[8 + k].array[i, j] = c;
                    }
            }
         }
        public static void Diapazon1(ZArrayDescriptor[] zArrayPicture, int regImage, int imax)
        {

            int nx = zArrayPicture[regImage].width;
            int ny = zArrayPicture[regImage].height;
            //MessageBox.Show("max= " + max + "min= "+min);

            double max = SumClass.getMax(zArrayPicture[regImage]);
            double min = SumClass.getMin(zArrayPicture[regImage]);
                for (int i = 0; i < nx; i++)
                    for (int j = 0; j < ny; j++)
                    {
                        int c = (int)((zArrayPicture[regImage].array[i, j] - min) * imax / (max - min));
                        zArrayPicture[regImage].array[i, j] = c;
                    }
            
        }

        public static ZArrayDescriptor Maska(ZArrayDescriptor[] zArrayPicture, int k1, int k2)
        {

            int nx = zArrayPicture[k2].width;
            int ny = zArrayPicture[k2].height;
            
            //MessageBox.Show("max= " + max + "min= "+min);
            ZArrayDescriptor res = new ZArrayDescriptor(nx, ny);
            
            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                {
                    double c = zArrayPicture[k1].array[i, j];
                    double k = zArrayPicture[k2].array[i, j];
                    if (k != 0) res.array[i, j] = c;
                }
            return res;
        }

        //------------------------------------------------------------------------------------------
    }
}
