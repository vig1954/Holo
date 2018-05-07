﻿using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using ClassLibrary;
using rab1.Forms;

namespace rab1
{
    class Model_object
    {
       public static ZComplexDescriptor Model_1(ZComplexDescriptor cmpl0, double sdvg, double noise, double Lambda)
        {
            int NX , NY;
            if (cmpl0 == null) { NX = 1024;         NY = 1024;         }
              else             { NX = cmpl0.width;  NY = cmpl0.height; }
            ZComplexDescriptor cmpl = new ZComplexDescriptor(NX, NY);       // Выходной массив

            //MessageBox.Show("sdvg= " + sdvg);
            int ax = 512;
            int by = 128;
            int i1 = NX / 2 - ax / 2;
            int j1 = NY / 2 - by / 2;

            double max = 2*Math.PI * sdvg / ax ;
            //Random rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            Random rnd = new Random();
            int ii = 0;
            for (int i = i1; i < ax+i1; i++)
                for (int j = j1; j < by+j1; j++)
                {
                    double fa = (0.5- rnd.NextDouble() )* Math.PI * noise;   //rnd.NextDouble() 0-1
                    double fz = (i - i1) * max - Math.PI;
                    if (j < NX/2) ii++; else ii--;
                    cmpl.array[i, j] = Complex.FromPolarCoordinates(ii, fz + fa);
                    
                }

            return cmpl;
        }

        public static ZComplexDescriptor Model_2(double sdvg, double noise, double Lambda)
        {
            int NX = 1024, NY = 1024;
           
            ZComplexDescriptor cmpl = new ZComplexDescriptor(NX, NY);       // Выходной массив

            //MessageBox.Show("sdvg= " + sdvg);
            int ax = 512;
            int by = 128;
            int i1 = NX / 2 - ax / 2;
            int j1 = NY / 2 - by / 2;

            double max = 2 * Math.PI * sdvg / ax;
            //Random rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            Random rnd = new Random();
            int ii = 0;
            for (int i = i1; i < ax + i1; i++)
                for (int j = j1; j < by + j1; j++)
                {
                    double fa =(0.5- rnd.NextDouble() )* Math.PI * noise;   //rnd.NextDouble() 0-1
                    double fz = (i - i1) * max - Math.PI;
                    if (j < NX / 2) ii++; else ii--;
                    cmpl.array[i, j] = Complex.FromPolarCoordinates(ii, fz + fa);
                }

            return cmpl;
        }
        // Голограмма двух экспозиций
        public static ZComplexDescriptor Model_ADD(ZComplexDescriptor cmpl0, ZArrayDescriptor cmpl1)
        {
            if (cmpl0 == null) { MessageBox.Show("Model_object ZComplexDescriptor == NULL"); return null; }
            if (cmpl1 == null) { MessageBox.Show("Model_object ZArrayDescriptor == NULL");   return null; }
            int NX = cmpl0.width;
            int NY = cmpl0.height;
            ZComplexDescriptor cmpl = new ZComplexDescriptor(NX, NY);       // Выходной массив

            for (int i = 0; i < NX; i++)
              for (int j = 0; j < NY; j++)
                {
                    double am1 = cmpl0.array[i, j].Magnitude;
                    double am2 = cmpl1.array[i, j];
                    cmpl.array[i, j] = new Complex(am1+am2, 0.0);              
                 }    

            return cmpl;
        }

        public static ZComplexDescriptor Model_ADD2(ZArrayDescriptor cmpl0, ZArrayDescriptor cmpl1)
        {
            if (cmpl0 == null) { MessageBox.Show("Model_object ZComplexDescriptor == NULL"); return null; }
            if (cmpl1 == null) { MessageBox.Show("Model_object ZArrayDescriptor == NULL"); return null; }
            int NX = cmpl0.width;
            int NY = cmpl0.height;
            ZComplexDescriptor cmpl = new ZComplexDescriptor(NX, NY);       // Выходной массив

            for (int i = 0; i < NX; i++)
                for (int j = 0; j < NY; j++)
                   { cmpl.array[i, j] = new Complex(cmpl0.array[i, j] + cmpl1.array[i, j], 0.0);  }

            return cmpl;
        }

      


