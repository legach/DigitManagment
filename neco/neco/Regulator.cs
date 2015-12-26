using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MatrixLibrary;

namespace neco
{
    class Regulator
    {
        /// <summary>
        /// принимает system_state текущей системы и diff_state со всей колбасой параметров на следующий шаг
        /// </summary>
        /// <param name="system_state"></param>
        /// <param name="diff_state"></param>
        /// <returns></returns>
        public static bool MakeRegulation(ref State system_state, State diff_state)
        {
            //к чорту ветвление - слишком наркоманское оно
            //да, здесь будет ПРОСТО условие и никто.. НИКТО, слышите меня, не в праве убрать его отсюда!
            if (system_state.determinancy && system_state.stationarity && (system_state.full_controllability || system_state.part_controllability) && (system_state.full_observersability || system_state.part_observersability))
            {
                //ему ничего на вход не надо - сам посчитает x[k]
                Regulate_Perm_Det(ref system_state);
                return true;
            }
            //выбрасываем исключение о нереализованности
            throw new NotImplementedException();
        }

        /// <summary>
        /// рулилка стационарной детерменированной системы
        /// </summary>
        /// <param name="system_state"></param>
        private static void Regulate_Perm_Det(ref State system_state)
        {
            //матрица для текущего управления
            Matrix Uk = new Matrix(system_state.m, 1);
#region переброска новых данных в system_state
            //очень неявный момент, но можно ничего не перебрасывать - для стационарной системы геттеры/путтеры сами уберут время
            //рассчет х
            Matrix new_x = new Matrix(system_state.n, 1);
            for (int i = 0; i < system_state.a; i++)
                new_x += system_state.get_A(i) * system_state.x[system_state.L[i]];
            for(int i=0;i<system_state.c;i++)
                new_x += system_state.B(i) * system_state.u[system_state.M[i]];
            if (system_state.isRandomEffect)
                new_x += _3rdparty.AWGN_generator(new_x.NoCols, new_x.NoRows);
            system_state.x.Insert(0, new_x);

            //приватные A, B, H и аналоговые можно не делать, т.к. никто не даст взять нето время.
#endregion
            //ф-ла 5.58 стр. 139
            //два цикла наплюсовывания в управление
            for (int i = 0; i < system_state.a; i++)
            {
                Uk += Gx(system_state, i) * system_state.x[system_state.L[i]];
                system_state.set_Gx(Gx(system_state, i), i);
            }

            for (int i = 1; i < system_state.c; i++)
            {
                Uk += Gu(system_state, i) * system_state.u[system_state.M[i]];
                system_state.set_Gu(Gu(system_state, i), i);
            }
                

            //внесение управления на место
            system_state.u.Insert(0, Uk);
            //рассчет критерия качества - 
            if (system_state.quality_f == null)
                system_state.quality_f = new List<double>();
            system_state.quality_f.Insert(0, 0);
            for (int i = 0; i < system_state.a; i++)
                for (int j = 0; j < system_state.a; j++)
                    system_state.quality_f[0] += (Matrix.Transpose(system_state.x[system_state.L[i]]) * P(system_state, i, j) * system_state.x[system_state.L[j]])[0, 0];
            for (int i = 1; i < system_state.c; i++)
                for (int j = 0; j < system_state.a; j++)
                    system_state.quality_f[0] += 2 * (Matrix.Transpose(system_state.u[system_state.M[i]]) * R(system_state, i, j) * system_state.x[system_state.L[j]])[0, 0];
            for (int i = 0; i < system_state.c; i++)
                for (int j = 0; j < system_state.c; j++)
                    system_state.quality_f[0] += (Matrix.Transpose(system_state.u[system_state.M[i]]) * S(system_state, i, j) * system_state.u[system_state.M[j]])[0, 0];
        }
#region формула 5.58 - эпилог
        private static Matrix Gx(State sys_state, int i)
        {
            return -1 * Matrix.Inverse(S(sys_state, 0, 0)) * R(sys_state, 0, i);
        }

        private static Matrix Gu(State sys_state, int i)
        {
            return -1 * Matrix.Inverse(S(sys_state, 0, 0)) * S(sys_state, 0, i);
        }
#endregion
#region формула 5.15 стр.122 убрана нестационарность
        private static Matrix S(State sys_state, int i, int j)
        {
            return Matrix.Transpose(sys_state.B(i)) * sys_state.Fi() * sys_state.B(j) + sys_state.Psi() * _3rdparty.δ(i, 0) * _3rdparty.δ(j, 0);
        }
        private static Matrix R(State sys_state, int i, int j)
        {
            return Matrix.Transpose(sys_state.B(i)) * sys_state.Fi() * sys_state.get_A(j);
        }
        private static Matrix P(State sys_state, int i, int j)
        {
            return Matrix.Transpose(sys_state.get_A(i)) * sys_state.Fi() * sys_state.get_A(j);
        }
#endregion

    }
}
