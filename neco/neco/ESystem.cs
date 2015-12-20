using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace neco
{
    class ESystem
    {
        public State current_state;

        public ESystem(State new_state)
        {
            //если система отцифрована успешно
            if (Digitizer.DigitizeSystem(ref new_state))
                current_state = new_state;//выполняем присвоение state
            else//если нельзя отцифровать систему - сообщаем об этом
                throw new ArgumentException("Не удалось выполнить отцифровку данной системы");
        }

        
        /// <summary>
        /// Функция построения последовательности обработки системы
        ///diff_state - состояние, содержащее A, B и т.п. для следующего шага
        ///следствия реализации регулятора: diff_state.x[0] - новый х. также для А, В, analogA, analog_B,H,...
        /// </summary>
        /// <param name="diff_state"></param>
        public void next_stage(State diff_state)
        {
            if (Digitizer.DigitizeSystem(ref this.current_state) && Controllability.IsSystemControllability(ref this.current_state) && Observability.IsSystemObservability(ref this.current_state))
                if (!Regulator.MakeRegulation(ref this.current_state, diff_state))
                    throw new ArgumentException("Ошибка при регулировании");//если вернули false, то для такой системы нет регулятора
            else
                throw new ArgumentException("Ошибка при определении параметров системы");//если не удалось даже подготовить к регуляции
        
        }   
        
    }
}