        public static ZArrayDescriptor Model_ADD_ZArray(ZArrayDescriptor cmpl0, ZArrayDescriptor cmpl1)
        {
            if (cmpl0 == null) { MessageBox.Show("Model_object ArrayDescriptorr == NULL"); return null; }
            if (cmpl1 == null) { MessageBox.Show("Model_object ArrayDescriptorr == NULL"); return null; }
            int NX = cmpl0.width;
            int NY = cmpl0.height;
            ZArrayDescriptor cmpl = new ZArrayDescriptor(NX, NY);       // Выходной массив

            for (int i = 0; i < NX; i++)
                for (int j = 0; j < NY; j++)
                   {  cmpl.array[i, j] = cmpl0.array[i, j] + cmpl1.array[i, j];   }

            return cmpl;
        }

        public static ZComplexDescriptor Model_ADD_Cmplx(ZComplexDescriptor[] zComplex, int k1, int k2)   //  Сложение комплекных массивов
        {
            if (zComplex[k1] == null) { MessageBox.Show("Model_object ZComplex[k1] == NULL"); return null; }
            if (zComplex[k2] == null) { MessageBox.Show("Model_object ZComplex[k2] == NULL"); return null; }
            int NX = zComplex[k1].width;
            int NY = zComplex[k1].height;
            ZComplexDescriptor cmpl = new ZComplexDescriptor(NX, NY);       // Выходной массив

            for (int i = 0; i < NX; i++)
                for (int j = 0; j < NY; j++)
                   { cmpl.array[i, j] = zComplex[k1].array[i, j] + zComplex[k2].array[i, j];  }

            return cmpl;
        }

        public static ZComplexDescriptor ADD_Cmplx(ZComplexDescriptor zComplex1, ZComplexDescriptor zComplex2)   //  Сложение комплекных массивов
        {
            if (zComplex1 == null) { MessageBox.Show("Model_object ZComplex1 == NULL"); return null; }
            if (zComplex2 == null) { MessageBox.Show("Model_object ZComplex2 == NULL"); return null; }
            int NX = zComplex1.width;
            int NY = zComplex1.height;
            ZComplexDescriptor cmpl = new ZComplexDescriptor(NX, NY);       // Выходной массив

            for (int i = 0; i < NX; i++)
                for (int j = 0; j < NY; j++)
                { cmpl.array[i, j] = zComplex1.array[i, j] + zComplex2.array[i, j]; }

            return cmpl;
        }

        public static ZComplexDescriptor SUB_Cmplx(ZComplexDescriptor zComplex1, ZComplexDescriptor zComplex2)   //  Сложение комплекных массивов
        {
            if (zComplex1 == null) { MessageBox.Show("Model_object ZComplex1 == NULL"); return null; }
            if (zComplex2 == null) { MessageBox.Show("Model_object ZComplex2 == NULL"); return null; }
            int NX = zComplex1.width;
            int NY = zComplex1.height;
            ZComplexDescriptor cmpl = new ZComplexDescriptor(NX, NY);       // Выходной массив

            for (int i = 0; i < NX; i++)
                for (int j = 0; j < NY; j++)
                { cmpl.array[i, j] = zComplex1.array[i, j] - zComplex2.array[i, j]; }

            return cmpl;
        }


        public static ZComplexDescriptor Model_ADD_zComplex(ZComplexDescriptor zComplex1, ZComplexDescriptor zComplex2)   //  Сложение комплекных массивов
        {
            if (zComplex1 == null)  { MessageBox.Show("Model_object Complex1 == NULL"); return null; }
            if (zComplex2 == null) { MessageBox.Show("Model_object zComplex2 == NULL"); return null; }
            int NX = zComplex1.width;
            int NY = zComplex1.height;
            ZComplexDescriptor cmpl = new ZComplexDescriptor(NX, NY);       // Выходной массив

            for (int i = 0; i < NX; i++)
                for (int j = 0; j < NY; j++)
                { cmpl.array[i, j] = zComplex1.array[i, j] + zComplex2.array[i, j]; }

            return cmpl;
        }

        // Моделирование голографической интерферометрии (сложение волновых фронтов) -> zComplex[2]
        public static void Glgr_Interf(ZComplexDescriptor[] zComplex, ZArrayDescriptor[] zArrayDescriptor, double sdvg, double noise, double Lambda)
        {
            int NX = 1024;
            int NY = 1024;
            int m = 10;
            double am = 255;                                                     // Амплитуда опорной волны
            double dx = 10000;                                                   // Размер объекта в мкм
            double AngleY = 0.687; 
     
            zComplex[0] = new ZComplexDescriptor(NX, NY);
           
            zComplex[0] = Model_1(zComplex[0], 0, noise, Lambda);                               // Модель объекта с нулевым сдвигом
            zComplex[0] = Furie.Invers(zComplex[0]);                                            // Циклический сдвиг
            zComplex[1] = Furie.FourierTransform(zComplex[0], m);                               // Преобразование Фурье
            zComplex[1] = Model_interf.Model_pl_ADD(am, zComplex[1], 0, AngleY, Lambda, dx, 0);    // Сложение с опорной волной
            ZComplexDescriptor.Complex_ArrayDescriptor(zComplex, zArrayDescriptor, 1);          // Из zComplex[] ->   zArrayDescriptor[]                                             
            ZArrayDescriptor amp = new ZArrayDescriptor(zArrayDescriptor[1 * 4 + 2]);           // Амплитуда     
            zComplex[1] = new ZComplexDescriptor(amp);                                          // Формирование голограммы (Re=амплитуда, Im=0)
            zComplex[1] = Furie.FourierTransform(zComplex[1], m);                               // Преобразование Фурье

           
            zComplex[0] = Model_1(zComplex[0], sdvg, noise, Lambda);                             // Модель объекта со сдвигом
            zComplex[0] = Furie.Invers(zComplex[0]);                                            // Циклический сдвиг
            zComplex[2] = Furie.FourierTransform(zComplex[0], m);                               // Преобразование Фурье
            zComplex[2] = Model_interf.Model_pl_ADD(am, zComplex[2], 0, AngleY, Lambda, dx, 0);
            ZComplexDescriptor.Complex_ArrayDescriptor(zComplex, zArrayDescriptor, 2);
            amp = new ZArrayDescriptor(zArrayDescriptor[2 * 4 + 2]);                            // Амплитуда     
            zComplex[2] = new ZComplexDescriptor(amp);                                          // Формирование голограммы (Re=амплитуда, Im=0)
            zComplex[2] = Furie.FourierTransform(zComplex[2], m);         
                   
            zComplex[2] = Model_ADD_Cmplx(zComplex, 1, 2);                      //  Сложение комплексных массивов

           
        }

        // Моделирование голографической интерферометрии (Двойная экспозиция) -> zComplex[2]

        public static void Glgr_Interf2(ZComplexDescriptor[] zComplex, ZArrayDescriptor[] zArrayDescriptor, double sdvg0, double sdvg, double noise, double Lambda, double lm, double dx, double AngleX, double AngleY)
        {
            int NX = 1024;
            int NY = 1024;
            zComplex[0] = new ZComplexDescriptor(NX, NY);

            double am = 4255;                                                                   // Амплитуда опорной волны
            int m = Furie.PowerOfTwo(NX);   
           
            zComplex[0] = Model_2(sdvg0, noise, Lambda);                                        // Модель объекта до деформации
            zComplex[0] = Furie.Invers(zComplex[0]);                                            // Циклический сдвиг
            //zComplex[1] = Furie.FourierTransform(zComplex[0], 10);                             // Преобразование Фурье
            MessageBox.Show("lm= "+lm + "dx= " + dx + " Lambda= " + Lambda + " AngleX= " + AngleX + " AngleY = " + AngleY);
            //zComplex[1] = FurieN.FrenelTransformN(zComplex[0], Lambda, lm, dx);
            zComplex[1] = Furie.FrenelTransform(zComplex[0], m, Lambda, lm, dx);
            //FurieN.FrenelTransformN(zComplex[k1], lambda, d, xmax);
            zComplex[1] = Model_interf.Model_pl_ADD(am, zComplex[1], AngleX, AngleY, Lambda, dx, 0); // Сложение с опорной волной
            zComplex[1] = Model_interf.Model_pl_MUL(am, zComplex[1], AngleX, AngleY, Lambda, dx);
            ZArrayDescriptor amp1 = Furie.zAmplituda(zComplex[1]);                               // Из комплекского числа => Амплитуду


            //ZComplexDescriptor.Complex_ArrayDescriptor(zComplex, zArrayDescriptor, 1);         // Из zComplex[] ->   zArrayDescriptor[]                                             
            //ZArrayDescriptor amp1 = new ZArrayDescriptor(zArrayDescriptor[1 * 4 + 2]);         // Амплитуда     
           
            // 2 состояние ------------------------------------------------------------------------------------------------------------------
            zComplex[0] = Model_2(sdvg, noise, Lambda);                                         // Модель объекта после деформации
            zComplex[0] = Furie.Invers(zComplex[0]);                                            // Циклический сдвиг
            //zComplex[2] = FurieN.FrenelTransformN(zComplex[0], Lambda, lm, dx);                 // Преобразование Френеля    
            zComplex[2] = Furie.FrenelTransform(zComplex[0], m, Lambda, lm, dx);
            zComplex[2] = Model_interf.Model_pl_ADD(am, zComplex[2], AngleX, AngleY, Lambda, dx, 0); // Сложение с опорной волной//zComplex[2] = Model_interf.Model_pl_ADD(am, zComplex[2], AngleX, AngleY, Lambda, dx, 0);
            zComplex[2] = Model_interf.Model_pl_MUL(am, zComplex[2], AngleX, AngleY, Lambda, dx);
            ZArrayDescriptor amp2 = Furie.zAmplituda(zComplex[2]);                              // Амплитуда   

            //zComplex[2] = Model_ADD(zComplex[2], amp1);


            ZArrayDescriptor amp3 = Model_ADD_ZArray(amp1, amp2);                               // Сложение двух амплитуд

            zArrayDescriptor[0 * 4 + 0] = amp1;                                 // 1-я голограмма
            zArrayDescriptor[0 * 4 + 1] = amp2;                                 // 2-я голограмма
            zArrayDescriptor[0 * 4 + 2] = amp3;                                 // Сложение голограмм

            zComplex[1] = new ZComplexDescriptor(NX, NY);                       // Нулевой zComplex[1] 
            zComplex[1] = new ZComplexDescriptor(zComplex[1], amp3);            // амплитуда = amp3, фаза - (прежняя) нулевая

            //zComplex[2] = FurieN.FrenelTransformN(zComplex[1], Lambda, lm, dx);
            zComplex[2] = Furie.FrenelTransform(zComplex[1], m, Lambda, lm, dx);
        }

        // Моделирование голографической интерферометрии (Две голограммы) -> zArrayDescriptor 9,10
        public static void Glgr_Interf3(ZComplexDescriptor[] zComplex, ZArrayDescriptor[] zArrayDescriptor, double sdvg, double noise, double Lambda)
        {
            int NX = 1024;
            int NY = 1024;
            int m = Furie.PowerOfTwo(NX);
            double am = 255;                                                     // Амплитуда опорной волны
            double dx = 10000;                                                   // Размер объекта в мкм
            double AngleY = 0.687;

            zComplex[0] = new ZComplexDescriptor(NX, NY);

            zComplex[0] = Model_1(zComplex[0], 0, noise, Lambda);                               // Модель объекта с нулевым сдвигом
            zComplex[0] = Furie.Invers(zComplex[0]);                                            // Циклический сдвиг
            zComplex[1] = Furie.FourierTransform(zComplex[0], m);                               // Преобразование Фурье
            zComplex[1] = Model_interf.Model_pl_ADD(am, zComplex[1], 0, AngleY, Lambda, dx, 0); // Сложение с опорной волной
            ZComplexDescriptor.Complex_ArrayDescriptor(zComplex, zArrayDescriptor, 1);          // Из zComplex[1] ->   zArrayDescriptor[1]                                             
            zArrayDescriptor[8] = new ZArrayDescriptor(zArrayDescriptor[1 * 4 + 2]);            // Амплитуда     


            zComplex[0] = Model_1(zComplex[0], sdvg, noise, Lambda);                             // Модель объекта со сдвигом
            zComplex[0] = Furie.Invers(zComplex[0]);                                             // Циклический сдвиг
            zComplex[1] = Furie.FourierTransform(zComplex[0], m);                                // Преобразование Фурье
            zComplex[1] = Model_interf.Model_pl_ADD(am, zComplex[1], 0, AngleY, Lambda, dx, 0);
            ZComplexDescriptor.Complex_ArrayDescriptor(zComplex, zArrayDescriptor, 1);           // Из zComplex[1] ->   zArrayDescriptor[1]   
            zArrayDescriptor[9] =  new ZArrayDescriptor(zArrayDescriptor[1 * 4 + 2]);            // Амплитуда     

        }

     
        //   Получение 4 интерференционных картин со сдвигом k1, k2, k3, k4
        //   Исходный фронт zComplex2 + опора(ki)
        //   Формируется волновой фронт (голограмма) с прежней амплитудой и фазой, определенной пошаговым сдвигом
        //   Складываем с zComplex1 (голографическая интерферограмма)
        //   Добавляется опора и восстанавливается исходное изображение zArray_inter

        public static ZArrayDescriptor PSI(double am, ZComplexDescriptor zComplex1, ZComplexDescriptor zComplex2, double AngleX, double AngleY, double noise, double Lambda, double dx, double d, double[] fz, int k1, int k2, int k3, int k4)
        {
            int NX = zComplex2.width;
            int NY = zComplex2.height;
            int m = Furie.PowerOfTwo(NX);

            ZArrayDescriptor[] zArray = new ZArrayDescriptor[4];

            zArray[0] = Model_interf.Model_pl_PSI(am, zComplex2, AngleX, AngleY, Lambda, dx, noise, fz[k1]);   // Сложение с опорой добавление фазового сдвига zComplex2 (второе состояние)
            zArray[1] = Model_interf.Model_pl_PSI(am, zComplex2, AngleX, AngleY, Lambda, dx, noise, fz[k2]);
            zArray[2] = Model_interf.Model_pl_PSI(am, zComplex2, AngleX, AngleY, Lambda, dx, noise, fz[k3]);
            zArray[3] = Model_interf.Model_pl_PSI(am, zComplex2, AngleX, AngleY, Lambda, dx, noise, fz[k4]);

            ZComplexDescriptor zComplex_tmp = new ZComplexDescriptor(NX, NY);
            zComplex_tmp = ATAN_PSI.ATAN_ar(zArray, fz, am);      
            zComplex_tmp = Model_ADD_zComplex(zComplex1, zComplex_tmp);
            zComplex_tmp = Model_interf.Model_pl_MUL(am, zComplex_tmp, AngleX, AngleY, Lambda, dx);
            zComplex_tmp = Furie.FrenelTransform(zComplex_tmp, m, Lambda, d, dx);  // Выходной массив

            ZArrayDescriptor zArray_inter = Furie.zAmplituda(zComplex_tmp);

            return zArray_inter;
        }


        // Формирование 4 голографических интерферограмм. Затем расшифровка их методом PSI
        public static void Glgr_Interf_PSI_Fr(ZComplexDescriptor[] zComplex, ZArrayDescriptor[] zArrayDescriptor, ProgressBar progressBar1, double sdvg0, double sdvg1, double noise, double Lambda, double dx, double d, double[] fz, double Ax, double Ay)
        {
            int NX = 1024;
            int NY = 1024;
            int m = Furie.PowerOfTwo(NX);
            double am = 255;                                                     // Амплитуда опорной волны

            double AngleX = Ax;
            double AngleY = Ay;
            double noise_add = 0;

            zComplex[0] = new ZComplexDescriptor(NX, NY);
            //MessageBox.Show(" sdvg0 = " + sdvg0 + " sdvg1 = " + sdvg1);

            // ----------------------------------------------------------------------------------------------------------------------------- 1 голограмма
            //zComplex[0] = Model_1(zComplex[0], sdvg0, noise, Lambda);                           // Модель объекта с нулевым сдвигом
            zComplex[0] = Model_2(sdvg0, noise, Lambda);                           // Модель объекта с нулевым сдвигом
            zComplex[0] = Furie.Invers(zComplex[0]);                                            // Циклический сдвиг
            zComplex[1] = Furie.FrenelTransform(zComplex[0], m, Lambda, d, dx);                 // Преобразование Френеля

            ZArrayDescriptor[] zArray = new ZArrayDescriptor[4];                                // Рабочие массивы
                                                                                               
            for (int i = 0; i < 4; i++)
                { zArray[i] = Model_interf.Model_pl_PSI(am, zComplex[1], AngleX, AngleY, Lambda, dx, noise_add, fz[i]); }  // Сложение с опорной волной + fz[i]
            zComplex[1]= ATAN_PSI.ATAN_ar(zArray, fz, am);   
                                      
            // ----------------------------------------------------------------------------------------------------------------------------- 2 голограмма
            //zComplex[0] = Model_1(zComplex[0], sdvg1, noise, Lambda);                          // Модель объекта со сдвигом  
            zComplex[0] = Model_2(sdvg1, noise, Lambda);                          // Модель объекта со сдвигом    
            zComplex[0] = Furie.Invers(zComplex[0]);                                           // Циклический сдвиг
            zComplex[2] = Furie.FrenelTransform(zComplex[0], m, Lambda, d, dx);                // Преобразование Френеля

        //    for (int i = 0; i < 4; i++)
        //    { zArray[i] = Model_interf.Model_pl_PSI(am, zComplex[2], AngleX, AngleY, Lambda, dx, noise_add, fz[i]); }  // Сложение с опорной волной + fz[i]
        //    zComplex[2] = ATAN_PSI.ATAN_ar(zArray, fz, am);

            zArrayDescriptor[8]  = PSI(am, zComplex[1], zComplex[2], AngleX, AngleY, noise, Lambda, dx, d, fz, 0, 1, 2, 3);
            zArrayDescriptor[9]  = PSI(am, zComplex[1], zComplex[2], AngleX, AngleY, noise, Lambda, dx, d, fz, 1, 2, 3, 0);
            zArrayDescriptor[10] = PSI(am, zComplex[1], zComplex[2], AngleX, AngleY, noise, Lambda, dx, d, fz, 2, 3, 0, 1);
            zArrayDescriptor[11] = PSI(am, zComplex[1], zComplex[2], AngleX, AngleY, noise, Lambda, dx, d, fz, 3, 0, 1, 2);

            //zArrayDescriptor[8]  = zArray[0];
            //zArrayDescriptor[9]  = zArray[1];
            //zArrayDescriptor[10] = zArray[2];
            //zArrayDescriptor[11] = zArray[3];

            zComplex[1] = ATAN_PSI.ATAN_891011(zArrayDescriptor, progressBar1, fz, am);
           
        }
        // Непосредственное сравнение двух волновых фронтов до и после деформации

        public static void Glgr_Interf8_PSI_Fr(ZComplexDescriptor[] zComplex, ZArrayDescriptor[] zArrayDescriptor, double sdvg0, double sdvg1, double noise, double Lambda, double dx, double d, double[] fz, double AngleX, double AngleY)
        {
            int NX = 1024;
            int NY = 1024;
            zComplex[0] = new ZComplexDescriptor(NX, NY);
            //zComplex[1] = new ZComplexDescriptor(NX, NY);
            //zComplex[2] = new ZComplexDescriptor(NX, NY);

            int m = Furie.PowerOfTwo(NX);
            double am = 255;                                                     // Амплитуда опорной волны

            double noise_add = 0;

            //zComplex[0] = new ZComplexDescriptor(NX, NY);
            ZComplexDescriptor zComplex_tmp  = new ZComplexDescriptor(NX, NY);
            ZComplexDescriptor zComplex_tmp1 = new ZComplexDescriptor(NX, NY);
            ZComplexDescriptor zComplex_tmp2 = new ZComplexDescriptor(NX, NY);
            // ----------------------------------------------------------------------------------------------------------------------------- 1 голограмма

            zComplex_tmp = Model_2(sdvg0, noise, Lambda);                           // Модель объекта с нулевым сдвигом
            zComplex_tmp = Furie.Invers(zComplex_tmp);                                            // Циклический сдвиг
            // zComplex[1] = Furie.FourierTransform(zComplex[0], m);                             // Преобразование Фурье
            zComplex_tmp = Furie.FrenelTransform(zComplex_tmp, m, Lambda, d, dx);                 // Преобразование Френеля
            //zComplex[1] = FurieN.FrenelTransformN(zComplex[0], Lambda, d, dx);
           
            ZArrayDescriptor[] zArray = new ZArrayDescriptor[4];                                // Рабочие массивы для 4 голограмм

            for (int i = 0; i < 4; i++)                                                         // Сложение с опорной волной + fz[i]
            {
                zArray[i] = Model_interf.Model_pl_PSI(am, zComplex_tmp, AngleX, AngleY, Lambda, dx, noise_add, fz[i]);   
            }
           
            zComplex_tmp = ATAN_PSI.ATAN_ar(zArray, fz, am);
                
            // ----------------------------------------------------------------------------------------------------------------------------- 2 голограмма


            zComplex_tmp1 = Model_2(sdvg1, noise, Lambda);                                        // Модель объекта со сдвигом         
            zComplex_tmp1 = Furie.Invers(zComplex_tmp1);                                           // Циклический сдвиг
            zComplex_tmp1 = Furie.FrenelTransform(zComplex_tmp1, m, Lambda, d, dx);                // Преобразование Френеля
            //zComplex[1] = FurieN.FrenelTransformN(zComplex[0], Lambda, d, dx);
           // MessageBox.Show(" m= "+m+ " Lambda= " + Lambda + " d= " + d + " dx= " + dx);
            
            for (int i = 0; i < 4; i++)                                                         // Сложение с опорной волной + fz[i]
              {
                  zArray[i] = Model_interf.Model_pl_PSI(am, zComplex_tmp1, AngleX, AngleY, Lambda, dx, noise_add, fz[i]);
              }          

            zComplex_tmp1 = ATAN_PSI.ATAN_ar(zArray, fz, am);
           
            // ----------------------------------------------------------------------------------------------------------------------------- Сложение волновых фронтов 
          
           zComplex[0] = Model_interf.Model_pl_MUL(am, zComplex_tmp, AngleX, AngleY, Lambda, dx);
           zComplex[0] = Furie.FrenelTransform(zComplex[0], m, Lambda, d, dx);
            
           zComplex_tmp2 = ADD_Cmplx(zComplex_tmp1, zComplex_tmp);
           zComplex_tmp2 = Model_interf.Model_pl_MUL(am, zComplex_tmp2, AngleX, AngleY, Lambda, dx);
           zComplex[2] = Furie.FrenelTransform(zComplex_tmp2, m, Lambda, d, dx);

           zComplex_tmp2 = SUB_Cmplx(zComplex_tmp1, zComplex_tmp);
           zComplex_tmp2 = Model_interf.Model_pl_MUL(am, zComplex_tmp2, AngleX, AngleY, Lambda, dx);
           zComplex[1] = Furie.FrenelTransform(zComplex_tmp2, m, Lambda, d, dx);


        }


    }
}
